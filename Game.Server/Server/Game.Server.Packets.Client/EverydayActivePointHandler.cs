using Game.Base.Packets;
using System;
namespace Game.Server.Packets.Client
{
	[PacketHandler(155, "场景用户离开")]
	public class EverydayActivePointHandler : IPacketHandler
	{
		public int HandlePacket(GameClient client, GSPacketIn packet)
		{
			byte b = packet.ReadByte();
			int iD = client.Player.PlayerCharacter.ID;
			byte b2 = b;
			if (b2 == 8)
			{
				client.Player.Out.SendExpBlessedData(iD);
			}
			else
			{
				Console.WriteLine("ActivityPackageType." + (ActivityPackageType)b);
			}
			return 0;
		}
	}
}
