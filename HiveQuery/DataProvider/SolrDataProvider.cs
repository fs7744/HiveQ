using HiveQuery.Common;
using HiveQuery.Data;
using Microsoft.Practices.ServiceLocation;
using SolrNet;
using SolrNet.Commands.Parameters;
using SolrNet.DSL;
using SolrNet.Impl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;

namespace HiveQuery.DataProvider
{
    public class SolrDataProvider : DataProviderBase
    {
        private DataTable Execute(string query, string url)
        {
            DataTable result = null;
            try
            {
                Startup.Container.Clear();
                Startup.InitContainer();
                Startup.Init<Dictionary<string, object>>(url);
                var solr = ServiceLocator.Current.GetInstance<ISolrOperations<Dictionary<string, object>>>();
                var schema = solr.GetSchema();
                var data = solr.Query(new SolrQuery(query), new QueryOptions() { Rows = int.MaxValue });
                result = LoadTable(data, schema == null || schema.SolrFields.IsEmpty() ? new List<string>() : schema.SolrFields.Select(i => i.Name));
            }
            catch (Exception ex)
            {
                result = new DataTable();
                result.Columns.Add("Error");
                result.Rows.Add(ex.Message);
            }

            return result;
        }

        private DataTable LoadTable(SolrQueryResults<Dictionary<string, object>> result, IEnumerable<string> columns)
        {
            DataTable table = new DataTable();
            if (!result.IsEmpty())
            {
                var first =
                table.Columns.Add(Index);
                foreach (var key in columns)
                {
                    table.Columns.Add(key);
                }

                int index = 0;
                foreach (var dic in result)
                {
                    var row = table.NewRow();
                    row[Index] = ++index;
                    foreach (var key in dic.Keys)
                    {
                        row[key] = dic[key];
                    }
                    table.Rows.Add(row);
                }
            }
            return table;
        }

        public override List<DataTable> Execute(string query, Connection conn, CancellationTokenSource token = null, object data = null)
        {
            base.Execute(query, conn, token, data);
            m_Token = token;
            var result = new List<DataTable>();
            if (m_Token == null || !m_Token.IsCancellationRequested)
            {
                var url = string.Format("{0}/{1}", conn.SolrUrl, data.ToString());
                var table = Execute(query, url);
                if (table != null)
                    result.Add(table);
            }
            return result;
        }

        public List<string> GetSolrCores(Connection conn)
        {
            Startup.Container.Clear();
            Startup.InitContainer();
            Startup.Container.Register<ISolrStatusResponseParser>(c => new SolrStatusResponseParser());
            var headerParser = ServiceLocator.Current.GetInstance<ISolrHeaderResponseParser>();
            var statusParser = ServiceLocator.Current.GetInstance<ISolrStatusResponseParser>();
            var solrCoreAdmin = new SolrCoreAdmin(new SolrConnection(conn.SolrUrl), headerParser, statusParser);
            var coreStatus = solrCoreAdmin.Status();
            return coreStatus.IsEmpty() ? new List<string>() : coreStatus.Select(i => i.Name).ToList();
        }
    }
}