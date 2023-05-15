using Game.Logic.Phy.Object;

namespace Game.Logic.Actions
{
	public class LivingRotateTurnAction : BaseAction
	{
		private string m_endPlay;

		private Player m_player;

		private int m_rotation;

		private int m_speed;

		public LivingRotateTurnAction(Player player, int rotation, int speed, string endPlay, int delay)
			: base(0, delay)
		{
			m_player = player;
			m_rotation = rotation;
			m_speed = speed;
			m_endPlay = endPlay;
		}

		protected override void ExecuteImp(BaseGame game, long tick)
		{
			game.SendLivingTurnRotation(m_player, m_rotation, m_speed, m_endPlay);
			Finish(tick);
		}
	}
}
