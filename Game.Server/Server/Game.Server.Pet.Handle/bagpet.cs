using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Logic;
using Game.Server.Packets;
using SqlDataProvider.Data;

namespace Game.Server.Pet.Handle
{
	[global::Pet(19)]
	public class bagpet : IPetCommandHadler
	{
		public bool CommandHandler(GamePlayer player, GSPacketIn packet)
		{			
			bool flag5 = packet.ReadBoolean();
			if (player.PlayerCharacter.HasBagPassword && player.PlayerCharacter.IsLocked)
			{
				player.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("Bag.Locked", new object[0]));
				return false;
			}
			UserFarmInfo currentFarm = player.Farm.CurrentFarm;
			int buyExpRemainNum = currentFarm.buyExpRemainNum;
			PetExpItemPriceInfo petExpItemPriceInfo = PetMgr.FindPetExpItemPrice(this.RealMoney(buyExpRemainNum));
			if (petExpItemPriceInfo == null || buyExpRemainNum < 1)
			{
				return false;
			}
			bool flag6 = false;
			int money = petExpItemPriceInfo.Money;
			if (flag5)
			{
				if (player.MoneyDirect(money))
				{
					player.AddExpVip(money);
					flag6 = true;
				}
			}
			else
			{
				if (money <= player.PlayerCharacter.GiftToken)
				{
					player.RemoveGiftToken(money);
					flag6 = true;
				}
			}
			if (!flag6)
			{
				if (GameProperties.IsDDTMoneyActive)
				{
					player.SendMessage("O bloqueio de moedas não é suficiente. Ação falhada.");
				}
				else
				{
					player.SendMessage("Tente Mais Tarde...");
				}
				return false;
			}
			ItemTemplateInfo goods2 = ItemMgr.FindItemTemplate(334102);
			ItemInfo itemInfo2 = ItemInfo.CreateFromTemplate(goods2, petExpItemPriceInfo.ItemCount, 102);
			itemInfo2.IsBinds = true;
			player.AddTemplate(itemInfo2, itemInfo2.Template.BagType, petExpItemPriceInfo.ItemCount, eGameView.RouletteTypeGet);
			currentFarm.buyExpRemainNum--;
			GSPacketIn gSPacketIn = new GSPacketIn(68);
			gSPacketIn.WriteByte(19);
			gSPacketIn.WriteInt(currentFarm.buyExpRemainNum);
			player.SendTCP(gSPacketIn);
			player.Farm.UpdateFarm(currentFarm);
			return false;
			
		}

        private int RealMoney(int timebuy)
		{
			switch (timebuy)
			{
				case 1:
					return 20;
				case 2:
					return 19;
				case 3:
					return 18;
				case 4:
					return 17;
				case 5:
					return 16;
				case 6:
					return 15;
				case 7:
					return 14;
				case 8:
					return 13;
				case 9:
					return 12;
				case 10:
					return 11;
				case 11:
					return 10;
				case 12:
					return 9;
				case 13:
					return 8;
				case 14:
					return 7;
				case 15:
					return 6;
				case 16:
					return 5;
				case 17:
					return 4;
				case 18:
					return 3;
				case 19:
					return 2;
				default:
					return 1;
			}
		}
	}
}
