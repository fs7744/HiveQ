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
  public partial class TGetOperationStatusResp : TBase
  {
    private TOperationState _operationState;
    private string _sqlState;
    private int _errorCode;
    private string _errorMessage;

    public TStatus Status { get; set; }

    /// <summary>
    /// 
    /// <seealso cref="TOperationState"/>
    /// </summary>
    public TOperationState OperationState
    {
      get
      {
        return _operationState;
      }
      set
      {
        __isset.operationState = true;
        this._operationState = value;
      }
    }

    public string SqlState
    {
      get
      {
        return _sqlState;
      }
      set
      {
        __isset.sqlState = true;
        this._sqlState = value;
      }
    }

    public int ErrorCode
    {
      get
      {
        return _errorCode;
      }
      set
      {
        __isset.errorCode = true;
        this._errorCode = value;
      }
    }

    public string ErrorMessage
    {
      get
      {
        return _errorMessage;
      }
      set
      {
        __isset.errorMessage = true;
        this._errorMessage = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool operationState;
      public bool sqlState;
      public bool errorCode;
      public bool errorMessage;
    }

    public TGetOperationStatusResp() {
    }

    public TGetOperationStatusResp(TStatus status) : this() {
      this.Status = status;
    }

    public void Read (TProtocol iprot)
    {
      bool isset_status = false;
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
            if (field.Type == TType.Struct) {
              Status = new TStatus();
              Status.Read(iprot);
              isset_status = true;
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.I32) {
              OperationState = (TOperationState)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.String) {
              SqlState = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.I32) {
              ErrorCode = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.String) {
              ErrorMessage = iprot.ReadString();
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
      if (!isset_status)
        throw new TProtocolException(TProtocolException.INVALID_DATA);
    }

    public void Write(TProtocol oprot) {
      TStruct struc = new TStruct("TGetOperationStatusResp");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      field.Name = "status";
      field.Type = TType.Struct;
      field.ID = 1;
      oprot.WriteFieldBegin(field);
      Status.Write(oprot);
      oprot.WriteFieldEnd();
      if (__isset.operationState) {
        field.Name = "operationState";
        field.Type = TType.I32;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)OperationState);
        oprot.WriteFieldEnd();
      }
      if (SqlState != null && __isset.sqlState) {
        field.Name = "sqlState";
        field.Type = TType.String;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(SqlState);
        oprot.WriteFieldEnd();
      }
      if (__isset.errorCode) {
        field.Name = "errorCode";
        field.Type = TType.I32;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(ErrorCode);
        oprot.WriteFieldEnd();
      }
      if (ErrorMessage != null && __isset.errorMessage) {
        field.Name = "errorMessage";
        field.Type = TType.String;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(ErrorMessage);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("TGetOperationStatusResp(");
      sb.Append("Status: ");
      sb.Append(Status== null ? "<null>" : Status.ToString());
      sb.Append(",OperationState: ");
      sb.Append(OperationState);
      sb.Append(",SqlState: ");
      sb.Append(SqlState);
      sb.Append(",ErrorCode: ");
      sb.Append(ErrorCode);
      sb.Append(",ErrorMessage: ");
      sb.Append(ErrorMessage);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
