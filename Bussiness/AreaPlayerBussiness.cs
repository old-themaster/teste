using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using Bussiness.Managers;
using log4net;
using SqlDataProvider.BaseClass;
using SqlDataProvider.Data;

namespace Bussiness

{

	public class AreaPlayerBussiness : IDisposable
	{
		private static readonly ILog ilog_0;

		protected Sql_DbObject db;

		private string string_0;

		private int int_0;

		public AreaPlayerBussiness(AreaConfigInfo config)
		{
			string_0 = "Area 1";
			int_0 = 1;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Data Source=");
			stringBuilder.Append(config.DataSource);
			stringBuilder.Append(";Initial Catalog=");
			stringBuilder.Append(config.Catalog);
			stringBuilder.Append(";Persist Security Info=True;User ID=");
			stringBuilder.Append(config.UserID);
			stringBuilder.Append(";Password=");
			stringBuilder.Append(config.Password);
			string conn_DB = stringBuilder.ToString();
			db = new Sql_DbObject("AreaConfig", conn_DB);
			string_0 = config.AreaName;
			int_0 = config.AreaID;
		}

		public bool UpdateRenames(string data)
		{
			bool result = false;
			try
			{
				SqlParameter[] array = new SqlParameter[1]
				{
				new SqlParameter("@Db_A", SqlDbType.NVarChar, 100)
				};
				array[0].Value = data;
				result = db.RunProcedure("Sp_Renames_Batch", array);
			}
			catch (Exception ex)
			{
				if (ilog_0.IsErrorEnabled)
				{
					ilog_0.Error((object)"Init Sp_Renames_Batch", ex);
				}
			}
			return result;
		}

		public PlayerInfo GetUserSingleByUserID(int UserID)
		{
			SqlDataReader ResultDataReader = null;
			try
			{
				SqlParameter[] array = new SqlParameter[1]
				{
				new SqlParameter("@UserID", SqlDbType.Int, 4)
				};
				array[0].Value = UserID;
				db.GetReader(ref ResultDataReader, "SP_Users_SingleByUserID", array);
				if (ResultDataReader.Read())
				{
					return InitPlayerInfo(ResultDataReader);
				}
			}
			catch (Exception ex)
			{
				if (ilog_0.IsErrorEnabled)
				{
					ilog_0.Error((object)"Init", ex);
				}
			}
			finally
			{
				if (ResultDataReader != null && !ResultDataReader.IsClosed)
				{
					ResultDataReader.Close();
				}
			}
			return null;
		}

