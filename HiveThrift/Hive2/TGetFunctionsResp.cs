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
  public partial class TGetFunctionsResp : TBase
  {
    private TOperationHandle _operationHandle;

    public TStatus Status { get; set; }

    public TOperationHandle OperationHandle
    {
      get
      {
        return _operationHandle;
      }
      set
      {
        __isset.operationHandle = true;
        this._operationHandle = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool operationHandle;
    }

    public TGetFunctionsResp() {
    }

    public TGetFunctionsResp(TStatus status) : this() {
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
            if (field.Type == TType.Struct) {
              OperationHandle = new TOperationHandle();
              OperationHandle.Read(iprot);
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
      TStruct struc = new TStruct("TGetFunctionsResp");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      field.Name = "status";
      field.Type = TType.Struct;
      field.ID = 1;
      oprot.WriteFieldBegin(field);
      Status.Write(oprot);
      oprot.WriteFieldEnd();
      if (OperationHandle != null && __isset.operationHandle) {
        field.Name = "operationHandle";
        field.Type = TType.Struct;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        OperationHandle.Write(oprot);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("TGetFunctionsResp(");
      sb.Append("Status: ");
      sb.Append(Status== null ? "<null>" : Status.ToString());
      sb.Append(",OperationHandle: ");
      sb.Append(OperationHandle== null ? "<null>" : OperationHandle.ToString());
      sb.Append(")");
      return sb.ToString();
    }

  }

}
