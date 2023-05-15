// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.DataObject
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
  public class DataObject
  {
    protected bool _isDirty;

    public bool IsDirty
    {
      get => this._isDirty;
      set => this._isDirty = value;
    }
  }
}
