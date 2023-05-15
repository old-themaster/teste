using Game.Logic.Phy.Object;

namespace Game.Logic.Actions
{
	public class GameShowCardAction : BaseAction
	{
		private PVEGame m_game;

		public GameShowCardAction(PVEGame game, int delay, int finishTime)
			: base(delay, finishTime)
		{
			m_game = game;
		}

		protected override void ExecuteImp(BaseGame game, long tick)
		{
			foreach (Player allFightPlayer in m_game.GetAllFightPlayers())
			{
				if (allFightPlayer.IsActive && allFightPlayer.CanTakeOut > 0)
				{
					allFightPlayer.HasPaymentTakeCard = true;
					int canTakeOut = allFightPlayer.CanTakeOut;
					for (int i = 0; i < canTakeOut; i++)
					{
						m_game.TakeCard(allFightPlayer);
					}
				}
			}
			m_game.SendShowCards();
			base.ExecuteImp(game, tick);
		}
	}
}
