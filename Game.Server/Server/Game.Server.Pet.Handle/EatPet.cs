using System;
using Game.Base.Packets;
using Game.Logic;
using SqlDataProvider.Data;

namespace Game.Server.Pet.Handle
{
    [global::Pet(33)]
    public class EatPet : IPetCommandHadler
    {
        public bool CommandHandler(GamePlayer player, GSPacketIn packet)
        {
            int amor = packet.ReadInt();
            int type = packet.ReadInt();
            int count = 0;
            int totalPoint = 0;
            int eatItem = 201567;
            if (type == 1)
            {
                count = packet.ReadInt();
                for (int i = 0; i < count; i++)
                {
                    int petPlace = packet.ReadInt();
                    int templateID = packet.ReadInt();
                    UsersPetInfo pet = player.PetBag.GetPetAt(petPlace);
                    if (pet != null)
                    {
                        PetTemplateInfo info = PetMgr.FindPetTemplate(templateID);
                        if (info != null)
                        {
                            totalPoint += (int)(Math.Pow(10, info.StarLevel - 2) + 5 * Math.Max(pet.Level - 8, pet.Level * 0.2));
                        }
                    }

               //     UpGrade(player, amor, type, totalPoint, null);
                    player.PetBag.RemovePet(pet);
                }
            }
            else
            {
                count = packet.ReadInt();
                ItemInfo info = player.GetItemByTemplateID(eatItem);
                if (info != null)
                {
                    int allCount = player.GetItemCount(eatItem);
                    if (allCount < count)
                    {
                        count = allCount < count ? allCount : count;
                    }

                    totalPoint += info.Template.Property2 * count;
                  //  int useCount = UpGrade(player, amor, type, totalPoint, info);
                    if (count == 1)
                    {
                        player.RemoveTemplate(eatItem, count);
                    }
                    else
                    {
                   //     player.RemoveTemplate(eatItem, useCount);
                    }
                }
                else
                {
                    player.SendMessage(string.Format("Không đủ số lượng đá manh hóa."));
                }
            }

            player.Out.SendEatPetsInfo(player.PetBag.EatPets);
            player.EquipBag.UpdatePlayerProperties();
            return false;
        }

      /*  private int UpGrade(GamePlayer player, int amor, int type, int totalPoint, ItemInfo eatItem)
        {
            int totalExp = 0;
            int oldLv = 0;
          //  int maxLv = PetMoePropertyMgr.FindMaxLevel();
            GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
            PetMoePropertyInfo item = null;
            switch (amor)
            {
                case 0: //att, luc
                    totalExp = player.PetBag.EatPets.weaponExp + totalPoint;
                    oldLv = player.PetBag.EatPets.weaponLevel;
                   // for (int i = oldLv; i <= maxLv; i++)
                    {
                     //   item = PetMoePropertyMgr.FindPetMoeProperty(i + 1);
                        if (item != null && item.Exp <= totalExp)
                        {
                         //   player.PetBag.EatPets.weaponLevel = i + 1;
                            totalExp -= item.Exp;
                            for (int z = 0; z < allPlayers.Length; z++)
                            {
                                allPlayers[z].Out.SendMessage(eMessageType.ChatNormal, string.Format("|Manh Hóa| - Chúc mừng [{0}] vừa manh hóa thành công Vũ Khí lên cấp {1} lực chiến tăng lên tầm cao mới",player.PlayerCharacter.NickName, player.PetBag.EatPets.weaponLevel));
                            }
                        }
                    }

                  //  if (player.PetBag.EatPets.weaponLevel == maxLv)
                    {
                        totalPoint = totalExp > 0 ? totalExp : totalPoint;
                        player.PetBag.EatPets.weaponExp = 0;
                    }
                    else
                    {
                        player.PetBag.EatPets.weaponExp = totalExp;
                    }

                    break;
                case 1: //agi, hp
                    totalExp = player.PetBag.EatPets.clothesExp + totalPoint;
                    oldLv = player.PetBag.EatPets.clothesLevel;
                  //  for (int i = oldLv; i <= maxLv; i++)
                    {
                       // item = PetMoePropertyMgr.FindPetMoeProperty(i + 1);
                        if (item != null && item.Exp <= totalExp)
                        {
                            player.PetBag.EatPets.clothesLevel = i + 1;
                            totalExp -= item.Exp;
                            for (int z = 0; z < allPlayers.Length; z++)
                            {
                                allPlayers[z].Out.SendMessage(eMessageType.ChatNormal, string.Format("|Manh Hóa| - Chúc mừng [{0}] vừa manh hóa thành công Giáp lên cấp {1} lực chiến tăng lên tầm cao mới", player.PlayerCharacter.NickName, player.PetBag.EatPets.clothesLevel));
                            }
                        }
                    }

                  //  if (player.PetBag.EatPets.clothesLevel == maxLv)
                    {
                        totalPoint = totalExp > 0 ? totalExp : totalPoint;
                        player.PetBag.EatPets.clothesExp = 0;
                    }
                    else
                    {
                        player.PetBag.EatPets.clothesExp = totalExp;
                    }

                    break;
                case 2: //def, guard
                    totalExp = player.PetBag.EatPets.hatExp + totalPoint;
                    oldLv = player.PetBag.EatPets.hatLevel;
                //    for (int i = oldLv; i <= maxLv; i++)
                    {
                      //  item = PetMoePropertyMgr.FindPetMoeProperty(i + 1);
                        if (item != null && item.Exp <= totalExp)
                        {
                            player.PetBag.EatPets.hatLevel = i + 1;
                            totalExp -= item.Exp;
                            for (int z = 0; z < allPlayers.Length; z++)
                            {
                                allPlayers[z].Out.SendMessage(eMessageType.ChatNormal, string.Format("|Manh Hóa| - Chúc mừng [{0}] vừa manh hóa thành công Nón lên cấp {1} lực chiến tăng lên tầm cao mới", player.PlayerCharacter.NickName, player.PetBag.EatPets.hatLevel));
                            }
                        }
                    }
                    }

                   // if (player.PetBag.EatPets.hatLevel == maxLv)
                  //  {
                    //    totalPoint = totalExp > 0 ? totalExp : totalPoint;
                    //    player.PetBag.EatPets.hatExp = 0;
                  //  }
                    else
                    {
                        player.PetBag.EatPets.hatExp = totalExp;
                    }

                    break;
            }

            if (type == 2 && eatItem != null)
                return totalPoint / eatItem.Template.Property2;

            return 0;*/
        }
    }
