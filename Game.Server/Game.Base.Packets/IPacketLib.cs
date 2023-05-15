using Game.Server.Buffer;
using Game.Server.ConsortiaTask;
using Game.Server.GameUtils;
using Game.Server.Packets;
using Game.Server.Quests;
using Game.Server.Rooms;
using Game.Server.SceneMarryRooms;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;

namespace Game.Base.Packets
{
    public interface IPacketLib
    {

        //void SendOpenEntertainmentMode(bool isOpen, DateTime timeStart, DateTime timeEnd);

        GSPacketIn SendUpdatePlayerNewHall(GamePlayer player);

        void SendOpenEntertainmentMode();
        GSPacketIn SendNecklaceStrength(PlayerInfo player);

        void SendCreateInviteFriends(GamePlayer player, int type);

        void IsLeagueOpen();

        GSPacketIn SendGrowthPackageOpen(int ID, int isBuy);

        GSPacketIn SendOpenGrowthPackageOpen(int ID);
        GSPacketIn SendGrowthPackageUpadte(int ID, int isBuy);
        void SendOpenGodsRoads();

        void SendOpenSevenDayTarget();

        void SendOpenOrCloseChristmas(int lastPacks, bool isOpen);
        void SendLeftRouleteResult(UsersExtraInfo info);

        void SendLeftRouleteOpen(UsersExtraInfo info);

        void SendAcademyGradute(GamePlayer app, int type);

        void SendPyramidOpenClose(PyramidConfigInfo info);

        void SendDragonBoat(PlayerInfo info);

        void SendOpenDDPlay(PlayerInfo player);
        void SendLittleGameActived();
        GSPacketIn SendOpenBoguAdventure();

        GSPacketIn SendBattleGoundOpen(int ID);

        GSPacketIn SendBattleGoundOver(int ID);

        GSPacketIn SendAcademyAppState(PlayerInfo player, int removeUserId);

        GSPacketIn SendAcademySystemNotice(string text, bool isAlert);

        GSPacketIn SendConsortiaTaskInfo(BaseConsortiaTask baseTask);

        GSPacketIn SendAvatarCollect(PlayerAvatarCollection avtCollect);

        GSPacketIn SendSystemConsortiaChat(string content, bool sendToSelf);

        void SendShopGoodsCountUpdate(List<ShopFreeCountInfo> list);


        GSPacketIn SendPlayerRefreshTotem(PlayerInfo player);

        GSPacketIn SendUpdateUpCount(PlayerInfo player);

        void SendEliteGameStartRoom();

        void SendExpBlessedData(int PlayerId);
        GSPacketIn SendLabyrinthUpdataInfo(int ID, UserLabyrinthInfo laby);

        GSPacketIn SendPetInfo(int id, int zoneId, UsersPetInfo[] pets);

        GSPacketIn SendUpdateUserPet(PetInventory bag, int[] slots);

        GSPacketIn SendDiceActiveClose(int ID);

        GSPacketIn SendDiceReceiveData(PlayerDice Dice);

        GSPacketIn SendDiceReceiveResult(PlayerDice Dice);

        GSPacketIn SendDiceActiveOpen(PlayerDice Dice);

        GSPacketIn sendBuyBadge(
      int consortiaID,
      int BadgeID,
      int ValidDate,
      bool result,
      string BadgeBuyTime,
      int playerid);

        void SendEdictumVersion();

        void SendEnthrallLight();

        void SendUpdateFirstRecharge(bool isRecharge, bool isGetAward);

        void SendOpenNoviceActive(
          int channel,
          int activeId,
          int condition,
          int awardGot,
          DateTime startTime,
          DateTime endTime);

        GSPacketIn SendOpenTimeBox(int condtion, bool isSuccess);

        void SendUpdateCardData(PlayerInfo player, List<UsersCardInfo> userCard);


        GSPacketIn SendFriendRemove(int FriendID);

        GSPacketIn SendChickenBoxOpen(int ID, int flushPrice, int[] openCardPrice, int[] eagleEyePrice);



        GSPacketIn SendFriendState(int playerID, int state, byte typeVip, int viplevel);

        GSPacketIn SendLuckStarOpen(int ID);

        GSPacketIn sendBuyBadge(
      int BadgeID,
      int ValidDate,
      bool result,
      string BadgeBuyTime,
      int playerid);


        GSPacketIn SendUserRanks2(List<UserRankInfo> rankList);
        GSPacketIn SendConsortiaMail(bool result, int playerid);

        GSPacketIn sendOneOnOneTalk(
          int receiverID,
          bool isAutoReply,
          string SenderNickName,
          string msg,
          int playerid);

