﻿// Decompiled with JetBrains decompiler
// Type: Game.Server.ChatServer.eChatServerPacket
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

namespace Game.Server.ChatServer
{
  public enum eChatServerPacket
  {
    RSAKey = 0,
    LIGHTRIDDLE_INFO = 1,
    LOGIN = 1,
    RANKING_LIGHTRIDDLE = 1,
    GET_LIGHTRIDDLE_INFO = 2,
    KITOFF_USER = 2,
    UPDATE_RANKING_LIGHTRIDDLE = 2,
    ALLOW_USER_LOGIN = 3,
    SEND_LIGHTRIDDLE_AWARD = 3,
    LUCKSTAR_REWARD_RECORD = 4,
    USER_OFFLINE = 4,
    USER_ONLINE = 5,
    USER_STATE = 6,
    UPDATE_ASS = 7,
    UPDATE_CONFIG_STATE = 8,
    CHARGE_MONEY = 9,
    SYS_NOTICE = 10, // 0x0000000A
    SYS_CMD = 11, // 0x0000000B
    PING = 12, // 0x0000000C
    UPDATE_PLAYER_MARRIED_STATE = 13, // 0x0000000D
    MARRY_ROOM_INFO_TO_PLAYER = 14, // 0x0000000E
    SHUTDOWN = 15, // 0x0000000F
    SCENE_CHAT = 19, // 0x00000013
    CHAT_PERSONAL = 37, // 0x00000025
    SYS_MESS = 38, // 0x00000026
    B_BUGLE = 72, // 0x00000048
    WORLD_BOSS_UPDATE_BLOOD = 79, // 0x0000004F
    WORLD_BOSS_START = 80, // 0x00000050
    WORLD_BOSS_RANK = 81, // 0x00000051
    WORLD_BOSS_FIGHTOVER = 82, // 0x00000052
    WORLD_BOSS_ROOMCLOSE = 83, // 0x00000053
    WORLD_BOSS_UPDATEBLOOD = 84, // 0x00000054
    WORLDBOSS_PRIVATE_INFO = 85, // 0x00000055
    WORLD_BOSS_VIEW_RANK = 86, // 0x00000056
    LEAGUE_OPEN_CLOSE = 87, // 0x00000057
    BATTLE_GOUND_OPEN_CLOSE = 88, // 0x00000058
    FIGHTFOOT_BALL_TIME = 89, // 0x00000059
    EVENT_RANKING = 90, // 0x0000005A
    WORLD_EVENT = 91, // 0x0000005B
    MAIL_RESPONSE = 117, // 0x00000075
    MESSAGE = 118, // 0x00000076
    DISPATCHES = 123, // 0x0000007B
    CONSORTIA_RESPONSE = 128, // 0x00000080
    CONSORTIA_CREATE = 130, // 0x00000082
    CONSORTIA_DELETE = 131, // 0x00000083
    CONSORTIA_ALLY_ADD = 147, // 0x00000093
    CONSORTIA_CHAT = 155, // 0x0000009B
    CONSORTIA_OFFER = 156, // 0x0000009C
    CONSORTIA_RICHES = 157, // 0x0000009D
    CONSORTIA_FIGHT = 158, // 0x0000009E
    CONSORTIA_UPGRADE = 159, // 0x0000009F
    IM_CMD = 160, // 0x000000A0
    FRIEND_STATE = 165, // 0x000000A5
    FRIEND_RESPONSE = 166, // 0x000000A6
    RATE = 177, // 0x000000B1
    MACRO_DROP = 178, // 0x000000B2
    CONSORTIA_BOSS_INFO = 180, // 0x000000B4
    CONSORTIA_BOSS_UPDATE_RANK = 181, // 0x000000B5
    CONSORTIA_BOSS_EXTEND_AVAILABLE = 182, // 0x000000B6
    CONSORTIA_BOSS_CREATE_BOSS = 183, // 0x000000B7
    CONSORTIA_BOSS_RELOAD = 184, // 0x000000B8
    CONSORTIA_BOSS_AWARD = 185, // 0x000000B9
    CONSORTIA_BOSS_UPDATE_BLOOD = 186, // 0x000000BA
    CONSORTIA_BOSS_CLOSE = 187, // 0x000000BB
    CONSORTIA_BOSS_DIE = 188, // 0x000000BC
    IP_PORT = 240, // 0x000000F0
    MARRY_ROOM_DISPOSE = 241, // 0x000000F1
    KIT_OFF_PLAYER = 242, // 0x000000F2
    ENTERTAMENT_MODE_GETPOINT = 0xbd,
    ENTERTAMENT_MODE_START_STOP = 0xc0,
    ENTERTAMENT_MODE_UPDATEPOINT = 190,
    }
}