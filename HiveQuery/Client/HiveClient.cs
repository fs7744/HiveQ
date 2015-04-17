using System;
using System.Collections.Generic;
using Thrift;
using Thrift.Protocol;
using Thrift.Transport;

namespace Hive
{
    public interface IHiveClient
    {
        void Open();

        HiveResult Execute(string query);

        List<string> Get_all_databases();

        List<string> Get_all_tables(string database);

        List<FieldSchema> Get_schema(string database, string table);

        void Close();
    }

    public class HiveClient : IHiveClient
    {
        private TBufferedTransport transport;

        public ThriftHive.Client Client { get; private set; }

        private TBinaryProtocol protocol;

        public HiveClient(string host, int port)
        {
            var socket = new TSocket(host, port);
            transport = new TBufferedTransport(socket);
            protocol = new TBinaryProtocol(transport);
            Client = new ThriftHive.Client(protocol);
        }

        public void Open()
        {
            if (transport != null && !transport.IsOpen)
                transport.Open();
        }

        public void Close()
        {
            if (transport != null && transport.IsOpen)
                transport.Close();
        }

        public HiveResult Execute(string query)
        {
            HiveResult result = new HiveResult();
            Client.send_execute(query);
            result.Error = GetExecuteInfo(protocol);
            if (result.Error == null)
            {
                result.Rows = Client.fetchAll();
                if (result.Rows.Count != 0)
                {
                    Schema sc = Client.getSchema();
                    result.Schemas = sc.FieldSchemas;
                }
            }
            return result;
        }

        private Exception GetExecuteInfo(TBinaryProtocol protocol)
        {
            TMessage msgs = protocol.ReadMessageBegin();
            if (msgs.Type == TMessageType.Exception)
            {
                TApplicationException x = TApplicationException.Read(protocol);
                protocol.ReadMessageEnd();
                throw x;
            }
            ThriftHive.execute_result results = new ThriftHive.execute_result();
            results.Read(protocol);
            protocol.ReadMessageEnd();
            return results.Ex != null ? new Exception(results.Ex.Message) : results.Ex;
        }

        public static string Way { get { return "Hive"; } }

        public List<string> Get_all_databases()
        {
            return Client.get_all_databases();
        }

        public List<string> Get_all_tables(string database)
        {
            return Client.get_all_tables(database);
        }

        public List<FieldSchema> Get_schema(string database, string table)
        {
            return Client.get_schema(database, table);
        }
    }

    public class HiveResult
    {
        public Exception Error { get; set; }

        public List<FieldSchema> Schemas { get; set; }

        public List<string> Rows { get; set; }
    }
}