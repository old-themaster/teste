using Game.Logic.Phy.Object;
using SqlDataProvider.Data;

namespace Game.Logic.Spells.FightingSpell
{
	[SpellAttibute(8)]
	public class BreachDefenceSpell : ISpellHandler
	{
		public void Execute(BaseGame game, Player player, ItemTemplateInfo item)
		{
			if (player.IsLiving)
			{
				player.IgnoreArmor = true;
			}
			else if (game.CurrentLiving != null && game.CurrentLiving is Player && game.CurrentLiving.Team == player.Team)
			{
				game.CurrentLiving.IgnoreArmor = true;
			}
		}
	}
}
