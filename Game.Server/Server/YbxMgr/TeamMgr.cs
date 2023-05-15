// Decompiled with JetBrains decompiler
// Type: YbxMgr.TeamMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 23E379A8-7519-4E9F-8BEC-E46601E9640E
// Assembly location: C:\server\road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using EntityDatabase.PlayerModels;
using EntityDatabase.ServerModels;
using Game.Base.Packets;
using Game.Server;
using Game.Server.GameObjects;
using Game.Server.Managers;
using Game.Server.Packets;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Expressions;

namespace YbxMgr
{
  public static class TeamMgr
  {
    public static List<BattleTeamSegmentLists> battleTeamSegmentList;
    public static List<BattleTeamLevelLists> battleTeamLevelList;
    public static List<BattleTeamActiveTemplateLists> ActiveList;
    public static List<BattleTeamShopItemLists> Shops;

    public static bool Init()
    {
      ServerData serverData = new ServerData();
      try
      {
        TeamMgr.battleTeamSegmentList = serverData.BattleTeamSegmentLists.OrderBy<BattleTeamSegmentLists, int>((Expression<Func<BattleTeamSegmentLists, int>>) (p => p.SegmentID)).ToList<BattleTeamSegmentLists>();
        TeamMgr.battleTeamLevelList = serverData.BattleTeamLevelLists.OrderBy<BattleTeamLevelLists, int>((Expression<Func<BattleTeamLevelLists, int>>) (p => p.NeedActive)).ToList<BattleTeamLevelLists>();
        TeamMgr.ActiveList = serverData.BattleTeamActiveTemplateLists.OrderBy<BattleTeamActiveTemplateLists, int>((Expression<Func<BattleTeamActiveTemplateLists, int>>) (p => p.Type)).ToList<BattleTeamActiveTemplateLists>();
        TeamMgr.Shops = serverData.BattleTeamShopItemLists.OrderBy<BattleTeamShopItemLists, int>((Expression<Func<BattleTeamShopItemLists, int>>) (p => p.ID)).ToList<BattleTeamShopItemLists>();
        return true;
      }
      catch
      {
        return false;
      }
    }

    public static Ybx_89_Team updateteam(int teamId)
    {
      PlayerData playerData = new PlayerData();
      if (teamId == 0)
        return new Ybx_89_Team();
      Ybx_89_Team entity = playerData.Ybx_89_Team.Find(new object[1]
      {
        (object) teamId
      });
      int teamScore = TeamMgr.GetTeamScore(teamId);
      int currentDivision = TeamMgr.GetCurrentDivision(teamScore);
      TeamMgr.GetTotalMembers(teamId);
      entity.teamDivision = currentDivision;
      entity.teamScore = teamScore;
      entity.totalMember = TeamMgr.GetTotalMembers(teamId);
      if (entity.AreaName == "")
        entity.AreaName = GameServer.Instance.Configuration.ServerName;
      playerData.Entry<Ybx_89_Team>(entity).State = EntityState.Modified;
      playerData.SaveChanges();
      return entity;
    }

    public static void SendToSingle(GamePlayer player)
    {
      player.PlayerCharacter.Team = TeamMgr.updateteam(player.PlayerCharacter.Team.teamID);
      player.UpdateProperties();
      BattleTeamLevelLists nextLvExp1 = TeamMgr.GetNextLvExp(player.PlayerCharacter.Team.teamGrade + 1);
      BattleTeamLevelLists nextLvExp2 = TeamMgr.GetNextLvExp(player.PlayerCharacter.Team.teamGrade);
      GSPacketIn packet = new GSPacketIn((short) 390, player.PlayerId);
      packet.WriteByte((byte) 1);
      packet.WriteInt(player.PlayerCharacter.Team.teamDivision);
      packet.WriteInt(player.PlayerCharacter.Team.teamID);
      packet.WriteString(player.PlayerCharacter.Team.teamName);
      packet.WriteString(player.PlayerCharacter.Team.teamTag);
      packet.WriteInt(player.PlayerCharacter.Team.teamGrade);
      packet.WriteInt(player.PlayerCharacter.Team.totalActive);
      packet.WriteInt(player.PlayerCharacter.Team.active);
      packet.WriteInt(nextLvExp1 != null ? nextLvExp1.NeedActive : 0);
      packet.WriteInt(player.PlayerCharacter.Team.teamWinTime);
      packet.WriteInt(player.PlayerCharacter.Team.teamTotalTime);
      packet.WriteDateTime(player.PlayerCharacter.Team.createDate);
      packet.WriteInt(player.PlayerCharacter.Team.teamScore);
      packet.WriteInt(player.PlayerCharacter.Team.totalMember);
      packet.WriteInt(nextLvExp2 != null ? nextLvExp2.MaxPlayerNum : 0);
      packet.WriteInt(player.PlayerCharacter.Team.season);
      packet.WriteString("");
      packet.WriteInt(player.PlayerCharacter.TeamUser.IsCaption ? 1 : 0);
      packet.WriteDateTime(player.PlayerCharacter.Team.teamLoginDate);
      player.Out.SendTCP(packet);
      TeamMgr.UpdateTeamPropertise(player);
    }

