using Bussiness;
using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System.Collections.Generic;
using System.Drawing;

namespace GameServerScript.AI.NPC
{
    public class WorldSoccerCaptain : ABrain
	{
		private int m_attackTurn = 0;

		private int isSay = 0;

		private PhysicalObj moive;

		private Point[] brithPoint = new Point[]
		{
			new Point(979, 630),
			new Point(1013, 630),
			new Point(1052, 630),
			new Point(1088, 630),
			new Point(1142, 630)
		};

		private static string[] AllAttackChat = new string[]
		{
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg1", new object[0]),
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg2", new object[0]),
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg3", new object[0])
		};

		private static string[] ShootChat = new string[]
		{
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg4", new object[0]),
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg5", new object[0])
		};

		private static string[] KillPlayerChat = new string[]
		{
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg6", new object[0]),
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg7", new object[0])
		};

		private static string[] CallChat = new string[]
		{
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg8", new object[0]),
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg9", new object[0])
		};

		private static string[] JumpChat = new string[]
		{
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg10", new object[0]),
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg11", new object[0]),
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg12", new object[0])
		};

		private static string[] KillAttackChat = new string[]
		{
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg13", new object[0]),
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg14", new object[0])
		};

		private static string[] ShootedChat = new string[]
		{
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg15", new object[0]),
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg16", new object[0])
		};

		private static string[] DiedChat = new string[]
		{
			LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SimpleQueenAntAi.msg17", new object[0])
		};

		public override void OnBeginSelfTurn()
		{
			base.OnBeginSelfTurn();
		}

		public override void OnBeginNewTurn()
		{
			base.OnBeginNewTurn();
			base.Body.CurrentDamagePlus = 1f;
			base.Body.CurrentShootMinus = 1f;
			this.isSay = 0;
		}

		public override void OnCreated()
		{
			base.OnCreated();
		}

		public override void OnStartAttacking()
		{
			base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
			bool flag = false;
			int num = 0;
			foreach (Player current in base.Game.GetAllFightPlayers())
			{
				if (current.IsLiving && current.X > 1225 && current.X < 1571)
				{
					int num2 = (int)base.Body.Distance(current.X, current.Y);
					if (num2 > num)
					{
						num = num2;
					}
					flag = true;
				}
			}
			if (flag)
			{
				this.KillAttack(base.Body.X - 10000, base.Body.X + 10000);
			}
			else if (this.m_attackTurn == 0)
			{
				this.AttackA(base.Body.X - 10000, base.Body.X + 10000);
				this.m_attackTurn++;
			}
			else if (this.m_attackTurn == 1)
			{
				this.AttackB(base.Body.X - 10000, base.Body.X + 10000);
				this.m_attackTurn++;
			}
			else
			{
				this.AttackC(base.Body.X - 10000, base.Body.X + 10000);
				this.m_attackTurn = 0;
			}
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}

		private void KillAttack(int fx, int tx)
		{
			int num = base.Game.Random.Next(0, WorldSoccerCaptain.KillAttackChat.Length);
			base.Body.Say(WorldSoccerCaptain.KillAttackChat[num], 1, 1000);
			base.Body.CurrentDamagePlus = 100f;
			base.Body.PlayMovie("beatD", 3000, 0);
			base.Body.RangeAttacking(fx, tx, "cry", 5000, null);
		}

		private void AttackA(int fx, int tx)
		{
			Player player = base.Game.FindRandomPlayer();
			if (player != null)
			{
				base.Body.CurrentDamagePlus = 10f;
				int num = base.Game.Random.Next(0, WorldSoccerCaptain.ShootChat.Length);
				base.Body.Say(WorldSoccerCaptain.ShootChat[num], 1, 0);
				int num2 = base.Game.Random.Next(0, 1200);
				base.Body.PlayMovie("beatA", 1700, 0);
				base.Body.RangeAttacking(fx, tx, "cry", 4000, null);
			}
		}

		private void AttackB(int fx, int tx)
		{
			Player player = base.Game.FindRandomPlayer();
			if (player != null)
			{
				base.Body.CurrentDamagePlus = 15f;
				int num = base.Game.Random.Next(0, WorldSoccerCaptain.ShootChat.Length);
				base.Body.Say(WorldSoccerCaptain.ShootChat[num], 1, 0);
				int num2 = base.Game.Random.Next(0, 1200);
				base.Body.PlayMovie("beatB", 1900, 0);
				base.Body.RangeAttacking(fx, tx, "cry", 4000, null);
				base.Body.CallFuction(new LivingCallBack(this.GoMovie), 4000);
			}
		}

		private void GoMovie()
		{
			List<Player> allFightPlayers = base.Game.GetAllFightPlayers();
			foreach (Player current in allFightPlayers)
			{
				this.moive = ((PVEGame)base.Game).Createlayer(current.X, current.Y, "moive", "asset.game.zero.294b", "out", 1, 0);
			}
		}

		private void AttackC(int fx, int tx)
		{
			Player player = base.Game.FindRandomPlayer();
			if (player != null)
			{
				base.Body.CurrentDamagePlus = 20f;
				int num = base.Game.Random.Next(0, WorldSoccerCaptain.ShootChat.Length);
				base.Body.Say(WorldSoccerCaptain.ShootChat[num], 1, 0);
				int num2 = base.Game.Random.Next(0, 1200);
				base.Body.PlayMovie("beatC", 1700, 0);
				base.Body.RangeAttacking(fx, tx, "cry", 4000, null);
			}
		}

		public override void OnKillPlayerSay()
		{
			base.OnKillPlayerSay();
			int num = base.Game.Random.Next(0, WorldSoccerCaptain.KillPlayerChat.Length);
			base.Body.Say(WorldSoccerCaptain.KillPlayerChat[num], 1, 0, 2000);
		}

		public override void OnShootedSay()
		{
			int num = base.Game.Random.Next(0, WorldSoccerCaptain.ShootedChat.Length);
			if (this.isSay == 0 && base.Body.IsLiving)
			{
				base.Body.Say(WorldSoccerCaptain.ShootedChat[num], 1, 900, 0);
				this.isSay = 1;
			}
			if (!base.Body.IsLiving)
			{
				num = base.Game.Random.Next(0, WorldSoccerCaptain.DiedChat.Length);
				base.Body.Say(WorldSoccerCaptain.DiedChat[num], 1, 100, 2000);
			}
		}
	}
}
