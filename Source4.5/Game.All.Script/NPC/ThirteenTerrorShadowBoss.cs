using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System.Collections.Generic;

namespace Game.Server.GameServerScript.AI.NPC
{
    public class ThirteenTerrorShadowBoss : ABrain
	{
		private int int_0;

		private SimpleBoss simpleBoss_0;

		protected Player m_targer;

		private List<PhysicalObj> list_0;

		private PhysicalObj physicalObj_0;

		private int VfxsmMwwZaa;

		private string[] string_0;

		private string[] string_1;

		private string[] string_2;

		private string[] string_3;

		private int rBksResHgyc;

		public ThirteenTerrorShadowBoss()
		{
			
			this.list_0 = new List<PhysicalObj>();
			this.VfxsmMwwZaa = 13308;
			this.string_0 = new string[] { "Você sente falta do seu pai?", "Você se lembra de mim?", "Esses jogadores de xadrez lembram quem somos?" };
			this.string_1 = new string[] { "Será que vai apoiá-lo?", "Eu vou matá-los um por um", "Você costumava nos comer cebolas antes. Agora não tente.", "Você será o primeiro alvo que eu mato", "Você será capaz de apoiá-lo?" };
			this.string_2 = new string[] { "Vocês deve morrer", "Não sonhe em ganhar contra mim", "Você acha que pode vencer o AD zai bonito?", "Ad éo cho ngươi qua đâu. Ahahaa...", "Feijão mungo matamos todos eles" };
			this.string_3 = new string[] { "Não aguento mais", "Em vez de jogar da maneira antiga. Viga morta", "Se eu morrer, você deve morrer.", "Não suporto se a bandeira estiver sinalizada", "Chega de brincadeira. Todos mortos. Mate todos eles" };
			
		}

		private void method_0()
		{
			int num = this.rBksResHgyc + this.rBksResHgyc / 100 * 10;
			if (num >= base.Body.Blood)
			{
				base.Body.Say("Não te perdoarei", 0, 1000);
				base.Body.PlayMovie("die", 2000, 0);
				base.Body.Die(5000);
				return;
			}
			base.Body.AddBlood(-num, 1);
			base.Body.Say("Brain te perdoarei", 0, 1000);
			base.Body.PlayMovie("angry", 1000, 0);
			base.Body.PlayMovie("beatC", 3000, 5000);
			base.Body.CallFuction(new LivingCallBack(this.method_2), 6000);
		}

		

		private void method_10()
		{
			this.physicalObj_0 = (base.Game as PVEGame).Createlayer(1212, 1020, "", "asset.game.ten.tedabiaoji", "", 1, 0);
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
			this.list_0.Add((base.Game as PVEGame).Createlayer(this.m_targer.X, this.m_targer.Y, "", "asset.game.ten.danbao", "", 1, 0));
		}

		private void method_5()
		{
			foreach (Player allLivingPlayer in base.Game.GetAllLivingPlayers())
			{
				this.list_0.Add((base.Game as PVEGame).Createlayer(allLivingPlayer.X, allLivingPlayer.Y, "", "asset.game.ten.qunbao", "", 1, 0));
			}
		}

		private void method_6()
		{
			base.Body.CurrentDamagePlus = 1.5f;
			int num = base.Game.Random.Next((int)this.string_2.Length);
			base.Body.Say(this.string_2[num], 0, 1000);
			base.Body.PlayMovie("cast", 1000, 0);
			this.m_targer = base.Game.FindRandomPlayer();
			if (this.m_targer != null)
			{
				(base.Game as PVEGame).SendObjectFocus(this.m_targer, 1, 2000, 0);
				base.Body.CallFuction(new LivingCallBack(this.method_5), 2200);
				base.Body.RangeAttacking(base.Body.X - 10000, base.Body.Y + 10000, "cry", 3000, false);
			}
		}

		private void method_7()
		{
			base.Body.CurrentDamagePlus = 10f;
			base.Body.PlayMovie("beatD", 1000, 2000);
			base.Body.CallFuction(new LivingCallBack(this.method_8), 1800);
			base.Body.RangeAttacking(568, 10000, "cry", 2500, true);
		}

		private void method_8()
		{
			base.Game.RemovePhysicalObj(this.physicalObj_0, true);
			this.physicalObj_0 = null;
		}

		private void method_9()
		{
			int num = base.Game.Random.Next((int)this.string_0.Length);
			base.Body.Say(this.string_0[num], 0, 1000);
			base.Body.PlayMovie("beatA", 1100, 4000);
			base.Body.CallFuction(new LivingCallBack(this.method_10), 2500);
		}

		public override void OnAfterTakedFrozen()
		{
			base.OnAfterTakedFrozen();
			base.Body.Properties1 = 0;
			base.Body.PlayMovie("stand", 100, 2000);
			base.Body.SetRelateDemagemRect((base.Body as SimpleBoss).NpcInfo.X, (base.Body as SimpleBoss).NpcInfo.Y, (base.Body as SimpleBoss).NpcInfo.Width, (base.Body as SimpleBoss).NpcInfo.Height);
			base.Body.Config.CanTakeDamage = true;
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
				SimpleBoss[] simpleBossArray = ((PVEGame)base.Game).FindLivingTurnBossWithID(this.VfxsmMwwZaa);
				if (simpleBossArray.Length != 0)
				{
					this.simpleBoss_0 = simpleBossArray[0];
				}
				simpleBossArray = ((PVEGame)base.Game).FindLivingTurnBossWithID(this.VfxsmMwwZaa + 100);
				if (simpleBossArray.Length != 0)
				{
					this.simpleBoss_0 = simpleBossArray[0];
				}
			}
			if (this.simpleBoss_0.IsLiving || this.rBksResHgyc == 0)
			{
				this.rBksResHgyc = this.simpleBoss_0.Blood;
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
			if ((int)base.Body.Properties1 == 1)
			{
				return;
			}
			switch (this.int_0)
			{
				
				case 1:
				{
					this.method_6();
					break;
				}
				case 2:
				{
					this.method_9();
					break;
				}
				case 3:
				{
					this.method_7();
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