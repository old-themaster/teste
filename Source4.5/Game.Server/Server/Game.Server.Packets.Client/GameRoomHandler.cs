using Bussiness;
using Game.Base.Packets;
using Game.Logic;
using Game.Server.Buffer;
using Game.Server.Managers;
using Game.Server.Rooms;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Server.Packets.Client
{
    [PacketHandler(94, "游戏创建")]
    public class GameRoomHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            int num = packet.ReadInt();
            switch (num)
            {
                case 0:
                    {
                        byte b = packet.ReadByte();
                        byte timeType = packet.ReadByte();
                        string name = packet.ReadString();
                        string password = packet.ReadString();
                        if (b == 15)
                        {
                            if (!client.Player.Labyrinth.completeChallenge)
                            {
                                client.Player.SendMessage("Você ficou sem desafios para hoje!");
                                return 0;
                            }
                            client.Player.Labyrinth.isInGame = true;
                        }
                        if (b == 14)
                        {
                            if (!RoomMgr.WorldBossRoom.WorldbossOpen)
                            {
                                client.Player.CurrentRoom.RemovePlayerUnsafe(client.Player);
                                return 0;
                            }
                            int num2 = DateTime.Compare(client.Player.LastEnterWorldBoss.AddSeconds(55.0), DateTime.Now);
                            if (num2 > 0)
                            {
                                client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("A velocidade do cliente é muito rápida.", new object[0]));
                                return 0;
                            }
                            client.Player.LastEnterWorldBoss = DateTime.Now;
                            client.Player.WorldbossBood = RoomMgr.WorldBossRoom.Blood;
                            AbstractBuffer abstractBuffer = BufferList.CreatePayBuffer(400, 50000, 1);
                            if (abstractBuffer != null)
                            {
                                abstractBuffer.Start(client.Player);
                            }
                            abstractBuffer = BufferList.CreatePayBuffer(406, 30000, 1);
                            if (abstractBuffer != null)
                            {
                                abstractBuffer.Start(client.Player);
                            }
                        }
                        RoomMgr.CreateRoom(client.Player, name, password, (eRoomType)b, timeType);
                        return 0;
                    }
                case 1:
                    bool isInvite = packet.ReadBoolean();
                    int type = packet.ReadInt();
                    int num3 = packet.ReadInt();
                    int roomId = -1;
                    string pwd = (string)null;
                    if (num3 == -1)
                    {
                        roomId = packet.ReadInt();
                        pwd = packet.ReadString();
                    }
                    switch (type)
                    {
                        case 1:
                            type = 0;
                            break;
                        case 2:
                            type = 4;
                            break;
                    }
                    RoomMgr.EnterRoom(client.Player, roomId, pwd, type, isInvite);
                    break;
                case 2:
                    if (client.Player.CurrentRoom != null && client.Player == client.Player.CurrentRoom.Host && !client.Player.CurrentRoom.IsPlaying)
                    {
                        int mapId = packet.ReadInt();
                        eRoomType roomType = (eRoomType)packet.ReadByte();
                        string roomPass = packet.ReadString();
                        string roomName = packet.ReadString();
                        byte timeMode = packet.ReadByte();
                        byte num4 = packet.ReadByte();
                        int levelLimits = packet.ReadInt();
                        bool isCross = packet.ReadBoolean();
                        int currentFloor = 1;
                        if (mapId == 0 && roomType == eRoomType.Labyrinth)
                        {
                            mapId = 401;
                            currentFloor = client.Player.Labyrinth.currentFloor;
                        }
                        RoomMgr.UpdateRoomGameType(client.Player.CurrentRoom, roomType, timeMode, (eHardLevel)num4, levelLimits, mapId, roomName, roomPass, isCross, currentFloor);
                        break;
                    }
                    if (!client.Player.CurrentRoom.IsPlaying)
                    {
                        client.Player.SendMessage("A sala não foi encontrada");
                        break;
                    }
                    break;
                case 3:
                    if (client.Player.CurrentRoom != null && client.Player == client.Player.CurrentRoom.Host)
                    {
                        RoomMgr.KickPlayer(client.Player.CurrentRoom, packet.ReadByte());
                        break;
                    }
                    break;
                case 5:
                    if (client.Player.CurrentRoom != null)
                    {
                        RoomMgr.ExitRoom(client.Player.CurrentRoom, client.Player);
                        break;
                    }
                    break;
                case 6:
                    if (client.Player.CurrentRoom == null || client.Player.CurrentRoom.RoomType == eRoomType.Match)
                        return 0;
                    RoomMgr.smethod_2(client.Player);
                    break;
                case 7:
                    BaseRoom currentRoom = client.Player.CurrentRoom;
                    if (currentRoom != null && currentRoom.Host == client.Player)
                    {
                        if (client.Player.MainWeapon == null)
                        {
                            client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.SceneGames.NoEquip"));
                            return 0;
                        }
                        if (currentRoom.RoomType == eRoomType.Dungeon)
                        {
                            if (!client.Player.IsPvePermission(currentRoom.MapId, currentRoom.HardLevel))
                            {
                                client.Player.SendMessage("Você não possui permissões para entrar na sala");
                                return 0;
                            }
                        }
                        else if (currentRoom.RoomType == eRoomType.Boss && client.Player.RemoveMoney(100) <= 0)
                        {
                            client.Player.SendInsufficientMoney(0);
                            return 0;
                        }
                        RoomMgr.StartGame(client.Player.CurrentRoom);
                        break;
                    }
                    client.Player.SendMessage("A sala está cheia");
                    this.roomGameStart(client.Player, packet);
                    break;
                case 9:
                    int num5 = packet.ReadInt();
                    packet.ReadInt();
                    if (packet.ReadInt() < 0)
                        ;
                    List<BaseRoom> source = num5 != 1 ? RoomMgr.GetAllDungeonRooms() : RoomMgr.GetAllMatchRooms();
                    if (source.Count > 0)
                    {
                        List<BaseRoom> list = source.OrderBy<BaseRoom, Guid>((Func<BaseRoom, Guid>)(a => Guid.NewGuid())).Take<BaseRoom>(8).ToList<BaseRoom>();
                        client.Out.SendUpdateRoomList(list);
                        break;
                    }
                    break;
                case 10:
                    if (client.Player.CurrentRoom != null && client.Player == client.Player.CurrentRoom.Host)
                    {
                        RoomMgr.UpdateRoomPos(client.Player.CurrentRoom, (int)packet.ReadByte(), packet.ReadBoolean());
                        break;
                    }
                    break;
                case 11:
                    if (client.Player.CurrentRoom != null && client.Player.CurrentRoom.BattleServer != null)
                    {
                        client.Player.CurrentRoom.BattleServer.RemoveRoom(client.Player.CurrentRoom);
                        break;
                    }
                    break;
                case 12:
                    if (client.Player.CurrentRoom != null)
                    {
                        if (packet.ReadInt() == 0)
                            client.Player.CurrentRoom.GameType = eGameType.Free;
                        else if (client.Player.CurrentRoom.IsAllSameGuild())
                            client.Player.CurrentRoom.GameType = eGameType.Guild;
                        GSPacketIn pkg = client.Player.Out.SendRoomType(client.Player, client.Player.CurrentRoom);
                        client.Player.CurrentRoom.SendToAll(pkg, client.Player);
                        break;
                    }
                    break;
                case 15:
                    if (client.Player.MainWeapon == null)
                    {
                        client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.SceneGames.NoEquip"));
                        break;
                    }
                    if (client.Player.CurrentRoom.Host == client.Player)
                    {
                        client.Player.CurrentRoom.SendPlaceState();
                        break;
                    }
                    if (client.Player.CurrentRoom != null)
                    {
                        RoomMgr.UpdatePlayerState(client.Player, packet.ReadByte());
                        break;
                    }
                    break;
                default:
                    Console.WriteLine("//gameroomcmd: " + (object)num);
                    break;
                case 18:
                    {
                        int num10 = packet.ReadInt();
                        if (client.Player.MainWeapon == null)
                        {
                            client.Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.SceneGames.NoEquip", new object[0]));
                            return 0;
                        }
                        int num11 = num10;
                        switch (num11)
                        {
                            case 1:
                                //	RoomMgr.CreateEncounterRoom(client.Player, eRoomType.Encounter);
                                return 0;
                            case 2:
                                {
                                    ConsortiaInfo consortiaById = ConsortiaBossMgr.GetConsortiaById(client.Player.PlayerCharacter.ConsortiaID);
                                    int bossLevel = 10;
                                    if (consortiaById != null)
                                    {
                                        bossLevel = consortiaById.callBossLevel;
                                    }
                                    RoomMgr.CreateConsortiaBossRoom(client.Player, eRoomType.ConsortiaBoss, bossLevel);
                                    return 0;
                                }
                            case 3:
                                //RoomMgr.CreateBattleRoom(client.Player, eRoomType.BattleRoom);
                                return 0;
                            case 4:
                                if (client.Player.CurrentRoom != null)
                                {
                                    client.Player.CurrentRoom.RemovePlayerUnsafe(client.Player);
                                }
                                if (!client.Player.IsActive)
                                {
                                    return 0;
                                }
                                RoomMgr.WaitingRoom.RemovePlayer(client.Player);
                                //RoomMgr.ConsBatRoom.AddPlayer(client.Player);
                                return 0;
                            case 5:
                            case 6:
                            case 11:
                            case 12:
                            case 13:
                            case 14:
                            case 15:
                            case 16:
                                break;
                            case 7:
                            case 8:
                            case 9:
                            case 10:
                                //	RoomMgr.CreateGroupBattleRoom(client.Player, num10);
                                return 0;
                            case 17:
                                if (client.Player.CurrentRoom != null)
                                {
                                    client.Player.CurrentRoom.RemovePlayerUnsafe(client.Player);
                                }
                                if (!client.Player.IsActive)
                                {
                                    return 0;
                                }
                                RoomMgr.WaitingRoom.RemovePlayer(client.Player);
                                //	RoomMgr.CampBattleRoom.AddPlayer(client.Player);
                                return 0;
                            default:
                                if (num11 == 20)
                                {
                                    client.Player.ClearFootballCard();
                                    //	RoomMgr.CreateFightFootballTimeRoom(client.Player, eRoomType.FightFootballTime);
                                    return 0;
                                }
                                break;
                        }
                        Console.WriteLine("SINGLE_ROOM_BEGIN  " + num10);
                        return 0;
                    }
                case 19:
                    {
                        ItemInfo itemByTemplateID = client.Player.PropBag.GetItemByTemplateID(0, 11101);
                        if (itemByTemplateID != null)
                        {
                            GSPacketIn gSPacketIn = new GSPacketIn(94, client.Player.PlayerId);
                            gSPacketIn.WriteByte(19);
                            gSPacketIn.WriteInt(client.Player.PlayerId);
                            gSPacketIn.WriteString(client.Player.PlayerCharacter.NickName);
                            gSPacketIn.WriteString(client.Player.CurrentRoom.GetNameByMapId());
                            gSPacketIn.WriteInt(client.Player.CurrentRoom.RoomId);
                            gSPacketIn.WriteString(client.Player.CurrentRoom.Password);
                            client.Player.PropBag.RemoveCountFromStack(itemByTemplateID, 1);
                            GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
                            for (int i = 0; i < allPlayers.Length; i++)
                            {
                                GamePlayer gamePlayer = allPlayers[i];
                                gSPacketIn.ClientID = gamePlayer.PlayerCharacter.ID;
                                gamePlayer.Out.SendTCP(gSPacketIn);
                            }
                            return 0;
                        }
                        return 0;
                    }
                case 20:
                    {
                        GSPacketIn gSPacketIn2 = new GSPacketIn(94, client.Player.PlayerId);
                        gSPacketIn2.WriteByte(20);
                        {
                            gSPacketIn2.WriteBoolean(true);
                        }
                        client.Player.SendTCP(gSPacketIn2);
                        return 0;
                    }
            }
            Console.WriteLine("GameRoomHandler: " + (GameRoomPackageType)num);
            return 0;
        }

        public bool roomGameStart(GamePlayer Player, GSPacketIn packet)
        {
            BaseRoom currentRoom = Player.CurrentRoom;

#if DEBUG
            Console.Write($"GameStart roomType {(eRoomType)currentRoom.RoomType}");
#endif
            if (currentRoom != null && currentRoom.Host == Player)
            {
                if (currentRoom.RoomType == eRoomType.EntertainmentRoomPK && !Player.MoneyDirect(100))
                    return false;
                List<GamePlayer> players = currentRoom.GetPlayers();
                bool flag = false;
                if (Player.MainWeapon == null)
                {
                    Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.SceneGames.NoEquip"));
                    flag = true;
                }
                else if (currentRoom.RoomType == eRoomType.Dungeon && !Player.IsPvePermission(currentRoom.MapId, currentRoom.HardLevel))
                {
                    Player.SendMessage(LanguageMgr.GetTranslation("GameStart.Msg1"));
                    flag = true;
                }
                else if (currentRoom.RoomType == eRoomType.FarmBoss && (players.Count == 1 || players.Count > 2))
                {
                    Player.SendMessage(LanguageMgr.GetTranslation("GameStart.Msg2"));
                    flag = true;
                }
                else
                {
                    if (currentRoom.RoomType == eRoomType.SpecialActivityDungeon)
                        flag = this.checkTicketInstance(currentRoom, Player);

                    if (!flag)
                    {
                        foreach (GamePlayer gamePlayer in players)
                        {
                            if (gamePlayer != null && !gamePlayer.PlayerCharacter.isViewer)
                            {
                                if (currentRoom.RoomType == eRoomType.ActivityDungeon && gamePlayer.Actives.Info.activityTanabataNum == 0)
                                    ++gamePlayer.Actives.Info.activityTanabataNum;
                                if (currentRoom.RoomType == eRoomType.SpecialActivityDungeon)
                                    gamePlayer.PropBag.RemoveTemplate(currentRoom.GetDungeonTicketId(currentRoom.MapId, currentRoom.HardLevel), 1);
                                if (currentRoom.RoomType == eRoomType.Lanbyrinth || currentRoom.RoomType == eRoomType.Dungeon)
                                    // gamePlayer.Extra.KingBlessStrengthEnchance(true);
                                    // else
                                    // gamePlayer.Extra.KingBlessStrengthEnchance(false);
                                    gamePlayer.PetBag.ReduceHunger();
                                if (currentRoom.RoomType == eRoomType.EntertainmentRoom || currentRoom.RoomType == eRoomType.EntertainmentRoomPK)
                                {
                                    gamePlayer.ClearFightBag();
                                    AbstractBuffer payBuffer = BufferList.CreatePayBuffer(400, 20000, 1);
                                    if (payBuffer != null)
                                        payBuffer.Start(gamePlayer);
                                }
                                gamePlayer.PlayerCharacter.ringFlag = false;

                            }
                        }
                        RoomMgr.StartGame(Player.CurrentRoom);
                    }
                }
                if (flag)
                {
                    Player.CurrentRoom.IsPlaying = false;
                    Player.CurrentRoom.SendCancelPickUp();
                }
            }

            return true;
        }

        public bool checkTicketInstance(BaseRoom room, GamePlayer Player)
        {
            bool flag = false;

            List<GamePlayer> players = room.GetPlayers();


            int dungeonTicketId = room.GetDungeonTicketId(room.MapId, room.HardLevel);
            int teamCount = 1;
            string playersName = "";

            foreach (GamePlayer gamePlayer in players)
            {
                if (gamePlayer.PropBag.GetItemCount(dungeonTicketId) >= 1)
                    continue;

                ++teamCount;

                flag = true;

                playersName = playersName + $"[{gamePlayer.PlayerCharacter.NickName}]";

                if (players.Count > 1 && players.Count != teamCount)
                    playersName += ", ";
            }

            if (teamCount == 1)
            {
                Player.SendMessage("Você não possui ingresso suficiente.");
            }
            else
            {
                foreach (GamePlayer gamePlayer in players)
                {
                    gamePlayer.SendMessage($"Os jogadores {playersName} não possui ingressos suficientes.");

                }
            }


            return flag;
        }
    }
}

