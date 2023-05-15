using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Effects;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Game.Server.GameServerScript.AI.NPC
{
    public class ThirteenNormalThirdBoss : ABrain
	{
		private int int_0;

		private SimpleBoss simpleBoss_0;

		protected Player m_targer;

		private List<Player> list_0;

		private List<PhysicalObj> list_1;

		private PhysicalObj physicalObj_0;

		private PhysicalObj physicalObj_1;

		private int int_1;

		private int int_2;

		private int int_3;

		private string[] string_0;

		private string[] string_1;

		private string[] string_2;

		public ThirteenNormalThirdBoss()
		{
			
			this.list_1 = new List<PhysicalObj>();
			this.int_1 = 13106;
			this.int_2 = 80;
			this.int_3 = 10000;
			this.string_0 = new string[] { "Droga para mim.", "Droga para mim.", "Cozinhe no vapor para nós.", "Merda eles por mim." };
			this.string_1 = new string[] { "Ah, está certo. É você", "O próximo será você", "Tu. É você !!" };
			this.string_2 = new string[] { "O que há de errado com você?", "Aprendizagem tola não vale nada.", "Você quer ser esfolado, hein?", "Essa bandeira me aperta?", "Você os está ajudando ou me ajudando?" };
			
		}

		private void method_0()
		{
			if ((int)base.Body.Properties1 != 0)
			{
				this.method_4();
				return;
			}
			this.method_9();
		}

		private void method_1()
		{
			base.Body.CurrentDamagePlus = 1.8f;
			if (this.m_targer != null)
			{
				this.list_0 = new List<Player>();
				base.Body.ChangeDirection(this.m_targer, 1000);
				if (base.Body.FindDirection(this.m_targer) == 1)
				{
					foreach (Player allLivingPlayer in base.Game.GetAllLivingPlayers())
					{
						if (allLivingPlayer.X < base.Body.X || allLivingPlayer.X > base.Game.Map.Bound.Width)
						{
							continue;
						}
						this.list_0.Add(allLivingPlayer);
					}
					Living body = base.Body;
					Rectangle bound = base.Game.Map.Bound;
					body.MoveTo(bound.Width, base.Body.Y, "walk", 2000, 10, new LivingCallBack(this.method_2));
					return;
				}
				foreach (Player player in base.Game.GetAllLivingPlayers())
				{
					if (player.X < 0 || player.X > base.Body.X)
					{
						continue;
					}
					this.list_0.Add(player);
				}
				base.Body.MoveTo(0, base.Body.Y, "walk", 2000, 10, new LivingCallBack(this.method_2));
			}
		}

		private void method_10()
		{
			if (this.m_targer != null)
			{
				this.m_targer.AddEffect(new AddTargetEffect(), 0);
			}
		}

		private void method_2()
		{
			if (this.list_0.Count > 0)
			{
				foreach (Player list0 in this.list_0)
				{
					base.Body.BeatDirect(list0, "", 1000, 1, 1);
				}
			}
			SimpleNpc[] simpleNpcArray = base.Game.FindAllNpcLiving();
			if (simpleNpcArray.Length != 0)
			{
				SimpleNpc[] simpleNpcArray1 = simpleNpcArray;
				for (int i = 0; i < (int)simpleNpcArray1.Length; i++)
				{
					SimpleNpc simpleNpc = simpleNpcArray1[i];
					if (simpleNpc.Properties1 != null)
					{
						Player player = base.Game.FindPlayer((int)simpleNpc.Properties1);
						if (player != null && player.IsLiving)
						{
							player.SetVisible(true);
							player.BlockTurn = false;
						}
					}
					simpleNpc.PlayMovie("die", 500, 0);
					simpleNpc.Die(2000);
				}
			}
			base.Body.CallFuction(new LivingCallBack(this.method_3), 2500);
		}

		private void method_3()
		{
			Random random = base.Game.Random;
			Rectangle bound = base.Game.Map.Bound;
			int num = random.Next(100, bound.Width - 100);
			base.Body.PlayMovie("jump", 1000, 0);
			base.Body.BoltMove(num, base.Body.Y, 3000);
			(base.Game as PVEGame).SendFreeFocus(num, base.Body.Y, 1, 2000, 0);
			base.Body.PlayMovie("fall", 3000, 2000);
		}

		private void method_4()
		{
			if ((int)base.Body.Properties1 != 0)
			{
				(base.Game as PVEGame).SendLivingActionMapping(base.Body, "stand", "stand");
				(base.Game as PVEGame).SendLivingActionMapping(base.Body, "cry", "cry");
				base.Body.Properties1 = 0;
				base.Body.Config.CancelGuard = false;
				base.Body.PlayMovie("stand", 500, 0);
				base.Body.ChangeDirection(this.simpleBoss_0, 1000);
				int num = base.Game.Random.Next((int)this.string_2.Length);
				base.Body.Say(this.string_2[num], 2000, 3000);
			}
			else
			{
				base.Body.PlayMovie("beatA", 1000, 4000);
				base.Body.CallFuction(new LivingCallBack(this.method_6), 3000);
			}
			base.Body.CallFuction(new LivingCallBack(this.method_5), 5000);
		}

		private void method_5()
		{
			this.m_targer = base.Game.FindRandomPlayer();
			base.Body.ChangeDirection(this.m_targer, 500);
			int num = base.Game.Random.Next((int)this.string_1.Length);
			base.Body.Say(this.string_1[num], 1000, 0);
			base.Body.CallFuction(new LivingCallBack(this.method_10), 2000);
		}

		private void method_6()
		{
			base.Body.AddBlood(this.int_3);
		}

		private void method_7()
		{
			base.Body.CurrentDamagePlus = 2.5f;
			base.Body.ChangeDirection(this.m_targer, 1000);
			base.Body.PlayMovie("jump", 2000, 1600);
			base.Body.BoltMove(this.m_targer.X, this.m_targer.Y, 3700);
			base.Body.PlayMovie("fallB", 4000, 2000);
			base.Body.BeatDirect(this.m_targer, "", 5000, 1, 1);
			base.Body.CallFuction(new LivingCallBack(this.method_8), 6000);
		}

		private void method_8()
		{
			if (this.m_targer == null)
			{
				foreach (Player allLivingPlayer in base.Game.GetAllLivingPlayers())
				{
					AddTargetEffect ofType = allLivingPlayer.EffectList.GetOfType(eEffectType.AddTargetEffect) as AddTargetEffect;
					if (ofType == null)
					{
						continue;
					}
					ofType.Stop();
				}
			}
			else
			{
				AddTargetEffect addTargetEffect = this.m_targer.EffectList.GetOfType(eEffectType.AddTargetEffect) as AddTargetEffect;
				if (addTargetEffect != null)
				{
					addTargetEffect.Stop();
					return;
				}
			}
		}

		private void method_9()
		{
			this.m_targer = base.Game.FindRandomPlayer();
			int num = base.Game.Random.Next((int)this.string_0.Length);
			base.Body.Say(this.string_0[num], 1000, 0);
			base.Body.CallFuction(new LivingCallBack(this.method_10), 3000);
		}

		public override void OnBeginNewTurn()
		{
			base.OnBeginNewTurn();
			this.m_body.CurrentDamagePlus = 1f;
			this.m_body.CurrentShootMinus = 1f;
			if (this.list_1 != null && this.list_1.Count > 0)
			{
				foreach (PhysicalObj list1 in this.list_1)
				{
					base.Game.RemovePhysicalObj(list1, true);
				}
				this.list_1.Clear();
			}
		}

		public override void OnBeginSelfTurn()
		{
			base.OnBeginSelfTurn();
			if (this.simpleBoss_0 == null)
			{
				SimpleBoss[] simpleBossArray = ((PVEGame)base.Game).FindLivingTurnBossWithID(this.int_1);
				if (simpleBossArray.Length != 0)
				{
					this.simpleBoss_0 = simpleBossArray[0];
				}
			}
		}

		public override void OnCreated()
		{
			base.OnCreated();
			base.Body.Properties1 = 0;
			base.Body.Config.CancelGuard = false;
		}

		public override void OnDie()
		{
			base.OnDie();
		}

		public override void OnStartAttacking()
		{
			base.OnStartAttacking();
			switch (this.int_0)
			{
				case 0:
				{
					this.method_0();
					break;
				}
				case 1:
				{
					this.method_7();
					break;
				}
				case 2:
				{
					this.method_4();
					break;
				}
				case 3:
				{
					this.method_1();
					break;
				}
				case 4:
				{
					this.int_0 = -1;
					break;
				}
			}
			this.int_0++;
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}
	}
}