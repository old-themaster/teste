using Bussiness;
using Game.Logic;
using Game.Base.Packets;
using Game.Server.Packets;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Pet.Handle
{
    [global::Pet(4)]
	public class FeedPet : IPetCommandHadler
	{
		public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
		{
			int place = packet.ReadInt();
			eBageType bagType = (eBageType)packet.ReadInt();
			int slot = packet.ReadInt();
			ItemInfo itemAt = Player.GetItemAt(bagType, place);
			if (itemAt == null)
			{
				Player.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("PetHandler.Msg9"));
				return false;
			}
			int num = Convert.ToInt32(PetMgr.FindConfig("MaxHunger").Value);
			UsersPetInfo petAt = Player.PetBag.GetPetAt(slot);
			int adet = itemAt.Count;
			int property = itemAt.Template.Property2;
			int property2 = itemAt.Template.Property1;
			int num3 = adet * property2;
			int num4 = num3 + petAt.Hunger;
			int num5 = adet * property;
			string text = "";
			if (itemAt.TemplateID == 334100)
			{
				num5 = itemAt.DefendCompose;
				if (petAt.breakGrade < itemAt.Hole1)
				{
					petAt.breakGrade = itemAt.Hole1;
				}
				if (petAt.breakBlood < itemAt.Hole6)
				{
					petAt.breakBlood = itemAt.Hole6;
				}
				if (petAt.breakAttack < itemAt.Hole2)
				{
					petAt.breakAttack = itemAt.Hole2;
				}
				if (petAt.breakDefence < itemAt.Hole3)
				{
					petAt.breakDefence = itemAt.Hole3;
				}
				if (petAt.breakAgility < itemAt.Hole4)
				{
					petAt.breakAgility = itemAt.Hole4;
				}
				if (petAt.breakLuck < itemAt.Hole5)
				{
					petAt.breakLuck = itemAt.Hole5;
				}
			}
			int SonSeviye = (Player.PetBag.MaxLevelByGrade > petAt.MaxLevel()) ? petAt.MaxLevel() : Player.PetBag.MaxLevelByGrade;
			if (petAt.Level < SonSeviye)
			{
				int num7 = num5 + petAt.GP;
				int level = petAt.Level;
				int level2 = PetMgr.GetLevel(num7, SonSeviye);
				int gP = PetMgr.GetGP(level2 + 1, SonSeviye);
				int gP2 = PetMgr.GetGP(SonSeviye, SonSeviye);
				int num8 = num7;
				if (num7 > gP2)
				{
					int num9 = num7 - gP2;
					if (num9 >= property && property != 0)
					{
						adet -= (int)Math.Ceiling((double)num9 / (double)property);
					}
				}
				petAt.GP = ((num8 >= gP2) ? gP2 : num8);
				petAt.Level = level2;
				petAt.MaxGP = ((gP == 0) ? gP2 : gP);
				petAt.Hunger = ((num4 > num) ? num : num4);
				int num10 = level2;
				if (level < num10)
				{
					Player.PetBag.UpdateEvolutionPet(petAt, level2,SonSeviye); // bu kısım evrimleştiriyor burdan geliyor 
					text = LanguageMgr.GetTranslation("FeedPet.Success", petAt.Name, level2);
					Player.EquipBag.UpdatePlayerProperties();
				}
				if (itemAt.TemplateID == 334100)
				{
					Player.StoreBag.RemoveItem(itemAt);
				}
				else
				{
					Player.StoreBag.RemoveCountFromStack(itemAt, adet);
					Player.OnUsingItem(itemAt.TemplateID, 1);
				}
				Player.PetBag.UpdatePet(petAt);
				Player.PetBag.SaveToDatabase(saveAdopt: false);
			}
			else if (petAt.Hunger < num)
			{
				petAt.Hunger = num4;
				Player.StoreBag.RemoveCountFromStack(itemAt, adet);
				text = LanguageMgr.GetTranslation("PetHandler.Msg10", num3);
				Player.PetBag.UpdatePet(petAt);
				Player.PetBag.SaveToDatabase(saveAdopt: false);
			}
			else
			{
				text = LanguageMgr.GetTranslation("PetHandler.Msg11");
			}
			if (!string.IsNullOrEmpty(text))
			{
				Player.SendMessage(text);
			}
			return false;
		}
	}
}
