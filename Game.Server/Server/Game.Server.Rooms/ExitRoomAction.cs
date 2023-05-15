// Decompiled with JetBrains decompiler
// Type: Game.Server.Rooms.ExitRoomAction
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

namespace Game.Server.Rooms
{
    public class ExitRoomAction : IAction
    {
        private BaseRoom baseRoom_0;
        private GamePlayer gamePlayer_0;
        private bool bool_0;

        public ExitRoomAction(BaseRoom room, GamePlayer player, bool isSystem)
        {
            this.baseRoom_0 = room;
            this.gamePlayer_0 = player;
            this.bool_0 = isSystem;
        }

        public void Execute()
        {
            this.baseRoom_0.RemovePlayerUnsafe(this.gamePlayer_0, this.bool_0);
            if (this.baseRoom_0.PlayerCount == 0)
                if (this.baseRoom_0.IsEmpty) // WONDER GAMES DEVELOPER TEAM !! < DO NOT CHANGE HERE !>
                this.baseRoom_0.Stop();

            RoomMgr.WaitingRoom.SendUpdateRoom(this.baseRoom_0.RoomType);
        }
    }
}
