﻿// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.IMPackageType
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

namespace Game.Server.Packets
{
  public enum IMPackageType
  {
    ONS_EQUIP = 45, // 0x0000002D
    ONE_ON_ONE_TALK = 51, // 0x00000033
    FRIEND_ADD = 160, // 0x000000A0
    FRIEND_REMOVE = 161, // 0x000000A1
    FRIEND_UPDATE = 162, // 0x000000A2
    SAME_CITY_FRIEND = 164, // 0x000000A4
    FRIEND_STATE = 165, // 0x000000A5
    FRIEND_RESPONSE = 166, // 0x000000A6
    ADD_CUSTOM_FRIENDS = 208, // 0x000000D0
  }
}
