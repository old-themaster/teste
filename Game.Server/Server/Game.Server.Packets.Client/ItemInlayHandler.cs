using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;

namespace Game.Server.Packets.Client
{
    [PacketHandler(121, "Inci Yeri")]
	public class ItemInlayHandler : IPacketHandler
	{
		public int HandlePacket(GameClient client, GSPacketIn packet)
		{
			GSPacketIn gSPacketIn = packet.Clone();
			gSPacketIn.ClearContext();
			int bagType = packet.ReadInt();
			int place = packet.ReadInt();
			int num = packet.ReadInt();
			int bagType2 = packet.ReadInt();
			int place2 = packet.ReadInt();
			ItemInfo itemAt = client.Player.GetItemAt((eBageType)bagType, place);
			ItemInfo itemAt2 = client.Player.GetItemAt((eBageType)bagType2, place2);
			string text = "";
			int inlayGoldPrice = GameProperties.InlayGoldPrice;
			if (itemAt == null || itemAt2 == null || itemAt2.Template.Property1 != 31)
			{
				return 0;
			}
			if (client.Player.PlayerCharacter.Gold >= inlayGoldPrice)
			{
				string[] array = itemAt.Template.Hole.Split('|');
				if (num > 0 && num < 7)
				{
					client.Player.RemoveGold(inlayGoldPrice);
					bool flag = false;
					switch (num)
					{
						case 1:
							if (Convert.ToInt32(array[0].Split(',')[1]) == itemAt2.Template.Property2)
							{
								itemAt.Hole1 = itemAt2.TemplateID;
								string text2 = text + "," + itemAt2.ItemID + "," + itemAt2.Template.Name;
								flag = true;
							}
							break;
						case 2:
							if (Convert.ToInt32(array[1].Split(',')[1]) == itemAt2.Template.Property2)
							{
								itemAt.Hole2 = itemAt2.TemplateID;
								string text3 = text + "," + itemAt2.ItemID + "," + itemAt2.Template.Name;
								flag = true;
							}
							break;
						case 3:
							if (Convert.ToInt32(array[2].Split(',')[1]) == itemAt2.Template.Property2)
							{
								itemAt.Hole3 = itemAt2.TemplateID;
								string text4 = text + "," + itemAt2.ItemID + "," + itemAt2.Template.Name;
								flag = true;
							}
							break;
						case 4:
							if (Convert.ToInt32(array[3].Split(',')[1]) == itemAt2.Template.Property2)
							{
								itemAt.Hole4 = itemAt2.TemplateID;
								string text5 = text + "," + itemAt2.ItemID + "," + itemAt2.Template.Name;
								flag = true;
							}
							break;
						case 5:
							{
								if (Convert.ToInt32(array[4].Split(',')[1]) != itemAt2.Template.Property2)
								{
									break;
								}
								if (itemAt.Hole5 != 0)
								{
									ItemInfo itemInfo2 = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(itemAt.Hole5), 1, 102);
									// itemInfo2.IsBinds = true;
									client.Player.UpdateItem(itemAt);
									client.Player.UpdateItem(itemAt2);
									client.Player.SendMessage(LanguageMgr.GetTranslation("Por favor, 'UPGRADE' primeiro.. "));
									Console.WriteLine("yarram ainda é o mesmo bug");

									itemInfo2.ValidDate = 0;
									if (!client.Player.AddItem(itemInfo2))
									{
										GamePlayer player2 = client.Player;
										List<ItemInfo> items2 = new List<ItemInfo>
								{
									itemInfo2
								};
										string content2 = "Mochila cheia.";
										string title2 = "Mochila cheia";
										int type2 = 8;
										player2.SendItemsToMail(items2, content2, title2, (eMailType)type2);
									}
								}
								itemAt.Hole5 = itemAt2.TemplateID;
								string text6 = text + "," + itemAt2.ItemID + "," + itemAt2.Template.Name;
								flag = true;
								break;
							}
						case 6:
							{
								if (Convert.ToInt32(array[5].Split(',')[1]) != itemAt2.Template.Property2)
								{
									break;
								}
								if (itemAt.Hole6 != 0)
								{
									ItemInfo itemInfo = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(itemAt.Hole6), 1, 102);
									//	itemInfo.IsBinds = true;
									client.Player.UpdateItem(itemAt);
									client.Player.UpdateItem(itemAt2);
									client.Player.SendMessage(LanguageMgr.GetTranslation("Por favor, 'ATUALIZE' primeiro.. "));
									Console.WriteLine("yarram ainda é o mesmo bug");
									itemInfo.ValidDate = 0;
									if (!client.Player.AddItem(itemInfo))
									{
										GamePlayer player = client.Player;
										List<ItemInfo> items = new List<ItemInfo>
								{
									itemInfo
								};
										string content = "Mochila cheia.";
										string title = "Mochila cheia";
										int type = 8;
										player.SendItemsToMail(items, content, title, (eMailType)type);
									}
								}
								itemAt.Hole6 = itemAt2.TemplateID;
								string text7 = text + "," + itemAt2.ItemID + "," + itemAt2.Template.Name;
								flag = true;
								break;
							}
					}
					if (flag)
					{
						gSPacketIn.WriteInt(0);
						itemAt2.Count--;
						client.Player.UpdateItem(itemAt2);
						client.Player.UpdateItem(itemAt);
					}
					else
					{
						client.Player.SendMessage(LanguageMgr.GetTranslation("GameServer.InlayItem.Msg1"));
					}
				}
				else
				{
					gSPacketIn.WriteByte(1);
					client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("ItemInlayHandle.NoPlace"));
				}
				client.Player.SendTCP(gSPacketIn);
				client.Player.SaveIntoDatabase();
			}
			else
			{
				client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("UserBuyItemHandler.NoMoney"));
			}
			return 0;
		}
	}
}
