using Bussiness;
using Game.Base.Packets;
using Game.Logic;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Farm.Handle
{
    [Farm5(1)]
	public class EnterFarm : IFarmCommandHadler
	{
		public EnterFarm()
		{
			
			
		}

		public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
		{
			if (Player.PlayerCharacter.Grade < 25)
			{
				Player.SendMessage(LanguageMgr.GetTranslation("EnterFarmHandler.Msg1"));
				return false;
			}
			int num = packet.ReadInt();
			if (num != Player.PlayerCharacter.ID)
			{
				Player.Farm.EnterFriendFarm(num);
			}
			else
			{
				Player.Farm.EnterFarm();
				if (GameProperties.VERSION < 41)
				{
					if (Player.PlayerCharacter.IsFistGetPet)
					{
						Player.PetBag.ClearAdoptPets();
						foreach (UsersPetInfo usersPetInfo in PetMgr.CreateFirstAdoptList(num, Player.Level, Player.PlayerCharacter.VIPLevel))
						{
							Player.PetBag.AddAdoptPetTo(usersPetInfo, usersPetInfo.Place);
						}
						Player.RemoveFistGetPet();
					}
					else if (Player.PlayerCharacter.LastRefreshPet.Date < DateTime.Now.Date)
					{
						Player.PetBag.ClearAdoptPets();
						foreach (UsersPetInfo usersPetInfo1 in PetMgr.CreateAdoptList(num, Player.Level, Player.PlayerCharacter.VIPLevel))
						{
							Player.PetBag.AddAdoptPetTo(usersPetInfo1, usersPetInfo1.Place);
						}
						Player.RemoveLastRefreshPet();
					}
				}
			}
			return true;
		}
	}
}