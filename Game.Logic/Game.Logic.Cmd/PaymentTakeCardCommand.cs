using Bussiness;
using Game.Base.Packets;
using Game.Logic.Phy.Object;

namespace Game.Logic.Cmd
{
	[GameCommand(114, "付费翻牌")]
	public class PaymentTakeCardCommand : ICommandHandler
	{
		public void HandleCommand(BaseGame game, Player player, GSPacketIn packet)
		{
			if (player.HasPaymentTakeCard)
			{
				return;
			}
			bool flag;
			if (player.GetFightBuffByType(BuffType.Card_Get) != null && !game.IsSpecialPVE() && player.PlayerDetail.UsePayBuff(BuffType.Card_Get))
			{
				flag = true;
			}
			else
			{
				int value = (player.PlayerDetail.PlayerCharacter.typeVIP > 0) ? 437 : 486;
				flag = (player.PlayerDetail.RemoveMoney(value) > 0);
			}
			if (flag)
			{
				int num = packet.ReadByte();
				player.CanTakeOut++;
				player.FinishTakeCard = false;
				player.HasPaymentTakeCard = true;
				if (num >= 0 && num < game.Cards.Length)
				{
					game.TakeCard(player, num);
				}
				else
				{
					game.TakeCard(player);
				}
			}
			else
			{
				player.PlayerDetail.SendInsufficientMoney(1);
			}
		}
	}
}
