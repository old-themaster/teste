// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.ActiveConditionInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class ActiveConditionInfo
  {
    public int ActiveID { get; set; }

    public string AwardId { get; set; }

    public int Condition { get; set; }

    public int Conditiontype { get; set; }

    public DateTime EndTime { get; set; }

    public int ID { get; set; }

    public bool IsMult { get; set; }

    public string LimitGrade { get; set; }

    public DateTime StartTime { get; set; }
  }
}
