using Bussiness;
using Game.Logic;
using Game.Base.Packets;
using Game.Server.Battle;
using Game.Server.Games;
using Game.Server.Managers;
using Game.Server.Packets;
using Game.Server.RingStation;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;

namespace Game.Server.Rooms
{
    public class StartGameAction : IAction
    {
        private BaseRoom baseRoom_0;

        public StartGameAction(BaseRoom room) => this.baseRoom_0 = room;

        public void Execute()
        {
            if (!this.baseRoom_0.CanStart())
                return;
            List<GamePlayer> players1 = this.baseRoom_0.GetPlayers();
            if (this.baseRoom_0.RoomType == eRoomType.Freedom)
            {
                List<IGamePlayer> red = new List<IGamePlayer>();
                List<IGamePlayer> blue = new List<IGamePlayer>();
                foreach (GamePlayer gamePlayer in players1)
                {
                    if (gamePlayer != null)
                    {
                        if (gamePlayer.CurrentRoomTeam == 1)
                            red.Add((IGamePlayer)gamePlayer);
                        else
                            blue.Add((IGamePlayer)gamePlayer);
                        gamePlayer.PetBag.ReduceHunger();
                    }
                }
                this.method_0(GameMgr.StartPVPGame(this.baseRoom_0.RoomId, red, blue, this.baseRoom_0.MapId, this.baseRoom_0.RoomType, this.baseRoom_0.GameType, (int)this.baseRoom_0.TimeMode));
            }
            else if (this.baseRoom_0.IsPVE())
            {
                List<IGamePlayer> players2 = new List<IGamePlayer>();
                foreach (GamePlayer gamePlayer in players1)
                {
                    if (gamePlayer != null)
                        players2.Add((IGamePlayer)gamePlayer);
                }
                this.method_1();
                this.method_0(GameMgr.StartPVEGame(this.baseRoom_0.RoomId, players2, this.baseRoom_0.MapId, this.baseRoom_0.RoomType, this.baseRoom_0.GameType, (int)this.baseRoom_0.TimeMode, this.baseRoom_0.HardLevel, this.baseRoom_0.LevelLimits, this.baseRoom_0.currentFloor));
            }
            else if (this.baseRoom_0.RoomType == eRoomType.Match)
            {
                this.baseRoom_0.UpdateAvgLevel();
                this.method_1();
                foreach (GamePlayer player in this.baseRoom_0.GetPlayers())
                {
                    DateTime dateTime = WorldMgr.CheckTimeEnterRoom(player.PlayerId);
                    if (dateTime > DateTime.Now)
                    {
                        this.baseRoom_0.SendToAll(this.baseRoom_0.Host.Out.SendMessage(eMessageType.ChatERROR, string.Format("3'devido a mais de dez tentativas {0} impedido de entrar na sala. {1} então você pode entrar novamente.", (object)player.PlayerCharacter.NickName, (object)dateTime.ToShortTimeString())), this.baseRoom_0.Host);
                        this.baseRoom_0.SendCancelPickUp();
                        return;
                    }
                }
                if (this.baseRoom_0.GetPlayers().Count == 1 && this.baseRoom_0.Host != null && !this.baseRoom_0.isCrosszone)
                {
                    if (this.baseRoom_0.Host.PlayerCharacter.Grade <= 5)
                    {
                        this.baseRoom_0.StartWithNpc = true;
                        this.baseRoom_0.PickUpNpcId = RingStationMgr.GetAutoBot(this.baseRoom_0.Host, (int)this.baseRoom_0.RoomType, (int)this.baseRoom_0.GameType);
                        Console.WriteLine("GetAutoBot {0}", (object)this.baseRoom_0.PickUpNpcId);
                    }
                    else
                    {
                        this.baseRoom_0.PickUpNpcId = RingStationConfiguration.NextPlayerID();
                        Console.WriteLine("NextPlayerID {0}", (object)this.baseRoom_0.PickUpNpcId);
                    }
                }
                if (this.baseRoom_0.GetPlayers().Count == 2 && this.baseRoom_0.Host != null && !this.baseRoom_0.isCrosszone)
                {
                    if (this.baseRoom_0.Host.PlayerCharacter.Grade <= 5)
                    {
                        this.baseRoom_0.StartWithNpc = true;
                        this.baseRoom_0.PickUpNpcId = RingStationMgr.GetAutoBot(this.baseRoom_0.Host, (int)this.baseRoom_0.RoomType, (int)this.baseRoom_0.GameType);
                        Console.WriteLine("GetAutoBot {0}", (object)this.baseRoom_0.PickUpNpcId);
                    }
                    else
                    {
                        this.baseRoom_0.PickUpNpcId = RingStationConfiguration.NextPlayerID();
                        Console.WriteLine("NextPlayerID {0}", (object)this.baseRoom_0.PickUpNpcId);
                    }
                }
                BattleServer battleServer = BattleMgr.AddRoom(this.baseRoom_0);
                if (battleServer != null)
                {
                    this.baseRoom_0.BattleServer = battleServer;
                    this.baseRoom_0.IsPlaying = true;
                    this.baseRoom_0.SendStartPickUp();
                }
                else
                {
                    this.baseRoom_0.SendToAll(this.baseRoom_0.Host.Out.SendMessage(eMessageType.ChatERROR, LanguageMgr.GetTranslation("GameServer.FightBattle.NotReady.Msg")), this.baseRoom_0.Host);
                    this.baseRoom_0.SendCancelPickUp();
                }
            }
            else if (this.baseRoom_0.RoomType == eRoomType.EliteGameScore || this.baseRoom_0.RoomType == eRoomType.EliteGameChampion)
            {
                this.baseRoom_0.UpdateAvgLevel();
                if (this.baseRoom_0.GetPlayers().Count == 1)
                {
                    if (ExerciseMgr.IsBlockWeapon(this.baseRoom_0.Host.MainWeapon.TemplateID))
                    {
                        this.baseRoom_0.Host.SendMessage(" Você está usando a arma errada. Por favor, use outra arma.");
                        this.baseRoom_0.SendCancelPickUp();
                        return;
                    }
                    if (this.baseRoom_0.RoomType == eRoomType.EliteGameChampion)
                    {
                        EliteGameRoundInfo eliteRoundByUser = ExerciseMgr.FindEliteRoundByUser(this.baseRoom_0.Host.PlayerCharacter.ID);
                        if (eliteRoundByUser != null)
                        {
                            this.baseRoom_0.Host.CurrentEnemyId = eliteRoundByUser.PlayerOne.UserID == this.baseRoom_0.Host.PlayerCharacter.ID ? eliteRoundByUser.PlayerTwo.UserID : eliteRoundByUser.PlayerOne.UserID;
                        }
                        else
                        {
                            Console.WriteLine("/// Motor principal NÃO PERMITIDO AQUI: " + (object)this.baseRoom_0.Host.PlayerCharacter.ID);
                            this.baseRoom_0.Host.SendMessage("Você não está qualificado para participar do Torneio de Elite");
                            this.baseRoom_0.SendCancelPickUp();
                            return;
                        }
                    }
                    BattleServer battleServer = BattleMgr.AddRoom(this.baseRoom_0);
                    if (battleServer != null)
                    {
                        this.baseRoom_0.BattleServer = battleServer;
                        this.baseRoom_0.IsPlaying = true;
                        this.baseRoom_0.SendStartPickUp();
                        if (this.baseRoom_0.RoomType == eRoomType.EliteGameChampion)
                            GameServer.Instance.LoginServer.SendEliteChampionBattleStatus(this.baseRoom_0.Host.PlayerCharacter.ID, true);
                    }
                    else
                    {
                        this.baseRoom_0.SendToAll(this.baseRoom_0.Host.Out.SendMessage(eMessageType.ChatERROR, LanguageMgr.GetTranslation("GameServer.FightBattle.NotReady.Msg")), this.baseRoom_0.Host);
                        this.baseRoom_0.SendCancelPickUp();
                    }
                    if ((this.baseRoom_0.RoomType == eRoomType.EntertainmentRoom) || (this.baseRoom_0.RoomType == eRoomType.EntertainmentRoomPK))
                    {
                        this.baseRoom_0.UpdateAvgLevel();
                        BattleServer server2 = BattleMgr.AddRoom(this.baseRoom_0);
                        if (server2 != null)
                        {
                            this.baseRoom_0.BattleServer = server2;
                            this.baseRoom_0.IsPlaying = true;
                            this.baseRoom_0.SendStartPickUp();
                        }
                        else
                        {
                            GSPacketIn in2 = this.baseRoom_0.Host.Out.SendMessage(eMessageType.const_3, LanguageMgr.GetTranslation("StartGameAction.noBattleServe", new object[0]));
                            this.baseRoom_0.SendToAll(in2, this.baseRoom_0.Host);
                            this.baseRoom_0.SendCancelPickUp();
                        }
                    }
                }
            }
            RoomMgr.WaitingRoom.SendUpdateCurrentRoom(this.baseRoom_0);
        }

