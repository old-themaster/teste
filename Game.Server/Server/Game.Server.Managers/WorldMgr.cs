// Decompiled with JetBrains decompiler
// Type: Game.Server.Managers.WorldMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base;
using Game.Base.Packets;
using Game.Server.GameUtils;
using Game.Server.Packets;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;

namespace Game.Server.Managers
{
    public sealed class WorldMgr
  {
    private static readonly ILog ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static ReaderWriterLock readerWriterLock_0 = new ReaderWriterLock();
    private static Dictionary<int, GamePlayer> dictionary_0 = new Dictionary<int, GamePlayer>();
    public static Dictionary<int, UsersExtraInfo> CaddyRank = new Dictionary<int, UsersExtraInfo>();
    private static Dictionary<int, AreaConfigInfo> dictionary_1 = new Dictionary<int, AreaConfigInfo>();
    private static Dictionary<int, EdictumInfo> dictionary_2 = new Dictionary<int, EdictumInfo>();
    private static Dictionary<int, ShopFreeCountInfo> dictionary_3 = new Dictionary<int, ShopFreeCountInfo>();
    private static Dictionary<int, UserExitRoomLogInfo> dictionary_4 = new Dictionary<int, UserExitRoomLogInfo>();
    private static AreaConfigInfo areaConfigInfo_0;
    public static DateTime LastTimeUpdateCaddyRank = DateTime.Now;
    public static Scene _marryScene;
    public static Scene _hotSpringScene;
    public static DateTime EntertamentModeEndDate;
    public static bool EntertamentModeOpen;
    public static DateTime EntertamentModeStartDate;
    public static bool FirstEntertamentModeUpdate;
    private static object object_0 = new object();
    private static RSACryptoServiceProvider rsacryptoServiceProvider_0;
    private static string[] string_0 = new string[17]
    {
      "gunny",
      "gun",
      "gunn",
      "g u n n y",
      "g unny",
      "g u nny",
      "g u n ny",
      "g un",
      "g u n",
      "com",
      "c om",
      "c o m",
      "net",
      "n et",
      "n e t",
      "ᶰ",
      "¥"
    };

    public static Scene MarryScene => WorldMgr._marryScene;

    public static Scene HotSpringScene => WorldMgr._hotSpringScene;

    public static RSACryptoServiceProvider RsaCryptor => WorldMgr.rsacryptoServiceProvider_0;

    public static bool Init()
    {
      bool flag = false;
      try
      {
        WorldMgr.rsacryptoServiceProvider_0 = new RSACryptoServiceProvider();
        WorldMgr.rsacryptoServiceProvider_0.FromXmlString(GameServer.Instance.Configuration.PrivateKey);
        WorldMgr.dictionary_0.Clear();
        using (ServiceBussiness serviceBussiness = new ServiceBussiness())
        {
          ServerInfo serviceSingle = serviceBussiness.GetServiceSingle(GameServer.Instance.Configuration.ServerID);
          if (serviceSingle != null)
          {
            WorldMgr._marryScene = new Scene(serviceSingle);
            WorldMgr._hotSpringScene = new Scene(serviceSingle);
            flag = true;
          }
        }
        EntertamentModeOpen = false;
        FirstEntertamentModeUpdate = false;
        Dictionary<int, EdictumInfo> dictionary = WorldMgr.smethod_0();
        if (dictionary.Values.Count > 0)
          Interlocked.Exchange<Dictionary<int, EdictumInfo>>(ref WorldMgr.dictionary_2, dictionary);
        WorldMgr.UpdateCaddyRank();
        WorldMgr.smethod_2();
      }
      catch (Exception ex)
      {
        WorldMgr.ilog_0.Error((object) "WordMgr Init", ex);
      }
      return flag;
    }

    public static bool ReloadEdictum()
    {
      bool flag = false;
      try
      {
        Dictionary<int, EdictumInfo> dictionary = WorldMgr.smethod_0();
        if (dictionary.Values.Count > 0)
          Interlocked.Exchange<Dictionary<int, EdictumInfo>>(ref WorldMgr.dictionary_2, dictionary);
        flag = true;
      }
      catch (Exception ex)
      {
        WorldMgr.ilog_0.Error((object) "WordMgr ReloadEdictum Init", ex);
      }
      return flag;
    }

