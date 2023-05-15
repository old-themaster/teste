// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.ConsortiaEventInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class ConsortiaEventInfo
  {
    public int ID { get; set; }

    public int ConsortiaID { get; set; }

    public DateTime Date { get; set; }

    public int Type { get; set; }

    public string NickName { get; set; }

    public int EventValue { get; set; }

    public string ManagerName { get; set; }

    public bool IsExist { get; set; }
  }
}
