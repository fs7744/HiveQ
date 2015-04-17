using Hive;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hive2
{
    public class HiveClient : IHiveClient
    {
        private Connection m_Connection;

        public HiveClient(string host, int port, string userName = "None", string password = "None")
        {
            m_Connection = new Connection(host, port, userName, password);
        }

        public void Open()
        {
            m_Connection.Open();
        }

        public HiveResult Execute(string query)
        {
            HiveResult result = new HiveResult();
            try
            {
                using (var cur = m_Connection.GetCursor())
                {
                    cur.Execute(query);
                    var data = cur.FetchMany(int.MaxValue);
                    if (!data.IsEmpty())
                    {
                        result.Rows = data.Select(i =>
                        {
                            var dict = i as IDictionary<string, object>;
                            return dict == null ? string.Empty
                                : string.Join("\t", dict.Values.Select(j => j == null ? string.Empty : j.ToString()));
                        }).ToList();
                        result.Schemas = data.First().Cast<KeyValuePair<string, object>>()
                            .Select(i => new FieldSchema() { Name = i.Key }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex;
            }

            return result;
        }

        public void Close()
        {
            m_Connection.Close();
        }

        public static string Way { get { return "Hive2"; } }

        public List<string> Get_all_databases()
        {
            return DoGetCommonValue(cur => cur.Execute("show databases"), GetData);
        }

        public List<string> Get_all_tables(string database)
        {
            return DoGetCommonValue(cur =>
            {
                cur.Execute(string.Format("use {0}", database));
                cur.Execute("show tables");
            }, GetData);
        }

        public List<FieldSchema> Get_schema(string database, string table)
        {
            return DoGetCommonValue(cur =>
            {
                cur.Execute(string.Format("use {0}", database));
                cur.Execute(string.Format("describe {0}", table));
            }, dict =>
            { 
                var result = new List<string>();
                if(dict.ContainsKey("col_name"))
                    result.Add(dict["col_name"].ToString());
                return result;
            }).Select(i => new FieldSchema() { Name = i }).ToList();
        }

        private List<string> DoGetCommonValue(Action<Cursor> action, Func<IDictionary<string, object>, List<string>> getData)
        {
            List<string> result = new List<string>();
            using (var cur = m_Connection.GetCursor())
            {
                action(cur);
                var data = cur.FetchMany(int.MaxValue);
                if (!data.IsEmpty())
                {
                    result = data.SelectMany(i =>
                    {
                        var dict = i as IDictionary<string, object>;
                        return dict == null ? new List<string>()
                            : getData(dict);
                    }).ToList();
                }
            }
            return result;
        }

        private List<string> GetData(IDictionary<string, object> dict)
        {
            return dict.Values.Select(j => j == null ? string.Empty : j.ToString()).ToList();
        }
    }
}