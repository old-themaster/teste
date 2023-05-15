using Bussiness;
using Game.Base.Packets;

namespace Game.Server.Farm.Handle
{
    [Farm5(4)]
	public class GainFields : IFarmCommandHadler
	{
		public GainFields()
		{
			
			
		}

		public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
		{
			int num = packet.ReadInt();
			int num1 = packet.ReadInt();
			string translation = LanguageMgr.GetTranslation("EnterFarmHandler.Msg2");
			if (num == Player.PlayerCharacter.ID && Player.Farm.GainField(num1))
			{
				translation = LanguageMgr.GetTranslation("EnterFarmHandler.Msg3");
			}
			else if (num != Player.PlayerCharacter.ID)
			{
				translation = (!Player.Farm.GainFriendFields(num, num1) ? LanguageMgr.GetTranslation("EnterFarmHandler.Msg5") : LanguageMgr.GetTranslation("EnterFarmHandler.Msg4"));
			}
			Player.SendMessage(translation);
			return true;
		}
	}
}