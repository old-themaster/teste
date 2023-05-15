// Decompiled with JetBrains decompiler
// Type: Game.Server.SceneMarryRooms.TankHandle.InviteCommand
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.Managers;

namespace Game.Server.SceneMarryRooms.TankHandle
{
    [MarryCommandAttbute(4)]
  public class InviteCommand : IMarryCommandHandler
  {
    public bool HandleCommand(
      TankMarryLogicProcessor process,
      GamePlayer player,
      GSPacketIn packet)
    {
      if (player.CurrentMarryRoom != null && player.CurrentMarryRoom.RoomState == eRoomState.FREE && (player.CurrentMarryRoom.Info.GuestInvite || player.PlayerCharacter.ID == player.CurrentMarryRoom.Info.GroomID || player.PlayerCharacter.ID == player.CurrentMarryRoom.Info.BrideID))
      {
        GSPacketIn packet1 = packet.Clone();
        packet1.ClearContext();
        GamePlayer playerById = WorldMgr.GetPlayerById(packet.ReadInt());
        if (playerById != null && playerById.CurrentRoom == null && playerById.CurrentMarryRoom == null)
        {
          packet1.WriteByte((byte) 4);
          packet1.WriteInt(player.PlayerCharacter.ID);
          packet1.WriteString(player.PlayerCharacter.NickName);
          packet1.WriteInt(player.CurrentMarryRoom.Info.ID);
          packet1.WriteString(player.CurrentMarryRoom.Info.Name);
          packet1.WriteString(player.CurrentMarryRoom.Info.Pwd);
          packet1.WriteInt(player.MarryMap);
          playerById.Out.SendTCP(packet1);
          return true;
        }
      }
      return false;
    }
  }
}
