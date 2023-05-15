// Decompiled with JetBrains decompiler
// Type: Game.Server.Rooms.BaseChristmasRoom
// Assembly: Game.Server, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 95F406BD-7233-42D4-AF91-73FA12644876
// Assembly location: C:\Users\Administrador.OMINIHOST\Desktop\dll8.6\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.Managers;
using System.Collections.Generic;
using System.Drawing;

namespace Game.Server.Rooms
{
    public class BaseChristmasRoom
    {
        public static ThreadSafeRandom random = new ThreadSafeRandom();
        public static int LIVIN = 0;
        public static int DEAD = 2;
        public static int FIGHTING = 1;
        public static int MonterAddCount = 15;
        protected int lastMonterID = 1000;
        private int[] monterType = new int[3] { 0, 1, 2 };
        private Point[] brithPoint = new Point[15]
        {
      new Point(353, 570),
      new Point(246, 760),
      new Point(593, 590),
      new Point(466, 898),
      new Point(800, 950),
      new Point(946, 748),
      new Point(1152, 873),
      new Point(1172, 874),
      new Point(1766, 630),
      new Point(1342, 581),
      new Point(1732, 401),
      new Point(1462, 326),
      new Point(1187, 207),
      new Point(878, 236),
      new Point(1590, 521)
        };
        private Dictionary<int, GamePlayer> m_players;
        private Dictionary<int, MonterInfo> m_monters;
        public int DefaultPosX = 500;
        public int DefaultPosY = 500;

        public Dictionary<int, MonterInfo> Monters => this.m_monters;

        public BaseChristmasRoom()
        {
            this.m_players = new Dictionary<int, GamePlayer>();
            this.m_monters = new Dictionary<int, MonterInfo>();
            this.AddFistMonters();
        }

        public void AddFistMonters()
        {
            lock (this.m_monters)
            {
                for (int index = 0; index < BaseChristmasRoom.MonterAddCount; ++index)
                {
                    MonterInfo monterInfo = new MonterInfo();
                    monterInfo.ID = this.lastMonterID;
                    monterInfo.type = BaseChristmasRoom.random.Next(this.monterType.Length);
                    monterInfo.MonsterPos = this.brithPoint[index];
                    monterInfo.MonsterNewPos = this.brithPoint[index];
                    monterInfo.state = BaseChristmasRoom.LIVIN;
                    monterInfo.PlayerID = 0;
                    if (!this.m_monters.ContainsKey(monterInfo.ID))
                        this.m_monters.Add(monterInfo.ID, monterInfo);
                    ++this.lastMonterID;
                }
            }
        }

        private int GetFreeMonter()
        {
            int num = 0;
            foreach (MonterInfo monterInfo in this.Monters.Values)
            {
                if (monterInfo.state == BaseChristmasRoom.LIVIN)
                    ++num;
            }
            return num;
        }

        public void AddMoreMonters()
        {
            if (this.GetFreeMonter() >= this.m_players.Count)
                return;
            this.AddMonters();
        }

        public void AddMonters()
        {
            lock (this.m_monters)
            {
                MonterInfo monterInfo = new MonterInfo();
                monterInfo.ID = this.lastMonterID;
                monterInfo.type = BaseChristmasRoom.random.Next(this.monterType.Length);
                int index = BaseChristmasRoom.random.Next(this.brithPoint.Length);
                monterInfo.MonsterPos = this.brithPoint[index];
                monterInfo.MonsterNewPos = this.brithPoint[index];
                monterInfo.state = BaseChristmasRoom.LIVIN;
                monterInfo.PlayerID = 0;
                if (!this.m_monters.ContainsKey(monterInfo.ID))
                    this.m_monters.Add(monterInfo.ID, monterInfo);
                ++this.lastMonterID;
            }
        }

        public bool SetFightMonter(int Id, int playerId)
        {
            bool flag = false;
            lock (this.m_monters)
            {
                if (this.m_monters.ContainsKey(Id))
                {
                    this.m_monters[Id].state = BaseChristmasRoom.FIGHTING;
                    this.m_monters[Id].PlayerID = playerId;
                    flag = true;
                }
            }
            this.AddMonters();
            return flag;
        }

        public void SetMonterDie(int playerId)
        {
            int num = -1;
            foreach (MonterInfo monterInfo in this.m_monters.Values)
            {
                if (monterInfo.PlayerID == playerId)
                {
                    num = monterInfo.ID;
                    break;
                }
            }
            if (num <= -1)
                return;
            lock (this.m_monters)
            {
                if (this.m_monters.ContainsKey(num))
                    this.m_monters.Remove(num);
            }
            GSPacketIn packet = new GSPacketIn((short)145);
            packet.WriteByte((byte)22);
            packet.WriteByte((byte)1);
            packet.WriteInt(num);
            this.SendToALL(packet);
        }

        public void AddPlayer(GamePlayer player)
        {
            lock (this.m_players)
            {
                if (!this.m_players.ContainsKey(player.PlayerId))
                {
                    player.IsInChristmasRoom = true;
                    this.m_players.Add(player.PlayerId, player);
                    player.Actives.BeginChristmasTimer();
                }
            }
            this.UpdateRoom();
        }

