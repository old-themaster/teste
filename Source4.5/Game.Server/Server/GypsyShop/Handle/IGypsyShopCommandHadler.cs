using Game.Base.Packets;

namespace Game.Server.GypsyShop.Handle
{
    public interface IGypsyShopCommandHadler
    {
        bool CommandHandler(GamePlayer player, GSPacketIn packet);
    }
}