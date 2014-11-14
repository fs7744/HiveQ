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
    public partial class RequestPartsSpec : TBase
    {
        private List<string> _names;
        private List<DropPartitionsExpr> _exprs;

        public List<string> Names
        {
            get
            {
                return _names;
            }
            set
            {
                __isset.names = true;
                this._names = value;
            }
        }

        public List<DropPartitionsExpr> Exprs
        {
            get
            {
                return _exprs;
            }
            set
            {
                __isset.exprs = true;
                this._exprs = value;
            }
        }

        public Isset __isset;
#if !SILVERLIGHT

        [Serializable]
#endif
        public struct Isset
        {
            public bool names;
            public bool exprs;
        }

        public RequestPartsSpec()
        {
        }

        public void Read(TProtocol iprot)
        {
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
                        if (field.Type == TType.List)
                        {
                            {
                                Names = new List<string>();
                                TList _list185 = iprot.ReadListBegin();
                                for (int _i186 = 0; _i186 < _list185.Count; ++_i186)
                                {
                                    string _elem187 = null;
                                    _elem187 = iprot.ReadString();
                                    Names.Add(_elem187);
                                }
                                iprot.ReadListEnd();
                            }
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
                                Exprs = new List<DropPartitionsExpr>();
                                TList _list188 = iprot.ReadListBegin();
                                for (int _i189 = 0; _i189 < _list188.Count; ++_i189)
                                {
                                    DropPartitionsExpr _elem190 = new DropPartitionsExpr();
                                    _elem190 = new DropPartitionsExpr();
                                    _elem190.Read(iprot);
                                    Exprs.Add(_elem190);
                                }
                                iprot.ReadListEnd();
                            }
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
        }

        public void Write(TProtocol oprot)
        {
            TStruct struc = new TStruct("RequestPartsSpec");
            oprot.WriteStructBegin(struc);
            TField field = new TField();
            if (Names != null && __isset.names)
            {
                field.Name = "names";
                field.Type = TType.List;
                field.ID = 1;
                oprot.WriteFieldBegin(field);
                {
                    oprot.WriteListBegin(new TList(TType.String, Names.Count));
                    foreach (string _iter191 in Names)
                    {
                        oprot.WriteString(_iter191);
                    }
                    oprot.WriteListEnd();
                }
                oprot.WriteFieldEnd();
            }
            if (Exprs != null && __isset.exprs)
            {
                field.Name = "exprs";
                field.Type = TType.List;
                field.ID = 2;
                oprot.WriteFieldBegin(field);
                {
                    oprot.WriteListBegin(new TList(TType.Struct, Exprs.Count));
                    foreach (DropPartitionsExpr _iter192 in Exprs)
                    {
                        _iter192.Write(oprot);
                    }
                    oprot.WriteListEnd();
                }
                oprot.WriteFieldEnd();
            }
            oprot.WriteFieldStop();
            oprot.WriteStructEnd();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("RequestPartsSpec(");
            sb.Append("Names: ");
            sb.Append(Names);
            sb.Append(",Exprs: ");
            sb.Append(Exprs);
            sb.Append(")");
            return sb.ToString();
        }
    }
}