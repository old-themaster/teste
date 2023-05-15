// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.HotSpringEnterHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.HotSpringRooms;
using Game.Server.Managers;

namespace Game.Server.Packets.Client
{
  [PacketHandler(187, "礼堂数据")]
  public class HotSpringEnterHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      if (WorldMgr.HotSpringScene.GetClientFromID(client.Player.PlayerCharacter.ID) == null)
        WorldMgr.HotSpringScene.AddPlayer(client.Player);
      HotSpringRoom[] allHotSpringRoom = HotSpringMgr.GetAllHotSpringRoom();
      HotSpringMgr.SendUpdateAllRoom(client.Player, allHotSpringRoom);
      return 0;
    }
  }
}
