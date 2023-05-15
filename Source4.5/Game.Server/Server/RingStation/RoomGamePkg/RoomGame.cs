// Decompiled with JetBrains decompiler
// Type: Game.Server.RingStation.RoomGamePkg.RoomGame
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Base.Packets;

namespace Game.Server.RingStation.RoomGamePkg
{
  public class RoomGame
  {
    private IGameProcessor _processor = (IGameProcessor) new TankGameLogicProcessor();
    private static object _syncStop = new object();

    protected void OnTick(object obj) => this._processor.OnTick(this);

    public void ProcessData(RingStationGamePlayer player, GSPacketIn data)
    {
      lock (RoomGame._syncStop)
        this._processor.OnGameData(this, player, data);
    }
  }
}
