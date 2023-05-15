// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.EventRewardInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using System.Collections.Generic;

namespace SqlDataProvider.Data
{
  public class EventRewardInfo
  {
    public int ActivityType { get; set; }

    public List<EventRewardGoodsInfo> AwardLists { get; set; }

    public int Condition { get; set; }

    public int SubActivityType { get; set; }
  }
}
