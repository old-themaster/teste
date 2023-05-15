// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.GameUserStartHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Logic;
using Game.Server.Rooms;

namespace Game.Server.Packets.Client
{
  [PacketHandler(82, "游戏开始")]
  public class GameUserStartHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int num = packet.ReadBoolean() ? 1 : 0;
      BaseRoom currentRoom = client.Player.CurrentRoom;
      if (num != 0 && currentRoom != null)
      {
        if (currentRoom.RoomType == eRoomType.FightLab && !client.Player.IsFightLabPermission(currentRoom.MapId, currentRoom.HardLevel))
        {
          client.Player.SendMessage(LanguageMgr.GetTranslation("GameUserStartHandler.level"));
          return 0;
        }
        RoomMgr.StartGameMission(currentRoom);
      }
      return 0;
    }
  }
}
