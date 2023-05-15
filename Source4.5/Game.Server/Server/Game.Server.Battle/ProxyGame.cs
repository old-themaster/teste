using Game.Base;
using Game.Base.Packets;
using Game.Logic;

namespace Game.Server.Battle
{
    public class ProxyGame : AbstractGame
    {
        private FightServerConnector fightServerConnector_0;

        public ProxyGame(
          int id,
          FightServerConnector fightServer,
          eRoomType roomType,
          eGameType gameType,
          int timeType)
          : base(id, roomType, gameType, timeType)
        {
            this.fightServerConnector_0 = fightServer;
            this.fightServerConnector_0.Disconnected += new ClientEventHandle(this.method_0);
        }

        private void method_0(BaseClient baseClient_0)
        {
            this.Stop();
        }

        public override void ProcessData(GSPacketIn pkg)
        {
            this.fightServerConnector_0.SendToGame(this.Id, pkg);
        }
    }
}