    private static Dictionary<int, EdictumInfo> smethod_0()
    {
      Dictionary<int, EdictumInfo> dictionary = new Dictionary<int, EdictumInfo>();
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        foreach (EdictumInfo edictumInfo in produceBussiness.GetAllEdictum())
        {
          if (!dictionary.ContainsKey(edictumInfo.ID))
            dictionary.Add(edictumInfo.ID, edictumInfo);
        }
      }
      return dictionary;
    }

    public static bool AddPlayer(int playerId, GamePlayer player)
    {
      WorldMgr.readerWriterLock_0.AcquireWriterLock(-1);
      try
      {
        if (WorldMgr.dictionary_0.ContainsKey(playerId))
          return false;
        WorldMgr.dictionary_0.Add(playerId, player);
      }
      finally
      {
        WorldMgr.readerWriterLock_0.ReleaseWriterLock();
      }
      return true;
    }

    public static bool RemovePlayer(int playerId)
    {
      WorldMgr.readerWriterLock_0.AcquireWriterLock(-1);
      GamePlayer gamePlayer = (GamePlayer) null;
      try
      {
        if (WorldMgr.dictionary_0.ContainsKey(playerId))
        {
          gamePlayer = WorldMgr.dictionary_0[playerId];
          WorldMgr.dictionary_0.Remove(playerId);
        }
      }
      finally
      {
        WorldMgr.readerWriterLock_0.ReleaseWriterLock();
      }
      if (gamePlayer == null)
        return false;
      GameServer.Instance.LoginServer.SendUserOffline(playerId, gamePlayer.PlayerCharacter.ConsortiaID);
      return true;
    }

    public static GamePlayer GetPlayerById(int playerId)
    {
      GamePlayer gamePlayer = (GamePlayer) null;
      WorldMgr.readerWriterLock_0.AcquireReaderLock(-1);
      try
      {
        if (WorldMgr.dictionary_0.ContainsKey(playerId))
          gamePlayer = WorldMgr.dictionary_0[playerId];
      }
      finally
      {
        WorldMgr.readerWriterLock_0.ReleaseReaderLock();
      }
      return gamePlayer;
    }

    public static GamePlayer GetClientByPlayerNickName(string nickName)
    {
      foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
      {
        if (allPlayer.PlayerCharacter.NickName == nickName)
          return allPlayer;
      }
      return (GamePlayer) null;
    }

    public static GamePlayer GetClientByPlayerUserName(string userName)
    {
      foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
      {
        if (allPlayer.PlayerCharacter.UserName == userName)
          return allPlayer;
      }
      return (GamePlayer) null;
    }

        public static void ChatEntertamentModeGetPoint(PlayerInfo p)
        {
            GSPacketIn packet = new GSPacketIn(0xbd);
            packet.WriteInt(p.ID);
            packet.WriteString(p.NickName);
            GameServer.Instance.LoginServer.SendPacket(packet);
        }

        public static void ChatEntertamentModeOpenOrClose()
        {
            GSPacketIn packet = new GSPacketIn(0xc0);
            GameServer.Instance.LoginServer.SendPacket(packet);
        }

        public static void ChatEntertamentModeUpdatePoint(PlayerInfo p, int point)
        {
            GSPacketIn packet = new GSPacketIn(190);
            packet.WriteInt(p.ID);
            packet.WriteString(p.NickName);
            packet.WriteInt(point);
            GameServer.Instance.LoginServer.SendPacket(packet);
        }

        public static GamePlayer[] GetAllPlayers()
    {
      List<GamePlayer> gamePlayerList = new List<GamePlayer>();
      WorldMgr.readerWriterLock_0.AcquireReaderLock(-1);
      try
      {
        foreach (GamePlayer gamePlayer in WorldMgr.dictionary_0.Values)
        {
          if (gamePlayer != null && gamePlayer.PlayerCharacter != null)
            gamePlayerList.Add(gamePlayer);
        }
      }
      finally
      {
        WorldMgr.readerWriterLock_0.ReleaseReaderLock();
      }
      return gamePlayerList.ToArray();
    }

