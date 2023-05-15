using Game.Base.Packets;
using Game.Logic.Phy.Object;

namespace Game.Logic.Cmd
{
	[GameCommand(84, "副武器")]
	public class SecondWeaponCommand : ICommandHandler
	{
		public void HandleCommand(BaseGame game, Player player, GSPacketIn packet)
		{
			player.UseSecondWeapon();
			GSPacketIn gSPacketIn = new GSPacketIn(91);
			gSPacketIn.WriteByte(84);
			gSPacketIn.WriteInt(player.deputyWeaponCount);
			player.PlayerDetail.SendTCP(gSPacketIn);
		}
	}
}