        GSPacketIn SendSingleRoomCreate(BaseRoom room, int zoneId);

        GSPacketIn SendUpdateConsotiaBoss(ConsortiaBossInfo bossInfo);

        GSPacketIn SendHotSpringUpdateTime(GamePlayer player, int expAdd);

        GSPacketIn SendPlayerDrill(int ID, Dictionary<int, UserDrillInfo> drills);

        void SendTCP(GSPacketIn packet);

        GSPacketIn SendConsortiaLevelUp(
          byte type,
          byte level,
          bool result,
          string msg,
          int playerid);

        GSPacketIn sendConsortiaOut(int id, bool result, string msg, int playerid);
        GSPacketIn SendUpdateConsotiaBuffer(
      GamePlayer player,
      Dictionary<string, BufferInfo> bufflist);

        void SendLoginSuccess();

        void SendLoginSuccess2();

        void SendCheckCode();

        void SendLoginFailed(string msg);

        void SendKitoff(string msg);

        void SendEditionError(string msg);

        void SendWeaklessGuildProgress(PlayerInfo player);

        GSPacketIn SendUpdateAchievementData(List<AchievementDataInfo> infos);

        GSPacketIn SendAchievementSuccess(AchievementDataInfo d);

        GSPacketIn SendUpdateAchievements(List<UsersRecordInfo> infos);

        GSPacketIn SendInitAchievements(List<UsersRecordInfo> infos);

        GSPacketIn SendUpdateAchievements(UsersRecordInfo info);

        GSPacketIn SendGameRoomSetupChange(BaseRoom room);

        void SendDateTime();

        GSPacketIn SendDailyAward(GamePlayer player);

        void SendPingTime(GamePlayer player);
        void SendLeagueNotice(int id, int restCount, int maxCount, byte type);
        void SendCatchBeastOpen(int playerID, bool isOpen);

        void SendLanternriddlesOpen(int playerID, bool isOpen);

        //  void SendUpdatePrivateInfo(PlayerInfo info);

        GSPacketIn SendAddPlayerNewHall(GamePlayer player);

        void SendUpdatePrivateInfo(PlayerInfo info, int medal);

        void SendOpenWorldBoss(int pX, int pY);

        GSPacketIn SendUpdatePublicPlayer(PlayerInfo info, UserMatchInfo matchInfo);
        GSPacketIn SendUpdatePublicPlayer(
      PlayerInfo info,
      UserMatchInfo matchInfo,
      UsersExtraInfo extraInfo);
        GSPacketIn SendUpdateConsotiaBuffer(GamePlayer player, Dictionary<int, BufferInfo> bufflist);

        GSPacketIn SendNetWork(int id, long delay);

        GSPacketIn SendUpdateBuffer(GamePlayer player, BufferInfo[] infos);

        GSPacketIn SendUserEquip(PlayerInfo player, List<SqlDataProvider.Data.ItemInfo> items);

        GSPacketIn SendUserEquip(PlayerInfo player, List<SqlDataProvider.Data.ItemInfo> items, List<UserGemStone> UserGemStone);



        GSPacketIn SendMessage(eMessageType type, string message);

        void SendWaitingRoom(bool result);

        GSPacketIn SendUpdateRoomList(List<BaseRoom> room);

        GSPacketIn SendSceneAddPlayer(GamePlayer player);

        GSPacketIn SendSceneRemovePlayer(GamePlayer player);

        GSPacketIn SendRoomCreate(BaseRoom room);

        GSPacketIn SendRoomLoginResult(bool result);

        GSPacketIn SendRoomPlayerAdd(GamePlayer player);

        GSPacketIn SendRoomPlayerRemove(GamePlayer player);

        GSPacketIn SendRoomUpdatePlayerStates(byte[] states);

        GSPacketIn SendRoomUpdatePlacesStates(int[] states);

        GSPacketIn SendRoomPlayerChangedTeam(GamePlayer player);

        GSPacketIn SendRoomPairUpStart(BaseRoom room);

        GSPacketIn SendRoomPairUpCancel(BaseRoom room);

        GSPacketIn SendEquipChange(GamePlayer player, int place, int goodsID, string style);

        GSPacketIn SendRoomChange(BaseRoom room);

        GSPacketIn SendFusionPreview(
          GamePlayer player,
          Dictionary<int, double> previewItemList,
          bool isBind,
          int MinValid);

        GSPacketIn SendFusionResult(GamePlayer player, bool result);

        GSPacketIn SendRefineryPreview(
          GamePlayer player,
          int templateid,
          bool isbind,
          SqlDataProvider.Data.ItemInfo item);

