// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.UserSynchActionHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.Managers;

namespace Game.Server.Packets.Client
{
    [PacketHandler(36, "用户同步动作")]
  public class UserSynchActionHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      GamePlayer playerById = WorldMgr.GetPlayerById(packet.ClientID);
      if (playerById != null)
      {
        packet.Code = (short) 35;
        packet.ClientID = client.Player.PlayerCharacter.ID;
        playerById.Out.SendTCP(packet);
      }
      return 1;
    }
  }
}
