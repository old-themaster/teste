using System.Collections.Generic;
using Game.Base.Packets;
using Game.Server.Packets;
using SqlDataProvider.Data;

namespace Game.Server.GypsyShop.Handle
{
    [GypsyShopHandleAttbute((byte) GypsyShopPackageType.RARE_ITEM_INFO)]
    public class GypsyShopRateItemInfo : IGypsyShopCommandHadler
    {
        public bool CommandHandler(GamePlayer player, GSPacketIn packet)
        {
            List<MysteryShopInfo> list = GypsyShopMgr.GetRateMysteryShop();
            GSPacketIn pkg = new GSPacketIn((short) ePackageType.MYSTERY_SHOP, player.PlayerCharacter.ID);
            pkg.WriteByte((byte) GypsyShopPackageType.RARE_ITEM_INFO);
            pkg.WriteInt(list.Count); //_itemCount = _loc_2.readInt();
            foreach (MysteryShopInfo info in list)
            {
                pkg.WriteInt(info.InfoID); //_loc_5 = _loc_2.readInt();infoID          
            }

            player.Out.SendTCP(pkg);
            return false;
        }
    }
}