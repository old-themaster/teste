namespace Game.Logic.Actions
{
	public class FocusAction : BaseAction
	{
		private int m_x;

		private int m_y;

		private int m_type;

		public FocusAction(int x, int y, int type, int delay, int finishTime)
			: base(delay, finishTime)
		{
			m_x = x;
			m_y = y;
			m_type = type;
		}

		protected override void ExecuteImp(BaseGame game, long tick)
		{
			game.SendPhysicalObjFocus(m_x, m_y, m_type);
			Finish(tick);
		}
	}
}