    public static void SendToAll(int teamId)
    {
      GamePlayer[] allPlayersByTeamId = WorldMgr.GetAllPlayersByTeamId(teamId);
      Ybx_89_Team ybx89Team = TeamMgr.updateteam(teamId);
      foreach (GamePlayer Player in allPlayersByTeamId)
      {
        BattleTeamLevelLists nextLvExp1 = TeamMgr.GetNextLvExp(Player.PlayerCharacter.Team.teamGrade + 1);
        BattleTeamLevelLists nextLvExp2 = TeamMgr.GetNextLvExp(Player.PlayerCharacter.Team.teamGrade);
        Player.PlayerCharacter.Team = ybx89Team;
        Player.UpdateProperties();
        GSPacketIn packet = new GSPacketIn((short) 390, Player.PlayerId);
        packet.WriteByte((byte) 1);
        packet.WriteInt(Player.PlayerCharacter.Team.teamDivision);
        packet.WriteInt(Player.PlayerCharacter.Team.teamID);
        packet.WriteString(Player.PlayerCharacter.Team.teamName);
        packet.WriteString(Player.PlayerCharacter.Team.teamTag);
        packet.WriteInt(Player.PlayerCharacter.Team.teamGrade);
        packet.WriteInt(Player.PlayerCharacter.Team.totalActive);
        packet.WriteInt(Player.PlayerCharacter.Team.active);
        packet.WriteInt(nextLvExp1 != null ? nextLvExp1.NeedActive : 0);
        packet.WriteInt(Player.PlayerCharacter.Team.teamWinTime);
        packet.WriteInt(Player.PlayerCharacter.Team.teamTotalTime);
        packet.WriteDateTime(Player.PlayerCharacter.Team.createDate);
        packet.WriteInt(Player.PlayerCharacter.Team.teamScore);
        packet.WriteInt(Player.PlayerCharacter.Team.totalMember);
        packet.WriteInt(nextLvExp2 != null ? nextLvExp2.MaxPlayerNum : 0);
        packet.WriteInt(Player.PlayerCharacter.Team.season);
        packet.WriteString("");
        packet.WriteInt(Player.PlayerCharacter.TeamUser.IsCaption ? 1 : 0);
        packet.WriteDateTime(Player.PlayerCharacter.Team.teamLoginDate);
        Player.Out.SendTCP(packet);
        TeamMgr.UpdateTeamPropertise(Player);
      }
    }

    public static int MaxLv() => ((IEnumerable<BattleTeamLevelLists>) TeamMgr.battleTeamLevelList.ToArray()).Last<BattleTeamLevelLists>().Level;

    public static int MaxDivision() => ((IEnumerable<BattleTeamSegmentLists>) TeamMgr.battleTeamSegmentList.ToArray()).Last<BattleTeamSegmentLists>().SegmentID;

