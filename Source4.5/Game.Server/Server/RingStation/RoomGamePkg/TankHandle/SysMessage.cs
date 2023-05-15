// Decompiled with JetBrains decompiler
// Type: Game.Server.RingStation.RoomGamePkg.TankHandle.SysMessage
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Base.Packets;

namespace Game.Server.RingStation.RoomGamePkg.TankHandle
{
  [GameCommandAttbute(3)]
  public class SysMessage : IGameCommandHandler
  {
    public bool HandleCommand(
      TankGameLogicProcessor process,
      RingStationGamePlayer player,
      GSPacketIn packet) => true;
  }
}
