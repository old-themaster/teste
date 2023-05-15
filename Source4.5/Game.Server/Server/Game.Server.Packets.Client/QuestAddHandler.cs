// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.QuestAddHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Bussiness.Managers;
using Game.Base.Packets;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
  [PacketHandler(176, "添加任务")]
  public class QuestAddHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int num = packet.ReadInt();
      for (int index = 0; index < num; ++index)
      {
        QuestInfo singleQuest = QuestMgr.GetSingleQuest(packet.ReadInt());
        client.Player.QuestInventory.AddQuest(singleQuest, out string _);
      }
      return 0;
    }
  }
}
