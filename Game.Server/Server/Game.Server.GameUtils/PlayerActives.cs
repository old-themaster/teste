using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.GypsyShop;
using Game.Server.Managers;
using Game.Server.Packets;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
namespace Game.Server.GameUtils
{
	public class PlayerActives
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		protected object m_lock = new object();
		protected Timer _christmasTimer;
		protected Timer _labyrinthTimer;
		protected Timer _lightriddleTimer;
		protected GamePlayer m_player;
		private PyramidConfigInfo m_pyramidConfig;
		private PyramidInfo m_pyramid;
		private UserGodcardInfo userGodcardInfo_0;
		private UserGodcardInfo userGodcardInfo_1;
		private DDQiYuanInfo ddqiYuanInfo_1;
		private DDQiYuanInfo ddqiYuanInfo_0;
		private BeautyvoteInfo beautyvoteInfo_0;
		private UserChristmasInfo m_christmas;
		private UserBoguAdventureInfo m_boguAdventure;
		private ActiveSystemInfo m_activeInfo;
		private NewChickenBoxItemInfo[] m_ChickenBoxRewards;
		private List<NewChickenBoxItemInfo> m_RemoveChickenBoxRewards;
		private int m_flushPrice;
		private int[] m_eagleEyePrice;
		private int[] m_openCardPrice;
		private int m_freeFlushTime;
		private int[] m_boguAdventureMoney;
		private ThreadSafeRandom rand = new ThreadSafeRandom();
		private bool m_saveToDb;
		private readonly int defaultCoins;
		private readonly int flushCoins;
		private readonly int ChikenBoxCount;
		private readonly int LuckyStartBoxCount;
		public readonly int coinTemplateID;
		public readonly int countBoguReset;
		private readonly int countBox1GoguAward;
		private readonly int countBox2GoguAward;
		private readonly int countBox3GoguAward;
		private int m_labyrinthCountDown = GameProperties.WarriorFamRaidTimeRemain;
		private int m_freeRefreshBoxCount;
		private int m_freeEyeCount;
		private int m_freeOpenCardCount;
		private DateTime m_luckyBegindate;
		private DateTime m_luckyEnddate;
		private int m_minUseNum;
		private NewChickenBoxItemInfo[] m_LuckyStartRewards;
		private NewChickenBoxItemInfo m_award;
		private int _lightriddleColdown = 15;
		public GamePlayer Player
		{
			get
			{
				return this.m_player;
			}
		}
		public PyramidConfigInfo PyramidConfig
		{
			get
			{
				return this.m_pyramidConfig;
			}
			set
			{
				this.m_pyramidConfig = value;
			}
		}
		public PyramidInfo Pyramid
		{
			get
			{
				return this.m_pyramid;
			}
			set
			{
				this.m_pyramid = value;
			}
		}
		public UserChristmasInfo Christmas
		{
			get
			{
				return this.m_christmas;
			}
			set
			{
				this.m_christmas = value;
			}
		}

		public UserGodcardInfo UserGodcardinfo
		{
			get => this.userGodcardInfo_0;
			set => this.userGodcardInfo_0 = value;
		}

		public UserBoguAdventureInfo BoguAdventure
		{
			get
			{
				return this.m_boguAdventure;
			}
			set
			{
				this.m_boguAdventure = value;
			}
		}
		public ActiveSystemInfo Info
		{
			get
			{
				return this.m_activeInfo;
			}
			set
			{
				this.m_activeInfo = value;
			}
		}
		public NewChickenBoxItemInfo[] ChickenBoxRewards
		{
			get
			{
				return this.m_ChickenBoxRewards;
			}
			set
			{
				this.m_ChickenBoxRewards = value;
			}
		}
		public int flushPrice
		{
			get
			{
				return this.m_flushPrice;
			}
			set
			{
				this.m_flushPrice = value;
			}
		}
		public int[] eagleEyePrice
		{
			get
			{
				return this.m_eagleEyePrice;
			}
			set
			{
				this.m_eagleEyePrice = value;
			}
		}
		public int[] openCardPrice
		{
			get
			{
				return this.m_openCardPrice;
			}
			set
			{
				this.m_openCardPrice = value;
			}
		}
		public int freeFlushTime
		{
			get
			{
				return this.m_freeFlushTime;
			}
			set
			{
				this.m_freeFlushTime = value;
			}
		}
		public int[] BoguAdventureMoney
		{
			get
			{
				return this.m_boguAdventureMoney;
			}
			set
			{
				this.m_boguAdventureMoney = value;
			}
		}
		public int freeRefreshBoxCount
		{
			get
			{
				return this.m_freeRefreshBoxCount;
			}
			set
			{
				this.m_freeRefreshBoxCount = value;
			}
		}

		public BeautyvoteInfo Beautyvote
		{
			get => this.beautyvoteInfo_0;
			set => this.beautyvoteInfo_0 = value;
		}

		public int freeEyeCount
		{
			get
			{
				return this.m_freeEyeCount;
			}
			set
			{
				this.m_freeEyeCount = value;
			}
		}
		public int freeOpenCardCount
		{
			get
			{
				return this.m_freeOpenCardCount;
			}
			set
			{
				this.m_freeOpenCardCount = value;
			}
		}
		public DateTime LuckyBegindate
		{
			get
			{
				return this.m_luckyBegindate;
			}
			set
			{
				this.m_luckyBegindate = value;
			}
		}
		public DateTime LuckyEnddate
		{
			get
			{
				return this.m_luckyEnddate;
			}
			set
			{
				this.m_luckyEnddate = value;
			}
		}
		public int minUseNum
		{
			get
			{
				return this.m_minUseNum;
			}
			set
			{
				this.m_minUseNum = value;
			}
		}
		public NewChickenBoxItemInfo Award
		{
			get
			{
				return this.m_award;
			}
			set
			{
				this.m_award = value;
			}
		}
		public void CreateUserGodcardInfo()
		{
			lock (this.m_lock)
			{
				this.userGodcardInfo_0 = new UserGodcardInfo();
				this.userGodcardInfo_0.ID = 0;
				this.userGodcardInfo_0.UserID = this.Player.PlayerCharacter.ID;
				this.userGodcardInfo_0.score = 0;
				this.userGodcardInfo_0.freeCount = 0;
				this.userGodcardInfo_0.chipCount = 0;
				this.userGodcardInfo_0.ListCard = "";
				this.userGodcardInfo_0.ListAward = "";
				this.userGodcardInfo_0.ListExchange = "";
				this.userGodcardInfo_0.scoreSan = 0;
				this.userGodcardInfo_0.stepRemain = 5;
				this.userGodcardInfo_0.crystalNum = 0;
				this.userGodcardInfo_0.ArraySanXiao = new int[49];
				this.userGodcardInfo_0.RewardsData = "";
				this.userGodcardInfo_0.StoreData = "";
				this.userGodcardInfo_0.MemoryGameCount = 5;
				this.userGodcardInfo_0.MemoryGameScore = 0;
				this.userGodcardInfo_0.MemoryGameRewardList = "";
				this.userGodcardInfo_0.TmpMemoryGame = "0";
				this.CreateMemoryGameInfo();
			}
		}
		public void CreateDDQiYuanInfo()
		{
			lock (this.m_lock)
			{
				this.ddqiYuanInfo_0 = new DDQiYuanInfo();
				this.ddqiYuanInfo_0.ID = 0;
				this.ddqiYuanInfo_0.UserID = this.Player.PlayerCharacter.ID;
				this.ddqiYuanInfo_0.NickName = this.Player.PlayerCharacter.NickName;
				this.ddqiYuanInfo_0.MyOfferTimes = 0;
				this.ddqiYuanInfo_0.HasGetGoodArr = "";
				this.ddqiYuanInfo_0.HasGainTreasureBoxNum = 0;
				this.ddqiYuanInfo_0.HasGainJoinRewardCount = 0;
				this.ddqiYuanInfo_0.TaskReward = "";
				this.ddqiYuanInfo_0.AreaId = this.Player.ZoneId;
				this.ddqiYuanInfo_0.AreaName = this.Player.ZoneName;
			}
		}

		private bool method_0() => DateTime.Now > Convert.ToDateTime(GameProperties.SanXiaoStartTime) && DateTime.Now < Convert.ToDateTime(GameProperties.SanXiaoEndTime);

		public void ResetSanXiao()
		{
			DateTime date1 = DateTime.Now.Date;
			DateTime dateTime = Convert.ToDateTime(GameProperties.SanXiaoEndTime);
			dateTime = dateTime.AddDays(1.0);
			DateTime date2 = dateTime.Date;
			if (date1 > date2)
			{
				this.userGodcardInfo_0.score = 0;
				this.userGodcardInfo_0.freeCount = 0;
				this.userGodcardInfo_0.chipCount = 0;
				this.userGodcardInfo_0.ListCard = "";
				this.userGodcardInfo_0.ListAward = "";
				this.userGodcardInfo_0.ListExchange = "";
				this.userGodcardInfo_0.scoreSan = 0;
				this.userGodcardInfo_0.stepRemain = 5;
				this.userGodcardInfo_0.crystalNum = 0;
				this.userGodcardInfo_0.ArraySanXiao = new int[49];
				this.userGodcardInfo_0.RewardsData = "";
				this.userGodcardInfo_0.StoreData = "";
				this.userGodcardInfo_0.MemoryGameCount = 5;
				this.userGodcardInfo_0.MemoryGameScore = 0;
				this.userGodcardInfo_0.MemoryGameRewardList = "";
				this.userGodcardInfo_0.TmpMemoryGame = "0";
				this.ddqiYuanInfo_0.MyOfferTimes = 0;
				this.ddqiYuanInfo_0.HasGetGoodArr = "";
				this.ddqiYuanInfo_0.HasGainTreasureBoxNum = 0;
				this.ddqiYuanInfo_0.HasGainJoinRewardCount = 0;
				this.ddqiYuanInfo_0.TaskReward = "";
				this.CreateMemoryGameInfo();
			}
			else
			{
				this.userGodcardInfo_0.freeCount = 0;
				this.userGodcardInfo_0.stepRemain = 5;
				this.userGodcardInfo_0.MemoryGameCount = 5;
			}
		}

