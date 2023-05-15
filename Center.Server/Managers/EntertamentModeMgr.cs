namespace Center.Server.Managers
{
    using Bussiness.Managers;
    using Center.Server;
    using Game.Base.Packets;
    using log4net;
    using SqlDataProvider.Data;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class EntertamentModeMgr
    {
        private static readonly ILog log;
        private static bool m_isOpen;
        private static bool m_isSendItem;
        private static object m_lock;
        private static Dictionary<int, PlayerInfo> m_players;
        private static object m_send;
        private static DateTime m_timeEnd;
        private static DateTime m_timeStart;

        static EntertamentModeMgr()
        {
            log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            m_lock = new object();
            m_send = new object();
            m_isOpen = false;
            m_timeStart = DateTime.Now;
            m_timeEnd = DateTime.Now;
            m_isSendItem = false;
            m_players = new Dictionary<int, PlayerInfo>();
        }

        public EntertamentModeMgr()
        {
        }

        private static void AddPlayer(PlayerInfo p)
        {
            lock (m_players)
            {
                if (!m_players.ContainsKey(p.ID))
                {
                    m_players.Add(p.ID, p);
                }
            }
        }

        public static void CheckOpenOrClose()
        {
            lock (m_lock)
            {
                DateTime now = DateTime.Now;
                if (m_isOpen && (m_timeEnd <= now))
                {
                    m_isOpen = false;
                    m_timeStart = DateTime.Now;
                    m_timeEnd = DateTime.Now;
                    if (!m_isSendItem)
                    {
                        SendItemToMail();
                    }
                    CenterServer.Instance.SendSystemNotice("V\x00f5 d\x00e0i 18+ d\x00e3 k?t th\x00fac. Ph?n thu?ng ho?t d?ng s? g?i v\x00e0o thu trong \x00edt ph\x00fat.");
                    SendOpenOrClose(true);
                }
                else if (!m_isOpen)
                {
                    switch (now.Hour)
                    {
                        case 10:
                            m_isOpen = true;
                            m_isSendItem = false;
                            m_timeStart = now;
                            m_timeEnd = now.AddHours(1.0);
                            break;

                        case 15:
                            m_isOpen = true;
                            m_isSendItem = false;
                            m_timeStart = now;
                            m_timeEnd = now.AddHours(1.0);
                            break;

                        case 0x13:
                            m_isOpen = true;
                            m_isSendItem = false;
                            m_timeStart = now;
                            m_timeEnd = now.AddHours(1.0);
                            break;
                    }
                    if (m_isOpen)
                    {
                        CenterServer.Instance.SendSystemNotice("V\x00f5 d\x00e0i 18+ d\x00e3 m?. H\x00e3y tham gia d? nh?n ph?n thu?ng h?p d?n.");
                        SendOpenOrClose(true);
                    }
                }
            }
        }

        private static PlayerInfo FindPlayer(int userId)
        {
            lock (m_players)
            {
                if (m_players.ContainsKey(userId))
                {
                    return m_players[userId];
                }
                return null;
            }
        }

        public static bool MailNotice(int playerID)
        {
            try
            {
                ServerClient serverClient = LoginMgr.GetServerClient(playerID);
                if (serverClient != null)
                {
                    GSPacketIn pkg = new GSPacketIn(0x75);
                    pkg.WriteInt(playerID);
                    pkg.WriteInt(1);
                    serverClient.SendTCP(pkg);
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }

        public static GSPacketIn SendGetPoint(PlayerInfo player)
        {
            PlayerInfo info = FindPlayer(player.ID);
            if (info == null)
            {
                player.EntertamentPoint = 0;
                AddPlayer(player);
                info = player;
            }
            GSPacketIn @in = new GSPacketIn(0xbd);
            @in.WriteInt(info.ID);
            @in.WriteInt(info.EntertamentPoint);
            return @in;
        }

        private static void SendItemToMail()
        {
            lock (m_send)
            {
                if (!m_isSendItem)
                {
                    m_isSendItem = true;
                    int num = 0;
                    foreach (PlayerInfo info in m_players.Values)
                    {
                        if (info.EntertamentPoint > 0)
                        {
                            List<EntertamentModeAwardInfo> entertamentAwardWithPoint = WorldEventMgr.GetEntertamentAwardWithPoint(info.EntertamentPoint);
                            if (entertamentAwardWithPoint.Count > 0)
                            {
                                string format = "Ph?n thu?ng {0} di?m ho?t d?ng V\x00f5 Ð\x00e0i 18+";
                                List<SqlDataProvider.Data.ItemInfo> infos = new List<SqlDataProvider.Data.ItemInfo>();
                                int pointNeed = 0;
                                foreach (EntertamentModeAwardInfo info2 in entertamentAwardWithPoint)
                                {
                                    SqlDataProvider.Data.ItemInfo item = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(info2.TemplateID), 1, 0x69);
                                    item.IsBinds = info2.IsBind;
                                    item.ValidDate = info2.VaildDate;
                                    item.Count = info2.Count;
                                    item.AttackCompose = info2.Attack;
                                    item.DefendCompose = info2.Defence;
                                    item.AgilityCompose = info2.Agility;
                                    item.LuckCompose = info2.Luck;                                  
                                    item.StrengthenLevel = info2.Strengthen;
                                    item.IsUsed = true;
                                    item.BeginDate = DateTime.Now;
                                    infos.Add(item);
                                    if (pointNeed == 0)
                                    {
                                        pointNeed = info2.PointNeed;
                                    }
                                }
                                format = string.Format(format, pointNeed);
                                if (WorldEventMgr.SendItemsToMail(infos, info.ID, info.NickName, format))
                                {
                                    MailNotice(info.ID);
                                    num++;
                                }
                            }
                        }
                    }
                    m_players = new Dictionary<int, PlayerInfo>();
                    log.Info("Send award to " + num + " players (Vo Dai 18+) completed!");
                }
            }
        }

        public static GSPacketIn SendOpenOrClose(bool sendToAll)
        {
            GSPacketIn pkg = new GSPacketIn(0xc0);
            pkg.WriteBoolean(m_isOpen);
            pkg.WriteDateTime(m_timeStart);
            pkg.WriteDateTime(m_timeEnd);
            if (sendToAll)
            {
                CenterServer.Instance.method_0(pkg);
            }
            return pkg;
        }

        public static void UpdatePoint(PlayerInfo player, int point)
        {
            lock (m_players)
            {
                if (m_players.ContainsKey(player.ID))
                {
                    PlayerInfo local1 = m_players[player.ID];
                    local1.EntertamentPoint += point;
                }
                else
                {
                    player.EntertamentPoint = point;
                    m_players.Add(player.ID, player);
                }
            }
        }
    }
}
