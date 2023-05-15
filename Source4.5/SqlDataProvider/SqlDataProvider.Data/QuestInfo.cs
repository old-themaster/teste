// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.QuestInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class QuestInfo : DataObject
  {
    public bool AutoEquip { get; set; }

    public bool CanRepeat { get; set; }

    public string Detail { get; set; }

    public DateTime EndDate { get; set; }

    public int ID { get; set; }

    public int IsOther { get; set; }

    public int MapID { get; set; }

    public int NeedMaxLevel { get; set; }

    public int NeedMinLevel { get; set; }

    public string NextQuestID { get; set; }

    public int NotMustCount { get; set; }

    public string Objective { get; set; }

    public int RewardMedal { get; set; }

    public string PreQuestID { get; set; }

    public int QuestID { get; set; }

    public int RandDouble { get; set; }

    public Decimal Rands { get; set; }

    public string Rank { get; set; }

    public int RepeatInterval { get; set; }

    public int RepeatMax { get; set; }

    public int RewardBuffDate { get; set; }

    public int RewardBuffID { get; set; }

    public int RewardGold { get; set; }

    public int RewardGP { get; set; }

    public int RewardGiftToken { get; set; }

    public int RewardMoney { get; set; }

    public int RewardOffer { get; set; }

    public int RewardRiches { get; set; }

    public int StarLev { get; set; }

    public DateTime StartDate { get; set; }

    public bool TimeMode { get; set; }

    public string Title { get; set; }
  }
}
