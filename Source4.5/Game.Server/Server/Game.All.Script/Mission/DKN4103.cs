using Bussiness;
using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.Messions

{
	public class DKN4103 : AMissionControl
	{
		private SimpleBoss m_king = null;

		private int bossID = 4108;

		private int npcId = 4107;

		private int kill = 0;

		private PhysicalObj m_moive;

		private PhysicalObj m_front;

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

		public override void OnPrepareNewSession()
		{
			base.OnPrepareNewSession();
			int[] npcIds = new int[2] { bossID, npcId };
			int[] npcIds2 = new int[1] { bossID };
			base.Game.AddLoadingFile(2, "image/game/effect/4/power.swf", "game.crazytank.assetmap.Buff_powup");
			base.Game.AddLoadingFile(2, "image/game/effect/4/blade.swf", "asset.game.4.blade");
			base.Game.AddLoadingFile(2, "image/game/thing/bossbornbgasset.swf", "game.asset.living.BossBgAsset");
			base.Game.AddLoadingFile(2, "image/game/thing/bossbornbgasset.swf", "game.asset.living.tingyuanlieshouAsset");
			base.Game.LoadResources(npcIds);
			base.Game.LoadNpcGameOverResources(npcIds2);
			base.Game.SetMap(1144);
		}

		public override void OnStartGame()
		{
			base.OnStartGame();
			LivingConfig livingConfig = base.Game.BaseLivingConfig();
			livingConfig.HaveShield = true;
			m_moive = base.Game.Createlayer(0, 0, "moive", "game.asset.living.BossBgAsset", "out", 1, 0);
			m_front = base.Game.Createlayer(1019, 620, "front", "game.asset.living.emozhanshiAsset", "out", 1, 0);
			m_king = base.Game.CreateBoss(bossID, 1255, 958, -1, 1, 0, "born", livingConfig);
			m_king.SetRelateDemagemRect(m_king.NpcInfo.X, m_king.NpcInfo.Y, m_king.NpcInfo.Width, m_king.NpcInfo.Height);
			m_king.CallFuction(MovieCreateBoss, 1000);
		}

		private void MovieCreateBoss()
		{
			base.Game.SendObjectFocus(m_king, 1, 500, 0);
			m_king.PlayMovie("in", 2000, 0);
			base.Game.SendObjectFocus(m_king, 2, 2000, 3000);
			m_king.PlayMovie("standA", 9000, 0);
			m_king.Say(LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalCattleBoss.msg1"), 0, 9200);
			m_moive.PlayMovie("in", 13000, 0);
			m_front.PlayMovie("in", 13200, 0);
			m_moive.PlayMovie("out", 16200, 0);
			m_front.PlayMovie("out", 16000, 0);
		}

		public override void OnNewTurnStarted()
		{
			base.OnNewTurnStarted();
		}

		public override void OnBeginNewTurn()
		{
			base.OnBeginNewTurn();
			if (m_king == null || m_king.IsLiving)
			{
				base.Game.TotalKillCount = 0;
			}
			if (base.Game.TurnIndex > 1)
			{
				if (m_moive != null)
				{
					base.Game.RemovePhysicalObj(m_moive, sendToClient: true);
					m_moive = null;
				}
				if (m_front != null)
				{
					base.Game.RemovePhysicalObj(m_front, sendToClient: true);
					m_front = null;
				}
			}
		}

		public override bool CanGameOver()
		{
			if (m_king != null && !m_king.IsLiving)
			{
				return true;
			}
			if (base.Game.TotalTurn > base.Game.MissionInfo.TotalTurn)
			{
				return true;
			}
			return false;
		}

		public override int UpdateUIData()
		{
			base.UpdateUIData();
			return base.Game.TotalKillCount;
		}

		public override void OnGameOver()
		{
			base.OnGameOver();
			if (m_king != null && !m_king.IsLiving)
			{
				base.Game.IsWin = true;
			}
			else
			{
				base.Game.IsWin = false;
			}
		}

		public override void OnShooted()
		{
			base.OnShooted();
		}
	}
}