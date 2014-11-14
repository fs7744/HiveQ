/**
 * Autogenerated by Thrift Compiler (0.9.1)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using System.Runtime.Serialization;
using Thrift.Protocol;
using Thrift.Transport;

namespace Hive2
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class TColumnDesc : TBase
  {
    private string _comment;

    public string ColumnName { get; set; }

    public TTypeDesc TypeDesc { get; set; }

    public int Position { get; set; }

    public string Comment
    {
      get
      {
        return _comment;
      }
      set
      {
        __isset.comment = true;
        this._comment = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool comment;
    }

    public TColumnDesc() {
    }

    public TColumnDesc(string columnName, TTypeDesc typeDesc, int position) : this() {
      this.ColumnName = columnName;
      this.TypeDesc = typeDesc;
      this.Position = position;
    }

    public void Read (TProtocol iprot)
    {
      bool isset_columnName = false;
      bool isset_typeDesc = false;
      bool isset_position = false;
      TField field;
      iprot.ReadStructBegin();
      while (true)
      {
        field = iprot.ReadFieldBegin();
        if (field.Type == TType.Stop) { 
          break;
        }
        switch (field.ID)
        {
          case 1:
            if (field.Type == TType.String) {
              ColumnName = iprot.ReadString();
              isset_columnName = true;
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.Struct) {
              TypeDesc = new TTypeDesc();
              TypeDesc.Read(iprot);
              isset_typeDesc = true;
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I32) {
              Position = iprot.ReadI32();
              isset_position = true;
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.String) {
              Comment = iprot.ReadString();
            } else { 
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
      if (!isset_columnName)
        throw new TProtocolException(TProtocolException.INVALID_DATA);
      if (!isset_typeDesc)
        throw new TProtocolException(TProtocolException.INVALID_DATA);
      if (!isset_position)
        throw new TProtocolException(TProtocolException.INVALID_DATA);
    }

    public void Write(TProtocol oprot) {
      TStruct struc = new TStruct("TColumnDesc");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      field.Name = "columnName";
      field.Type = TType.String;
      field.ID = 1;
      oprot.WriteFieldBegin(field);
      oprot.WriteString(ColumnName);
      oprot.WriteFieldEnd();
      field.Name = "typeDesc";
      field.Type = TType.Struct;
      field.ID = 2;
      oprot.WriteFieldBegin(field);
      TypeDesc.Write(oprot);
      oprot.WriteFieldEnd();
      field.Name = "position";
      field.Type = TType.I32;
      field.ID = 3;
      oprot.WriteFieldBegin(field);
      oprot.WriteI32(Position);
      oprot.WriteFieldEnd();
      if (Comment != null && __isset.comment) {
        field.Name = "comment";
        field.Type = TType.String;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Comment);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("TColumnDesc(");
      sb.Append("ColumnName: ");
      sb.Append(ColumnName);
      sb.Append(",TypeDesc: ");
      sb.Append(TypeDesc== null ? "<null>" : TypeDesc.ToString());
      sb.Append(",Position: ");
      sb.Append(Position);
      sb.Append(",Comment: ");
      sb.Append(Comment);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
