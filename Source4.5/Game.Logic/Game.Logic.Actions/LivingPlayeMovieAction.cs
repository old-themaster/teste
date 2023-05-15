using Game.Logic.Phy.Object;

namespace Game.Logic.Actions
{
	public class LivingPlayeMovieAction : BaseAction
	{
		private Living m_living;

		private string m_action;

		private LivingCallBack m_callBack;

		private int m_movieTime;

		public LivingPlayeMovieAction(Living living, string action, int delay, int movieTime, LivingCallBack callBack)
			: base(delay, movieTime)
		{
			m_living = living;
			m_action = action;
			m_callBack = callBack;
			m_movieTime = movieTime;
		}

		protected override void ExecuteImp(BaseGame game, long tick)
		{
			game.SendLivingPlayMovie(m_living, m_action);
			if (m_callBack != null)
			{
				m_living.CallFuction(m_callBack, m_movieTime);
			}
			Finish(tick);
		}
	}
}
