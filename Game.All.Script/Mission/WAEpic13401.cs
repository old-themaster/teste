using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System.Collections.Generic;

namespace Game.Server.GameServerScript.AI.Messions
{
    public class WAEpic13401 : AMissionControl
	{
		private List<SimpleBoss> list_0;

		private int dshszbvoaIy;

		private int int_0;

		private int int_1;

		private PhysicalObj physicalObj_0;

		private PhysicalObj physicalObj_1;

		public WAEpic13401()
		{
			
			this.list_0 = new List<SimpleBoss>();
			this.dshszbvoaIy = 13401;
			this.int_0 = 13402;
			
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
			this.int_1 = 0;
			bool flag = true;
			base.CanGameOver();
			foreach (SimpleBoss list0 in this.list_0)
			{
				if (!list0.IsLiving)
				{
					this.int_1++;
				}
				else
				{
					flag = false;
				}
			}
			if (flag && this.int_1 == base.Game.MissionInfo.TotalCount)
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
			if (this.int_1 == base.Game.MissionInfo.TotalCount)
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
			int[] int0 = new int[] { this.dshszbvoaIy, this.int_0 };
			int[] numArray = new int[0];
			base.Game.LoadResources(int0);
			base.Game.LoadNpcGameOverResources(numArray);
			base.Game.AddLoadingFile(1, "bombs/51.swf", "tank.resource.bombs.Bomb51");
			base.Game.AddLoadingFile(1, "bombs/99.swf", "tank.resource.bombs.Bomb99");
			base.Game.AddLoadingFile(2, "image/game/effect/10/jianyu.swf", "asset.game.ten.jianyu");
			base.Game.AddLoadingFile(1, "bombs/61.swf", "tank.resource.bombs.Bomb61");
			base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BossBgAsset");
			base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.canbaoAsset");
			base.Game.SetMap(1214);
		}

		public override void OnStartGame()
		{
			base.OnStartGame();
			base.Game.IsBossWar = "13301";
			this.physicalObj_0 = base.Game.Createlayer(0, 0, "moive", "game.asset.living.BossBgAsset", "out", 1, 1);
			this.physicalObj_1 = base.Game.Createlayer(1008, 304, "font", "game.asset.living.canbaoAsset", "out", 1, 1);
			SimpleBoss simpleBoss = base.Game.CreateBoss(this.int_0, 1269, 840, -1, 1, "");
			simpleBoss.SetRelateDemagemRect(simpleBoss.NpcInfo.X, simpleBoss.NpcInfo.Y, simpleBoss.NpcInfo.Width, simpleBoss.NpcInfo.Height);
			this.list_0.Add(simpleBoss);
			simpleBoss = base.Game.CreateBoss(this.dshszbvoaIy, 1269, 180, -1, 1, "");
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
			return this.int_1;
		}
	}
}