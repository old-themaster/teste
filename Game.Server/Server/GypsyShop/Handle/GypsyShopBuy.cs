using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.Packets;
using SqlDataProvider.Data;

namespace Game.Server.GypsyShop.Handle
{
    [GypsyShopHandleAttbute((byte) GypsyShopPackageType.BUY)]
    public class GypsyShopBuy : IGypsyShopCommandHadler
    {
        public bool CommandHandler(GamePlayer player, GSPacketIn packet)
        {
            int id = packet.ReadInt();
            bool isBind = packet.ReadBoolean();
            GypsyItemDataInfo gypsy = player.Actives.GetMysteryShopByID(id);
            if (gypsy != null)
            {
                if (gypsy.CanBuy == 1)
                {
                    bool canBuy = false;
                    if (gypsy.Unit == 1 && player.MoneyDirect(gypsy.Price))
                    {
                        canBuy = true;
                    }
                    else if (gypsy.Unit == 2 && player.PlayerCharacter.myHonor >= gypsy.Price)
                    {
                        canBuy = true;
                    }

                    if (canBuy)
                    {
                        ItemTemplateInfo info = ItemMgr.FindItemTemplate(gypsy.InfoID);
                        if (info != null)
                        {
                            ItemInfo item = ItemInfo.CreateFromTemplate(info, 1, 105);
                            item.IsBinds = true;
                            item.ValidDate = gypsy.Num;
                            player.AddTemplate(item);
                        }

                        player.SendMessage(LanguageMgr.GetTranslation("GypsyShopBuy.Success"));
                        player.Actives.UpdateMysteryShopByID(id);
                        if (gypsy.Unit == 2)
                        {
                            player.RemoveHonor(gypsy.Price);
                        }
                    }

                    if (gypsy.Unit == 2 && !canBuy)
                    {
                        player.SendMessage(LanguageMgr.GetTranslation("GypsyShopBuy.OutHornor"));
                    }

                    GSPacketIn pkg = new GSPacketIn((short) ePackageType.MYSTERY_SHOP, player.PlayerCharacter.ID);
                    pkg.WriteByte((byte) GypsyShopPackageType.BUY);
                    pkg.WriteInt(gypsy.GypsyID);
                    pkg.WriteBoolean(canBuy);
                    if (canBuy)
                    {
                        pkg.WriteInt(gypsy.CanBuy);
                    }

                    player.Out.SendTCP(pkg);
                }
                else
                {
                    player.SendMessage(LanguageMgr.GetTranslation("GypsyShopBuy.OutOfDate"));
                }
            }
            else
            {
                player.SendMessage(LanguageMgr.GetTranslation("GypsyShopBuy.Fail"));
            }

            return false;
        }
    }
}