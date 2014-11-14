/**
 * Autogenerated by Thrift Compiler (0.9.1)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */

using System;
using System.Text;
using Thrift.Protocol;

namespace Hive
{
#if !SILVERLIGHT

    [Serializable]
#endif
    public partial class HeartbeatTxnRangeRequest : TBase
    {
        public long Min { get; set; }

        public long Max { get; set; }

        public HeartbeatTxnRangeRequest()
        {
        }

        public HeartbeatTxnRangeRequest(long min, long max)
            : this()
        {
            this.Min = min;
            this.Max = max;
        }

        public void Read(TProtocol iprot)
        {
            bool isset_min = false;
            bool isset_max = false;
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
                            Min = iprot.ReadI64();
                            isset_min = true;
                        }
                        else
                        {
                            TProtocolUtil.Skip(iprot, field.Type);
                        }
                        break;

                    case 2:
                        if (field.Type == TType.I64)
                        {
                            Max = iprot.ReadI64();
                            isset_max = true;
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
            if (!isset_min)
                throw new TProtocolException(TProtocolException.INVALID_DATA);
            if (!isset_max)
                throw new TProtocolException(TProtocolException.INVALID_DATA);
        }

        public void Write(TProtocol oprot)
        {
            TStruct struc = new TStruct("HeartbeatTxnRangeRequest");
            oprot.WriteStructBegin(struc);
            TField field = new TField();
            field.Name = "min";
            field.Type = TType.I64;
            field.ID = 1;
            oprot.WriteFieldBegin(field);
            oprot.WriteI64(Min);
            oprot.WriteFieldEnd();
            field.Name = "max";
            field.Type = TType.I64;
            field.ID = 2;
            oprot.WriteFieldBegin(field);
            oprot.WriteI64(Max);
            oprot.WriteFieldEnd();
            oprot.WriteFieldStop();
            oprot.WriteStructEnd();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("HeartbeatTxnRangeRequest(");
            sb.Append("Min: ");
            sb.Append(Min);
            sb.Append(",Max: ");
            sb.Append(Max);
            sb.Append(")");
            return sb.ToString();
        }
    }
}