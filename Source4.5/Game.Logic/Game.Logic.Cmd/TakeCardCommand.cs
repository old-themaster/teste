using Game.Base.Packets;
using Game.Logic.Phy.Object;

namespace Game.Logic.Cmd
{
	[GameCommand(98, "翻牌")]
	public class TakeCardCommand : ICommandHandler
	{
		public void HandleCommand(BaseGame game, Player player, GSPacketIn packet)
		{
			if (!player.FinishTakeCard && player.CanTakeOut > 0)
			{
				int num = packet.ReadByte();
				if (num < 0 || num > game.Cards.Length)
				{
					game.TakeCard(player);
				}
				else
				{
					game.TakeCard(player, num);
				}
			}
		}
	}
}
