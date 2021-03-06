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
    public partial class GrantRevokeRoleRequest : TBase
    {
        private GrantRevokeType _requestType;
        private string _roleName;
        private string _principalName;
        private PrincipalType _principalType;
        private string _grantor;
        private PrincipalType _grantorType;
        private bool _grantOption;

        /// <summary>
        ///
        /// <seealso cref="GrantRevokeType"/>
        /// </summary>
        public GrantRevokeType RequestType
        {
            get
            {
                return _requestType;
            }
            set
            {
                __isset.requestType = true;
                this._requestType = value;
            }
        }

        public string RoleName
        {
            get
            {
                return _roleName;
            }
            set
            {
                __isset.roleName = true;
                this._roleName = value;
            }
        }

        public string PrincipalName
        {
            get
            {
                return _principalName;
            }
            set
            {
                __isset.principalName = true;
                this._principalName = value;
            }
        }

        /// <summary>
        ///
        /// <seealso cref="PrincipalType"/>
        /// </summary>
        public PrincipalType PrincipalType
        {
            get
            {
                return _principalType;
            }
            set
            {
                __isset.principalType = true;
                this._principalType = value;
            }
        }

        public string Grantor
        {
            get
            {
                return _grantor;
            }
            set
            {
                __isset.grantor = true;
                this._grantor = value;
            }
        }

        /// <summary>
        ///
        /// <seealso cref="PrincipalType"/>
        /// </summary>
        public PrincipalType GrantorType
        {
            get
            {
                return _grantorType;
            }
            set
            {
                __isset.grantorType = true;
                this._grantorType = value;
            }
        }

        public bool GrantOption
        {
            get
            {
                return _grantOption;
            }
            set
            {
                __isset.grantOption = true;
                this._grantOption = value;
            }
        }

        public Isset __isset;
#if !SILVERLIGHT

        [Serializable]
#endif
        public struct Isset
        {
            public bool requestType;
            public bool roleName;
            public bool principalName;
            public bool principalType;
            public bool grantor;
            public bool grantorType;
            public bool grantOption;
        }

        public GrantRevokeRoleRequest()
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
                        if (field.Type == TType.I32)
                        {
                            RequestType = (GrantRevokeType)iprot.ReadI32();
                        }
                        else
                        {
                            TProtocolUtil.Skip(iprot, field.Type);
                        }
                        break;

                    case 2:
                        if (field.Type == TType.String)
                        {
                            RoleName = iprot.ReadString();
                        }
                        else
                        {
                            TProtocolUtil.Skip(iprot, field.Type);
                        }
                        break;

                    case 3:
                        if (field.Type == TType.String)
                        {
                            PrincipalName = iprot.ReadString();
                        }
                        else
                        {
                            TProtocolUtil.Skip(iprot, field.Type);
                        }
                        break;

                    case 4:
                        if (field.Type == TType.I32)
                        {
                            PrincipalType = (PrincipalType)iprot.ReadI32();
                        }
                        else
                        {
                            TProtocolUtil.Skip(iprot, field.Type);
                        }
                        break;

                    case 5:
                        if (field.Type == TType.String)
                        {
                            Grantor = iprot.ReadString();
                        }
                        else
                        {
                            TProtocolUtil.Skip(iprot, field.Type);
                        }
                        break;

                    case 6:
                        if (field.Type == TType.I32)
                        {
                            GrantorType = (PrincipalType)iprot.ReadI32();
                        }
                        else
                        {
                            TProtocolUtil.Skip(iprot, field.Type);
                        }
                        break;

                    case 7:
                        if (field.Type == TType.Bool)
                        {
                            GrantOption = iprot.ReadBool();
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
            TStruct struc = new TStruct("GrantRevokeRoleRequest");
            oprot.WriteStructBegin(struc);
            TField field = new TField();
            if (__isset.requestType)
            {
                field.Name = "requestType";
                field.Type = TType.I32;
                field.ID = 1;
                oprot.WriteFieldBegin(field);
                oprot.WriteI32((int)RequestType);
                oprot.WriteFieldEnd();
            }
            if (RoleName != null && __isset.roleName)
            {
                field.Name = "roleName";
                field.Type = TType.String;
                field.ID = 2;
                oprot.WriteFieldBegin(field);
                oprot.WriteString(RoleName);
                oprot.WriteFieldEnd();
            }
            if (PrincipalName != null && __isset.principalName)
            {
                field.Name = "principalName";
                field.Type = TType.String;
                field.ID = 3;
                oprot.WriteFieldBegin(field);
                oprot.WriteString(PrincipalName);
                oprot.WriteFieldEnd();
            }
            if (__isset.principalType)
            {
                field.Name = "principalType";
                field.Type = TType.I32;
                field.ID = 4;
                oprot.WriteFieldBegin(field);
                oprot.WriteI32((int)PrincipalType);
                oprot.WriteFieldEnd();
            }
            if (Grantor != null && __isset.grantor)
            {
                field.Name = "grantor";
                field.Type = TType.String;
                field.ID = 5;
                oprot.WriteFieldBegin(field);
                oprot.WriteString(Grantor);
                oprot.WriteFieldEnd();
            }
            if (__isset.grantorType)
            {
                field.Name = "grantorType";
                field.Type = TType.I32;
                field.ID = 6;
                oprot.WriteFieldBegin(field);
                oprot.WriteI32((int)GrantorType);
                oprot.WriteFieldEnd();
            }
            if (__isset.grantOption)
            {
                field.Name = "grantOption";
                field.Type = TType.Bool;
                field.ID = 7;
                oprot.WriteFieldBegin(field);
                oprot.WriteBool(GrantOption);
                oprot.WriteFieldEnd();
            }
            oprot.WriteFieldStop();
            oprot.WriteStructEnd();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("GrantRevokeRoleRequest(");
            sb.Append("RequestType: ");
            sb.Append(RequestType);
            sb.Append(",RoleName: ");
            sb.Append(RoleName);
            sb.Append(",PrincipalName: ");
            sb.Append(PrincipalName);
            sb.Append(",PrincipalType: ");
            sb.Append(PrincipalType);
            sb.Append(",Grantor: ");
            sb.Append(Grantor);
            sb.Append(",GrantorType: ");
            sb.Append(GrantorType);
            sb.Append(",GrantOption: ");
            sb.Append(GrantOption);
            sb.Append(")");
            return sb.ToString();
        }
    }
}