// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.AreaConfigInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
  public class AreaConfigInfo
  {
    public int AreaID { get; set; }

    public string AreaServer { get; set; }

    public string AreaName { get; set; }

    public string DataSource { get; set; }

    public string Catalog { get; set; }

    public string UserID { get; set; }

    public string Password { get; set; }

    public string RequestUrl { get; set; }

    public bool CrossChatAllow { get; set; }

    public bool CrossPrivateChat { get; set; }

    public string Version { get; set; }
  }
}
