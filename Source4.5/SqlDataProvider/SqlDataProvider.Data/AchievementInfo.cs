// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.AchievementInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class AchievementInfo : DataObject
  {
    public int AchievementPoint { get; set; }

    public int AchievementType { get; set; }

    public bool CanHide { get; set; }

    public string Detail { get; set; }

    public DateTime EndDate { get; set; }

    public int ID { get; set; }

    public int IsActive { get; set; }

    public int IsOther { get; set; }

    public bool IsShare { get; set; }

    public int NeedMaxLevel { get; set; }

    public int NeedMinLevel { get; set; }

    public int PicID { get; set; }

    public int PlaceID { get; set; }

    public string PreAchievementID { get; set; }

    public DateTime StartDate { get; set; }

    public string Title { get; set; }
  }
}
