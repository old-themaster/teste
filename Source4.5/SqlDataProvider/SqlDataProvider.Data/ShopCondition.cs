// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.ShopCondition
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6DE9576-B5AF-4392-BBCE-95C72793F7EA
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
  public class ShopCondition
  {
    public static bool isDDTMoney(int type) => type == 2;

    public static bool isLabyrinth(int type) => type == 94;

    public static bool isLeague(int type) => type == 93;

    public static bool isMoney(int type) => type == 1;

    public static bool isOffer(int type)
    {
      switch (type)
      {
        case 11:
        case 12:
        case 13:
        case 14:
        case 15:
          return true;
        default:
          return false;
      }
    }

    public static bool isPetScrore(int type) => type == 92;

    public static bool isSearchGoods(int type) => type == 99;

    public static bool isWorldBoss(int type) => type == 91;
  }
}
