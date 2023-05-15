// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.SubActiveInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class SubActiveInfo
  {
    public int ActiveID { get; set; }

    public string ActiveInfo { get; set; }

    public DateTime EndDate { get; set; }

    public DateTime EndTime { get; set; }

    public int ID { get; set; }

    public bool IsContinued { get; set; }

    public bool IsOpen { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime StartTime { get; set; }

    public int SubID { get; set; }
  }
}
