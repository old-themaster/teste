// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.QuestRemoveHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.Quests;

namespace Game.Server.Packets.Client
{
  [PacketHandler(177, "删除任务")]
  public class QuestRemoveHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int id = packet.ReadInt();
      BaseQuest quest = client.Player.QuestInventory.FindQuest(id);
      if (quest != null)
        client.Player.QuestInventory.RemoveQuest(quest);
      return 0;
    }
  }
}
