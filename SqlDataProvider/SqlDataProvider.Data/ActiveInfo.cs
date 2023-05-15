// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.ActiveInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class ActiveInfo
  {
    public string ActionTimeContent { get; set; }

    public int ActiveID { get; set; }

    public int ActiveType { get; set; }

    public string AwardContent { get; set; }

    public string Content { get; set; }

    public string Description { get; set; }

    public DateTime? EndDate { get; set; }

    public string GoodsExchangeNum { get; set; }

    public string GoodsExchangeTypes { get; set; }

    public int HasKey { get; set; }

    public int IconID { get; set; }

    public bool IsAdvance { get; set; }

    public int IsOnly { get; set; }

    public bool IsShow { get; set; }

    public string limitType { get; set; }

    public string limitValue { get; set; }

    public DateTime StartDate { get; set; }

    public string Title { get; set; }

    public int Type { get; set; }
  }
}
