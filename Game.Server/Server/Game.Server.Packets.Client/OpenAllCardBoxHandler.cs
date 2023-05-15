// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.OpenAllCardBoxHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Bussiness.Managers;
using Game.Base.Packets;
using SqlDataProvider.Data;
using System.Collections.Generic;

namespace Game.Server.Packets.Client
{
  [PacketHandler(204, "打开物品")]
  public class OpenAllCardBoxHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      ItemInfo itemByTemplateId = client.Player.GetItemByTemplateID(112150);
      List<ItemInfo> itemInfoList = new List<ItemInfo>();
      int gold = 0;
      int giftToken = 0;
      int point = 0;
      int exp = 0;
      for (int index = 0; index < itemByTemplateId.Count; ++index)
        ItemBoxMgr.CreateItemBox(itemByTemplateId.TemplateID, itemInfoList, ref gold, ref point, ref giftToken, ref exp);
      client.Player.PropBag.RemoveItem(itemByTemplateId);
      if (client.Player.SendItemsToMail(itemInfoList, "Ao abrir todos os cartões de uma vez, você recebeu na sua caixa de correio.", "Caixa de cartões misteriosos", eMailType.Default))
        client.Player.SendMessage("As recompensas foram enviadas para a sua caixa de correio");
      return 1;
    }
  }
}
