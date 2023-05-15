
using Game.Base.Packets;

namespace Game.Server.Packets.Client
{
    [PacketHandler(0x195, "场景用户离开")]
    public class WonderfulActivityInitHandler : IPacketHandler
    {
        public WonderfulActivityInitHandler()
        {

        }

        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            int val = packet.ReadInt();
            GSPacketIn pkg = new GSPacketIn(0x195, client.Player.PlayerCharacter.ID);
            pkg.WriteInt(val);
            pkg.WriteInt(1);
            pkg.WriteString("Evento louco 1");
            pkg.WriteInt(1);
            pkg.WriteInt(3);
            pkg.WriteInt(13);
            pkg.WriteInt(1);
            pkg.WriteString("Evento louco 2");
            pkg.WriteInt(0x21);
            client.Player.SendTCP(pkg);
            return 0;
        }
    }
}

