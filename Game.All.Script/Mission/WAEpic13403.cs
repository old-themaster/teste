using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace Game.Server.GameServerScript.AI.Messions
{
    public class WAEpic13403 : AMissionControl
	{
		private SimpleBoss simpleBoss_0;

		private SimpleBoss simpleBoss_1;

		private int int_0;

		private int int_1;

		private int int_2;

		private int int_3;

		private int int_4;

		private PhysicalObj physicalObj_0;

		private PhysicalObj physicalObj_1;

		public WAEpic13403()
		{
			
			this.int_0 = 13406;
			this.int_1 = 13407;
			this.int_2 = 5322;
			this.int_3 = 5323;
			
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
			if (this.simpleBoss_1 == null || this.simpleBoss_1.IsLiving)
			{
				return false;
			}
			this.int_4++;
			return true;
		}

		private void method_0()
		{
			this.simpleBoss_0 = base.Game.CreateBoss(this.int_0, 1445, 650, -1, 0, "");
			this.simpleBoss_0.SetRelateDemagemRect(this.simpleBoss_0.NpcInfo.X, this.simpleBoss_0.NpcInfo.Y, this.simpleBoss_0.NpcInfo.Width, this.simpleBoss_0.NpcInfo.Height);
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
			if (this.simpleBoss_1 != null && !this.simpleBoss_1.IsLiving)
			{
				base.Game.IsWin = true;
				return;
			}
			base.Game.IsWin = false;
		}

		public override void OnNewTurnStarted()
		{
		}

		public override void OnPrepareNewSession()
		{
			base.OnPrepareNewSession();
			int[] int1 = new int[] { this.int_1, this.int_0, this.int_2, this.int_3 };
			int[] int0 = new int[] { this.int_0 };
			base.Game.LoadResources(int1);
			base.Game.LoadNpcGameOverResources(int0);
			base.Game.AddLoadingFile(2, "image/game/effect/5/heip.swf", "asset.game.4.heip");
			base.Game.AddLoadingFile(2, "image/game/effect/10/chengtuo.swf", "asset.game.ten.chengtuo");
			base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BossBgAsset");
			base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.dadangAsset");
			base.Game.SetMap(1216);
		}

		public override void OnStartGame()
		{
			base.OnStartGame();
			base.Game.IsBossWar = "13003";
			this.physicalObj_0 = base.Game.Createlayer(0, 0, "moive", "game.asset.living.BossBgAsset", "out", 1, 1);
			this.physicalObj_1 = base.Game.Createlayer(950, 650, "font", "game.asset.living.dadangAsset", "out", 1, 1);
			this.simpleBoss_1 = base.Game.CreateBoss(this.int_1, 1390, 1000, -1, 1, "");
			this.simpleBoss_1.CallFuction(new LivingCallBack(this.method_0), 3500);
			this.simpleBoss_1.SetRelateDemagemRect(this.simpleBoss_1.NpcInfo.X, this.simpleBoss_1.NpcInfo.Y, this.simpleBoss_1.NpcInfo.Width, this.simpleBoss_1.NpcInfo.Height);
			this.physicalObj_0.PlayMovie("in", 6000, 0);
			this.physicalObj_1.PlayMovie("in", 6100, 0);
			this.physicalObj_0.PlayMovie("out", 10000, 1000);
			this.physicalObj_1.PlayMovie("out", 9900, 0);
		}

		public override int UpdateUIData()
		{
			base.UpdateUIData();
			return this.int_4;
		}
	}
}