    public static void AddnewPlayerToTeam(
      PlayerData db,
      PlayerInfo PlayerCharacter,
      int teamID,
      bool IsCaption,
      Ybx_89_Team Team = null)
    {
      if (!IsCaption)
        PlayerCharacter.Team = Team;
      Ybx_89_Team_User entity = db.Ybx_89_Team_User.Where<Ybx_89_Team_User>((Expression<Func<Ybx_89_Team_User, bool>>) (p => p.UserID == PlayerCharacter.ID)).FirstOrDefault<Ybx_89_Team_User>();
      if (entity != null)
      {
        entity.teamID = teamID;
        entity.IsCaption = IsCaption;
        entity.CreateDate = DateTime.Now;
        db.Entry<Ybx_89_Team_User>(entity).State = EntityState.Modified;
        PlayerCharacter.TeamUser = entity;
      }
      else
      {
        PlayerCharacter.TeamUser = new Ybx_89_Team_User();
        PlayerCharacter.TeamUser.UserID = PlayerCharacter.ID;
        PlayerCharacter.TeamUser.teamID = teamID;
        PlayerCharacter.TeamUser.BattleScore = 0;
        PlayerCharacter.TeamUser.IsCaption = IsCaption;
        PlayerCharacter.TeamUser.CreateDate = DateTime.Now;
        db.Ybx_89_Team_User.Add(PlayerCharacter.TeamUser);
      }
      TeamMgr.SendToAll(PlayerCharacter.Team.teamID);
    }

    public static void onGetInviteList(PlayerData db, GamePlayer Player)
    {
      List<Ybx_89_TeamInviteList> list = db.Ybx_89_TeamInviteList.Where<Ybx_89_TeamInviteList>((Expression<Func<Ybx_89_TeamInviteList, bool>>) (p => p.teamID == Player.PlayerCharacter.Team.teamID)).ToList<Ybx_89_TeamInviteList>();
      GSPacketIn packet = new GSPacketIn((short) 390, Player.PlayerId);
      packet.WriteByte((byte) 15);
      packet.WriteInt(list.Count);
      foreach (Ybx_89_TeamInviteList obj in list)
      {
        packet.WriteInt(obj.ID);
        packet.WriteString(obj.name);
        packet.WriteDateTime(obj.date);
        packet.WriteInt(obj.VipLv);
      }
      Player.Out.SendTCP(packet);
    }

    public static void exitteam(PlayerData db, GamePlayer Player, bool IsCaption)
    {
      Player.SendMessage("Thoát khỏi đội thành công");
      Ybx_89_Team teaminDb = Player.PlayerCharacter.Team;
      TeamMgr.ClearTeam(db, Player);
      if (teaminDb.totalMember > 1 & IsCaption)
      {
        Ybx_89_Team_User entity = db.Ybx_89_Team_User.Where<Ybx_89_Team_User>((Expression<Func<Ybx_89_Team_User, bool>>) (p => p.teamID == teaminDb.teamID && p.UserID != Player.PlayerId && !p.IsCaption)).OrderBy<Ybx_89_Team_User, DateTime>((Expression<Func<Ybx_89_Team_User, DateTime>>) (p => p.CreateDate)).FirstOrDefault<Ybx_89_Team_User>();
        if (entity != null)
        {
          entity.IsCaption = true;
          GamePlayer playerById = WorldMgr.GetPlayerById(entity.UserID);
          if (playerById != null)
          {
            playerById.PlayerCharacter.TeamUser = entity;
            playerById.Out.SendMessage(eMessageType.Normal, "Đội trưởng của bạn đã rời đội nên vị trí đội trưởng sẽ do bạn tiếp nhận!");
          }
          db.Entry<Ybx_89_Team_User>(entity).State = EntityState.Modified;
          db.SaveChanges();
        }
      }
      --teaminDb.totalMember;
      TeamMgr.SaveTeam(db, teaminDb);
      TeamMgr.SendToSingle(Player);
      TeamMgr.SendToAll(teaminDb.teamID);
      foreach (GamePlayer gamePlayer in WorldMgr.GetAllPlayersByTeamId(teaminDb.teamID))
        gamePlayer.Out.SendMessage(eMessageType.Normal, "Người chơi " + Player.PlayerCharacter.NickName + " đã rời đội ngũ!");
    }

    public static void SendUpdateDayActive(GamePlayer player)
    {
      GSPacketIn packet = new GSPacketIn((short) 390, player.PlayerId);
      packet.WriteByte((byte) 12);
      packet.WriteInt(player.PlayerCharacter.TeamDayActive.selfScore);
      packet.WriteInt(player.PlayerCharacter.TeamDayActive.selfActive);
      packet.WriteInt(player.PlayerCharacter.TeamDayActive.selfAllActive);
      player.Out.SendTCP(packet);
    }

