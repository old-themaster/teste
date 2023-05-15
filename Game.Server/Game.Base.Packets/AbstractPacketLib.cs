using Bussiness;
using Bussiness.Managers;
using Game.Server;
using Game.Server.Buffer;
using Game.Server.ConsortiaTask;
using Game.Server.GameUtils;
using Game.Server.Managers;
using Game.Server.Packets;
using Game.Server.Quests;
using Game.Server.Rooms;
using Game.Server.SceneMarryRooms;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Game.Base.Packets
{
    [PacketLib(1)]
    public class AbstractPacketLib : IPacketLib
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected readonly GameClient m_gameClient;

        public AbstractPacketLib(GameClient client) => this.m_gameClient = client;

        public static IPacketLib CreatePacketLibForVersion(int rawVersion, GameClient client)
        {
            foreach (Type derivedClass in ScriptMgr.GetDerivedClasses(typeof(IPacketLib)))
            {
                foreach (PacketLibAttribute customAttribute in derivedClass.GetCustomAttributes(typeof(PacketLibAttribute), false))
                {
                    if (customAttribute.RawVersion == rawVersion)
                    {
                        try
                        {
                            return (IPacketLib)Activator.CreateInstance(derivedClass, (object)client);
                        }
                        catch (Exception ex)
                        {
                            if (AbstractPacketLib.log.IsErrorEnabled)
                                AbstractPacketLib.log.Error((object)("error creating packetlib (" + derivedClass.FullName + ") for raw version " + (object)rawVersion), ex);
                        }
                    }
                }
            }
            return (IPacketLib)null;
        }

        public void SendCreateInviteFriends(GamePlayer player, int type)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(107, player.PlayerId);
            gSPacketIn.WriteInt(type);
            switch (type)
            {
                case 2:
                    gSPacketIn.WriteInt(0);
                    gSPacketIn.WriteInt(0);
                    gSPacketIn.WriteInt(player.PlayerCharacter.MyInvitedCount);
                    break;
                case 3:
                    gSPacketIn.WriteInt(player.PlayerCharacter.MyRewardStatus ? 1 : 0);
                    break;
                case 4:
                    gSPacketIn.WriteBoolean(val: true);
                    gSPacketIn.WriteInt(player.PlayerCharacter.MyColumn1);
                    gSPacketIn.WriteInt(player.PlayerCharacter.MyColumn2);
                    gSPacketIn.WriteInt(player.PlayerCharacter.MyColumn3);
                    gSPacketIn.WriteInt(player.PlayerCharacter.MyColumn4);
                    break;
                case 5:
                    gSPacketIn.WriteString(player.PlayerCharacter.MyInviteCode);
                    gSPacketIn.WriteInt(player.PlayerCharacter.MyRewardStatus ? 1 : 0);
                    gSPacketIn.WriteDateTime(DateTime.Now);
                    gSPacketIn.WriteInt(player.PlayerCharacter.MyColumn1);
                    gSPacketIn.WriteInt(player.PlayerCharacter.MyColumn2);
                    gSPacketIn.WriteInt(player.PlayerCharacter.MyColumn3);
                    gSPacketIn.WriteInt(player.PlayerCharacter.MyColumn4);
                    gSPacketIn.WriteInt(4);
                    break;
                case 6:
                    gSPacketIn.WriteDateTime(DateTime.Now);
                    break;
            }
            SendTCP(gSPacketIn);
        }
        public void SendTCP(GSPacketIn packet) => this.m_gameClient.SendTCP(packet);

        public void SendAcademyGradute(GamePlayer app, int type)
        {
            GSPacketIn packet = new GSPacketIn((short)141);
            packet.WriteByte((byte)11);
            packet.WriteInt(type);
            packet.WriteInt(app.PlayerId);
            packet.WriteString(app.PlayerCharacter.NickName);
            this.SendTCP(packet);
        }

        public GSPacketIn SendAcademySystemNotice(string text, bool isAlert)
        {
            GSPacketIn packet = new GSPacketIn((short)141);
            packet.WriteByte((byte)17);
            packet.WriteString(text);
            packet.WriteBoolean(isAlert);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendAcademyAppState(PlayerInfo player, int removeUserId)
        {
            GSPacketIn packet = new GSPacketIn((short)141);
            packet.WriteByte((byte)10);
            packet.WriteInt(player.apprenticeshipState);
            packet.WriteInt(player.masterID);
            packet.WriteString(player.masterOrApprentices);
            packet.WriteInt(removeUserId);
            packet.WriteInt(player.graduatesCount);
            packet.WriteString(player.honourOfMaster);
            packet.WriteDateTime(player.freezesDate);
            this.SendTCP(packet);
            return packet;
        }


        public GSPacketIn sendCompose(GamePlayer Player)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(81, Player.PlayerCharacter.ID);
            gSPacketIn.WriteByte(5);
            this.SendTCP(gSPacketIn);
            return gSPacketIn;
        }


        public GSPacketIn sendConsortiaOut(int id, bool result, string msg, int playerid)
        {
            GSPacketIn packet = new GSPacketIn((short)129, playerid);
            packet.WriteByte((byte)3);
            packet.WriteInt(id);
            packet.WriteBoolean(result);
            packet.WriteString(msg);
            this.SendTCP(packet);
            return packet;
        }
        public GSPacketIn SendConsortiaTaskInfo(BaseConsortiaTask baseTask)
        {
            GSPacketIn packet = new GSPacketIn((short)129);
            packet.WriteByte((byte)22);
            packet.WriteByte((byte)3);
            if (baseTask != null)
            {
                packet.WriteInt(baseTask.ConditionList.Count);
                foreach (KeyValuePair<int, ConsortiaTaskInfo> condition in baseTask.ConditionList)
                {
                    packet.WriteInt(condition.Key);
                    packet.WriteInt(3);
                    packet.WriteString(condition.Value.CondictionTitle);
                    packet.WriteInt(baseTask.GetTotalValueByConditionPlace(condition.Key));
                    packet.WriteInt(condition.Value.Para2);
                    packet.WriteInt(baseTask.GetValueByConditionPlace(this.m_gameClient.Player.PlayerCharacter.ID, condition.Key));
                }
                packet.WriteInt(baseTask.Info.TotalExp);
                packet.WriteInt(baseTask.Info.TotalOffer);
                packet.WriteInt(baseTask.Info.TotalRiches);
                packet.WriteInt(baseTask.Info.BuffID);
                packet.WriteDateTime(baseTask.Info.StartTime);
                packet.WriteInt(baseTask.Info.VaildDate);
            }
            else
            {
                packet.WriteInt(0);
                packet.WriteInt(0);
                packet.WriteInt(0);
                packet.WriteInt(0);
                packet.WriteInt(0);
                packet.WriteDateTime(DateTime.Now);
                packet.WriteInt(0);
            }
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SenddoMature(GamePlayer Player)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(81, Player.PlayerCharacter.ID);
            gSPacketIn.WriteByte(3);
            this.SendTCP(gSPacketIn);
            return gSPacketIn;
        }
        public void SendDragonBoat(PlayerInfo info)
        {
            GSPacketIn packet = new GSPacketIn((short)100, info.ID);
            packet.WriteByte((byte)1);
            packet.WriteInt(0);
            packet.WriteInt(ActiveSystemMgr.periodType);
            packet.WriteInt(ActiveSystemMgr.boatCompleteExp);
            this.SendTCP(packet);
        }
        public GSPacketIn SendBattleGoundOpen(int ID)
        {
            GSPacketIn packet = new GSPacketIn((short)132, ID);
            packet.WriteByte((byte)1);
            this.SendTCP(packet);
            return packet;
        }
        public GSPacketIn SendBattleGoundOver(int ID)
        {
            GSPacketIn packet = new GSPacketIn((short)132, ID);
            packet.WriteByte((byte)2);
            this.SendTCP(packet);
            return packet;
        }
        public GSPacketIn SendSystemConsortiaChat(string content, bool sendToSelf)
        {
            GSPacketIn packet = new GSPacketIn((short)129);
            packet.WriteByte((byte)20);
            packet.WriteByte((byte)0);
            packet.WriteString("");
            packet.WriteString(content);
            if (sendToSelf)
                this.SendTCP(packet);
            return packet;
        }
        public void SendPyramidOpenClose(PyramidConfigInfo info)
        {
            GSPacketIn packet = new GSPacketIn((short)145, info.UserID);
            packet.WriteByte((byte)0);
            packet.WriteBoolean(info.isOpen);
            packet.WriteBoolean(info.isScoreExchange);
            packet.WriteDateTime(info.beginTime);
            packet.WriteDateTime(info.endTime);
            packet.WriteInt(info.freeCount);
            packet.WriteInt(info.turnCardPrice);
            packet.WriteInt(info.revivePrice.Length);
            for (int index = 0; index < info.revivePrice.Length; ++index)
                packet.WriteInt(info.revivePrice[index]);
            this.SendTCP(packet);
        }

        public void SendShopGoodsCountUpdate(List<ShopFreeCountInfo> list)
        {
            GSPacketIn packet = new GSPacketIn((short)168);
            packet.WriteInt(list.Count);
            foreach (ShopFreeCountInfo shopFreeCountInfo in list)
            {
                packet.WriteInt(shopFreeCountInfo.ShopID);
                packet.WriteInt(shopFreeCountInfo.Count);
            }
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            this.SendTCP(packet);
        }

        public void SendEliteGameStartRoom()
        {
            GSPacketIn packet = new GSPacketIn((short)162);
            packet.WriteByte((byte)2);
            this.SendTCP(packet);
        }

        public void SendEliteGameInfo(int type)
        {
            GSPacketIn packet = new GSPacketIn((short)162);
            packet.WriteByte((byte)1);
            packet.WriteInt(type);
            this.SendTCP(packet);
        }

        public void IsLeagueOpen()
        {
            GSPacketIn packet = new GSPacketIn((short)42);
            packet.WriteByte((byte)1);
           // packet.WriteInt(isMsg);
            this.SendTCP(packet);
        }
        public void SendOpenWorldBoss(int pX, int pY)
        {
            BaseWorldBossRoom worldBossRoom = RoomMgr.WorldBossRoom;
            GSPacketIn gSPacketIn = new GSPacketIn(102);
            gSPacketIn.WriteByte(0);
            gSPacketIn.WriteString(worldBossRoom.BossResourceId);
            gSPacketIn.WriteInt(worldBossRoom.CurrentPVE);
            gSPacketIn.WriteString("Thần thú");
            gSPacketIn.WriteString(worldBossRoom.Name);
            gSPacketIn.WriteLong(worldBossRoom.MaxBlood);
            gSPacketIn.WriteInt(0);
            gSPacketIn.WriteInt(0);
            gSPacketIn.WriteInt(1);
            gSPacketIn.WriteInt((pX == 0 ? worldBossRoom.playerDefaultPosX : pX));
            gSPacketIn.WriteInt((pY == 0 ? worldBossRoom.playerDefaultPosY : pY));
            gSPacketIn.WriteDateTime(worldBossRoom.Begin_time);
            gSPacketIn.WriteDateTime(worldBossRoom.End_time);
            gSPacketIn.WriteInt(worldBossRoom.Fight_time);
            gSPacketIn.WriteBoolean(worldBossRoom.FightOver);
            gSPacketIn.WriteBoolean(worldBossRoom.RoomClose);
            gSPacketIn.WriteInt(worldBossRoom.ticketID);
            gSPacketIn.WriteInt(worldBossRoom.need_ticket_count);
            gSPacketIn.WriteInt(worldBossRoom.timeCD);
            gSPacketIn.WriteInt(worldBossRoom.reviveMoney);
            gSPacketIn.WriteInt(worldBossRoom.reFightMoney);
            gSPacketIn.WriteInt(worldBossRoom.addInjureBuffMoney);
            gSPacketIn.WriteInt(worldBossRoom.addInjureValue);
            gSPacketIn.WriteInt(0);
            gSPacketIn.WriteBoolean(true);
            gSPacketIn.WriteBoolean(false);
            this.SendTCP(gSPacketIn);
        }
        public void SendOpenDDPlay(PlayerInfo player)
        {
            DateTime dateTime = DateTime.Parse(GameProperties.DDPlayActivityBeginDate);
            DateTime dateTime2 = DateTime.Parse(GameProperties.DDPlayActivityEndDate);
            int dDPlayActivityMoney = GameProperties.DDPlayActivityMoney;
            GSPacketIn gSPacketIn = new GSPacketIn(145);
            gSPacketIn.WriteByte(74);
            if (DateTime.Now >= dateTime && DateTime.Now <= dateTime2)
            {
                gSPacketIn.WriteBoolean(true);
            }
            else
            {
                gSPacketIn.WriteBoolean(false);
            }
            gSPacketIn.WriteDateTime(dateTime);
            gSPacketIn.WriteDateTime(dateTime2);
            gSPacketIn.WriteInt(dDPlayActivityMoney);
            gSPacketIn.WriteInt(0);
            this.SendTCP(gSPacketIn);
        }
        public GSPacketIn SendLabyrinthUpdataInfo(int ID, UserLabyrinthInfo laby)
        {
            GSPacketIn packet = new GSPacketIn((short)131, ID);
            packet.WriteByte((byte)2);
            packet.WriteInt(laby.myProgress);
            packet.WriteInt(laby.currentFloor);
            packet.WriteBoolean(laby.completeChallenge);
            packet.WriteInt(laby.remainTime);
            packet.WriteInt(laby.accumulateExp);
            packet.WriteInt(laby.cleanOutAllTime);
            packet.WriteInt(laby.cleanOutGold);
            packet.WriteInt(laby.myRanking);
            packet.WriteBoolean(laby.isDoubleAward);
            packet.WriteBoolean(laby.isInGame);
            packet.WriteBoolean(laby.isCleanOut);
            packet.WriteBoolean(laby.serverMultiplyingPower);
            this.SendTCP(packet);
            return packet;
        }



        public GSPacketIn SendUpdateUserPet(PetInventory bag, int[] slots)
        {
            if (m_gameClient.Player == null)
            {
                return null;
            }
            GSPacketIn gSPacketIn = new GSPacketIn(68, m_gameClient.Player.PlayerId);
            gSPacketIn.WriteByte(1);
            gSPacketIn.WriteInt(m_gameClient.Player.PlayerId);
            gSPacketIn.WriteInt(m_gameClient.Player.ZoneId);
            gSPacketIn.WriteInt(slots.Length);
            foreach (int num in slots)
            {
                gSPacketIn.WriteInt(num);
                UsersPetInfo petAt = bag.GetPetAt(num);
                if (petAt == null)
                {
                    gSPacketIn.WriteBoolean(val: false);
                    continue;
                }
                gSPacketIn.WriteBoolean(val: true);
                gSPacketIn.WriteInt(petAt.ID);
                gSPacketIn.WriteInt(petAt.TemplateID);
                gSPacketIn.WriteString(petAt.Name);
                gSPacketIn.WriteInt(petAt.UserID);
                gSPacketIn.WriteInt(petAt.Attack);
                gSPacketIn.WriteInt(petAt.Defence);
                gSPacketIn.WriteInt(petAt.Luck);
                gSPacketIn.WriteInt(petAt.Agility);
                gSPacketIn.WriteInt(petAt.Blood);
                gSPacketIn.WriteInt(petAt.Damage);
                gSPacketIn.WriteInt(petAt.Guard);
                gSPacketIn.WriteInt(petAt.AttackGrow);
                gSPacketIn.WriteInt(petAt.DefenceGrow);
                gSPacketIn.WriteInt(petAt.LuckGrow);
                gSPacketIn.WriteInt(petAt.AgilityGrow);
                gSPacketIn.WriteInt(petAt.BloodGrow);
                gSPacketIn.WriteInt(petAt.DamageGrow);
                gSPacketIn.WriteInt(petAt.GuardGrow);
                gSPacketIn.WriteInt(petAt.Level);
                gSPacketIn.WriteInt(petAt.GP);
                gSPacketIn.WriteInt(petAt.MaxGP);
                gSPacketIn.WriteInt(petAt.Hunger);
                gSPacketIn.WriteInt(petAt.PetHappyStar);
                gSPacketIn.WriteInt(petAt.MP);
                List<string> skill = petAt.GetSkill();
                gSPacketIn.WriteInt(skill.Count);
                foreach (string item in skill)
                {
                    gSPacketIn.WriteInt(int.Parse(item.Split(',')[0]));
                    gSPacketIn.WriteInt(int.Parse(item.Split(',')[1]));
                }
                List<string> skillEquip = petAt.GetSkillEquip();
                gSPacketIn.WriteInt(skillEquip.Count);
                foreach (string item2 in skillEquip)
                {
                    gSPacketIn.WriteInt(int.Parse(item2.Split(',')[1]));
                    gSPacketIn.WriteInt(int.Parse(item2.Split(',')[0]));
                }
                gSPacketIn.WriteBoolean(petAt.IsEquip);
                gSPacketIn.WriteInt(petAt.PetEquips.Count);
                foreach (PetEquipInfo petEquip in petAt.PetEquips)
                {
                    gSPacketIn.WriteInt(petEquip.eqType);
                    gSPacketIn.WriteInt(petEquip.eqTemplateID);
                    gSPacketIn.WriteDateTime(petEquip.startTime);
                    gSPacketIn.WriteInt(petEquip.ValidDate);
                }
                gSPacketIn.WriteInt(petAt.currentStarExp);
            }
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public void SendOpenOrCloseChristmas(int lastPacks, bool isOpen)
        {
            GSPacketIn packet = new GSPacketIn((short)145);
            packet.WriteByte((byte)16);
            packet.WriteBoolean(isOpen);
            if (isOpen)
            {
                DateTime date1 = DateTime.Parse(GameProperties.ChristmasBeginDate);
                DateTime date2 = DateTime.Parse(GameProperties.ChristmasEndDate);
                packet.WriteDateTime(date1);
                packet.WriteDateTime(date2);
                string[] strArray1 = GameProperties.ChristmasGifts.Split('|');
                packet.WriteInt(strArray1.Length);
                for (int index = 0; index < strArray1.Length; ++index)
                {
                    string[] strArray2 = strArray1[index].Split(',');
                    packet.WriteInt(int.Parse(strArray2[0]));
                    packet.WriteInt(int.Parse(strArray2[1]));
                }
                packet.WriteInt(lastPacks);
                packet.WriteInt(GameProperties.ChristmasBuildSnowmanDoubleMoney);
            }
            this.SendTCP(packet);
        }

        public GSPacketIn SendPetInfo(int id, int zoneId, UsersPetInfo[] pets)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(68, id);
            gSPacketIn.WriteByte(1);
            gSPacketIn.WriteInt(id);
            gSPacketIn.WriteInt(zoneId);
            gSPacketIn.WriteInt(pets.Length);
            foreach (UsersPetInfo usersPetInfo in pets)
            {
                gSPacketIn.WriteInt(usersPetInfo.Place);
                gSPacketIn.WriteBoolean(val: true);
                gSPacketIn.WriteInt(usersPetInfo.ID);
                gSPacketIn.WriteInt(usersPetInfo.TemplateID);
                gSPacketIn.WriteString(usersPetInfo.Name);
                gSPacketIn.WriteInt(usersPetInfo.UserID);
                gSPacketIn.WriteInt(usersPetInfo.Attack);
                gSPacketIn.WriteInt(usersPetInfo.Defence);
                gSPacketIn.WriteInt(usersPetInfo.Luck);
                gSPacketIn.WriteInt(usersPetInfo.Agility);
                gSPacketIn.WriteInt(usersPetInfo.Blood);
                gSPacketIn.WriteInt(usersPetInfo.Damage);
                gSPacketIn.WriteInt(usersPetInfo.Guard);
                gSPacketIn.WriteInt(usersPetInfo.AttackGrow);
                gSPacketIn.WriteInt(usersPetInfo.DefenceGrow);
                gSPacketIn.WriteInt(usersPetInfo.LuckGrow);
                gSPacketIn.WriteInt(usersPetInfo.AgilityGrow);
                gSPacketIn.WriteInt(usersPetInfo.BloodGrow);
                gSPacketIn.WriteInt(usersPetInfo.DamageGrow);
                gSPacketIn.WriteInt(usersPetInfo.GuardGrow);
                gSPacketIn.WriteInt(usersPetInfo.Level);
                gSPacketIn.WriteInt(usersPetInfo.GP);
                gSPacketIn.WriteInt(usersPetInfo.MaxGP);
                gSPacketIn.WriteInt(usersPetInfo.Hunger);
                gSPacketIn.WriteInt(usersPetInfo.PetHappyStar);
                gSPacketIn.WriteInt(usersPetInfo.MP);
                List<string> skill = usersPetInfo.GetSkill();
                List<string> skillEquip = usersPetInfo.GetSkillEquip();
                gSPacketIn.WriteInt(skill.Count);
                foreach (string item in skill)
                {
                    gSPacketIn.WriteInt(int.Parse(item.Split(',')[0]));
                    gSPacketIn.WriteInt(int.Parse(item.Split(',')[1]));
                }
                gSPacketIn.WriteInt(skillEquip.Count);
                foreach (string item2 in skillEquip)
                {
                    gSPacketIn.WriteInt(int.Parse(item2.Split(',')[1]));
                    gSPacketIn.WriteInt(int.Parse(item2.Split(',')[0]));
                }
                gSPacketIn.WriteBoolean(usersPetInfo.IsEquip);
                gSPacketIn.WriteInt(usersPetInfo.PetEquips.Count);
                foreach (PetEquipInfo petEquip in usersPetInfo.PetEquips)
                {
                    gSPacketIn.WriteInt(petEquip.eqType);
                    gSPacketIn.WriteInt(petEquip.eqTemplateID);
                    gSPacketIn.WriteDateTime(petEquip.startTime);
                    gSPacketIn.WriteInt(petEquip.ValidDate);
                }
                gSPacketIn.WriteInt(usersPetInfo.currentStarExp);
            }
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn sendBuyBadge(
      int consortiaID,
      int BadgeID,
      int ValidDate,
      bool result,
      string BadgeBuyTime,
      int playerid)
        {
            GSPacketIn packet = new GSPacketIn((short)129, playerid);
            packet.WriteByte((byte)28);
            packet.WriteInt(consortiaID);
            packet.WriteInt(BadgeID);
            packet.WriteInt(ValidDate);
            packet.WriteDateTime(Convert.ToDateTime(BadgeBuyTime));
            packet.WriteBoolean(result);
            this.SendTCP(packet);
            return packet;
        }

        public void SendEdictumVersion()
        {
            EdictumInfo[] allEdictumVersion = WorldMgr.GetAllEdictumVersion();
            Random random = new Random();
            if (allEdictumVersion.Length == 0)
                return;
            GSPacketIn packet = new GSPacketIn((short)75);
            packet.WriteInt(allEdictumVersion.Length);
            foreach (EdictumInfo edictumInfo in allEdictumVersion)
                packet.WriteInt(edictumInfo.ID + random.Next(10000));
            this.SendTCP(packet);
        }

        public void SendLeftRouleteOpen(UsersExtraInfo info)
        {
            GSPacketIn packet = new GSPacketIn((short)137);
            packet.WriteInt(1);
            packet.WriteInt(1);
            packet.WriteBoolean(true);
            packet.WriteInt((double)info.LeftRoutteRate > 0.0 ? 0 : info.LeftRoutteCount);
            packet.WriteString(string.Format("{0:N1}", (object)info.LeftRoutteRate));
            foreach (char ch in GameProperties.LeftRouterRateData)
            {
                switch (ch)
                {
                    case '.':
                    case '|':
                        packet.WriteInt(0);
                        break;
                    default:
                        packet.WriteInt(int.Parse(ch.ToString()));
                        break;
                }
            }
            this.SendTCP(packet);
        }

        public void SendLeftRouleteResult(UsersExtraInfo info)
        {
            GSPacketIn packet = new GSPacketIn((short)163);
            packet.WriteInt((double)info.LeftRoutteRate > 0.0 ? 0 : info.LeftRoutteCount);
            packet.WriteString(string.Format("{0:N1}", (object)info.LeftRoutteRate));
            this.SendTCP(packet);
        }

        public void SendEnthrallLight()
        {
            GSPacketIn packet = new GSPacketIn((short)227);
            packet.WriteBoolean(false);
            packet.WriteInt(0);
            packet.WriteBoolean(false);
            packet.WriteBoolean(false);
            this.SendTCP(packet);
        }

        public void SendLoginFailed(string msg)
        {
            GSPacketIn packet = new GSPacketIn((short)1);
            packet.WriteByte((byte)1);
            packet.WriteString(msg);
            this.SendTCP(packet);
        }

        public void SendOpenNoviceActive(
          int channel,
          int activeId,
          int condition,
          int awardGot,
          DateTime startTime,
          DateTime endTime)
        {
            GSPacketIn packet = new GSPacketIn((short)258);
            packet.WriteInt(channel);
            switch (channel)
            {
                case 0:
                    packet.WriteInt(activeId);
                    packet.WriteInt(condition);
                    packet.WriteInt(awardGot);
                    packet.WriteDateTime(startTime);
                    packet.WriteDateTime(endTime);
                    break;
                case 1:
                    packet.WriteBoolean(false);
                    break;
            }
            this.SendTCP(packet);
        }

        public void SendUpdateFirstRecharge(bool isRecharge, bool isGetAward)
        {
            GSPacketIn packet = new GSPacketIn((short)259);
            packet.WriteBoolean(isRecharge);
            packet.WriteBoolean(isGetAward);
            this.SendTCP(packet);
        }

        public GSPacketIn sendBuyBadge(
          int BadgeID,
          int ValidDate,
          bool result,
          string BadgeBuyTime,
          int playerid)
        {
            GSPacketIn packet = new GSPacketIn((short)164, playerid);
            packet.WriteInt(BadgeID);
            packet.WriteInt(BadgeID);
            packet.WriteInt(ValidDate);
            packet.WriteDateTime(Convert.ToDateTime(BadgeBuyTime));
            packet.WriteBoolean(result);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendOpenTimeBox(int condtion, bool isSuccess)
        {
            GSPacketIn packet = new GSPacketIn((short)53);
            packet.WriteBoolean(isSuccess);
            packet.WriteInt(condtion);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendConsortiaMail(bool result, int playerid)
        {
            GSPacketIn packet = new GSPacketIn((short)215, playerid);
            packet.WriteBoolean(result);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendAddFriend(PlayerInfo user, int relation, bool state)
        {
            GSPacketIn packet = new GSPacketIn((short)160, user.ID);
            packet.WriteByte((byte)160);
            packet.WriteBoolean(state);
            if (state)
            {
                packet.WriteInt(user.ID);
                packet.WriteString(user.NickName);
                packet.WriteByte(user.typeVIP);
                packet.WriteInt(user.VIPLevel);
                packet.WriteBoolean(user.Sex);
                packet.WriteString(user.Style);
                packet.WriteString(user.Colors);
                packet.WriteString(user.Skin);
                packet.WriteInt(user.State == 1 ? 1 : 0);
                packet.WriteInt(user.Grade);
                packet.WriteInt(user.Hide);
                packet.WriteString(user.ConsortiaName);
                packet.WriteInt(user.Total);
                packet.WriteInt(user.Escape);
                packet.WriteInt(user.Win);
                packet.WriteInt(user.Offer);
                packet.WriteInt(user.Repute);
                packet.WriteInt(relation);
                packet.WriteString(user.UserName);
                packet.WriteInt(user.Nimbus);
                packet.WriteInt(user.FightPower);
                packet.WriteInt(user.apprenticeshipState);
                packet.WriteInt(user.masterID);
                packet.WriteString(user.masterOrApprentices);
                packet.WriteInt(user.graduatesCount);
                packet.WriteString(user.honourOfMaster);
                packet.WriteInt(user.AchievementPoint);
                packet.WriteString(user.Honor);
                packet.WriteBoolean(user.IsMarried);
            }
            this.SendTCP(packet);
            return packet;
        }
        public GSPacketIn SendDiceActiveClose(int ID)
        {
            GSPacketIn packet = new GSPacketIn((short)134, ID);
            packet.WriteByte((byte)2);
            this.SendTCP(packet);
            return packet;
        }
        public GSPacketIn SendDiceReceiveData(PlayerDice Dice)
        {
            GSPacketIn packet = new GSPacketIn((short)134, Dice.Data.UserID);
            packet.WriteByte((byte)3);
            packet.WriteBoolean(Dice.Data.UserFirstCell);
            packet.WriteInt(Dice.Data.CurrentPosition);
            packet.WriteInt(Dice.Data.LuckIntegralLevel);
            packet.WriteInt(Dice.Data.LuckIntegral);
            packet.WriteInt(Dice.Data.FreeCount);
            packet.WriteInt(Dice.RewardItem.Count);
            for (int index = 0; index < Dice.RewardItem.Count; ++index)
            {
                packet.WriteInt(Dice.RewardItem[index].TemplateID);
                packet.WriteInt(index);
                packet.WriteInt(Dice.RewardItem[index].StrengthenLevel);
                packet.WriteInt(Dice.RewardItem[index].Count);
                packet.WriteInt(Dice.RewardItem[index].ValidDate);
                packet.WriteBoolean(Dice.RewardItem[index].IsBinds);
            }
            this.SendTCP(packet);
            return packet;
        }
        public GSPacketIn SendDiceReceiveResult(PlayerDice Dice)
        {
            GSPacketIn packet = new GSPacketIn((short)134, Dice.Data.UserID);
            packet.WriteByte((byte)4);
            packet.WriteInt(Dice.Data.CurrentPosition);
            packet.WriteInt(Dice.result);
            packet.WriteInt(Dice.Data.LuckIntegral);
            packet.WriteInt(Dice.Data.Level);
            packet.WriteInt(Dice.Data.FreeCount);
            packet.WriteString(Dice.RewardName);
            this.SendTCP(packet);
            return packet;
        }
        public GSPacketIn SendDiceActiveOpen(PlayerDice Dice)
        {
            GSPacketIn packet = new GSPacketIn((short)134, Dice.Data.UserID);
            packet.WriteByte((byte)1);
            packet.WriteInt(Dice.Data.FreeCount);
            packet.WriteInt(Dice.refreshPrice);
            packet.WriteInt(Dice.commonDicePrice);
            packet.WriteInt(Dice.doubleDicePrice);
            packet.WriteInt(Dice.bigDicePrice);
            packet.WriteInt(Dice.smallDicePrice);
            packet.WriteInt(Dice.MAX_LEVEL);
            for (int key = 0; key < Dice.MAX_LEVEL; ++key)
            {
                List<DiceLevelAwardInfo> diceLevelAwardInfoList = Dice.LevelAward[key];
                packet.WriteInt(Dice.IntegralPoint[key]);
                packet.WriteInt(diceLevelAwardInfoList.Count);
                for (int index = 0; index < diceLevelAwardInfoList.Count; ++index)
                {
                    packet.WriteInt(diceLevelAwardInfoList[index].TemplateID);
                    packet.WriteInt(diceLevelAwardInfoList[index].Count);
                }
            }
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendChickenBoxOpen(
     int ID,
     int flushPrice,
     int[] openCardPrice,
     int[] eagleEyePrice)
        {
            GSPacketIn packet = new GSPacketIn((short)87, ID);
            packet.WriteInt(1);
            packet.WriteInt(openCardPrice.Length);
            for (int length = openCardPrice.Length; length > 0; --length)
                packet.WriteInt(openCardPrice[length - 1]);
            packet.WriteInt(eagleEyePrice.Length);
            for (int length = eagleEyePrice.Length; length > 0; --length)
                packet.WriteInt(eagleEyePrice[length - 1]);
            packet.WriteInt(flushPrice);
            packet.WriteDateTime(DateTime.Parse(GameProperties.NewChickenEndTime));
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendLuckStarOpen(int ID)
        {
            GSPacketIn packet = new GSPacketIn((short)87, ID);
            packet.WriteInt(25);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendFriendRemove(int FriendID)
        {
            GSPacketIn packet = new GSPacketIn((short)160, FriendID);
            packet.WriteByte((byte)161);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendFriendState(
          int playerID,
          int state,
          byte typeVip,
          int viplevel)
        {
            GSPacketIn packet = new GSPacketIn((short)160, playerID);
            packet.WriteByte((byte)165);
            packet.WriteInt(state);
            packet.WriteInt((int)typeVip);
            packet.WriteInt(viplevel);
            packet.WriteBoolean(true);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn sendOneOnOneTalk(
          int receiverID,
          bool isAutoReply,
          string SenderNickName,
          string msg,
          int playerid)
        {
            GSPacketIn packet = new GSPacketIn((short)160, playerid);
            packet.WriteByte((byte)51);
            packet.WriteInt(receiverID);
            packet.WriteString(SenderNickName);
            packet.WriteDateTime(DateTime.Now);
            packet.WriteString(msg);
            packet.WriteBoolean(isAutoReply);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendUpdateConsotiaBoss(ConsortiaBossInfo bossInfo)
        {
            GSPacketIn packet = new GSPacketIn((short)162);
            packet.WriteByte((byte)bossInfo.typeBoss);
            packet.WriteInt(bossInfo.powerPoint);
            packet.WriteInt(bossInfo.callBossCount);
            packet.WriteDateTime(bossInfo.BossOpenTime);
            packet.WriteInt(bossInfo.BossLevel);
            packet.WriteBoolean(false);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendUpdateConsotiaBuffer(
          GamePlayer player,
          Dictionary<string, BufferInfo> bufflist)
        {
            List<ConsortiaBuffTempInfo> allConsortiaBuff = ConsortiaExtraMgr.GetAllConsortiaBuff();
            GSPacketIn packet = new GSPacketIn((short)129, player.PlayerId);
            packet.WriteByte((byte)26);
            packet.WriteInt(allConsortiaBuff.Count);
            foreach (ConsortiaBuffTempInfo consortiaBuffTempInfo in allConsortiaBuff)
            {
                if (bufflist.ContainsKey(consortiaBuffTempInfo.id.ToString()))
                {
                    BufferInfo bufferInfo = bufflist[consortiaBuffTempInfo.id.ToString()];
                    packet.WriteInt(consortiaBuffTempInfo.id);
                    packet.WriteBoolean(true);
                    packet.WriteDateTime(bufferInfo.BeginDate);
                    packet.WriteInt(bufferInfo.ValidDate / 24 / 60);
                }
                else
                {
                    packet.WriteInt(consortiaBuffTempInfo.id);
                    packet.WriteBoolean(false);
                    packet.WriteDateTime(DateTime.Now);
                    packet.WriteInt(0);
                }
            }
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendPlayerDrill(int ID, Dictionary<int, UserDrillInfo> drills)
        {
            GSPacketIn packet = new GSPacketIn((short)121, ID);
            packet.WriteByte((byte)6);
            packet.WriteInt(ID);
            packet.WriteInt(drills[0].HoleExp);
            packet.WriteInt(drills[1].HoleExp);
            packet.WriteInt(drills[2].HoleExp);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(drills[0].HoleLv);
            packet.WriteInt(drills[1].HoleLv);
            packet.WriteInt(drills[2].HoleLv);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendUpdateAchievementData(List<AchievementDataInfo> infos)
        {
            bool flag;
            if (infos != null)
            {
                int id = this.m_gameClient.Player.PlayerCharacter.ID;
                flag = true;
            }
            else
                flag = false;
            GSPacketIn gsPacketIn1;
            if (!flag)
            {
                gsPacketIn1 = (GSPacketIn)null;
            }
            else
            {
                GSPacketIn packet = new GSPacketIn((short)231, this.m_gameClient.Player.PlayerCharacter.ID);
                packet.WriteInt(infos.Count);
                for (int index = 0; index < infos.Count; ++index)
                {
                    AchievementDataInfo info = infos[index];
                    packet.WriteInt(info.AchievementID);
                    GSPacketIn gsPacketIn2 = packet;
                    DateTime completedDate = info.CompletedDate;
                    int year = completedDate.Year;
                    gsPacketIn2.WriteInt(year);
                    GSPacketIn gsPacketIn3 = packet;
                    completedDate = info.CompletedDate;
                    int month = completedDate.Month;
                    gsPacketIn3.WriteInt(month);
                    GSPacketIn gsPacketIn4 = packet;
                    completedDate = info.CompletedDate;
                    int day = completedDate.Day;
                    gsPacketIn4.WriteInt(day);
                }
                this.SendTCP(packet);
                gsPacketIn1 = packet;
            }
            return gsPacketIn1;
        }

        public GSPacketIn SendAchievementSuccess(AchievementDataInfo d)
        {
            GSPacketIn packet = new GSPacketIn((short)230);
            packet.WriteInt(d.AchievementID);
            packet.WriteInt(d.CompletedDate.Year);
            packet.WriteInt(d.CompletedDate.Month);
            packet.WriteInt(d.CompletedDate.Day);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendUpdateAchievements(List<UsersRecordInfo> infos)
        {
            bool flag;
            if (infos != null && this.m_gameClient != null && this.m_gameClient.Player != null)
            {
                int id = this.m_gameClient.Player.PlayerCharacter.ID;
                flag = true;
            }
            else
                flag = false;
            GSPacketIn gsPacketIn;
            if (!flag)
            {
                gsPacketIn = (GSPacketIn)null;
            }
            else
            {
                GSPacketIn packet = new GSPacketIn((short)229, this.m_gameClient.Player.PlayerCharacter.ID);
                packet.WriteInt(infos.Count);
                for (int index = 0; index < infos.Count; ++index)
                {
                    UsersRecordInfo info = infos[index];
                    packet.WriteInt(info.RecordID);
                    packet.WriteInt(info.Total);
                }
                this.SendTCP(packet);
                gsPacketIn = packet;
            }
            return gsPacketIn;
        }

        public GSPacketIn SendUpdateAchievements(UsersRecordInfo info)
        {
            bool flag;
            if (info != null && this.m_gameClient != null && this.m_gameClient.Player != null)
            {
                int id = this.m_gameClient.Player.PlayerCharacter.ID;
                flag = true;
            }
            else
                flag = false;
            GSPacketIn gsPacketIn;
            if (!flag)
            {
                gsPacketIn = (GSPacketIn)null;
            }
            else
            {
                GSPacketIn packet = new GSPacketIn((short)229, this.m_gameClient.Player.PlayerCharacter.ID);
                packet.WriteInt(1);
                for (int index = 0; index < 1; ++index)
                {
                    packet.WriteInt(info.RecordID);
                    packet.WriteInt(info.Total);
                }
                this.SendTCP(packet);
                gsPacketIn = packet;
            }
            return gsPacketIn;
        }
        public GSPacketIn SendUpdateUpCount(PlayerInfo player)
        {
            GSPacketIn packet = new GSPacketIn((short)96, player.ID);
            packet.WriteInt(player.MaxBuyHonor);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendPlayerRefreshTotem(PlayerInfo player)
        {
            GSPacketIn packet = new GSPacketIn((short)136, player.ID);
            packet.WriteInt((byte)1);
            packet.WriteInt(player.myHonor);
            packet.WriteInt(player.totemId);
            this.SendTCP(packet);
            return packet;
        }


        public GSPacketIn SendInitAchievements(List<UsersRecordInfo> infos)
        {
            bool flag;
            if (infos != null && this.m_gameClient.Player != null)
            {
                int id = this.m_gameClient.Player.PlayerCharacter.ID;
                flag = true;
            }
            else
                flag = false;
            GSPacketIn gsPacketIn;
            if (!flag)
            {
                gsPacketIn = (GSPacketIn)null;
            }
            else
            {
                GSPacketIn packet = new GSPacketIn((short)228, this.m_gameClient.Player.PlayerCharacter.ID);
                packet.WriteInt(infos.Count);
                for (int index = 0; index < infos.Count; ++index)
                {
                    UsersRecordInfo info = infos[index];
                    packet.WriteInt(info.RecordID);
                    packet.WriteInt(info.Total);
                }
                this.SendTCP(packet);
                this.SendUpdateAchievements(infos);
                gsPacketIn = packet;
            }
            return gsPacketIn;
        }
        public void SendLoginSuccess()
        {
            if (this.m_gameClient.Player == null)
                return;
            GSPacketIn packet = new GSPacketIn((short)1, this.m_gameClient.Player.PlayerCharacter.ID);
            packet.WriteByte((byte)0);
            packet.WriteInt(this.m_gameClient.Player.ZoneId);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.Attack);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.Defence);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.Agility);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.Luck);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.GP);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.Repute);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.Gold);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.Money + this.m_gameClient.Player.PlayerCharacter.MoneyLock);
            packet.WriteInt(this.m_gameClient.Player.GetMedalNum());
           // packet.WriteInt(this.m_gameClient.Player.GetAscensionNum());
            packet.WriteInt(0);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.Hide);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.FightPower);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.accumulativeLoginDays);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.accumulativeAwardDays);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.apprenticeshipState);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.masterID);
            packet.WriteString(this.m_gameClient.Player.PlayerCharacter.masterOrApprentices);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.graduatesCount);
            packet.WriteString(this.m_gameClient.Player.PlayerCharacter.honourOfMaster);
            packet.WriteDateTime(this.m_gameClient.Player.PlayerCharacter.freezesDate);
            packet.WriteByte(this.m_gameClient.Player.PlayerCharacter.typeVIP);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.VIPLevel);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.VIPExp);
            packet.WriteDateTime(this.m_gameClient.Player.PlayerCharacter.VIPExpireDay);
            packet.WriteDateTime(this.m_gameClient.Player.PlayerCharacter.LastDate);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.VIPNextLevelDaysNeeded);
            packet.WriteDateTime(DateTime.Now);
            packet.WriteBoolean(this.m_gameClient.Player.PlayerCharacter.CanTakeVipReward);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.OptionOnOff);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.AchievementPoint);
            packet.WriteString(this.m_gameClient.Player.PlayerCharacter.Honor);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.honorId);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.OnlineTime);
            packet.WriteBoolean(this.m_gameClient.Player.PlayerCharacter.Sex);
            packet.WriteString(this.m_gameClient.Player.PlayerCharacter.Style + "&" + this.m_gameClient.Player.PlayerCharacter.Colors);
            packet.WriteString(this.m_gameClient.Player.PlayerCharacter.Skin);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.ConsortiaID);
            packet.WriteString(this.m_gameClient.Player.PlayerCharacter.ConsortiaName);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.badgeID);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.DutyLevel);
            packet.WriteString(this.m_gameClient.Player.PlayerCharacter.DutyName);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.Right);
            packet.WriteString(this.m_gameClient.Player.PlayerCharacter.ChairmanName);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.ConsortiaHonor);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.ConsortiaRiches);
            packet.WriteBoolean(this.m_gameClient.Player.PlayerCharacter.HasBagPassword);
            packet.WriteString(this.m_gameClient.Player.PlayerCharacter.PasswordQuest1);
            packet.WriteString(this.m_gameClient.Player.PlayerCharacter.PasswordQuest2);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.FailedPasswordAttemptCount);
            packet.WriteString(this.m_gameClient.Player.PlayerCharacter.UserName);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.Nimbus);
            packet.WriteString(this.m_gameClient.Player.PlayerCharacter.PvePermission);
            packet.WriteString(this.m_gameClient.Player.PlayerCharacter.FightLabPermission);
            packet.WriteInt(99999);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.BoxProgression);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.GetBoxLevel);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.AlreadyGetBox);
            packet.WriteDateTime(this.m_gameClient.Player.Extra.Info.LastTimeHotSpring);
            packet.WriteDateTime(this.m_gameClient.Player.PlayerCharacter.ShopFinallyGottenTime);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.Riches);
            packet.WriteInt(this.m_gameClient.Player.MatchInfo.dailyScore);
            packet.WriteInt(this.m_gameClient.Player.MatchInfo.dailyWinCount);
            packet.WriteInt(this.m_gameClient.Player.MatchInfo.dailyGameCount);
            packet.WriteBoolean(this.m_gameClient.Player.MatchInfo.DailyLeagueFirst);
            packet.WriteInt(this.m_gameClient.Player.MatchInfo.DailyLeagueLastScore);
            packet.WriteInt(this.m_gameClient.Player.MatchInfo.weeklyScore);
            packet.WriteInt(this.m_gameClient.Player.MatchInfo.weeklyGameCount);
            packet.WriteInt(this.m_gameClient.Player.MatchInfo.weeklyRanking);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.Texp.spdTexpExp);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.Texp.attTexpExp);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.Texp.defTexpExp);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.Texp.hpTexpExp);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.Texp.lukTexpExp);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.Texp.texpTaskCount);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.Texp.texpCount);
            packet.WriteDateTime(this.m_gameClient.Player.PlayerCharacter.Texp.texpTaskDate);
            packet.WriteBoolean(false);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.badLuckNumber);
            packet.WriteInt(0);
            packet.WriteDateTime(DateTime.Now);
            packet.WriteInt(0);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.totemId);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.necklaceExp);
            this.SendTCP(packet);
        }
        public GSPacketIn SendAddPlayerNewHall(GamePlayer player)
        {
            GSPacketIn packet = new GSPacketIn(0x106);
            packet.WriteByte(1);
            packet.WriteInt(player.PlayerCharacter.ID);
            packet.WriteString(player.PlayerCharacter.NickName);
            packet.WriteInt(player.PlayerCharacter.VIPLevel);
            packet.WriteInt(player.PlayerCharacter.typeVIP);
            packet.WriteBoolean(player.PlayerCharacter.Sex);
            packet.WriteString(player.PlayerCharacter.Style);
            packet.WriteString(player.PlayerCharacter.Colors);
            packet.WriteInt(player.PlayerCharacter.MountsType);
            packet.WriteInt(player.NewHallX);
            packet.WriteInt(player.NewHallY);
            packet.WriteInt(player.PlayerCharacter.ConsortiaID);
            packet.WriteInt(player.PlayerCharacter.badgeID);
            packet.WriteString(player.PlayerCharacter.ConsortiaName);
            packet.WriteString(player.PlayerCharacter.Honor);
            return packet;
        }

        public GSPacketIn SendUpdatePlayerNewHall(GamePlayer player)
        {
            GSPacketIn @in = new GSPacketIn(0x106);
            @in.WriteByte(5);
            @in.WriteInt(player.PlayerCharacter.ID);
            @in.WriteString(player.PlayerCharacter.NickName);
            @in.WriteInt(player.PlayerCharacter.VIPLevel);
            @in.WriteInt(player.PlayerCharacter.typeVIP);
            @in.WriteBoolean(player.PlayerCharacter.Sex);
            @in.WriteString(player.PlayerCharacter.Style);
            @in.WriteString(player.PlayerCharacter.Colors);
            @in.WriteInt(player.PlayerCharacter.MountsType);
            @in.WriteInt(player.NewHallX);
            @in.WriteInt(player.NewHallY);
            @in.WriteInt(player.PlayerCharacter.ConsortiaID);
            @in.WriteInt(player.PlayerCharacter.badgeID);
            @in.WriteString(player.PlayerCharacter.ConsortiaName);
            @in.WriteString(player.PlayerCharacter.Honor);
            return @in;
        }
        public GSPacketIn SendNecklaceStrength(PlayerInfo player)
        {
            GSPacketIn packet = new GSPacketIn(0x5f, player.ID);
            packet.WriteInt(player.necklaceExp);
            packet.WriteInt(player.necklaceExpAdd);
            this.SendTCP(packet);
            return packet;
        }


        public void SendLoginSuccess2()
        {
        }

        public void method_0(byte[] m, byte[] e)
        {
            GSPacketIn packet = new GSPacketIn((short)7);
            packet.Write(m);
            packet.Write(e);
            this.SendTCP(packet);
        }

        public void SendCheckCode()
        {
            if (this.m_gameClient.Player == null || this.m_gameClient.Player.PlayerCharacter.CheckCount < GameProperties.CHECK_MAX_FAILED_COUNT)
                return;
            if (this.m_gameClient.Player.PlayerCharacter.CheckError == 0)
                this.m_gameClient.Player.PlayerCharacter.CheckCount += 10000;
            GSPacketIn packet = new GSPacketIn((short)200, this.m_gameClient.Player.PlayerCharacter.ID, 10240);
            if (this.m_gameClient.Player.PlayerCharacter.CheckError < 1)
                packet.WriteByte((byte)0);
            else
                packet.WriteByte((byte)2);
            packet.WriteBoolean(true);
            this.m_gameClient.Player.PlayerCharacter.CheckCode = CheckCode.GenerateCheckCode();
            packet.Write(CheckCode.CreateImage(this.m_gameClient.Player.PlayerCharacter.CheckCode));
            this.SendTCP(packet);
        }

        public void SendKitoff(string msg)
        {
            GSPacketIn packet = new GSPacketIn((short)2);
            packet.WriteString(msg);
            this.SendTCP(packet);
        }

        public void SendEditionError(string msg)
        {
            GSPacketIn packet = new GSPacketIn((short)12);
            packet.WriteString(msg);
            this.SendTCP(packet);
        }

        public void SendWaitingRoom(bool result)
        {
            GSPacketIn packet = new GSPacketIn((short)16);
            packet.WriteByte(result ? (byte)1 : (byte)0);
            this.SendTCP(packet);
        }

        public GSPacketIn SendPlayerState(int id, byte state)
        {
            GSPacketIn packet = new GSPacketIn((short)32, id);
            packet.WriteByte(state);
            this.SendTCP(packet);
            return packet;
        }

        public virtual GSPacketIn SendMessage(eMessageType type, string message)
        {
            GSPacketIn packet = new GSPacketIn((short)3);
            packet.WriteInt((int)type);
            packet.WriteString(message);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendAvatarCollect(PlayerAvatarCollection avtCollect)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(402);
            gSPacketIn.WriteByte(5);
            gSPacketIn.WriteInt(avtCollect.AvatarCollect.Count);
            if (avtCollect.AvatarCollect.Count > 0)
            {
                foreach (UserAvatarCollectionInfo current in avtCollect.AvatarCollect)
                {
                    gSPacketIn.WriteInt(current.AvatarID);
                    gSPacketIn.WriteInt(current.Sex);
                    if (current.Items == null)
                    {
                        current.UpdateItems();
                    }
                    gSPacketIn.WriteInt(current.Items.Count);
                    if (current.Items.Count > 0)
                    {
                        foreach (UserAvatarCollectionDataInfo current2 in current.Items)
                        {
                            gSPacketIn.WriteInt(current2.TemplateID);
                        }
                    }
                    gSPacketIn.WriteDateTime(current.TimeEnd);
                }
            }
            this.SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public void SendReady() => this.SendTCP(new GSPacketIn((short)0));

        public void SendUpdatePrivateInfo(PlayerInfo info)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(38, info.ID);
            gSPacketIn.WriteInt(info.Money);
            gSPacketIn.WriteInt(info.GiftToken);
            gSPacketIn.WriteInt(0);
            gSPacketIn.WriteInt(info.Score);
            gSPacketIn.WriteInt(info.Gold);
            gSPacketIn.WriteInt(info.badLuckNumber);
            gSPacketIn.WriteInt(info.damageScores);
            gSPacketIn.WriteInt(info.petScore);
            gSPacketIn.WriteInt(info.damageScores);
            gSPacketIn.WriteInt(0);
            this.SendTCP(gSPacketIn);
        }

        public void SendUpdatePrivateInfo(PlayerInfo info, int medal)
        {
            GSPacketIn packet = new GSPacketIn((short)38, info.ID);
            packet.WriteInt(info.Money + info.MoneyLock);
            packet.WriteInt(medal);
            packet.WriteInt(info.Ascension);
            packet.WriteInt(19924);
            packet.WriteInt(info.Gold);
            packet.WriteInt(info.GiftToken);
            packet.WriteInt(info.badLuckNumber);
            packet.WriteInt(info.hardCurrency);
            packet.WriteInt(info.myHonor);
            packet.WriteInt(info.damageScores);
            this.SendTCP(packet);
        }

       /* public void SendUpdatePrivateInfo(PlayerInfo info, int medal)
        {
            GSPacketIn packet = new GSPacketIn((short)38, info.ID);
            packet.WriteInt(info.Money + info.MoneyLock);
            packet.WriteInt(medal);
            packet.WriteInt(info.Ascension);
            packet.WriteInt(19924);
            packet.WriteInt(info.Gold);
            packet.WriteInt(info.GiftToken);
            packet.WriteInt(info.badLuckNumber);
            packet.WriteInt(info.hardCurrency);
            packet.WriteInt(info.myHonor);
            packet.WriteInt(info.damageScores);
            this.SendTCP(packet);
        }*/
     

        public GSPacketIn SendUpdatePublicPlayer(PlayerInfo info, UserMatchInfo matchInfo)
        {
            GSPacketIn packet = new GSPacketIn((short)67, info.ID);
            packet.WriteInt(info.GP);
            packet.WriteInt(info.Offer);
            packet.WriteInt(info.RichesOffer);
            packet.WriteInt(info.RichesRob);
            packet.WriteInt(info.Win);
            packet.WriteInt(info.Total);
            packet.WriteInt(info.Escape);
            packet.WriteInt(info.Attack);
            packet.WriteInt(info.Defence);
            packet.WriteInt(info.Agility);
            packet.WriteInt(info.Luck);
            packet.WriteInt(info.hp);
            packet.WriteInt(info.Hide);
            packet.WriteString(info.Style);
            packet.WriteString(info.Colors);
            packet.WriteString(info.Skin);
            packet.WriteBoolean(info.IsShowConsortia);
            packet.WriteInt(info.ConsortiaID);
            packet.WriteString(info.ConsortiaName);
            packet.WriteInt(info.badgeID);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(info.Nimbus);
            packet.WriteString(info.PvePermission);
            packet.WriteString(info.FightLabPermission);
            packet.WriteInt(info.FightPower);
            packet.WriteInt(info.apprenticeshipState);
            packet.WriteInt(info.masterID);
            packet.WriteString(info.masterOrApprentices);
            packet.WriteInt(info.graduatesCount);
            packet.WriteString(info.honourOfMaster);
            packet.WriteInt(info.AchievementPoint);
            packet.WriteString(info.Honor);
            packet.WriteInt(info.honorId);
            packet.WriteDateTime(info.LastSpaDate);
            packet.WriteInt(info.charmGP);
            packet.WriteInt(0);
            packet.WriteDateTime(info.ShopFinallyGottenTime);
            packet.WriteInt(info.Riches);
            packet.WriteInt(matchInfo.dailyScore);
            packet.WriteInt(matchInfo.dailyWinCount);
            packet.WriteInt(matchInfo.dailyGameCount);
            packet.WriteInt(matchInfo.weeklyScore);
            packet.WriteInt(matchInfo.weeklyGameCount);
            packet.WriteInt(info.Texp.spdTexpExp);
            packet.WriteInt(info.Texp.attTexpExp);
            packet.WriteInt(info.Texp.defTexpExp);
            packet.WriteInt(info.Texp.hpTexpExp);
            packet.WriteInt(info.Texp.lukTexpExp);
            packet.WriteInt(info.Texp.texpTaskCount);
            packet.WriteInt(info.Texp.texpCount);
            packet.WriteDateTime(info.Texp.texpTaskDate);
            packet.WriteInt(0);
            packet.WriteInt(info.evolutionGrade);
            packet.WriteInt(info.evolutionExp);
            this.SendTCP(packet);
            return packet;
        }

          public GSPacketIn SendUpdatePublicPlayer(
             PlayerInfo info,
             UserMatchInfo matchInfo,
             UsersExtraInfo extraInfo)
           {
               GSPacketIn packet = new GSPacketIn((short)67, info.ID);
               packet.WriteInt(info.GP);
               packet.WriteInt(info.Offer);
               packet.WriteInt(info.RichesOffer);
               packet.WriteInt(info.RichesRob);
               packet.WriteInt(info.Win);
               packet.WriteInt(info.Total);
               packet.WriteInt(info.Escape);
               packet.WriteInt(info.Attack);
               packet.WriteInt(info.Defence);
               packet.WriteInt(info.Agility);
               packet.WriteInt(info.Luck);
               packet.WriteInt(info.hp);
               packet.WriteInt(info.Hide);
               packet.WriteString(info.Style);
               packet.WriteString(info.Colors);
               packet.WriteString(info.Skin);
               packet.WriteBoolean(info.IsShowConsortia);
               packet.WriteInt(info.ConsortiaID);
               packet.WriteString(info.ConsortiaName);
               packet.WriteInt(info.badgeID);
               packet.WriteInt(0);
               packet.WriteInt(0);
               packet.WriteInt(info.Nimbus);
               packet.WriteString(info.PvePermission);
               packet.WriteString(info.FightLabPermission);
               packet.WriteInt(info.FightPower);
               packet.WriteInt(info.apprenticeshipState);
               packet.WriteInt(info.masterID);
               packet.WriteString(info.masterOrApprentices);
               packet.WriteInt(info.graduatesCount);
               packet.WriteString(info.honourOfMaster);
               packet.WriteInt(info.AchievementPoint);
               packet.WriteString(info.Honor);
               packet.WriteInt(info.honorId);
               packet.WriteDateTime(info.LastSpaDate);
               packet.WriteInt(info.charmGP);
               packet.WriteInt(0);
               packet.WriteDateTime(info.ShopFinallyGottenTime);
               packet.WriteInt(info.Riches);
               packet.WriteInt(matchInfo.dailyScore);
               packet.WriteInt(matchInfo.dailyWinCount);
               packet.WriteInt(matchInfo.dailyGameCount);
               packet.WriteInt(matchInfo.weeklyScore);
               packet.WriteInt(matchInfo.weeklyGameCount);
               packet.WriteInt(info.Texp.spdTexpExp);
               packet.WriteInt(info.Texp.attTexpExp);
               packet.WriteInt(info.Texp.defTexpExp);
               packet.WriteInt(info.Texp.hpTexpExp);
               packet.WriteInt(info.Texp.lukTexpExp);
               packet.WriteInt(info.Texp.texpTaskCount);
               packet.WriteInt(info.Texp.texpCount);
               packet.WriteDateTime(info.Texp.texpTaskDate);
               packet.WriteInt(0);
               packet.WriteInt(info.evolutionGrade);
               packet.WriteInt(info.evolutionExp);
               this.SendTCP(packet);
               return packet;
           }

        public void SendPingTime(GamePlayer player)
        {
            GSPacketIn packet = new GSPacketIn((short)4);
            player.PingStart = DateTime.Now.Ticks;
            packet.WriteInt(player.PlayerCharacter.AntiAddiction);
            this.SendTCP(packet);
        }

        public GSPacketIn SendNetWork(int id, long delay)
        {
            GSPacketIn packet = new GSPacketIn((short)6, id);
            packet.WriteInt((int)delay / 1000 / 10);
            this.SendTCP(packet);
            return packet;
        }

         public GSPacketIn SendUserEquip(
           PlayerInfo player,
           List<SqlDataProvider.Data.ItemInfo> items,
           List<UserGemStone> userGemStone)
         {
             GSPacketIn packet = new GSPacketIn((short)74, player.ID);
             packet.WriteInt(player.ID);
             packet.WriteString(player.NickName);
             packet.WriteInt(player.Agility);
             packet.WriteInt(player.Attack);
             packet.WriteString(player.Colors);
             packet.WriteString(player.Skin);
             packet.WriteInt(player.Defence);
             packet.WriteInt(player.GP);
             packet.WriteInt(player.Grade);
             packet.WriteInt(player.Luck);
             packet.WriteInt(player.hp);
             packet.WriteInt(player.Hide);
             packet.WriteInt(player.Repute);
             packet.WriteBoolean(player.Sex);
             packet.WriteString(player.Style);
             packet.WriteInt(player.Offer);
             packet.WriteByte(player.typeVIP);
             packet.WriteInt(player.VIPLevel);
             packet.WriteInt(player.Win);
             packet.WriteInt(player.Total);
             packet.WriteInt(player.Escape);
             packet.WriteInt(player.ConsortiaID);
             packet.WriteString(player.ConsortiaName);
             packet.WriteInt(player.badgeID);
             packet.WriteInt(player.RichesOffer);
             packet.WriteInt(player.RichesRob);
             packet.WriteBoolean(player.IsMarried);
             packet.WriteInt(player.SpouseID);
             packet.WriteString(player.SpouseName);
             packet.WriteString(player.DutyName);
             packet.WriteInt(player.Nimbus);
             packet.WriteInt(player.FightPower);
             packet.WriteInt(player.apprenticeshipState);
             packet.WriteInt(player.masterID);
             packet.WriteString(player.masterOrApprentices);
             packet.WriteInt(player.graduatesCount);
             packet.WriteString(player.honourOfMaster);
             packet.WriteInt(player.AchievementPoint);
             packet.WriteString(player.Honor);
             packet.WriteDateTime(DateTime.Now.AddDays(-2.0));
             packet.WriteInt(player.Texp.spdTexpExp);
             packet.WriteInt(player.Texp.attTexpExp);
             packet.WriteInt(player.Texp.defTexpExp);
             packet.WriteInt(player.Texp.hpTexpExp);
             packet.WriteInt(player.Texp.lukTexpExp);
             packet.WriteBoolean(false);
             packet.WriteInt(0);
             packet.WriteInt(player.totemId);
             packet.WriteInt(player.necklaceExp);
             packet.WriteInt(items.Count);
             foreach (SqlDataProvider.Data.ItemInfo itemInfo in items)
             {
                 packet.WriteByte((byte)itemInfo.BagType);
                 packet.WriteInt(itemInfo.UserID);
                 packet.WriteInt(itemInfo.ItemID);
                 packet.WriteInt(itemInfo.Count);
                 packet.WriteInt(itemInfo.Place);
                 packet.WriteInt(itemInfo.TemplateID);
                 packet.WriteInt(itemInfo.AttackCompose);
                 packet.WriteInt(itemInfo.DefendCompose);
                 packet.WriteInt(itemInfo.AgilityCompose);
                 packet.WriteInt(itemInfo.LuckCompose);
                 packet.WriteInt(itemInfo.StrengthenLevel);
                 packet.WriteBoolean(itemInfo.IsBinds);
                 packet.WriteBoolean(itemInfo.IsJudge);
                 packet.WriteDateTime(itemInfo.BeginDate);
                 packet.WriteInt(itemInfo.ValidDate);
                 packet.WriteString(itemInfo.Color);
                 packet.WriteString(itemInfo.Skin);
                 packet.WriteBoolean(itemInfo.IsUsed);
                 packet.WriteInt(itemInfo.Hole1);
                 packet.WriteInt(itemInfo.Hole2);
                 packet.WriteInt(itemInfo.Hole3);
                 packet.WriteInt(itemInfo.Hole4);
                 packet.WriteInt(itemInfo.Hole5);
                 packet.WriteInt(itemInfo.Hole6);
                 packet.WriteString(itemInfo.Pic);
                 packet.WriteInt(itemInfo.RefineryLevel);
                 packet.WriteDateTime(DateTime.Now);
                 packet.WriteByte((byte)itemInfo.Hole5Level);
                 packet.WriteInt(itemInfo.Hole5Exp);
                 packet.WriteByte((byte)itemInfo.Hole6Level);
                 packet.WriteInt(itemInfo.Hole6Exp);
                 packet.WriteBoolean(itemInfo.isGold);
                 if (itemInfo.IsGold)
                 {
                     packet.WriteBoolean(itemInfo.IsGold);
                     packet.WriteInt(itemInfo.goldValidDate);
                     packet.WriteDateTime(itemInfo.goldBeginTime);
                 }                             
             }
             packet.WriteInt(userGemStone.Count);
             for (int index = 0; index < userGemStone.Count; ++index)
             {
                 packet.WriteInt(userGemStone[index].FigSpiritId);
                 packet.WriteString(userGemStone[index].FigSpiritIdValue);
                 packet.WriteInt(userGemStone[index].EquipPlace);
             }
             packet.Compress();
             this.SendTCP(packet);
             return packet;
         }
       
        public void SendDateTime()
        {
            GSPacketIn packet = new GSPacketIn((short)5);
            packet.WriteDateTime(DateTime.Now);
            this.SendTCP(packet);
        }

        public GSPacketIn SendDailyAward(GamePlayer player)
        {
            bool val = false;
            DateTime dateTime = DateTime.Now;
            DateTime date1 = dateTime.Date;
            dateTime = player.PlayerCharacter.LastAward;
            DateTime date2 = dateTime.Date;
            if (date1 != date2)
                val = true;
            GSPacketIn packet = new GSPacketIn((short)13);
            packet.WriteBoolean(val);
            packet.WriteInt(0);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendUpdateRoomList(List<BaseRoom> roomlist)
        {
            GSPacketIn packet = new GSPacketIn((short)94);
            packet.WriteByte((byte)9);
            packet.WriteInt(roomlist.Count);
            int val = roomlist.Count < 8 ? roomlist.Count : 8;
            packet.WriteInt(val);
            for (int index = 0; index < val; ++index)
            {
                BaseRoom baseRoom = roomlist[index];
                packet.WriteInt(baseRoom.RoomId);
                packet.WriteByte((byte)baseRoom.RoomType);
                packet.WriteByte(baseRoom.TimeMode);
                packet.WriteByte((byte)baseRoom.PlayerCount);
                packet.WriteByte((byte)baseRoom.viewerCnt);
                packet.WriteByte((byte)baseRoom.maxViewerCnt);
                packet.WriteByte((byte)baseRoom.PlacesCount);
                packet.WriteBoolean(!string.IsNullOrEmpty(baseRoom.Password));
                packet.WriteInt(baseRoom.MapId);
                packet.WriteBoolean(baseRoom.IsPlaying);
                packet.WriteString(baseRoom.Name);
                packet.WriteByte((byte)baseRoom.GameType);
                packet.WriteByte((byte)baseRoom.HardLevel);
                packet.WriteInt(baseRoom.LevelLimits);
              //  packet.WriteBoolean(baseRoom.isOpenBoss);
            }
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendSceneAddPlayer(GamePlayer player)
        {
            GSPacketIn packet = new GSPacketIn((short)18, player.PlayerCharacter.ID);
            packet.WriteInt(player.PlayerCharacter.Grade);
            packet.WriteBoolean(player.PlayerCharacter.Sex);
            packet.WriteString(player.PlayerCharacter.NickName);
            packet.WriteByte(player.PlayerCharacter.typeVIP);
            packet.WriteInt(player.PlayerCharacter.VIPLevel);
            packet.WriteString(player.PlayerCharacter.ConsortiaName);
            packet.WriteInt(player.PlayerCharacter.Offer);
            packet.WriteInt(player.PlayerCharacter.Win);
            packet.WriteInt(player.PlayerCharacter.Total);
            packet.WriteInt(player.PlayerCharacter.Escape);
            packet.WriteInt(player.PlayerCharacter.ConsortiaID);
            packet.WriteInt(player.PlayerCharacter.Repute);
            packet.WriteBoolean(player.PlayerCharacter.IsMarried);
            if (player.PlayerCharacter.IsMarried)
            {
                packet.WriteInt(player.PlayerCharacter.SpouseID);
                packet.WriteString(player.PlayerCharacter.SpouseName);
            }
            packet.WriteString(player.PlayerCharacter.UserName);
            packet.WriteInt(player.PlayerCharacter.FightPower);
            packet.WriteInt(player.PlayerCharacter.apprenticeshipState);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendSceneRemovePlayer(GamePlayer player)
        {
            GSPacketIn packet = new GSPacketIn((short)21, player.PlayerCharacter.ID);
            this.SendTCP(packet);
            return packet;
        }
        
         public GSPacketIn SendRoomPlayerAdd(GamePlayer player)
          {
              GSPacketIn packet = new GSPacketIn((short)94, player.PlayerId);
              packet.WriteByte((byte)4);
              bool val = false;
              if (player.CurrentRoom.Game != null)
                  val = true;
              packet.WriteBoolean(val);
              packet.WriteByte((byte)player.CurrentRoomIndex);
              packet.WriteByte((byte)player.CurrentRoomTeam);
              packet.WriteBoolean(false);
              packet.WriteInt(player.PlayerCharacter.Grade);
              packet.WriteInt(player.PlayerCharacter.Offer);
              packet.WriteInt(player.PlayerCharacter.Hide);
              packet.WriteInt(player.PlayerCharacter.Repute);
              packet.WriteInt((int)player.PingTime / 1000 / 10);
              packet.WriteInt(player.ZoneId);
              packet.WriteInt(player.PlayerCharacter.ID);
              packet.WriteString(player.PlayerCharacter.NickName);
              packet.WriteByte(player.PlayerCharacter.typeVIP);
              packet.WriteInt(player.PlayerCharacter.VIPLevel);
              packet.WriteBoolean(player.PlayerCharacter.Sex);
              packet.WriteString(player.PlayerCharacter.Style);
              packet.WriteString(player.PlayerCharacter.Colors);
              packet.WriteString(player.PlayerCharacter.Skin);
              SqlDataProvider.Data.ItemInfo itemAt = player.EquipBag.GetItemAt(6);
              packet.WriteInt(itemAt == null ? -1 : itemAt.TemplateID);
              if (player.SecondWeapon == null)
                  packet.WriteInt(0);
              else
                  packet.WriteInt(player.SecondWeapon.TemplateID);
              packet.WriteInt(player.PlayerCharacter.ConsortiaID);
              packet.WriteString(player.PlayerCharacter.ConsortiaName);
              packet.WriteInt(player.PlayerCharacter.badgeID);
              packet.WriteInt(player.PlayerCharacter.Win);
              packet.WriteInt(player.PlayerCharacter.Total);
              packet.WriteInt(player.PlayerCharacter.Escape);
              packet.WriteInt(player.PlayerCharacter.ConsortiaLevel);
              packet.WriteInt(player.PlayerCharacter.ConsortiaRepute);
              packet.WriteBoolean(player.PlayerCharacter.IsMarried);
              if (player.PlayerCharacter.IsMarried)
              {
                  packet.WriteInt(player.PlayerCharacter.SpouseID);
                  packet.WriteString(player.PlayerCharacter.SpouseName);
              }
              packet.WriteString(player.PlayerCharacter.UserName);
              packet.WriteInt(player.PlayerCharacter.Nimbus);
              packet.WriteInt(player.PlayerCharacter.FightPower);
              packet.WriteInt(player.PlayerCharacter.apprenticeshipState);
              packet.WriteInt(player.PlayerCharacter.masterID);
              packet.WriteString(player.PlayerCharacter.masterOrApprentices);
              packet.WriteInt(player.PlayerCharacter.graduatesCount);
              packet.WriteString(player.PlayerCharacter.honourOfMaster);
              packet.WriteBoolean(player.MatchInfo.DailyLeagueFirst);
              packet.WriteInt(player.MatchInfo.DailyLeagueLastScore);
              if (player.Pet == null)
              {
                  packet.WriteInt(0);
              }
              else
              {
                  packet.WriteInt(1);
                  packet.WriteInt(player.Pet.Place);
                  packet.WriteInt(player.Pet.TemplateID);
                  packet.WriteInt(player.Pet.ID);
                  packet.WriteString(player.Pet.Name);
                  packet.WriteInt(player.PlayerCharacter.ID);
                  packet.WriteInt(player.Pet.Level);
                  List<string> skillEquip = player.Pet.GetSkillEquip();
                  packet.WriteInt(skillEquip.Count);
                  foreach (string str in skillEquip)
                  {
                      packet.WriteInt(int.Parse(str.Split(',')[1]));
                      packet.WriteInt(int.Parse(str.Split(',')[0]));
                  }
              }
              this.SendTCP(packet);
              return packet;
          }

        public GSPacketIn SendRoomPlayerRemove(GamePlayer player)
        {
            GSPacketIn packet = new GSPacketIn((short)94, player.PlayerId);
            packet.WriteByte((byte)5);
            packet.Parameter1 = player.PlayerId;
            packet.ClientID = player.PlayerId;
            packet.WriteInt(player.ZoneId);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendRoomUpdatePlayerStates(byte[] states)
        {
            GSPacketIn packet = new GSPacketIn((short)94);
            packet.WriteByte((byte)15);
            for (int index = 0; index < states.Length; ++index)
                packet.WriteByte(states[index]);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendRoomUpdatePlacesStates(int[] states)
        {
            GSPacketIn packet = new GSPacketIn((short)94);
            packet.WriteByte((byte)10);
            for (int index = 0; index < states.Length; ++index)
                packet.WriteInt(states[index]);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendRoomPlayerChangedTeam(GamePlayer player)
        {
            GSPacketIn packet = new GSPacketIn((short)94, player.PlayerId);
            packet.WriteByte((byte)6);
            packet.WriteByte((byte)player.CurrentRoomTeam);
            packet.WriteByte((byte)player.CurrentRoomIndex);
            this.SendTCP(packet);
            return packet;
        }

         public GSPacketIn SendRoomCreate(BaseRoom room)
         {
             GSPacketIn packet = new GSPacketIn((short)94);
             packet.WriteByte((byte)0);
             packet.WriteInt(room.RoomId);
             packet.WriteByte((byte)room.RoomType);
             packet.WriteByte((byte)room.HardLevel);
             packet.WriteByte(room.TimeMode);
             packet.WriteByte((byte)room.PlayerCount);
             packet.WriteByte((byte)room.viewerCnt);
             packet.WriteByte((byte)room.PlacesCount);
             packet.WriteBoolean(!string.IsNullOrEmpty(room.Password));
             packet.WriteInt(room.MapId);
             packet.WriteBoolean(room.IsPlaying);
             packet.WriteString(room.Name);
             packet.WriteByte((byte)room.GameType);
             packet.WriteInt(room.LevelLimits);
             packet.WriteBoolean(false);
             packet.WriteBoolean(false);
             this.SendTCP(packet);
             return packet;
         }

       /* public GSPacketIn SendRoomCreate(BaseRoom room)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(94);
            gSPacketIn.WriteByte(0);
            gSPacketIn.WriteInt(room.RoomId);
            gSPacketIn.WriteByte((byte)room.RoomType);
            gSPacketIn.WriteByte((byte)room.HardLevel);
            gSPacketIn.WriteByte(room.TimeMode);
            gSPacketIn.WriteByte((byte)room.PlayerCount);
            gSPacketIn.WriteByte((byte)room.viewerCnt);
            gSPacketIn.WriteByte((byte)room.PlacesCount);
            gSPacketIn.WriteBoolean(!string.IsNullOrEmpty(room.Password));
            gSPacketIn.WriteInt(room.MapId);
            gSPacketIn.WriteBoolean(room.IsPlaying);
            gSPacketIn.WriteString(room.Name);
            gSPacketIn.WriteByte((byte)room.GameType);
            gSPacketIn.WriteInt(room.LevelLimits);
            gSPacketIn.WriteBoolean(room.isCrosszone);
            gSPacketIn.WriteBoolean(room.isWithinLeageTime);
            gSPacketIn.WriteBoolean(room.isOpenBoss);
            gSPacketIn.WriteString(room.Pic);
            gSPacketIn.WriteBoolean(false);
            this.SendTCP(gSPacketIn);
            return gSPacketIn;
        }*/

        public GSPacketIn SendSingleRoomCreate(BaseRoom room, int zoneId)
        {
            GSPacketIn packet = new GSPacketIn((short)76);
            packet.WriteInt(room.RoomId);
            packet.WriteByte((byte)room.RoomType);
            packet.WriteBoolean(room.IsPlaying);
            packet.WriteByte((byte)room.GameType);
            packet.WriteInt(room.MapId);
            packet.WriteBoolean(room.isCrosszone);
            packet.WriteInt(room.ZoneId);
            this.SendTCP(packet);
            return packet;
        }

      /*  public GSPacketIn SendSingleRoomCreate(BaseRoom room, int zoneId)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(94);
            gSPacketIn.WriteByte(18);
            gSPacketIn.WriteInt(room.RoomId);
            gSPacketIn.WriteByte((byte)room.RoomType);
            gSPacketIn.WriteBoolean(room.IsPlaying);
            gSPacketIn.WriteByte((byte)room.GameType);
            gSPacketIn.WriteInt(room.MapId);
            gSPacketIn.WriteBoolean(room.isCrosszone);
            gSPacketIn.WriteInt(room.ZoneId);
            gSPacketIn.WriteBoolean(false);
            this.SendTCP(gSPacketIn);
            return gSPacketIn;
        }
        */
        public GSPacketIn SendRoomLoginResult(bool result)
        {
            GSPacketIn packet = new GSPacketIn((short)94);
            packet.WriteByte((byte)1);
            packet.WriteBoolean(result);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendRoomPairUpStart(BaseRoom room)
        {
            GSPacketIn packet = new GSPacketIn((short)94);
            packet.WriteByte((byte)13);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendGameRoomInfo(GamePlayer player, BaseRoom game) => new GSPacketIn((short)94, player.PlayerCharacter.ID);

        public GSPacketIn SendRoomType(GamePlayer player, BaseRoom game)
        {
            GSPacketIn packet = new GSPacketIn((short)94);
            packet.WriteByte((byte)12);
            packet.WriteByte((byte)game.GameType);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendRoomPairUpCancel(BaseRoom room)
        {
            GSPacketIn packet = new GSPacketIn((short)94);
            packet.WriteByte((byte)11);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendRoomClear(GamePlayer player, BaseRoom game)
        {
            GSPacketIn packet = new GSPacketIn((short)96, player.PlayerCharacter.ID);
            packet.WriteInt(game.RoomId);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendEquipChange(
          GamePlayer player,
          int place,
          int goodsID,
          string style)
        {
            GSPacketIn packet = new GSPacketIn((short)66, player.PlayerCharacter.ID);
            packet.WriteByte((byte)place);
            packet.WriteInt(goodsID);
            packet.WriteString(style);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendRoomChange(BaseRoom room)
        {
            GSPacketIn packet = new GSPacketIn((short)94);
            packet.WriteByte((byte)2);
            packet.WriteInt(room.MapId);
            packet.WriteByte((byte)room.RoomType);
            packet.WriteString(room.Password);
            packet.WriteString(room.Name);
            packet.WriteByte(room.TimeMode);
            packet.WriteByte((byte)room.HardLevel);
            packet.WriteInt(room.LevelLimits);
            packet.WriteBoolean(room.isCrosszone);
            this.SendTCP(packet);
            return packet;
        }

       /* public GSPacketIn SendGameRoomSetupChange(BaseRoom room)
        {
            GSPacketIn packet = new GSPacketIn((short)94);
            packet.WriteByte((byte)2);
            packet.WriteInt(room.MapId);
            packet.WriteByte((byte)room.RoomType);
            packet.WriteString(room.Password == null ? "" : room.Password);
            packet.WriteString(room.Name == null ? "Gunny1" : room.Name);
            packet.WriteByte(room.TimeMode);
            packet.WriteByte((byte)room.HardLevel);
            packet.WriteInt(room.LevelLimits);
            packet.WriteBoolean(room.isCrosszone);           
            return packet;
        }*/

        public GSPacketIn SendGameRoomSetupChange(BaseRoom room)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(94);
            gSPacketIn.WriteByte(2);
            /*gSPacketIn.WriteBoolean(room.isOpenBoss);   
			if (room.isOpenBoss)
			{
				gSPacketIn.WriteString(room.Pic);
			}*/
            gSPacketIn.WriteInt(room.MapId);
            gSPacketIn.WriteByte((byte)room.RoomType);
            gSPacketIn.WriteString((room.Password == null) ? "" : room.Password);
            gSPacketIn.WriteString((room.Name == null) ? "GunnyII" : room.Name);
            gSPacketIn.WriteByte(room.TimeMode);
            gSPacketIn.WriteByte((byte)room.HardLevel);
            gSPacketIn.WriteInt(room.LevelLimits);
            gSPacketIn.WriteBoolean(room.isCrosszone);
            this.SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendFusionPreview(
          GamePlayer player,
          Dictionary<int, double> previewItemList,
          bool isbind,
          int MinValid)
        {
            GSPacketIn packet = new GSPacketIn((short)76, player.PlayerCharacter.ID);
            packet.WriteInt(previewItemList.Count);
            foreach (KeyValuePair<int, double> previewItem in previewItemList)
            {
                packet.WriteInt(previewItem.Key);
                packet.WriteInt(MinValid);
                int int32 = Convert.ToInt32(previewItem.Value);
                packet.WriteInt(int32 > 100 ? 100 : (int32 < 0 ? 0 : int32));
            }
            packet.WriteBoolean(isbind);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendFusionResult(GamePlayer player, bool result)
        {
            GSPacketIn packet = new GSPacketIn((short)78, player.PlayerCharacter.ID);
            packet.WriteInt(2);
            packet.WriteBoolean(result);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendRefineryPreview(
          GamePlayer player,
          int templateid,
          bool isbind,
          SqlDataProvider.Data.ItemInfo item)
        {
            GSPacketIn packet = new GSPacketIn((short)111, player.PlayerCharacter.ID);
            packet.WriteInt(templateid);
            packet.WriteInt(item.ValidDate);
            packet.WriteBoolean(isbind);
            packet.WriteInt(item.AgilityCompose);
            packet.WriteInt(item.AttackCompose);
            packet.WriteInt(item.DefendCompose);
            packet.WriteInt(item.LuckCompose);
            this.SendTCP(packet);
            return packet;
        }

        public void SendUpdateInventorySlot(PlayerInventory bag, int[] updatedSlots)
        {
            if (m_gameClient.Player == null)
            {
                return;
            }
            GSPacketIn packet = new GSPacketIn(64, m_gameClient.Player.PlayerCharacter.ID, 10240);
            packet.WriteInt(bag.BagType);
            packet.WriteInt(updatedSlots.Length);
            foreach (int updatedSlot in updatedSlots)
            {
                packet.WriteInt(updatedSlot);
                ItemInfo itemAt = bag.GetItemAt(updatedSlot);
                if (itemAt == null)
                {
                    packet.WriteBoolean(val: false);
                    continue;
                }
                packet.WriteBoolean(val: true);
                packet.WriteInt(itemAt.UserID);
                packet.WriteInt(itemAt.ItemID);
                packet.WriteInt(itemAt.Count);
                packet.WriteInt(itemAt.Place);
                packet.WriteInt(itemAt.TemplateID);
                packet.WriteInt(itemAt.AttackCompose);
                packet.WriteInt(itemAt.DefendCompose);
                packet.WriteInt(itemAt.AgilityCompose);
                packet.WriteInt(itemAt.LuckCompose);
                packet.WriteInt(itemAt.StrengthenLevel);
                packet.WriteInt(itemAt.StrengthenExp);
                packet.WriteBoolean(itemAt.IsBinds);
                packet.WriteBoolean(itemAt.IsJudge);
                packet.WriteDateTime(itemAt.BeginDate);
                packet.WriteInt(itemAt.ValidDate);
                packet.WriteString((itemAt.Color == null) ? "" : itemAt.Color);
                packet.WriteString((itemAt.Skin == null) ? "" : itemAt.Skin);
                packet.WriteBoolean(itemAt.IsUsed);
                packet.WriteInt(itemAt.Hole1);
                packet.WriteInt(itemAt.Hole2);
                packet.WriteInt(itemAt.Hole3);
                packet.WriteInt(itemAt.Hole4);
                packet.WriteInt(itemAt.Hole5);
                packet.WriteInt(itemAt.Hole6);
                packet.WriteString(itemAt.Pic);
                packet.WriteInt(itemAt.RefineryLevel);
                packet.WriteDateTime(DateTime.Now.AddDays(5.0));
                packet.WriteInt(itemAt.StrengthenTimes);
                packet.WriteByte((byte)itemAt.Hole5Level);
                packet.WriteInt(itemAt.Hole5Exp);
                packet.WriteByte((byte)itemAt.Hole6Level);
                packet.WriteInt(itemAt.Hole6Exp);
                packet.WriteBoolean(itemAt.isGold);
                if (itemAt.isGold)
                {
                    packet.WriteInt(itemAt.goldValidDate);
                    packet.WriteDateTime(itemAt.goldBeginTime);
                }
                packet.WriteString(itemAt.latentEnergyCurStr);
                packet.WriteString(itemAt.latentEnergyCurStr);
                packet.WriteDateTime(itemAt.latentEnergyEndTime);
            }
            SendTCP(packet);
        }

        public void SendUpdateCardData(CardInventory bag, int[] updatedSlots)
        {
            if (this.m_gameClient.Player == null)
                return;
            GSPacketIn packet = new GSPacketIn((short)216, this.m_gameClient.Player.PlayerCharacter.ID);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.ID);
            packet.WriteInt(updatedSlots.Length);
            foreach (int updatedSlot in updatedSlots)
            {
                packet.WriteInt(updatedSlot);
                UsersCardInfo itemAt = bag.GetItemAt(updatedSlot);
                if (itemAt != null && itemAt.TemplateID != 0)
                {
                    packet.WriteBoolean(true);
                    packet.WriteInt(itemAt.CardID);
                    packet.WriteInt(itemAt.UserID);
                    packet.WriteInt(itemAt.Count);
                    packet.WriteInt(itemAt.Place);
                    packet.WriteInt(itemAt.TemplateID);
                    packet.WriteInt(itemAt.Attack);
                    packet.WriteInt(itemAt.Defence);
                    packet.WriteInt(itemAt.Agility);
                    packet.WriteInt(itemAt.Luck);
                    packet.WriteInt(itemAt.Damage);
                    packet.WriteInt(itemAt.Guard);
                    packet.WriteInt(itemAt.Level);
                    packet.WriteInt(itemAt.CardGP);
                    packet.WriteBoolean(itemAt.isFirstGet);
                }
                else
                    packet.WriteBoolean(false);
            }
            this.SendTCP(packet);
        }

        public void SendUpdateCardData(PlayerInfo player, List<UsersCardInfo> userCard)
        {
            if (this.m_gameClient.Player == null)
                return;
            GSPacketIn packet = new GSPacketIn((short)216, player.ID);
            packet.WriteInt(player.ID);
            packet.WriteInt(userCard.Count);
            foreach (UsersCardInfo usersCardInfo in userCard)
            {
                packet.WriteInt(usersCardInfo.Place);
                packet.WriteBoolean(true);
                packet.WriteInt(usersCardInfo.CardID);
                packet.WriteInt(usersCardInfo.UserID);
                packet.WriteInt(usersCardInfo.Count);
                packet.WriteInt(usersCardInfo.Place);
                packet.WriteInt(usersCardInfo.TemplateID);
                packet.WriteInt(usersCardInfo.Attack);
                packet.WriteInt(usersCardInfo.Defence);
                packet.WriteInt(usersCardInfo.Agility);
                packet.WriteInt(usersCardInfo.Luck);
                packet.WriteInt(usersCardInfo.Damage);
                packet.WriteInt(usersCardInfo.Guard);
                packet.WriteInt(usersCardInfo.Level);
                packet.WriteInt(usersCardInfo.CardGP);
                packet.WriteBoolean(usersCardInfo.isFirstGet);
            }
            this.SendTCP(packet);
        }

        public GSPacketIn SendUpdateQuests(
          GamePlayer player,
          byte[] states,
          BaseQuest[] infos)
        {
            if (player == null || states == null || infos == null)
                return (GSPacketIn)null;
            GSPacketIn packet = new GSPacketIn((short)178, player.PlayerCharacter.ID);
            packet.WriteInt(infos.Length);
            foreach (BaseQuest info in infos)
            {
                packet.WriteInt(info.Data.QuestID);
                packet.WriteBoolean(info.Data.IsComplete);
                packet.WriteInt(info.Data.Condition1);
                packet.WriteInt(info.Data.Condition2);
                packet.WriteInt(info.Data.Condition3);
                packet.WriteInt(info.Data.Condition4);
                packet.WriteDateTime(info.Data.CompletedDate.Date);
                packet.WriteInt(info.Data.RepeatFinish);
                packet.WriteInt(info.Data.RandDobule);
                packet.WriteBoolean(info.Data.IsExist);
            }
            packet.Write(states);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendUpdateBuffer(GamePlayer player, AbstractBuffer[] infos)
        {
            GSPacketIn packet = new GSPacketIn((short)185, player.PlayerId);
            packet.WriteInt(infos.Length);
            foreach (AbstractBuffer info in infos)
            {
                packet.WriteInt(info.Info.Type);
                packet.WriteBoolean(info.Info.IsExist);
                packet.WriteDateTime(info.Info.BeginDate);
                if (info.IsPayBuff())
                    packet.WriteInt(info.Info.ValidDate / 60 / 24);
                else
                    packet.WriteInt(info.Info.ValidDate);
                packet.WriteInt(info.Info.Value);
                packet.WriteInt(info.Info.ValidCount);
                packet.WriteInt(info.Info.TemplateID);
            }
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendUpdateBuffer(GamePlayer player, BufferInfo[] infos)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(185, player.PlayerId);
            gSPacketIn.WriteInt(infos.Length);
            for (int i = 0; i < infos.Length; i++)
            {
                BufferInfo bufferInfo = infos[i];
                gSPacketIn.WriteInt(bufferInfo.Type);
                gSPacketIn.WriteBoolean(bufferInfo.IsExist);
                gSPacketIn.WriteDateTime(bufferInfo.BeginDate);
                gSPacketIn.WriteInt(bufferInfo.ValidDate);
                gSPacketIn.WriteInt(bufferInfo.Value);
                gSPacketIn.WriteInt(bufferInfo.ValidCount);
                gSPacketIn.WriteInt(bufferInfo.TemplateID);
            }
            this.SendTCP(gSPacketIn);
            return gSPacketIn;
        }
        public GSPacketIn SendUpdateConsotiaBuffer(GamePlayer player, Dictionary<int, BufferInfo> bufflist)
        {
            List<ConsortiaBuffTempInfo> allConsortiaBuff = ConsortiaExtraMgr.GetAllConsortiaBuff();
            GSPacketIn gSPacketIn = new GSPacketIn(129, player.PlayerId);
            gSPacketIn.WriteByte(26);
            gSPacketIn.WriteInt(allConsortiaBuff.Count);
            foreach (ConsortiaBuffTempInfo current in allConsortiaBuff)
            {
                if (bufflist.ContainsKey(current.id))
                {
                    BufferInfo bufferInfo = bufflist[current.id];
                    gSPacketIn.WriteInt(bufferInfo.TemplateID);
                    gSPacketIn.WriteBoolean(true);
                    gSPacketIn.WriteDateTime(bufferInfo.BeginDate);
                    gSPacketIn.WriteInt(bufferInfo.ValidDate / 24 / 60);
                }
                else
                {
                    gSPacketIn.WriteInt(current.id);
                    gSPacketIn.WriteBoolean(false);
                    gSPacketIn.WriteDateTime(DateTime.Now);
                    gSPacketIn.WriteInt(0);
                }
            }
            this.SendTCP(gSPacketIn);
            return gSPacketIn;
        }
        public GSPacketIn SendBufferList(GamePlayer player, List<AbstractBuffer> infos)
        {
            GSPacketIn packet = new GSPacketIn((short)186, player.PlayerId);
            packet.WriteInt(infos.Count);
            foreach (AbstractBuffer info1 in infos)
            {
                BufferInfo info2 = info1.Info;
                packet.WriteInt(info2.Type);
                packet.WriteBoolean(info2.IsExist);
                packet.WriteDateTime(info2.BeginDate);
                if (info1.IsPayBuff())
                    packet.WriteInt(info2.ValidDate / 60 / 24);
                else
                    packet.WriteInt(info2.ValidDate);
                packet.WriteInt(info2.Value);
                packet.WriteInt(info2.ValidCount);
                packet.WriteInt(info2.TemplateID);
            }
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendMailResponse(int playerID, eMailRespose type)
        {
            GSPacketIn packet = new GSPacketIn((short)117);
            packet.WriteInt(playerID);
            packet.WriteInt((int)type);
            GameServer.Instance.LoginServer.SendPacket(packet);
            return packet;
        }

        public GSPacketIn SendConsortiaLevelUp(
          byte type,
          byte level,
          bool result,
          string msg,
          int playerid)
        {
            GSPacketIn packet = new GSPacketIn((short)159, playerid);
            packet.WriteByte(type);
            packet.WriteByte(level);
            packet.WriteBoolean(result);
            packet.WriteString(msg);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendAuctionRefresh(
          AuctionInfo info,
          int auctionID,
          bool isExist,
          SqlDataProvider.Data.ItemInfo item)
        {
            GSPacketIn packet = new GSPacketIn((short)195);
            packet.WriteInt(auctionID);
            packet.WriteBoolean(isExist);
            if (isExist)
            {
                packet.WriteInt(info.AuctioneerID);
                packet.WriteString(info.AuctioneerName);
                packet.WriteDateTime(info.BeginDate);
                packet.WriteInt(info.BuyerID);
                packet.WriteString(info.BuyerName);
                packet.WriteInt(info.ItemID);
                packet.WriteInt(info.Mouthful);
                packet.WriteInt(info.PayType);
                packet.WriteInt(info.Price);
                packet.WriteInt(info.Rise);
                packet.WriteInt(info.ValidDate);
                packet.WriteBoolean(item != null);
                if (item != null)
                {
                    packet.WriteInt(item.Count);
                    packet.WriteInt(item.TemplateID);
                    packet.WriteInt(item.AttackCompose);
                    packet.WriteInt(item.DefendCompose);
                    packet.WriteInt(item.AgilityCompose);
                    packet.WriteInt(item.LuckCompose);
                    packet.WriteInt(item.StrengthenLevel);
                    packet.WriteBoolean(item.IsBinds);
                    packet.WriteBoolean(item.IsJudge);
                    packet.WriteDateTime(item.BeginDate);
                    packet.WriteInt(item.ValidDate);
                    packet.WriteString(item.Color);
                    packet.WriteString(item.Skin);
                    packet.WriteBoolean(item.IsUsed);
                    packet.WriteInt(item.Hole1);
                    packet.WriteInt(item.Hole2);
                    packet.WriteInt(item.Hole3);
                    packet.WriteInt(item.Hole4);
                    packet.WriteInt(item.Hole5);
                    packet.WriteInt(item.Hole6);
                    packet.WriteString(item.Pic);
                    packet.WriteInt(item.RefineryLevel);
                    packet.WriteDateTime(DateTime.Now);
                    packet.WriteByte((byte)item.Hole5Level);
                    packet.WriteInt(item.Hole5Exp);
                    packet.WriteByte((byte)item.Hole6Level);
                    packet.WriteInt(item.Hole6Exp);
                }
            }
            packet.Compress();
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendAASState(bool result)
        {
            GSPacketIn packet = new GSPacketIn((short)224);
            packet.WriteBoolean(result);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendIDNumberCheck(bool result)
        {
            GSPacketIn packet = new GSPacketIn((short)226);
            packet.WriteBoolean(result);
            this.SendTCP(packet);
            return packet;
        }

        public void SendExpBlessedData(int PlayerId)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(155, PlayerId);
            gSPacketIn.WriteByte(8);
            gSPacketIn.WriteInt(0);
            this.SendTCP(gSPacketIn);
        }
        public GSPacketIn SendAASInfoSet(bool result)
        {
            GSPacketIn packet = new GSPacketIn((short)224);
            packet.WriteBoolean(result);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendAASControl(bool result, bool bool_0, bool IsMinor)
        {
            GSPacketIn packet = new GSPacketIn((short)227);
            packet.WriteBoolean(true);
            packet.WriteInt(1);
            packet.WriteBoolean(true);
            packet.WriteBoolean(IsMinor);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendMarryRoomInfo(GamePlayer player, MarryRoom room)
        {
            GSPacketIn packet = new GSPacketIn((short)241, player.PlayerCharacter.ID);
            bool val = room != null;
            packet.WriteBoolean(val);
            if (val)
            {
                packet.WriteInt(room.Info.ID);
                packet.WriteBoolean(room.Info.IsHymeneal);
                packet.WriteString(room.Info.Name);
                packet.WriteBoolean(!(room.Info.Pwd == ""));
                packet.WriteInt(room.Info.MapIndex);
                packet.WriteInt(room.Info.AvailTime);
                packet.WriteInt(room.Count);
                packet.WriteInt(room.Info.PlayerID);
                packet.WriteString(room.Info.PlayerName);
                packet.WriteInt(room.Info.GroomID);
                packet.WriteString(room.Info.GroomName);
                packet.WriteInt(room.Info.BrideID);
                packet.WriteString(room.Info.BrideName);
                packet.WriteDateTime(room.Info.BeginTime);
                packet.WriteByte((byte)room.RoomState);
                packet.WriteString(room.Info.RoomIntroduction);
            }
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendMarryRoomLogin(GamePlayer player, bool result)
        {
            GSPacketIn packet = new GSPacketIn((short)242, player.PlayerCharacter.ID);
            packet.WriteBoolean(result);
            if (result)
            {
                packet.WriteInt(player.CurrentMarryRoom.Info.ID);
                packet.WriteString(player.CurrentMarryRoom.Info.Name);
                packet.WriteInt(player.CurrentMarryRoom.Info.MapIndex);
                packet.WriteInt(player.CurrentMarryRoom.Info.AvailTime);
                packet.WriteInt(player.CurrentMarryRoom.Count);
                packet.WriteInt(player.CurrentMarryRoom.Info.PlayerID);
                packet.WriteString(player.CurrentMarryRoom.Info.PlayerName);
                packet.WriteInt(player.CurrentMarryRoom.Info.GroomID);
                packet.WriteString(player.CurrentMarryRoom.Info.GroomName);
                packet.WriteInt(player.CurrentMarryRoom.Info.BrideID);
                packet.WriteString(player.CurrentMarryRoom.Info.BrideName);
                packet.WriteDateTime(player.CurrentMarryRoom.Info.BeginTime);
                packet.WriteBoolean(player.CurrentMarryRoom.Info.IsHymeneal);
                packet.WriteByte((byte)player.CurrentMarryRoom.RoomState);
                packet.WriteString(player.CurrentMarryRoom.Info.RoomIntroduction);
                packet.WriteBoolean(player.CurrentMarryRoom.Info.GuestInvite);
                packet.WriteInt(player.MarryMap);
                packet.WriteBoolean(player.CurrentMarryRoom.Info.IsGunsaluteUsed);
            }
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendPlayerEnterMarryRoom(GamePlayer player)
        {
            GSPacketIn packet = new GSPacketIn((short)243, player.PlayerCharacter.ID);
            packet.WriteInt(player.PlayerCharacter.Grade);
            packet.WriteInt(player.PlayerCharacter.Hide);
            packet.WriteInt(player.PlayerCharacter.Repute);
            packet.WriteInt(player.PlayerCharacter.ID);
            packet.WriteString(player.PlayerCharacter.NickName);
            packet.WriteByte(player.PlayerCharacter.typeVIP);
            packet.WriteInt(player.PlayerCharacter.VIPLevel);
            packet.WriteBoolean(player.PlayerCharacter.Sex);
            packet.WriteString(player.PlayerCharacter.Style);
            packet.WriteString(player.PlayerCharacter.Colors);
            packet.WriteString(player.PlayerCharacter.Skin);
            packet.WriteInt(player.X);
            packet.WriteInt(player.Y);
            packet.WriteInt(player.PlayerCharacter.FightPower);
            packet.WriteInt(player.PlayerCharacter.Win);
            packet.WriteInt(player.PlayerCharacter.Total);
            packet.WriteInt(player.PlayerCharacter.Offer);
            this.SendTCP(packet);
            return packet;
        }
        public GSPacketIn SendPlayerFigSpiritinit(int ID, List<UserGemStone> gems)
        {
            GSPacketIn packet = new GSPacketIn((short)209, ID);
            packet.WriteByte((byte)1);
            packet.WriteBoolean(true);
            packet.WriteInt(gems.Count);
            foreach (UserGemStone gem in gems)
            {
                packet.WriteInt(gem.UserID);
                packet.WriteInt(gem.FigSpiritId);
                packet.WriteString(gem.FigSpiritIdValue);
                packet.WriteInt(gem.EquipPlace);
            }
            this.SendTCP(packet);
            return packet;
        }
        public GSPacketIn SendPlayerFigSpiritUp(
             int ID,
             UserGemStone gem,
             bool isUp,
             bool isMaxLevel,
             bool isFall,
             int num,
             int dir)
        {
            GSPacketIn packet = new GSPacketIn((short)209, ID);
            packet.WriteByte((byte)2);
            string[] strArray = gem.FigSpiritIdValue.Split('|');
            packet.WriteBoolean(isUp);
            packet.WriteBoolean(isMaxLevel);
            packet.WriteBoolean(isFall);
            packet.WriteInt(num);
            packet.WriteInt(strArray.Length);
            for (int index = 0; index < strArray.Length; ++index)
            {
                string str = strArray[index];
                packet.WriteInt(gem.FigSpiritId);
                packet.WriteInt(Convert.ToInt32(str.Split(',')[0]));
                packet.WriteInt(Convert.ToInt32(str.Split(',')[1]));
                packet.WriteInt(Convert.ToInt32(str.Split(',')[2]));
            }
            packet.WriteInt(gem.EquipPlace);
            packet.WriteInt(dir);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendMarryInfoRefresh(MarryInfo info, int ID, bool isExist)
        {
            GSPacketIn packet = new GSPacketIn((short)239);
            packet.WriteInt(ID);
            packet.WriteBoolean(isExist);
            if (isExist)
            {
                packet.WriteInt(info.UserID);
                packet.WriteBoolean(info.IsPublishEquip);
                packet.WriteString(info.Introduction);
            }
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendPlayerMarryStatus(
          GamePlayer player,
          int userID,
          bool isMarried)
        {
            GSPacketIn packet = new GSPacketIn((short)246, player.PlayerCharacter.ID);
            packet.WriteInt(userID);
            packet.WriteBoolean(isMarried);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendPlayerMarryApply(
          GamePlayer player,
          int userID,
          string userName,
          string loveProclamation,
          int id)
        {
            GSPacketIn packet = new GSPacketIn((short)247, player.PlayerCharacter.ID);
            packet.WriteInt(userID);
            packet.WriteString(userName);
            packet.WriteString(loveProclamation);
            packet.WriteInt(id);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendPlayerDivorceApply(
          GamePlayer player,
          bool result,
          bool isProposer)
        {
            GSPacketIn packet = new GSPacketIn((short)248, player.PlayerCharacter.ID);
            packet.WriteBoolean(result);
            packet.WriteBoolean(isProposer);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendMarryApplyReply(
          GamePlayer player,
          int UserID,
          string UserName,
          bool result,
          bool isApplicant,
          int id)
        {
            GSPacketIn packet = new GSPacketIn((short)250, player.PlayerCharacter.ID);
            packet.WriteInt(UserID);
            packet.WriteBoolean(result);
            packet.WriteString(UserName);
            packet.WriteBoolean(isApplicant);
            packet.WriteInt(id);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendBigSpeakerMsg(GamePlayer player, string msg)
        {
            GSPacketIn packet = new GSPacketIn((short)72, player.PlayerCharacter.ID);
            packet.WriteInt(player.PlayerCharacter.ID);
            packet.WriteString(player.PlayerCharacter.NickName);
            packet.WriteString(msg);
            GameServer.Instance.LoginServer.SendPacket(packet);
            foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
                allPlayer.Out.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendPlayerLeaveMarryRoom(GamePlayer player)
        {
            GSPacketIn packet = new GSPacketIn((short)244, player.PlayerCharacter.ID);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendMarryRoomInfoToPlayer(
          GamePlayer player,
          bool state,
          MarryRoomInfo info)
        {
            GSPacketIn packet = new GSPacketIn((short)252, player.PlayerCharacter.ID);
            packet.WriteBoolean(state);
            if (state)
            {
                packet.WriteInt(info.ID);
                packet.WriteString(info.Name);
                packet.WriteInt(info.MapIndex);
                packet.WriteInt(info.AvailTime);
                packet.WriteInt(info.PlayerID);
                packet.WriteInt(info.GroomID);
                packet.WriteInt(info.BrideID);
                packet.WriteDateTime(info.BeginTime);
                packet.WriteBoolean(info.IsGunsaluteUsed);
            }
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendMarryInfo(GamePlayer player, MarryInfo info)
        {
            GSPacketIn packet = new GSPacketIn((short)235, player.PlayerCharacter.ID);
            packet.WriteString(info.Introduction);
            packet.WriteBoolean(info.IsPublishEquip);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendContinuation(GamePlayer player, MarryRoomInfo info)
        {
            GSPacketIn packet = new GSPacketIn((short)249, player.PlayerCharacter.ID);
            packet.WriteByte((byte)3);
            packet.WriteInt(info.AvailTime);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendMarryProp(GamePlayer player, MarryProp info)
        {
            GSPacketIn packet = new GSPacketIn((short)234, player.PlayerCharacter.ID);
            packet.WriteBoolean(info.IsMarried);
            packet.WriteInt(info.SpouseID);
            packet.WriteString(info.SpouseName);
            packet.WriteBoolean(info.IsCreatedMarryRoom);
            packet.WriteInt(info.SelfMarryRoomID);
            packet.WriteBoolean(info.IsGotRing);
            this.SendTCP(packet);
            return packet;
        }

        public void SendWeaklessGuildProgress(PlayerInfo player)
        {
            GSPacketIn packet = new GSPacketIn((short)15, player.ID);
            packet.WriteInt(player.weaklessGuildProgress.Length);
            for (int index = 0; index < player.weaklessGuildProgress.Length; ++index)
                packet.WriteByte(player.weaklessGuildProgress[index]);
            this.SendTCP(packet);
        }

        public void SendUserLuckyNum()
        {
            GSPacketIn packet = new GSPacketIn((short)161);
            packet.WriteInt(1);
            packet.WriteString("");
            this.SendTCP(packet);
        }

        public GSPacketIn SendUserRanks2(List<UserRankInfo> rankList)
        {
            GSPacketIn packet = new GSPacketIn((short)34);
            packet.WriteInt(rankList.Count);
            foreach (UserRankInfo rank in rankList)
                packet.WriteString(rank.Name);
            this.SendTCP(packet);
            return packet;
        }
        public GSPacketIn SendUserRanks(int Id, List<UserRankInfo> ranks)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(34, Id);
            gSPacketIn.WriteInt(ranks.Count);
            foreach (UserRankInfo rank in ranks)
            {
                gSPacketIn.WriteInt(rank.NewTitleID);
                gSPacketIn.WriteString(rank.Name);
                gSPacketIn.WriteDateTime(rank.BeginDate);
                gSPacketIn.WriteDateTime(rank.EndDate);
            }
            this.SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendUpdatePlayerProperty(PlayerInfo info, PlayerProperty PlayerProp)
        {
            string[] strArray = new string[4]
            {
        "Attack",
        "Defence",
        "Agility",
        "Luck"
            };
            GSPacketIn packet = new GSPacketIn((short)167, info.ID);
            packet.WriteInt(this.m_gameClient.Player.PlayerCharacter.ID);
            foreach (string key in strArray)
            {
                packet.WriteInt(0);
                packet.WriteInt(PlayerProp.Current["Texp"][key]);
                packet.WriteInt(PlayerProp.Current["Card"][key]);
            }
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(PlayerProp.Current["Damage"]["Bead"]);
            packet.WriteInt(PlayerProp.Current["Armor"]["Bead"]);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendOpenVIP(GamePlayer Player)
        {
            GSPacketIn packet = new GSPacketIn((short)92, Player.PlayerCharacter.ID);
            packet.WriteByte(Player.PlayerCharacter.typeVIP);
            packet.WriteInt(Player.PlayerCharacter.VIPLevel);
            packet.WriteInt(Player.PlayerCharacter.VIPExp);
            packet.WriteDateTime(Player.PlayerCharacter.VIPExpireDay);
            packet.WriteDateTime(Player.PlayerCharacter.LastDate);
            packet.WriteInt(Player.PlayerCharacter.VIPNextLevelDaysNeeded);
            packet.WriteBoolean(Player.PlayerCharacter.CanTakeVipReward);
            this.SendTCP(packet);
            return packet;
        }

        public void SendLittleGameActived()
        {
            GSPacketIn packet = new GSPacketIn((short)80);

            packet.WriteByte((byte)166);
            packet.WriteBoolean(true);
            this.SendTCP(packet);
        }
        public GSPacketIn SendEnterHotSpringRoom(GamePlayer player)
        {
            if (player.CurrentHotSpringRoom == null)
                return (GSPacketIn)null;
            GSPacketIn packet = new GSPacketIn((short)202, player.PlayerCharacter.ID);
            packet.WriteInt(player.CurrentHotSpringRoom.Info.roomID);
            packet.WriteInt(player.CurrentHotSpringRoom.Info.roomNumber);
            packet.WriteString(player.CurrentHotSpringRoom.Info.roomName);
            packet.WriteString(player.CurrentHotSpringRoom.Info.roomPassword);
            packet.WriteInt(player.CurrentHotSpringRoom.Info.effectiveTime);
            packet.WriteInt(player.CurrentHotSpringRoom.Count);
            packet.WriteInt(player.CurrentHotSpringRoom.Info.playerID);
            packet.WriteString(player.CurrentHotSpringRoom.Info.playerName);
            packet.WriteDateTime(player.CurrentHotSpringRoom.Info.startTime);
            packet.WriteString(player.CurrentHotSpringRoom.Info.roomIntroduction);
            packet.WriteInt(player.CurrentHotSpringRoom.Info.roomType);
            packet.WriteInt(player.CurrentHotSpringRoom.Info.maxCount);
            packet.WriteDateTime(player.Extra.Info.LastTimeHotSpring);
            packet.WriteInt(player.Extra.Info.MinHotSpring);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendHotSpringUpdateTime(GamePlayer player, int expAdd)
        {
            if (player.CurrentHotSpringRoom == null)
                return (GSPacketIn)null;
            GSPacketIn packet = new GSPacketIn((short)191, player.PlayerCharacter.ID);
            packet.WriteByte((byte)7);
            packet.WriteInt(player.Extra.Info.MinHotSpring);
            packet.WriteInt(expAdd);
            this.SendTCP(packet);
            return packet;
        }
        public GSPacketIn SendOpenBoguAdventure()
        {
            GSPacketIn packet = new GSPacketIn((short)145);
            packet.WriteByte((byte)89);
            packet.WriteBoolean(true);
            this.SendTCP(packet);
            return packet;
        }
        public GSPacketIn SendGetUserGift(PlayerInfo player, UserGiftInfo[] allGifts)
        {
            GSPacketIn packet = new GSPacketIn((short)218);
            packet.WriteInt(player.ID);
            packet.WriteInt(player.charmGP);
            packet.WriteInt(allGifts.Length);
            foreach (UserGiftInfo allGift in allGifts)
            {
                packet.WriteInt(allGift.TemplateID);
                packet.WriteInt(allGift.Count);
            }
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendUserEquip(PlayerInfo player, List<ItemInfo> items)
        {
            throw new NotImplementedException();
        }

        public void SendLeagueNotice(int id, int restCount, int maxCount, byte type)
        {
            GSPacketIn packet = new GSPacketIn((short)42, id);
            packet.WriteByte(type);
            if (type == (byte)1)
            {
                packet.WriteInt(restCount);
                packet.WriteInt(maxCount);
            }
            else
                packet.WriteInt(restCount);
            this.SendTCP(packet);
        }
        public void SendCatchBeastOpen(int playerID, bool isOpen)
        {
            GSPacketIn packet = new GSPacketIn((short)ePackageType.SEVENDAYTARGET_GODSROADS, playerID);
            Console.WriteLine("SendCatchBestOpen");
            GSPacketIn gSPacketIn = new GSPacketIn(145, playerID);
            gSPacketIn.WriteByte(32);
            gSPacketIn.WriteBoolean(isOpen);
            this.SendTCP(gSPacketIn);
        }

        public void SendLanternriddlesOpen(int playerID, bool isOpen)
        {
            GSPacketIn packet = new GSPacketIn((short)145, playerID);
            packet.WriteByte((byte)37);
            packet.WriteBoolean(isOpen);
            this.SendTCP(packet);
        }
        public GSPacketIn SendEatPetsInfo(EatPetsInfo info)
        {
            if (info == null)
                return null;
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.PET, m_gameClient.Player.PlayerId);
            pkg.WriteByte((byte)PetPackageType.EAT_PETS);
            pkg.WriteInt(info.weaponExp); // _loc_2.add("weaponExp", event.pkg.readInt());
            pkg.WriteInt(info.weaponLevel); //_loc_2.add("weaponLevel", event.pkg.readInt());
            pkg.WriteInt(info.clothesExp); //_loc_2.add("clothesExp", event.pkg.readInt());
            pkg.WriteInt(info.clothesLevel); //_loc_2.add("clothesLevel", event.pkg.readInt());
            pkg.WriteInt(info.hatExp); //_loc_2.add("hatExp", event.pkg.readInt());
            pkg.WriteInt(info.hatLevel); //_loc_2.add("hatLevel", event.pkg.readInt());
            SendTCP(pkg);
            return pkg;
        }

        /*  public GSPacketIn SendRefreshPet(GamePlayer player, UsersPetInfo[] pets, ItemInfo[] items, bool refreshBtn)
          {
              GSPacketIn gSPacketIn = new GSPacketIn(68, player.PlayerCharacter.ID);
              gSPacketIn.WriteByte(5);
              int val = 10;
              int val2 = 10;
              int val3 = 100;
              if (!player.PlayerCharacter.IsFistGetPet)
              {
                  gSPacketIn.WriteBoolean(refreshBtn);
                  gSPacketIn.WriteInt(pets.Length);
                  for (int i = 0; i < pets.Length; i++)
                  {
                      UsersPetInfo usersPetinfo = pets[i];
                      gSPacketIn.WriteInt(usersPetinfo.Place);
                      gSPacketIn.WriteInt(usersPetinfo.TemplateID);
                      gSPacketIn.WriteString(usersPetinfo.Name);
                      gSPacketIn.WriteInt(usersPetinfo.Attack);
                      gSPacketIn.WriteInt(usersPetinfo.Defence);
                      gSPacketIn.WriteInt(usersPetinfo.Luck);
                      gSPacketIn.WriteInt(usersPetinfo.Agility);
                      gSPacketIn.WriteInt(usersPetinfo.Blood);
                      gSPacketIn.WriteInt(usersPetinfo.Damage);
                      gSPacketIn.WriteInt(usersPetinfo.Guard);
                      gSPacketIn.WriteInt(usersPetinfo.AttackGrow);
                      gSPacketIn.WriteInt(usersPetinfo.DefenceGrow);
                      gSPacketIn.WriteInt(usersPetinfo.LuckGrow);
                      gSPacketIn.WriteInt(usersPetinfo.AgilityGrow);
                      gSPacketIn.WriteInt(usersPetinfo.BloodGrow);
                      gSPacketIn.WriteInt(usersPetinfo.DamageGrow);
                      gSPacketIn.WriteInt(usersPetinfo.GuardGrow);
                      gSPacketIn.WriteInt(usersPetinfo.Level);
                      gSPacketIn.WriteInt(usersPetinfo.GP);
                      gSPacketIn.WriteInt(usersPetinfo.MaxGP);
                      gSPacketIn.WriteInt(usersPetinfo.Hunger);
                      gSPacketIn.WriteInt(usersPetinfo.MP);
                      List<string> skill = usersPetinfo.GetSkill();
                      gSPacketIn.WriteInt(skill.Count);
                      foreach (string current in skill)
                      {
                          gSPacketIn.WriteInt(int.Parse(current.Split(new char[]
                          {
                              ','
                          })[0]));
                          gSPacketIn.WriteInt(int.Parse(current.Split(new char[]
                          {
                              ','
                          })[1]));
                      }
                      gSPacketIn.WriteInt(val);
                      gSPacketIn.WriteInt(val2);
                      gSPacketIn.WriteInt(val3);
                  }
                  gSPacketIn.WriteInt(0);
              }
              else
              {
                  gSPacketIn.WriteBoolean(refreshBtn);
                  gSPacketIn.WriteInt(pets.Length);
                  for (int j = 0; j < pets.Length; j++)
                  {
                      UsersPetInfo usersPetinfo2 = pets[j];
                      gSPacketIn.WriteInt(usersPetinfo2.Place);
                      gSPacketIn.WriteInt(usersPetinfo2.TemplateID);
                      gSPacketIn.WriteString(usersPetinfo2.Name);
                      gSPacketIn.WriteInt(usersPetinfo2.Attack);
                      gSPacketIn.WriteInt(usersPetinfo2.Defence);
                      gSPacketIn.WriteInt(usersPetinfo2.Luck);
                      gSPacketIn.WriteInt(usersPetinfo2.Agility);
                      gSPacketIn.WriteInt(usersPetinfo2.Blood);
                      gSPacketIn.WriteInt(usersPetinfo2.Damage);
                      gSPacketIn.WriteInt(usersPetinfo2.Guard);
                      gSPacketIn.WriteInt(usersPetinfo2.AttackGrow);
                      gSPacketIn.WriteInt(usersPetinfo2.DefenceGrow);
                      gSPacketIn.WriteInt(usersPetinfo2.LuckGrow);
                      gSPacketIn.WriteInt(usersPetinfo2.AgilityGrow);
                      gSPacketIn.WriteInt(usersPetinfo2.BloodGrow);
                      gSPacketIn.WriteInt(usersPetinfo2.DamageGrow);
                      gSPacketIn.WriteInt(usersPetinfo2.GuardGrow);
                      gSPacketIn.WriteInt(usersPetinfo2.Level);
                      gSPacketIn.WriteInt(usersPetinfo2.GP);
                      gSPacketIn.WriteInt(usersPetinfo2.MaxGP);
                      gSPacketIn.WriteInt(usersPetinfo2.Hunger);
                      gSPacketIn.WriteInt(usersPetinfo2.MP);
                      List<string> skill2 = usersPetinfo2.GetSkill();
                      gSPacketIn.WriteInt(skill2.Count);
                      foreach (string current2 in skill2)
                      {
                          gSPacketIn.WriteInt(int.Parse(current2.Split(new char[]
                          {
                              ','
                          })[0]));
                          gSPacketIn.WriteInt(int.Parse(current2.Split(new char[]
                          {
                              ','
                          })[1]));
                      }
                      gSPacketIn.WriteInt(val);
                      gSPacketIn.WriteInt(val2);
                      gSPacketIn.WriteInt(val3);
                  }
              }
              this.SendTCP(gSPacketIn);
              return gSPacketIn;
          }*/
        public GSPacketIn SendRefreshPet(GamePlayer player, UsersPetInfo[] pets, ItemInfo[] items, bool refreshBtn)
        {
            GSPacketIn gSPacketIn = new GSPacketIn((byte)ePackageType.PET, player.PlayerCharacter.ID);
            gSPacketIn.WriteByte(5);
            int MaxActiveSkillCount = 10;
            int MaxStaticSkillCount = 10;
            int MaxSkillCount = 100;           
            if (!player.PlayerCharacter.IsFistGetPet)
            {
                gSPacketIn.WriteBoolean(refreshBtn);
                gSPacketIn.WriteInt(pets.Length);
                for (int i = 0; i < pets.Length; i++)
                {
                    UsersPetInfo usersPetinfo = pets[i];
                    gSPacketIn.WriteInt(usersPetinfo.Place);
                    gSPacketIn.WriteInt(usersPetinfo.TemplateID);
                    gSPacketIn.WriteString(usersPetinfo.Name);
                    gSPacketIn.WriteInt(usersPetinfo.Attack);
                    gSPacketIn.WriteInt(usersPetinfo.Defence);
                    gSPacketIn.WriteInt(usersPetinfo.Luck);
                    gSPacketIn.WriteInt(usersPetinfo.Agility);
                    gSPacketIn.WriteInt(usersPetinfo.Blood);
                    gSPacketIn.WriteInt(usersPetinfo.Damage);
                    gSPacketIn.WriteInt(usersPetinfo.Guard);
                    gSPacketIn.WriteInt(usersPetinfo.AttackGrow);
                    gSPacketIn.WriteInt(usersPetinfo.DefenceGrow);
                    gSPacketIn.WriteInt(usersPetinfo.LuckGrow);
                    gSPacketIn.WriteInt(usersPetinfo.AgilityGrow);
                    gSPacketIn.WriteInt(usersPetinfo.BloodGrow);
                    gSPacketIn.WriteInt(usersPetinfo.DamageGrow);
                    gSPacketIn.WriteInt(usersPetinfo.GuardGrow);
                    gSPacketIn.WriteInt(usersPetinfo.Level);
                    gSPacketIn.WriteInt(usersPetinfo.GP);
                    gSPacketIn.WriteInt(usersPetinfo.MaxGP);
                    gSPacketIn.WriteInt(usersPetinfo.Hunger);
                    gSPacketIn.WriteInt(usersPetinfo.MP);
                    List<string> skill = usersPetinfo.GetSkill();
                    gSPacketIn.WriteInt(skill.Count);
                    foreach (string current in skill)
                    {
                        gSPacketIn.WriteInt(int.Parse(current.Split(new char[]
                        {
                                ','
                        })[0]));
                        gSPacketIn.WriteInt(int.Parse(current.Split(new char[]
                        {
                                ','
                        })[1]));
                    }
                    gSPacketIn.WriteInt(MaxActiveSkillCount);
                    gSPacketIn.WriteInt(MaxStaticSkillCount);
                    gSPacketIn.WriteInt(MaxSkillCount);
                }
                gSPacketIn.WriteInt(0);
            }

            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendEnterFarm(PlayerInfo Player, UserFarmInfo farm, UserFieldInfo[] fields)
        {
            Console.WriteLine("SendEnterFarm PKG = 1");
            //GSPacketIn pkg = new GSPacketIn(81, Player.ID);
            //pkg.WriteByte(1);
            //pkg.WriteInt(farm.FarmID);
            //pkg.WriteBoolean(farm.isFarmHelper);
            //pkg.WriteInt(farm.isAutoId);
            //pkg.WriteDateTime(farm.AutoPayTime);
            //pkg.WriteInt(farm.AutoValidDate);
            //pkg.WriteInt(farm.GainFieldId);
            //pkg.WriteInt(farm.KillCropId);
            //pkg.WriteInt(fields.Length);
            //foreach (UserFieldInfo field in fields)
            //{
            //	pkg.WriteInt(field.FieldID);
            //	pkg.WriteInt(field.SeedID);
            //	pkg.WriteDateTime(field.PayTime);
            //	pkg.WriteDateTime(field.PlantTime);
            //	pkg.WriteInt(field.GainCount);
            //	pkg.WriteInt(field.FieldValidDate);
            //	pkg.WriteInt(field.AccelerateTime);
            //         }
            //if (farm.FarmID == Player.ID)
            //         {
            //	pkg.WriteString(farm.PayFieldMoney);
            //	pkg.WriteString(farm.PayAutoMoney);
            //	pkg.WriteDateTime(farm.AutoPayTime);
            //	pkg.WriteInt(farm.AutoValidDate);
            //	pkg.WriteInt(Player.VIPLevel);
            //	pkg.WriteInt(farm.buyExpRemainNum);
            //}
            //else
            //         {
            //	pkg.WriteBoolean(farm.isArrange);
            //         }
            //this.SendTCP(pkg);
            //return pkg;
            #region Old EnterFarm High Version
            GSPacketIn packet = new GSPacketIn((byte)ePackageType.FARM, Player.ID);
            packet.WriteByte((byte)FarmPackageType.ENTER_FARM);
            packet.WriteInt(farm.FarmID); //_model.currentFarmerId = _loc_2.readInt();
            packet.WriteBoolean(farm.isFarmHelper); // _loc_3:* = _loc_2.readBoolean(); isFarmHelper/isAutomatic
            packet.WriteInt(farm.isAutoId); // _loc_4:* = _loc_2.readInt(); isAutoId/autoSeedID
            packet.WriteDateTime(farm.AutoPayTime); // _loc_5:* = _loc_2.readDate();//startdate
            packet.WriteInt(farm.AutoValidDate); // _loc_6:* = _loc_2.readInt();//_autoTime
            packet.WriteInt(farm.GainFieldId); // _loc_7:* = _loc_2.readInt();//_needSeed
            packet.WriteInt(farm.KillCropId); // _loc_8:* = _loc_2.readInt();//_getSeed
            packet.WriteInt(fields.Length); // _loc_9:* = _loc_2.readInt();//field count
            foreach (UserFieldInfo field in fields)
            {
                packet.WriteInt(field.FieldID); //_loc_11 = _loc_2.readInt();//fieldID
                packet.WriteInt(field.SeedID); //_loc_12 = _loc_2.readInt();//seedID :332112
                packet.WriteDateTime(field.PayTime); //_loc_13 = _loc_2.readDate();//payTime
                packet.WriteDateTime(field.PlantTime); //_loc_14 = _loc_2.readDate();//plantTime
                packet.WriteInt(field.GainCount); //_loc_15 = _loc_2.readInt();//gainCount
                packet.WriteInt(field.FieldValidDate); //_loc_16 = _loc_2.readInt();//fieldValidDate
                packet.WriteInt(field.AccelerateTime); //_loc_17 = _loc_2.readInt();//AccelerateTime  
            }

            if (farm.FarmID == Player.ID)
            {
                packet.WriteInt(33333); //gropPrice = _loc_2.readInt();
                packet.WriteString(farm.PayFieldMoney); //_model.payFieldMoney = _loc_2.readUTF();
                packet.WriteString(farm.PayAutoMoney); //_model.payAutoMoney = _loc_2.readUTF();
                packet.WriteDateTime(farm.AutoPayTime); //_model.autoPayTime = _loc_2.readDate();
                packet.WriteInt(farm.AutoValidDate); //_model.autoValidDate = _loc_2.readInt();
                packet.WriteInt(Player.VIPLevel); //_model.vipLimitLevel = _loc_2.readInt(); of player
                packet.WriteInt(farm.buyExpRemainNum); //_model.buyExpRemainNum = _loc_2.readInt(); 7road
            }
            else
            {
                packet.WriteBoolean(farm.isArrange); //_model.isArrange = _loc_2.readBoolean();
            }

            SendTCP(packet);
            return packet;
            #endregion
        }

        public GSPacketIn SendFarmLandInfo(PlayerFarm farm)
        {
            UserFieldInfo[] fields = farm.GetFields();
            GSPacketIn gSPacketIn = new GSPacketIn((byte)81, farm.CurrentFarm.FarmID);
            gSPacketIn.WriteByte(17);
            gSPacketIn.WriteInt(fields.Length);
            UserFieldInfo[] array = fields;
            for (int i = 0; i < array.Length; i++)
            {
                UserFieldInfo userFieldInfo = array[i];
                gSPacketIn.WriteInt(userFieldInfo.FieldID);
                gSPacketIn.WriteInt(userFieldInfo.SeedID);
                gSPacketIn.WriteDateTime(userFieldInfo.PlantTime);
                gSPacketIn.WriteInt(userFieldInfo.GainCount);
                gSPacketIn.WriteInt(userFieldInfo.AccelerateTime);
            }
        //    gSPacketIn.WriteBoolean(farm.midAutumnFlag);
            gSPacketIn.WriteBoolean(farm.CurrentFarm.isFarmHelper);
            this.SendTCP(gSPacketIn);
            return gSPacketIn;
        }
        public GSPacketIn SendSeeding(PlayerInfo Player, UserFieldInfo field)
        {
            GSPacketIn packet = new GSPacketIn((byte)ePackageType.FARM, Player.ID);
            packet.WriteByte((byte)FarmPackageType.GROW_FIELD);
            packet.WriteInt(field.FieldID); //_loc_3:* = fieldId.readInt();
            packet.WriteInt(field.SeedID); // _loc_4:* = seedID.readInt();
            packet.WriteDateTime(field.PlantTime); // _loc_5:* = plantTime.readDate();
            packet.WriteDateTime(field.PayTime); // _loc_6:* = _loc_2.readDate();
            packet.WriteInt(field.GainCount); // _loc_7:* = gainCount.readInt();
            packet.WriteInt(field.FieldValidDate); // _loc_8:* = _loc_2.readInt();
            SendTCP(packet);
            return packet;
        }
      
        public GSPacketIn SenddoMature(UserFieldInfo[] Field)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(81);
            gSPacketIn.WriteByte(3);
            gSPacketIn.WriteInt(Field.Length);
            for (int i = 0; i < Field.Length; i++)
            {
                UserFieldInfo userFieldInfo = Field[i];
                if (userFieldInfo != null)
                {
                    gSPacketIn.WriteBoolean(true);
                    gSPacketIn.WriteInt(Field[i].FieldID);
                    gSPacketIn.WriteInt(Field[i].GainCount);
                    gSPacketIn.WriteInt(Field[i].AccelerateTime);
                }
                else
                {
                    gSPacketIn.WriteBoolean(false);
                }
            }
            this.SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SenddoMature(PlayerFarm farm)
        {
            GSPacketIn packet = new GSPacketIn((byte)ePackageType.FARM, farm.Player.PlayerCharacter.ID);
            packet.WriteByte((byte)FarmPackageType.ACCELERATE_FIELD);
            packet.WriteInt(farm.CurrentFields.Length); // _loc_9:* = _loc_2.readInt();//field count
            foreach (UserFieldInfo field in farm.CurrentFields)
            {
                if (field != null)
                {
                    packet.WriteBoolean(true);
                    packet.WriteInt(field.FieldID); //    this.model.matureId = event.pkg.readInt();
                    packet.WriteInt(field.GainCount); //    _loc_5.gainCount = event.pkg.readInt();
                    packet.WriteInt(field.AccelerateTime); //    _loc_5.AccelerateTime = event.pkg.readInt();
                }
                else
                {
                    packet.WriteBoolean(false);
                }
            }
            SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendtoGather(PlayerInfo Player, UserFieldInfo field)
        {
            GSPacketIn packet = new GSPacketIn((byte)ePackageType.FARM, Player.ID);
            packet.WriteByte((byte)FarmPackageType.GAIN_FIELD);
            packet.WriteBoolean(true); //var _loc_3:* = event.pkg.readBoolean();
            packet.WriteInt(field.FieldID); //model.gainFieldId = event.pkg.readInt();
            packet.WriteInt(field.SeedID); //_loc_2.seedID = event.pkg.readInt();
            packet.WriteDateTime(field.PlantTime); //_loc_2.plantTime = event.pkg.readDate();
            packet.WriteInt(field.GainCount); //_loc_2.gainCount = event.pkg.readInt();
            packet.WriteInt(field.AccelerateTime); //_loc_2.AccelerateTime = event.pkg.readInt();
            SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendCompose(GamePlayer Player)
        {
            GSPacketIn packet = new GSPacketIn((byte)ePackageType.FARM, Player.PlayerCharacter.ID);
            packet.WriteByte((byte)FarmPackageType.COMPOSE_FOOD);
            SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendPayFields(GamePlayer Player, List<int> fieldIds)
        {
            GSPacketIn packet = new GSPacketIn((byte)ePackageType.FARM, Player.PlayerCharacter.ID);
            packet.WriteByte((byte)FarmPackageType.PAY_FIELD);
            packet.WriteInt(Player.PlayerCharacter.ID);
            packet.WriteInt(fieldIds.Count); // _loc_9:* = _loc_2.readInt();//field count
            foreach (int id in fieldIds)
            {
                UserFieldInfo field = Player.Farm.GetFieldAt(id);
                packet.WriteInt(field.FieldID); //_loc_11 = _loc_2.readInt();//fieldID
                packet.WriteInt(field.SeedID); //_loc_12 = _loc_2.readInt();//seedID :332112
                packet.WriteDateTime(field.PayTime); //_loc_13 = _loc_2.readDate();//payTime
                packet.WriteDateTime(field.PlantTime); //_loc_14 = _loc_2.readDate();//plantTime
                packet.WriteInt(field.GainCount); //_loc_15 = _loc_2.readInt();//gainCount
                packet.WriteInt(field.FieldValidDate); //_loc_16 = _loc_2.readInt();//fieldValidDate
                packet.WriteInt(field.AccelerateTime); //_loc_17 = _loc_2.readInt();//AccelerateTime
            }

            SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendKillCropField(PlayerInfo Player, UserFieldInfo field)
        {
            GSPacketIn packet = new GSPacketIn((byte)ePackageType.FARM, Player.ID);
            packet.WriteByte((byte)FarmPackageType.KILLCROP_FIELD);
            packet.WriteBoolean(true);
            packet.WriteInt(field.FieldID); //_loc_3:* = fieldId.readInt();
            packet.WriteInt(field.SeedID); // _loc_4:* = seedID.readInt();
            packet.WriteInt(field.AccelerateTime); // _loc_8:* = _loc_2.readInt();
            SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendHelperSwitchField(PlayerInfo Player, UserFarmInfo farm)
        {
            GSPacketIn packet = new GSPacketIn((byte)ePackageType.FARM, Player.ID);
            packet.WriteByte((byte)FarmPackageType.HELPER_SWITCH_FIELD);
            packet.WriteBoolean(farm.isFarmHelper); // _loc_3:* = _loc_2.readBoolean(); isFarmHelper/isAutomatic
            packet.WriteInt(farm.isAutoId); // _loc_4:* = _loc_2.readInt(); isAutoId/autoSeedID
            packet.WriteDateTime(farm.AutoPayTime); // _loc_5:* = _loc_2.readDate();//startdate
            packet.WriteInt(farm.AutoValidDate); // _loc_6:* = _loc_2.readInt();//_autoTime
            packet.WriteInt(farm.GainFieldId); // _loc_7:* = _loc_2.readInt();//_needSeed
            packet.WriteInt(farm.KillCropId); // _loc_8:* = _loc_2.readInt();//_getSeed
            SendTCP(packet);
            return packet;
        }

        public void SendQQtips(int UserID, QQtipsMessagesInfo QQTips)
        {
            if (QQTips == null)
            {
                return;
            }
            GSPacketIn gSPacketIn = new GSPacketIn(99, UserID);
            gSPacketIn.WriteString(QQTips.title);
            gSPacketIn.WriteString(QQTips.content);
            gSPacketIn.WriteInt(QQTips.maxLevel);
            gSPacketIn.WriteInt(QQTips.minLevel);
            gSPacketIn.WriteInt(QQTips.outInType);
            if (QQTips.outInType == 0)
            {
                gSPacketIn.WriteInt(QQTips.moduleType);
                gSPacketIn.WriteInt(QQTips.inItemID);
            }
            else
            {
                gSPacketIn.WriteString(QQTips.url);
            }
            this.SendTCP(gSPacketIn);
        }

        public void SendOpenGodsRoads()
        {
            GSPacketIn packet = new GSPacketIn((short)ePackageType.SEVENDAYTARGET_GODSROADS);
            packet.WriteByte((byte)GodsRoadsPackageType.GODS_ROADS_OPEN);
            packet.WriteBoolean(true);
            this.SendTCP(packet);
        }

        public void SendOpenSevenDayTarget()
        {
            //Seven day target
            GSPacketIn packet = new GSPacketIn((short)ePackageType.SEVENDAYTARGET_GODSROADS);
            packet.WriteByte((byte)SevenDayTargetPackageType.SEVENDAYTARGET_OPEN_CLOSE);
            packet.WriteBoolean(true);
            this.SendTCP(packet);

            //new player
            GSPacketIn packet2 = new GSPacketIn((short)ePackageType.SEVENDAYTARGET_GODSROADS);
            packet2.WriteByte((byte)SevenDayTargetPackageType.NEWPLAYERREWARD_OPEN_CLOSE);
            packet2.WriteBoolean(true);
            this.SendTCP(packet2);
        }

        public GSPacketIn SendOpenGrowthPackageOpen(int ID)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(84, ID);
            gSPacketIn.WriteInt(1);
            gSPacketIn.WriteInt(3);
            gSPacketIn.WriteBoolean(true);
            this.SendTCP(gSPacketIn);
            return gSPacketIn;
        }
        public GSPacketIn SendGrowthPackageOpen(int ID, int isBuy)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(84, ID);
            gSPacketIn.WriteInt(1);
            gSPacketIn.WriteInt(1);
            gSPacketIn.WriteInt(isBuy);
            for (int i = 0; i < 9; i++)
            {
                if (i < isBuy)
                {
                    gSPacketIn.WriteInt(1);
                }
                else
                {
                    gSPacketIn.WriteInt(0);
                }
            }
            this.SendTCP(gSPacketIn);
            return gSPacketIn;
        }
        public GSPacketIn SendGrowthPackageUpadte(int ID, int isBuy)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(84, ID);
            gSPacketIn.WriteInt(1);
            gSPacketIn.WriteInt(2);
            gSPacketIn.WriteInt(isBuy);
            for (int i = 0; i < 9; i++)
            {
                if (i < isBuy)
                {
                    gSPacketIn.WriteInt(1);
                }
                else
                {
                    gSPacketIn.WriteInt(0);
                }
            }
            this.SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public void SendOpenEntertainmentMode()
        {
            GSPacketIn gSPacketIn = new GSPacketIn(145);
            gSPacketIn.WriteByte(71);
            gSPacketIn.WriteBoolean(true);
            gSPacketIn.WriteDateTime(System.DateTime.Now);
            gSPacketIn.WriteDateTime(System.DateTime.Now.AddDays(7.0));
            this.SendTCP(gSPacketIn);
        }

    }
}
