using Game.Logic.PetEffects.ContinueElement;
using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects.Element.Actives
{
	public class CASE1042A : BasePetEffect
	{
		private int int_0;

		private int int_1;

		private int int_2;

		private int int_3;

		private int int_4;

		private int int_5;

		private int int_6;

		public CASE1042A(int count, int probability, int type, int skillId, int delay, string elementID) : base(ePetEffectType.CASE1042A, elementID)
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
			CASE1042A aE = living.PetEffectList.GetOfType(ePetEffectType.CASE1042A) as CASE1042A;
			if (aE != null)
			{
				aE.int_2 = ((int_2 > aE.int_2) ? int_2 : aE.int_2);
				return true;
			}
			return base.Start(living);
		}

		protected override void OnAttachedToPlayer(Player player)
		{
			player.PlayerBuffSkillPet += method_0;
		}

		protected override void OnRemovedFromPlayer(Player player)
		{
			player.PlayerBuffSkillPet -= method_0;
		}

		private void method_0(Player player_0)
		{
			if (player_0.PetEffects.CurrentUseSkill == int_5)
			{
				//player_0.Game.method_10(player_0, base.ElementInfo, bool_1: true);
				player_0.AddPetEffect(new CASE1042A(2, int_2, int_0, int_5, int_3, base.ElementInfo.ToString()), 0);
			}
		}
	}
}
