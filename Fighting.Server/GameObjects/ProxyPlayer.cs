﻿// Decompiled with JetBrains decompiler
// Type: Fighting.Server.GameObjects.ProxyPlayer
// Assembly: Fighting.Server, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DD84ADA-AC24-47D6-83C2-9107959F6D72
// Assembly location: C:\Users\Administrador.OMINIHOST\Downloads\DDBTank4.1\Emulador\Fight\Fighting.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Logic;
using Game.Logic.Phy.Object;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;

namespace Fighting.Server.GameObjects
{
    public class ProxyPlayer : IGamePlayer
    {
        public List<BufferInfo> Buffers;
        private double GPRate;
        public double m_antiAddictionRate;
        private double m_baseAglilty;
        private double m_baseAttack;
        private double m_baseBlood;
        private double m_baseDefence;
        private bool m_canUseProp;
        private bool m_canX2Exp;
        private bool m_canX3Exp;
        private ServerClient m_client;
        public string FightFootballStyle;
        private SqlDataProvider.Data.ItemInfo m_currentWeapon;
        private PlayerInfo m_character;
        private List<int> m_equipEffect;
        private List<BufferInfo> m_fightBuffs;
        private int m_gamePlayerId;
        private SqlDataProvider.Data.ItemInfo m_healstone;
        private UserMatchInfo m_matchInfo;
        private UsersPetInfo m_pet;
        private SqlDataProvider.Data.ItemInfo m_secondWeapon;
        public int m_serverid;
        private int m_zoneId;
        private string m_zoneName;
        private double OfferRate;
        private ServerClient serverClient;
        private PlayerInfo info;
        private ItemTemplateInfo itemTemplate;
        private SqlDataProvider.Data.ItemInfo item;
        private double baseAttack;
        private double baseDefence;
        private double baseAgility;
        private double baseBlood;
        private double gprate;
        private double offerrate;
        private double rate;
        private List<BufferInfo> infos;
        private int serverid;
        private int currentEnemyId;
        private int int_1;

        public ProxyPlayer(
          ServerClient client,
          PlayerInfo character,
          UsersPetInfo pet,
          List<BufferInfo> buffers,
          List<int> equipEffect,
          List<BufferInfo> fightBuffer,
          ProxyPlayerInfo proxyPlayer,
          UserMatchInfo matchInfo)
        {
            this.m_client = client;
            this.m_character = character;
            this.m_serverid = proxyPlayer.ServerId;
            this.m_baseAttack = proxyPlayer.BaseAttack;
            this.m_baseDefence = proxyPlayer.BaseDefence;
            this.m_baseAglilty = proxyPlayer.BaseAgility;
            this.m_baseBlood = proxyPlayer.BaseBlood;
            this.m_currentWeapon = proxyPlayer.GetItemTemplateInfo();
            this.m_secondWeapon = proxyPlayer.GetItemInfo();
            this.m_healstone = proxyPlayer.GetHealstone();
            this.m_equipEffect = equipEffect;
            this.m_fightBuffs = fightBuffer;
            this.m_matchInfo = matchInfo;
            this.OfferRate = proxyPlayer.OfferAddPlus;
            this.GPRate = proxyPlayer.GPAddPlus;
            this.Buffers = buffers;
            this.m_zoneId = proxyPlayer.ZoneId;
            this.m_zoneName = proxyPlayer.ZoneName;
            this.m_pet = pet;
            this.m_antiAddictionRate = proxyPlayer.AntiAddictionRate;
        }

        public ProxyPlayer(
          ServerClient serverClient,
          PlayerInfo info,
          ItemTemplateInfo itemTemplate,
          SqlDataProvider.Data.ItemInfo item,
          double baseAttack,
          double baseDefence,
          double baseAgility,
          double baseBlood,
          double gprate,
          double offerrate,
          double rate,
          List<BufferInfo> infos,
          int serverid)
        {
            this.serverClient = serverClient;
            this.info = info;
            this.itemTemplate = itemTemplate;
            this.item = item;
            this.baseAttack = baseAttack;
            this.baseDefence = baseDefence;
            this.baseAgility = baseAgility;
            this.baseBlood = baseBlood;
            this.gprate = gprate;
            this.offerrate = offerrate;
            this.rate = rate;
            this.infos = infos;
            this.serverid = serverid;
        }

        public int CurrentEnemyId
        {
            get => this.currentEnemyId;
            set => this.currentEnemyId = value;
        }

        public long WorldbossBood
        {
            get
            {
                return 0L;
            }
        }
        public int AddRichesOffer(int value) => value;

        public int AddGold(int value)
        {
            if (value > 0)
                this.m_client.SendPlayerAddGold(this.PlayerCharacter.ID, value);
            return value;
        }

        public int AddGP(int gp)
        {
            if (gp > 0)
                this.m_client.SendPlayerAddGP(this.PlayerCharacter.ID, gp);
            return (int)(this.GPRate * (double)gp);
        }

        public int AddGiftToken(int value)
        {
            if (value > 0)
                this.m_client.SendPlayerAddGiftToken(this.m_character.ID, value);
            return value;
        }

