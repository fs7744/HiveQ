/**
 * Autogenerated by Thrift Compiler (0.9.1)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */

using System;
using System.Collections.Generic;
using System.Text;
using Thrift.Protocol;

namespace Hive
{
#if !SILVERLIGHT

    [Serializable]
#endif
    public partial class GetOpenTxnsInfoResponse : TBase
    {
        public long Txn_high_water_mark { get; set; }

        public List<TxnInfo> Open_txns { get; set; }

        public GetOpenTxnsInfoResponse()
        {
        }

        public GetOpenTxnsInfoResponse(long txn_high_water_mark, List<TxnInfo> open_txns)
            : this()
        {
            this.Txn_high_water_mark = txn_high_water_mark;
            this.Open_txns = open_txns;
        }

        public void Read(TProtocol iprot)
        {
            bool isset_txn_high_water_mark = false;
            bool isset_open_txns = false;
            TField field;
            iprot.ReadStructBegin();
            while (true)
            {
                field = iprot.ReadFieldBegin();
                if (field.Type == TType.Stop)
                {
                    break;
                }
                switch (field.ID)
                {
                    case 1:
                        if (field.Type == TType.I64)
                        {
                            Txn_high_water_mark = iprot.ReadI64();
                            isset_txn_high_water_mark = true;
                        }
                        else
                        {
                            TProtocolUtil.Skip(iprot, field.Type);
                        }
                        break;

                    case 2:
                        if (field.Type == TType.List)
                        {
                            {
                                Open_txns = new List<TxnInfo>();
                                TList _list197 = iprot.ReadListBegin();
                                for (int _i198 = 0; _i198 < _list197.Count; ++_i198)
                                {
                                    TxnInfo _elem199 = new TxnInfo();
                                    _elem199 = new TxnInfo();
                                    _elem199.Read(iprot);
                                    Open_txns.Add(_elem199);
                                }
                                iprot.ReadListEnd();
                            }
                            isset_open_txns = true;
                        }
                        else
                        {
                            TProtocolUtil.Skip(iprot, field.Type);
                        }
                        break;

                    default:
                        TProtocolUtil.Skip(iprot, field.Type);
                        break;
                }
                iprot.ReadFieldEnd();
            }
            iprot.ReadStructEnd();
            if (!isset_txn_high_water_mark)
                throw new TProtocolException(TProtocolException.INVALID_DATA);
            if (!isset_open_txns)
                throw new TProtocolException(TProtocolException.INVALID_DATA);
        }

        public void Write(TProtocol oprot)
        {
            TStruct struc = new TStruct("GetOpenTxnsInfoResponse");
            oprot.WriteStructBegin(struc);
            TField field = new TField();
            field.Name = "txn_high_water_mark";
            field.Type = TType.I64;
            field.ID = 1;
            oprot.WriteFieldBegin(field);
            oprot.WriteI64(Txn_high_water_mark);
            oprot.WriteFieldEnd();
            field.Name = "open_txns";
            field.Type = TType.List;
            field.ID = 2;
            oprot.WriteFieldBegin(field);
            {
                oprot.WriteListBegin(new TList(TType.Struct, Open_txns.Count));
                foreach (TxnInfo _iter200 in Open_txns)
                {
                    _iter200.Write(oprot);
                }
                oprot.WriteListEnd();
            }
            oprot.WriteFieldEnd();
            oprot.WriteFieldStop();
            oprot.WriteStructEnd();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("GetOpenTxnsInfoResponse(");
            sb.Append("Txn_high_water_mark: ");
            sb.Append(Txn_high_water_mark);
            sb.Append(",Open_txns: ");
            sb.Append(Open_txns);
            sb.Append(")");
            return sb.ToString();
        }
    }
}