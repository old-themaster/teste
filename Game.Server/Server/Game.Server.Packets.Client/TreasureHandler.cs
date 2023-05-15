using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
namespace Game.Server.Packets.Client
{
    [PacketHandler(135, "场景用户离开")]
	public class TreasureHandler : IPacketHandler
	{
		public int HandlePacket(GameClient client, GSPacketIn packet)
		{
			int num = packet.ReadInt();
			int iD = client.Player.PlayerCharacter.ID;
			GSPacketIn gSPacketIn = new GSPacketIn(135, iD);
			switch (num)
			{
				case 0:
					{
						client.Player.Treasure.UpdateLoginDay();
						UserTreasureInfo currentTreasure = client.Player.Treasure.CurrentTreasure;
						List<TreasureDataInfo> treasureData = client.Player.Treasure.TreasureData;
						List<TreasureDataInfo> treasureDig = client.Player.Treasure.TreasureDig;
						gSPacketIn.WriteInt(0);
						gSPacketIn.WriteInt(currentTreasure.logoinDays);
						gSPacketIn.WriteInt(currentTreasure.treasure);
						gSPacketIn.WriteInt(currentTreasure.treasureAdd);
						gSPacketIn.WriteInt(currentTreasure.friendHelpTimes);
						gSPacketIn.WriteBoolean(currentTreasure.isEndTreasure);
						gSPacketIn.WriteBoolean(currentTreasure.isBeginTreasure);
						gSPacketIn.WriteInt(treasureData.Count);
						for (int i = 0; i < treasureData.Count; i++)
						{
							gSPacketIn.WriteInt(treasureData[i].TemplateID);
							gSPacketIn.WriteInt(treasureData[i].ValidDate);
							gSPacketIn.WriteInt(treasureData[i].Count);
						}
						gSPacketIn.WriteInt(treasureDig.Count);
						for (int i = 0; i < treasureDig.Count; i++)
						{
							gSPacketIn.WriteInt(treasureDig[i].TemplateID);
							gSPacketIn.WriteInt(treasureDig[i].pos);
							gSPacketIn.WriteInt(treasureDig[i].ValidDate);
							gSPacketIn.WriteInt(treasureDig[i].Count);
						}
						client.Player.Out.SendTCP(gSPacketIn);
						return 0;
					}
				case 1:
					{
						int num2 = packet.ReadInt();
						gSPacketIn.WriteInt(1);
						gSPacketIn.WriteInt(0);
						client.Player.SendTCP(gSPacketIn);
						GamePlayer playerById = WorldMgr.GetPlayerById(num2);
						if (playerById == null)
						{
							using (PlayerBussiness playerBussiness = new PlayerBussiness())
							{
								playerBussiness.RemoveIsArrange(num2);
								playerBussiness.UpdateFriendHelpTimes(num2);
								return 0;
							}
						}
					//	playerById.Farm.ClearIsArrange();
						playerById.Treasure.AddfriendHelpTimes();
						gSPacketIn.ClientID = playerById.PlayerCharacter.ID;
						playerById.SendTCP(gSPacketIn);
						return 0;
					}
				case 2:
					{
						UserTreasureInfo currentTreasure2 = client.Player.Treasure.CurrentTreasure;
						currentTreasure2.isBeginTreasure = false;
						currentTreasure2.isEndTreasure = true;
						gSPacketIn.WriteInt(2);
						gSPacketIn.WriteBoolean(currentTreasure2.isEndTreasure);
						client.Player.SendTCP(gSPacketIn);
						client.Player.Treasure.UpdateUserTreasure(currentTreasure2);
						client.Out.SendMessage(eMessageType.ERROR, LanguageMgr.GetTranslation("Số lần đào hôm nay đã hết.", new object[0]));
						return 0;
					}
				case 3:
					{
						int num3 = packet.ReadInt();
						int index = num3 - 1;
						bool flag = true;
						TreasureDataInfo treasureDataInfo = client.Player.Treasure.TreasureData[index];
						if (treasureDataInfo == null)
						{
							return 0;
						}
						UserTreasureInfo currentTreasure3 = client.Player.Treasure.CurrentTreasure;
						if (currentTreasure3.treasure > 0)
						{
							currentTreasure3.treasure--;
						}
						else
						{
							if (currentTreasure3.treasureAdd > 0)
							{
								currentTreasure3.treasureAdd--;
							}
							else
							{
								flag = false;
							}
						}
						if (flag)
						{
							gSPacketIn.WriteInt(3);
							gSPacketIn.WriteInt(treasureDataInfo.TemplateID);
							gSPacketIn.WriteInt(num3);
							gSPacketIn.WriteInt(treasureDataInfo.Count);
							gSPacketIn.WriteInt(currentTreasure3.treasure);
							gSPacketIn.WriteInt(currentTreasure3.treasureAdd);
							client.Player.Out.SendTCP(gSPacketIn);
							treasureDataInfo.pos = num3;
							client.Player.Treasure.AddTreasureDig(treasureDataInfo, index);
							client.Player.Treasure.UpdateUserTreasure(currentTreasure3);
							ItemInfo itemInfo = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(treasureDataInfo.TemplateID), treasureDataInfo.Count, 105);
							itemInfo.IsBinds = true;
							itemInfo.ValidDate = treasureDataInfo.ValidDate;
							client.Player.AddTemplate(itemInfo, itemInfo.Template.BagType, treasureDataInfo.Count, eGameView.RouletteTypeGet);
							client.Player.SendHideMessage(LanguageMgr.GetTranslation(string.Concat(new object[]
							{
						"Você Ganhou ",
						itemInfo.Template.Name,
						" x",
						treasureDataInfo.Count
							}), new object[0]));
							return 0;
						}
						return 0;
					}
				case 6:
					{
						UserTreasureInfo currentTreasure4 = client.Player.Treasure.CurrentTreasure;
						currentTreasure4.isBeginTreasure = true;
						gSPacketIn.WriteInt(6);
						gSPacketIn.WriteBoolean(currentTreasure4.isBeginTreasure);
						client.Player.SendTCP(gSPacketIn);
						client.Player.Treasure.UpdateUserTreasure(currentTreasure4);
						return 0;
					}
			}
			Console.WriteLine("//treasure_cmd: " + (TreasurePackageType)num);
			return 0;
		}
	}
}
