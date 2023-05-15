// Decompiled with JetBrains decompiler
// Type: Game.Server.Rooms.UpdateRoomPosAction
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

namespace Game.Server.Rooms
{
  public class UpdateRoomPosAction : IAction
  {
    private BaseRoom baseRoom_0;
    private int int_0;
    private bool bool_0;

    public UpdateRoomPosAction(BaseRoom room, int pos, bool isOpened)
    {
      this.baseRoom_0 = room;
      this.int_0 = pos;
      this.bool_0 = isOpened;
    }

    public void Execute()
    {
      if (this.baseRoom_0.PlayerCount <= 0 || !this.baseRoom_0.UpdatePosUnsafe(this.int_0, this.bool_0))

        return;
      RoomMgr.WaitingRoom.SendUpdateCurrentRoom(this.baseRoom_0);
    }
  }
}
