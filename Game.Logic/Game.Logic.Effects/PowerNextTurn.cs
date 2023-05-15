using Game.Logic;
using Game.Logic.Phy.Object;
using System;

namespace Game.Logic.Effects
{
	public class PowerNextTurn : AbstractEffect
	{
		private int int_0;

		private int int_1;

		public PowerNextTurn(int count, int damage) : base(eEffectType.PowerNextTurn)
		{
			
			
			this.int_0 = count;
			this.int_1 = damage;
		}

		private void method_0(Living living_0)
		{
			this.int_0--;
			if (this.int_0 != 0)
			{
				if (this.int_0 < 0)
				{
					this.Stop();
				}
				return;
			}
			living_0.CurrentDamagePlus += (float)(this.int_1 / 100);
			living_0.SetNiutou(true);
		}

		public override void OnAttached(Living living)
		{
			living.BeginSelfTurn += new LivingEventHandle(this.method_0);
		}

		public override void OnRemoved(Living living)
		{
			living.BeginSelfTurn -= new LivingEventHandle(this.method_0);
			living.SetNiutou(false);
		}

		public override bool Start(Living living)
		{
			PowerNextTurn ofType = living.EffectList.GetOfType(eEffectType.PowerNextTurn) as PowerNextTurn;
			if (ofType == null)
			{
				return base.Start(living);
			}
			ofType.int_0 = this.int_0;
			return true;
		}
	}
}