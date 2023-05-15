// Decompiled with JetBrains decompiler
// Type: Game.Server.SceneMarryRooms.TankHandle.GunsaluteCommand
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.Packets;

namespace Game.Server.SceneMarryRooms.TankHandle
{
    [MarryCommandAttbute(11)]
  public class GunsaluteCommand : IMarryCommandHandler
  {
    public bool HandleCommand(
      TankMarryLogicProcessor process,
      GamePlayer player,
      GSPacketIn packet)
    {
      if (player.CurrentMarryRoom != null)
      {
        packet.ReadInt();
        if (ItemMgr.FindItemTemplate(packet.ReadInt()) != null && !player.CurrentMarryRoom.Info.IsGunsaluteUsed && (player.CurrentMarryRoom.Info.GroomID == player.PlayerCharacter.ID || player.CurrentMarryRoom.Info.BrideID == player.PlayerCharacter.ID))
        {
          player.CurrentMarryRoom.ReturnPacketForScene(player, packet);
          player.CurrentMarryRoom.Info.IsGunsaluteUsed = true;
          GSPacketIn packet1 = player.Out.SendMessage(eMessageType.ChatNormal, LanguageMgr.GetTranslation("GunsaluteCommand.Successed1", (object) player.PlayerCharacter.NickName));
          player.CurrentMarryRoom.SendToPlayerExceptSelfForScene(packet1, player);
          GameServer.Instance.LoginServer.SendMarryRoomInfoToPlayer(player.CurrentMarryRoom.Info.GroomID, true, player.CurrentMarryRoom.Info);
          GameServer.Instance.LoginServer.SendMarryRoomInfoToPlayer(player.CurrentMarryRoom.Info.BrideID, true, player.CurrentMarryRoom.Info);
          using (PlayerBussiness playerBussiness = new PlayerBussiness())
            playerBussiness.UpdateMarryRoomInfo(player.CurrentMarryRoom.Info);
          return true;
        }
      }
      return false;
    }
  }
}