		public PlayerInfo InitPlayerInfo(SqlDataReader reader)
		{
			PlayerInfo playerInfo = new PlayerInfo();
			playerInfo.Password = (string)reader["Password"];
			playerInfo.IsConsortia = (bool)reader["IsConsortia"];
			playerInfo.Agility = (int)reader["Agility"];
			playerInfo.Attack = (int)reader["Attack"];
			playerInfo.hp = (int)reader["hp"];
			playerInfo.Colors = ((reader["Colors"] == null) ? "" : reader["Colors"].ToString());
			playerInfo.ConsortiaID = (int)reader["ConsortiaID"];
			playerInfo.Defence = (int)reader["Defence"];
			playerInfo.Gold = (int)reader["Gold"];
			playerInfo.GP = (int)reader["GP"];
			playerInfo.Grade = (int)reader["Grade"];
			playerInfo.ID = (int)reader["UserID"];
			playerInfo.Luck = (int)reader["Luck"];
			playerInfo.Money = (int)reader["Money"];
			playerInfo.NickName = (((string)reader["NickName"] == null) ? "" : ((string)reader["NickName"]));
			playerInfo.Sex = (bool)reader["Sex"];
			playerInfo.State = (int)reader["State"];
			playerInfo.Style = ((reader["Style"] == null) ? "" : reader["Style"].ToString());
			playerInfo.Hide = (int)reader["Hide"];
			playerInfo.Repute = (int)reader["Repute"];
			playerInfo.UserName = ((reader["UserName"] == null) ? "" : reader["UserName"].ToString());
			playerInfo.ConsortiaName = ((reader["ConsortiaName"] == null) ? "" : reader["ConsortiaName"].ToString());
			playerInfo.Offer = (int)reader["Offer"];
			playerInfo.Win = (int)reader["Win"];
			playerInfo.Total = (int)reader["Total"];
			playerInfo.Escape = (int)reader["Escape"];
			playerInfo.Skin = ((reader["Skin"] == null) ? "" : reader["Skin"].ToString());
			playerInfo.IsBanChat = (bool)reader["IsBanChat"];
			playerInfo.ReputeOffer = (int)reader["ReputeOffer"];
			playerInfo.ConsortiaRepute = (int)reader["ConsortiaRepute"];
			playerInfo.ConsortiaLevel = (int)reader["ConsortiaLevel"];
			playerInfo.StoreLevel = (int)reader["StoreLevel"];
			playerInfo.ShopLevel = (int)reader["ShopLevel"];
			playerInfo.SmithLevel = (int)reader["SmithLevel"];
			playerInfo.ConsortiaHonor = (int)reader["ConsortiaHonor"];
			playerInfo.RichesOffer = (int)reader["RichesOffer"];
			playerInfo.RichesRob = (int)reader["RichesRob"];
			playerInfo.AntiAddiction = (int)reader["AntiAddiction"];
			playerInfo.DutyLevel = (int)reader["DutyLevel"];
			playerInfo.DutyName = ((reader["DutyName"] == null) ? "" : reader["DutyName"].ToString());
			playerInfo.Right = (int)reader["Right"];
			playerInfo.ChairmanName = ((reader["ChairmanName"] == null) ? "" : reader["ChairmanName"].ToString());
			playerInfo.AddDayGP = (int)reader["AddDayGP"];
			playerInfo.AddDayOffer = (int)reader["AddDayOffer"];
			playerInfo.AddWeekGP = (int)reader["AddWeekGP"];
			playerInfo.AddWeekOffer = (int)reader["AddWeekOffer"];
			playerInfo.ConsortiaRiches = (int)reader["ConsortiaRiches"];
			playerInfo.CheckCount = (int)reader["CheckCount"];
			playerInfo.IsMarried = (bool)reader["IsMarried"];
			playerInfo.SpouseID = (int)reader["SpouseID"];
			playerInfo.SpouseName = ((reader["SpouseName"] == null) ? "" : reader["SpouseName"].ToString());
			playerInfo.MarryInfoID = (int)reader["MarryInfoID"];
			playerInfo.IsCreatedMarryRoom = (bool)reader["IsCreatedMarryRoom"];
			playerInfo.DayLoginCount = (int)reader["DayLoginCount"];
			playerInfo.PasswordTwo = ((reader["PasswordTwo"] == null) ? "" : reader["PasswordTwo"].ToString());
			playerInfo.SelfMarryRoomID = (int)reader["SelfMarryRoomID"];
			playerInfo.IsGotRing = (bool)reader["IsGotRing"];
			playerInfo.Rename = (bool)reader["Rename"];
			playerInfo.ConsortiaRename = (bool)reader["ConsortiaRename"];
			playerInfo.IsDirty = false;
			playerInfo.IsFirst = (int)reader["IsFirst"];
			playerInfo.Nimbus = (int)reader["Nimbus"];
			playerInfo.LastAward = (DateTime)reader["LastAward"];
			playerInfo.GiftToken = (int)reader["GiftToken"];
			playerInfo.QuestSite = ((reader["QuestSite"] == null) ? new byte[200] : ((byte[])reader["QuestSite"]));
			playerInfo.PvePermission = ((reader["PvePermission"] == null) ? "" : reader["PvePermission"].ToString());
			playerInfo.FightPower = (int)reader["FightPower"];
			playerInfo.PasswordQuest1 = ((reader["PasswordQuestion1"] == null) ? "" : reader["PasswordQuestion1"].ToString());
			playerInfo.PasswordQuest2 = ((reader["PasswordQuestion2"] == null) ? "" : reader["PasswordQuestion2"].ToString());
			playerInfo.FailedPasswordAttemptCount = ((!((DateTime)reader["LastFindDate"] != DateTime.Today.Date)) ? ((int)reader["FailedPasswordAttemptCount"]) : 5);
			playerInfo.AnswerSite = (int)reader["AnswerSite"];
			playerInfo.medal = (int)reader["Medal"];
			playerInfo.ChatCount = (int)reader["ChatCount"];
			playerInfo.SpaPubGoldRoomLimit = (int)reader["SpaPubGoldRoomLimit"];
			playerInfo.LastSpaDate = (DateTime)reader["LastSpaDate"];
			playerInfo.FightLabPermission = (string)reader["FightLabPermission"];
			playerInfo.SpaPubMoneyRoomLimit = (int)reader["SpaPubMoneyRoomLimit"];
			playerInfo.IsInSpaPubGoldToday = (bool)reader["IsInSpaPubGoldToday"];
			playerInfo.IsInSpaPubMoneyToday = (bool)reader["IsInSpaPubMoneyToday"];
			playerInfo.AchievementPoint = (int)reader["AchievementPoint"];
			playerInfo.LastWeekly = (DateTime)reader["LastWeekly"];
			playerInfo.LastWeeklyVersion = (int)reader["LastWeeklyVersion"];
			playerInfo.badgeID = (int)reader["BadgeID"];
			playerInfo.typeVIP = Convert.ToByte(reader["typeVIP"]);
			playerInfo.VIPLevel = (int)reader["VIPLevel"];
			playerInfo.VIPExp = (int)reader["VIPExp"];
			playerInfo.VIPExpireDay = (DateTime)reader["VIPExpireDay"];
			playerInfo.VIPNextLevelDaysNeeded = (int)reader["VIPNextLevelDaysNeeded"];
			playerInfo.LastVIPPackTime = (DateTime)reader["LastVIPPackTime"];
			playerInfo.CanTakeVipReward = (bool)reader["CanTakeVipReward"];
			playerInfo.WeaklessGuildProgressStr = (string)reader["WeaklessGuildProgressStr"];
			playerInfo.IsOldPlayer = (bool)reader["IsOldPlayer"];
			playerInfo.LastDate = (DateTime)reader["LastDate"];
			playerInfo.VIPLastDate = (DateTime)reader["VIPLastDate"];
			playerInfo.Score = (int)reader["Score"];
			playerInfo.OptionOnOff = (int)reader["OptionOnOff"];
			playerInfo.isOldPlayerHasValidEquitAtLogin = (bool)reader["isOldPlayerHasValidEquitAtLogin"];
			playerInfo.badLuckNumber = (int)reader["badLuckNumber"];
			playerInfo.OnlineTime = (int)reader["OnlineTime"];
			playerInfo.luckyNum = (int)reader["luckyNum"];
			playerInfo.lastLuckyNumDate = (DateTime)reader["lastLuckyNumDate"];
			playerInfo.lastLuckNum = (int)reader["lastLuckNum"];
			playerInfo.IsShowConsortia = (bool)reader["IsShowConsortia"];
			playerInfo.NewDay = (DateTime)reader["NewDay"];
			playerInfo.Honor = (string)reader["Honor"];
			playerInfo.BoxGetDate = (DateTime)reader["BoxGetDate"];
			playerInfo.AlreadyGetBox = (int)reader["AlreadyGetBox"];
			playerInfo.BoxProgression = (int)reader["BoxProgression"];
			playerInfo.GetBoxLevel = (int)reader["GetBoxLevel"];
			playerInfo.IsRecharged = (bool)reader["IsRecharged"];
			playerInfo.IsGetAward = (bool)reader["IsGetAward"];
			playerInfo.apprenticeshipState = (int)reader["apprenticeshipState"];
			playerInfo.masterID = (int)reader["masterID"];
			playerInfo.masterOrApprentices = ((reader["masterOrApprentices"] == DBNull.Value) ? "" : ((string)reader["masterOrApprentices"]));
			playerInfo.graduatesCount = (int)reader["graduatesCount"];
			playerInfo.honourOfMaster = ((reader["honourOfMaster"] == DBNull.Value) ? "" : ((string)reader["honourOfMaster"]));
			playerInfo.freezesDate = ((reader["freezesDate"] == DBNull.Value) ? DateTime.Now : ((DateTime)reader["freezesDate"]));
			playerInfo.charmGP = ((reader["charmGP"] != DBNull.Value) ? ((int)reader["charmGP"]) : 0);
			playerInfo.evolutionGrade = (int)reader["evolutionGrade"];
			playerInfo.evolutionExp = (int)reader["evolutionExp"];
			playerInfo.hardCurrency = (int)reader["hardCurrency"];
			playerInfo.EliteScore = (int)reader["EliteScore"];
			playerInfo.ShopFinallyGottenTime = ((reader["ShopFinallyGottenTime"] == DBNull.Value) ? DateTime.Now.AddDays(-1.0) : ((DateTime)reader["ShopFinallyGottenTime"]));
			playerInfo.MoneyLock = ((reader["MoneyLock"] != DBNull.Value) ? ((int)reader["MoneyLock"]) : 0);
			playerInfo.totemId = (int)reader["totemId"];
			playerInfo.myHonor = (int)reader["myHonor"];
			playerInfo.MaxBuyHonor = (int)reader["MaxBuyHonor"];
			playerInfo.honorId = (int)reader["honorId"];
			playerInfo.accumulativeLoginDays = (int)reader["accumulativeLoginDays"];
			playerInfo.accumulativeAwardDays = (int)reader["accumulativeAwardDays"];
			var data = ((string)reader["GodsRoadLevelData"]).Split(',');
			for (var x = 0; x < data.Length; x++)
			{
				playerInfo.GodsRoadLevelData[x] = bool.Parse(data[x]);
			}
			return playerInfo;
		}