    public static void SendOpenClose(GamePlayer player)
    {
      GSPacketIn packet = new GSPacketIn((short) 390, player.PlayerId);
      packet.WriteByte((byte) 14);
      packet.WriteBoolean(true);
      player.Out.SendTCP(packet);
    }

    public static void SaveTeam(PlayerData db, Ybx_89_Team team)
    {
      db.Entry<Ybx_89_Team>(team).State = EntityState.Modified;
      db.SaveChanges();
    }

    public static void SaveUserTeam(PlayerData db, Ybx_89_Team_User userteam)
    {
      db.Entry<Ybx_89_Team_User>(userteam).State = EntityState.Modified;
      db.SaveChanges();
    }

    public static void UpdateTeamPropertise(GamePlayer Player)
    {
      GSPacketIn packet = new GSPacketIn((short) 390, Player.PlayerId);
      packet.WriteByte((byte) 19);
      Player.Out.SendTCP(packet);
    }

    public static void ClearTeam(PlayerData db, GamePlayer Player)
    {
      Player.PlayerCharacter.Team = new Ybx_89_Team();
      Ybx_89_Team_User entity = db.Ybx_89_Team_User.Find(new object[1]
      {
        (object) Player.PlayerCharacter.TeamUser.ID
      });
      entity.teamID = 0;
      entity.IsCaption = false;
      Player.PlayerCharacter.TeamUser = entity;
      db.Entry<Ybx_89_Team_User>(entity).State = EntityState.Modified;
      db.SaveChanges();
    }

    public static void GetActiveList(PlayerData db, GamePlayer Player)
    {
      List<Ybx_89_TeamActive> list = db.Ybx_89_TeamActive.Where<Ybx_89_TeamActive>((Expression<Func<Ybx_89_TeamActive, bool>>) (p => p.teamID == Player.PlayerCharacter.Team.teamID)).ToList<Ybx_89_TeamActive>();
      GSPacketIn packet = new GSPacketIn((short) 390, Player.PlayerId);
      packet.WriteByte((byte) 10);
      packet.WriteInt(list.Count);
      foreach (Ybx_89_TeamActive ybx89TeamActive in list)
      {
        packet.WriteString(ybx89TeamActive.NickName);
        packet.WriteInt(ybx89TeamActive.UserActive);
        packet.WriteInt(ybx89TeamActive.ActiveType);
      }
      Player.Out.SendTCP(packet);
    }

    public static int GetTotalMemberAvable(int TeamId) => new PlayerData().Ybx_89_Team_User.Where<Ybx_89_Team_User>((Expression<Func<Ybx_89_Team_User, bool>>) (p => p.teamID == TeamId && SqlFunctions.DateDiff("hour", (DateTime?) p.CreateDate, (DateTime?) DateTime.Now) >= (int?) 24)).OrderByDescending<Ybx_89_Team_User, int>((Expression<Func<Ybx_89_Team_User, int>>) (p => p.BattleScore)).Count<Ybx_89_Team_User>();

    public static int GetTotalMembers(int TeamId) => new PlayerData().Ybx_89_Team_User.Where<Ybx_89_Team_User>((Expression<Func<Ybx_89_Team_User, bool>>) (p => p.teamID == TeamId)).Count<Ybx_89_Team_User>();

    public static int GetCurrentDivision(int Scrore) => TeamMgr.battleTeamSegmentList.Where<BattleTeamSegmentLists>((Func<BattleTeamSegmentLists, bool>) (p => p.NeedScore >= Scrore)).FirstOrDefault<BattleTeamSegmentLists>().SegmentID;

