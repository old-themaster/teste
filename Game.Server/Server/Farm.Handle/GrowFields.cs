using Game.Base.Packets;

namespace Game.Server.Farm.Handle
{
    [Farm5(2)]
	public class GrowFields : IFarmCommandHadler
	{
		public GrowFields()
		{
			
			
		}

		public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
		{
			packet.ReadByte();
			int num = packet.ReadInt();
			int num1 = packet.ReadInt();
			if (Player.Farm.GrowField(num1, num))
			{
				Player.FarmBag.RemoveTemplate(num, 1);
				Player.OnSeedFoodPetEvent();
			}
			return true;
		}
	}
}