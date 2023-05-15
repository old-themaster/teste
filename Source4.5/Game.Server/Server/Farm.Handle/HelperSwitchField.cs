using Bussiness;
using Game.Base.Packets;

namespace Game.Server.Farm.Handle
{
    [Farm5(9)]
	public class HelperSwitchField : IFarmCommandHadler
	{
		public HelperSwitchField()
		{
			
			
		}

		public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
		{
			string translation = LanguageMgr.GetTranslation("EnterFarmHandler.Msg6");
			bool flag = packet.ReadBoolean();
			int num = packet.ReadInt();
			int num1 = packet.ReadInt();
			int num2 = packet.ReadInt();
			int num3 = packet.ReadInt();
			int num4 = packet.ReadInt();
			int num5 = packet.ReadInt();
			bool flag1 = false;
			if (!flag)
			{
				translation = LanguageMgr.GetTranslation("EnterFarmHandler.Msg9");
				Player.Farm.CropHelperSwitchField(true);
			}
			else if (Player.MoneyDirect(num5) && num4 == -1)
			{
				flag1 = true;
			}
			else if (Player.PlayerCharacter.GiftToken < num5 || num4 != -2)
			{
				translation = (num4 != -1 ? LanguageMgr.GetTranslation("EnterFarmHandler.Msg8") : LanguageMgr.GetTranslation("EnterFarmHandler.Msg7"));
			}
			else
			{
				Player.RemoveGiftToken(num5);
				flag1 = true;
			}
			if (flag1)
			{
				translation = LanguageMgr.GetTranslation("EnterFarmHandler.Msg10");
				Player.Farm.HelperSwitchField(flag, num, num1, num2, num3);
				Player.FarmBag.RemoveTemplate(num, num2);
			}
			Player.SendMessage(translation);
			return true;
		}
	}
}