    public static int GetTeamScore(int TeamId)
    {
      PlayerData playerData = new PlayerData();
      int teamScore = 0;
      List<Ybx_89_Team_User> list = playerData.Ybx_89_Team_User.Where<Ybx_89_Team_User>((Expression<Func<Ybx_89_Team_User, bool>>) (p => p.teamID == TeamId && SqlFunctions.DateDiff("hour", (DateTime?) p.CreateDate, (DateTime?) DateTime.Now) >= (int?) 24)).OrderByDescending<Ybx_89_Team_User, int>((Expression<Func<Ybx_89_Team_User, int>>) (p => p.BattleScore)).ToList<Ybx_89_Team_User>();
      if (list.Count > 0)
      {
        Ybx_89_Team_User ybx89TeamUser1 = list.FirstOrDefault<Ybx_89_Team_User>();
        if (ybx89TeamUser1.BattleScore > 0)
        {
          int num1 = 0;
          foreach (Ybx_89_Team_User ybx89TeamUser2 in list)
          {
            int y = ybx89TeamUser2.BattleScore / ybx89TeamUser1.BattleScore;
            num1 += Convert.ToInt32(Math.Pow(16.0, (double) y));
          }
          foreach (Ybx_89_Team_User ybx89TeamUser3 in list)
          {
            int y = ybx89TeamUser3.BattleScore / ybx89TeamUser1.BattleScore;
            teamScore += ybx89TeamUser3.BattleScore * Convert.ToInt32(Math.Pow(16.0, (double) y)) / num1;
          }
          double num2 = 0.0;
          switch (list.Count)
          {
            case 2:
              num2 = 1.0;
              break;
            case 3:
              num2 = 1.05;
              break;
            case 4:
              num2 = 1.06;
              break;
            case 5:
              num2 = 1.07;
              break;
            case 6:
              num2 = 1.08;
              break;
            case 7:
              num2 = 1.09;
              break;
            case 8:
              num2 = 1.1;
              break;
            case 9:
              num2 = 1.11;
              break;
          }
          teamScore = (int) Convert.ToInt16((double) teamScore * num2);
        }
      }
      return teamScore;
    }

    public static BattleTeamLevelLists GetNextLvExp(int nextlv) => TeamMgr.battleTeamLevelList.Where<BattleTeamLevelLists>((Func<BattleTeamLevelLists, bool>) (p => p.Level == nextlv)).FirstOrDefault<BattleTeamLevelLists>();

    public static void AddScore(
      PlayerData db,
      GamePlayer Player,
      int Score,
      int Type,
      string Mes,
      ref int AddMoney)
    {
      if (Player.PlayerCharacter.Team.teamGrade == TeamMgr.MaxLv())
        Player.Out.SendMessage(eMessageType.Normal, "Đội của bạn đã đạp cấp tối đa, không thể nhận thêm exp!");
      else if (!TeamMgr.CanAddScore(Player, Type))
        Player.Out.SendMessage(eMessageType.Normal, "Bạn đã đạt giới hạn cho hoạt động này nên không thể nhận điểm năng động!");
      else if ((DateTime.Now - Player.PlayerCharacter.TeamUser.CreateDate).TotalHours < 24.0)
      {
        Player.Out.SendMessage(eMessageType.Normal, "Thời gian vào đội của bạn chưa đủ 24h nên không được tính điểm năng động!");
      }
      else
      {
        Ybx_89_Team_User entity1 = db.Ybx_89_Team_User.Find(new object[1]
        {
          (object) Player.PlayerCharacter.TeamUser.ID
        });
        Ybx_89_Team entity2 = db.Ybx_89_Team.Find(new object[1]
        {
          (object) Player.PlayerCharacter.Team.teamID
        });
        if (entity1 == null || entity2 == null)
          return;
        entity1.ActiveTotalScore += Score;
        entity1.ActiveWeekScore += Score;
        entity1.ActiveSeasonScore += Score;
        Ybx_89_TeamActive entity3 = new Ybx_89_TeamActive();
        entity3.ActiveType = Type;
        entity3.NickName = Player.PlayerCharacter.NickName;
        entity3.teamID = entity2.teamID;
        entity3.UserActive = Score;
        entity2.active += Score;
        entity2.totalActive += Score;
        int ResidualActive = 0;
        int active = entity2.active;
        if (TeamMgr.CanUpLv(entity2.teamGrade, entity2.active, ref ResidualActive))
        {
          ++entity2.teamGrade;
          entity2.active = 0;
          if (ResidualActive > 0)
          {
            entity2.totalActive -= ResidualActive;
            entity1.ActiveTotalScore -= ResidualActive;
            entity1.ActiveWeekScore -= ResidualActive;
            entity1.ActiveSeasonScore -= ResidualActive;
            entity3.UserActive -= ResidualActive;
            AddMoney = GameProperties.MoneyRichesOffer * ResidualActive;
          }
        }
        TeamMgr.AddSelfActive(Player, Score - ResidualActive, Type);
        db.Ybx_89_TeamActive.Add(entity3);
        db.Entry<Ybx_89_Team>(entity2).State = EntityState.Modified;
        db.Entry<Ybx_89_Team_User>(entity1).State = EntityState.Modified;
        Player.PlayerCharacter.Team = entity2;
        Player.PlayerCharacter.TeamUser = entity1;
        Player.Out.SendMessage(eMessageType.Normal, Mes);
        db.SaveChanges();
        TeamMgr.SendToAll(entity2.teamID);
      }
    }

