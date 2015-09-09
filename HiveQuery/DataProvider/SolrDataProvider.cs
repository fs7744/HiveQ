using HiveQuery.Common;
using HiveQuery.Data;
using HiveQuery.SolrNet;
using Microsoft.Practices.ServiceLocation;
using SolrNet;
using SolrNet.Impl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HiveQuery.DataProvider
{
    public class SolrDataProvider : DataProviderBase
    {
        public SolrDataProvider()
        {
            Startup.Container.Clear();
            Startup.InitContainer();
        }

        private DataTable Execute(string query, string url)
        {
            DataTable result = null;
            try
            {
                var conn = new SimpleSolrConnection(url);
                var data = conn.Execute(query);
                result = LoadTable(data);
            }
            catch (Exception ex)
            {
                result = new DataTable();
                result.Columns.Add("Error");
                result.Rows.Add(ex.Message);
            }

            return result;
        }

        private DataTable LoadTable(SolrQueryResults<Dictionary<string, object>> result)
        {
            DataTable table = new DataTable();
            if (!result.IsEmpty())
            {
                var first =
                table.Columns.Add(Index);

                int index = 0;
                foreach (var dic in result)
                {
                    var row = table.NewRow();
                    row[Index] = ++index;
                    foreach (var key in dic.Keys)
                    {
                        if (!table.Columns.Contains(key))
                            table.Columns.Add(key);
                        row[key] = dic[key];
                    }
                    table.Rows.Add(row);
                }
            }
            return table;
        }

        public async override Task<List<DataTable>> Execute(string query, Connection conn, CancellationTokenSource token = null, object data = null)
        {
            await base.Execute(query, conn, token, data);
            m_Token = token;
            var result = new List<DataTable>();
            if (m_Token == null || !m_Token.IsCancellationRequested)
            {
                var table = Execute(query, conn.SolrUrl);
                if (table != null)
                    result.Add(table);
            }
            return result;
        }
    }
}