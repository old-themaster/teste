using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.ContinueElement
{
	public class CASE1046 : BasePetEffect
	{
		private int int_0;

		private int int_1;

		private int int_2;

		private int int_3;

		private int int_4;

		private int int_5;

		private int int_6;

		public CASE1046(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1046, elementID)
		{


			int_1 = count;
			int_4 = count;
			int_2 = ((probability == -1) ? 10000 : probability);
			int_0 = type;
			int_3 = delay;
			int_5 = skillId;
		}

		public override bool Start(Living living)
		{
			CASE1046 cE = living.PetEffectList.GetOfType(ePetEffectType.CASE1046) as CASE1046;
			if (cE != null)
			{
				cE.int_2 = ((int_2 > cE.int_2) ? int_2 : cE.int_2);
				return true;
			}
			return base.Start(living);
		}

		protected override void OnAttachedToPlayer(Player player)
		{
			player.BeginSelfTurn += RmPreBbwgf5;
			if (int_6 == 0)
			{
				int_6 = 100;
				player.BaseGuard += int_6;
			}
			player.PlayerClearBuffSkillPet += method_0;
		}

		private void method_0(Player player_0)
		{
			Stop();
		}

		private void RmPreBbwgf5(Living living_0)
		{
			int_1--;
			if (int_1 < 0)
			{
				Stop();
			}
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			//player.Game.method_10(player, base.ElementInfo, bool_1: false);
			player.BaseGuard -= int_6;
			int_6 = 0;
			player.BeginSelfTurn -= RmPreBbwgf5;
		}
	}
}
