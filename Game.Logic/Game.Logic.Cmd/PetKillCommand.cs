using Game.Base.Packets;
using Game.Logic.Phy.Object;

namespace Game.Logic.Cmd
{
    [GameCommand(144, "Pet vurus komutu")]
    public class PetKillCommand : ICommandHandler
    {
        public void HandleCommand(BaseGame game, Player player, GSPacketIn packet)
        {
            if (game.GameState != eGameState.Playing || player.GetSealState())
                return;
            int skillID = packet.ReadInt();
            int type = packet.ReadInt();
            player.PetUseKill(skillID, type);
        }
    }
}
