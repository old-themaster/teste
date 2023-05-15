using Game.Base.Packets;



namespace Game.Server.Packets.Client
{
    [PacketHandler(262, "场景用户离开")]
    public class NewHallHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            byte num = packet.ReadByte();
            new GSPacketIn(0x106);
            switch (num)
            {
                case 0:
                    this.SendPlayerInfo(client, packet);
                    break;

                case 2:
                    packet.ReadInt();
                    break;

                case 3:
                    {
                        int num2 = packet.ReadInt();
                        int num3 = packet.ReadInt();
                        client.Player.NewHallX = num2;
                        client.Player.NewHallY = num3;
                        break;
                    }
                case 4:
                    packet.ReadInt();
                    break;

                case 7:
                    packet.ReadBoolean();
                    break;
            }
            return 0;
        }

        private void SendPlayerInfo(GameClient client, GSPacketIn packet)
        {
            GSPacketIn pkg = new GSPacketIn(262);
            pkg.WriteByte(0);
            pkg.WriteInt(client.Player.NewHallX);
            pkg.WriteInt(client.Player.NewHallY);
            pkg.WriteInt(0);
            client.Player.SendTCP(pkg);
        }
    }
}