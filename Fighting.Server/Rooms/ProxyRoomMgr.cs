// Decompiled with JetBrains decompiler
// Type: Fighting.Server.Rooms.ProxyRoomMgr
// Assembly: Fighting.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D337844E-B3A4-4EED-AF5B-EF5071E1E62B
// Assembly location: C:\Users\Administrador.OMINIHOST\Downloads\Nova pasta\Nova pasta\DDtank v41\Emulador\Fight\Fighting.Server.dll

using Fighting.Server.Games;
using Fighting.Server.Guild;
using Game.Logic;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Fighting.Server.Rooms
{
    public class ProxyRoomMgr
    {
        private static readonly ILog ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static readonly int THREAD_INTERVAL = 40;
        public static readonly int PICK_UP_INTERVAL = 5000;
        public static readonly int CLEAR_ROOM_INTERVAL = 60;
        private static bool startWithNpc = false;
        private static int serverId = 1;
        private static Queue<IAction> queue_0 = new Queue<IAction>();
        private static Dictionary<int, ProxyRoom> dictionary_0 = new Dictionary<int, ProxyRoom>();
        private static int int_1 = 0;
        private static long long_0 = 0;
        private static long long_1 = 0;
        private static Thread thread_0;

        public static bool Setup()
        {
            ProxyRoomMgr.thread_0 = new Thread(new ThreadStart(ProxyRoomMgr.smethod_0));
            return true;
        }

        public static void Start()
        {
            if (ProxyRoomMgr.startWithNpc)
                return;
            ProxyRoomMgr.startWithNpc = true;
            ProxyRoomMgr.thread_0.Start();
        }

        public static void Stop()
        {
            if (!ProxyRoomMgr.startWithNpc)
                return;
            ProxyRoomMgr.startWithNpc = false;
            ProxyRoomMgr.thread_0.Join();
        }

        public static void AddAction(IAction action)
        {
            lock (ProxyRoomMgr.queue_0)
                ProxyRoomMgr.queue_0.Enqueue(action);
        }

        private static void smethod_0()
        {
            long num = 0;
            ProxyRoomMgr.long_1 = TickHelper.GetTickCount();
            ProxyRoomMgr.long_0 = TickHelper.GetTickCount();
            while (ProxyRoomMgr.startWithNpc)
            {
                long tickCount1 = TickHelper.GetTickCount();
                try
                {
                    ProxyRoomMgr.smethod_1();
                    if (ProxyRoomMgr.long_0 <= tickCount1)
                    {
                        ProxyRoomMgr.long_0 += (long)ProxyRoomMgr.PICK_UP_INTERVAL;
                        ProxyRoomMgr.smethod_2(tickCount1);
                    }
                    if (ProxyRoomMgr.long_1 <= tickCount1)
                    {
                        ProxyRoomMgr.long_1 += (long)ProxyRoomMgr.CLEAR_ROOM_INTERVAL;
                        ProxyRoomMgr.smethod_4(tickCount1);
                        ProxyRoomMgr.smethod_5();
                    }
                }
                catch (Exception ex)
                {
                    ProxyRoomMgr.ilog_0.Error((object)"Room Mgr Thread Error:", ex);
                }
                long tickCount2 = TickHelper.GetTickCount();
                num += (long)ProxyRoomMgr.THREAD_INTERVAL - (tickCount2 - tickCount1);
                if (num > 0L)
                {
                    Thread.Sleep((int)num);
                    num = 0L;
                }
                else if (num < -1000L)
                {
                    ProxyRoomMgr.ilog_0.WarnFormat("Room Mgr is delay {0} ms!", (object)num);
                    num += 1000L;
                }
            }
        }

        private static void smethod_1()
        {
            IAction[] array = (IAction[])null;
            lock (ProxyRoomMgr.queue_0)
            {
                if (ProxyRoomMgr.queue_0.Count > 0)
                {
                    array = new IAction[ProxyRoomMgr.queue_0.Count];
                    ProxyRoomMgr.queue_0.CopyTo(array, 0);
                    ProxyRoomMgr.queue_0.Clear();
                }
            }
            if (array == null)
                return;
            foreach (IAction action in array)
            {
                try
                {
                    action.Execute();
                }
                catch (Exception ex)
                {
                    ProxyRoomMgr.ilog_0.Error((object)"RoomMgr execute action error:", ex);
                }
            }
        }

        private static void smethod_2(long long_2)
        {
            List<ProxyRoom> waitMatchRoomUnsafe = ProxyRoomMgr.GetWaitMatchRoomUnsafe();
            foreach (ProxyRoom proxyRoom in waitMatchRoomUnsafe)
            {
                int minValue = int.MinValue;
                ProxyRoom proxyRoom_1_1 = (ProxyRoom)null;
                if (!proxyRoom.IsPlaying)
                {
                    if (proxyRoom.RoomType == eRoomType.Match)
                    {
                        switch (proxyRoom.GameType)
                        {
                            case eGameType.Guild:
                                using (List<ProxyRoom>.Enumerator enumerator = waitMatchRoomUnsafe.GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        ProxyRoom current = enumerator.Current;
                                        if ((current.GuildId == 0 || current.GuildId != proxyRoom.GuildId) && (current != proxyRoom && current.GameType == eGameType.Guild) && (proxyRoom.GameType == eGameType.Guild && !current.IsPlaying && (current.PlayerCount == proxyRoom.PlayerCount && !proxyRoom.isAutoBot)) && (!current.isAutoBot && proxyRoom.ZoneId == current.ZoneId) && (GuildMgr.FindGuildRelationShip(proxyRoom.GuildId, current.GuildId) + 1) * 10000 + (int)current.GameType * 1000 + Math.Abs(proxyRoom.FightPower - current.FightPower) + Math.Abs(proxyRoom.AvgLevel - current.AvgLevel) > minValue)
                                            proxyRoom_1_1 = current;
                                    }
                                    break;
                                }
                            case eGameType.ALL:
                                using (List<ProxyRoom>.Enumerator enumerator = waitMatchRoomUnsafe.GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        ProxyRoom current = enumerator.Current;
                                        if ((current.GuildId == 0 || current.GuildId != proxyRoom.GuildId) && (current != proxyRoom && !current.IsPlaying) && current.PlayerCount == proxyRoom.PlayerCount)
                                        {
                                            int num1 = GuildMgr.FindGuildRelationShip(proxyRoom.GuildId, current.GuildId) + 1;
                                            int gameType = (int)current.GameType;
                                            int num2 = Math.Abs(proxyRoom.AvgLevel - current.AvgLevel);
                                            int num3 = Math.Abs(proxyRoom.FightPower - current.FightPower);
                                            if (num1 * 10000 + gameType * 1000 + num3 + num2 > minValue)
                                                proxyRoom_1_1 = current;
                                        }
                                    }
                                    break;
                                }
                            default:
                                if (!proxyRoom.isAutoBot && !proxyRoom.startWithNpc)
                                {
                                    ProxyRoom unsafeWithResult = ProxyRoomMgr.GetMathRoomUnsafeWithResult(proxyRoom);
                                    if (unsafeWithResult != null)
                                    {
                                        proxyRoom_1_1 = unsafeWithResult;
                                        Console.WriteLine("StartMatch in rate: {0}% FP and ratelevel: {1}", (object)proxyRoom.PickUpRate, (object)proxyRoom.PickUpRateLevel);
                                        break;
                                    }
                                    ++proxyRoom.PickUpRateLevel;
                                    proxyRoom.PickUpRate += 10;
                                    break;
                                }
                                break;
                        }
                    }
                    if (proxyRoom_1_1 != null)
                    {
                        ProxyRoomMgr.smethod_6(proxyRoom, proxyRoom_1_1);
                    }
                    else
                    {
                        if (!proxyRoom.IsCrossZone)
                        {
                            if (proxyRoom.PickUpCount >= 4 && !proxyRoom.startWithNpc && (!proxyRoom.isAutoBot && proxyRoom.RoomType == eRoomType.Match) && proxyRoom.PlayerCount > 0)
                            {
                                proxyRoom.startWithNpc = true;
                                proxyRoom.Client.SendBeginFightNpc(proxyRoom.selfId, 0, (int)proxyRoom.GameType, proxyRoom.NpcId, proxyRoom.PlayerCount);
                                Console.WriteLine("Call AutoBot No.{0}", (object)proxyRoom.NpcId);
                            }
                            else if (proxyRoom.startWithNpc && !proxyRoom.isAutoBot)
                            {
                                bool flag = false;
                                foreach (ProxyRoom proxyRoom_1_2 in waitMatchRoomUnsafe)
                                {
                                    if (proxyRoom_1_2 != proxyRoom && proxyRoom_1_2.PlayerCount == proxyRoom.PlayerCount && (!proxyRoom_1_2.IsPlaying && proxyRoom_1_2.isAutoBot) && proxyRoom.NpcId == proxyRoom_1_2.NpcId)
                                    {
                                        flag = true;
                                        Console.WriteLine("Start fight with AutoBot No.{0}. RoomType: {1}, GameType: {2}", (object)proxyRoom.NpcId, (object)proxyRoom.RoomType, (object)proxyRoom.GameType);
                                        ProxyRoomMgr.smethod_6(proxyRoom, proxyRoom_1_2);
                                        break;
                                    }
                                }
                                if (!flag)
                                {
                                    ++proxyRoom.PickUpNPCTotal;
                                    Console.WriteLine("Fight with AutoBot No.{0} - Step: {1} is error no room", (object)proxyRoom.NpcId, (object)proxyRoom.PickUpNPCTotal);
                                    if (proxyRoom.PickUpNPCTotal > 3)
                                    {
                                        proxyRoom.startWithNpc = false;
                                        proxyRoom.PickUpNPCTotal = 0;
                                    }
                                }
                            }
                        }
                        if (proxyRoom.isAutoBot && !proxyRoom.IsPlaying)
                            --proxyRoom.PickUpCount;
                        else
                            ++proxyRoom.PickUpCount;
                    }
                }
            }
        }

        private static void smethod_3(long long_2)
        {
            List<ProxyRoom> waitMatchRoomUnsafe = ProxyRoomMgr.GetWaitMatchRoomUnsafe();
            foreach (ProxyRoom proxyRoom_0 in waitMatchRoomUnsafe)
            {
                int minValue = int.MinValue;
                int num1 = 2;
                ProxyRoom proxyRoom_1_1 = (ProxyRoom)null;
                if (proxyRoom_0.IsPlaying)
                    break;
                if (proxyRoom_0.GameType == eGameType.ALL)
                {
                    foreach (ProxyRoom proxyRoom in waitMatchRoomUnsafe)
                    {
                        if ((proxyRoom.GuildId == 0 || proxyRoom.GuildId != proxyRoom_0.GuildId) && (proxyRoom != proxyRoom_0 && !proxyRoom.IsPlaying) && proxyRoom.PlayerCount == proxyRoom_0.PlayerCount)
                        {
                            int num2 = GuildMgr.FindGuildRelationShip(proxyRoom_0.GuildId, proxyRoom.GuildId) + 1;
                            int gameType = (int)proxyRoom.GameType;
                            int num3 = Math.Abs(proxyRoom_0.AvgLevel - proxyRoom.AvgLevel);
                            int num4 = Math.Abs(proxyRoom_0.FightPower - proxyRoom.FightPower);
                            if (num2 * 10000 + gameType * 1000 + num4 + num3 > minValue)
                                proxyRoom_1_1 = proxyRoom;
                        }
                    }
                }
                else if (proxyRoom_0.GameType == eGameType.Guild)
                {
                    foreach (ProxyRoom proxyRoom in waitMatchRoomUnsafe)
                    {
                        if ((proxyRoom.GuildId == 0 || proxyRoom.GuildId != proxyRoom_0.GuildId) && (proxyRoom != proxyRoom_0 && proxyRoom.GameType == eGameType.Guild) && (!proxyRoom.IsPlaying && proxyRoom.PlayerCount == proxyRoom_0.PlayerCount && (!proxyRoom_0.isAutoBot && !proxyRoom.isAutoBot)) && (GuildMgr.FindGuildRelationShip(proxyRoom_0.GuildId, proxyRoom.GuildId) + 1) * 10000 + (int)proxyRoom.GameType * 1000 + Math.Abs(proxyRoom_0.FightPower - proxyRoom.FightPower) + Math.Abs(proxyRoom_0.AvgLevel - proxyRoom.AvgLevel) > minValue)
                            proxyRoom_1_1 = proxyRoom;
                    }
                }
                else
                {
                    foreach (ProxyRoom proxyRoom in waitMatchRoomUnsafe)
                    {
                        if (proxyRoom != proxyRoom_0 && !proxyRoom_0.isAutoBot && (!proxyRoom.isAutoBot && proxyRoom.GameType != eGameType.Guild) && (!proxyRoom.IsPlaying && proxyRoom.PlayerCount == proxyRoom_0.PlayerCount && proxyRoom.IsCrossZone == proxyRoom_0.IsCrossZone) && ((!proxyRoom.IsCrossZone || proxyRoom.ZoneId != proxyRoom_0.ZoneId) && (proxyRoom_0.AvgLevel <= 40 && proxyRoom.AvgLevel <= 40 || proxyRoom_0.AvgLevel > 40 && proxyRoom.AvgLevel > 40)))
                            proxyRoom_1_1 = proxyRoom;
                    }
                }
                if (proxyRoom_1_1 != null)
                    ProxyRoomMgr.smethod_6(proxyRoom_0, proxyRoom_1_1);
                else if (!proxyRoom_0.IsCrossZone)
                {
                    if (proxyRoom_0.PickUpCount == num1 && !proxyRoom_0.startWithNpc && (!proxyRoom_0.isAutoBot && proxyRoom_0.RoomType == eRoomType.Match))
                    {
                        proxyRoom_0.startWithNpc = true;
                        proxyRoom_0.Client.SendBeginFightNpc(proxyRoom_0.selfId, 0, (int)proxyRoom_0.GameType, proxyRoom_0.NpcId, proxyRoom_0.PlayerCount);
                        Console.WriteLine("Call AutoBot No.{0}", (object)proxyRoom_0.NpcId);
                    }
                    else if (proxyRoom_0.PickUpCount > num1 && proxyRoom_0.startWithNpc && !proxyRoom_0.isAutoBot)
                    {
                        foreach (ProxyRoom proxyRoom_1_2 in waitMatchRoomUnsafe)
                        {
                            if (proxyRoom_1_2 != proxyRoom_0 && proxyRoom_1_2.PlayerCount == proxyRoom_0.PlayerCount && (!proxyRoom_1_2.IsPlaying && proxyRoom_1_2.isAutoBot) && proxyRoom_0.NpcId == proxyRoom_1_2.NpcId)
                            {
                                Console.WriteLine("Start fight with AutoBot No.{0}", (object)proxyRoom_0.NpcId);
                                ProxyRoomMgr.smethod_6(proxyRoom_0, proxyRoom_1_2);
                            }
                        }
                    }
                    if (proxyRoom_0.isAutoBot && !proxyRoom_0.IsPlaying)
                        --proxyRoom_0.PickUpCount;
                    else
                        ++proxyRoom_0.PickUpCount;
                }
            }
        }

        private static int WCSHKRMMIU(object object_0, object object_1)
        {
            if (((ProxyRoom)object_0).PickUpCount < 3)
            {
                int num = Math.Abs(((ProxyRoom)object_0).FightPower - ((ProxyRoom)object_1).FightPower);
                if (((ProxyRoom)object_0).FightPower.ToString().Length < 5 && ((ProxyRoom)object_1).FightPower.ToString().Length < 5 && num < 10000 || ((ProxyRoom)object_0).FightPower.ToString().Length > 5 && ((ProxyRoom)object_1).FightPower.ToString().Length > 5 && num < 100000 || ((ProxyRoom)object_0).FightPower.ToString().Length > 6 && ((ProxyRoom)object_1).FightPower.ToString().Length > 6 && num < 600000)
                    return 1;
            }
            return Math.Abs(((ProxyRoom)object_0).AvgLevel - ((ProxyRoom)object_1).AvgLevel);
        }

        private static void smethod_4(long long_2)
        {
            List<ProxyRoom> proxyRoomList = new List<ProxyRoom>();
            foreach (ProxyRoom proxyRoom in ProxyRoomMgr.dictionary_0.Values)
            {
                if (!proxyRoom.IsPlaying && proxyRoom.Game != null)
                    proxyRoomList.Add(proxyRoom);
            }
            foreach (ProxyRoom proxyRoom in proxyRoomList)
            {
                ProxyRoomMgr.dictionary_0.Remove(proxyRoom.RoomId);
                try
                {
                    proxyRoom.Dispose();
                }
                catch (Exception ex)
                {
                    ProxyRoomMgr.ilog_0.Error((object)"Room dispose error:", ex);
                }
            }
        }

        private static void smethod_5()
        {
            List<ProxyRoom> proxyRoomList = new List<ProxyRoom>();
            foreach (ProxyRoom proxyRoom in ProxyRoomMgr.dictionary_0.Values)
            {
                if (!proxyRoom.IsPlaying && proxyRoom.PickUpCount < -1)
                    proxyRoomList.Add(proxyRoom);
            }
            foreach (ProxyRoom proxyRoom in proxyRoomList)
            {
                ProxyRoomMgr.dictionary_0.Remove(proxyRoom.RoomId);
                try
                {
                    proxyRoom.Dispose();
                }
                catch (Exception ex)
                {
                    ProxyRoomMgr.ilog_0.Error((object)"Room dispose error:", ex);
                }
            }
        }

        private static void smethod_6(ProxyRoom proxyRoom_0, ProxyRoom proxyRoom_1)
        {
            int mapIndex = MapMgr.GetMapIndex(0, (byte)0, ProxyRoomMgr.serverId);
            eGameType gameType = eGameType.Free;
            eRoomType roomType = eRoomType.Match;
            if (proxyRoom_0.GameType == proxyRoom_1.GameType)
                gameType = proxyRoom_0.GameType;
            BaseGame game = (BaseGame)GameMgr.StartBattleGame(proxyRoom_0.GetPlayers(), proxyRoom_0, proxyRoom_1.GetPlayers(), proxyRoom_1, mapIndex, roomType, gameType, 2);
            if (game == null)
                return;
            proxyRoom_1.StartGame(game);
            proxyRoom_0.StartGame(game);
            if (game.GameType != eGameType.Guild)
                return;
            proxyRoom_0.Client.SendConsortiaAlly(proxyRoom_0.GetPlayers()[0].PlayerCharacter.ConsortiaID, proxyRoom_1.GetPlayers()[0].PlayerCharacter.ConsortiaID, game.Id);
        }

        public static void StartWithNpcUnsafe(ProxyRoom room)
        {
            int npcId = room.NpcId;
            ProxyRoom roomUnsafe = ProxyRoomMgr.GetRoomUnsafe(room.RoomId);
            foreach (ProxyRoom proxyRoom_1 in ProxyRoomMgr.GetWaitMatchRoomUnsafe())
            {
                if (proxyRoom_1.isAutoBot && !proxyRoom_1.IsPlaying && (proxyRoom_1.Game == null && proxyRoom_1.NpcId == npcId))
                {
                    Console.WriteLine("Start fight with AutoBot or VPlayer No.{0} ", (object)npcId);
                    ProxyRoomMgr.smethod_6(roomUnsafe, proxyRoom_1);
                }
            }
        }

        public static bool AddRoomUnsafe(ProxyRoom room)
        {
            if (ProxyRoomMgr.dictionary_0.ContainsKey(room.RoomId))
                return false;
            ProxyRoomMgr.dictionary_0.Add(room.RoomId, room);
            return true;
        }

        public static bool RemoveRoomUnsafe(int roomId)
        {
            if (!ProxyRoomMgr.dictionary_0.ContainsKey(roomId))
                return false;
            ProxyRoomMgr.dictionary_0.Remove(roomId);
            return true;
        }

        public static ProxyRoom GetRoomUnsafe(int roomId) => ProxyRoomMgr.dictionary_0.ContainsKey(roomId) ? ProxyRoomMgr.dictionary_0[roomId] : (ProxyRoom)null;

        public static ProxyRoom[] GetAllRoom()
        {
            lock (ProxyRoomMgr.dictionary_0)
                return ProxyRoomMgr.GetAllRoomUnsafe();
        }

        public static ProxyRoom[] GetAllRoomUnsafe()
        {
            ProxyRoom[] array = new ProxyRoom[ProxyRoomMgr.dictionary_0.Values.Count];
            ProxyRoomMgr.dictionary_0.Values.CopyTo(array, 0);
            return array;
        }

        public static List<ProxyRoom> GetWaitMatchRoomUnsafe()
        {
            List<ProxyRoom> proxyRoomList = new List<ProxyRoom>();
            foreach (ProxyRoom proxyRoom in ProxyRoomMgr.dictionary_0.Values)
            {
                if (!proxyRoom.IsPlaying && proxyRoom.Game == null)
                    proxyRoomList.Add(proxyRoom);
            }
            return proxyRoomList;
        }

        public static List<ProxyRoom> GetWaitMatchRoomWithoutBotUnsafe(
          ProxyRoom roomCompare)
        {
            List<ProxyRoom> proxyRoomList = new List<ProxyRoom>();
            foreach (ProxyRoom proxyRoom in ProxyRoomMgr.dictionary_0.Values)
            {
                if (!proxyRoom.IsPlaying && proxyRoom.Game == null && (!proxyRoom.isAutoBot && !proxyRoom.startWithNpc) && (proxyRoom.IsCrossZone == roomCompare.IsCrossZone && (proxyRoom.IsCrossZone || proxyRoom.ZoneId == roomCompare.ZoneId)))
                    proxyRoomList.Add(proxyRoom);
            }
            return proxyRoomList;
        }

        public static ProxyRoom GetMathRoomUnsafeWithResult(ProxyRoom roomCompare)
        {
            List<ProxyRoom> source = new List<ProxyRoom>();
            bool flag = false;
            List<ProxyRoom> withoutBotUnsafe = ProxyRoomMgr.GetWaitMatchRoomWithoutBotUnsafe(roomCompare);
            foreach (ProxyRoom proxyRoom in withoutBotUnsafe)
            {
                if (!proxyRoom.IsPlaying && proxyRoom.Game == null && proxyRoom != roomCompare && ((proxyRoom.PickUpRateLevel >= roomCompare.PickUpRateLevel || roomCompare.PickUpRateLevel == 1) && proxyRoom.PlayerCount == roomCompare.PlayerCount))
                {
                    int num1 = roomCompare.AvgLevel - roomCompare.PickUpRateLevel;
                    int num2 = roomCompare.AvgLevel + roomCompare.PickUpRateLevel;
                    if (proxyRoom.AvgLevel >= num1 && proxyRoom.AvgLevel <= num2)
                    {
                        source.Add(proxyRoom);
                        flag = true;
                    }
                }
            }
            if (source.Count == 0)
            {
                foreach (ProxyRoom proxyRoom in withoutBotUnsafe)
                {
                    if (!proxyRoom.IsPlaying && proxyRoom.Game == null && proxyRoom != roomCompare && ((proxyRoom.PickUpRate >= roomCompare.PickUpRate || roomCompare.PickUpRate == 1) && proxyRoom.PlayerCount == roomCompare.PlayerCount))
                    {
                        int num1 = roomCompare.FightPower - roomCompare.FightPower / 100 * roomCompare.PickUpRate;
                        int num2 = roomCompare.FightPower + roomCompare.FightPower / 100 * roomCompare.PickUpRate;
                        if (proxyRoom.FightPower >= num1 && proxyRoom.FightPower <= num2)
                            source.Add(proxyRoom);
                    }
                }
            }
            if (source.Count <= 0)
                return (ProxyRoom)null;
            List<ProxyRoom> proxyRoomList = flag ? source.OrderBy<ProxyRoom, int>((Func<ProxyRoom, int>)(a => a.AvgLevel)).ToList<ProxyRoom>() : source.OrderBy<ProxyRoom, int>((Func<ProxyRoom, int>)(a => a.FightPower)).ToList<ProxyRoom>();
            return proxyRoomList[proxyRoomList.Count / 2];
        }

        public static int NextRoomId() => Interlocked.Increment(ref ProxyRoomMgr.int_1);

        public static void AddRoom(ProxyRoom room) => ProxyRoomMgr.AddAction((IAction)new AddRoomAction(room));

        public static void RemoveRoom(ProxyRoom room) => ProxyRoomMgr.AddAction((IAction)new RemoveRoomAction(room));
    }
}
