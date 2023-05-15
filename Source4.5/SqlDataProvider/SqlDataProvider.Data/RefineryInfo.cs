// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.RefineryInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using System.Collections.Generic;

namespace SqlDataProvider.Data
{
  public class RefineryInfo
  {
    public List<int> m_Equip = new List<int>();
    public List<int> m_Reward = new List<int>();

    public int Item1 { get; set; }

    public int Item1Count { get; set; }

    public int Item2 { get; set; }

    public int Item2Count { get; set; }

    public int Item3 { get; set; }

    public int Item3Count { get; set; }

    public int Item4 { get; set; }

    public int Item4Count { get; set; }

    public int RefineryID { get; set; }
  }
}
