// Decompiled with JetBrains decompiler
// Type: Game.Server.Rooms.StopProxyGameAction
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

namespace Game.Server.Rooms
{
  public class StopProxyGameAction : IAction
  {
    private BaseRoom m_room;

    public StopProxyGameAction(BaseRoom room) => this.m_room = room;

    public void Execute()
    {
      if (this.m_room.Game != null)
        this.m_room.Game.Stop();
      RoomMgr.WaitingRoom.SendUpdateRoom(this.m_room);
    }
  }
}
