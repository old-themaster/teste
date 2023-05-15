// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.UserEnterSceneHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.Managers;

namespace Game.Server.Packets.Client
{
  [PacketHandler(16, "Player enter scene.")]
  public class UserEnterSceneHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      switch (packet.ReadInt())
      {
        case 1:
          client.Player.PlayerState = ePlayerState.Manual;
          break;
        case 2:
          client.Player.PlayerState = ePlayerState.Away;
          break;
        case 8:
          client.Player.PlayerState = ePlayerState.Busy;
          break;
        default:
          client.Player.PlayerState = ePlayerState.Manual;
          break;
      }
      if (WorldMgr.HotSpringScene.GetClientFromID(client.Player.PlayerCharacter.ID) != null)
        WorldMgr.HotSpringScene.RemovePlayer(client.Player);
      return 1;
    }
  }
}
