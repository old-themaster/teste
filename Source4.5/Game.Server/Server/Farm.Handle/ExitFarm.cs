using Game.Base.Packets;

namespace Game.Server.Farm.Handle
{
    [Farm5(16)]
	public class ExitFarm : IFarmCommandHadler
	{
		public ExitFarm()
		{
			
			
		}

		public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
		{
			Player.Farm.ExitFarm();
			return true;
		}
	}
}