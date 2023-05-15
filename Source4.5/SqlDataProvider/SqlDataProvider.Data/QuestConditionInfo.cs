// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.QuestConditionInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
  public class QuestConditionInfo
  {
    public int Tagert() => this.CondictionType == 20 && this.Para1 != 3 ? 0 : this.Para2;

    public int CondictionID { get; set; }

    public string CondictionTitle { get; set; }

    public int CondictionType { get; set; }

    public bool isOpitional { get; set; }

    public int Para1 { get; set; }

    public int Para2 { get; set; }

    public int QuestID { get; set; }
  }
}
