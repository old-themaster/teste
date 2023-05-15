using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;

namespace Game.Server.Rooms
{
    public class BaseCatchInsectRoom
    {
        private Dictionary<int, GamePlayer> m_list = new Dictionary<int, GamePlayer>();

        public bool AddPlayer(GamePlayer player)
        {
            bool flag = false;
            lock (this.m_list)
            {
                if (!this.m_list.ContainsKey(player.PlayerId))
                {
                    this.m_list.Add(player.PlayerId, player);
                    flag = true;
                }
            }
            if (flag)
            {
                GSPacketIn packet = player.Out.SendAddPlayerCatchInsect(player);
                this.SendToALL(packet);
            }
            return flag;
        }

        public GamePlayer FindPlayerNotSafeWithID(int ID)
        {
            GamePlayer result;
            lock (this.m_list)
            {
                if (!this.m_list.ContainsKey(ID))
                {
                    result = this.m_list[ID];
                }
                else
                {
                    result = null;
                }
            }
            return result;
        }

        public GamePlayer FindPlayerSafeWithID(int ID)
        {
            GamePlayer[] playersSafe = this.GetPlayersSafe();
            GamePlayer gamePlayer2;
            GamePlayer result;
            for (int i = 0; i < playersSafe.Length; i++)
            {
                GamePlayer gamePlayer = playersSafe[i];
                if (gamePlayer.PlayerCharacter.ID == ID)
                {
                    gamePlayer2 = gamePlayer;
                    result = gamePlayer2;
                    return result;
                }
            }
            gamePlayer2 = null;
            result = gamePlayer2;
            return result;
        }

        public GamePlayer[] GetPlayersSafe()
        {
            GamePlayer[] array = null;
            lock (this.m_list)
            {
                array = new GamePlayer[this.m_list.Count];
                this.m_list.Values.CopyTo(array, 0);
            }
            GamePlayer[] result;
            if (array != null)
            {
                result = array;
            }
            else
            {
                result = new GamePlayer[0];
            }
            return result;
        }

        public GamePlayer[] GetPlayersSafeWithOutPlayer(GamePlayer player)
        {
            GamePlayer[] playersSafe = this.GetPlayersSafe();
            List<GamePlayer> list = new List<GamePlayer>();
            int num = 0;
            GamePlayer[] array = playersSafe;
            for (int i = 0; i < array.Length; i++)
            {
                GamePlayer gamePlayer = array[i];
                if (gamePlayer != player)
                {
                    list.Add(gamePlayer);
                }
                if (num >= 20)
                {
                    break;
                }
                num++;
            }
            return list.ToArray();
        }

        public bool RemovePlayer(GamePlayer player)
        {
            bool flag = false;
            lock (this.m_list)
            {
                flag = this.m_list.Remove(player.PlayerId);
            }
            if (flag)
            {
                if (player.CatchInsectBossId > 0 && player.CatchInsectState == 1)
                {
                    CatchInsectNpcInfo catchInsectNpcInfo = ActiveSystemMgr.FindCatchInsectNpc(player.CatchInsectBossId);
                    if (catchInsectNpcInfo != null)
                    {
                        catchInsectNpcInfo.State = 0;
                        this.SendUpdateNpc(catchInsectNpcInfo);
                    }
                }
                GSPacketIn packet = player.Out.SendRemovePlayerCatchInsect(player);
                this.SendToALL(packet);
            }
            return true;
        }

        public GSPacketIn SendAddNpc(List<CatchInsectNpcInfo> lists, bool toAll)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(145);
            gSPacketIn.WriteByte(133);
            gSPacketIn.WriteByte(0);
            gSPacketIn.WriteInt(lists.Count);
            foreach (CatchInsectNpcInfo current in lists)
            {
                gSPacketIn.WriteInt(current.ID);
                gSPacketIn.WriteInt(current.Type);
                gSPacketIn.WriteInt(current.State);
                gSPacketIn.WriteInt(current.X);
                gSPacketIn.WriteInt(current.Y);
            }
            if (toAll)
            {
                this.SendToALL(gSPacketIn);
            }
            return gSPacketIn;
        }

