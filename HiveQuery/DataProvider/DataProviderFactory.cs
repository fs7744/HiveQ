using HiveQuery.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HiveQuery.DataProvider
{
    public sealed class DataProviderFactory
    {
        public static readonly DataProviderFactory Instance = new DataProviderFactory();
        private Dictionary<string, DataProviderBase> m_Providers;

        private DataProviderFactory()
        {
            m_Providers = new Dictionary<string, DataProviderBase>();
            m_Providers.Add("Hive", new HiveDataProvider());
            m_Providers.Add("Hive2", new Hive2DataProvider());
            m_Providers.Add("Solr", new SolrDataProvider());
            m_Providers.Add("HDFS_CSV", new HDFSDataProvider());
        }

        public string[] GetKeys()
        {
            return m_Providers.Keys.ToArray();
        }

        public DataProviderBase GetProvider(string key)
        {
            DataProviderBase provider = null;
            m_Providers.TryGetValue(key, out provider);
            return provider;
        }

        public async Task<List<DataTable>> Execute(string key, string query, Connection conn, CancellationTokenSource token = null, object data = null)
        {
            DataProviderBase provider = GetProvider(key);
            return provider != null ? await provider.Execute(query, conn, token, data) : new List<DataTable>();
        }
    }
}