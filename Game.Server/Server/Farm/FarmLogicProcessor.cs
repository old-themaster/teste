using Game.Base.Packets;
using Game.Server.Farm.Handle;
using Game.Server.Packets;
using log4net;
using System;
using System.Reflection;

namespace Game.Server.Farm
{
    [FarmProcessorAtribute(99, "礼堂逻辑")]
	public class FarmLogicProcessor : AbstractFarmProcessor
	{
		private readonly static ILog ilog_0;

		private FarmHandleMgr farmHandleMgr_0;

		static FarmLogicProcessor()
		{
			
			FarmLogicProcessor.ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}

		public FarmLogicProcessor()
		{
			
			
			this.farmHandleMgr_0 = new FarmHandleMgr();
		}

		public override void OnGameData(GamePlayer player, GSPacketIn packet)
		{
			FarmPackageType farmPackageType = (FarmPackageType)packet.ReadByte();
			try
			{
				IFarmCommandHadler gInterface16 = this.farmHandleMgr_0.LoadCommandHandler((int)farmPackageType);
				if (gInterface16 == null)
				{
					Console.WriteLine("______________ERROR______________");
					Console.WriteLine("LoadCommandHandler not found!");
					Console.WriteLine("_______________END_______________");
				}
				else
				{
					gInterface16.CommandHandler(player, packet);
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				FarmLogicProcessor.ilog_0.Error(string.Format("IP:{1}, OnGameData is Error: {0}", exception.ToString(), player.Client.TcpEndpoint));
			}
		}
	}
}