        private void method_0(BaseGame baseGame_0)
        {
            if (baseGame_0 != null)
            {
                this.baseRoom_0.IsPlaying = true;
                this.baseRoom_0.StartGame((AbstractGame)baseGame_0);
            }
            else
            {
                this.baseRoom_0.IsPlaying = false;
                this.baseRoom_0.SendPlayerState();
            }
        }
        // eu achei que aqui tava certo 
        private bool IsPVE(eRoomType roomType)
        {
            if (roomType <= eRoomType.ConsortiaBoss)
            {
                switch (roomType)
                {
                    case eRoomType.Dungeon:
                        break;
                    default:
                        switch (roomType)
                        {
                            case eRoomType.Entertainment:
                            case eRoomType.EntertainmentPK:
                                break;
                            case eRoomType.Freshman:
                            case eRoomType.AcademyDungeon:
                            case eRoomType.WordBossFight:
                            case eRoomType.Lanbyrinth:
                            case eRoomType.ConsortiaBoss:
                                break;
                            case eRoomType.ScoreLeage:
                            case eRoomType.GuildLeageRank:
                                return false;
                            default:
                                return false;
                        }
                        break;
                }
            }
            else
            {
                switch (roomType)
                {
                    case eRoomType.ActivityDungeon:
                    case eRoomType.SpecialActivityDungeon:
                        break;
                    case eRoomType.TransnationalFight:
                        return false;
                    default:
                        if (roomType != eRoomType.Christmas)
                        {
                            return false;
                        }
                        break;
                }
            }
            return true;
        }
        private void method_1()
        {
            if (this.baseRoom_0.IsPVE())
            {
                switch (this.baseRoom_0.HardLevel)
                {
                    case eHardLevel.Normal:
                        this.baseRoom_0.TimeMode = (byte)5;
                        break;
                    case eHardLevel.Hard:
                        this.baseRoom_0.TimeMode = (byte)4;
                        break;
                    case eHardLevel.Terror:
                        this.baseRoom_0.TimeMode = (byte)3;
                        break;
                    case eHardLevel.Simple:
                        this.baseRoom_0.TimeMode = (byte)6;
                        break;
                }
            }
            else
                this.baseRoom_0.TimeMode = (byte)5;
        }
    }
}
