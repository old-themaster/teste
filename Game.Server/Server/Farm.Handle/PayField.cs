using Bussiness;
using Game.Base.Packets;
using Game.Server.GameUtils;
using System.Collections.Generic;

namespace Game.Server.Farm.Handle
{
    [Farm5(6)]
	public class PayField : IFarmCommandHadler
	{
		public PayField()
		{
			
			
		}

		public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
		{
			string translation = LanguageMgr.GetTranslation("EnterFarmHandler.Msg11");
			List<int> nums = new List<int>();
			int num = packet.ReadInt();
			for (int i = 0; i < num; i++)
			{
				nums.Add(packet.ReadInt());
			}
			int num1 = packet.ReadInt();
			int num2 = 0;
			PlayerFarm farm = Player.Farm;
			num2 = (farm.payFieldTimeToMonth() != num1 ? num * farm.payFieldMoneyToWeek() : num * farm.payFieldMoneyToMonth());
			if (Player.MoneyDirect(num2))
			{
				farm.PayField(nums, num1);
				Player.SendMessage(translation);
			}
			return true;
		}
	}
}