		public DDQiYuanInfo DdQiYuanInfo
		{
			get => this.ddqiYuanInfo_0;
			set => this.ddqiYuanInfo_0 = value;
		}

		public void CreateMemoryGameInfo()
		{
			ThreadSafeRandom threadSafeRandom = new ThreadSafeRandom();


		}

		public PlayerActives(GamePlayer player, bool saveTodb)
		{
			this.m_lock = new object();
			this.rand = new ThreadSafeRandom();
			this.defaultCoins = 1000;
			this.flushCoins = 15;
			this.ChikenBoxCount = 18;
			this.LuckyStartBoxCount = 14;
			this.coinTemplateID = 201193;
			this.countBox1GoguAward = 20;
			this.countBox2GoguAward = 35;		
			this.countBox3GoguAward = 50;
			this.countBoguReset = 5;
			this.m_labyrinthCountDown = GameProperties.WarriorFamRaidTimeRemain;
			this._lightriddleColdown = 15;
			this.m_player = player;
			this.m_saveToDb = saveTodb;
			this.m_eagleEyePrice = GameProperties.ConvertStringArrayToIntArray("NewChickenEagleEyePrice");
			this.m_openCardPrice = GameProperties.ConvertStringArrayToIntArray("NewChickenOpenCardPrice");
			this.m_boguAdventureMoney = GameProperties.ConvertStringArrayToIntArray("BoguAdventurePrice");
			this.m_flushPrice = GameProperties.NewChickenFlushPrice;
			this.m_freeFlushTime = 120;
			this.m_RemoveChickenBoxRewards = new List<NewChickenBoxItemInfo>();
			this.m_freeEyeCount = 0;
			this.m_freeOpenCardCount = 0;
			this.m_freeRefreshBoxCount = 0;
			this.SetupPyramidConfig();
			this.SetupLuckyStart();
		}
		private void SetupPyramidConfig()
		{
			object @lock;
			Monitor.Enter(@lock = this.m_lock);
			try
			{
				this.m_pyramidConfig = new PyramidConfigInfo();
				this.m_pyramidConfig.isOpen = this.IsPyramidOpen();
				this.m_pyramidConfig.isScoreExchange = !this.IsPyramidOpen();
				this.m_pyramidConfig.beginTime = Convert.ToDateTime(GameProperties.PyramidBeginTime);
				this.m_pyramidConfig.endTime = Convert.ToDateTime(GameProperties.PyramidEndTime);
				this.m_pyramidConfig.freeCount = 3;
				this.m_pyramidConfig.revivePrice = GameProperties.ConvertStringArrayToIntArray("PyramidRevivePrice");
				this.m_pyramidConfig.turnCardPrice = GameProperties.PyramydTurnCardPrice;
			}
			finally
			{
				Monitor.Exit(@lock);
			}
		}
		public virtual void LoadFromDatabase()
		{
			if (this.m_saveToDb)
			{
				using (PlayerBussiness playerBussiness = new PlayerBussiness())
				{
					if (this.IsChristmasOpen())
					{
						this.m_christmas = playerBussiness.GetSingleUserChristmas(this.Player.PlayerCharacter.ID);
						if (this.m_christmas == null)
						{
							this.CreateChristmasInfo(this.Player.PlayerCharacter.ID);
						}
					}
					this.m_activeInfo = playerBussiness.GetSingleActiveSystem(this.Player.PlayerCharacter.ID);
					if (this.m_activeInfo == null)
					{
						this.CreateActiveSystemInfo(this.Player.PlayerCharacter.ID, this.Player.PlayerCharacter.NickName);
					}
					this.m_boguAdventure = playerBussiness.GetSingleBoguAdventure(this.Player.PlayerCharacter.ID);
					if (this.m_boguAdventure == null)
					{
						this.CreateBoguAdventureInfo();
					}
					else
					{
						this.m_boguAdventure.MapData = this.CovertBoguMapToArray(this.m_boguAdventure.Map);
					}
					this.userGodcardInfo_0 = playerBussiness.UserGodcardInfo(this.Player.PlayerCharacter.ID);
					if (this.userGodcardInfo_0 == null)
						this.CreateUserGodcardInfo();
					this.ddqiYuanInfo_0 = playerBussiness.GetDDQiYuanInfoByUserID(this.Player.PlayerCharacter.ID, this.Player.ZoneId);
					if (this.ddqiYuanInfo_0 == null)
						this.CreateDDQiYuanInfo();
					this.beautyvoteInfo_0 = playerBussiness.GetBeautyvoteInfoByUserID(this.Player.PlayerCharacter.ID);
				}
			}
		}

		public bool IsDiceOpen()
		{
			Convert.ToDateTime(GameProperties.DiceBeginTime);
			return DateTime.Now.Date < Convert.ToDateTime(GameProperties.DiceEndTime).Date;
		}
		public bool IsChristmasOpen()
		{
			Convert.ToDateTime(GameProperties.ChristmasBeginDate);
			DateTime dateTime = Convert.ToDateTime(GameProperties.ChristmasEndDate);
			return DateTime.Now.Date < dateTime.Date;
		}
		public bool IsChickenBoxOpen()
		{
			Convert.ToDateTime(GameProperties.NewChickenBeginTime);
			DateTime dateTime = Convert.ToDateTime(GameProperties.NewChickenEndTime);
			return DateTime.Now.Date < dateTime.Date;
		}
		public bool IsPyramidOpen()
		{
			Convert.ToDateTime(GameProperties.PyramidBeginTime);
			DateTime dateTime = Convert.ToDateTime(GameProperties.PyramidEndTime);
			return DateTime.Now.Date < dateTime.Date;
		}
		public bool IsDragonBoatOpen()
		{
			Convert.ToDateTime(GameProperties.DragonBoatBeginDate);
			DateTime dateTime = Convert.ToDateTime(GameProperties.DragonBoatEndDate);
			return DateTime.Now.Date < dateTime.Date;
		}
		public bool IsYearMonsterOpen()
		{
			Convert.ToDateTime(GameProperties.YearMonsterBeginDate);
			DateTime dateTime = Convert.ToDateTime(GameProperties.YearMonsterEndDate);
			return DateTime.Now.Date < dateTime.Date;
		}
		public NewChickenBoxItemInfo GetAward(int pos)
		{
			NewChickenBoxItemInfo[] chickenBoxRewards = this.m_ChickenBoxRewards;
			for (int i = 0; i < chickenBoxRewards.Length; i++)
			{
				NewChickenBoxItemInfo newChickenBoxItemInfo = chickenBoxRewards[i];
				if (newChickenBoxItemInfo.Position == pos && !newChickenBoxItemInfo.IsSelected)
				{
					return newChickenBoxItemInfo;
				}
			}
			return null;
		}

