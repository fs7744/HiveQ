using System;
using System.Collections.Generic;
using Thrift.Protocol;
using Thrift.Transport;

namespace Hive2
{
    public class HiveClient
    {
        private TSaslClientTransport transport;
        private TCLIService.Client client;
        private TBinaryProtocol protocol;

        protected HiveClient(string host, int port, string userName = "None", string password = "None")
        {
            var socket = new TSocket(host, port);
            transport = new TSaslClientTransport(socket, userName, password);
            protocol = new TBinaryProtocol(transport);
            client = new TCLIService.Client(protocol);
        }

        public void Open()
        {
            if (transport != null && !transport.IsOpen)
                transport.Open();
        }

        public HiveResult Execute(string query)
        {
            HiveResult result = new HiveResult();

            try
            {
                TOpenSessionReq openReq = new TOpenSessionReq();
                TOpenSessionResp openResp = client.OpenSession(openReq);
                CheckStatus(openResp.Status);

                TSessionHandle sessHandle = openResp.SessionHandle;

                openReq = new TOpenSessionReq();
                openResp = client.OpenSession(openReq);
                CheckStatus(openResp.Status);

                sessHandle = openResp.SessionHandle;

                TExecuteStatementReq execReq = new TExecuteStatementReq();
                execReq.SessionHandle = sessHandle;
                execReq.Statement = query;
                TExecuteStatementResp execResp = client.ExecuteStatement(execReq);
                CheckStatus(execResp.Status);

                TOperationHandle stmtHandle = execResp.OperationHandle;
                TFetchResultsReq fetchReq = new TFetchResultsReq();
                fetchReq.MaxRows = int.MaxValue;
                fetchReq.OperationHandle = stmtHandle;
                fetchReq.Orientation = TFetchOrientation.FETCH_LAST;
                TFetchResultsResp resultsResp = client.FetchResults(fetchReq);
                CheckStatus(resultsResp.Status);

                TRowSet resultsSet = resultsResp.Results;
                result.Rows = resultsSet.Rows;

                TGetResultSetMetadataReq schemasReq = new TGetResultSetMetadataReq();
                schemasReq.OperationHandle = stmtHandle;
                TGetResultSetMetadataResp schemasResp = client.GetResultSetMetadata(schemasReq);
                CheckStatus(schemasResp.Status);
                result.Schemas = schemasResp.Schema.Columns;

                TCloseOperationReq closeReq = new TCloseOperationReq();
                closeReq.OperationHandle = stmtHandle;
                TCloseOperationResp closeOperationResp = client.CloseOperation(closeReq);
                CheckStatus(closeOperationResp.Status);

                TCloseSessionReq closeSessionReq = new TCloseSessionReq();
                closeSessionReq.SessionHandle = sessHandle;
                TCloseSessionResp closeSessionResp = client.CloseSession(closeSessionReq);
                CheckStatus(closeSessionResp.Status);
            }
            catch (Exception ex)
            {
                result.Error = ex;
            }

            return result;
        }

        private static void CheckStatus(TStatus status)
        {
            if ((TStatusCode)status.ErrorCode == TStatusCode.ERROR_STATUS || (TStatusCode)status.ErrorCode == TStatusCode.INVALID_HANDLE_STATUS)
            {
                throw new Exception(status.ErrorMessage);
            }
        }

        public void Close()
        {
            if (transport != null && transport.IsOpen)
                transport.Close();
        }

        public static string Way { get { return "Hive2"; } }
    }

    public class HiveResult
    {
        public Exception Error { get; set; }

        public List<TColumnDesc> Schemas { get; set; }

        public List<TRow> Rows { get; set; }
    }
}