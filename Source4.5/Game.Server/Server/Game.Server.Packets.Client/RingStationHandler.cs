
using Game.Base.Packets;
using System;

namespace Game.Server.Packets.Client
{
    [PacketHandler(404, "场景用户离开")]
    public class RingStationHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            byte num = packet.ReadByte();
            int id = client.Player.PlayerCharacter.ID;
            GSPacketIn pkg = new GSPacketIn((short)404, id);
            switch (num)
            {
                case 1:
                    pkg.WriteByte((byte)1);
                    pkg.WriteInt(1);
                    pkg.WriteInt(10);
                    pkg.WriteInt(10);
                    pkg.WriteInt(80000);
                    pkg.WriteDateTime(DateTime.Now);
                    pkg.WriteInt(100000);
                    pkg.WriteInt(0);
                    pkg.WriteString("Ủn ỉn chán nản vì không làm được lôi đài. Hu huuuu");
                    pkg.WriteInt(300);
                    pkg.WriteDateTime(DateTime.Now.AddDays(7.0));
                    pkg.WriteString("Ủn ỉn No.1");
                    pkg.WriteInt(4);
                    for (int index = 0; index < 4; ++index)
                    {
                        pkg.WriteInt(id);
                        pkg.WriteString(client.Player.PlayerCharacter.UserName);
                        pkg.WriteString(client.Player.PlayerCharacter.NickName);
                        pkg.WriteByte(client.Player.PlayerCharacter.typeVIP);
                        pkg.WriteInt(client.Player.PlayerCharacter.VIPLevel);
                        pkg.WriteInt(client.Player.PlayerCharacter.Grade);
                        pkg.WriteBoolean(client.Player.PlayerCharacter.Sex);
                        pkg.WriteString(client.Player.PlayerCharacter.Style);
                        pkg.WriteString(client.Player.PlayerCharacter.Colors);
                        pkg.WriteString(client.Player.PlayerCharacter.Skin);
                        pkg.WriteString(client.Player.PlayerCharacter.ConsortiaName);
                        pkg.WriteInt(client.Player.PlayerCharacter.Hide);
                        pkg.WriteInt(client.Player.PlayerCharacter.Offer);
                        pkg.WriteInt(client.Player.PlayerCharacter.Win);
                        pkg.WriteInt(client.Player.PlayerCharacter.Total);
                        pkg.WriteInt(client.Player.PlayerCharacter.Escape);
                        pkg.WriteInt(client.Player.PlayerCharacter.Repute);
                        pkg.WriteInt(client.Player.PlayerCharacter.Nimbus);
                        pkg.WriteInt(client.Player.PlayerCharacter.GP);
                        pkg.WriteInt(client.Player.PlayerCharacter.FightPower);
                        pkg.WriteInt(client.Player.PlayerCharacter.AchievementPoint);
                        pkg.WriteInt(2 + index);
                        if (client.Player.MainWeapon == null)
                            pkg.WriteInt(7008);
                        else
                            pkg.WriteInt(client.Player.MainWeapon.TemplateID);
                        pkg.WriteString("Ủn ỉn thách thức");
                    }
                    client.Player.SendTCP(pkg);
                    break;
                case 6:
                    pkg.WriteByte((byte)6);
                    pkg.WriteInt(0);
                    pkg.WriteDateTime(DateTime.Now);
                    client.Player.SendTCP(pkg);
                    break;
                default:
                    Console.WriteLine("RingStationPackageType." + (object)(RingStationPackageType)num);
                    break;
            }
            return 0;
        }
    }
}
