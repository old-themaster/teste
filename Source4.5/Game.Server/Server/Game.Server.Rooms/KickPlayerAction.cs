// Decompiled with JetBrains decompiler
// Type: Game.Server.Rooms.KickPlayerAction
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

namespace Game.Server.Rooms
{
  public class KickPlayerAction : IAction
  {
    private int m_place;
    private BaseRoom m_room;

    public KickPlayerAction(BaseRoom room, int place)
    {
      this.m_room = room;
      this.m_place = place;
    }

    public void Execute() => this.m_room.RemovePlayerAtUnsafe(this.m_place);
  }
}
