// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.GamePairUpCancelHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.Rooms;

namespace Game.Server.Packets.Client
{
    [PacketHandler(210, "撮合取消")]
    public class GamePairUpCancelHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            GamePlayer player = client.Player;
            if (player.CurrentRoom != null && player.CurrentRoom.BattleServer != null)
            {
                player.CurrentRoom.BattleServer.RemoveRoom(player.CurrentRoom);
                if (player != player.CurrentRoom.Host)
                {
                    player.CurrentRoom.Host.Out.SendMessage(eMessageType.ChatERROR, LanguageMgr.GetTranslation("Game.Server.SceneGames.PairUp.Failed"));
                    RoomMgr.UpdatePlayerState(player, (byte)0);
                }
                           
                    RoomMgr.UpdatePlayerState(player, (byte)2);
            }
            else
                player.CurrentRoom.RemovePlayerUnsafe(player);            
            return 0;
    }
  }
}

