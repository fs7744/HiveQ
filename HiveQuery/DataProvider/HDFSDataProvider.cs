using HiveQuery.Data;
using Microsoft.Hadoop.WebHDFS;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HiveQuery.DataProvider
{
    public class HDFSDataProvider : DataProviderBase
    {
        public async override Task<List<DataTable>> Execute(string query, Connection conn, CancellationTokenSource token = null, object data = null)
        {
            var result = new List<DataTable>();
            if (m_Token == null || !m_Token.IsCancellationRequested)
            {
                var table = await Execute(query, conn);
                if (table != null)
                    result.Add(table);
            }
            return result;
        }

        private async Task<DataTable> Execute(string query, Connection conn)
        {
            var result = new DataTable();
            try
            {
                var client = new WebHDFSClient(new Uri(string.Format("http://{0}:{1}", conn.HiveIP, conn.HivePort)), conn.UserName);
                var fileStatus = await client.GetFileStatus(query);
                if (fileStatus.Length > 1024 * 1024 * 30)
                    throw new Exception(string.Format("The file is too big : {0}", fileStatus.Length));
                var content = await client.OpenFile(query);
                using (var stream = await content.Content.ReadAsStreamAsync())
                {
                    var reader = new StreamReader(stream);
                    var index = 0;
                    while (!reader.EndOfStream)
                    {
                        var line = await reader.ReadLineAsync();
                        var list = line.Split(new string[1] { conn.CSVSeparator }, StringSplitOptions.None).ToList();
                        SetColumns(result, index, list);
                        list.Insert(0, index.ToString());
                        result.Rows.Add(list.ToArray<object>());
                        index++;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Columns.Add("Error");
                result.Rows.Add(ex.Message);
            }

            return result;
        }

        private void SetColumns(DataTable result, int index, List<string> list)
        {
            if (index == 0)
            {
                result.Columns.Add(Index);
                for (int i = 0; i < list.Count; i++)
                {
                    result.Columns.Add("F" + i.ToString());
                }
            }
        }
    }
}