    public static void AddBattleScore(PlayerData db, GamePlayer Player, int Score)
    {
      Ybx_89_Team_User entity = db.Ybx_89_Team_User.Find(new object[1]
      {
        (object) Player.PlayerCharacter.TeamUser.ID
      });
      if ((Player.PlayerCharacter.TeamUser.CreateDate - DateTime.Now).TotalHours < 24.0)
      {
        Player.Out.SendMessage(eMessageType.Normal, "Thời gian vào đội của bạn chưa đủ 24h nên không được tính điểm xếp hạng!");
      }
      else
      {
        entity.BattleScore += Score;
        if (entity.BattleScore < 0)
          entity.BattleScore = 0;
        db.Entry<Ybx_89_Team_User>(entity).State = EntityState.Modified;
        Player.PlayerCharacter.TeamUser = entity;
        Player.PlayerCharacter.TeamDayActive.selfScore += Score;
        db.SaveChanges();
        TeamMgr.SendToAll(entity.teamID);
        TeamMgr.SendUpdateDayActive(Player);
      }
    }

    public static void AddSelfActive(GamePlayer Player, int Score, int Type)
    {
      if (!TeamMgr.CanAddScore(Player, Type))
        return;
      Ybx_89_TeamSelfActive obj1 = Player.PlayerCharacter.TeamSelfActive.Where<Ybx_89_TeamSelfActive>((Func<Ybx_89_TeamSelfActive, bool>) (p => p.Type == Type)).FirstOrDefault<Ybx_89_TeamSelfActive>();
      if (obj1 != null)
      {
        Ybx_89_TeamSelfActive obj2 = obj1;
        ++obj2.Times;
        obj2.haveScore += Score;
      }
      else
        Player.PlayerCharacter.TeamSelfActive.Add(new Ybx_89_TeamSelfActive()
        {
          UserID = Player.PlayerId,
          Type = Type,
          Times = 1,
          haveScore = Score,
          LastDate = DateTime.Now
        });
      Player.PlayerCharacter.TeamDayActive.selfActive += Score;
      Player.PlayerCharacter.TeamDayActive.selfAllActive += Score;
      TeamMgr.SendUpdateDayActive(Player);
    }

    public static bool CanAddScore(GamePlayer Player, int Type)
    {
      BattleTeamActiveTemplateLists activeTemplateLists = TeamMgr.ActiveList.Where<BattleTeamActiveTemplateLists>((Func<BattleTeamActiveTemplateLists, bool>) (p => p.Type == Type)).FirstOrDefault<BattleTeamActiveTemplateLists>();
      Ybx_89_TeamSelfActive obj = Player.PlayerCharacter.TeamSelfActive.Where<Ybx_89_TeamSelfActive>((Func<Ybx_89_TeamSelfActive, bool>) (p => p.Type == Type)).FirstOrDefault<Ybx_89_TeamSelfActive>();
      return obj == null || activeTemplateLists.MaxLimit > obj.haveScore;
    }

    public static bool CanUpLv(int currentLv, int currentExp, ref int ResidualActive)
    {
      int nextLevel = currentLv + 1;
      BattleTeamLevelLists battleTeamLevelLists = TeamMgr.battleTeamLevelList.Where<BattleTeamLevelLists>((Func<BattleTeamLevelLists, bool>) (p => p.Level == nextLevel)).FirstOrDefault<BattleTeamLevelLists>();
      if (currentExp < battleTeamLevelLists.NeedActive)
        return false;
      ResidualActive = currentExp - battleTeamLevelLists.NeedActive;
      return true;
    }

    public static bool CanUpDivision(int currentDivision, int currentScore)
    {
      int nextDivision = currentDivision + 1;
      BattleTeamSegmentLists teamSegmentLists = TeamMgr.battleTeamSegmentList.Where<BattleTeamSegmentLists>((Func<BattleTeamSegmentLists, bool>) (p => p.SegmentID == nextDivision)).FirstOrDefault<BattleTeamSegmentLists>();
      return currentDivision >= teamSegmentLists.NeedScore;
    }

