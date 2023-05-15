// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.MarryRoomInfoUpdateHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.SceneMarryRooms;

namespace Game.Server.Packets.Client
{
  [PacketHandler(237, "更新礼堂信息")]
  internal class MarryRoomInfoUpdateHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      if (client.Player.CurrentMarryRoom == null || client.Player.PlayerCharacter.ID != client.Player.CurrentMarryRoom.Info.PlayerID)
        return 1;
      string str1 = packet.ReadString();
      int num = packet.ReadBoolean() ? 1 : 0;
      string str2 = packet.ReadString();
      string str3 = packet.ReadString();
      MarryRoom currentMarryRoom = client.Player.CurrentMarryRoom;
      currentMarryRoom.Info.RoomIntroduction = str3;
      currentMarryRoom.Info.Name = str1;
      if (num != 0)
        currentMarryRoom.Info.Pwd = str2;
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
        playerBussiness.UpdateMarryRoomInfo(currentMarryRoom.Info);
      currentMarryRoom.SendMarryRoomInfoUpdateToScenePlayers(currentMarryRoom);
      client.Player.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("MarryRoomInfoUpdateHandler.Successed"));
      return 0;
    }
  }
}
