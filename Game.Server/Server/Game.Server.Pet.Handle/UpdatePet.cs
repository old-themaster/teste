using Bussiness;
using Game.Base.Packets;
using Game.Server.Managers;
using SqlDataProvider.Data;

namespace Game.Server.Pet.Handle
{
    [global::Pet(1)]
	public class UpdatePet : IPetCommandHadler
	{
		public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
		{
			int num = packet.ReadInt();
			GamePlayer playerById = WorldMgr.GetPlayerById(num);
			UsersPetInfo[] array;
			if (playerById != null)
			{
				array = playerById.PetBag.GetPets();
			}
			else
			{
				using (PlayerBussiness playerBussiness = new PlayerBussiness())
				{
					array = playerBussiness.GetUserPetSingles(num);
					for (int i = 0; i < array.Length; i++)
					{
						array[i].PetEquips = Player.PetBag.DeserializePetEquip(array[i].eQPets);
					}
				}
			}
			if (array != null)
			{
				Player.Out.SendPetInfo(num, Player.ZoneId, array);
			}
			return false;
		}
	}
}
