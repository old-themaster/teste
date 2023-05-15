using Game.Base.Packets;
using Game.Server.GameObjects;
using System;

namespace Game.Server.GameRoom.Handle
{
    public interface IGameRoomCommandHadler
    {
        bool CommandHandler(GamePlayer Player, GSPacketIn packet);
    }
}
