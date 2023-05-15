using Game.Logic;
using Game.Logic.Phy.Object;
using System;

namespace Game.Logic.Effects
{
	public class LockMoveEffect : AbstractEffect
	{
		private int int_0;

		public LockMoveEffect(int count) : base(eEffectType.LockMoveEffect)
		{
			
			
			this.int_0 = count;
		}

		private void method_0(Living living_0)
		{
			this.int_0--;
			if (this.int_0 < 0)
			{
				this.Stop();
			}
		}

		public override void OnAttached(Living living)
		{
			living.BeginSelfTurn += new LivingEventHandle(this.method_0);
			living.Game.method_69(living, 1000, true);
		}

		public override void OnRemoved(Living living)
		{
			living.BeginSelfTurn -= new LivingEventHandle(this.method_0);
			living.Game.method_69(living, 1000, false);
		}

		public override bool Start(Living living)
		{
			LockMoveEffect ofType = living.EffectList.GetOfType(eEffectType.LockMoveEffect) as LockMoveEffect;
			if (ofType == null)
			{
				return base.Start(living);
			}
			ofType.int_0 = this.int_0;
			return true;
		}
	}
}