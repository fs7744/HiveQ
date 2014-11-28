using HiveQuery.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;

namespace HiveQuery.DataProvider
{
    public abstract class DataProviderBase
    {
        public const string Index = "index";

        protected CancellationTokenSource m_Token = null;

        public virtual List<DataTable> Execute(string query, Connection conn, CancellationTokenSource token = null, object data = null)
        {
            m_Token = token;
            return null;
        }
    }
}