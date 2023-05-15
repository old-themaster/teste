// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.BaglockedHandle
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Base.Packets;
using System;

namespace Game.Server.Packets.Client
{
  [PacketHandler(403, "二级密码")]
  public class BaglockedHandle : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int num = (int) packet.ReadByte();
      GSPacketIn packet1 = new GSPacketIn((short) 403, client.Player.PlayerCharacter.ID);
      if (num == 5)
      {
        packet1.WriteByte((byte) 5);
        packet1.WriteBoolean(true);
        client.Out.SendTCP(packet1);
      }
      else
        Console.WriteLine("BaglockedPackageType." + (object) (BaglockedPackageType) num);
      return 0;
    }
  }
}
