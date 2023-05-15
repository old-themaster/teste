// Decompiled with JetBrains decompiler
// Type: Bussiness.CenterService.ServerData
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D7B17810-90E2-4665-9C80-45CCAF971AD1
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Bussiness.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Bussiness.CenterService
{
  [DebuggerStepThrough]
  [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
  [DataContract(Name = "ServerData", Namespace = "http://schemas.datacontract.org/2004/07/Center.Server")]
  [Serializable]
  public class ServerData : IExtensibleDataObject, INotifyPropertyChanged
  {
    [NonSerialized]
    private ExtensionDataObject extensionDataField;
    [OptionalField]
    private int IdField;
    [OptionalField]
    private string IpField;
    [OptionalField]
    private int LowestLevelField;
    [OptionalField]
    private int MustLevelField;
    [OptionalField]
    private string NameField;
    [OptionalField]
    private int OnlineField;
    [OptionalField]
    private int PortField;
    [OptionalField]
    private int StateField;

    [Browsable(false)]
    public ExtensionDataObject ExtensionData
    {
      get => this.extensionDataField;
      set => this.extensionDataField = value;
    }

    [DataMember]
    public int Id
    {
      get => this.IdField;
      set
      {
        if (this.IdField.Equals(value))
          return;
        this.IdField = value;
        this.RaisePropertyChanged(nameof (Id));
      }
    }

    [DataMember]
    public string Ip
    {
      get => this.IpField;
      set
      {
        if ((object) this.IpField == (object) value)
          return;
        this.IpField = value;
        this.RaisePropertyChanged(nameof (Ip));
      }
    }

    [DataMember]
    public int LowestLevel
    {
      get => this.LowestLevelField;
      set
      {
        if (this.LowestLevelField.Equals(value))
          return;
        this.LowestLevelField = value;
        this.RaisePropertyChanged(nameof (LowestLevel));
      }
    }

    [DataMember]
    public int MustLevel
    {
      get => this.MustLevelField;
      set
      {
        if (this.MustLevelField.Equals(value))
          return;
        this.MustLevelField = value;
        this.RaisePropertyChanged(nameof (MustLevel));
      }
    }

    [DataMember]
    public string Name
    {
      get => this.NameField;
      set
      {
        if ((object) this.NameField == (object) value)
          return;
        this.NameField = value;
        this.RaisePropertyChanged(nameof (Name));
      }
    }

    [DataMember]
    public int Online
    {
      get => this.OnlineField;
      set
      {
        if (this.OnlineField.Equals(value))
          return;
        this.OnlineField = value;
        this.RaisePropertyChanged(nameof (Online));
      }
    }

    [DataMember]
    public int Port
    {
      get => this.PortField;
      set
      {
        if (this.PortField.Equals(value))
          return;
        this.PortField = value;
        this.RaisePropertyChanged(nameof (Port));
      }
    }

    [DataMember]
    public int State
    {
      get => this.StateField;
      set
      {
        if (this.StateField.Equals(value))
          return;
        this.StateField = value;
        this.RaisePropertyChanged(nameof (State));
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void RaisePropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
