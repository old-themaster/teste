using log4net;
using System.Reflection;

namespace Game.Logic.Actions
{
	public class CheckPVPGameStateAction : IAction
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private bool m_isFinished;

		private long m_tick;

		public CheckPVPGameStateAction(int delay)
		{
			m_tick += TickHelper.GetTickCount() + delay;
		}

		public void Execute(BaseGame game, long tick)
		{
			if (m_tick > tick)
			{
				return;
			}
			PVPGame pVPGame = game as PVPGame;
			if (pVPGame != null)
			{
				switch (game.GameState)
				{
				case eGameState.Inited:
					pVPGame.Prepare();
					break;
				case eGameState.Prepared:
					pVPGame.StartLoading();
					break;
				case eGameState.Loading:
					if (pVPGame.IsAllComplete())
					{
						pVPGame.StartGame();
					}
					game.SendLoading();
					break;
					case eGameState.Playing:
						if (pVPGame.CurrentPlayer != null && pVPGame.CurrentPlayer.IsAttacking)
						{
							break;
						}
						if (pVPGame.CanGameOver())
						{
							if (pVPGame.EntertainmentMode())
							{
								pVPGame.GameOverEntertainmentMode();
							}
							else
							{
								pVPGame.GameOver();
							}
						}
						else
						{
							pVPGame.NextTurn();
						}
						break;
					case eGameState.GameOver:
						pVPGame.Stop();
						break;
				}
			}
			m_isFinished = true;
		}

		public bool IsFinished(long tick)
		{
			return m_isFinished;
		}
	}
}
