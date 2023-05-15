// Decompiled with JetBrains decompiler
// Type: Game.Server.Rooms.StartProxyGameAction
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Logic;
using Game.Server.Battle;

namespace Game.Server.Rooms
{
  public class StartProxyGameAction : IAction
  {
    private ProxyGame m_game;
    private BaseRoom m_room;

    public StartProxyGameAction(BaseRoom room, ProxyGame game)
    {
      this.m_room = room;
      this.m_game = game;
    }

    public void Execute() => this.m_room.StartGame((AbstractGame) this.m_game);
  }
}
