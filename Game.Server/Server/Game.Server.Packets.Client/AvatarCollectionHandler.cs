using Game.Base.Packets;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;
namespace Game.Server.Packets.Client
{
    [PacketHandler(402, "Çizim Yeri")]
    public class AvatarCollectionHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            byte b = packet.ReadByte();
            switch (b)
            {
                case 3:
                    {
                        int num = packet.ReadInt();
                        int num2 = packet.ReadInt();
                        int num3 = packet.ReadInt();
                        try
                        {
                            if (client.Player.EquipBag.GetItemByTemplateID(0, num2) != null)
                            {
                                ClothGroupTemplateInfo clothGroup = ClothGroupTemplateInfoMgr.GetClothGroup(num, num2, num3);
                                if (clothGroup != null)
                                {
                                    ClothPropertyTemplateInfo clothPropertyWithID = ClothPropertyTemplateInfoMgr.GetClothPropertyWithID(clothGroup.ID);
                                    if (clothPropertyWithID != null)
                                    {
                                        if (client.Player.PlayerCharacter.Gold >= clothGroup.Cost)
                                        {
                                            client.Player.RemoveGold(clothGroup.Cost);
                                            bool flag = false;
                                            bool flag2 = false;
                                            UserAvatarCollectionInfo userAvatarCollectionInfo = client.Player.AvatarCollect.GetAvatarCollectWithAvatarID(clothGroup.ID);
                                            if (userAvatarCollectionInfo == null)
                                            {
                                                userAvatarCollectionInfo = new UserAvatarCollectionInfo(client.Player.PlayerCharacter.ID, clothPropertyWithID.ID, clothPropertyWithID.Sex, false, DateTime.Now);
                                                client.Player.AvatarCollect.AddAvatarCollection(userAvatarCollectionInfo);
                                                flag = true;
                                            }
                                            UserAvatarCollectionDataInfo item = new UserAvatarCollectionDataInfo(clothGroup.TemplateID, clothGroup.Sex);
                                            bool flag3 = userAvatarCollectionInfo.AddItem(item);
                                            if (flag3)
                                            {
                                                int num4 = ClothGroupTemplateInfoMgr.CountClothGroupWithID(userAvatarCollectionInfo.AvatarID);
                                                if (userAvatarCollectionInfo.Items.Count == num4 / 2 && !userAvatarCollectionInfo.IsActive)
                                                {
                                                    userAvatarCollectionInfo.ActiveAvatar(10);
                                                    flag = true;
                                                }
                                                if (userAvatarCollectionInfo.Items.Count == num4 / 2 || userAvatarCollectionInfo.Items.Count == num4)
                                                {
                                                    flag2 = true;
                                                }
                                                GSPacketIn gSPacketIn = new GSPacketIn(402);
                                                gSPacketIn.WriteByte(3);
                                                gSPacketIn.WriteInt(num);
                                                gSPacketIn.WriteInt(num2);
                                                gSPacketIn.WriteInt(num3);
                                                client.Player.SendTCP(gSPacketIn);
                                                if (flag)
                                                {
                                                    client.Player.Out.SendAvatarCollect(client.Player.AvatarCollect);
                                                }
                                                if (flag2)
                                                {
                                                    client.Player.EquipBag.UpdatePlayerProperties();
                                                }
                                                ItemInfo IT = client.Player.EquipBag.GetItemByTemplateID(0, num2);
                                                IT.IsBinds = true;
                                                IT.IsUsed = true;
                                                client.Player.UpdateItem(IT);
                                                client.Player.SendMessage("Aktivasyon başarılı!");
                                            }
                                            else
                                            {
                                                client.Player.AddGold(clothGroup.Cost);
                                                client.Player.SendMessage("Çizim Aktive Edilirken Bir Hata Oluştu.");
                                            }
                                        }
                                        else
                                        {
                                            client.Player.SendMessage("Yeterli Altınınız Yok.");
                                        }
                                    }
                                    else
                                    {
                                        client.Player.SendMessage("Bu Öge Sistemde Bulunmuyor.");
                                    }
                                }
                                else
                                {
                                    client.Player.SendMessage("Bu Çizim Mevcut Değil.");
                                }
                            }
                            else
                            {
                                client.Player.SendMessage("Bu Eşyaya Sahip Değilsin.");
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;
                    }
                case 4:
                    {
                        int num5 = packet.ReadInt();
                        int num6 = packet.ReadInt();
                        if (num6 <= 0)
                        {
                            return 0;
                        }
                        UserAvatarCollectionInfo avatarCollectWithAvatarID = client.Player.AvatarCollect.GetAvatarCollectWithAvatarID(num5);
                        if (avatarCollectWithAvatarID == null)
                        {
                            client.Player.SendMessage("Çizim Aktive Etme Başarısız.");
                        }
                        else
                        {
                            if (avatarCollectWithAvatarID.Items == null)
                            {
                                avatarCollectWithAvatarID.UpdateItems();
                            }
                            int num7 = ClothGroupTemplateInfoMgr.CountClothGroupWithID(avatarCollectWithAvatarID.AvatarID);
                            if (avatarCollectWithAvatarID.Items.Count >= num7 / 2)
                            {
                                ClothPropertyTemplateInfo clothPropertyWithID2 = ClothPropertyTemplateInfoMgr.GetClothPropertyWithID(num5);
                                if (clothPropertyWithID2 == null)
                                {
                                    client.Player.SendMessage("Bu Çizim Aktive Edilemez.");
                                }
                                else
                                {
                                    int num8 = clothPropertyWithID2.Cost * num6;
                                    if (client.Player.PlayerCharacter.myHonor < clothPropertyWithID2.Cost || num8 <= 0)
                                    {
                                        client.Player.SendMessage("Onur Özü Yetersiz.");
                                    }
                                    else
                                    {
                                        client.Player.RemovemyHonor(num8);
                                        avatarCollectWithAvatarID.ActiveAvatar(num6);
                                        client.Player.Out.SendAvatarCollect(client.Player.AvatarCollect);
                                        client.Player.SendMessage("Başarıyla Süresi Uzatıldı.");
                                    }
                                }
                            }
                            else
                            {
                                client.Player.SendMessage("Yenilemek için Çizim'in yarısından fazlasını etkinleştirmelisiniz.");
                            }
                        }
                        break;
                    }
                default:
                    Console.WriteLine("Avatar System: " + b);
                    break;
            }
            return 1;
        }
    }
}
