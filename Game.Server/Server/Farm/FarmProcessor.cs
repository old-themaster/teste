using Game.Base.Packets;

namespace Game.Server.Farm
{
    public class FarmProcessor
	{
		private static object object_0;

		private IFarmProcessor ifarmProcessor_0;

		static FarmProcessor()
		{
			
			FarmProcessor.object_0 = new object();
		}

		public FarmProcessor(IFarmProcessor processor)
		{
			
			
			this.ifarmProcessor_0 = processor;
		}

		public void ProcessData(GamePlayer player, GSPacketIn data)
		{
			lock (FarmProcessor.object_0)
			{
				this.ifarmProcessor_0.OnGameData(player, data);
			}
		}
	}
}