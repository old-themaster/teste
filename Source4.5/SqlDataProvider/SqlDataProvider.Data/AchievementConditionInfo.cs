// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.AchievementConditionInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
  public class AchievementConditionInfo : DataObject
  {
    public int AchievementID { get; set; }

    public string Condiction_Para1 { get; set; }

    public int Condiction_Para2 { get; set; }

    public int CondictionID { get; set; }

    public int CondictionType { get; set; }
  }
}
