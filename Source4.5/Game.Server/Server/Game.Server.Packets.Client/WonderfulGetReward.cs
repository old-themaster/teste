using Game.Base.Packets;
using Game.Server.WonderFul;

namespace Game.Server.Packets.Client
{
    [PacketHandler(373, "WonderfulGetReward")]
    internal class WonderfulGetReward : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            WonderFulActivityManager.SendWonderFulReward(client.Player, packet);
            return 0;
        }
    }
}
