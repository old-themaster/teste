using Bussiness;
using Game.Base.Packets;
using Game.Server.Packets;

namespace Game.Server.GypsyShop.Handle
{
    [GypsyShopHandleAttbute((byte) GypsyShopPackageType.REFRESH)]
    public class GypsyShopRefresh : IGypsyShopCommandHadler
    {
        public bool CommandHandler(GamePlayer player, GSPacketIn packet)
        {
            int curRefreshedTimes = player.Actives.Info.CurRefreshedTimes;
            int price = curRefreshedTimes * curRefreshedTimes * 30 + 500;
            if (player.PlayerCharacter.myHonor >= price)
            {
                player.Actives.RefreshMysteryShop();
                player.Actives.Info.CurRefreshedTimes++;
                player.Actives.SendGypsyShopPlayerInfo();
                player.RemoveHonor(price);
                player.SendMessage(LanguageMgr.GetTranslation("GypsyShopRefresh.Success"));
            }
            else
            {
                player.SendMessage(LanguageMgr.GetTranslation("GypsyShopRefresh.Fail", price));
            }

            return false;
        }
    }
}