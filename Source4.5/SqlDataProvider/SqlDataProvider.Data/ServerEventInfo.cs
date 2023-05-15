// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.ServerEventInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
  public class ServerEventInfo
  {
    private int id;
    private string name;
    private string value;

    public int ID
    {
      get => this.id;
      set => this.id = value;
    }

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    public string Value
    {
      get => this.value;
      set => this.value = value;
    }
  }
}
