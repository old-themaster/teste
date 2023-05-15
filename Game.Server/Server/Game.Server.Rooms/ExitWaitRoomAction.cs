// Decompiled with JetBrains decompiler
// Type: Game.Server.Rooms.ExitWaitRoomAction
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

namespace Game.Server.Rooms
{
    public class ExitWaitRoomAction : IAction
  {
    private GamePlayer m_player;

    public ExitWaitRoomAction(GamePlayer player) => this.m_player = player;

    public void Execute() => RoomMgr.WaitingRoom.RemovePlayer(this.m_player);
  }
}
