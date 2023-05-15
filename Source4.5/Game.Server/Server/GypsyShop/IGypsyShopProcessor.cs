using Game.Base.Packets;

namespace Game.Server.GypsyShop
{
    public interface IGypsyShopProcessor
    {
        void OnGameData(GamePlayer player, GSPacketIn packet);
    }
}