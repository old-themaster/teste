using Bussiness;
using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Effects;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.NPC

{
	public class FourNormalCattleBoss : ABrain
	{
		private int m_attackTurn = 0;

		private PhysicalObj m_effectAttack = null;

		private PhysicalObj m_powerUpEffect = null;

		private int m_totalNpc = 3;

		private int npcId = 4107;

		private float m_perPowerUp = 0.2f;

		private int m_reduceStreng = 50;

		private bool IsFear = false;

		private Player target = null;

		private float lastpowDamage = 0f;

		private float m_currentPowDamage = 0f;

		private int isSay = 0;

		private string[] CallNpcSay = new string[2]
		{
		LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalCattleBoss.msg2"),
		LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalCattleBoss.msg3")
		};

		private string[] KillAttackSay = new string[4]
		{
		LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalCattleBoss.msg4"),
		LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalCattleBoss.msg5"),
		LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalCattleBoss.msg6"),
		LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalCattleBoss.msg7")
		};

		private string[] AllAttackPlayerSay = new string[4]
		{
		LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalCattleBoss.msg8"),
		LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalCattleBoss.msg9"),
		LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalCattleBoss.msg10"),
		LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalCattleBoss.msg11")
		};

		private string[] TiredSay = new string[3]
		{
		LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalCattleBoss.msg12"),
		LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalCattleBoss.msg13"),
		LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalCattleBoss.msg14")
		};

		private string[] FearSay = new string[3]
		{
		LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalCattleBoss.msg15"),
		LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalCattleBoss.msg16"),
		LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalCattleBoss.msg17")
		};

		private string[] OnShootedChat = new string[12]
		{
		LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalCattleBoss.msg18"),
		LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalCattleBoss.msg19"),
		LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalCattleBoss.msg20"),
		LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalCattleBoss.msg21"),
		LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalCattleBoss.msg22"),
		LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalCattleBoss.msg23"),
		LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalCattleBoss.msg24"),
		LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalCattleBoss.msg25"),
		LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalCattleBoss.msg26"),
		LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalCattleBoss.msg27"),
		LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalCattleBoss.msg28"),
		LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalCattleBoss.msg29")
		};

		private string[] SpecialAttackSay = new string[4]
		{
		LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalCattleBoss.msg30"),
		LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalCattleBoss.msg31"),
		LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalCattleBoss.msg32"),
		LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalCattleBoss.msg33")
		};

		private string[] DiedChat = new string[1] { LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalCattleBoss.msg34") };

		private string[] KillPlayerChat = new string[4]
		{
		LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalCattleBoss.msg35"),
		LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalCattleBoss.msg36"),
		LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalCattleBoss.msg37"),
		LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourNormalCattleBoss.msg38")
		};

		public override void OnBeginSelfTurn()
		{
			base.OnBeginSelfTurn();
		}

		public override void OnBeginNewTurn()
		{
			base.OnBeginNewTurn();
			ClearEffect();
			base.Body.CurrentShootMinus = 1f;
			isSay = 0;
			if (!IsFear)
			{
				base.Body.Config.HaveShield = true;
			}
			else
			{
				base.Body.Config.HaveShield = false;
			}
		}

		public override void OnCreated()
		{
			base.OnCreated();
			IsFear = false;
			base.Body.CurrentDamagePlus = 1f;
			m_currentPowDamage = 1f;
		}

		public override void OnStartAttacking()
		{
			if (!IsFear)
			{
				bool flag = false;
				foreach (Player allFightPlayer in base.Game.GetAllFightPlayers())
				{
					if (allFightPlayer.IsLiving && allFightPlayer.X > base.Body.X - 200 && allFightPlayer.X < base.Body.X + 200)
					{
						flag = true;
					}
				}
				if (flag)
				{
					KillAttack(base.Body.X - 200, base.Body.X + 200);
					return;
				}
			}
			if (m_attackTurn == 0)
			{
				if (!IsFear)
				{
					base.Body.CallFuction(PowerUpEffect, 2000);
					if (((SimpleBoss)base.Body).CurrentLivingNpcNum <= 0)
					{
						int num = base.Game.Random.Next(0, CallNpcSay.Length);
						base.Body.Say(CallNpcSay[num], 0, 5000);
						base.Body.CallFuction(CallNpc, 7000);
					}
					else
					{
						base.Body.CallFuction(AttackAllPlayer, 4000);
					}
				}
				m_attackTurn++;
			}
			else if (m_attackTurn == 1)
			{
				if (!IsFear)
				{
					base.Body.CallFuction(PowerUpEffect, 2000);
					base.Body.CallFuction(AttackPerson, 4000);
					m_attackTurn++;
				}
				else
				{
					m_attackTurn = 3;
				}
			}
			else if (m_attackTurn == 2)
			{
				if (!IsFear)
				{
					base.Body.CallFuction(PowerUpEffect, 2000);
					base.Body.CallFuction(AttackAllPlayer, 4000);
				}
				m_attackTurn++;
			}
			else
			{
				if (m_attackTurn != 3)
				{
					return;
				}
				if (!IsFear)
				{
					base.Body.CallFuction(PowerUpEffect, 2000);
					if (((SimpleBoss)base.Body).CurrentLivingNpcNum <= 0)
					{
						IsFear = true;
						int num2 = base.Game.Random.Next(0, TiredSay.Length);
						base.Body.Say(TiredSay[num2], 0, 2200);
						base.Body.CallFuction(ChangeAtoB, 4000);
					}
					else
					{
						base.Body.CallFuction(JumpAndAttack, 4000);
					}
				}
				else
				{
					IsFear = false;
					m_currentPowDamage = 1f;
					base.Body.CallFuction(ChangeBtoA, 2000);
				}
				m_attackTurn = 0;
			}
		}

		private void KillAttack(int fx, int tx)
		{
			lastpowDamage = base.Body.CurrentDamagePlus;
			base.Body.CurrentDamagePlus = 1000f;
			base.Body.ChangeDirection(base.Game.FindlivingbyDir(base.Body), 100);
			((SimpleBoss)base.Body).RandomSay(KillAttackSay, 0, 2000, 0);
			base.Body.PlayMovie("beatC", 2000, 0);
			base.Body.PlayMovie("beatE", 5000, 0);
			base.Body.RangeAttacking(fx, tx, "cry", 7000, null);
			base.Body.CallFuction(SetState, 8000);
		}

		private void AttackPerson()
		{
			target = base.Game.FindNearestPlayer(base.Body.X, base.Body.Y);
			if (target != null)
			{
				base.Body.ChangeDirection(target, 100);
				int num = base.Game.Random.Next(0, KillAttackSay.Length);
				base.Body.Say(KillAttackSay[num], 0, 1000);
				base.Body.PlayMovie("beatA", 1200, 0);
				((PVEGame)base.Game).SendObjectFocus(target, 1, 3200, 0);
				base.Body.CallFuction(CreateAttackEffect, 4000);
				if (base.Body.FindDirection(target) == -1)
				{
					base.Body.RangeAttacking(target.X - 50, base.Body.X, "cry", 4800, null);
				}
				else
				{
					base.Body.RangeAttacking(base.Body.X, target.X + 50, "cry", 4800, null);
				}
				base.Body.CallFuction(SetState, 6000);
			}
		}

		private void AttackAllPlayer()
		{
			((SimpleBoss)base.Body).RandomSay(AllAttackPlayerSay, 0, 1000, 0);
			base.Body.PlayMovie("beatB", 1000, 0);
			base.Body.RangeAttacking(base.Body.X - 10000, base.Body.X + 10000, "cry", 4100, null);
			foreach (Player allLivingPlayer in base.Game.GetAllLivingPlayers())
			{
				allLivingPlayer.AddEffect(new ReduceStrengthEffect(1, m_reduceStreng), 4200);
			}
			base.Body.CallFuction(SetState, 5000);
		}

		private void ChangeAtoB()
		{
			base.Body.PlayMovie("beatD", 1000, 0);
			((SimpleBoss)base.Body).RandomSay(FearSay, 0, 4100, 0);
			base.Body.PlayMovie("AtoB", 4000, 0);
			base.Body.CallFuction(SetState, 7000);
		}

		private void ChangeBtoA()
		{
			base.Body.PlayMovie("AtoB", 1000, 0);
			((SimpleBoss)base.Body).RandomSay(SpecialAttackSay, 0, 2200, 0);
			base.Body.CallFuction(JumpAndAttack, 2000);
		}

		private void JumpAndAttack()
		{
			target = base.Game.FindRandomPlayer();
			if (target != null)
			{
				base.Body.PlayMovie("jump", 500, 0);
				((PVEGame)base.Game).SendObjectFocus(target, 1, 2000, 0);
				base.Body.BoltMove(target.X, target.Y, 2500);
				base.Body.PlayMovie("fall", 2600, 0);
				base.Body.RangeAttacking(target.X - 100, target.X + 100, "cry", 3000, null);
				base.Body.CallFuction(SetState, 4000);
			}
		}

		private void PowerUpEffect()
		{
			m_currentPowDamage += m_perPowerUp;
			base.Body.CurrentDamagePlus = m_currentPowDamage;
			m_powerUpEffect = ((PVEGame)base.Game).Createlayer(base.Body.X, base.Body.Y - 60, "", "game.crazytank.assetmap.Buff_powup", "", 1, 0);
		}

		private void CreateAttackEffect()
		{
			if (target != null)
			{
				m_effectAttack = ((PVEGame)base.Game).Createlayer(target.X, target.Y, "", "asset.game.4.blade", "", 1, 0);
			}
		}

		private void ClearEffect()
		{
			if (m_powerUpEffect != null)
			{
				base.Game.RemovePhysicalObj(m_powerUpEffect, sendToClient: true);
			}
			if (m_effectAttack != null)
			{
				base.Game.RemovePhysicalObj(m_effectAttack, sendToClient: true);
			}
		}

		private void CallNpc()
		{
			LivingConfig livingConfig = ((PVEGame)base.Game).BaseLivingConfig();
			livingConfig.IsFly = true;
			for (int i = 0; i < m_totalNpc; i++)
			{
				int x = base.Game.Random.Next(350, 1300);
				int y = base.Game.Random.Next(100, 700);
				((SimpleBoss)base.Body).CreateChild(npcId, x, y, 0, -1, showBlood: true, livingConfig);
			}
		}

		private void RemoveAllNpc()
		{
			((SimpleBoss)base.Body).RemoveAllChild();
		}

		private void SetState()
		{
			if (IsFear)
			{
				((PVEGame)base.Game).SendLivingActionMapping(base.Body, "stand", "standB");
			}
			else
			{
				((PVEGame)base.Game).SendLivingActionMapping(base.Body, "stand", "standA");
			}
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}

		public override void OnShootedSay(int delay)
		{
			base.OnShootedSay(delay);
			int num = base.Game.Random.Next(0, OnShootedChat.Length);
			if (isSay == 0 && base.Body.IsLiving)
			{
				base.Body.Say(OnShootedChat[num], 0, 1000 + delay, 0);
				isSay = 1;
			}
		}

		public override void OnKillPlayerSay()
		{
			base.OnKillPlayerSay();
			((SimpleBoss)base.Body).RandomSay(KillPlayerChat, 0, 0, 2000);
		}

		public override void OnDiedSay()
		{
			base.OnDiedSay();
			int num = base.Game.Random.Next(0, DiedChat.Length);
			base.Body.Say(DiedChat[num], 1, 0, 1500);
		}
	}
}