    public static int CheckCreateTeam(string text, bool typesubmit, PlayerData db)
    {
      int team = 0;
      if (text.Length > 12 || text.Length < 2)
        team = 3;
      int num;
      if (typesubmit)
        num = db.Ybx_89_Team.Where<Ybx_89_Team>((Expression<Func<Ybx_89_Team, bool>>) (p => p.teamName == text)).Count<Ybx_89_Team>();
      else
        num = db.Ybx_89_Team.Where<Ybx_89_Team>((Expression<Func<Ybx_89_Team, bool>>) (p => p.teamTag == text)).Count<Ybx_89_Team>();
      if (num > 0)
        team = 2;
      return team;
    }

    public static bool CheckBeforAddNewUsers(GamePlayer Player) => TeamMgr.GetTotalMembers(Player.PlayerCharacter.Team.teamID) != TeamMgr.battleTeamLevelList.Where<BattleTeamLevelLists>((Func<BattleTeamLevelLists, bool>) (p => p.Level == Player.PlayerCharacter.Team.teamGrade)).FirstOrDefault<BattleTeamLevelLists>().MaxPlayerNum;

    public static void traotoptuan(ref string Mes)
    {
      PlayerData playerData = new PlayerData();
      ServerData serverData = new ServerData();
      List<Ybx_89_Team> list1 = playerData.Ybx_89_Team.OrderByDescending<Ybx_89_Team, int>((Expression<Func<Ybx_89_Team, int>>) (p => p.teamScore)).Take<Ybx_89_Team>(5).ToList<Ybx_89_Team>();
      List<BattleTeamSegmentLists> list2 = serverData.BattleTeamSegmentLists.OrderByDescending<BattleTeamSegmentLists, int>((Expression<Func<BattleTeamSegmentLists, int>>) (p => p.SegmentID)).ToList<BattleTeamSegmentLists>();
      int index = 1;
      try
      {
        foreach (Ybx_89_Team ybx89Team in list1)
        {
          Ybx_89_Team item = ybx89Team;
          DbSet<Ybx_89_Team_User> ybx89TeamUser1 = playerData.Ybx_89_Team_User;
          Expression<Func<Ybx_89_Team_User, bool>> predicate = (Expression<Func<Ybx_89_Team_User, bool>>) (p => p.teamID == item.teamID);
          foreach (Ybx_89_Team_User ybx89TeamUser2 in ybx89TeamUser1.Where<Ybx_89_Team_User>(predicate).ToList<Ybx_89_Team_User>())
          {
            using (PlayerBussiness playerBussiness = new PlayerBussiness())
            {
              PlayerInfo userSingleByUserId = playerBussiness.GetUserSingleByUserID(ybx89TeamUser2.UserID);
              SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(list2[index].GiftBagId), 1, 105);
              fromTemplate.IsBinds = true;
              fromTemplate.UserID = 0;
              playerBussiness.AddGoods(fromTemplate);
              MailInfo mail = new MailInfo()
              {
                Annex1 = fromTemplate.ItemID.ToString(),
                Content = "Phần thưởng tuần tổ đội",
                Gold = 0,
                Money = 0,
                Receiver = userSingleByUserId.NickName,
                ReceiverID = userSingleByUserId.ID
              };
              mail.Sender = mail.Receiver;
              mail.SenderID = mail.ReceiverID;
              mail.Title = "Quà thưởng tuần top " + index.ToString() + " tổ đội";
              mail.Type = 12;
              playerBussiness.SendMail(mail);
              GamePlayer playerById = WorldMgr.GetPlayerById(userSingleByUserId.ID);
              playerById?.Out.SendMailResponse(playerById.PlayerCharacter.ID, eMailRespose.Receiver);
            }
          }
          ++index;
        }
        Mes = "trao thưởng hoàn tất";
      }
      catch (Exception ex)
      {
        Mes = "trao thưởng thất bại " + ex.ToString();
      }
    }

