using Bussiness;
using Game.Base.Packets;
using Game.Logic;
using Game.Server.Buffer;
using Game.Server.GameObjects;
using Game.Server.Rooms;
using System;
using System.Collections.Generic;

namespace Game.Server.GameRoom.Handle
{
    [Attribute7(7)]
    public class GameStart : IGameRoomCommandHadler
    {
        public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
        {
            BaseRoom currentRoom = Player.CurrentRoom;
            bool result;
            if (currentRoom != null && currentRoom.Host == Player)
            {
                /*if (currentRoom.RoomType == eRoomType.EntertainmentPK && !Player.MoneyDirect(100))
                {
                    result = false;
                    return result;
                }
                System.Collections.Generic.List<GamePlayer> players = currentRoom.GetPlayers();
                bool flag = false;
                if (Player.MainWeapon == null)
                {
                    Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.SceneGames.NoEquip", new object[0]));
                    flag = true;
                }
                else if (currentRoom.RoomType == eRoomType.Dungeon && !Player.IsPvePermission(currentRoom.MapId, currentRoom.HardLevel))
                {
                    Player.SendMessage(LanguageMgr.GetTranslation("GameStart.Msg1", new object[0]));
                    flag = true;
                }
                /*else if (currentRoom.RoomType == eRoomType.FarmBoss && (players.Count == 1 || players.Count > 2))
                {
                    Player.SendMessage(LanguageMgr.GetTranslation("GameStart.Msg2", new object[0]));
                    flag = true;
                }//
                else
                {
                    foreach (GamePlayer current in players)
                    {
                        if (current != null)
                        {
                            if (currentRoom.RoomType == eRoomType.ActivityDungeon && current.Actives.Info.activityTanabataNum == 0)
                            {
                                current.Actives.Info.activityTanabataNum++;
                            }
                            if (currentRoom.RoomType == eRoomType.Lanbyrinth)
                            {
                                goto IL_21C;
                            }
                            if (currentRoom.RoomType == eRoomType.Dungeon)
                            {
                                goto IL_21C;
                            }
                            current.Extra.KingBlessStrengthEnchance(false);
                        IL_1BD:
                            current.PetBag.ReduceHunger();
                            if (currentRoom.RoomType != eRoomType.Entertainment && currentRoom.RoomType != eRoomType.EntertainmentPK)
                            {
                                continue;
                            }
                            current.ClearFightBag();
                            AbstractBuffer abstractBuffer = BufferList.CreatePayBuffer(400, 20000, 1);
                            if (abstractBuffer != null)
                            {
                                abstractBuffer.Start(current);
                                continue;
                            }
                            continue;
                        IL_21C:
                            current.Extra.KingBlessStrengthEnchance(true);
                            goto IL_1BD;
                        }
                    }
                    RoomMgr.StartGame(Player.CurrentRoom);
                }
                if (flag)
                {
                    Player.CurrentRoom.IsPlaying = false;
                    Player.CurrentRoom.SendCancelPickUp();
                }*/
                return true;
            }
            result = true;
            return result;
        }
    }
}
