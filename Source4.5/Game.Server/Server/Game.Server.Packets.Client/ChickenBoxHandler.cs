// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.ChickenBoxHandler
// Assembly: Game.Server, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 95F406BD-7233-42D4-AF91-73FA12644876
// Assembly location: C:\arquivos 4.1\5.9\SERVIDOR\Emulador\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.GameUtils;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Packets.Client
{
    [PacketHandler(87, "客户端日记")]
    public class ChickenBoxHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            int num1 = packet.ReadInt();
            GSPacketIn gsPacketIn = new GSPacketIn((short)87);
            ActiveSystemInfo info = client.Player.Actives.Info;
            switch (num1)
            {
                case 10:
                    client.Player.Actives.EnterChickenBox();
                    client.Player.Actives.SendChickenBoxItemList();
                    goto case 32;
                case 11:
                    int pos1 = packet.ReadInt();
                    int num2 = info.canEagleEyeCounts;
                    if (num2 > 0)
                    {
                        NewChickenBoxItemInfo box = client.Player.Actives.ViewAward(pos1);
                        if (box != null)
                        {
                            if (num2 > client.Player.Actives.eagleEyePrice.Length)
                                num2 = client.Player.Actives.eagleEyePrice.Length;
                            int num3 = client.Player.Actives.eagleEyePrice[num2 - 1];
                            if (client.Player.MoneyDirect(num3))
                            {
                                box.IsSeeded = true;
                                gsPacketIn.WriteInt(7);
                                gsPacketIn.WriteInt(box.TemplateID);
                                gsPacketIn.WriteInt(box.StrengthenLevel);
                                gsPacketIn.WriteInt(box.Count);
                                gsPacketIn.WriteInt(box.ValidDate);
                                gsPacketIn.WriteInt(box.AttackCompose);
                                gsPacketIn.WriteInt(box.DefendCompose);
                                gsPacketIn.WriteInt(box.AgilityCompose);
                                gsPacketIn.WriteInt(box.LuckCompose);
                                gsPacketIn.WriteInt(box.Position);
                                gsPacketIn.WriteBoolean(box.IsSelected);
                                gsPacketIn.WriteBoolean(box.IsSeeded);
                                gsPacketIn.WriteBoolean(box.IsBinds);
                                gsPacketIn.WriteInt(client.Player.Actives.freeEyeCount);
                                client.Player.SendTCP(gsPacketIn);
                                client.Player.Actives.UpdateChickenBoxAward(box);
                                --info.canEagleEyeCounts;
                                goto case 32;
                            }
                            else
                                goto case 32;
                        }
                        else
                        {
                            client.Player.SendMessage("Erro de servidor de dados.");
                            goto case 32;
                        }
                    }
                    else
                    {
                        client.Player.SendMessage("Este número de penetração do anel terminou.");
                        goto case 32;
                    }
                case 12:
                    client.Player.Actives.SendChickenBoxItemList();
                    client.Player.Actives.PayFlushView();
                    goto case 32;
                case 13:
                    int pos2 = packet.ReadInt();
                    if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
                    {
                        client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("Bag.Locked"));
                        return 1;
                    }
                    int num4 = info.canOpenCounts;
                    if (num4 > 0)
                    {
                        NewChickenBoxItemInfo award = client.Player.Actives.GetAward(pos2);
                        if (award != null)
                        {
                            award.IsBinds = true;
                            award.IsSelected = true;
                            if (num4 > client.Player.Actives.openCardPrice.Length)
                                num4 = client.Player.Actives.openCardPrice.Length;
                            int num3 = client.Player.Actives.openCardPrice[num4 - 1];
                            if (client.Player.MoneyDirect(num3))
                            {
                                gsPacketIn.WriteInt(4);
                                gsPacketIn.WriteInt(award.TemplateID);
                                gsPacketIn.WriteInt(award.StrengthenLevel);
                                gsPacketIn.WriteInt(award.Count);
                                gsPacketIn.WriteInt(award.ValidDate);
                                gsPacketIn.WriteInt(award.AttackCompose);
                                gsPacketIn.WriteInt(award.DefendCompose);
                                gsPacketIn.WriteInt(award.AgilityCompose);
                                gsPacketIn.WriteInt(award.LuckCompose);
                                gsPacketIn.WriteInt(award.Position);
                                gsPacketIn.WriteBoolean(award.IsSelected);
                                gsPacketIn.WriteBoolean(award.IsSeeded);
                                gsPacketIn.WriteBoolean(award.IsBinds);
                                gsPacketIn.WriteInt(client.Player.Actives.freeOpenCardCount);
                                client.Out.SendTCP(gsPacketIn);
                                client.Player.Actives.UpdateChickenBoxAward(award);
                                SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(award.TemplateID), 1, 105);
                                fromTemplate.IsBinds = award.IsBinds;
                                fromTemplate.ValidDate = award.ValidDate;
                                client.Player.AddTemplate(fromTemplate, "carta do frango");
                                client.Player.SendMessage("você já recebeu " + fromTemplate.Template.Name + " x" + (object)award.Count);
                                --info.canOpenCounts;
                                if (info.canOpenCounts == 0)
                                {
                                    GSPacketIn pkg = new GSPacketIn((short)87);
                                    pkg.WriteInt(6);
                                    client.Player.SendTCP(pkg);
                                }
                                goto case 32;
                            }
                            else
                                goto case 32;
                        }
                        else
                        {
                            client.Player.SendMessage("Dados do servidor de erro.");
                            goto case 32;
                        }
                    }
                    else
                    {
                        client.Player.SendMessage("Esta rodada de virada de cartão acabou.");
                        goto case 32;
                    }
                case 14:
                    int flushPrice = client.Player.Actives.flushPrice;
                    if (client.Player.Actives.IsFreeFlushTime())
                    {
                        if (client.Player.MoneyDirect(flushPrice))
                        {
                            client.Player.Actives.PayFlushView();
                            client.Player.Actives.SendChickenBoxItemList();
                            client.Player.SendMessage("Consuma moedas, crie um novo sucesso.");
                            goto case 32;
                        }
                        else
                            goto case 32;
                    }
                    else
                    {
                        client.Player.Actives.PayFlushView();
                        client.Player.Actives.SendChickenBoxItemList();
                        client.Player.SendMessage("Nova criação gratuita com sucesso.");
                        goto case 32;
                    }
                case 15:
                    info.isShowAll = false;
                    client.Player.Actives.RandomPosition();
                    gsPacketIn.WriteInt(5);
                    gsPacketIn.WriteBoolean(true);
                    client.Player.SendTCP(gsPacketIn);
                    goto case 32;
                case 31:
                    client.Player.Actives.CreateLuckyStartAward();
                    client.Player.Actives.SendLuckStarAllGoodsInfo();
                    client.Player.Actives.SendLuckStarRewardRank();
                    client.Player.Actives.SendLuckStarRewardRecord();
                    goto case 32;
                case 32:
                    return 0;
                case 33:
                    int templateId = 201192;
                    ItemTemplateInfo itemTemplate1 = ItemMgr.FindItemTemplate(templateId);
                    if (itemTemplate1 != null)
                    {
                        PlayerInventory inventory = client.Player.GetInventory(itemTemplate1.BagType);
                        SqlDataProvider.Data.ItemInfo itemByTemplateId = inventory.GetItemByTemplateID(0, templateId);
                        if (itemByTemplateId != null && itemByTemplateId.Count > 0)

                        {
                            client.Player.LastDrillUpTime = DateTime.Now;
                            inventory.RemoveTemplate(templateId, 1);
                            client.Player.Actives.ChangeLuckyStartAwardPlace();
                            client.Player.Actives.SendLuckStarTurnGoodsInfo();
                        }
                        else
                            client.Player.SendMessage(string.Format("{0} insuficiente.", (object)itemTemplate1.Name));
                        goto case 32;
                    }
                    else
                        goto case 32;
                case 34:
                    client.Player.Actives.SendUpdateReward();
                    NewChickenBoxItemInfo award1 = client.Player.Actives.Award;
                    ItemTemplateInfo itemTemplate2 = ItemMgr.FindItemTemplate(award1.TemplateID);
                    if (itemTemplate2 != null && itemTemplate2.CategoryID != client.Player.Actives.coinTemplateID)
                    {
                        SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(itemTemplate2, award1.Count, 105);
                        client.Player.AddTemplate(fromTemplate, "Estrela da sorte");
                        goto case 32;
                    }
                    else
                        goto case 32;
                default:
                    Console.WriteLine("NewChickenBoxPackageType." + (object)(NewChickenBoxPackageType)num1);
                    goto case 32;
            }
        }
    }
}