        public void UpdateRoom()
        {
            GamePlayer[] playersSafe = this.GetPlayersSafe();
            GSPacketIn packet = new GSPacketIn((short)145);
            packet.WriteByte((byte)18);
            packet.WriteInt(playersSafe.Length);
            foreach (GamePlayer gamePlayer in playersSafe)
            {
                packet.WriteInt(gamePlayer.PlayerCharacter.Grade);
                packet.WriteInt(gamePlayer.PlayerCharacter.Hide);
                packet.WriteInt(gamePlayer.PlayerCharacter.Repute);
                packet.WriteInt(gamePlayer.PlayerCharacter.ID);
                packet.WriteString(gamePlayer.PlayerCharacter.NickName);
                packet.WriteByte(gamePlayer.PlayerCharacter.typeVIP);
                packet.WriteInt(gamePlayer.PlayerCharacter.VIPLevel);
                packet.WriteBoolean(gamePlayer.PlayerCharacter.Sex);
                packet.WriteString(gamePlayer.PlayerCharacter.Style);
                packet.WriteString(gamePlayer.PlayerCharacter.Colors);
                packet.WriteString(gamePlayer.PlayerCharacter.Skin);
                packet.WriteInt(gamePlayer.PlayerCharacter.FightPower);
                packet.WriteInt(gamePlayer.PlayerCharacter.Win);
                packet.WriteInt(gamePlayer.PlayerCharacter.Total);
                packet.WriteInt(gamePlayer.PlayerCharacter.Offer);
                packet.WriteInt(gamePlayer.X);
                packet.WriteInt(gamePlayer.Y);
                packet.WriteByte(gamePlayer.States);
            }
            this.SendToALL(packet);
        }

        public void ViewOtherPlayerRoom(GamePlayer player)
        {
            GamePlayer[] playersSafe = this.GetPlayersSafe();
            GSPacketIn pkg = new GSPacketIn((short)145);
            pkg.WriteByte((byte)18);
            pkg.WriteInt(playersSafe.Length);
            foreach (GamePlayer gamePlayer in playersSafe)
            {
                pkg.WriteInt(gamePlayer.PlayerCharacter.Grade);
                pkg.WriteInt(gamePlayer.PlayerCharacter.Hide);
                pkg.WriteInt(gamePlayer.PlayerCharacter.Repute);
                pkg.WriteInt(gamePlayer.PlayerCharacter.ID);
                pkg.WriteString(gamePlayer.PlayerCharacter.NickName);
                pkg.WriteByte(gamePlayer.PlayerCharacter.typeVIP);
                pkg.WriteInt(gamePlayer.PlayerCharacter.VIPLevel);
                pkg.WriteBoolean(gamePlayer.PlayerCharacter.Sex);
                pkg.WriteString(gamePlayer.PlayerCharacter.Style);
                pkg.WriteString(gamePlayer.PlayerCharacter.Colors);
                pkg.WriteString(gamePlayer.PlayerCharacter.Skin);
                pkg.WriteInt(gamePlayer.PlayerCharacter.FightPower);
                pkg.WriteInt(gamePlayer.PlayerCharacter.Win);
                pkg.WriteInt(gamePlayer.PlayerCharacter.Total);
                pkg.WriteInt(gamePlayer.PlayerCharacter.Offer);
                pkg.WriteInt(gamePlayer.X);
                pkg.WriteInt(gamePlayer.Y);
                pkg.WriteByte(gamePlayer.States);
            }
            player.SendTCP(pkg);
        }

        public bool RemovePlayer(GamePlayer player)
        {
            bool flag = false;
            lock (this.m_players)
            {
                player.Actives.StopChristmasTimer();
                flag = this.m_players.Remove(player.PlayerId);
            }
            if (flag)
            {
                GSPacketIn packet = new GSPacketIn((short)145);
                packet.WriteByte((byte)19);
                packet.WriteInt(player.PlayerId);
                this.SendToALL(packet);
                player.IsInChristmasRoom = false;
                player.Out.SendSceneRemovePlayer(player);
            }
            return flag;
        }

        public GamePlayer[] GetPlayersSafe()
        {
            GamePlayer[] array = (GamePlayer[])null;
            lock (this.m_players)
            {
                array = new GamePlayer[this.m_players.Count];
                this.m_players.Values.CopyTo(array, 0);
            }
            return array ?? new GamePlayer[0];
        }

        public void SendToALLPlayers(GSPacketIn packet)
        {
            foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
                allPlayer.SendTCP(packet);
        }

        public void SendToALL(GSPacketIn packet) => this.SendToALL(packet, (GamePlayer)null);

        public void SendToALL(GSPacketIn packet, GamePlayer except)
        {
            GamePlayer[] playersSafe = this.GetPlayersSafe();
            if (playersSafe == null)
                return;
            foreach (GamePlayer gamePlayer in playersSafe)
            {
                if (gamePlayer != null && gamePlayer != except)
                    gamePlayer.Out.SendTCP(packet);
            }
        }
    }
}
