using Game.Logic.Phy.Maths;
using Game.Logic.Phy.Object;
using System.Drawing;

namespace Game.Logic.Actions
{
	public class GhostMoveAction : BaseAction
	{
		private bool m_isSend;

		private Player m_player;

		private Point m_target;

		private Point m_v;

		public GhostMoveAction(Player player, Point target)
			: base(0, 1000)
		{
			m_player = player;
			m_target = target;
			m_v = new Point(target.X - m_player.X, target.Y - m_player.Y);
			m_v.Normalize(2);
		}

		protected override void ExecuteImp(BaseGame game, long tick)
		{
			if (!m_isSend)
			{
				m_isSend = true;
				game.SendPlayerMove(m_player, 2, m_target.X, m_target.Y, (byte)((m_v.X > 0) ? 1 : byte.MaxValue), m_player.IsLiving, sendExcept: true);
			}
			if (m_target.Distance(m_player.X, m_player.Y) > 2.0)
			{
				m_player.SetXY(m_player.X + m_v.X, m_player.Y + m_v.Y);
			}
			else
			{
				m_player.SetXY(m_target.X, m_target.Y);
				Finish(tick);
			}
			m_player.SetXY(m_target.X, m_target.Y);
		}
	}
}
