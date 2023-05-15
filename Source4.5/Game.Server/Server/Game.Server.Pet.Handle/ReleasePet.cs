using Bussiness;
using Game.Base.Packets;
using SqlDataProvider.Data;

namespace Game.Server.Pet.Handle
{
    [global::Pet(8)]
	public class ReleasePet : IPetCommandHadler
	{
		public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
		{
			int slot = packet.ReadInt();
			UsersPetInfo petAt = Player.PetBag.GetPetAt(slot);
			if (Player.PetBag.RemovePet(petAt))
			{
				int capalility = Player.PetBag.Capalility;
				UsersPetInfo[] pets = Player.PetBag.GetPets();
				Player.PetBag.BeginChanges();
				try
				{
					if (Player.PetBag.FindFirstEmptySlot(Player.PetBag.BeginSlot) != -1)
					{
						for (int i = 1; Player.PetBag.FindFirstEmptySlot(Player.PetBag.BeginSlot) < pets[pets.Length - i].Place; i++)
						{
							Player.PetBag.MovePet(pets[pets.Length - i].Place, Player.PetBag.FindFirstEmptySlot(Player.PetBag.BeginSlot));
						}
					}
				}
				catch
				{
				}
				finally
				{
					Player.PetBag.CommitChanges();
				}
			}
			Player.SendMessage(LanguageMgr.GetTranslation("PetHandler.Msg19"));
			Player.PetBag.SaveToDatabase(saveAdopt: false);
			return false;
		}
	}
}
