using System;
using System.Collections.Generic;
using Game.Base.Packets;
using Game.Logic;
using SqlDataProvider.Data;

namespace Game.Server.Pet.Handle
{
    [global::Pet(5)]
    public class RefereshPet : IPetCommandHadler
    {
        public bool CommandHandler(GamePlayer player, GSPacketIn packet)
        {
			int num2 = (player.Level > 60) ? 60 : player.Level;
			bool flag2 = packet.ReadBoolean();
			int num10 = Convert.ToInt32(PetMgr.FindConfig("AdoptRefereshCost").Value);
			int templateId = Convert.ToInt32(PetMgr.FindConfig("FreeRefereshID").Value);
			ItemInfo itemByTemplateID = player.PropBag.GetItemByTemplateID(0, templateId);
			if (flag2)
			{
				bool flag3 = true;
				if (player.Extra.UseKingBless(2))
				{
					player.SendMessage(string.Format("Actualização Sucedida", num10));
					flag3 = false;
				}
				else
				{
					if (!player.MoneyDirect(num10))
					{
						return false;
					}
					if (itemByTemplateID != null)
					{
						player.PropBag.RemoveTemplate(templateId, 1);
						flag3 = false;
					}
				}
				if (flag3)
				{
					player.AddPetScore(num10 / 100);
				}
				List<UsersPetInfo> list = PetMgr.CreateAdoptList(player.PlayerCharacter.ID, num2);
				player.PetBag.ClearAdoptPets();
				foreach (UsersPetInfo current in list)
				{
					player.PetBag.AddAdoptPetTo(current, current.Place);
				}
			}
			//player.Out.SendRefreshPet(player, player.PetBag.GetAdoptPet(), null, flag2);
			return false;
		}
    }
}