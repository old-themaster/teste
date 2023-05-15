using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System.Collections.Generic;

namespace Game.Server.GameServerScript.AI.Messions
{
    public class WAEpic13404 : AMissionControl
	{
		private List<SimpleBoss> list_0;

		private int int_0;

		private int int_1;

		private int int_2;

		private int int_3;

		private int int_4;

		private PhysicalObj physicalObj_0;

		private PhysicalObj physicalObj_1;

		public WAEpic13404()
		{
			
			this.list_0 = new List<SimpleBoss>();
			this.int_0 = 13408;
			this.int_1 = 13409;
			this.int_2 = 13412;
			
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
			this.int_3 = 0;
			bool flag = true;
			base.CanGameOver();
			foreach (SimpleBoss list0 in this.list_0)
			{
				if (!list0.IsLiving)
				{
					this.int_3++;
				}
				else
				{
					flag = false;
				}
			}
			if (flag && this.int_3 == base.Game.MissionInfo.TotalCount)
			{
				return true;
			}
			return false;
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
			if (this.int_3 == base.Game.MissionInfo.TotalCount)
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
			int[] int1 = new int[] { this.int_1, this.int_0, this.int_2 };
			int[] int0 = new int[] { this.int_0 };
			base.Game.LoadResources(int1);
			base.Game.LoadNpcGameOverResources(int0);
			base.Game.AddLoadingFile(2, "image/game/effect/10/tedabiaoji.swf", "asset.game.ten.tedabiaoji");
			base.Game.AddLoadingFile(2, "image/game/effect/10/qunbao.swf", "asset.game.ten.qunbao");
			base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BossBgAsset");
			base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.canbaoAsset");
			base.Game.SetMap(1217);
		}

		public override void OnStartGame()
		{
			base.OnStartGame();
			this.physicalObj_0 = base.Game.Createlayer(0, 0, "moive", "game.asset.living.BossBgAsset", "out", 1, 1);
			this.physicalObj_1 = base.Game.Createlayer(1000, 600, "font", "game.asset.living.canbaoAsset", "out", 1, 1);
			SimpleBoss simpleBoss = base.Game.CreateBoss(this.int_0, 100, 990, 1, 1, "");
			simpleBoss.SetRelateDemagemRect(simpleBoss.NpcInfo.X, simpleBoss.NpcInfo.Y, simpleBoss.NpcInfo.Width, simpleBoss.NpcInfo.Height);
			this.list_0.Add(simpleBoss);
			simpleBoss = base.Game.CreateBoss(this.int_1, 1890, 990, -1, 1, "");
			simpleBoss.SetRelateDemagemRect(simpleBoss.NpcInfo.X, simpleBoss.NpcInfo.Y, simpleBoss.NpcInfo.Width, simpleBoss.NpcInfo.Height);
			this.list_0.Add(simpleBoss);
			this.physicalObj_0.PlayMovie("in", 6000, 0);
			this.physicalObj_1.PlayMovie("in", 6100, 0);
			this.physicalObj_0.PlayMovie("out", 10000, 1000);
			this.physicalObj_1.PlayMovie("out", 9900, 0);
		}

		public override int UpdateUIData()
		{
			base.UpdateUIData();
			return this.int_3;
		}
	}
}