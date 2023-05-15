using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System.Collections.Generic;

namespace Game.Server.GameServerScript.AI.NPC
{
    public class ThirteenHardFourthBoss : ABrain
	{
		private int int_0;

		private int int_1;

		private SimpleBoss simpleBoss_0;

		protected Player m_targer;

		private List<PhysicalObj> list_0;

		private PhysicalObj physicalObj_0;

		private int int_2;

		private string[] string_0;

		private string[] string_1;

		private string[] string_2;

		private string[] string_3;

		public ThirteenHardFourthBoss()
		{
			
			this.list_0 = new List<PhysicalObj>();
			this.int_2 = 13209;
			this.string_0 = new string[] { "Você se lembra do seu pai?", "Você se lembra de mim?", "Essas pessoas se lembram de quem eu sou?" };
			this.string_1 = new string[] { "Vai ajudar?", "Eu mato você um por um", "Você costumava comer cebolas antes. Agora não pense.", "Você será o primeiro alvo que eu mato", " Você será capaz de lidar com isso? " };
			this.string_2 = new string[] { "Vocês todos têm que morrer", "Não sonham em me ganhar", "Você acha que pode vencer o AD zai bonito?", "O anúncio não vai deixar você passar. Ahahaa ...", "Feijão verde Eu mato todos eles. " };
			this.string_3 = new string[] { "Não aguento mais", "Não quero jogar novamente o jogo antigo. Morra morto", "Eu morro, você também tem que morrer", "Não aguento mais","Mate todos eles. " };
			
		}

		private void method_0()
		{
			int int1 = this.int_1 + this.int_1 / 100 * 10;
			if (int1 >= base.Body.Blood)
			{
				base.Body.Say("Eu não vou te perdoar.", 0, 1000);
				base.Body.PlayMovie("die", 2000, 0);
				base.Body.Die(5000);
				return;
			}
			base.Body.AddBlood(-int1, 1);
			base.Body.Say("Você acha que isso é suficiente para me derrotar?", 0, 1000);
			base.Body.PlayMovie("angry", 1000, 0);
			base.Body.PlayMovie("beatC", 3000, 5000);
			base.Body.CallFuction(new LivingCallBack(this.method_2), 6000);
		}

		

		private void method_10()
		{
			int num = base.Game.Random.Next((int)this.string_0.Length);
			base.Body.Say(this.string_0[num], 0, 1000);
			base.Body.PlayMovie("beatA", 1100, 4000);
			base.Body.CallFuction(new LivingCallBack(this.method_11), 2500);
		}

		private void method_11()
		{
			this.physicalObj_0 = (base.Game as PVEGame).Createlayer(783, 1020, "", "asset.game.ten.tedabiaoji", "", 1, 0);
		}

		private void method_2()
		{
			foreach (Player allLivingPlayer in base.Game.GetAllLivingPlayers())
			{
				allLivingPlayer.SyncAtTime = true;
				allLivingPlayer.Die();
			}
		}

		

		private void method_4()
		{
			int num = base.Game.Random.Next((int)this.string_1.Length);
			base.Body.Say(this.string_1[num], 0, 1000);
			base.Body.PlayMovie("cast", 1000, 0);
			this.m_targer = base.Game.FindRandomPlayer();
			if (this.m_targer != null)
			{
				(base.Game as PVEGame).SendObjectFocus(this.m_targer, 1, 2000, 0);
				base.Body.CallFuction(new LivingCallBack(this.method_5), 2200);
				base.Body.BeatDirect(this.m_targer, "", 3000, 1, 1);
			}
		}

		private void method_5()
		{
			this.list_0.Add((base.Game as PVEGame).Createlayer(this.m_targer.X, this.m_targer.Y, "", "asset.game.ten.danbao", "", 1, 0));
		}

