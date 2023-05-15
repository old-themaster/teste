// Decompiled with JetBrains decompiler
// Type: Game.Server.Rooms.StartGameMissionAction
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Logic;

namespace Game.Server.Rooms
{
  public class StartGameMissionAction : IAction
  {
    private BaseRoom m_room;

    public StartGameMissionAction(BaseRoom room) => this.m_room = room;

    public void Execute() => this.m_room.Game.MissionStart((IGamePlayer) this.m_room.Host);
  }
}
