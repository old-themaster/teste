using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace Game.Server.GameServerScript.AI.NPC
{
    public class TwelveTerrorFlyThirdBoss : ABrain
	{
		private int int_0;

		public int currentCount;

		public int Dander;

		public TwelveTerrorFlyThirdBoss()
		{
			
			
		}

		private void method_0(int int_1, int int_2)
		{
			base.Body.CurrentDamagePlus = 1000f;
			base.Body.PlayMovie("beatA", 1000, 0);
			base.Body.RangeAttacking(int_1, int_2, "cry", 4000, null);
		}

		private void method_1()
		{
			base.Body.MoveTo(base.Game.Random.Next(641, 1110), 781, "walk", 500, "", 12, new LivingCallBack(this.method_2));
		}

		private void method_2()
		{
			base.Body.CurrentDamagePlus = 0.5f;
			base.Body.PlayMovie("beatA", 1000, 0);
			base.Body.CallFuction(new LivingCallBack(this.method_6), 2000);
		}

		private void method_3()
		{
			base.Body.CurrentDamagePlus = 0.8f;
			base.Body.PlayMovie("beatB", 1000, 0);
			base.Body.CallFuction(new LivingCallBack(this.method_6), 4000);
		}

		private void method_4()
		{
			base.Body.CurrentDamagePlus = 1.1f;
			base.Body.PlayMovie("beatD", 1000, 0);
			base.Body.CallFuction(new LivingCallBack(this.method_6), 3500);
		}

		private void method_5()
		{
			base.Body.CurrentDamagePlus = 1.1f;
			base.Body.PlayMovie("beatC", 1000, 0);
			base.Body.CallFuction(new LivingCallBack(this.method_6), 3500);
		}

		private void method_6()
		{
			base.Body.RangeAttacking(0, base.Game.Map.Info.ForegroundWidth + 1, "cry", 0, null);
		}

		public override void OnBeginNewTurn()
		{
			base.OnBeginNewTurn();
			base.Body.CurrentDamagePlus = 1f;
			base.Body.CurrentShootMinus = 1f;
			base.Body.SetRect(((SimpleBoss)base.Body).NpcInfo.X, ((SimpleBoss)base.Body).NpcInfo.Y, ((SimpleBoss)base.Body).NpcInfo.Width, ((SimpleBoss)base.Body).NpcInfo.Height);
			if (base.Body.Direction == -1)
			{
				base.Body.SetRect(((SimpleBoss)base.Body).NpcInfo.X, ((SimpleBoss)base.Body).NpcInfo.Y, ((SimpleBoss)base.Body).NpcInfo.Width, ((SimpleBoss)base.Body).NpcInfo.Height);
				return;
			}
			base.Body.SetRect(-((SimpleBoss)base.Body).NpcInfo.X - ((SimpleBoss)base.Body).NpcInfo.Width, ((SimpleBoss)base.Body).NpcInfo.Y, ((SimpleBoss)base.Body).NpcInfo.Width, ((SimpleBoss)base.Body).NpcInfo.Height);
		}

		public override void OnBeginSelfTurn()
		{
			base.OnBeginSelfTurn();
		}

		public override void OnCreated()
		{
			base.OnCreated();
		}

		public override void OnStartAttacking()
		{
			base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
			int num = 0;
			foreach (Player allFightPlayer in base.Game.GetAllFightPlayers())
			{
				if (!allFightPlayer.IsLiving || allFightPlayer.X <= 480 || allFightPlayer.X >= 1000)
				{
					continue;
				}
				int num1 = (int)base.Body.Distance(allFightPlayer.X, allFightPlayer.Y);
				if (num1 <= num)
				{
					continue;
				}
				num = num1;
			}
			if (this.int_0 == 0)
			{
				this.method_2();
				this.int_0++;
				return;
			}
			if (this.int_0 == 1)
			{
				this.method_3();
				this.int_0++;
				return;
			}
			if (this.int_0 != 2)
			{
				this.method_5();
				this.int_0 = 0;
				return;
			}
			this.method_4();
			this.int_0++;
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}
	}
}