		public ItemInfo InitItem(SqlDataReader reader)
		{
			ItemInfo itemInfo = new ItemInfo(ItemMgr.FindItemTemplate((int)reader["TemplateID"]));
			itemInfo.AgilityCompose = (int)reader["AgilityCompose"];
			itemInfo.AttackCompose = (int)reader["AttackCompose"];
			itemInfo.Color = reader["Color"].ToString();
			itemInfo.Count = (int)reader["Count"];
			itemInfo.DefendCompose = (int)reader["DefendCompose"];
			itemInfo.ItemID = (int)reader["ItemID"];
			itemInfo.LuckCompose = (int)reader["LuckCompose"];
			itemInfo.Place = (int)reader["Place"];
			itemInfo.StrengthenLevel = (int)reader["StrengthenLevel"];
			itemInfo.TemplateID = (int)reader["TemplateID"];
			itemInfo.UserID = (int)reader["UserID"];
			itemInfo.ValidDate = (int)reader["ValidDate"];
			itemInfo.IsDirty = false;
			itemInfo.IsExist = (bool)reader["IsExist"];
			itemInfo.IsBinds = (bool)reader["IsBinds"];
			itemInfo.IsUsed = (bool)reader["IsUsed"];
			itemInfo.BeginDate = (DateTime)reader["BeginDate"];
			itemInfo.IsJudge = (bool)reader["IsJudge"];
			itemInfo.BagType = (int)reader["BagType"];
			itemInfo.Skin = reader["Skin"].ToString();
			itemInfo.RemoveDate = (DateTime)reader["RemoveDate"];
			itemInfo.RemoveType = (int)reader["RemoveType"];
			itemInfo.Hole1 = (int)reader["Hole1"];
			itemInfo.Hole2 = (int)reader["Hole2"];
			itemInfo.Hole3 = (int)reader["Hole3"];
			itemInfo.Hole4 = (int)reader["Hole4"];
			itemInfo.Hole5 = (int)reader["Hole5"];
			itemInfo.Hole6 = (int)reader["Hole6"];
			itemInfo.Hole5Level = (int)reader["Hole5Level"];
			itemInfo.Hole5Exp = (int)reader["Hole5Exp"];
			itemInfo.Hole6Level = (int)reader["Hole6Level"];
			itemInfo.Hole6Exp = (int)reader["Hole6Exp"];
			itemInfo.StrengthenTimes = (int)reader["StrengthenTimes"];
			itemInfo.goldBeginTime = (DateTime)reader["goldBeginTime"];
			itemInfo.goldValidDate = (int)reader["goldValidDate"];
			itemInfo.StrengthenExp = (int)reader["StrengthenExp"];
			itemInfo.GoldEquip = ItemMgr.FindGoldItemTemplate(itemInfo.TemplateID, itemInfo.isGold);
			itemInfo.IsDirty = false;
			return itemInfo;
		}

