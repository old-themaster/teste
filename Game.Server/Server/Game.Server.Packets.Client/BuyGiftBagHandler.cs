// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.BuyGiftBagHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
    [PacketHandler(46, "Pacote de Oferta")]
    public class BuyGiftBagHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            packet.ReadInt();
            int num = 99999999;
            packet.ReadBoolean();
            if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
            {
                client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Bag.Locked"));
                return 1;
            }
            if (client.Player.MoneyDirect(num, false))
            {
                ItemInfo fromTemplate1 = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(11020), 1, 104);
                ItemInfo fromTemplate2 = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(11020), 1, 104);
                ItemInfo fromTemplate3 = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(11020), 1, 104);
                ItemInfo fromTemplate4 = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(11020), 1, 104);
                ItemInfo fromTemplate5 = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(11020), 1, 104);
                if (!client.Player.StoreBag.AddItemTo(fromTemplate1, 0))
                    client.Player.AddTemplate(fromTemplate1);
                if (!client.Player.StoreBag.AddItemTo(fromTemplate2, 1))
                    client.Player.AddTemplate(fromTemplate2);
                if (!client.Player.StoreBag.AddItemTo(fromTemplate3, 2))
                    client.Player.AddTemplate(fromTemplate3);
                if (!client.Player.StoreBag.AddItemTo(fromTemplate4, 3))
                    client.Player.AddTemplate(fromTemplate4);
                if (!client.Player.StoreBag.AddItemTo(fromTemplate5, 4))
                    client.Player.AddTemplate(fromTemplate5);
                client.Player.RemoveMoney(num);
                client.Out.SendMessage(eMessageType.GM_NOTICE, "pacote de fortalecimento, garantido sua arma não regredir de lv, valor 9 mil cupons a melhor coisa para ser comprar no game Boa sorte!.");
            }
            return 0;
        }
    }
}
