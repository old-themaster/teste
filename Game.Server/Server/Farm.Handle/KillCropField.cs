using Game.Base.Packets;

namespace Game.Server.Farm.Handle
{
    [Farm5(7)]
	public class KillCropField : IFarmCommandHadler
	{
		public KillCropField()
		{
			
			
		}

		public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
		{
			int num = packet.ReadInt();
			Player.Farm.killCropField(num);
			return true;
		}
	}
}