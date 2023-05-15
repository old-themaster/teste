using Game.Logic.Effects;
using Game.Logic.Phy.Object;
using SqlDataProvider.Data;

namespace Game.Logic.Spells.NormalSpell
{
	[SpellAttibute(3)]
	public class HideSpell : ISpellHandler
	{
		public void Execute(BaseGame game, Player player, ItemTemplateInfo item)
		{
			switch (item.Property2)
			{
			case 0:
				if (!player.IsLiving)
				{
					if (game.CurrentLiving != null && game.CurrentLiving is Player && game.CurrentLiving.Team == player.Team)
					{
						new HideEffect(item.Property3).Start(game.CurrentLiving);
					}
				}
				else
				{
					new HideEffect(item.Property3).Start(player);
				}
				break;
			case 1:
				foreach (Player allFightPlayer in player.Game.GetAllFightPlayers())
				{
					if (allFightPlayer.IsLiving && allFightPlayer.Team == player.Team)
					{
						new HideEffect(item.Property3).Start(allFightPlayer);
					}
				}
				break;
			}
		}
	}
}
