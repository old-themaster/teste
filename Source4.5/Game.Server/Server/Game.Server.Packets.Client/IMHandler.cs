using System;
using System.Collections.Generic;
using Bussiness;
using Game.Base.Packets;
using Game.Server.Managers;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{

    [PacketHandler(160, "添加好友")]
	public class IMHandler : IPacketHandler
	{
		private static List<ConnectionList> connection = new List<ConnectionList>();

		public int HandlePacket(GameClient client, GSPacketIn packet)
		{
			int count = 0;
			connection.Add(new ConnectionList
			{
				conntime = DateTime.Now
			});
			connection.ForEach(delegate (ConnectionList a)
			{
				int num5 = Convert.ToInt32((DateTime.Now - a.conntime).TotalSeconds);
				if (num5 <= 11)
				{
					count++;
				}
			});
			if (count >= 14)
			{
				try
				{
					Console.ResetColor();
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine("Cheat engine detectado IMHandler o jogador: [" + client.Player.PlayerCharacter.NickName + "] foi expulso.");
					Console.ResetColor();
					client.Disconnect();
					GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
					foreach (GamePlayer gamePlayer in allPlayers)
					{
						if (ComandosMgr.checkStaff(gamePlayer.PlayerCharacter.ID))
						{
							GSPacketIn pkg = WorldMgr.SendStaffHelp("O jogador: [" + client.Player.PlayerCharacter.NickName + "] foi expulso por usar HACK IMHandler");
							gamePlayer.SendTCP(pkg);
						}
					}
					return 0;
				}
				catch (Exception)
				{
					Console.WriteLine("Algo deu errado!");
				}
			}
			switch (packet.ReadByte())
			{
				case 51:
					{
						int num4 = packet.ReadInt();
						string msg = packet.ReadString();
						packet.ReadBoolean();
						GamePlayer playerById = WorldMgr.GetPlayerById(num4);
						if (playerById != null)
						{
							client.Player.Out.sendOneOnOneTalk(num4, isAutoReply: false, client.Player.PlayerCharacter.NickName, msg, client.Player.PlayerCharacter.ID);
							playerById.Out.sendOneOnOneTalk(client.Player.PlayerCharacter.ID, isAutoReply: false, client.Player.PlayerCharacter.NickName, msg, num4);
						}
						else
						{
							client.Player.Out.SendMessage(eMessageType.GM_NOTICE, "O jogador não está online!");
						}
						break;
					}
				case 160:
					{
						string text = packet.ReadString();
						int num3 = packet.ReadInt();
						if (num3 < 0 || num3 > 1)
						{
							return 1;
						}
						using (PlayerBussiness playerBussiness2 = new PlayerBussiness())
						{
							GamePlayer clientByPlayerNickName = WorldMgr.GetClientByPlayerNickName(text);
							PlayerInfo playerInfo = ((clientByPlayerNickName == null) ? playerBussiness2.GetUserSingleByNickName(text) : clientByPlayerNickName.PlayerCharacter);
							if (playerInfo != null)
							{
								if (client.Player.Friends.ContainsKey(playerInfo.ID) && client.Player.Friends[playerInfo.ID] == num3)
								{
									client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("FriendAddHandler.Falied"));
								}
								else
								{
									if (!playerBussiness2.AddFriends(new FriendInfo
									{
										FriendID = playerInfo.ID,
										IsExist = true,
										Remark = "",
										UserID = client.Player.PlayerCharacter.ID,
										Relation = num3
									}))
									{
										break;
									}
									client.Player.FriendsAdd(playerInfo.ID, num3);
									if (num3 != 1 && playerInfo.State != 0)
									{
										GSPacketIn gSPacketIn2 = new GSPacketIn(160, client.Player.PlayerCharacter.ID);
										gSPacketIn2.WriteByte(166);
										gSPacketIn2.WriteInt(playerInfo.ID);
										gSPacketIn2.WriteString(client.Player.PlayerCharacter.NickName);
										gSPacketIn2.WriteBoolean(val: false);
										if (clientByPlayerNickName != null)
										{
											clientByPlayerNickName.SendTCP(gSPacketIn2);
										}
										else
										{
											GameServer.Instance.LoginServer.SendPacket(gSPacketIn2);
										}
									}
									client.Out.SendAddFriend(playerInfo, num3, state: true);
									client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("FriendAddHandler.Success2"));
									break;
								}
							}
							else
							{
								client.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation("FriendAddHandler.Success") + text);
							}
						}
						break;
					}
				case 161:
					{
						int num2 = packet.ReadInt();
						using (PlayerBussiness playerBussiness = new PlayerBussiness())
						{
							if (playerBussiness.DeleteFriends(client.Player.PlayerCharacter.ID, num2))
							{
								client.Player.FriendsRemove(num2);
								client.Out.SendFriendRemove(num2);
							}
						}
						break;
					}
				case 165:
					{
						int num = packet.ReadInt();
						GSPacketIn gSPacketIn = new GSPacketIn(160, client.Player.PlayerCharacter.ID);
						gSPacketIn.WriteByte(165);
						gSPacketIn.WriteInt(num);
						gSPacketIn.WriteByte(client.Player.PlayerCharacter.typeVIP);
						gSPacketIn.WriteInt(client.Player.PlayerCharacter.VIPLevel);
						gSPacketIn.WriteBoolean(val: false);
						GameServer.Instance.LoginServer.SendPacket(gSPacketIn);
						WorldMgr.ChangePlayerState(client.Player.PlayerCharacter.ID, num, client.Player.PlayerCharacter.ConsortiaID);
						break;
					}
			}
			return 1;
		}
	}
}
