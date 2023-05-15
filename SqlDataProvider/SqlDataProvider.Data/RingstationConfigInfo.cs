// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.RingstationConfigInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class RingstationConfigInfo
  {
    public int AwardBattleByRank(int rank, bool isWin)
    {
      if (rank == 0 & isWin)
        return 10;
      if (!((uint) rank > 0U | isWin))
        return 5;
      if (!string.IsNullOrEmpty(this.AwardFightLost) && !string.IsNullOrEmpty(this.AwardFightWin))
      {
        string[] strArray1 = this.AwardFightLost.Split('|');
        if (isWin)
          strArray1 = this.AwardFightWin.Split('|');
        if (strArray1.Length < 3)
          return 0;
        foreach (string str in strArray1)
        {
          char[] chArray = new char[1]{ ',' };
          string[] strArray2 = str.Split(chArray);
          if (strArray2.Length < 2)
            return 0;
          int num1 = int.Parse(strArray2[0].Split('-')[0]);
          int num2 = int.Parse(strArray2[0].Split('-')[1]);
          if (rank >= num1 && rank <= num2)
            return int.Parse(strArray2[1]);
        }
      }
      return 0;
    }

    public int AwardNumByRank(int rank)
    {
      if (rank == 0)
        return 0;
      return rank < 30 && rank > 0 ? this.AwardNum - 10 * rank : this.AwardNum - 350;
    }

    public bool IsEndTime()
    {
      DateTime dateTime = this.AwardTime;
      DateTime date1 = dateTime.Date;
      dateTime = DateTime.Now;
      DateTime date2 = dateTime.Date;
      return date1 < date2;
    }

    public string AwardFightLost { get; set; }

    public string AwardFightWin { get; set; }

    public int AwardNum { get; set; }

    public DateTime AwardTime { get; set; }

    public int buyCount { get; set; }

    public int buyPrice { get; set; }

    public int cdPrice { get; set; }

    public int ChallengeNum { get; set; }

    public string ChampionText { get; set; }

    public int ID { get; set; }

    public bool IsFirstUpdateRank { get; set; }
  }
}