		public UsersPetInfo InitPet(SqlDataReader reader)
		{
			return new UsersPetInfo
			{
				ID = (int)reader["ID"],
				TemplateID = (int)reader["TemplateID"],
				Name = reader["Name"].ToString(),
				UserID = (int)reader["UserID"],
				Attack = (int)reader["Attack"],
				AttackGrow = (int)reader["AttackGrow"],
				Agility = (int)reader["Agility"],
				AgilityGrow = (int)reader["AgilityGrow"],
				Defence = (int)reader["Defence"],
				DefenceGrow = (int)reader["DefenceGrow"],
				Luck = (int)reader["Luck"],
				LuckGrow = (int)reader["LuckGrow"],
				Blood = (int)reader["Blood"],
				BloodGrow = (int)reader["BloodGrow"],
				Damage = (int)reader["Damage"],
				DamageGrow = (int)reader["DamageGrow"],
				Guard = (int)reader["Guard"],
				GuardGrow = (int)reader["GuardGrow"],
				Level = (int)reader["Level"],
				GP = (int)reader["GP"],
				MaxGP = (int)reader["MaxGP"],
				Hunger = (int)reader["Hunger"],
				MP = (int)reader["MP"],
				Place = (int)reader["Place"],
				IsEquip = (bool)reader["IsEquip"],
				IsExit = (bool)reader["IsExit"],
				Skill = reader["Skill"].ToString(),
				SkillEquip = reader["SkillEquip"].ToString(),
				currentStarExp = (int)reader["currentStarExp"],
				breakGrade = (int)reader["breakGrade"],
				breakAttack = (int)reader["breakAttack"],
				breakDefence = (int)reader["breakDefence"],
				breakAgility = (int)reader["breakAgility"],
				breakLuck = (int)reader["breakLuck"],
				breakBlood = (int)reader["breakBlood"],
				eQPets = ((reader["eQPets"] == null) ? "" : reader["eQPets"].ToString()),
				BaseProp = ((reader["BaseProp"] == null) ? "" : reader["BaseProp"].ToString())
			};
		}

