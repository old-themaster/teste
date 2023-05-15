using Game.Base.Packets;
using Game.Logic.Phy.Object;

namespace Game.Logic.Cmd
{
	[GameCommand(9, "开始移动")]
	public class MoveStartCommand : ICommandHandler
	{
		public void HandleCommand(BaseGame game, Player player, GSPacketIn packet)
		{
			if (player.IsAttacking)
			{
				packet.ReadBoolean();
				byte b = packet.ReadByte();
				int x = packet.ReadInt();
				int y = packet.ReadInt();
				byte dir = packet.ReadByte();
				packet.ReadBoolean();
				packet.ReadShort();
				if ((uint)b <= 1u)
				{
					player.SetXY(x, y);
					player.StartMoving();
					game.SendPlayerMove(player, b, player.X, player.Y, dir, player.IsLiving, sendExcept: true);
				}
			}
		}
	}
}
