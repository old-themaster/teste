using Game.Logic.Phy.Object;

namespace Game.Logic.Actions
{
	public class WaitPlayerLoadingAction : IAction
	{
		private bool m_isFinished;

		private long m_time;

		public WaitPlayerLoadingAction(BaseGame game, int maxTime)
		{
			m_time = TickHelper.GetTickCount() + maxTime;
			game.GameStarted += game_GameStarted;
		}

		public void Execute(BaseGame game, long tick)
		{
			if (!m_isFinished && tick > m_time && game.GameState == eGameState.Loading)
			{
				if (game.GameState == eGameState.Loading)
				{
					foreach (Player allFightPlayer in game.GetAllFightPlayers())
					{
						if (allFightPlayer.LoadingProcess < 100)
						{
							game.SendPlayerRemove(allFightPlayer);
							game.RemovePlayer(allFightPlayer.PlayerDetail, IsKick: false);
						}
					}
					game.CheckState(0);
				}
				m_isFinished = true;
			}
		}

		private void game_GameStarted(AbstractGame game)
		{
			game.GameStarted -= game_GameStarted;
			m_isFinished = true;
		}

		public bool IsFinished(long tick)
		{
			return m_isFinished;
		}
	}
}
