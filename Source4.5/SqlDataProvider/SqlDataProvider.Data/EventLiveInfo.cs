// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.EventLiveInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class EventLiveInfo : DataObject
  {
    public int EventID { get; set; }

    public string Description { get; set; }

    public int CondictionType { get; set; }

    public int Condiction_Para1 { get; set; }

    public int Condiction_Para2 { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
  }
}
