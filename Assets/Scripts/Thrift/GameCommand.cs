/**
 * Autogenerated by Thrift Compiler (0.9.3)
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


#if !SILVERLIGHT
[Serializable]
#endif
public partial class GameCommand : TBase
{
  private string _commandJSON;
  private string _commandType;
  private long _executionTime;

  public string CommandJSON
  {
    get
    {
      return _commandJSON;
    }
    set
    {
      __isset.commandJSON = true;
      this._commandJSON = value;
    }
  }

  public string CommandType
  {
    get
    {
      return _commandType;
    }
    set
    {
      __isset.commandType = true;
      this._commandType = value;
    }
  }

  public long ExecutionTime
  {
    get
    {
      return _executionTime;
    }
    set
    {
      __isset.executionTime = true;
      this._executionTime = value;
    }
  }


  public Isset __isset;
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public struct Isset {
    public bool commandJSON;
    public bool commandType;
    public bool executionTime;
  }

  public GameCommand() {
  }

  public void Read (TProtocol iprot)
  {
    iprot.IncrementRecursionDepth();
    try
    {
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
              CommandJSON = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.String) {
              CommandType = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I64) {
              ExecutionTime = iprot.ReadI64();
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
    }
    finally
    {
      iprot.DecrementRecursionDepth();
    }
  }

  public void Write(TProtocol oprot) {
    oprot.IncrementRecursionDepth();
    try
    {
      TStruct struc = new TStruct("GameCommand");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (CommandJSON != null && __isset.commandJSON) {
        field.Name = "commandJSON";
        field.Type = TType.String;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(CommandJSON);
        oprot.WriteFieldEnd();
      }
      if (CommandType != null && __isset.commandType) {
        field.Name = "commandType";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(CommandType);
        oprot.WriteFieldEnd();
      }
      if (__isset.executionTime) {
        field.Name = "executionTime";
        field.Type = TType.I64;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(ExecutionTime);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }
    finally
    {
      oprot.DecrementRecursionDepth();
    }
  }

  public override string ToString() {
    StringBuilder __sb = new StringBuilder("GameCommand(");
    bool __first = true;
    if (CommandJSON != null && __isset.commandJSON) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("CommandJSON: ");
      __sb.Append(CommandJSON);
    }
    if (CommandType != null && __isset.commandType) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("CommandType: ");
      __sb.Append(CommandType);
    }
    if (__isset.executionTime) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("ExecutionTime: ");
      __sb.Append(ExecutionTime);
    }
    __sb.Append(")");
    return __sb.ToString();
  }

}
