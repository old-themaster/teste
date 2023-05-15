using System.Drawing;

namespace Game.Logic.Phy.Object
{
	public class Ball : PhysicalObj
	{
		private int _liveCount;

		public int LiveCount
		{
			get
			{
				return _liveCount;
			}
			set
			{
				_liveCount = value;
			}
		}

		public new int Type => 2;

		public Ball(int id, string action)
			: base(id, "", "asset.game.six.ball", action, 1, 1, 0)
		{
			m_rect = new Rectangle(-15, -15, 30, 30);
		}

		public Ball(int id, string name, string defaultAction, int scale, int rotation)
			: base(id, name, "asset.game.six.ball", defaultAction, scale, rotation, 0)
		{
			m_rect = new Rectangle(-30, -30, 60, 60);
		}

		public override void CollidedByObject(Physics phy)
		{
			if (phy is SimpleBomb)
			{
				(phy as SimpleBomb).Owner.PickPhy(this);
			}
		}
	}
}