        void SendUpdateInventorySlot(PlayerInventory bag, int[] updatedSlots);

        void SendUpdateCardData(CardInventory bag, int[] updatedSlots);

        GSPacketIn SendUpdateBuffer(GamePlayer player, AbstractBuffer[] infos);

        GSPacketIn SendBufferList(GamePlayer player, List<AbstractBuffer> infos);

        GSPacketIn SendUpdateQuests(GamePlayer player, byte[] states, BaseQuest[] quests);

        GSPacketIn SendMailResponse(int playerID, eMailRespose type);

        GSPacketIn SendAuctionRefresh(
          AuctionInfo info,
          int auctionID,
          bool isExist,
          SqlDataProvider.Data.ItemInfo item);

        GSPacketIn SendIDNumberCheck(bool result);

        GSPacketIn SendAASState(bool result);

        GSPacketIn SendAASInfoSet(bool result);

        GSPacketIn SendAASControl(bool result, bool bool_0, bool IsMinor);

        GSPacketIn SendGameRoomInfo(GamePlayer player, BaseRoom game);

        GSPacketIn SendMarryInfoRefresh(MarryInfo info, int ID, bool isExist);

        GSPacketIn SendMarryRoomInfo(GamePlayer player, MarryRoom room);

        GSPacketIn SendPlayerEnterMarryRoom(GamePlayer player);

        GSPacketIn SendPlayerMarryStatus(GamePlayer player, int userID, bool isMarried);

        GSPacketIn SendPlayerMarryApply(
          GamePlayer player,
          int userID,
          string userName,
          string loveProclamation,
          int ID);

        GSPacketIn SendPlayerDivorceApply(GamePlayer player, bool result, bool isProposer);

        GSPacketIn SendMarryApplyReply(
          GamePlayer player,
          int UserID,
          string UserName,
          bool result,
          bool isApplicant,
          int ID);
        GSPacketIn SendPlayerFigSpiritinit(int ID, List<UserGemStone> gems);

        GSPacketIn SendPlayerFigSpiritUp(int ID, UserGemStone gem, bool isUp, bool isMaxLevel, bool isFall, int num, int dir);

        GSPacketIn SendBigSpeakerMsg(GamePlayer player, string msg);

        GSPacketIn SendPlayerLeaveMarryRoom(GamePlayer player);

        GSPacketIn SendMarryRoomLogin(GamePlayer player, bool result);

        GSPacketIn SendMarryRoomInfoToPlayer(
          GamePlayer player,
          bool state,
          MarryRoomInfo info);

        GSPacketIn SendMarryInfo(GamePlayer player, MarryInfo info);

        GSPacketIn SendContinuation(GamePlayer player, MarryRoomInfo info);

        GSPacketIn SendMarryProp(GamePlayer player, MarryProp info);

        GSPacketIn SendRoomType(GamePlayer player, BaseRoom game);

        void SendUserLuckyNum();

        GSPacketIn SendUpdatePlayerProperty(PlayerInfo info, PlayerProperty PlayerProp);

        GSPacketIn SendOpenVIP(GamePlayer Player);

        //GSPacketIn SendUserRanks(List<UserRankInfo> rankList);

        GSPacketIn SendEatPetsInfo(EatPetsInfo info);
        GSPacketIn SendUserRanks(int Id, List<UserRankInfo> ranks);

        GSPacketIn SendEnterHotSpringRoom(GamePlayer player);

        GSPacketIn SendGetUserGift(PlayerInfo player, UserGiftInfo[] allGifts);

        GSPacketIn SendAddFriend(PlayerInfo user, int relation, bool state);

        GSPacketIn SendRefreshPet(GamePlayer player, UsersPetInfo[] pets, ItemInfo[] items, bool refreshBtn);

        GSPacketIn SendEnterFarm(PlayerInfo Player, UserFarmInfo farm, UserFieldInfo[] fields);

        GSPacketIn SendSeeding(PlayerInfo Player, UserFieldInfo field);

        GSPacketIn SenddoMature(PlayerFarm farm);

        GSPacketIn SendtoGather(PlayerInfo Player, UserFieldInfo field);

        GSPacketIn SendPayFields(GamePlayer Player, List<int> fieldIds);

        GSPacketIn SendKillCropField(PlayerInfo Player, UserFieldInfo field);

        GSPacketIn SendHelperSwitchField(PlayerInfo Player, UserFarmInfo farm);

        GSPacketIn SendFarmLandInfo(PlayerFarm farm);

        GSPacketIn SenddoMature(UserFieldInfo[] Field);

        void SendQQtips(int UserID, QQtipsMessagesInfo QQTips);


    }
}
