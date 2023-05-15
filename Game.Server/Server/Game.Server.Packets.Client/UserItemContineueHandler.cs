// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.UserItemContineueHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using SqlDataProvider.Data;
using System;
using System.Text;

namespace Game.Server.Packets.Client
{
	[PacketHandler(62, "续费")]
	public class UserItemContineueHandler : IPacketHandler
	{
		public int HandlePacket(GameClient client, GSPacketIn packet)
		{
			if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
			{
				client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Bag.Locked"));
				return 0;
			}
			new StringBuilder();
			int num = packet.ReadInt();
			string translateId = "UserItemContineueHandler.Success";
			for (int i = 0; i < num; i++)
			{
				eBageType eBageType = (eBageType)packet.ReadByte();
				int num2 = packet.ReadInt();
				int iD = packet.ReadInt();
				int num3 = packet.ReadByte();
				bool flag = packet.ReadBoolean();
				packet.ReadInt();
				ShopItemInfo shopItemInfoById = ShopMgr.GetShopItemInfoById(iD);
				ItemInfo itemAt = client.Player.GetItemAt(eBageType, num2);
				if (eBageType != 0 || itemAt == null || shopItemInfoById == null || shopItemInfoById.TemplateID != itemAt.TemplateID)
				{
					continue;
				}
				if (itemAt.ValidDate != 0)
				{
					int gold = 0;
					int money = 0;
					int offer = 0;
					int gifttoken = 0;
					int medal = 0;
					int Ascension = 0;
					int damageScore = 0;
					int petScore = 0;
					int iTemplateID = 0;
					int iCount = 0;
					int hardCurrency = 0;
					int LeagueMoney = 0;
					int validDate = itemAt.ValidDate;
					DateTime beginDate = itemAt.BeginDate;
					int count = itemAt.Count;
					bool flag2 = itemAt.IsValidItem();
					if (!ShopMgr.SetItemType(shopItemInfoById, num3, ref damageScore, ref petScore, ref iTemplateID, ref iCount, ref gold, ref money, ref offer, ref gifttoken, ref medal, ref Ascension, ref hardCurrency, ref LeagueMoney, ref medal))
					{
						client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Não foi possível renovar o item."));
						return 0;
					}
					if (gold <= client.Player.PlayerCharacter.Gold && money <= client.Player.PlayerCharacter.Money + client.Player.PlayerCharacter.MoneyLock && offer <= client.Player.PlayerCharacter.Offer && gifttoken <= client.Player.PlayerCharacter.GiftToken)
					{
						if (!flag2)
						{
							if (1 == num3)
							{
								itemAt.ValidDate = shopItemInfoById.AUnit;
							}
							if (2 == num3)
							{
								itemAt.ValidDate = shopItemInfoById.BUnit;
							}
							if (3 == num3)
							{
								itemAt.ValidDate = shopItemInfoById.CUnit;
							}
							itemAt.BeginDate = DateTime.Now;
							itemAt.IsUsed = true;
							client.Player.RemoveMoney(money);
							client.Player.RemoveGold(gold);
							client.Player.RemoveOffer(offer);
							client.Player.RemoveGiftToken(gifttoken);
						}
						else
						{
							translateId = "Falha ao efetuar a renovação do item";
						}
					}
					else
					{
						itemAt.ValidDate = validDate;
						itemAt.Count = count;
						translateId = "UserItemContineueHandler.NoMoney";
					}
				}
				if (flag)
				{
					int num4 = client.Player.EquipBag.FindItemEpuipSlot(itemAt.Template);
					if (client.Player.GetItemAt(eBageType, num4) == null && num2 > client.Player.EquipBag.BeginSlot)
					{
						client.Player.EquipBag.MoveItem(num2, num4, 1);
					}
				}
				else
				{
					client.Player.EquipBag.UpdateItem(itemAt);
				}
			}
			client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation(translateId));
			return 0;
		}
	}
}
