// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.DailyLeagueAwardList
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using System.Collections.Generic;

namespace SqlDataProvider.Data
{
  public class DailyLeagueAwardList
  {
    public int Class { get; set; }

    public List<DailyLeagueAwardItems> AwardLists { get; set; }

    public int Grade { get; set; }

    public int Score { get; set; }

    public int Rank { get; set; }
  }
}
