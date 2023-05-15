
using Game.Base.Packets;
namespace Game.Server.Packets.Client
{
    [PacketHandler(0x9f, "场景用户离开")]
    public class WonderfulActivityHandler : IPacketHandler
    {
        public WonderfulActivityHandler()
        {

        }

        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            int num = packet.ReadInt();
            packet.ReadInt();
            GSPacketIn pkg = new GSPacketIn(0x9f, client.Player.PlayerCharacter.ID);
            pkg.WriteByte((byte)num);
            pkg.WriteInt(0);
            client.Player.SendTCP(pkg);
            return 0;
        }
    }
}

