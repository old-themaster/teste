// Decompiled with JetBrains decompiler
// Type: Game.Server.RingStation.RoomGamePkg.TankHandle.GamePacket
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Base.Packets;

namespace Game.Server.RingStation.RoomGamePkg.TankHandle
{
  [GameCommandAttbute(91)]
  public class GamePacket : IGameCommandHandler
  {
    public bool HandleCommand(
      TankGameLogicProcessor process,
      RingStationGamePlayer player,
      GSPacketIn packet)
    {
      switch (packet.ReadByte())
      {
        case 6:
          player.NextTurn(packet);
          break;
        case 46:
          player.AddTurn(packet);
          break;
        case 100:
          player.CurRoom.RemovePlayer(player);
          break;
        case 103:
          player.SendLoadingComplete(100);
          break;
      }
      return true;
    }
  }
}
