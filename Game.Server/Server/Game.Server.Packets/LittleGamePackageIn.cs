// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.LittleGamePackageIn
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A37EE0F5-57E4-4106-BB52-B5DFCAAC518E
// Assembly location: C:\Users\55849\Downloads\teste\10.2\Server\Road\Game.Server.dll

namespace Game.Server.Packets
{
    public enum LittleGamePackageIn
    {
        LOAD_WORLD_LIST = 1,
        ENTER_WORLD = 2,
        LOAD_COMPLETED = 3,
        LEAVE_WORLD = 4,
        PING = 6,
        MOVE = 32, // 0x00000020
        POS_SYNC = 33, // 0x00000021
        REPORT_SCORE = 64, // 0x00000040
        CLICK = 65, // 0x00000041
        CANCEL_CLICK = 66, // 0x00000042
    }
}