        public void SendAllPlayerInRoom(GamePlayer player)
        {
            GamePlayer[] playersSafe = this.GetPlayersSafe();
            if (playersSafe.Length > 0)
            {
                GSPacketIn gSPacketIn = new GSPacketIn(145);
                gSPacketIn.WriteByte(129);
                gSPacketIn.WriteInt(playersSafe.Length);
                GamePlayer[] array = playersSafe;
                for (int i = 0; i < array.Length; i++)
                {
                    GamePlayer gamePlayer = array[i];
                    gSPacketIn.WriteInt(gamePlayer.PlayerCharacter.Grade);
                    gSPacketIn.WriteInt(gamePlayer.PlayerCharacter.Hide);
                    gSPacketIn.WriteInt(gamePlayer.PlayerCharacter.Repute);
                    gSPacketIn.WriteInt(gamePlayer.PlayerCharacter.ID);
                    gSPacketIn.WriteString(gamePlayer.PlayerCharacter.NickName);
                    gSPacketIn.WriteByte(gamePlayer.PlayerCharacter.typeVIP);
                    gSPacketIn.WriteInt(gamePlayer.PlayerCharacter.VIPLevel);
                    gSPacketIn.WriteBoolean(gamePlayer.PlayerCharacter.Sex);
                    gSPacketIn.WriteString(gamePlayer.PlayerCharacter.Style);
                    gSPacketIn.WriteString(gamePlayer.PlayerCharacter.Colors);
                    gSPacketIn.WriteString(gamePlayer.PlayerCharacter.Skin);
                    gSPacketIn.WriteInt(gamePlayer.PlayerCharacter.FightPower);
                    gSPacketIn.WriteInt(gamePlayer.PlayerCharacter.Win);
                    gSPacketIn.WriteInt(gamePlayer.PlayerCharacter.Total);
                    gSPacketIn.WriteInt(gamePlayer.PlayerCharacter.Offer);
                    gSPacketIn.WriteInt(gamePlayer.CatchInsectX);
                    gSPacketIn.WriteInt(gamePlayer.CatchInsectY);
                    gSPacketIn.WriteByte(gamePlayer.CatchInsectState);
                }
                player.SendTCP(gSPacketIn);
            }
        }

        public void SendPlayerMove(GamePlayer player, string pointState)
        {
            lock (this.m_list)
            {
                if (this.m_list.ContainsKey(player.PlayerCharacter.ID))
                {
                    GSPacketIn packet = player.Out.SendPlayerMoveCatchInsect(player, pointState);
                    this.SendToALL(packet, player);
                }
            }
        }

        public void SendRemoveNpc(CatchInsectNpcInfo npc)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(145);
            gSPacketIn.WriteByte(133);
            gSPacketIn.WriteByte(1);
            gSPacketIn.WriteInt(npc.ID);
            this.SendToALL(gSPacketIn);
        }

        public void SendToALL(GSPacketIn packet)
        {
            this.SendToALL(packet, null);
        }

        public void SendToALL(GSPacketIn packet, GamePlayer except)
        {
            lock (this.m_list)
            {
                GamePlayer[] array = new GamePlayer[this.m_list.Count];
                this.m_list.Values.CopyTo(array, 0);
                if (array != null)
                {
                    GamePlayer[] array2 = array;
                    for (int i = 0; i < array2.Length; i++)
                    {
                        GamePlayer gamePlayer = array2[i];
                        if (gamePlayer != null && gamePlayer != except)
                        {
                            gamePlayer.Out.SendTCP(packet);
                        }
                    }
                }
            }
        }

        public void SendUpdateNpc(CatchInsectNpcInfo npc)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(145);
            gSPacketIn.WriteByte(133);
            gSPacketIn.WriteByte(3);
            gSPacketIn.WriteInt(npc.ID);
            gSPacketIn.WriteInt(npc.State);
            this.SendToALL(gSPacketIn);
        }

        public void SendUpdatePlayerState(GamePlayer p)
        {
            if (this.FindPlayerSafeWithID(p.PlayerCharacter.ID) != null)
            {
                GSPacketIn gSPacketIn = new GSPacketIn(145);
                gSPacketIn.WriteByte(131);
                gSPacketIn.WriteInt(p.PlayerCharacter.ID);
                gSPacketIn.WriteByte(p.CatchInsectState);
                gSPacketIn.WriteInt(p.CatchInsectX);
                gSPacketIn.WriteInt(p.CatchInsectY);
                this.SendToALL(gSPacketIn);
            }
        }
    }
}
