using System;
using System.Reflection;
using Game.Base.Packets;
using Game.Server.GypsyShop.Handle;
using Game.Server.Packets;
using log4net;

namespace Game.Server.GypsyShop
{
    [GypsyShopProcessorAtribute(255, "礼堂逻辑")]
    public class GypsyShopLogicProcessor : AbstractGypsyShopProcessor
    {
        public GypsyShopLogicProcessor()
        {
            _commandMgr = new GypsyShopHandleMgr();
        }

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private GypsyShopHandleMgr _commandMgr;

        public override void OnGameData(GamePlayer player, GSPacketIn packet)
        {
            GypsyShopPackageType type = (GypsyShopPackageType) packet.ReadByte();
            try
            {
                IGypsyShopCommandHadler commandHandler = _commandMgr.LoadCommandHandler((int) type);
                if (commandHandler != null)
                {
                    commandHandler.CommandHandler(player, packet);
                }
                else
                {
                    //log.Error(string.Format("IP: {0}", player.Client.TcpEndpoint));
                    Console.WriteLine("______________ERROR______________");
                    Console.WriteLine("LoadCommandHandler not found!");
                    Console.WriteLine("_______________END_______________");
                }
            }
            catch (Exception e)
            {
                log.Error(string.Format("GypsyShopPackageType: {1}, OnGameData is Error: {0}", e.ToString(),
                    (GypsyShopPackageType) type));
            }
        }
    }
}