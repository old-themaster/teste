using Game.Logic.Phy.Object;

namespace Game.Logic.Actions
{
	public class LivingBoltMoveAction : BaseAction
	{
		private int m_x;

		private int m_y;

		private string m_action;

		private Living m_living;

		public LivingBoltMoveAction(Living living, int toX, int toY, string action, int delay, int finishTime)
			: base(delay, finishTime)
		{
			m_living = living;
			m_x = toX;
			m_y = toY;
			m_action = action;
		}

		protected override void ExecuteImp(BaseGame game, long tick)
		{
			m_living.SetXY(m_x, m_y);
			m_living.StartMoving();
			game.SendLivingBoltMove(m_living);
			Finish(tick);
		}
	}
}
