using Game.Base.Packets;
using Game.Logic.Phy.Object;

namespace Game.Logic.Cmd
{
	[GameCommand(23, "触发关卡事件")]
	public class MissionEventCommand : ICommandHandler
	{
		public void HandleCommand(BaseGame game, Player player, GSPacketIn packet)
		{
			if (game.GameState == eGameState.Playing)
			{
				player.OnMissionEventHandle(packet);
			}
		}
	}
}
