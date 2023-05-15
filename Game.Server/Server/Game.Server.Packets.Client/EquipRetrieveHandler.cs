// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.EquipRetrieveHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Logic;
using Game.Server.GameUtils;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Game.Server.Packets.Client
{
  [PacketHandler(222, "防沉迷系统开关")]
  public class EquipRetrieveHandler : IPacketHandler
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private ThreadSafeRandom random = new ThreadSafeRandom();

    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      PlayerInventory inventory = client.Player.GetInventory(eBageType.Store);
      int num1 = 0;
      for (int slot = 1; slot < 5; ++slot)
      {
        SqlDataProvider.Data.ItemInfo itemAt = inventory.GetItemAt(slot);
        if (itemAt != null)
        {
          int num2 = itemAt.IsBinds ? 1 : 0;
          num1 += itemAt.Template.Quality;
        }
        else
        {
          client.Player.SendMessage("Itens insuficientes para refinar");
          return 0;
        }
      }
      inventory.ClearBag();
      int user = num1 > 5 ? (num1 > 15 ? (num1 >= 20 ? 4 : 3) : 2) : 1;
      List<SqlDataProvider.Data.ItemInfo> itemInfoList = (List<SqlDataProvider.Data.ItemInfo>) null;
      ref List<SqlDataProvider.Data.ItemInfo> local = ref itemInfoList;
      DropInventory.RetrieveDrop(user, ref local);
      int index = this.random.Next(itemInfoList.Count);
      SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(itemInfoList[index].TemplateID), 1, 105);
      fromTemplate.IsBinds = itemInfoList[index].IsBinds;
      fromTemplate.BeginDate = DateTime.Now;
      fromTemplate.ValidDate = itemInfoList[index].ValidDate;
      if (inventory.AddItemTo(fromTemplate, 0))
        client.Player.RemoveGold(400);
      return 0;
    }
  }
}
