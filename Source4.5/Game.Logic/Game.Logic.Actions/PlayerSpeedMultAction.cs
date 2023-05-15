using Game.Logic.Phy.Object;
using System.Drawing;

namespace Game.Logic.Actions
{
	public class PlayerSpeedMultAction : BaseAction
	{
		private Point point_0;

		private Player player_0;

		private bool bool_0;

		public PlayerSpeedMultAction(Player player, Point target, int delay)
			: base(0, delay)
		{
			player_0 = player;
			point_0 = target;
			bool_0 = false;
		}

		protected override void ExecuteImp(BaseGame game, long tick)
		{
			int num = 4;
			if (!bool_0)
			{
				bool_0 = true;
				game.SendPlayerMove(player_0, num, point_0.X, point_0.Y, (byte)((point_0.X > player_0.X) ? 1 : byte.MaxValue), player_0.IsLiving, sendExcept: false);
			}
			if (player_0.Distance(point_0) > (double)player_0.StepX && point_0.X != player_0.X)
			{
				player_0.Direction = ((point_0.X > player_0.X) ? 1 : (-1));
				Point left = player_0.getNextWalkPoint(player_0.Direction);
				if (left == Point.Empty)
				{
					int num2 = player_0.X + player_0.Direction * player_0.MOVE_SPEED;
					left = player_0.FindYLineNotEmptyPointDown(num2, player_0.Y - player_0.StepY);
					if (left == Point.Empty)
					{
						num = 1;
						left = new Point(num2, point_0.Y);
					}
				}
				player_0.SetXY(left.X, left.Y);
				if ((player_0.Direction > 0 && left.X >= point_0.X) || (player_0.Direction < 0 && left.X <= point_0.X) || num == 1)
				{
					player_0.StartMoving();
					Finish(tick);
				}
			}
			else
			{
				Finish(tick);
			}
		}
	}
}