        public int AddHardCurrency(int value) => 0;

        public void PVEFightMessage(string translation, SqlDataProvider.Data.ItemInfo itemInfo, int areaID)
        {
        }

        public bool UsePayBuff(BuffType type) => true;

        public int AddMoney(int value)
        {
            if (value > 0)
                this.m_client.SendPlayerAddMoney(this.m_character.ID, value, true);
            return value;
        }

        public int AddHonor(int value)
        {
            return value;
        }

        public int AddDamageScores(int value)
        {
            return value;
        }
        public int AddEliteScore(int value)
        {
            if (value > 0)
                this.m_client.SendAddEliteGameScore(this.PlayerCharacter.ID, value);
            return value;
        }

        public int RemoveEliteScore(int value)
        {
            if (value > 0)
                this.m_client.SendRemoveEliteGameScore(this.PlayerCharacter.ID, value);
            return value;
        }

        public void SendWinEliteChampion() => this.m_client.SendEliteGameWinUpdate(this.PlayerCharacter.ID);

        public int AddOffer(int value)
        {
            int num;
            if (value > 0)
            {
                this.m_client.SendPlayerAddOffer(this.m_character.ID, value);
                num = value;
            }
            else
            {
                this.RemoveOffer(Math.Abs(value));
                num = value;
            }
            return num;
        }

        public string ProcessLabyrinthAward { get; set; }

        public void UpdateLabyrinth(int currentFloor, int m_missionInfoId, bool bigAward)
        {
        }

        public void OutLabyrinth(bool iswin)
        {
        }

        public bool AddTemplate(SqlDataProvider.Data.ItemInfo cloneItem, eBageType bagType, int count, eGameView typeGet)
        {
            this.m_client.SendPlayerAddTemplate(this.m_character.ID, cloneItem, bagType, count);
            return true;
        }

        public bool ClearFightBag() => true;

        public void ClearFightBuffOneMatch()
        {
        }

        public int GameId
        {
            get => this.int_1;
            set
            {
                this.int_1 = value;
                //this.m_client.SendGamePlayerId((IGamePlayer)this); BUG
            }
        }

        public bool ClearTempBag() => true;

        public int ConsortiaFight(
          int consortiaWin,
          int consortiaLose,
          Dictionary<int, Player> players,
          eRoomType roomType,
          eGameType gameClass,
          int totalKillHealth,
          int count)
        {
            this.m_client.SendPlayerConsortiaFight(this.m_character.ID, consortiaWin, consortiaLose, players, roomType, gameClass, totalKillHealth);
            return 0;
        }

        public void Disconnect() => this.m_client.SendDisconnectPlayer(this.m_character.ID);

        public double GetBaseAgility() => this.m_baseAglilty;

        public double GetBaseAttack() => this.m_baseAttack;

        public double GetBaseBlood() => this.m_baseBlood;

        public double GetBaseDefence() => this.m_baseDefence;

        public bool isDoubleAward() => false;

        public bool IsPvePermission(int missionId, eHardLevel hardLevel) => true;

        public void LogAddMoney(
          AddMoneyType masterType,
          AddMoneyType sonType,
          int userId,
          int moneys,
          int SpareMoney)
        {
        }

        public bool MissionEnergyEmpty(int value) => false;

        public void OnGameOver(
          AbstractGame game,
          bool isWin,
          int gainXp,
          bool isSpanArea,
          bool isCouple,
          int blood,
          int playerCount)
        {
            this.m_client.SendPlayerOnGameOver(this.PlayerCharacter.ID, game.Id, isWin, gainXp, isSpanArea, isCouple, blood, playerCount);
        }

        public void OnKillingBoss(AbstractGame game, NpcInfo npc, int damage) => throw new NotImplementedException();

        public void OnKillingLiving(AbstractGame game, int type, int id, bool isLiving, int demage) => this.m_client.SendPlayerOnKillingLiving(this.m_character.ID, game, type, id, isLiving, demage);

        public void OnMissionOver(AbstractGame game, bool isWin, int MissionID, int turnNum) => this.m_client.SendPlayerOnMissionOver(this.m_character.ID, game, isWin, MissionID, turnNum);

        public void OutLabyrinth()
        {
        }

        public int RemoveGold(int value)
        {
            this.m_client.SendPlayerRemoveGold(this.m_character.ID, value);
            return 0;
        }

        public int RemoveGP(int gp)
        {
            this.m_client.SendPlayerRemoveGP(this.PlayerCharacter.ID, gp);
            return gp;
        }

        public int RemoveGiftToken(int value) => 0;

        public bool RemoveHealstone()
        {
            this.m_client.SendPlayerRemoveHealstone(this.m_character.ID);
            return false;
        }

        public int RemoveMedal(int value) => 0;

        public bool RemoveMissionEnergy(int value) => false;

        public int RemoveMoney(int value)
        {
            this.m_client.SendPlayerRemoveMoney(this.m_character.ID, value);
            return 0;
        }

        public int RemoveOffer(int value)
        {
            this.m_client.SendPlayerRemoveOffer(this.m_character.ID, value);
            return value;
        }