		private void method_6()
		{
			foreach (Player allLivingPlayer in base.Game.GetAllLivingPlayers())
			{
				this.list_0.Add((base.Game as PVEGame).Createlayer(allLivingPlayer.X, allLivingPlayer.Y, "", "asset.game.ten.qunbao", "", 1, 0));
			}
		}

		private void method_7()
		{
			base.Body.CurrentDamagePlus = 1.5f;
			int num = base.Game.Random.Next((int)this.string_2.Length);
			base.Body.Say(this.string_2[num], 0, 1000);
			base.Body.PlayMovie("cast", 1000, 0);
			this.m_targer = base.Game.FindRandomPlayer();
			if (this.m_targer != null)
			{
				(base.Game as PVEGame).SendObjectFocus(this.m_targer, 1, 2000, 0);
				base.Body.CallFuction(new LivingCallBack(this.method_6), 2200);
				base.Body.RangeAttacking(base.Body.X - 10000, base.Body.Y + 10000, "cry", 3000, false);
			}
		}

		private void method_8()
		{
			base.Body.CurrentDamagePlus = 10f;
			base.Body.PlayMovie("beatD", 1000, 2000);
			base.Body.CallFuction(new LivingCallBack(this.method_9), 1800);
			base.Body.RangeAttacking(0, 1427, "cry", 2500, true);
		}

		private void method_9()
		{
			base.Game.RemovePhysicalObj(this.physicalObj_0, true);
			this.physicalObj_0 = null;
		}

		public override void OnAfterTakedFrozen()
		{
			base.OnAfterTakedFrozen();
			if ((int)base.Body.Properties1 == 1)
			{
				base.Body.Properties1 = 0;
				base.Body.PlayMovie("stand", 100, 2000);
				base.Body.SetRelateDemagemRect((base.Body as SimpleBoss).NpcInfo.X, (base.Body as SimpleBoss).NpcInfo.Y, (base.Body as SimpleBoss).NpcInfo.Width, (base.Body as SimpleBoss).NpcInfo.Height);
				base.Body.Config.CanTakeDamage = true;
			}
		}

		public override void OnBeginNewTurn()
		{
			base.OnBeginNewTurn();
			this.m_body.CurrentDamagePlus = 1f;
			this.m_body.CurrentShootMinus = 1f;
			if (this.list_0 != null && this.list_0.Count > 0)
			{
				foreach (PhysicalObj list0 in this.list_0)
				{
					base.Game.RemovePhysicalObj(list0, true);
				}
				this.list_0.Clear();
			}
		}

		public override void OnBeginSelfTurn()
		{
			base.OnBeginSelfTurn();
			if (this.simpleBoss_0 == null)
			{
				SimpleBoss[] simpleBossArray = ((PVEGame)base.Game).FindLivingTurnBossWithID(this.int_2);
				if (simpleBossArray.Length != 0)
				{
					this.simpleBoss_0 = simpleBossArray[0];
				}
			}
			if (this.simpleBoss_0.IsLiving || this.int_1 == 0)
			{
				this.int_1 = this.simpleBoss_0.Blood;
			}
		}

		public override void OnCreated()
		{
			base.OnCreated();
			base.Body.Properties1 = 0;
		}

		public override void OnDie()
		{
			base.OnDie();
		}

		public override void OnStartAttacking()
		{
			base.OnStartAttacking();
			if (this.simpleBoss_0 != null && !this.simpleBoss_0.IsLiving)
			{
				this.method_0();
				return;
			}
			switch (this.int_0)
			{
				case 0:
				{
					this.method_10();
					break;
				}
				case 1:
				{
					this.method_8();
					break;
				}
				case 2:
				{
					this.method_7();
					break;
				}
				case 3:
				{
					this.method_4();
					break;
				}
				
				case 4:
				{
					if ((int)base.Body.Properties1 != 0 || (int)this.simpleBoss_0.Properties1 != 0)
					
					{
						this.method_10();
					}
					this.int_0 = 0;
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