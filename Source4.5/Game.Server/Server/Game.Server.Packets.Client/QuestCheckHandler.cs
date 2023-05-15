// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.QuestCheckHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.Quests;

namespace Game.Server.Packets.Client
{
  [PacketHandler(181, "客服端任务检查")]
  public class QuestCheckHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int id1 = packet.ReadInt();
      int id2 = packet.ReadInt();
      int num = packet.ReadInt();
      BaseQuest quest = client.Player.QuestInventory.FindQuest(id1);
      if (quest != null && quest.GetConditionById(id2) is ClientModifyCondition conditionById)
        conditionById.Value = num;
      return 0;
    }
  }
}
