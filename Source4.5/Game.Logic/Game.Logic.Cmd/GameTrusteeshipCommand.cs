using Game.Base.Packets;
using Game.Logic.Phy.Object;

namespace Game.Logic.Cmd
{
	[GameCommand(149, "使用道具")]
	public class GameTrusteeshipCommand : ICommandHandler
	{
		public void HandleCommand(BaseGame game, Player player, GSPacketIn packet)
		{
			if (game.GameState == eGameState.Playing && !player.GetSealState())
			{
				packet.ReadBoolean();
			}
		}
	}
}