        public void SendConsortiaFight(int consortiaID, int riches, string msg) => this.m_client.SendPlayerSendConsortiaFight(this.m_character.ID, consortiaID, riches, msg);

        public int AddRobRiches(int value)
        {
            int num;
            if (value > 0)
            {
                value = (int)(double)value;
                if (value > 100000)
                    Console.WriteLine(string.Format("ProxyPlayer ====== player.nickname : {0}, player.RichesRob : {1}, add rob riches: {2}", new object[3]
                    {
            (object) this.PlayerCharacter.NickName,
            (object) this.PlayerCharacter.RichesRob,
            (object) value
                    }));
                this.m_client.SendAddRobRiches(this.m_character.ID, value);
                num = value;
            }
            else
                num = 0;
            return num;
        }

        public void SendHideMessage(string msg)
        {
            GSPacketIn pkg = new GSPacketIn((short)3);
            pkg.WriteInt(3);
            pkg.WriteString(msg);
            this.SendTCP(pkg);
        }

        public void SendInsufficientMoney(int type)
        {
        }

        public void SendMessage(string msg)
        {
            GSPacketIn pkg = new GSPacketIn((short)3);
            pkg.WriteInt(0);
            pkg.WriteString(msg);
            this.SendTCP(pkg);
        }

        public string GetFightFootballStyle(int team)
        {
            if (team == 1)
            {
                return this.FightFootballStyle.Split(new char[] { ';' })[0];
            }
            return this.FightFootballStyle.Split(new char[] { ';' })[1];
        }

        public void SendTCP(GSPacketIn pkg) => this.m_client.SendPacketToPlayer(this.m_character.ID, pkg);

        public bool SetPvePermission(int missionId, eHardLevel hardLevel) => true;

        public void UpdateBarrier(int barrier, string pic)
        {
        }

        public void UpdatePveResult(string type, int value, bool isWin)
        {
        }

        public bool UseKingBlessHelpStraw(eRoomType roomType) => false;

        public bool UsePropItem(AbstractGame game, int bag, int place, int templateId, bool isLiving)
        {
            this.m_client.SendPlayerUsePropInGame(this.PlayerCharacter.ID, bag, place, templateId, isLiving);
            game.Pause(500);
            return false;
        }

        public long AllWorldDameBoss
        {
            get
            {
                return 0L;
            }
        }

        public bool CanUseProp
        {
            get => this.m_canUseProp;
            set => this.m_canUseProp = value;
        }

        public bool CanX2Exp
        {
            get => this.m_canX2Exp;
            set => this.m_canX2Exp = value;
        }

        public bool CanX3Exp
        {
            get => this.m_canX3Exp;
            set => this.m_canX3Exp = value;
        }

        public List<int> EquipEffect => this.m_equipEffect;

        public List<BufferInfo> FightBuffs => this.m_fightBuffs;

        public int GamePlayerId
        {
            get => this.m_gamePlayerId;
            set
            {
                this.m_gamePlayerId = value;
                //this.m_client.SendGamePlayerId((IGamePlayer)this); BUG
            }
        }

        public void UpdateGamePlayerId()
        {
			this.m_client.SendGamePlayerId((IGamePlayer)this);
		}

		public void OnGameOver(AbstractGame game, bool isWin, int gainXp)
        {
            //this.m_client.SendPlayerOnGameOver(this.PlayerCharacter.ID, game.Id, isWin, gainXp);
        }

        public int AddEnterModePoint(int value)
        {
            if (value > 0)
            {
                //this.m_client.SendPlayerAddEnterModePoint(this.m_character.ID, value);
            }
            return value;
        }

        public void CreateRandomEnterModeItem()
        {
            //this.m_client.SendCreateEnterModeItem(this.PlayerCharacter.ID);
        }
        public SqlDataProvider.Data.ItemInfo Healstone => this.m_healstone;

        public SqlDataProvider.Data.ItemInfo MainWeapon => this.m_currentWeapon;

        public UserMatchInfo MatchInfo => this.m_matchInfo;

        public UsersPetInfo Pet => this.m_pet;

        public PlayerInfo PlayerCharacter => this.m_character;

        public SqlDataProvider.Data.ItemInfo SecondWeapon => this.m_secondWeapon;

        public int ServerID
        {
            get => this.m_serverid;
            set => this.m_serverid = value;
        }

        public int ZoneId => this.m_zoneId;

        public string ZoneName => this.m_zoneName;

        public bool SetFightLabPermission(int missionId, eHardLevel hardLevel) => true;

        public bool IsFightLabPermission(int missionId, eHardLevel hardLevel) => true;

        public bool SetFightLabPermission(int copyId, eHardLevel hardLevel, int missionId) => true;

        public double GPAddPlus { get; set; }

        public double OfferAddPlus { get; set; }

        public double GPApprenticeOnline { get; set; }

        public double GPApprenticeTeam { get; set; }

        public double GPSpouseTeam { get; set; }

        public bool IsAccountLimit { get; set; }

        public void AddLog(string type, string content)
        {
        }
    }
}
