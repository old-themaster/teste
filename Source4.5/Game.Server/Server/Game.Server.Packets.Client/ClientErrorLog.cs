// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.ClientErrorLog
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Base.Packets;
using log4net;

namespace Game.Server.Packets.Client
{
  [PacketHandler(8, "客户端日记")]
  public class ClientErrorLog : IPacketHandler
  {
    public static readonly ILog log = LogManager.GetLogger("FlashErrorLogger");

    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      string str = "Client log: " + packet.ReadString();
      ClientErrorLog.log.Error((object) str);
      return 0;
    }
  }
}
