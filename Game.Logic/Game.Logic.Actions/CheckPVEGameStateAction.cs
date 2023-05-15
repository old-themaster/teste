namespace Game.Logic.Actions
{
	public class CheckPVEGameStateAction : IAction
	{
		private long m_time;

		private bool m_isFinished;

		public CheckPVEGameStateAction(int delay)
		{
			m_time = TickHelper.GetTickCount() + delay;
			m_isFinished = false;
		}

		public void Execute(BaseGame game, long tick)
		{
			if (m_time > tick || game.GetWaitTimer() >= tick)
			{
				return;
			}
			PVEGame pVEGame = game as PVEGame;
			if (pVEGame != null)
			{
				switch (pVEGame.GameState)
				{
				case eGameState.Inited:
					pVEGame.Prepare();
					break;
				case eGameState.Prepared:
					pVEGame.PrepareNewSession();
					break;
				case eGameState.Loading:
					if (pVEGame.IsAllComplete())
					{
						pVEGame.StartGame();
						break;
					}
					game.SendLoading();
					game.WaitTime(1000);
					break;
				case eGameState.GameStart:
					if (game.RoomType == eRoomType.FightLab)
					{
						if (game.CurrentActionCount <= 1)
						{
							pVEGame.PrepareFightingLivings();
						}
					}
					else
					{
						pVEGame.PrepareNewGame();
					}
					break;
				case eGameState.Playing:
					if ((pVEGame.CurrentLiving != null && pVEGame.CurrentLiving.IsAttacking) || game.CurrentActionCount > 1)
					{
						break;
					}
					if (pVEGame.CanGameOver())
					{
						if (pVEGame.IsLabyrinth() && pVEGame.CanEnterGate)
						{
							pVEGame.GameOverMovie();
						}
						else if (pVEGame.CurrentActionCount <= 1)
						{
							pVEGame.GameOver();
						}
					}
					else
					{
						pVEGame.NextTurn();
					}
					break;
				case eGameState.PrepareGameOver:
					if (pVEGame.CurrentActionCount <= 1)
					{
						pVEGame.GameOver();
					}
					break;
				case eGameState.GameOver:
					if (!pVEGame.HasNextSession())
					{
						pVEGame.GameOverAllSession();
					}
					else
					{
						pVEGame.PrepareNewSession();
					}
					break;
				case eGameState.SessionPrepared:
					if (pVEGame.CanStartNewSession())
					{
						pVEGame.SetupStyle();
						pVEGame.StartLoading();
					}
					else
					{
						game.WaitTime(1000);
					}
					break;
				case eGameState.ALLSessionStopped:
					if (pVEGame.PlayerCount != 0 && pVEGame.WantTryAgain != 0)
					{
						if (pVEGame.WantTryAgain == 1)
						{
							pVEGame.ShowDragonLairCard();
							pVEGame.PrepareNewSession();
						}
						else if (pVEGame.WantTryAgain == 2)
						{
							pVEGame.SessionId--;
							pVEGame.PrepareNewSession();
						}
						else
						{
							game.WaitTime(1000);
						}
					}
					else
					{
						pVEGame.Stop();
					}
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
