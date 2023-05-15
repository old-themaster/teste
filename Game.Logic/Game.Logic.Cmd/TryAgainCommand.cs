using Game.Base.Packets;
using Game.Logic.Phy.Object;

namespace Game.Logic.Cmd
{
	[GameCommand(119, "关卡失败再试一次")]
	public class TryAgainCommand : ICommandHandler
	{
		public void HandleCommand(BaseGame game, Player player, GSPacketIn packet)
		{
			if (!(game is PVEGame))
			{
				return;
			}
			PVEGame pVEGame = game as PVEGame;
			bool flag = packet.ReadBoolean();
			if (!packet.ReadBoolean())
			{
				return;
			}
			if (flag)
			{
				if (player.PlayerDetail.RemoveMoney(100) > 0)
				{
					pVEGame.WantTryAgain = 1;
					game.SendToAll(packet);
					player.PlayerDetail.LogAddMoney(AddMoneyType.Game, AddMoneyType.Game_TryAgain, player.PlayerDetail.PlayerCharacter.ID, 100, player.PlayerDetail.PlayerCharacter.Money);
				}
				else
				{
					player.PlayerDetail.SendInsufficientMoney(2);
				}
			}
			else
			{
				pVEGame.WantTryAgain = 0;
				game.SendToAll(packet);
			}
			pVEGame.CheckState(0);
		}
	}
}
