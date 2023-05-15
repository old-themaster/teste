using Game.Logic.Phy.Object;
using SqlDataProvider.Data;

namespace Game.Logic.Spells.NormalSpell
{
	[SpellAttibute(1)]
	public class AddLifeSpell : ISpellHandler
	{
		public void Execute(BaseGame game, Player player, ItemTemplateInfo item)
		{
			switch (item.Property2)
			{
			case 0:
			{
				int num = item.Property3;
				if (!player.IsLiving)
				{
					if (game.CurrentLiving != null && game.CurrentLiving is Player && game.CurrentLiving.Team == player.Team)
					{
						game.CurrentLiving.AddBlood(num);
					}
					break;
				}
				if (player.FightBuffers.ConsortionAddSpellCount > 0)
				{
					num += player.FightBuffers.ConsortionAddSpellCount;
				}
				player.AddBlood(num);
				break;
			}
			case 1:
				foreach (Player allFightPlayer in player.Game.GetAllFightPlayers())
				{
					if (allFightPlayer.IsLiving && allFightPlayer.Team == player.Team)
					{
						allFightPlayer.AddBlood(item.Property3);
					}
				}
				break;
			}
		}
	}
}