    public static void traotopmua(ref string Mes)
    {
      try
      {
        PlayerData playerData = new PlayerData();
        ServerData serverData = new ServerData();
        List<Ybx_89_Team> list = playerData.Ybx_89_Team.OrderByDescending<Ybx_89_Team, int>((Expression<Func<Ybx_89_Team, int>>) (p => p.teamScore)).Take<Ybx_89_Team>(3).ToList<Ybx_89_Team>();
        string[] strArray = serverData.BattleTeamSeasonLists.FirstOrDefault<BattleTeamSeasonLists>().RankGift.Split(',');
        int index = 0;
        foreach (Ybx_89_Team ybx89Team in list)
        {
          Ybx_89_Team item = ybx89Team;
          DbSet<Ybx_89_Team_User> ybx89TeamUser1 = playerData.Ybx_89_Team_User;
          Expression<Func<Ybx_89_Team_User, bool>> predicate = (Expression<Func<Ybx_89_Team_User, bool>>) (p => p.teamID == item.teamID);
          foreach (Ybx_89_Team_User ybx89TeamUser2 in ybx89TeamUser1.Where<Ybx_89_Team_User>(predicate).ToList<Ybx_89_Team_User>())
          {
            using (PlayerBussiness playerBussiness = new PlayerBussiness())
            {
              PlayerInfo userSingleByUserId = playerBussiness.GetUserSingleByUserID(ybx89TeamUser2.UserID);
              SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(Convert.ToInt32(strArray[index])), 1, 105);
              fromTemplate.IsBinds = true;
              fromTemplate.UserID = 0;
              playerBussiness.AddGoods(fromTemplate);
              MailInfo mail = new MailInfo()
              {
                Annex1 = fromTemplate.ItemID.ToString(),
                Content = "Phần thưởng mùa giải tổ đội",
                Gold = 0,
                Money = 0,
                Receiver = userSingleByUserId.NickName,
                ReceiverID = userSingleByUserId.ID
              };
              mail.Sender = mail.Receiver;
              mail.SenderID = mail.ReceiverID;
              mail.Title = "Quà thưởng mùa giải top " + index.ToString() + " tổ đội";
              mail.Type = 12;
              playerBussiness.SendMail(mail);
              GamePlayer playerById = WorldMgr.GetPlayerById(userSingleByUserId.ID);
              playerById?.Out.SendMailResponse(playerById.PlayerCharacter.ID, eMailRespose.Receiver);
            }
          }
          ++index;
        }
        foreach (Ybx_89_Team ybx89Team in playerData.Ybx_89_Team.ToList<Ybx_89_Team>())
        {
          Ybx_89_Team item = ybx89Team;
          DbSet<Ybx_89_Team_User> ybx89TeamUser = playerData.Ybx_89_Team_User;
          Expression<Func<Ybx_89_Team_User, bool>> predicate = (Expression<Func<Ybx_89_Team_User, bool>>) (p => p.teamID == item.teamID && p.teamID != 0);
          foreach (Ybx_89_Team_User entity in ybx89TeamUser.Where<Ybx_89_Team_User>(predicate).ToList<Ybx_89_Team_User>())
          {
            entity.ActiveSeasonScore = 0;
            entity.BattleScore = 0;
            entity.ActiveWeekScore = 0;
            entity.ActiveTotalScore = 0;
            GamePlayer playerById = WorldMgr.GetPlayerById(entity.UserID);
            if (playerById != null)
            {
              playerById.PlayerCharacter.TeamUser = entity;
              playerData.Entry<Ybx_89_Team_User>(entity).State = EntityState.Modified;
            }
          }
          item.teamGrade = 1;
          item.teamWinTime = 0;
          item.teamTotalTime = 0;
          item.teamDivision = 0;
          item.teamScore = 0;
          item.teamDuty = 0;
          item.teamPersonalScore = 0;
          item.freeInvitedUsedCnt = 0;
          item.season = 0;
          item.active = 0;
          playerData.Entry<Ybx_89_Team>(item).State = EntityState.Modified;
          playerData.SaveChanges();
          playerData.Database.ExecuteSqlCommand("truncate table Ybx_89_TeamRecord");
          playerData.Database.ExecuteSqlCommand("truncate table Ybx_89_TeamActive");
          TeamMgr.SendToAll(item.teamID);
        }
        Mes = "trao top mùa thành công";
      }
      catch
      {
        Mes = "trao top mùa thất bại";
      }
    }
  }
}