		public bool IsLuckStarActivityOpen()
		{
			DateTime inicio = Convert.ToDateTime(GameProperties.LuckStarActivityBeginDate);
			DateTime time = Convert.ToDateTime(GameProperties.LuckStarActivityEndDate);
			DateTime agora = DateTime.Now;
			if (inicio > time)
			{
				Console.WriteLine("Configuração da Luckstar está incorreta");
				return false;
			}

			else
			{
				if (time > agora)
				{
					if (agora > inicio)
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				else
				{
					return false;
				}

			}
		}

		public NewChickenBoxItemInfo ViewAward(int pos)
		{
			NewChickenBoxItemInfo[] chickenBoxRewards = this.m_ChickenBoxRewards;
			for (int i = 0; i < chickenBoxRewards.Length; i++)
			{
				NewChickenBoxItemInfo newChickenBoxItemInfo = chickenBoxRewards[i];
				if (newChickenBoxItemInfo.Position == pos && !newChickenBoxItemInfo.IsSeeded)
				{
					return newChickenBoxItemInfo;
				}
			}
			return null;
		}
		public bool UpdateChickenBoxAward(NewChickenBoxItemInfo box)
		{
			for (int i = 0; i < this.m_ChickenBoxRewards.Length; i++)
			{
				if (this.m_ChickenBoxRewards[i].Position == box.Position)
				{
					this.m_ChickenBoxRewards[i] = box;
					return true;
				}
			}
			return false;
		}
		public void LoadChickenBox()
		{
			using (PlayerBussiness playerBussiness = new PlayerBussiness())
			{
				this.m_ChickenBoxRewards = playerBussiness.GetSingleNewChickenBox(this.Player.PlayerCharacter.ID);
				if (this.m_ChickenBoxRewards.Length == 0)
				{
					this.PayFlushView();
				}
			}
		}
		public void EnterChickenBox()
		{
			if (this.m_ChickenBoxRewards == null)
			{
				this.LoadChickenBox();
			}
		}
		public void RandomPosition()
		{
			List<int> list = new List<int>();
			for (int i = 0; i < this.m_ChickenBoxRewards.Length; i++)
			{
				list.Add(this.m_ChickenBoxRewards[i].Position);
			}
			this.rand.Shuffer<NewChickenBoxItemInfo>(this.m_ChickenBoxRewards);
			for (int j = 0; j < list.Count; j++)
			{
				this.m_ChickenBoxRewards[j].Position = list[j];
			}
		}
		public NewChickenBoxItemInfo[] CreateChickenBoxAward(int count, eEventType DataId)
		{
			List<NewChickenBoxItemInfo> list = new List<NewChickenBoxItemInfo>();
			Dictionary<int, NewChickenBoxItemInfo> dictionary = new Dictionary<int, NewChickenBoxItemInfo>();
			int num = 0;
			int num2 = 0;
			while (list.Count < count)
			{
				List<NewChickenBoxItemInfo> newChickenBoxAward = EventAwardMgr.GetNewChickenBoxAward(DataId);
				if (newChickenBoxAward.Count > 0)
				{
					NewChickenBoxItemInfo newChickenBoxItemInfo = newChickenBoxAward[0];
					if (!dictionary.Keys.Contains(newChickenBoxItemInfo.TemplateID))
					{
						dictionary.Add(newChickenBoxItemInfo.TemplateID, newChickenBoxItemInfo);
						newChickenBoxItemInfo.Position = num;
						list.Add(newChickenBoxItemInfo);
						num++;
					}
				}
				num2++;
			}
			return list.ToArray();
		}
		public void PayFlushView()
		{
			this.m_activeInfo.lastFlushTime = DateTime.Now;
			this.m_activeInfo.isShowAll = true;
			this.m_activeInfo.canOpenCounts = 5;
			this.m_activeInfo.canEagleEyeCounts = 5;
			this.RemoveChickenBoxRewards();
			this.m_ChickenBoxRewards = this.CreateChickenBoxAward(this.ChikenBoxCount, eEventType.CHICKEN_BOX);
			for (int i = 0; i < this.m_ChickenBoxRewards.Length; i++)
			{
				this.m_ChickenBoxRewards[i].UserID = this.Player.PlayerCharacter.ID;
			}
		}
		public void RemoveChickenBoxRewards()
		{
			for (int i = 0; i < this.m_ChickenBoxRewards.Length; i++)
			{
				NewChickenBoxItemInfo newChickenBoxItemInfo = this.m_ChickenBoxRewards[i];
				if (newChickenBoxItemInfo != null && newChickenBoxItemInfo.ID > 0)
				{
					newChickenBoxItemInfo.Position = -1;
					this.m_RemoveChickenBoxRewards.Add(newChickenBoxItemInfo);
				}
			}
		}
		public bool IsFreeFlushTime()
		{
			DateTime lastFlushTime = this.Info.lastFlushTime;
			DateTime d = lastFlushTime.AddMinutes((double)this.freeFlushTime);
			TimeSpan timeSpan = DateTime.Now - this.Info.lastFlushTime;
			double num = (d - lastFlushTime).TotalMinutes - timeSpan.TotalMinutes;
			return num > 0.0;
		}
		public bool LoadPyramid()
		{
			if (this.m_pyramid == null)
			{
				using (PlayerBussiness playerBussiness = new PlayerBussiness())
				{
					this.m_pyramid = playerBussiness.GetSinglePyramid(this.Player.PlayerCharacter.ID);
					if (this.m_pyramid == null)
					{
						this.CreatePyramidInfo();
					}
				}
			}
			return true;
		}
		public void CreateBoguAdventureInfo()
		{
			object @lock;
			Monitor.Enter(@lock = this.m_lock);
			try
			{
				BoguCeilInfo[] array = this.CreateRandomBoguMap();
				this.m_boguAdventure = new UserBoguAdventureInfo();
				this.m_boguAdventure.UserID = this.Player.PlayerCharacter.ID;
				this.m_boguAdventure.CurrentPostion = 0;
				this.m_boguAdventure.OpenCount = 0;
				this.m_boguAdventure.HP = 2;
				this.m_boguAdventure.ResetCount = this.countBoguReset;
				this.m_boguAdventure.Map = this.CovertBoguMapToString(array);
				this.m_boguAdventure.MapData = array.ToList<BoguCeilInfo>();
				this.m_boguAdventure.Award = "0,0,0";
				this.SaveToDatabase();
			}
			finally
			{
				Monitor.Exit(@lock);
			}
		}
		public void ResetBoguAdventureInfo()
		{
			object @lock;
			Monitor.Enter(@lock = this.m_lock);
			try
			{
				BoguCeilInfo[] array = this.CreateRandomBoguMap();
				this.m_boguAdventure.CurrentPostion = 0;
				this.m_boguAdventure.OpenCount = 0;
				this.m_boguAdventure.HP = 2;
				this.m_boguAdventure.Map = this.CovertBoguMapToString(array);
				this.m_boguAdventure.MapData = array.ToList<BoguCeilInfo>();
			}
			finally
			{
				Monitor.Exit(@lock);
			}
		}
		public int CountOpenCanTakeBoxGoguAdventure(int type)
		{
			switch (type)
			{
				case 0:
					return this.countBox1GoguAward;
				case 1:
					return this.countBox2GoguAward;
				case 2:
					return this.countBox3GoguAward;		
				default:
					return 0;
			}
		}
		public bool UpdateCeilBoguMap(BoguCeilInfo ceilInfo)
		{
			object @lock;
			Monitor.Enter(@lock = this.m_lock);
			bool result;
			try
			{
				bool flag = false;
				if (this.m_boguAdventure.MapData == null)
				{
					this.m_boguAdventure.MapData = this.CovertBoguMapToArray(this.m_boguAdventure.Map);
				}
				BoguCeilInfo boguCeilInfo = this.FindCeilBoguMap(ceilInfo.Index);
				if (boguCeilInfo != null)
				{
					flag = true;
				}
				if (flag)
				{
					this.m_boguAdventure.Map = this.CovertBoguMapToString(this.m_boguAdventure.MapData.ToArray());
				}
				result = flag;
			}
			finally
			{
				Monitor.Exit(@lock);
			}
			return result;
		}
		public GypsyItemDataInfo GetMysteryShopByID(int ID)
		{
			foreach (GypsyItemDataInfo item in MysteryShop)
			{
				if (item.GypsyID == ID)
				{
					return item;
				}
			}

			return null;
		}
		public bool UpdateMysteryShopByID(int ID)
		{
			lock (m_lock)
			{
				for (int i = 0; i < MysteryShop.Length; i++)
				{
					if (m_mysteryShop[i].GypsyID == ID)
					{
						m_mysteryShop[i].CanBuy = 0;
					}
				}
			}

			return true;
		}
		public virtual bool LoadGypsyItemDataFromDatabase()
		{
			if (m_saveToDb)
			{
				using (PlayerBussiness pb = new PlayerBussiness())
				{
					try
					{
						if (m_mysteryShop == null)
						{
							m_mysteryShop = pb.GetAllGypsyItemDataByID(Player.PlayerCharacter.ID);
							if (MysteryShop.Length < 8)
							{
								m_mysteryShop = null;
								RefreshMysteryShop();
							}
						}
					}
					catch
					{
						return false;
					}
				}
			}

			return true;
		}
		public GSPacketIn SendGypsyShopPlayerInfo()
		{
			GSPacketIn pkg = new GSPacketIn((short)ePackageType.MYSTERY_SHOP, Player.PlayerCharacter.ID);
			pkg.WriteByte((byte)GypsyShopPackageType.PLAYER_INFO);
			pkg.WriteInt(Info.CurRefreshedTimes); //_curRefreshedTimes = _loc_2.readInt();
			pkg.WriteInt(MysteryShop.Length); //_itemCount = _loc_2.readInt();
			foreach (GypsyItemDataInfo info in MysteryShop)
			{
				pkg.WriteInt(info.GypsyID); //_loc_3 = _loc_2.readInt();id
				pkg.WriteInt(info.Unit); //_loc_4 = _loc_2.readInt();unit
				pkg.WriteInt(info.Price); //_loc_7 = _loc_2.readInt();price                   
				pkg.WriteInt(info.Num); //_loc_6 = _loc_2.readInt();num
				if (GameProperties.VERSION >= 8200)
				{
					pkg.WriteInt(info.type); //type
				}

				pkg.WriteInt(info.InfoID); //_loc_5 = _loc_2.readInt();infoID
				pkg.WriteInt(info.CanBuy); //_loc_8 = _loc_2.readInt();canBuy 
				pkg.WriteInt(info.Quality); //_loc_9 = _loc_2.readInt();quality
			}

			Player.Out.SendTCP(pkg);
			return pkg;
		}
		private GypsyItemDataInfo[] m_mysteryShop;

		public GypsyItemDataInfo[] MysteryShop
		{
			get { return m_mysteryShop; }
			set { m_mysteryShop = value; }
		}
		public void RefreshMysteryShopByHour()
		{
			DateTime currTime = DateTime.Now;
			if (GameProperties.MysteryShopFreshTime == currTime.Hour && Info.LastRefresh.Date < currTime.Date)
			{
				Info.LastRefresh = currTime;
				RefreshMysteryShop();
			}
		}
		public void RefreshMysteryShop()
		{
			List<MysteryShopInfo> list = GypsyShopMgr.GetMysteryShop();
			bool isNew = false;
			lock (m_lock)
			{
				if (m_mysteryShop == null)
				{
					m_mysteryShop = new GypsyItemDataInfo[8];
					isNew = true;
				}

				int index = 0;
				foreach (MysteryShopInfo item in list)
				{
					if (index < MysteryShop.Length)
					{
						if (isNew)
						{
							GypsyItemDataInfo info = new GypsyItemDataInfo();
							info.UserID = m_player.PlayerId;
							info.GypsyID = item.ID;
							info.InfoID = item.InfoID;
							info.Unit = item.Unit;
							info.Num = item.Num;
							info.Price = item.Price;
							info.CanBuy = item.CanBuy;
							info.Quality = item.Quality;
							m_mysteryShop[index] = info;
						}
						else
						{
							m_mysteryShop[index].GypsyID = item.ID;
							m_mysteryShop[index].InfoID = item.InfoID;
							m_mysteryShop[index].Unit = item.Unit;
							m_mysteryShop[index].Num = item.Num;
							m_mysteryShop[index].Price = item.Price;
							m_mysteryShop[index].CanBuy = item.CanBuy;
							m_mysteryShop[index].Quality = item.Quality;
						}
					}

					index++;
				}
			}
		}
		public GSPacketIn SendGypsyShopOpenClose(bool open)
		{
			GSPacketIn pkg = new GSPacketIn((short)ePackageType.MYSTERY_SHOP, m_player.PlayerId);
			pkg.WriteByte((byte)GypsyShopPackageType.OPEN_INFO);
			pkg.WriteBoolean(open);
			m_player.SendTCP(pkg);
			return pkg;
		}

		public void ResetMysteryShop()
		{
			if (m_activeInfo == null)
				return;

			lock (m_lock)
			{
				m_activeInfo.CurRefreshedTimes = 0;
			}
		}
		public BoguCeilInfo FindCeilBoguMap(int index)
		{
			object @lock;
			Monitor.Enter(@lock = this.m_lock);
			BoguCeilInfo result;
			try
			{
				if (this.m_boguAdventure.MapData == null)
				{
					this.m_boguAdventure.MapData = this.CovertBoguMapToArray(this.m_boguAdventure.Map);
				}
				foreach (BoguCeilInfo current in this.m_boguAdventure.MapData)
				{
					if (current.Index == index)
					{
						result = current;
						return result;
					}
				}
				result = null;
			}
			finally
			{
				Monitor.Exit(@lock);
			}
			return result;
		}
		public BoguCeilInfo[] GetTotalMineAround(int index)
		{
			object @lock;
			Monitor.Enter(@lock = this.m_lock);
			BoguCeilInfo[] result;
			try
			{
				List<BoguCeilInfo> list = new List<BoguCeilInfo>();
				int[] countAroundIndex = this.GetCountAroundIndex(index);
				int[] array = countAroundIndex;
				for (int i = 0; i < array.Length; i++)
				{
					int index2 = array[i];
					BoguCeilInfo boguCeilInfo = this.FindCeilBoguMap(index2);
					if (boguCeilInfo != null && boguCeilInfo.Result == -1)
					{
						list.Add(boguCeilInfo);
					}
				}
				result = list.ToArray();
			}
			finally
			{
				Monitor.Exit(@lock);
			}
			return result;
		}
		public BoguCeilInfo[] GetTotalMineAroundNotOpen(int index)
		{
			object @lock;
			Monitor.Enter(@lock = this.m_lock);
			BoguCeilInfo[] result;
			try
			{
				List<BoguCeilInfo> list = new List<BoguCeilInfo>();
				int[] countAroundIndex = this.GetCountAroundIndex(index);
				int[] array = countAroundIndex;
				for (int i = 0; i < array.Length; i++)
				{
					int index2 = array[i];
					BoguCeilInfo boguCeilInfo = this.FindCeilBoguMap(index2);
					if (boguCeilInfo != null && boguCeilInfo.Result == -1 && boguCeilInfo.State == 3)
					{
						list.Add(boguCeilInfo);
					}
				}
				result = list.ToArray();
			}
			finally
			{
				Monitor.Exit(@lock);
			}
			return result;
		}
		public int[] GetCountAroundIndex(int index)
		{
			List<int> list = new List<int>();
			int[] source = new int[]
			{
				1,
				11,
				21,
				31,
				41,
				51,
				61
			};
			int[] source2 = new int[]
			{
				10,
				20,
				30,
				40,
				50,
				60,
				70
			};
			if (index > 0 && index <= 70)
			{
				int num = index - 1;
				if (num >= 1 && num <= 70)
				{
					list.Add(num);
				}
				int num2 = index - 9;
				if (num2 >= 1 && num2 <= 70)
				{
					list.Add(num2);
				}
				int num3 = index - 10;
				if (num3 >= 1 && num3 <= 70)
				{
					list.Add(num3);
				}
				int num4 = index - 11;
				if (num4 >= 1 && num4 <= 70)
				{
					list.Add(num4);
				}
				int num5 = index + 1;
				if (num5 >= 1 && num5 <= 70)
				{
					list.Add(num5);
				}
				int num6 = index + 9;
				if (num6 >= 1 && num6 <= 70)
				{
					list.Add(num6);
				}
				int num7 = index + 10;
				if (num7 >= 1 && num7 <= 70)
				{
					list.Add(num7);
				}
				int num8 = index + 11;
				if (num8 >= 1 && num8 <= 70)
				{
					list.Add(num8);
				}
				if (source.Contains(index))
				{
					list.Remove(num);
					list.Remove(num6);
					list.Remove(num4);
				}
				if (source2.Contains(index))
				{
					list.Remove(num5);
					list.Remove(num2);
					list.Remove(num8);
				}
			}
			return list.ToArray();
		}
		public List<BoguCeilInfo> CovertBoguMapToArray(string boguMap)
		{
			List<BoguCeilInfo> list = new List<BoguCeilInfo>();
			string[] array = boguMap.Split(new char[]
			{
				'|'
			});
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i];
				string[] array3 = text.Split(new char[]
				{
					','
				});
				list.Add(new BoguCeilInfo
				{
					Index = int.Parse(array3[0]),
					State = int.Parse(array3[1]),
					Result = int.Parse(array3[2])
				});
			}
			return list;
		}
		public string CovertBoguMapToString(BoguCeilInfo[] boguMap)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < boguMap.Length; i++)
			{
				BoguCeilInfo boguCeilInfo = boguMap[i];
				string item = string.Concat(new object[]
				{
					boguCeilInfo.Index,
					",",
					boguCeilInfo.State,
					",",
					boguCeilInfo.Result
				});
				list.Add(item);
			}
			return string.Join("|", list.ToArray());
		}
		private BoguCeilInfo[] CreateRandomBoguMap()
		{
			BoguCeilInfo boguCeilInfo = new BoguCeilInfo();
			BoguCeilInfo[] array = new BoguCeilInfo[70];
			int[] source = this.RandomMine();
			for (int i = 0; i < 70; i++)
			{
				array[i] = new BoguCeilInfo
				{
					Index = i + 1,
					State = 3,
					Result = source.Contains(i + 1) ? -1 : -2,
					AroundCount = 0
				};
			}
			return array;
		}
		private int[] RandomMine()
		{
			List<int> list = new List<int>();
			int i = 20;
			List<int> list2 = new List<int>();
			for (int j = 1; j <= 70; j++)
			{
				list2.Add(j);
			}
			while (i > 0)
			{
				int index = this.rand.Next(0, list2.Count - 1);
				list.Add(list2[index]);
				list2.RemoveAt(index);
				i--;
			}
			return list.ToArray();
		}
		private int[] RandomMine2()
		{
			List<int> list = new List<int>();
			List<int> list2 = new List<int>
			{
				1,
				2,
				3,
				4,
				5,
				6,
				7,
				8,
				9,
				10
			};
			List<int> list3 = new List<int>
			{
				11,
				12,
				13,
				14,
				15,
				16,
				17,
				18,
				19,
				20
			};
			List<int> list4 = new List<int>
			{
				21,
				22,
				23,
				24,
				25,
				26,
				27,
				28,
				29,
				30
			};
			List<int> list5 = new List<int>
			{
				31,
				32,
				33,
				34,
				35,
				36,
				37,
				38,
				39,
				40
			};
			List<int> list6 = new List<int>
			{
				41,
				42,
				43,
				44,
				45,
				46,
				47,
				48,
				49,
				50
			};
			List<int> list7 = new List<int>
			{
				51,
				52,
				53,
				54,
				55,
				56,
				57,
				58,
				59,
				60
			};
			List<int> list8 = new List<int>
			{
				61,
				62,
				63,
				64,
				65,
				66,
				67,
				68,
				69,
				70
			};
			int i = this.rand.Next(2, 4);
			int j = this.rand.Next(2, 4);
			int k = this.rand.Next(2, 4);
			int l = this.rand.Next(2, 4);
			int m = this.rand.Next(2, 4);
			int n = this.rand.Next(2, 4);
			int num = this.rand.Next(2, 4);
			for (int num2 = 0; num2 < 7; num2++)
			{
				switch (num2)
				{
					case 0:
						while (i > 0)
						{
							int index = this.rand.Next(0, list2.Count - 1);
							list.Add(list2[index]);
							list2.RemoveAt(index);
							i--;
						}
						break;
					case 1:
						while (j > 0)
						{
							int index = this.rand.Next(0, list3.Count - 1);
							list.Add(list3[index]);
							list3.RemoveAt(index);
							j--;
						}
						break;
					case 2:
						while (k > 0)
						{
							int index = this.rand.Next(0, list4.Count - 1);
							list.Add(list4[index]);
							list4.RemoveAt(index);
							k--;
						}
						break;
					case 3:
						while (l > 0)
						{
							int index = this.rand.Next(0, list5.Count - 1);
							list.Add(list5[index]);
							list5.RemoveAt(index);
							l--;
						}
						break;
					case 4:
						while (m > 0)
						{
							int index = this.rand.Next(0, list6.Count - 1);
							list.Add(list6[index]);
							list6.RemoveAt(index);
							m--;
						}
						break;
					case 5:
						while (n > 0)
						{
							int index = this.rand.Next(0, list7.Count - 1);
							list.Add(list7[index]);
							list7.RemoveAt(index);
							n--;
						}
						break;
					case 6:
						while (num > 0)
						{
							int index = this.rand.Next(0, list8.Count - 1);
							list.Add(list8[index]);
							list8.RemoveAt(index);
							num--;
						}
						break;
				}
			}
			return list.ToArray();
		}
		public void CreatePyramidInfo()
		{
			object @lock;
			Monitor.Enter(@lock = this.m_lock);
			try
			{
				this.m_pyramid = new PyramidInfo();
				this.m_pyramid.ID = 0;
				this.m_pyramid.UserID = this.Player.PlayerCharacter.ID;
				this.m_pyramid.currentLayer = 1;
				this.m_pyramid.maxLayer = 1;
				this.m_pyramid.totalPoint = 0;
				this.m_pyramid.turnPoint = 0;
				this.m_pyramid.pointRatio = 0;
				this.m_pyramid.currentFreeCount = 0;
				this.m_pyramid.currentReviveCount = 0;
				this.m_pyramid.isPyramidStart = false;
				this.m_pyramid.LayerItems = "";
			}
			finally
			{
				Monitor.Exit(@lock);
			}
		}
		public void ResetChristmas()
		{
			object @lock;
			Monitor.Enter(@lock = this.m_lock);
			try
			{
				if (this.m_christmas != null)
				{
					this.m_christmas.dayPacks = 0;
					this.m_christmas.AvailTime = 0;
					this.m_christmas.isEnter = false;
					this.m_activeInfo.dayScore = 0;
				}
			}
			finally
			{
				Monitor.Exit(@lock);
			}
		}
		public void ResetDragonBoat()
		{
			object @lock;
			Monitor.Enter(@lock = this.m_lock);
			try
			{
				this.m_activeInfo.useableScore = 0;
				this.m_activeInfo.totalScore = 0;
				this.m_activeInfo.dayScore = 0;
				this.m_activeInfo.CanGetGift = true;
			}
			finally
			{
				Monitor.Exit(@lock);
			}
		}
		public void SendDragonBoatAward()
		{
			if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday && this.IsDragonBoatOpen())
			{
				int dragonBoatMinScore = GameProperties.DragonBoatMinScore;
				int dragonBoatAreaMinScore = GameProperties.DragonBoatAreaMinScore;
				List<ActiveSystemInfo> list = ActiveSystemMgr.SelectTopTenCurrenServer(dragonBoatMinScore);
				int num = 0;
				List<ItemInfo> infos = new List<ItemInfo>();
				string text = "Phần thưởng Thuyền rồng hạng {0}";
				foreach (ActiveSystemInfo current in list)
				{
					if (current.UserID == this.Player.PlayerCharacter.ID && this.m_activeInfo.CanGetGift)
					{
						int myRank = current.myRank;
						if (myRank <= 10)
						{
							text = string.Format(text, myRank);
							infos = CommunalActiveMgr.GetAwardInfos(1, myRank);
							WorldEventMgr.SendItemsToMail(infos, current.UserID, this.Player.PlayerCharacter.NickName, text);
							num++;
							break;
						}
						break;
					}
				}
				list = ActiveSystemMgr.SelectTopTenAllServer(dragonBoatAreaMinScore);
				foreach (ActiveSystemInfo current2 in list)
				{
					if (current2.UserID == this.Player.PlayerCharacter.ID && this.m_activeInfo.CanGetGift)
					{
						int myRank = current2.myRank;
						if (myRank <= 10)
						{
							text = string.Format(text, myRank);
							text += " liên server";
							infos = CommunalActiveMgr.GetAwardInfos(2, myRank);
							WorldEventMgr.SendItemsToMail(infos, current2.UserID, this.Player.PlayerCharacter.NickName, text);
							num++;
							break;
						}
						break;
					}
				}
				if (num > 0)
				{
					this.m_activeInfo.CanGetGift = false;
				}
			}
		}
		public void BeginChristmasTimer()
		{
			int num = 60000;
			if (this._christmasTimer == null)
			{
				this._christmasTimer = new Timer(new TimerCallback(this.ChristmasTimeCheck), null, num, num);
				return;
			}
			this._christmasTimer.Change(num, num);
		}
		protected void ChristmasTimeCheck(object sender)
		{
			try
			{
				int num = Environment.TickCount;
				ThreadPriority priority = Thread.CurrentThread.Priority;
				Thread.CurrentThread.Priority = ThreadPriority.Lowest;
				this.UpdateChristmasTime();
				Thread.CurrentThread.Priority = priority;
				num = Environment.TickCount - num;
			}
			catch (Exception arg)
			{
				Console.WriteLine("ChristmasTimeCheck: " + arg);
			}
		}
		public void StopChristmasTimer()
		{
			if (this._christmasTimer != null)
			{
				this._christmasTimer.Dispose();
				this._christmasTimer = null;
			}
		}
		public void UpdateChristmasTime()
		{
			DateTime gameBeginTime = this.Christmas.gameBeginTime;
			DateTime gameEndTime = this.Christmas.gameEndTime;
			TimeSpan timeSpan = DateTime.Now - gameBeginTime;
			double num = (gameEndTime - gameBeginTime).TotalMinutes - timeSpan.TotalMinutes;
			UserChristmasInfo christmas;
			Monitor.Enter(christmas = this.m_christmas);
			try
			{
				this.m_christmas.AvailTime = (((int)num < 0) ? 0 : ((int)num));
			}
			finally
			{
				Monitor.Exit(christmas);
			}
		}
		public void AddTime(int min)
		{
			UserChristmasInfo christmas;
			Monitor.Enter(christmas = this.m_christmas);
			try
			{
				this.m_christmas.AvailTime += min;
			}
			finally
			{
				Monitor.Exit(christmas);
			}
		}
		private void BeginLabyrinthTimer()
		{
			int num = 1000;
			if (this._labyrinthTimer == null)
			{
				this._labyrinthTimer = new Timer(new TimerCallback(this.LabyrinthCheck), null, num, num);
				return;
			}
			this._labyrinthTimer.Change(num, num);
		}
		protected void LabyrinthCheck(object sender)
		{
			try
			{
				int num = Environment.TickCount;
				ThreadPriority priority = Thread.CurrentThread.Priority;
				Thread.CurrentThread.Priority = ThreadPriority.Lowest;
				this.UpdateLabyrinthTime();
				Thread.CurrentThread.Priority = priority;
				num = Environment.TickCount - num;
			}
			catch (Exception arg)
			{
				Console.WriteLine("LabyrinthCheck: " + arg);
			}
		}
		public void StopLabyrinthTimer()
		{
			if (this._labyrinthTimer != null)
			{
				this._labyrinthTimer.Dispose();
				this._labyrinthTimer = null;
			}
		}
		public void UpdateLabyrinthTime()
		{
			UserLabyrinthInfo labyrinth = this.Player.Labyrinth;
			labyrinth.isCleanOut = true;
			labyrinth.isInGame = true;
			if (labyrinth.remainTime > 0 && labyrinth.currentRemainTime > 0)
			{
				labyrinth.remainTime--;
				labyrinth.currentRemainTime--;
				this.m_labyrinthCountDown--;
			}
			if (this.m_labyrinthCountDown == 0)
			{
				this.GetLabyrinthAward();
				this.m_labyrinthCountDown = 120;
				labyrinth.currentFloor++;
				if (labyrinth.currentFloor > labyrinth.myProgress)
				{
					labyrinth.currentFloor = labyrinth.myProgress;
					this.StopLabyrinthTimer();
				}
			}
			this.Player.Out.SendLabyrinthUpdataInfo(this.Player.PlayerId, labyrinth);
		}
		public void CleantOutLabyrinth()
		{
			this.BeginLabyrinthTimer();
		}
		private void GetLabyrinthAward()
		{
			int num = this.m_player.Labyrinth.currentFloor;
			num--;
			int[] array = this.m_player.CreateExps();
			int num2 = array[num];
			string text = this.m_player.labyrinthGolds[num];
			int num3 = int.Parse(text.Split(new char[]
			{
				'|'
			})[0]);
			int num4 = int.Parse(text.Split(new char[]
			{
				'|'
			})[1]);
			ItemInfo itemByTemplateID = this.m_player.PropBag.GetItemByTemplateID(0, 11916);
			if (itemByTemplateID == null || !this.m_player.RemoveTemplate(11916, 1))
			{
				this.m_player.Labyrinth.isDoubleAward = false;
			}
			if (this.m_player.Labyrinth.isDoubleAward)
			{
				int num5 = 2;
				num2 *= num5;
				num3 *= num5;
				num4 *= num5;
			}
			this.m_player.Labyrinth.accumulateExp += num2;
			List<ItemInfo> list = new List<ItemInfo>();
			if (this.CanGetBigAward())
			{
				list = this.m_player.CopyDrop(2, 40002);
				this.m_player.AddTemplate(list, num3, eGameView.dungeonTypeGet);
				this.m_player.AddHardCurrency(num4);
			}
			this.m_player.AddGP(num2);
			this.PlusCleantOutInfo(this.m_player.Labyrinth.currentFloor, num2, num4, list);
		}
		private bool CanGetBigAward()
		{
			bool result = false;
			for (int i = 0; i <= this.m_player.Labyrinth.myProgress; i += 2)
			{
				if (i == this.m_player.Labyrinth.currentFloor)
				{
					result = true;
					break;
				}
			}
			return result;
		}
		private void PlusCleantOutInfo(int FamRaidLevel, int exp, int HardCurrency, List<ItemInfo> lists)
		{
			GSPacketIn gSPacketIn = new GSPacketIn(131, this.m_player.PlayerId);
			gSPacketIn.WriteByte(7);
			gSPacketIn.WriteInt(FamRaidLevel);
			gSPacketIn.WriteInt(exp);
			gSPacketIn.WriteInt(lists.Count);
			foreach (ItemInfo current in lists)
			{
				gSPacketIn.WriteInt(current.TemplateID);
				gSPacketIn.WriteInt(current.Count);
			}
			gSPacketIn.WriteInt(HardCurrency);
			this.m_player.SendTCP(gSPacketIn);
		}
		public void StopCleantOutLabyrinth()
		{
			UserLabyrinthInfo labyrinth = this.Player.Labyrinth;
			labyrinth.isCleanOut = false;
			this.Player.Out.SendLabyrinthUpdataInfo(this.Player.PlayerId, labyrinth);
			this.StopLabyrinthTimer();
		}
		public void SpeededUpCleantOutLabyrinth()
		{
			UserLabyrinthInfo labyrinth = this.Player.Labyrinth;
			labyrinth.isCleanOut = false;
			labyrinth.isInGame = false;
			labyrinth.completeChallenge = false;
			labyrinth.remainTime = 0;
			labyrinth.currentRemainTime = 0;
			labyrinth.cleanOutAllTime = 0;
			for (int i = labyrinth.currentFloor; i <= labyrinth.myProgress; i++)
			{
				this.GetLabyrinthAward();
				labyrinth.currentFloor++;
			}
			labyrinth.currentFloor = labyrinth.myProgress;
			this.Player.Out.SendLabyrinthUpdataInfo(this.Player.PlayerId, labyrinth);
			this.StopLabyrinthTimer();
		}
		public bool AvailTime()
		{
			DateTime gameBeginTime = this.Christmas.gameBeginTime;
			DateTime gameEndTime = this.Christmas.gameEndTime;
			TimeSpan timeSpan = DateTime.Now - gameBeginTime;
			double num = (gameEndTime - gameBeginTime).TotalMinutes - timeSpan.TotalMinutes;
			return num > 0.0;
		}
		public void CreateActiveSystemInfo(int UserID, string name)
		{
			lock (this.m_lock)
			{
				this.m_activeInfo = new ActiveSystemInfo();
				this.m_activeInfo.ID = 0;
				this.m_activeInfo.UserID = UserID;
				this.m_activeInfo.useableScore = 0;
				this.m_activeInfo.totalScore = 0;
				this.m_activeInfo.AvailTime = 0;
				this.m_activeInfo.NickName = name;
				this.m_activeInfo.dayScore = 0;
				this.m_activeInfo.CanGetGift = true;
				this.m_activeInfo.canOpenCounts = 5;
				this.m_activeInfo.canEagleEyeCounts = 5;
				this.m_activeInfo.lastFlushTime = DateTime.Now;
				this.m_activeInfo.isShowAll = true;
				this.m_activeInfo.ActiveMoney = 0;
				this.m_activeInfo.activityTanabataNum = 0;
				this.m_activeInfo.LuckystarCoins = this.defaultCoins;
				this.m_activeInfo.ChallengeNum = GameProperties.YearMonsterFightNum;
				this.m_activeInfo.BuyBuffNum = GameProperties.YearMonsterFightNum;
				this.m_activeInfo.lastEnterYearMonter = DateTime.Now;
				this.m_activeInfo.DamageNum = 0;
				this.m_activeInfo.PuzzleAwardGet = "0,0,0";
				this.CreateYearMonterBoxState();
			}
		}
		public void YearMonterValidate()
		{
			object @lock;
			Monitor.Enter(@lock = this.m_lock);
			try
			{
				if (this.m_activeInfo.lastEnterYearMonter.Date < DateTime.Now.Date)
				{
					this.m_activeInfo.ChallengeNum = GameProperties.YearMonsterFightNum;
					this.m_activeInfo.BuyBuffNum = GameProperties.YearMonsterFightNum;
					this.m_activeInfo.lastEnterYearMonter = DateTime.Now;
					this.m_activeInfo.DamageNum = 0;
					this.CreateYearMonterBoxState();
				}
			}
			finally
			{
				Monitor.Exit(@lock);
			}
		}
		public void CreateYearMonterBoxState()
		{
			string[] array = GameProperties.YearMonsterBoxInfo.Split(new char[]
			{
				'|'
			});
			int num = array.Length;
			string[] array2 = new string[num];
			for (int i = 0; i < num; i++)
			{
				int num2 = int.Parse(array[i].Split(new char[]
				{
					','
				})[1]) * 10000;
				if (num2 <= this.m_activeInfo.DamageNum)
				{
					array2[i] = "2";
				}
				else
				{
					array2[i] = "1";
				}
			}
			this.m_activeInfo.BoxState = string.Join("-", array2);
		}
		public void SetYearMonterBoxState(int id)
		{
			string[] array = this.m_activeInfo.BoxState.Split(new char[]
			{
				'-'
			});
			int num = array.Length;
			string[] array2 = new string[num];
			for (int i = 0; i < num; i++)
			{
				if (i == id)
				{
					array2[i] = "3";
				}
				else
				{
					array2[i] = array[i];
				}
			}
			this.m_activeInfo.BoxState = string.Join("-", array2);
		}
		public void CreateChristmasInfo(int UserID)
		{
			object @lock;
			Monitor.Enter(@lock = this.m_lock);
			try
			{
				this.m_christmas = new UserChristmasInfo();
				this.m_christmas.ID = 0;
				this.m_christmas.UserID = UserID;
				this.m_christmas.count = 0;
				this.m_christmas.exp = 0;
				this.m_christmas.awardState = 0;
				this.m_christmas.lastPacks = 1100;
				this.m_christmas.packsNumber = -1;
				this.m_christmas.gameBeginTime = DateTime.Now;
				this.m_christmas.gameEndTime = DateTime.Now.AddMinutes(60.0);
				this.m_christmas.isEnter = false;
				this.m_christmas.dayPacks = 0;
				this.m_christmas.AvailTime = 0;
			}
			finally
			{
				Monitor.Exit(@lock);
			}
		}
		public virtual void SaveToDatabase()
		{
			if (this.m_saveToDb)
			{
				using (PlayerBussiness playerBussiness = new PlayerBussiness())
				{
					object @lock;
					Monitor.Enter(@lock = this.m_lock);
					try
					{
						if (this.m_pyramid != null && this.m_pyramid.IsDirty)
						{
							if (this.m_pyramid.ID > 0)
							{
								playerBussiness.UpdatePyramid(this.m_pyramid);
							}
							else
							{
								playerBussiness.AddPyramid(this.m_pyramid);
							}
						}
						if (this.m_boguAdventure != null && this.m_boguAdventure.IsDirty)
						{
							playerBussiness.UpdateBoguAdventure(this.m_boguAdventure);
						}
						if (this.m_christmas != null && this.m_christmas.IsDirty)
						{
							if (this.m_christmas.ID > 0)
							{
								playerBussiness.UpdateUserChristmas(this.m_christmas);
							}
							else
							{
								playerBussiness.AddUserChristmas(this.m_christmas);
							}
						}
						if (this.m_activeInfo != null && this.m_activeInfo.IsDirty)
						{
							if (this.m_activeInfo.ID > 0)
							{
								playerBussiness.UpdateActiveSystem(this.m_activeInfo);
							}
							else
							{
								playerBussiness.AddActiveSystem(this.m_activeInfo);
							}
						}
						if (this.userGodcardInfo_1 != null)
						{
							if (this.userGodcardInfo_1.ID > 0)
								playerBussiness.UpdateUserGodcardInfo(this.userGodcardInfo_1);
							else
								playerBussiness.AddUserGodcardInfo(this.userGodcardInfo_1);
						}
						if (this.ddqiYuanInfo_1 != null)
						{
							if (this.ddqiYuanInfo_1.ID > 0)
								playerBussiness.UpdateDDQiYuanInfo(this.ddqiYuanInfo_1);
							else
								playerBussiness.AddDDQiYuanInfo(this.ddqiYuanInfo_1);
						}
						if (this.m_ChickenBoxRewards != null)
						{
							NewChickenBoxItemInfo[] chickenBoxRewards = this.m_ChickenBoxRewards;
							for (int i = 0; i < chickenBoxRewards.Length; i++)
							{
								NewChickenBoxItemInfo newChickenBoxItemInfo = chickenBoxRewards[i];
								if (newChickenBoxItemInfo != null && newChickenBoxItemInfo.IsDirty)
								{
									if (newChickenBoxItemInfo.ID > 0)
									{
										playerBussiness.UpdateNewChickenBox(newChickenBoxItemInfo);
									}
									else
									{
										playerBussiness.AddNewChickenBox(newChickenBoxItemInfo);
									}
								}
							}
						}
						if (this.m_RemoveChickenBoxRewards.Count > 0)
						{
							foreach (NewChickenBoxItemInfo current in this.m_RemoveChickenBoxRewards)
							{
								playerBussiness.UpdateNewChickenBox(current);
							}
						}
					}
					finally
					{
						Monitor.Exit(@lock);
					}
				}
			}
			LanternriddlesInfo lanternriddles = ActiveSystemMgr.GetLanternriddles(this.m_player.PlayerCharacter.ID);
			GameServer.Instance.LoginServer.SendLightriddleInfo(lanternriddles);
		}
		public void SendChickenBoxItemList()
		{
			GSPacketIn gSPacketIn = new GSPacketIn(87);
			gSPacketIn.WriteInt(3);
			gSPacketIn.WriteDateTime(this.Info.lastFlushTime);
			gSPacketIn.WriteInt(this.freeFlushTime);
			gSPacketIn.WriteInt(this.freeRefreshBoxCount);
			gSPacketIn.WriteInt(this.freeEyeCount);
			gSPacketIn.WriteInt(this.freeOpenCardCount);
			gSPacketIn.WriteBoolean(this.Info.isShowAll);
			gSPacketIn.WriteInt(this.ChickenBoxRewards.Length);
			NewChickenBoxItemInfo[] chickenBoxRewards = this.ChickenBoxRewards;
			for (int i = 0; i < chickenBoxRewards.Length; i++)
			{
				NewChickenBoxItemInfo newChickenBoxItemInfo = chickenBoxRewards[i];
				gSPacketIn.WriteInt(newChickenBoxItemInfo.TemplateID);
				gSPacketIn.WriteInt(newChickenBoxItemInfo.StrengthenLevel);
				gSPacketIn.WriteInt(newChickenBoxItemInfo.Count);
				gSPacketIn.WriteInt(newChickenBoxItemInfo.ValidDate);
				gSPacketIn.WriteInt(newChickenBoxItemInfo.AttackCompose);
				gSPacketIn.WriteInt(newChickenBoxItemInfo.DefendCompose);
				gSPacketIn.WriteInt(newChickenBoxItemInfo.AgilityCompose);
				gSPacketIn.WriteInt(newChickenBoxItemInfo.LuckCompose);
				gSPacketIn.WriteInt(newChickenBoxItemInfo.Position);
				gSPacketIn.WriteBoolean(newChickenBoxItemInfo.IsSelected);
				gSPacketIn.WriteBoolean(newChickenBoxItemInfo.IsSeeded);
				gSPacketIn.WriteBoolean(newChickenBoxItemInfo.IsBinds);
			}
			this.m_player.SendTCP(gSPacketIn);
		}
		private bool method_9() => DateTime.Now > Convert.ToDateTime(GameProperties.SanXiaoStartTime) && DateTime.Now < Convert.ToDateTime(GameProperties.SanXiaoEndTime).AddDays(1.0);

		public void SendGodCardRaiseOpenClose(bool value)
		{
			GSPacketIn pkg = new GSPacketIn((short)329);
			pkg.WriteByte((byte)32);
			pkg.WriteBoolean(value);
			pkg.WriteDateTime(Convert.ToDateTime(GameProperties.SanXiaoEndTime));
			this.m_player.SendTCP(pkg);
		}

		public void SendSanXiaoOpenClose(bool value)
		{
			GSPacketIn pkg = new GSPacketIn((short)329);
			pkg.WriteByte((byte)6);
			pkg.WriteBoolean(value);
			pkg.WriteDateTime(Convert.ToDateTime(GameProperties.SanXiaoEndTime));
			pkg.WriteInt(0);
			this.m_player.SendTCP(pkg);
		}

		public void SendMemoryGameOpenClose(bool value)
		{
			GSPacketIn pkg = new GSPacketIn((short)329);
			pkg.WriteByte((byte)10);
			pkg.WriteBoolean(value);
			pkg.WriteDateTime(Convert.ToDateTime(GameProperties.SanXiaoEndTime));
			this.m_player.SendTCP(pkg);
		}

		public void SendDDQiYuanOpenClose(bool value)
		{
			GSPacketIn pkg = new GSPacketIn((short)329);
			pkg.WriteByte((byte)40);
			pkg.WriteBoolean(value);
			pkg.WriteDateTime(Convert.ToDateTime(GameProperties.SanXiaoEndTime));
			this.m_player.SendTCP(pkg);
		}

		public void SendEvent()
		{
			if (this.IsChickenBoxOpen())
			{
				this.m_player.Out.SendChickenBoxOpen(this.m_player.PlayerId, this.flushPrice, this.openCardPrice, this.eagleEyePrice);
			}
			if (this.IsLuckStarActivityOpen())
			{
				this.m_player.Out.SendLuckStarOpen(this.m_player.PlayerId);
			}
			if (this.method_10())
			{
				this.SendGodCardRaiseOpenClose(true);
				this.SendSanXiaoOpenClose(true);
				this.SendMemoryGameOpenClose(true);
			}
			else
			{
				this.SendGodCardRaiseOpenClose(false);
				this.SendSanXiaoOpenClose(false);
				this.SendMemoryGameOpenClose(false);
			}
			if (this.wOtuhQjLoi3())
				this.SendDDQiYuanOpenClose(true);
			else
				this.SendDDQiYuanOpenClose(false);
			this.SendGypsyShopOpenClose(true);
			this.RefreshMysteryShopByHour();
		}

		private bool method_10()
		{
			DateTime dateTime1 = DateTime.Now;
			DateTime date1 = dateTime1.Date;
			dateTime1 = Convert.ToDateTime(GameProperties.SanXiaoStartTime);
			DateTime date2 = dateTime1.Date;
			if (!(date1 >= date2))
				return false;
			DateTime dateTime2 = DateTime.Now;
			DateTime date3 = dateTime2.Date;
			dateTime2 = Convert.ToDateTime(GameProperties.SanXiaoEndTime);
			DateTime date4 = dateTime2.Date;
			return date3 < date4;
		}

		private bool wOtuhQjLoi3()
		{
			DateTime dateTime1 = DateTime.Now;
			DateTime date1 = dateTime1.Date;
			dateTime1 = Convert.ToDateTime(GameProperties.SanXiaoStartTime);
			DateTime date2 = dateTime1.Date;
			if (!(date1 >= date2))
				return false;
			DateTime dateTime2 = DateTime.Now;
			DateTime date3 = dateTime2.Date;
			dateTime2 = Convert.ToDateTime(GameProperties.SanXiaoEndTime);
			DateTime date4 = dateTime2.Date;
			return date3 < date4;
		}

		public GSPacketIn SendLuckStarClose()
		{
			GSPacketIn gSPacketIn = new GSPacketIn(87, this.m_player.PlayerId);
			gSPacketIn.WriteInt(26);
			this.m_player.SendTCP(gSPacketIn);
			return gSPacketIn;
		}
		private void SetupLuckyStart()
		{
			this.m_luckyBegindate = DateTime.Parse(GameProperties.LuckStarActivityBeginDate);
			this.m_luckyEnddate = DateTime.Parse(GameProperties.LuckStarActivityEndDate);
			this.m_minUseNum = GameProperties.MinUseNum;
		}
		public void CreateLuckyStartAward()
		{
			this.m_LuckyStartRewards = this.CreateChickenBoxAward(this.LuckyStartBoxCount, eEventType.LUCKY_STAR);
			NewChickenBoxItemInfo newChickenBoxItemInfo = new NewChickenBoxItemInfo();
			newChickenBoxItemInfo.TemplateID = this.coinTemplateID;
			newChickenBoxItemInfo.StrengthenLevel = 0;
			newChickenBoxItemInfo.Count = 1;
			newChickenBoxItemInfo.IsBinds = true;
			newChickenBoxItemInfo.Quality = 1;
			this.m_LuckyStartRewards[0] = newChickenBoxItemInfo;
		}
		public void ChangeLuckyStartAwardPlace()
		{
		}
		public void SendLuckStarAllGoodsInfo()
		{
			GSPacketIn gSPacketIn = new GSPacketIn(87, this.m_player.PlayerId);
			gSPacketIn.WriteInt(21);
			gSPacketIn.WriteInt(this.m_activeInfo.LuckystarCoins);
			gSPacketIn.WriteDateTime(this.LuckyBegindate);
			gSPacketIn.WriteDateTime(this.LuckyEnddate);
			gSPacketIn.WriteInt(this.minUseNum);
			int num = this.m_LuckyStartRewards.Length;
			int i = 0;
			gSPacketIn.WriteInt(num);
			while (i < num)
			{
				gSPacketIn.WriteInt(this.m_LuckyStartRewards[i].TemplateID);
				gSPacketIn.WriteInt(this.m_LuckyStartRewards[i].StrengthenLevel);
				gSPacketIn.WriteInt(this.m_LuckyStartRewards[i].Count);
				gSPacketIn.WriteInt(this.m_LuckyStartRewards[i].ValidDate);
				gSPacketIn.WriteInt(this.m_LuckyStartRewards[i].AttackCompose);
				gSPacketIn.WriteInt(this.m_LuckyStartRewards[i].DefendCompose);
				gSPacketIn.WriteInt(this.m_LuckyStartRewards[i].AgilityCompose);
				gSPacketIn.WriteInt(this.m_LuckyStartRewards[i].LuckCompose);
				gSPacketIn.WriteBoolean(this.m_LuckyStartRewards[i].IsBinds);
				gSPacketIn.WriteInt(this.m_LuckyStartRewards[i].Quality);
				i++;
			}
			this.m_player.SendTCP(gSPacketIn);
		}
		public void SendLuckStarRewardRecord()
		{
			List<LuckStarRewardRecordInfo> recordList = ActiveSystemMgr.RecordList;
			GSPacketIn gSPacketIn = new GSPacketIn(87, this.m_player.PlayerId);
			gSPacketIn.WriteInt(22);
			gSPacketIn.WriteInt(recordList.Count);
			foreach (LuckStarRewardRecordInfo current in recordList)
			{
				gSPacketIn.WriteInt(current.TemplateID);
				gSPacketIn.WriteInt(current.Count);
				gSPacketIn.WriteString(current.nickName);
			}
			this.m_player.SendTCP(gSPacketIn);
		}
		public void SendUpdateReward()
		{
			if (this.Award.TemplateID == this.coinTemplateID)
			{
				return;
			}
			GSPacketIn gSPacketIn = new GSPacketIn(87, this.m_player.PlayerId);
			gSPacketIn.WriteInt(24);
			gSPacketIn.WriteInt(this.Award.TemplateID);
			gSPacketIn.WriteInt(this.Award.Count);
			gSPacketIn.WriteString(this.m_player.PlayerCharacter.NickName);
			this.m_player.SendTCP(gSPacketIn);
		}
		private void GetAward()
		{
			int num = this.rand.Next(this.m_LuckyStartRewards.Length);
			this.m_award = this.m_LuckyStartRewards[num];
		}
		public void SendLuckStarTurnGoodsInfo()
		{
			this.GetAward();
			this.m_activeInfo.LuckystarCoins += this.flushCoins;
			GSPacketIn gSPacketIn = new GSPacketIn(87, this.m_player.PlayerId);
			gSPacketIn.WriteInt(23);
			gSPacketIn.WriteInt(this.m_activeInfo.LuckystarCoins);
			gSPacketIn.WriteInt(this.Award.TemplateID);
			gSPacketIn.WriteInt(this.Award.StrengthenLevel);
			gSPacketIn.WriteInt(this.Award.Count);
			gSPacketIn.WriteInt(this.Award.ValidDate);
			gSPacketIn.WriteInt(this.Award.AttackCompose);
			gSPacketIn.WriteInt(this.Award.DefendCompose);
			gSPacketIn.WriteInt(this.Award.AgilityCompose);
			gSPacketIn.WriteInt(this.Award.LuckCompose);
			gSPacketIn.WriteBoolean(this.Award.IsBinds);
			this.m_player.SendTCP(gSPacketIn);
			if (this.Award.TemplateID == this.coinTemplateID)
			{
				if (GameProperties.IsActiveMoney)
				{
					this.m_player.AddActiveMoney(this.m_activeInfo.LuckystarCoins);
				}
				else
				{
					this.m_player.AddMoney(this.m_activeInfo.LuckystarCoins);
				}
				this.m_activeInfo.LuckystarCoins = this.defaultCoins;
			}
			ActiveSystemMgr.UpdateLuckStarRewardRecord(this.m_player.PlayerCharacter.ID, this.m_player.PlayerCharacter.NickName, this.Award.TemplateID, this.Award.Count, (int)this.m_player.PlayerCharacter.typeVIP);
		}
		public void SendLuckStarRewardRank()
		{
			GSPacketIn gSPacketIn = new GSPacketIn(87, this.m_player.PlayerId);
			gSPacketIn.WriteInt(27);
			List<LuckyStartToptenAwardInfo> luckyStartToptenAward = WorldEventMgr.GetLuckyStartToptenAward();
			gSPacketIn.WriteInt(luckyStartToptenAward.Count);
			foreach (LuckyStartToptenAwardInfo current in luckyStartToptenAward)
			{
				gSPacketIn.WriteInt(current.TemplateID);
				gSPacketIn.WriteInt(current.StrengthenLevel);
				gSPacketIn.WriteInt(current.Count);
				gSPacketIn.WriteInt(current.Validate);
				gSPacketIn.WriteInt(current.AttackCompose);
				gSPacketIn.WriteInt(current.DefendCompose);
				gSPacketIn.WriteInt(current.AgilityCompose);
				gSPacketIn.WriteInt(current.LuckCompose);
				gSPacketIn.WriteBoolean(current.IsBinds);
				gSPacketIn.WriteInt(current.Type);
			}
			this.m_player.SendTCP(gSPacketIn);
		}
		public GSPacketIn SendLightriddleRank(int myRank, List<RankingLightriddleInfo> list)
		{
			GSPacketIn gSPacketIn = new GSPacketIn(145, this.m_player.PlayerId);
			gSPacketIn.WriteByte(42);
			gSPacketIn.WriteInt(myRank);
			gSPacketIn.WriteInt(list.Count);
			foreach (RankingLightriddleInfo current in list)
			{
				gSPacketIn.WriteInt(current.Rank);
				gSPacketIn.WriteString(current.NickName);
				gSPacketIn.WriteByte((byte)current.TypeVIP);
				gSPacketIn.WriteInt(current.Integer);
				List<LuckyStartToptenAwardInfo> lanternriddlesAwardByRank = WorldEventMgr.GetLanternriddlesAwardByRank(current.Rank);
				gSPacketIn.WriteInt(lanternriddlesAwardByRank.Count);
				foreach (LuckyStartToptenAwardInfo current2 in lanternriddlesAwardByRank)
				{
					gSPacketIn.WriteInt(current2.TemplateID);
					gSPacketIn.WriteInt(current2.Count);
					gSPacketIn.WriteBoolean(current2.IsBinds);
					gSPacketIn.WriteInt(current2.Validate);
				}
			}
			this.m_player.SendTCP(gSPacketIn);
			return gSPacketIn;
		}
		public GSPacketIn SendLightriddleQuestion(LanternriddlesInfo Lanternriddles)
		{
			GSPacketIn gSPacketIn = new GSPacketIn(145, this.m_player.PlayerId);
			gSPacketIn.WriteByte(38);
			gSPacketIn.WriteInt(Lanternriddles.QuestionIndex);
			gSPacketIn.WriteInt(Lanternriddles.GetQuestionID);
			gSPacketIn.WriteInt(Lanternriddles.QuestionView);
			gSPacketIn.WriteDateTime(Lanternriddles.EndDate);
			gSPacketIn.WriteInt(Lanternriddles.DoubleFreeCount);
			gSPacketIn.WriteInt(Lanternriddles.DoublePrice);
			gSPacketIn.WriteInt(Lanternriddles.HitFreeCount);
			gSPacketIn.WriteInt(Lanternriddles.HitPrice);
			gSPacketIn.WriteInt(Lanternriddles.MyInteger);
			gSPacketIn.WriteInt(Lanternriddles.QuestionNum);
			gSPacketIn.WriteInt(Lanternriddles.Option);
			gSPacketIn.WriteBoolean(Lanternriddles.IsHint);
			gSPacketIn.WriteBoolean(Lanternriddles.IsDouble);
			this.m_player.SendTCP(gSPacketIn);
			return gSPacketIn;
		}
		public GSPacketIn SendLightriddleAnswerResult(bool Iscorrect, int option, string award)
		{
			GSPacketIn gSPacketIn = new GSPacketIn(145, this.m_player.PlayerId);
			gSPacketIn.WriteByte(39);
			gSPacketIn.WriteBoolean(Iscorrect);
			gSPacketIn.WriteBoolean(Iscorrect);
			gSPacketIn.WriteInt(option);
			gSPacketIn.WriteString(award);
			this.m_player.SendTCP(gSPacketIn);
			return gSPacketIn;
		}
		public void BeginLightriddleTimer()
		{
			int num = 1000;
			if (this._lightriddleTimer == null)
			{
				this._lightriddleTimer = new Timer(new TimerCallback(this.LightriddleCheck), null, num, num);
				return;
			}
			this._lightriddleTimer.Change(num, num);
		}
		protected void LightriddleCheck(object sender)
		{
			try
			{
				int num = Environment.TickCount;
				ThreadPriority priority = Thread.CurrentThread.Priority;
				Thread.CurrentThread.Priority = ThreadPriority.Lowest;
				if (this._lightriddleColdown > 0)
				{
					this._lightriddleColdown--;
					if (this._lightriddleColdown == 1)
					{
						LanternriddlesInfo lanternriddles = ActiveSystemMgr.GetLanternriddles(this.m_player.PlayerId);
						if (lanternriddles == null)
						{
							this.StopLightriddleTimer();
							return;
						}
						LightriddleQuestInfo getCurrentQuestion = lanternriddles.GetCurrentQuestion;
						string text = "5 Chiến Hồn Đơn, 10.000 EXP và 29 điểm tích lũy";
						string text2 = "1 Chiến Hồn Đơn và 1.000 EXP.";
						string award = "Hệ thống Nguyên Tiêu lổi.";
						int gp = 1000;
						ItemTemplateInfo goods = ItemMgr.FindItemTemplate(100100);
						ItemInfo itemInfo = ItemInfo.CreateFromTemplate(goods, 1, 105);
						bool iscorrect = false;
						if (getCurrentQuestion != null)
						{
							if (lanternriddles.IsHint)
							{
								lanternriddles.Option = getCurrentQuestion.OptionTrue;
							}
							if (lanternriddles.Option == getCurrentQuestion.OptionTrue)
							{
								itemInfo.Count = 5;
								gp = 10000;
								lanternriddles.MyInteger += 29;
								lanternriddles.QuestionNum++;
								if (lanternriddles.IsDouble)
								{
									text = "5 Chiến Hồn Đơn, 10.000 EXP và 58 điểm tích lũy";
									lanternriddles.MyInteger += 29;
								}
								award = text;
								iscorrect = true;
							}
							else
							{
								itemInfo.Count = 1;
								award = text2;
							}
						}
						if (lanternriddles.Option > 0)
						{
							this.m_player.AddGP(gp);
							itemInfo.IsBinds = true;
							this.m_player.AddTemplate(itemInfo);
							this.SendLightriddleAnswerResult(iscorrect, lanternriddles.Option, award);
							GameServer.Instance.LoginServer.SendLightriddleUpateRank(lanternriddles.MyInteger, this.m_player.PlayerCharacter);
						}
						GameServer.Instance.LoginServer.SendLightriddleInfo(lanternriddles);
					}
				}
				else
				{
					LanternriddlesInfo lanternriddles2 = ActiveSystemMgr.GetLanternriddles(this.m_player.PlayerId);
					if (lanternriddles2 == null)
					{
						this.StopLightriddleTimer();
						return;
					}
					if (lanternriddles2.CanNextQuest)
					{
						lanternriddles2.QuestionIndex++;
						lanternriddles2.Option = -1;
						lanternriddles2.IsHint = false;
						lanternriddles2.IsDouble = false;
						lanternriddles2.EndDate = ActiveSystemMgr.EndDate;
						this.SendLightriddleQuestion(lanternriddles2);
						this._lightriddleColdown = 15;
						GameServer.Instance.LoginServer.SendLightriddleRank(this.m_player.PlayerCharacter.NickName, this.m_player.PlayerCharacter.ID);
					}
					else
					{
						lanternriddles2.QuestionIndex = lanternriddles2.QuestionView;
						lanternriddles2.IsHint = true;
						lanternriddles2.IsDouble = true;
						lanternriddles2.EndDate = DateTime.Now;
						this.SendLightriddleQuestion(lanternriddles2);
						this.StopLightriddleTimer();
					}
					GameServer.Instance.LoginServer.SendLightriddleInfo(lanternriddles2);
				}
				Thread.CurrentThread.Priority = priority;
				num = Environment.TickCount - num;
			}
			catch (Exception arg)
			{
				Console.WriteLine("LabyrinthCheck: " + arg);
			}
		}
		public void StopLightriddleTimer()
		{
			if (this._lightriddleTimer != null)
			{
				this._lightriddleColdown = 15;
				this._lightriddleTimer.Dispose();
				this._lightriddleTimer = null;
			}
		}
	}
}