    public static string smethod_1(GamePlayer p) => p.Client.Socket.RemoteEndPoint is IPEndPoint remoteEndPoint ? remoteEndPoint.Address.ToString() : (string) null;

        public static bool IsAccountLimit(GamePlayer p)
        {
            if (p.Client == null)
            {
                return false;
            }
            bool result = false;
            string hWID = p.Client.HWID;
            if (hWID != null && hWID.Length > 0)
            {
                GamePlayer[] allPlayerWithHWID = GetAllPlayerWithHWID(hWID);
                if (allPlayerWithHWID.Length > 3)
                {
                    p.BlockReceiveMoney = true;
                    p.Out.SendMessage(eMessageType.ALERT, LanguageMgr.GetTranslation("GameServer.LimitIPConnentLauncher.Msg"));
                    p.Disconnect();
                    string text = "";
                    GamePlayer[] array = allPlayerWithHWID;
                    foreach (GamePlayer gamePlayer in array)
                    {
                        text = text + gamePlayer.PlayerCharacter.UserName + "(" + gamePlayer.PlayerCharacter.NickName + ").";
                    }
                    p.AddLog("OnlineMore", "LAUNCHER Username: " + p.PlayerCharacter.UserName + "|NickName: " + p.PlayerCharacter.NickName + "|ListAccountSameIP: " + text);
                    result = true;
                }
            }
            else
            {
                string text2 = smethod_1(p);
                if (text2 != null)
                {
                    GamePlayer[] allPlayerWithIP = GetAllPlayerWithIP(text2);
                    if (allPlayerWithIP.Length > 3)
                    {
                        foreach (GamePlayer psingle in allPlayerWithIP)
                        {
                            psingle.BlockReceiveMoney = true;
                            psingle.Out.SendMessage(eMessageType.ALERT, LanguageMgr.GetTranslation("GameServer.LimitIPConnent.Msg"));
                           // psingle.Disconnect();
                        }
                        string text3 = "";
                        GamePlayer[] array = allPlayerWithIP;
                        foreach (GamePlayer gamePlayer2 in array)
                        {
                            text3 = text3 + gamePlayer2.PlayerCharacter.UserName + "(" + gamePlayer2.PlayerCharacter.NickName + ").";
                        }
                        p.AddLog("OnlineMore", "Username: " + p.PlayerCharacter.UserName + "|NickName: " + p.PlayerCharacter.NickName + "|ListAccountSameIP: " + text3);
                        result = true;
                    }
                }
            }
            return result;
        }

        public static GamePlayer[] GetAllPlayerWithHWID(string hwid)
    {
      List<GamePlayer> gamePlayerList = new List<GamePlayer>();
      WorldMgr.readerWriterLock_0.AcquireReaderLock(-1);
      try
      {
        foreach (GamePlayer gamePlayer in WorldMgr.dictionary_0.Values)
        {
          if (gamePlayer != null && gamePlayer.PlayerCharacter != null && (gamePlayer.Client != null && gamePlayer.Client.IsConnected) && (gamePlayer.Client.HWID != null && gamePlayer.Client.HWID.Length > 0 && gamePlayer.Client.HWID == hwid))
            gamePlayerList.Add(gamePlayer);
        }
      }
      finally
      {
        WorldMgr.readerWriterLock_0.ReleaseReaderLock();
      }
      return gamePlayerList.ToArray();
    }

    public static GamePlayer[] GetAllPlayerWithIP(string ip)
    {
      List<GamePlayer> gamePlayerList = new List<GamePlayer>();
      WorldMgr.readerWriterLock_0.AcquireReaderLock(-1);
      try
      {
        foreach (GamePlayer gamePlayer in WorldMgr.dictionary_0.Values)
        {
          if (gamePlayer != null && gamePlayer.PlayerCharacter != null && (gamePlayer.Client != null && gamePlayer.Client.Socket != null) && (gamePlayer.Client.IsConnected && gamePlayer.Client.Socket.RemoteEndPoint is IPEndPoint remoteEndPoint && remoteEndPoint.Address.ToString().Contains(ip)))
            gamePlayerList.Add(gamePlayer);
        }
      }
      finally
      {
        WorldMgr.readerWriterLock_0.ReleaseReaderLock();
      }
      return gamePlayerList.ToArray();
    }