		private UsersCardInfo InitCard(SqlDataReader sqlDataReader_0)
		{
			return new UsersCardInfo
			{
				CardID = (int)sqlDataReader_0["CardID"],
				UserID = (int)sqlDataReader_0["UserID"],
				TemplateID = (int)sqlDataReader_0["TemplateID"],
				Place = (int)sqlDataReader_0["Place"],
				Count = (int)sqlDataReader_0["Count"],
				Attack = (int)sqlDataReader_0["Attack"],
				Defence = (int)sqlDataReader_0["Defence"],
				Agility = (int)sqlDataReader_0["Agility"],
				Luck = (int)sqlDataReader_0["Luck"],
				AttackReset = (int)sqlDataReader_0["AttackReset"],
				DefenceReset = (int)sqlDataReader_0["DefenceReset"],
				AgilityReset = (int)sqlDataReader_0["AgilityReset"],
				LuckReset = (int)sqlDataReader_0["LuckReset"],
				Guard = (int)sqlDataReader_0["Guard"],
				Damage = (int)sqlDataReader_0["Damage"],
				Level = (int)sqlDataReader_0["Level"],
				CardGP = (int)sqlDataReader_0["CardGP"],
				isFirstGet = (bool)sqlDataReader_0["isFirstGet"]
			};
		}

