// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.UserHideItemHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Base.Packets;

namespace Game.Server.Packets.Client
{
  [PacketHandler(60, "隐藏装备")]
  public class UserHideItemHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      bool hide = packet.ReadBoolean();
      int categoryID = packet.ReadInt();
      switch (categoryID)
      {
        case 13:
          categoryID = 3;
          break;
        case 15:
          categoryID = 4;
          break;
      }
      client.Player.HideEquip(categoryID, hide);
      return 0;
    }
  }
}
