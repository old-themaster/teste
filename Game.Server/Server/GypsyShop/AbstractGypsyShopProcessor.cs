using Game.Base.Packets;

namespace Game.Server.GypsyShop
{
    public abstract class AbstractGypsyShopProcessor : IGypsyShopProcessor
    {
        public virtual void OnGameData(GamePlayer player, GSPacketIn packet)
        {
        }
    }
}