    public static GamePlayer[] GetAllPlayersNoGame()
    {
      List<GamePlayer> gamePlayerList = new List<GamePlayer>();
      WorldMgr.readerWriterLock_0.AcquireReaderLock(-1);
      try
      {
        foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
        {
          if (allPlayer.CurrentRoom == null)
            gamePlayerList.Add(allPlayer);
        }
      }
      finally
      {
        WorldMgr.readerWriterLock_0.ReleaseReaderLock();
      }
      return gamePlayerList.ToArray();
    }

    public static GamePlayer GetSinglePlayerWithConsortia(int ConsortiaID)
    {
      GamePlayer gamePlayer1 = (GamePlayer) null;
      WorldMgr.readerWriterLock_0.AcquireReaderLock(-1);
      try
      {
        foreach (GamePlayer gamePlayer2 in WorldMgr.dictionary_0.Values)
        {
          if (gamePlayer2 != null && gamePlayer2.PlayerCharacter != null && gamePlayer2.PlayerCharacter.ConsortiaID == ConsortiaID)
            gamePlayer1 = gamePlayer2;
        }
      }
      finally
      {
        WorldMgr.readerWriterLock_0.ReleaseReaderLock();
      }
      return gamePlayer1;
    }

    public static GamePlayer[] GetAllPlayersWithConsortia(int ConsortiaID)
    {
      List<GamePlayer> gamePlayerList = new List<GamePlayer>();
      WorldMgr.readerWriterLock_0.AcquireReaderLock(-1);
      try
      {
        foreach (GamePlayer gamePlayer in WorldMgr.dictionary_0.Values)
        {
          if (gamePlayer != null && gamePlayer.PlayerCharacter != null && gamePlayer.PlayerCharacter.ConsortiaID == ConsortiaID)
            gamePlayerList.Add(gamePlayer);
        }
      }
      finally
      {
        WorldMgr.readerWriterLock_0.ReleaseReaderLock();
      }
      return gamePlayerList.ToArray();
    }

