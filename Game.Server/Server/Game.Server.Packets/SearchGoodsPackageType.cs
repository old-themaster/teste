// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.SearchGoodsPackageType
// Assembly: Game.Server, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 95F406BD-7233-42D4-AF91-73FA12644876
// Assembly location: C:\Users\Administrador.OMINIHOST\Desktop\dll8.6\Game.Server.dll

namespace Game.Server.Packets
{
    public enum SearchGoodsPackageType
    {
        TRYENTER = 0,
        RollDice = 1,
        UpgradeStartLevel = 2,
        TakeCard = 3,
        Refresh = 4,
        QuitTakeCard = 5,
        PlayerEnter = 16, // 0x00000010
        PlayerRollDice = 17, // 0x00000011
        PlayerUpgradeStartLevel = 18, // 0x00000012
        BeforeStep = 19, // 0x00000013
        BackStep = 20, // 0x00000014
        ReachTheEnd = 21, // 0x00000015
        BackToStart = 22, // 0x00000016
        GetGoods = 23, // 0x00000017
        FlopCard = 24, // 0x00000018
        PlayNowPosition = 25, // 0x00000019
        TakeCardResponse = 32, // 0x00000020
        RemoveEvent = 33, // 0x00000021
        OneStep = 34, // 0x00000022
    }
}
