﻿using Game.Base.Packets;
using SqlDataProvider.Data;
namespace Game.Server.Packets.Client
{
    [PacketHandler(95, "客户端日记")]
	public class NecklaceStrengthHandler : IPacketHandler
	{
		public int HandlePacket(GameClient client, GSPacketIn packet)
		{
			int num = (int)packet.ReadByte();
			int slot = packet.ReadInt();
			int num2 = packet.ReadInt();
			ItemInfo itemAt = client.Player.PropBag.GetItemAt(slot);
			if (itemAt.Count < num2)
			{
				num2 = itemAt.Count;
			}
			switch (num)
			{
				case 2:
					{
						int lv = 12;
						int necklaceExpAdd = client.Player.PlayerCharacter.necklaceExpAdd;
						int num3 = client.Player.PlayerCharacter.necklaceExp;
						//int necklaceMaxExp = StrengthenMgr.GetNecklaceMaxExp(lv);
						//if (num3 <= necklaceMaxExp)
						{
							if (itemAt != null && itemAt.TemplateID == 11160 && itemAt.Count > 0 && num2 > 0)
							{
								int property = itemAt.Template.Property2;
								num3 += property * num2;
								//int num4 = StrengthenMgr.GetNecklacePlus(num3, necklaceExpAdd);
								/*if (num3 >= necklaceMaxExp)
								{
									num4 = StrengthenMgr.GetNecklaceMaxPlus(lv);
									num2 -= (num3 - necklaceMaxExp) / property;
									client.Player.PlayerCharacter.necklaceExp = necklaceMaxExp + property;
								}*/
								//else
								{
									client.Player.PlayerCharacter.necklaceExp = num3;
								}
								//if (num4 > necklaceExpAdd)
								{
								//	client.Player.PlayerCharacter.necklaceExpAdd = num4;
									client.Player.EquipBag.UpdatePlayerProperties();
								}
								client.Player.RemoveTemplate(itemAt.TemplateID, num2);
							}
						}
						//else
						{
							client.Player.SendMessage("Colar já está no lv Maximo !");
						}
						break;
					}
			}
			client.Player.Out.SendNecklaceStrength(client.Player.PlayerCharacter);
			return 0;
		}
	}
}
