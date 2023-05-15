using System;
using Bussiness;
using Game.Base.Packets;
using Game.Logic;
using Game.Server.Managers;
using Game.Server.Buffer;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client

{

	[PacketHandler(99, "Prática")]
	public class TexpHandler : IPacketHandler
	{
		public int HandlePacket(GameClient client, GSPacketIn packet)
		{
			int num = packet.ReadInt();
			int num2 = packet.ReadInt();
			int num3 = packet.ReadInt();
			int slot = packet.ReadInt();
			string attr = "HP";
			ItemInfo itemAt = client.Player.StoreBag.GetItemAt(slot);
			TexpInfo texp = client.Player.PlayerCharacter.Texp;
			int oldExp = 0;
			if (itemAt == null || texp == null || itemAt.TemplateID != num2)
			{
				client.Out.SendMessage(eMessageType.GM_NOTICE, "Ocorreu um erro, mude o canal e tente novamente.");
				return 0;
			}
			if (!itemAt.isTexp())
			{
				client.Out.SendMessage(eMessageType.GM_NOTICE, "Ocorreu um erro, mude o canal e tente novamente.");
				return 0;
			}
			if (texp.texpCount <= client.Player.PlayerCharacter.Grade)
			{
				if (num3 + texp.texpCount >= client.Player.PlayerCharacter.Grade)
				{
					num3 = client.Player.PlayerCharacter.Grade - texp.texpCount;
				}
				if (client.Player.UsePayBuff(BuffType.Train_Good))
				{
					AbstractBuffer ofType = client.Player.BufferList.GetOfType(BuffType.Train_Good);					
				}
				switch (num)
				{
					case 0:
						oldExp = texp.hpTexpExp;
						texp.hpTexpExp += itemAt.Template.Property2 * num3;
						client.Player.OnUsingItem(45005, 1);
						break;
					case 1:
						oldExp = texp.attTexpExp;
						texp.attTexpExp += itemAt.Template.Property2 * num3;
						client.Player.OnUsingItem(45001, 1);
						attr = "Ataque";
						break;
					case 2:
						oldExp = texp.defTexpExp;
						texp.defTexpExp += itemAt.Template.Property2 * num3;
						client.Player.OnUsingItem(45002, 1);
						attr = "Defesa";
						break;
					case 3:
						oldExp = texp.spdTexpExp;
						texp.spdTexpExp += itemAt.Template.Property2 * num3;
						client.Player.OnUsingItem(45003, 1);
						attr = "Sorte";
						break;
					case 4:
						oldExp = texp.lukTexpExp;
						texp.lukTexpExp += itemAt.Template.Property2 * num3;
						client.Player.OnUsingItem(45004, 1);
						attr = "Agilidade";
						break;
				}
				texp.texpCount += num3;
				texp.texpTaskCount++;
				texp.texpTaskDate = DateTime.Now;
				using (PlayerBussiness playerBussiness = new PlayerBussiness())
				{
					playerBussiness.UpdateUserTexpInfo(texp);
				}
				client.Player.PlayerCharacter.Texp = texp;
				client.Player.StoreBag.RemoveTemplate(num2, num3);
				client.Player.EquipBag.UpdatePlayerProperties();
				client.Player.OnUsingItem(num2, num3);
				if (ExerciseMgr.isUp(num, oldExp, texp))
				{
					GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
					for (int i = 0; i < allPlayers.Length; i++)
					{
						allPlayers[i].Out.SendMessage(eMessageType.ChatNormal, LanguageMgr.GetTranslation("TexpHandler.Success", client.Player.ZoneName, client.Player.PlayerCharacter.NickName, attr));
					}
				}
			}
			else
			{
				client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("texpSystem.texpCountToplimit"));
			}
			return 0;
		}
	}
}