		public TexpInfo InitTexpInfo(SqlDataReader reader)
		{
			return new TexpInfo
			{
				UserID = (int)reader["UserID"],
				attTexpExp = (int)reader["attTexpExp"],
				defTexpExp = (int)reader["defTexpExp"],
				hpTexpExp = (int)reader["hpTexpExp"],
				lukTexpExp = (int)reader["lukTexpExp"],
				spdTexpExp = (int)reader["spdTexpExp"],
				texpCount = (int)reader["texpCount"],
				texpTaskCount = (int)reader["texpTaskCount"],
				texpTaskDate = (DateTime)reader["texpTaskDate"]
			};
		}

		public List<UsersCardInfo> GetUserCardEuqip(int UserID)
		{
			List<UsersCardInfo> list = new List<UsersCardInfo>();
			SqlDataReader ResultDataReader = null;
			try
			{
				SqlParameter[] array = new SqlParameter[1]
				{
				new SqlParameter("@UserID", SqlDbType.Int, 4)
				};
				array[0].Value = UserID;
				db.GetReader(ref ResultDataReader, "SP_Users_Items_Card_Equip", array);
				while (ResultDataReader.Read())
				{
					list.Add(InitCard(ResultDataReader));
				}
			}
			catch (Exception ex)
			{
				if (ilog_0.IsErrorEnabled)
				{
					ilog_0.Error((object)"Init", ex);
				}
			}
			finally
			{
				if (ResultDataReader != null && !ResultDataReader.IsClosed)
				{
					ResultDataReader.Close();
				}
			}
			return list;
		}

		public TexpInfo GetUserTexpInfoSingle(int ID)
		{
			SqlDataReader ResultDataReader = null;
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[1]
				{
				new SqlParameter("@UserID", ID)
				};
				db.GetReader(ref ResultDataReader, "SP_Get_UserTexp_By_ID", sqlParameters);
				if (ResultDataReader.Read())
				{
					return InitTexpInfo(ResultDataReader);
				}
			}
			catch (Exception ex)
			{
				if (ilog_0.IsErrorEnabled)
				{
					ilog_0.Error((object)"GetTexpInfoSingle", ex);
				}
			}
			finally
			{
				if (ResultDataReader != null && !ResultDataReader.IsClosed)
				{
					ResultDataReader.Close();
				}
			}
			return null;
		}

		public List<ItemInfo> GetUserEuqip(int UserID)
		{
			List<ItemInfo> list = new List<ItemInfo>();
			SqlDataReader ResultDataReader = null;
			try
			{
				SqlParameter[] array = new SqlParameter[1]
				{
				new SqlParameter("@UserID", SqlDbType.Int, 4)
				};
				array[0].Value = UserID;
				db.GetReader(ref ResultDataReader, "SP_Users_Items_Equip", array);
				while (ResultDataReader.Read())
				{
					list.Add(InitItem(ResultDataReader));
				}
			}
			catch (Exception ex)
			{
				if (ilog_0.IsErrorEnabled)
				{
					ilog_0.Error((object)"Init", ex);
				}
			}
			finally
			{
				if (ResultDataReader != null && !ResultDataReader.IsClosed)
				{
					ResultDataReader.Close();
				}
			}
			return list;
		}

