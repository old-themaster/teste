using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.NPC
{
    public class TwelveTankNpc : ABrain
	{
		private int m_attackTurn = 0;

		public int currentCount = 0;

		public int Dander = 0;

		public override void OnBeginSelfTurn()
		{
			base.OnBeginSelfTurn();
		}

		public override void OnBeginNewTurn()
		{
			base.OnBeginNewTurn();
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
				if (current.IsLiving && current.X > 480 && current.X < 1000)
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
				this.PersonalAttack();
				this.m_attackTurn++;
			}
			else
			{
				this.AllAttack();
				this.m_attackTurn = 0;
			}
		}

		public override void OnStopAttacking()
		{
			base.OnStopAttacking();
		}

		private void KillAttack(int fx, int tx)
		{
			this.ChangeDirection(3);
			base.Body.CurrentDamagePlus = 10f;
			base.Body.PlayMovie("stand", 3000, 0);
			base.Body.RangeAttacking(fx, tx, "cry", 5000, null);
		}

		private void AllAttack()
		{
			this.ChangeDirection(3);
			base.Body.CurrentDamagePlus = 0.5f;
			base.Body.PlayMovie("stand", 1000, 0);
			base.Body.RangeAttacking(base.Body.X - 1000, base.Body.X + 1000, "cry", 4000, null);
		}

		private void PersonalAttack()
		{
			this.ChangeDirection(3);
			int x = base.Game.Random.Next(550, 1200);
			int direction = base.Body.Direction;
			base.Body.MoveTo(x, base.Body.Y, "walk", 1000, "", ((SimpleBoss)base.Body).NpcInfo.speed);
			base.Body.ChangeDirection(base.Game.FindlivingbyDir(base.Body), 9000);
		}

		private void ChangeDirection(int count)
		{
			int direction = base.Body.Direction;
			for (int i = 0; i < count; i++)
			{
				base.Body.ChangeDirection(-direction, i * 200 + 100);
				base.Body.ChangeDirection(direction, (i + 1) * 100 + i * 200);
			}
		}
	}
}
