// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.AuctionInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
  public class AuctionInfo
  {
    public int AuctioneerID { get; set; }

    public string AuctioneerName { get; set; }

    public int AuctionID { get; set; }

    public DateTime BeginDate { get; set; }

    public int BuyerID { get; set; }

    public string BuyerName { get; set; }

    public int Category { get; set; }

    public int goodsCount { get; set; }

    public bool IsExist { get; set; }

    public int ItemID { get; set; }

    public int Mouthful { get; set; }

    public string Name { get; set; }

    public int PayType { get; set; }

    public int Price { get; set; }

    public int Random { get; set; }

    public int Rise { get; set; }

    public int TemplateID { get; set; }

    public int ValidDate { get; set; }
  }
}
