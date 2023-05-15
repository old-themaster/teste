// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.CommunalActiveInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class CommunalActiveInfo
  {
    public int ActiveID { get; set; }

    public string AddPropertyByMoney { get; set; }

    public string AddPropertyByProp { get; set; }

    public DateTime BeginTime { get; set; }

    public int DayMaxScore { get; set; }

    public DateTime EndTime { get; set; }

    public bool IsReset { get; set; }

    public bool IsSendAward { get; set; }

    public int LimitGrade { get; set; }

    public int MinScore { get; set; }
  }
}