		public List<ItemInfo> GetUserBeadEuqip(int UserID)
		{
			List<ItemInfo> list = new List<ItemInfo>();
			SqlDataReader ResultDataReader = null;
			try
			{
				SqlParameter[] array = new SqlParameter[1]
				{
				new SqlParameter("@UserID", SqlDbType.Int, 4)
				};
				array[0].Value = UserID;
				db.GetReader(ref ResultDataReader, "SP_Users_Bead_Equip", array);
				while (ResultDataReader.Read())
				{
					list.Add(InitItem(ResultDataReader));
				}
			}
			catch (Exception ex)
			{
				if (ilog_0.IsErrorEnabled)
				{
					ilog_0.Error((object)"Init", ex);
				}
			}
			finally
			{
				if (ResultDataReader != null && !ResultDataReader.IsClosed)
				{
					ResultDataReader.Close();
				}
			}
			return list;
		}

		public List<ItemInfo> GetUserMagicstoneEuqip(int UserID)
		{
			List<ItemInfo> list = new List<ItemInfo>();
			SqlDataReader ResultDataReader = null;
			try
			{
				SqlParameter[] array = new SqlParameter[1]
				{
				new SqlParameter("@UserID", SqlDbType.Int, 4)
				};
				array[0].Value = UserID;
				db.GetReader(ref ResultDataReader, "SP_Magicstone_Equip", array);
				while (ResultDataReader.Read())
				{
					list.Add(InitItem(ResultDataReader));
				}
			}
			catch (Exception ex)
			{
				if (ilog_0.IsErrorEnabled)
				{
					ilog_0.Error((object)"Init", ex);
				}
			}
			finally
			{
				if (ResultDataReader != null && !ResultDataReader.IsClosed)
				{
					ResultDataReader.Close();
				}
			}
			return list;
		}

		public ItemInfo[] GetUserBagByType(int UserID, int bagType)
		{
			List<ItemInfo> list = new List<ItemInfo>();
			SqlDataReader ResultDataReader = null;
			try
			{
				SqlParameter[] array = new SqlParameter[2]
				{
				new SqlParameter("@UserID", SqlDbType.Int, 4),
				null
				};
				array[0].Value = UserID;
				array[1] = new SqlParameter("@BagType", bagType);
				db.GetReader(ref ResultDataReader, "SP_Users_BagByType", array);
				while (ResultDataReader.Read())
				{
					list.Add(InitItem(ResultDataReader));
				}
			}
			catch (Exception ex)
			{
				if (ilog_0.IsErrorEnabled)
				{
					ilog_0.Error((object)"Init", ex);
				}
			}
			finally
			{
				if (ResultDataReader != null && !ResultDataReader.IsClosed)
				{
					ResultDataReader.Close();
				}
			}
			return list.ToArray();
		}

		public UsersPetInfo[] GetUserPetSingles(int UserID)
		{
			List<UsersPetInfo> list = new List<UsersPetInfo>();
			SqlDataReader ResultDataReader = null;
			try
			{
				SqlParameter[] array = new SqlParameter[1]
				{
				new SqlParameter("@UserID", SqlDbType.Int, 4)
				};
				array[0].Value = UserID;
				db.GetReader(ref ResultDataReader, "SP_Get_UserPet_By_ID", array);
				while (ResultDataReader.Read())
				{
					list.Add(InitPet(ResultDataReader));
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				if (ResultDataReader != null && !ResultDataReader.IsClosed)
				{
					ResultDataReader.Close();
				}
			}
			return list.ToArray();
		}

		public void Dispose()
		{
			db.Dispose();
			GC.SuppressFinalize(this);
		}

		static AreaPlayerBussiness()
		{
			ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}
	}
}
