using System.Collections.Generic;
using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.Managers;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
	[PacketHandler(107, "inviteFriendHandler")]
	public class inviteFriendHandler : IPacketHandler
	{
		public int HandlePacket(GameClient client, GSPacketIn packet)
		{
			switch (packet.ReadInt())
			{
				case 1:
					{
						string text = packet.ReadString();
						if (client.Player.PlayerCharacter.MyInviteCode == text)
						{
							client.Player.SendMessage("Você não pode usar seu próprio código.");
							return 0;
						}
						PlayerBussiness playerBussiness = new PlayerBussiness();
						PlayerInfo userSingleByUserInviteCode = playerBussiness.GetUserSingleByUserInviteCode(text);
						if (userSingleByUserInviteCode != null)
						{
							if (!userSingleByUserInviteCode.CodeStatus)
							{
								client.Player.PlayerCharacter.MyRewardStatus = true;
								userSingleByUserInviteCode.MyInvitedCount++;
								userSingleByUserInviteCode.CodeStatus = true;
								List<ItemInfo> list9 = new List<ItemInfo>();
								List<DailyAwardInfo> list10 = AwardMgr.FindReward(200);
								foreach (DailyAwardInfo item in list10)
								{
									ItemInfo itemInfo5 = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(item.TemplateID), 1, 102);
									if (itemInfo5 != null)
									{
										itemInfo5.IsBinds = item.IsBinds;
										itemInfo5.Count = item.Count;
										itemInfo5.ValidDate = item.ValidDate;
										list9.Add(itemInfo5);
									}
								}
								if (!client.Player.SendItemsToMail(list9, "Evento de Código", "Evento de Código", eMailType.Manage))
								{
									return 0;
								}
								client.Player.Out.SendMailResponse(client.Player.PlayerCharacter.ID, eMailRespose.Receiver);
								client.Player.SendMessage(LanguageMgr.GetTranslation("As recompensas foram enviadas para sua caixa de e-mail. ✔\ufe0f"));
								client.Player.Out.SendCreateInviteFriends(client.Player, 3);
								playerBussiness.UpdatePlayer(client.Player.PlayerCharacter);
								playerBussiness.UpdatePlayer(userSingleByUserInviteCode);
								WorldMgr.SendSysNotice(string.Concat("O jogador [" + client.Player.PlayerCharacter.NickName + "] coletou uma recompensa lendária no evento convite de amigos. faça como ele ganhe recompensas convidando jogadores para o jogo."));
								break;
							}
							client.Player.SendMessage("Este código já foi utilizado.");
							return 0;
						}
						client.Player.SendMessage("Nenhum código cadastrado foi encontrado no sistema.");
						return 0;
					}
				case 2:
					packet.ReadInt();
					client.Player.Out.SendCreateInviteFriends(client.Player, 2);
					break;
				case 4:
					switch (packet.ReadInt())
					{
						case 1:
							{
								if (client.Player.PlayerCharacter.MyColumn1 != 0)
								{
									break;
								}
								client.Player.PlayerCharacter.MyColumn1 = 1;
								List<ItemInfo> list3 = new List<ItemInfo>();
								List<DailyAwardInfo> list4 = AwardMgr.FindReward(201);
								foreach (DailyAwardInfo item2 in list4)
								{
									ItemInfo itemInfo2 = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(item2.TemplateID), 1, 102);
									if (itemInfo2 != null)
									{
										itemInfo2.IsBinds = item2.IsBinds;
										itemInfo2.Count = item2.Count;
										itemInfo2.ValidDate = item2.ValidDate;
										list3.Add(itemInfo2);
									}
								}
								if (!client.Player.SendItemsToMail(list3, "Evento de Código", "Evento de Código", eMailType.Manage))
								{
									return 0;
								}
								client.Player.Out.SendMailResponse(client.Player.PlayerCharacter.ID, eMailRespose.Receiver);
								client.Player.SendMessage(LanguageMgr.GetTranslation("As recompensas foram enviadas para sua caixa de e-mail. ✔\ufe0f"));
								client.Player.Out.SendCreateInviteFriends(client.Player, 4);
								WorldMgr.SendSysNotice(string.Concat("O jogador [" + client.Player.PlayerCharacter.NickName + "] coletou uma recompensa lendária no evento convite de amigos. faça como ele ganhe recompensas convidando jogadores para o jogo."));
								break;
							}
						case 3:
							{
								if (client.Player.PlayerCharacter.MyColumn2 != 0)
								{
									break;
								}
								client.Player.PlayerCharacter.MyColumn2 = 1;
								List<ItemInfo> list7 = new List<ItemInfo>();
								List<DailyAwardInfo> list8 = AwardMgr.FindReward(203);
								foreach (DailyAwardInfo item3 in list8)
								{
									ItemInfo itemInfo4 = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(item3.TemplateID), 1, 102);
									if (itemInfo4 != null)
									{
										itemInfo4.IsBinds = item3.IsBinds;
										itemInfo4.Count = item3.Count;
										itemInfo4.ValidDate = item3.ValidDate;
										list7.Add(itemInfo4);
									}
								}
								if (!client.Player.SendItemsToMail(list7, "Evento de Código", "Evento de Código", eMailType.Manage))
								{
									return 0;
								}
								client.Player.Out.SendMailResponse(client.Player.PlayerCharacter.ID, eMailRespose.Receiver);
								client.Player.SendMessage(LanguageMgr.GetTranslation("As recompensas foram enviadas para sua caixa de e-mail. ✔\ufe0f"));
								client.Player.Out.SendCreateInviteFriends(client.Player, 4);
								WorldMgr.SendSysNotice(string.Concat("O jogador [" + client.Player.PlayerCharacter.NickName + "] coletou uma recompensa lendária no evento convite de amigos. faça como ele ganhe recompensas convidando jogadores para o jogo."));
								break;
							}
						case 5:
							{
								if (client.Player.PlayerCharacter.MyColumn3 != 0)
								{
									break;
								}
								client.Player.PlayerCharacter.MyColumn3 = 1;
								List<ItemInfo> list5 = new List<ItemInfo>();
								List<DailyAwardInfo> list6 = AwardMgr.FindReward(205);
								foreach (DailyAwardInfo item4 in list6)
								{
									ItemInfo itemInfo3 = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(item4.TemplateID), 1, 102);
									if (itemInfo3 != null)
									{
										itemInfo3.IsBinds = item4.IsBinds;
										itemInfo3.Count = item4.Count;
										itemInfo3.ValidDate = item4.ValidDate;
										list5.Add(itemInfo3);
									}
								}
								if (!client.Player.SendItemsToMail(list5, "Evento de Código", "Evento de Código", eMailType.Manage))
								{
									return 0;
								}
								client.Player.Out.SendMailResponse(client.Player.PlayerCharacter.ID, eMailRespose.Receiver);
								client.Player.SendMessage(LanguageMgr.GetTranslation("As recompensas foram enviadas para sua caixa de e-mail. ✔\ufe0f"));
								client.Player.Out.SendCreateInviteFriends(client.Player, 4);
								WorldMgr.SendSysNotice(string.Concat("O jogador [" + client.Player.PlayerCharacter.NickName + "] coletou uma recompensa lendária no evento convite de amigos. faça como ele ganhe recompensas convidando jogadores para o jogo."));
								break;
							}
						case 10:
							{
								if (client.Player.PlayerCharacter.MyColumn4 != 0)
								{
									break;
								}
								client.Player.PlayerCharacter.MyColumn4 = 1;
								List<ItemInfo> list = new List<ItemInfo>();
								List<DailyAwardInfo> list2 = AwardMgr.FindReward(210);
								foreach (DailyAwardInfo item5 in list2)
								{
									ItemInfo itemInfo = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(item5.TemplateID), 1, 102);
									if (itemInfo != null)
									{
										itemInfo.IsBinds = item5.IsBinds;
										itemInfo.Count = item5.Count;
										itemInfo.ValidDate = item5.ValidDate;
										list.Add(itemInfo);
									}
								}
								if (!client.Player.SendItemsToMail(list, "Evento de Código", "Evento de Código", eMailType.Manage))
								{
									return 0;
								}
								client.Player.Out.SendMailResponse(client.Player.PlayerCharacter.ID, eMailRespose.Receiver);
								client.Player.SendMessage(LanguageMgr.GetTranslation("As recompensas foram enviadas para sua caixa de e-mail. ✔\ufe0f"));
								client.Player.Out.SendCreateInviteFriends(client.Player, 4);
								WorldMgr.SendSysNotice(string.Concat("O jogador [" + client.Player.PlayerCharacter.NickName + "] coletou uma recompensa lendária no evento convite de amigos. faça como ele ganhe recompensas convidando jogadores para o jogo."));
								break;
							}
					}
					break;
			}
			return 0;
		}
	}
}