// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.MarryInfoGetHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
  [PacketHandler(235, "获取征婚信息")]
  internal class MarryInfoGetHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      if (client.Player.PlayerCharacter.MarryInfoID != 0)
      {
        int ID = packet.ReadInt();
        using (PlayerBussiness playerBussiness = new PlayerBussiness())
        {
          MarryInfo marryInfoSingle = playerBussiness.GetMarryInfoSingle(ID);
          if (marryInfoSingle != null)
          {
            client.Player.Out.SendMarryInfo(client.Player, marryInfoSingle);
            return 0;
          }
        }
      }
      return 1;
    }
  }
}
