// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.HotSpringRoomQuickEnterHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.HotSpringRooms;
using Game.Server.Managers;

namespace Game.Server.Packets.Client
{
  [PacketHandler(190, "礼堂数据")]
  public class HotSpringRoomQuickEnterHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      if (client.Player.CurrentHotSpringRoom == null)
      {
        HotSpringRoom randomRoom = HotSpringMgr.GetRandomRoom();
        if (randomRoom != null)
        {
          if (randomRoom.AddPlayer(client.Player))
            client.Out.SendEnterHotSpringRoom(client.Player);
        }
        else
          client.Player.SendMessage(LanguageMgr.GetTranslation("SpaRoomLoginHandler.Failed4"));
      }
      else
        client.Player.SendMessage(LanguageMgr.GetTranslation("SpaRoomLoginHandler.Failed"));
      return 0;
    }
  }
}
