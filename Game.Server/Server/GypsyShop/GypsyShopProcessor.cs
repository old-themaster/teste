using Game.Base.Packets;

namespace Game.Server.GypsyShop
{
    public class GypsyShopProcessor
    {
        private static object _syncStop = new object();
        private IGypsyShopProcessor _processor;

        public GypsyShopProcessor(IGypsyShopProcessor processor)
        {
            _processor = processor;
        }

        public void ProcessData(GamePlayer player, GSPacketIn data)
        {
            lock (_syncStop)
            {
                _processor.OnGameData(player, data);
            }
        }
    }
}