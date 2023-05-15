
using Game.Base.Packets;
using Bussiness;
using Game.Server.Managers;
using System;

namespace Game.Server.Packets.Client.Roulette
{
    [PacketHandler(128, "Start Roulette")]
    public class StartTurn : IPacketHandler
    {
        public GamePlayer Player
        {
            get
            {
                return this.m_player;
            }
        }
        protected GamePlayer m_player;
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            if (packet.ReadInt() == 1)

            {
                string[] turnSelected = GameProperties.LeftRouterRateData.Split(new char[] { '|' });
                string reward = "0";
                switch (new Random().Next(1, 5))
                {
                    case 1:
                        reward = turnSelected[1];
                        break;
                    case 2:
                        reward = turnSelected[2];
                        break;
                    case 3:
                        reward = turnSelected[3];
                        break;
                    case 4:
                        reward = turnSelected[4];
                        break;
                    case 5:
                        reward = turnSelected[5];
                        break;

                    case 0:
                        reward = turnSelected[0];
                        break;
                }
                if (reward != "0")
                {
                    if (reward == turnSelected[0] || reward == turnSelected[1] || reward == turnSelected[2])
                        client.Player.SendMessage("Parabéns por obter o multiplicador " + (object)reward + " na Roleta da Sorte!");
                    else if (reward == turnSelected[3])
                    {
                        client.Player.SendMessage("Parabéns! Você receberá " + (object)reward + "  vezes mais dobro na roleta. Os ganhos serão enviados para o seu e-mail ou para sua Mochila.");
                    }
                    else
                    {
                        GamePlayer[] players = WorldMgr.GetAllPlayers();
                        for (int i = 0; i < players.Length; i++) ;
                        foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
                            allPlayer.Client.Player.SendMessage("Parabéns ao jogador [" + client.Player.PlayerCharacter.NickName + "]! A roleta da sorte está do lado dele, agora ele receberá " + (object)reward + "   vezes mais de dobro na sua conta. ");
                    }

                }
                this.SendSocketRoulette(client, reward);
            }
            return 0;
        }
        private void SendSocketRoulette(GameClient client, string turnSelected)
        {
            GSPacketIn packet = new GSPacketIn((short)163);
            packet.WriteInt(1);
            packet.WriteString(turnSelected);
            client.Out.SendTCP(packet);
        }
    }
}
