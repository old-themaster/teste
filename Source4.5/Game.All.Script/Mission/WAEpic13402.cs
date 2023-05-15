using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System.Collections.Generic;

namespace Game.Server.GameServerScript.AI.Messions
{
    public class WAEpic13402 : AMissionControl
	{
		private SimpleBoss simpleBoss_0;

		private List<SimpleNpc> list_0;

		private List<PhysicalObj> list_1;

		private List<PhysicalObj> list_2;

		private int int_0;

		private int int_1;

		private int int_2;

		private int int_3;

		private int int_4;

		private int int_5;

		private PhysicalObj physicalObj_0;

		private PhysicalObj physicalObj_1;

		private PhysicalObj physicalObj_2;

		private SimpleBoss simpleBoss_1;

		public WAEpic13402()
		{
			
			this.list_0 = new List<SimpleNpc>();
			this.list_1 = new List<PhysicalObj>();
			this.list_2 = new List<PhysicalObj>();
			this.int_0 = 13405;
			this.int_1 = 13403;
			this.int_2 = 13404;
			this.int_3 = 3312;
			this.int_4 = 13000;
			
		}

		public override int CalculateScoreGrade(int score)
		{
			base.CalculateScoreGrade(score);
			if (score > 1750)
			{
				return 3;
			}
			if (score > 1675)
			{
				return 2;
			}
			if (score > 1600)
			{
				return 1;
			}
			return 0;
		}

		public override bool CanGameOver()
		{
			if (this.simpleBoss_0 == null || this.simpleBoss_0.IsLiving)
			{
				return false;
			}
			this.int_5++;
			return true;
		}

		public override void OnBeginNewTurn()
		{
			base.OnBeginNewTurn();
			if (base.Game.TurnIndex > 1)
			{
				if (this.physicalObj_0 != null)
				{
					base.Game.RemovePhysicalObj(this.physicalObj_0, true);
					this.physicalObj_0 = null;
				}
				if (this.physicalObj_1 != null)
				{
					base.Game.RemovePhysicalObj(this.physicalObj_1, true);
					this.physicalObj_1 = null;
				}
			}
		}

		public override void OnGameOver()
		{
			base.OnGameOver();
			if (this.simpleBoss_0 != null && !this.simpleBoss_0.IsLiving)
			{
				base.Game.IsWin = true;
				return;
			}
			base.Game.IsWin = false;
		}

		public override void OnNewTurnStarted()
		{
			base.OnNewTurnStarted();
		}

		public override void OnPrepareNewSession()
		{
			base.OnPrepareNewSession();
			int[] int0 = new int[] { this.int_0, this.int_1, this.int_2, this.int_3 };
			int[] numArray = new int[] { this.int_0 };
			base.Game.LoadResources(int0);
			base.Game.LoadNpcGameOverResources(numArray);
			base.Game.AddLoadingFile(1, "bombs/55.swf", "tank.resource.bombs.Bomb55");
			base.Game.AddLoadingFile(2, "image/game/effect/10/jitan.swf", "asset.game.ten.jitan");
			base.Game.AddLoadingFile(2, "image/game/living/living035.swf", "game.living.Living035");
			base.Game.AddLoadingFile(2, "image/game/living/living126.swf", "game.living.Living126");
			base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BossBgAsset");
			base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.ClanLeaderAsset");
			base.Game.SetMap(1215);
		}

		public override void OnStartGame()
		{
			base.OnStartGame();
			base.Game.IsBossWar = "13002";
			this.physicalObj_0 = base.Game.Createlayer(0, 0, "moive", "game.asset.living.BossBgAsset", "out", 1, 1);
			this.physicalObj_1 = base.Game.Createlayer(950, 750, "font", "game.asset.living.ClanLeaderAsset", "out", 1, 1);
			this.physicalObj_2 = base.Game.CreatePhysicalObj(609, 1023, "font", "game.living.Living035", "", 1, 0);
			this.physicalObj_2 = base.Game.CreatePhysicalObj(1604, 1023, "font", "game.living.Living035", "", 1, 0);
			this.simpleBoss_0 = base.Game.CreateBoss(this.int_0, 1100, 1000, -1, 1, "");
			this.simpleBoss_0.SetRelateDemagemRect(this.simpleBoss_0.NpcInfo.X, this.simpleBoss_0.NpcInfo.Y, this.simpleBoss_0.NpcInfo.Width, this.simpleBoss_0.NpcInfo.Height);
			//this.simpleBoss_0.Say(LanguageMgr.GetTranslation("A ira do espírito irá destruí-lo !", Array.Empty<object>()), 0, 200, 0);
			this.physicalObj_0.PlayMovie("in", 6000, 0);
			this.physicalObj_1.PlayMovie("in", 6100, 0);
			this.physicalObj_0.PlayMovie("out", 10000, 1000);
			this.physicalObj_1.PlayMovie("out", 9900, 0);
		}

		public override int UpdateUIData()
		{
			base.UpdateUIData();
			return this.int_5;
		}
	}
}