    public static string GetPlayerStringByPlayerNickName(string nickName)
    {
      foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
      {
        if (allPlayer.PlayerCharacter.NickName == nickName)
          return allPlayer.ToString();
      }
      return nickName + " is not online!";
    }
        public static List<ItemInfo> CreateRandomPropItem()
        {
            List<ItemInfo> result;
            lock (WorldMgr.object_0)
            {
                List<ItemInfo> list = new List<ItemInfo>();
                List<int> list2 = new List<int>
                {
                    10455,
                    10459,
                    10461,
                    10450,
                    10451,
                    10452,
                    10453,
                    10454,
                    10456,
                    10457,
                    10458,
                    10460,
                    10462,
                    10463,
                    10464,
                    10465,
                    10466,
                    10470
                };
                Random random = new System.Random();
                for (int i = 0; i < 3; i++)
                {
                    int index = random.Next(0, list2.Count - 1);
                    ItemInfo itemInfo = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(list2[index]), 1, 105);
                    if (itemInfo != null)
                    {
                        list.Add(itemInfo);
                    }
                    list2.RemoveAt(index);
                }
                result = list;
            }
            return result;
        }
        public static string DisconnectPlayerByName(string nickName)
    {
      foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
      {
        if (allPlayer.PlayerCharacter.NickName == nickName)
        {
          allPlayer.Disconnect();
          return "OK";
        }
      }
      return nickName + " is not online!";
    }

    public static void OnPlayerOffline(int playerid, int consortiaID) => WorldMgr.ChangePlayerState(playerid, 0, consortiaID);

    public static void OnPlayerOnline(int playerid, int consortiaID) => WorldMgr.ChangePlayerState(playerid, 1, consortiaID);

    public static void ChangePlayerState(int playerID, int state, int consortiaID)
    {
      GSPacketIn pkg = (GSPacketIn) null;
      foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
      {
        if (allPlayer.Friends != null && allPlayer.Friends.ContainsKey(playerID) && allPlayer.Friends[playerID] == 0 || allPlayer.PlayerCharacter.ConsortiaID != 0 && allPlayer.PlayerCharacter.ConsortiaID == consortiaID)
        {
          if (pkg == null)
            pkg = allPlayer.Out.SendFriendState(playerID, state, allPlayer.PlayerCharacter.typeVIP, allPlayer.PlayerCharacter.VIPLevel);
          else
            allPlayer.SendTCP(pkg);
        }
      }
    }

    public static void UpdateExitGame(int userId)
    {
      lock (WorldMgr.dictionary_4)
      {
        if (WorldMgr.dictionary_4.ContainsKey(userId))
        {
          if (WorldMgr.dictionary_4[userId].TotalExitTime <= 3)
            ++WorldMgr.dictionary_4[userId].TotalExitTime;
          else
            WorldMgr.dictionary_4[userId].TimeBlock = DateTime.Now.AddMinutes(30.0);
          WorldMgr.dictionary_4[userId].LastLogout = DateTime.Now;
        }
        else
          WorldMgr.dictionary_4.Add(userId, new UserExitRoomLogInfo()
          {
            UserID = userId,
            TimeBlock = DateTime.MinValue,
            TotalExitTime = 1,
            LastLogout = DateTime.Now
          });
      }
    }

    public static DateTime CheckTimeEnterRoom(int userid)
    {
      lock (WorldMgr.dictionary_4)
      {
        if (WorldMgr.dictionary_4.ContainsKey(userid))
        {
          if (WorldMgr.dictionary_4[userid].TimeBlock > DateTime.Now)
            return WorldMgr.dictionary_4[userid].TimeBlock;
          if (WorldMgr.dictionary_4[userid].TotalExitTime > 3)
            WorldMgr.dictionary_4[userid].TotalExitTime = 0;
        }
        return DateTime.MinValue;
      }
    }

    public static bool UpdateShopFreeCount(int shopId, int total)
    {
      bool flag = false;
      lock (WorldMgr.dictionary_3)
      {
        if (WorldMgr.dictionary_3.ContainsKey(shopId))
        {
          if (WorldMgr.dictionary_3[shopId].Count > 0)
          {
            --WorldMgr.dictionary_3[shopId].Count;
            flag = true;
          }
        }
        else
        {
          WorldMgr.dictionary_3.Add(shopId, new ShopFreeCountInfo()
          {
            ShopID = shopId,
            Count = total - 1,
            CreateDate = DateTime.Now
          });
          flag = true;
        }
      }
      return flag;
    }

    private static void smethod_2()
    {
      WorldMgrDataInfo worldMgrDataInfo = Marshal.LoadDataFile<WorldMgrDataInfo>("shopfreecount", true);
      if (worldMgrDataInfo == null || worldMgrDataInfo.ShopFreeCount == null)
        return;
      WorldMgr.dictionary_3 = worldMgrDataInfo.ShopFreeCount;
    }

    private static void smethod_3() => Marshal.SaveDataFile<WorldMgrDataInfo>(new WorldMgrDataInfo()
    {
      ShopFreeCount = WorldMgr.dictionary_3
    }, "shopfreecount", true);

    public static void ScanShopFreeVaildDate()
    {
      lock (WorldMgr.dictionary_3)
      {
        bool flag = false;
        foreach (ShopFreeCountInfo shopFreeCountInfo in WorldMgr.dictionary_3.Values)
        {
          DateTime dateTime = shopFreeCountInfo.CreateDate;
          DateTime date1 = dateTime.Date;
          dateTime = DateTime.Now;
          DateTime date2 = dateTime.Date;
          if (date1 != date2)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          return;
        WorldMgr.dictionary_3.Clear();
      }
    }

    public static List<ShopFreeCountInfo> GetAllShopFreeCount()
    {
      List<ShopFreeCountInfo> shopFreeCountInfoList = new List<ShopFreeCountInfo>();
      lock (WorldMgr.dictionary_3)
      {
        foreach (ShopFreeCountInfo shopFreeCountInfo in WorldMgr.dictionary_3.Values)
          shopFreeCountInfoList.Add(shopFreeCountInfo);
      }
      return shopFreeCountInfoList;
    }
        public static GSPacketIn SendSysNotice(eMessageType type, string msg, int ItemID, int TemplateID, string key, int zoneID)
        {
            int val = msg.IndexOf("@");
            GSPacketIn gSPacketIn = new GSPacketIn(10);
            gSPacketIn.WriteInt((int)type);
            gSPacketIn.WriteString(msg.Replace("@", ""));
            if (type == eMessageType.CROSS_NOTICE)
            {
                gSPacketIn.WriteInt(zoneID);
            }
            if (ItemID > 0)
            {
                gSPacketIn.WriteByte(1);
                gSPacketIn.WriteInt(val);
                gSPacketIn.WriteInt(TemplateID);
                gSPacketIn.WriteInt(ItemID);
                gSPacketIn.WriteString(key);
            }
            WorldMgr.SendToAll(gSPacketIn);
            return gSPacketIn;
        }
        public static GSPacketIn SendSysNotice(
      eMessageType type,
      string msg,
      int ItemID,
      int TemplateID,
      string key)
    {
      int val = msg.IndexOf(TemplateID.ToString(), StringComparison.Ordinal);
      GSPacketIn pkg = new GSPacketIn((short) 10);
      pkg.WriteInt((int) type);
      pkg.WriteString(msg.Replace(TemplateID.ToString(), ""));
      pkg.WriteByte((byte) 1);
      pkg.WriteInt(val);
      pkg.WriteInt(TemplateID);
      pkg.WriteInt(ItemID);
      if (!string.IsNullOrEmpty(key))
        pkg.WriteString(key);
      WorldMgr.SendToAll(pkg);
      return pkg;
    }

    public static GSPacketIn SendSysTipNotice(string msg)
    {
      GSPacketIn pkg = new GSPacketIn((short) 10);
      pkg.WriteInt(2);
      pkg.WriteString(msg);
      WorldMgr.SendToAll(pkg);
      return pkg;
    }

    public static GSPacketIn SendSysNotice(string msg)
    {
      GSPacketIn pkg = new GSPacketIn((short) 10);
      pkg.WriteInt(3);
      pkg.WriteString(msg);
      WorldMgr.SendToAll(pkg);
      return pkg;
    }

        public static void SendOpenEntertainmentMode(GamePlayer p)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(145);
            gSPacketIn.WriteByte(71);
            gSPacketIn.WriteBoolean(WorldMgr.EntertamentModeOpen);
            gSPacketIn.WriteDateTime(WorldMgr.EntertamentModeStartDate);
            gSPacketIn.WriteDateTime(WorldMgr.EntertamentModeEndDate);
            if (p == null)
            {
                WorldMgr.SendToAll(gSPacketIn);
            }
            else
            {
                p.SendTCP(gSPacketIn);
            }
        }
        public static void SendSysNotice(string msg, int consortiaId)
    {
      foreach (GamePlayer playersWithConsortium in WorldMgr.GetAllPlayersWithConsortia(consortiaId))
      {
        GSPacketIn gsPacketIn = new GSPacketIn((short) 10);
        gsPacketIn.WriteInt(3);
        gsPacketIn.WriteString(msg);
        GSPacketIn pkg = gsPacketIn;
        playersWithConsortium.SendTCP(pkg);
      }
    }

    public static GSPacketIn SendSysNotice(
      eMessageType type,
      string msg,
      List<SqlDataProvider.Data.ItemInfo> items,
      int zoneID)
    {
      List<int> intList = WorldMgr.smethod_4(msg, "@");
      GSPacketIn pkg = (GSPacketIn) null;
      if (intList.Count == items.Count)
      {
        pkg = new GSPacketIn((short) 10);
        pkg.WriteInt((int) type);
        pkg.WriteString(msg.Replace("@", ""));
        if (type == eMessageType.CROSS_NOTICE)
          pkg.WriteInt(zoneID);
        int index = 0;
        pkg.WriteByte((byte) intList.Count);
        foreach (int val in intList)
        {
          SqlDataProvider.Data.ItemInfo itemInfo = items[index];
          pkg.WriteInt(val);
          pkg.WriteInt(itemInfo.TemplateID);
          pkg.WriteInt(itemInfo.ItemID);
          pkg.WriteString("");
          ++index;
        }
        WorldMgr.SendToAll(pkg);
      }
      else
        WorldMgr.ilog_0.Error((object) ("wrong msg: " + msg + ": itemcount: " + (object) items.Count));
      return pkg;
    }

    private static List<int> smethod_4(string string_1, string string_2)
    {
      List<int> intList = new List<int>();
      int length = string_2.Length;
      int num = -length;
      while (true)
      {
        num = string_1.IndexOf(string_2, num + length);
        if (num != -1)
          intList.Add(num);
        else
          break;
      }
      return intList;
    }

    public static void UpdateCaddyRank()
    {
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        UsersExtraInfo[] rankCaddy = playerBussiness.GetRankCaddy();
        WorldMgr.CaddyRank = new Dictionary<int, UsersExtraInfo>();
        foreach (UsersExtraInfo usersExtraInfo in rankCaddy)
        {
          if (!WorldMgr.CaddyRank.ContainsKey(usersExtraInfo.UserID))
            WorldMgr.CaddyRank.Add(usersExtraInfo.UserID, usersExtraInfo);
        }
        WorldMgr.LastTimeUpdateCaddyRank = DateTime.Now;
      }
    }

    public static void AddAreaConfig(AreaConfigInfo[] Areas)
    {
      foreach (AreaConfigInfo area in Areas)
      {
        if (!WorldMgr.dictionary_1.ContainsKey(area.AreaID))
        {
          if (area.AreaID == GameServer.Instance.Configuration.ZoneId)
            WorldMgr.areaConfigInfo_0 = area;
          WorldMgr.dictionary_1.Add(area.AreaID, area);
        }
      }
    }

    public static AreaConfigInfo FindAreaConfig(int zoneId)
    {
      WorldMgr.readerWriterLock_0.AcquireWriterLock(-1);
      try
      {
        if (WorldMgr.dictionary_1.ContainsKey(zoneId))
          return WorldMgr.dictionary_1[zoneId];
      }
      finally
      {
        WorldMgr.readerWriterLock_0.ReleaseWriterLock();
      }
      return (AreaConfigInfo) null;
    }

    public static AreaConfigInfo[] GetAllAreaConfig()
    {
      List<AreaConfigInfo> areaConfigInfoList = new List<AreaConfigInfo>();
      foreach (AreaConfigInfo areaConfigInfo in WorldMgr.dictionary_1.Values)
        areaConfigInfoList.Add(areaConfigInfo);
      return areaConfigInfoList.ToArray();
    }

    public static EdictumInfo[] GetAllEdictumVersion()
    {
      List<EdictumInfo> edictumInfoList = new List<EdictumInfo>();
      foreach (EdictumInfo edictumInfo in WorldMgr.dictionary_2.Values)
      {
        DateTime dateTime = edictumInfo.EndDate;
        DateTime date1 = dateTime.Date;
        dateTime = DateTime.Now;
        DateTime date2 = dateTime.Date;
        if (date1 > date2)
          edictumInfoList.Add(edictumInfo);
      }
      return edictumInfoList.ToArray();
    }

    public static void SendToAll(GSPacketIn pkg)
    {
      foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
        allPlayer.SendTCP(pkg);
    }

    public static bool CheckBadWord(string msg)
    {
      foreach (string str in WorldMgr.string_0)
      {
        if (msg.ToLower().Contains(str.ToLower()))
          return true;
      }
      return false;
    }

        public static GSPacketIn SendStaffHelp(string msg)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(10);
            gSPacketIn.WriteInt(2);
            gSPacketIn.WriteString(msg);
            return gSPacketIn;
        }
        public static void Stop() => WorldMgr.smethod_3();
  }
}
