// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.MoveGoodsAllHandler
// Assembly: Game.Server, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0B480679-DF24-46B7-834E-821AA9A4FB3F
// Assembly location: C:\Users\Anderson\Desktop\Source 4.2\Game.Server.dll

using Game.Base.Packets;
using Game.Server.GameUtils;
using log4net;
using SqlDataProvider.Data;
using System.Collections.Generic;
using System.Reflection;

namespace Game.Server.Packets.Client
{
  [PacketHandler(124, "物品比较")]
  public class MoveGoodsAllHandler : IPacketHandler
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      bool flag = packet.ReadBoolean();
      int num1 = packet.ReadInt();
      int num2 = packet.ReadInt();
      PlayerInventory inventory = client.Player.GetInventory((eBageType) num2);
      int maxSlot = inventory.Capalility;
      if (inventory.BagType == 0)
        maxSlot = 80;
      List<ItemInfo> items1 = inventory.GetItems(inventory.BeginSlot, maxSlot);
      if (num1 == items1.Count)
      {
        inventory.BeginChanges();
        try
        {
          if (inventory.FindFirstEmptySlot(inventory.BeginSlot, maxSlot) != -1)
          {
            for (int index = 1; inventory.FindFirstEmptySlot(inventory.BeginSlot, maxSlot) < items1[items1.Count - index].Place; ++index)
              inventory.MoveItem(items1[items1.Count - index].Place, inventory.FindFirstEmptySlot(inventory.BeginSlot, maxSlot), items1[items1.Count - index].Count);
          }
        }
        finally
        {
          if (flag)
          {
            try
            {
              List<ItemInfo> items2 = inventory.GetItems(inventory.BeginSlot, maxSlot);
              List<int> intList = new List<int>();
              for (int index1 = 0; index1 < items2.Count; ++index1)
              {
                if (!intList.Contains(index1))
                {
                  for (int index2 = items2.Count - 1; index2 > index1; --index2)
                  {
                    if (!intList.Contains(index2) && (items2[index1].TemplateID == items2[index2].TemplateID && items2[index1].CanStackedTo(items2[index2])))
                    {
                      inventory.MoveItem(items2[index2].Place, items2[index1].Place, items2[index2].Count);
                      intList.Add(index2);
                    }
                  }
                }
              }
            }
            finally
            {
              List<ItemInfo> items2 = inventory.GetItems(inventory.BeginSlot, maxSlot);
              if (inventory.FindFirstEmptySlot(inventory.BeginSlot, maxSlot) != -1)
              {
                for (int index = 1; inventory.FindFirstEmptySlot(inventory.BeginSlot, maxSlot) < items2[items2.Count - index].Place; ++index)
                  inventory.MoveItem(items2[items2.Count - index].Place, inventory.FindFirstEmptySlot(inventory.BeginSlot, maxSlot), items2[items2.Count - index].Count);
              }
            }
          }
          inventory.CommitChanges();
        }
      }
      return 0;
    }
  }
}
