// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.UserSceneReadyHandler
// Assembly: Game.Server, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0B480679-DF24-46B7-834E-821AA9A4FB3F
// Assembly location: C:\Users\Anderson\Desktop\Source 4.2\Game.Server.dll

using Game.Base.Packets;
using Game.Server.GameObjects;

namespace Game.Server.Packets.Client
{
  [PacketHandler(17, "Client scene ready1")]
  public class UserSceneReadyHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      if (client.Player.CurrentRoom != null)
      {
        GSPacketIn packet1 = (GSPacketIn) null;
        foreach (GamePlayer player in client.Player.CurrentRoom.GetPlayers())
        {
          if (player != client.Player)
          {
            if (packet1 == null)
              packet1 = player.Out.SendSceneAddPlayer(client.Player);
            else
              player.Out.SendTCP(packet1);
            client.Out.SendSceneRemovePlayer(player);
          }
        }
      }
      return 1;
    }
  }
}
