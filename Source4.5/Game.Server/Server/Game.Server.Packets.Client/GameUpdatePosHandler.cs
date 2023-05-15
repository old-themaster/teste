// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.GameUpdatePosHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Base.Packets;

namespace Game.Server.Packets.Client
{
  [PacketHandler(100, "客户端日记")]
  public class GameUpdatePosHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      if (client.Player.CurrentRoom != null && client.Player == client.Player.CurrentRoom.Host)
      {
        byte num1 = packet.ReadByte();
        int num2 = packet.ReadInt();
        packet.ReadBoolean();
        packet.ReadInt();
        if ((num1 == (byte) 8 || num1 == (byte) 9) && num2 == -1)
        {
          client.Player.SendMessage("Você deve alcançar o nível 40 para abrir salas com modo espectador");
          return 0;
        }
      }
      return 0;
    }
  }
}
