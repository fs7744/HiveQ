using Hive;
using HiveQuery.Common;
using HiveQuery.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;

namespace HiveQuery.DataProvider
{
    public class HiveDataProvider : DataProviderBase
    {
        public static List<CompletionData> KeyWords = new List<CompletionData>();

        private List<Tuple<string, List<CompletionData>>> GetDataBaseInfo(Connection conn, IEnumerable<string> onlyShowDataBase)
        {
            List<Tuple<string, List<CompletionData>>> result = new List<Tuple<string, List<CompletionData>>>();
            var client = new HiveClient(conn.HiveIP, conn.HivePort);

            try
            {
                client.Open();
                var op = client.Client;
                var databases = onlyShowDataBase != null && onlyShowDataBase.Count() > 0
                    ? onlyShowDataBase : op.get_all_databases();
                foreach (var database in databases)
                {
                    var tables = op.get_all_tables(database);
                    result.Add(Tuple.Create(database, tables.Select(i => new CompletionData(i)).ToList()));
                    foreach (var table in tables)
                    {
                        var fileds = op.get_schema(database, table);
                        if (!fileds.IsEmpty())
                            result.Add(Tuple.Create(table, fileds.Select(i => new CompletionData(i.Name)).ToList()));
                    }
                }
            }
            catch
            { }
            finally
            {
                client.Close();
            }
            return result;
        }

        private DataTable Execute(string query, HiveClient client)
        {
            var result = client.Execute(query);

            if (result.Error != null)
            {
                var table = new DataTable();
                table.Columns.Add("Error");
                table.Rows.Add(result.Error.Message);
                return table;
            }
            else
            {
                return LoadTable(result.Schemas, result.Rows);
            }
        }

        private DataTable LoadTable(List<FieldSchema> flieds, List<string> rows)
        {
            if (flieds.IsEmpty() && rows.IsEmpty()) return null;

            DataTable table = new DataTable();

            if (!flieds.IsEmpty())
            {
                table.Columns.Add(Index);

                //处理列头，主要避免同名问题
                flieds.ForEach(field =>
                {
                    string str = field.Name;
                    while (table.Columns.Contains(str))
                    {
                        str += "_";
                    }
                    table.Columns.Add(str);
                });
            }

            if (!rows.IsEmpty())
            {
                //处理列头，主要避免分区名没有的问题
                if (rows.Count > 0)
                {
                    string str = "Partition";
                    for (int i = 0; i < rows[0].Split('\t').Length - flieds.Count; i++)
                    {
                        table.Columns.Add(str + i);
                    }
                }

                //填充数据
                int j = 0;
                rows.ForEach(row =>
                {
                    List<string> fs = row.Split('\t').ToList();
                    fs.Insert(0, (++j).ToString());
                    table.Rows.Add(fs.ToArray());
                });
            }

            return table;
        }

        private List<string> GetQuerys(string query)
        {
            query = query.Replace("\r\n", " ");
            query = query.Replace("\\;", "[-I-]");
            var querys = query.Split(";".ToArray(), StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < querys.Length; i++)
            {
                querys[i] = querys[i].Replace("[-I-]", "\\;");
            }
            return querys.ToList();
        }

        public override List<DataTable> Execute(string query, Connection conn, CancellationTokenSource token = null, object data = null)
        {
            base.Execute(query, conn, token, data);
            var result = new List<DataTable>();
            m_Token = token;

            if (m_Token == null || !m_Token.IsCancellationRequested)
            {
                var querys = GetQuerys(query);
                if (!querys.IsEmpty())
                {
                    var client = new HiveClient(conn.HiveIP, conn.HivePort);
                    client.Open();
                    try
                    {
                        foreach (var hive in querys)
                        {
                            if (m_Token == null || !m_Token.IsCancellationRequested)
                            {
                                var table = Execute(hive, client);
                                if (table != null)
                                    result.Add(table);
                            }
                        }
                    }
                    finally
                    {
                        client.Close();
                    }
                }
            }
            return result;
        }

        public Tuple<List<CompletionData>, List<Tuple<string, List<CompletionData>>>> GetCompletionData(Connection conn)
        {
            var list = KeyWords.ToList();
            var completionDataBase = GetDataBaseInfo(conn, ConfigManager.Instance.Config.OnlyShowHiveDataBase);
            if (!completionDataBase.IsEmpty())
                list.AddRange(completionDataBase.Select(i => new CompletionData(i.Item1)));
            return Tuple.Create(list, completionDataBase);
        }
    }
}