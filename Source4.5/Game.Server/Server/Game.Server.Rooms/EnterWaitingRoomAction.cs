// Decompiled with JetBrains decompiler
// Type: Game.Server.Rooms.EnterWaitingRoomAction
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

namespace Game.Server.Rooms
{
    public class EnterWaitingRoomAction : IAction
  {
    private GamePlayer m_player;

    public EnterWaitingRoomAction(GamePlayer player) => this.m_player = player;

    public void Execute()
    {
      if (this.m_player == null)
        return;
      if (this.m_player.CurrentRoom != null)
        this.m_player.CurrentRoom.RemovePlayerUnsafe(this.m_player);
      BaseWaitingRoom waitingRoom = RoomMgr.WaitingRoom;
      if (!waitingRoom.AddPlayer(this.m_player))
        return;
      this.m_player.Out.SendUpdateRoomList(RoomMgr.GetAllRooms());
      this.m_player.Out.SendSceneAddPlayer(this.m_player);
      foreach (GamePlayer player in waitingRoom.GetPlayersSafe())
      {
        if (player != this.m_player)
          this.m_player.Out.SendSceneAddPlayer(player);
      }
    }
  }
}
