// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.OpenFiveSixHoleHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using System;

namespace Game.Server.Packets.Client
{
  [PacketHandler(217, "游戏创建")]
  public class OpenFiveSixHoleHandler : IPacketHandler
  {
    public static Random random = new Random();

    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int slot = packet.ReadInt();
      int val1 = packet.ReadInt();
      int templateId = packet.ReadInt();
      if (DateTime.Compare(client.Player.LastOpenHole.AddMilliseconds(100.0), DateTime.Now) > 0)
      {
        client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("GoSlow"));
        return 0;
      }
      client.Player.LastOpenHole = DateTime.Now;
      SqlDataProvider.Data.ItemInfo itemAt = client.Player.StoreBag.GetItemAt(slot);
      if (itemAt != null && (itemAt.Template.CategoryID == 7 || itemAt.Template.CategoryID == 1 || itemAt.Template.CategoryID == 5))
      {
        SqlDataProvider.Data.ItemInfo itemByTemplateId = client.Player.PropBag.GetItemByTemplateID(0, templateId);
        if (itemByTemplateId == null || itemByTemplateId.Count <= 0)
          return 0;
        bool val2 = false;
        switch (val1)
        {
          case 5:
            if (itemByTemplateId.isDrill(itemAt.Hole5Level))
            {
              client.Player.PropBag.RemoveCountFromStack(itemByTemplateId, 1);
              client.Player.OnUsingItem(itemByTemplateId.TemplateID, 1);
              int num = OpenFiveSixHoleHandler.random.Next(itemByTemplateId.Template.Property7, itemByTemplateId.Template.Property8);
              itemAt.Hole5Exp += num;
              switch (itemAt.Hole5Level)
              {
                case 0:
                  if (itemAt.Hole5Exp >= int.Parse(GameProperties.HoleLevelUpExpList.Split('|')[0]))
                  {
                    ++itemAt.Hole5Level;
                    itemAt.Hole5Exp = 0;
                    val2 = true;
                    break;
                  }
                  break;
                case 1:
                  if (itemAt.Hole5Exp >= int.Parse(GameProperties.HoleLevelUpExpList.Split('|')[1]))
                  {
                    ++itemAt.Hole5Level;
                    itemAt.Hole5Exp = 0;
                    val2 = true;
                    break;
                  }
                  break;
                case 2:
                  if (itemAt.Hole5Exp >= int.Parse(GameProperties.HoleLevelUpExpList.Split('|')[2]))
                  {
                    ++itemAt.Hole5Level;
                    itemAt.Hole5Exp = 0;
                    val2 = true;
                    break;
                  }
                  break;
                case 3:
                  if (itemAt.Hole5Exp >= int.Parse(GameProperties.HoleLevelUpExpList.Split('|')[3]))
                  {
                    ++itemAt.Hole5Level;
                    itemAt.Hole5Exp = 0;
                    val2 = true;
                    break;
                  }
                  break;
                case 4:
                  if (itemAt.Hole5Exp >= int.Parse(GameProperties.HoleLevelUpExpList.Split('|')[4]))
                  {
                    ++itemAt.Hole5Level;
                    itemAt.Hole5Exp = 0;
                    val2 = true;
                    break;
                  }
                  break;
              }
            }
            else
            {
              client.Player.SendMessage("Nível de perfuração não é adequado.");
              return 0;
            }
            break;
          case 6:
            if (itemByTemplateId.isDrill(itemAt.Hole6Level))
            {
              client.Player.PropBag.RemoveCountFromStack(itemByTemplateId, 1);
              client.Player.OnUsingItem(itemByTemplateId.TemplateID, 1);
              int num = OpenFiveSixHoleHandler.random.Next(itemByTemplateId.Template.Property7, itemByTemplateId.Template.Property8);
              itemAt.Hole6Exp += num;
              switch (itemAt.Hole6Level)
              {
                case 0:
                  if (itemAt.Hole6Exp >= int.Parse(GameProperties.HoleLevelUpExpList.Split('|')[0]))
                  {
                    ++itemAt.Hole6Level;
                    itemAt.Hole6Exp = 0;
                    val2 = true;
                    break;
                  }
                  break;
                case 1:
                  if (itemAt.Hole6Exp >= int.Parse(GameProperties.HoleLevelUpExpList.Split('|')[1]))
                  {
                    ++itemAt.Hole6Level;
                    itemAt.Hole6Exp = 0;
                    val2 = true;
                    break;
                  }
                  break;
                case 2:
                  if (itemAt.Hole6Exp >= int.Parse(GameProperties.HoleLevelUpExpList.Split('|')[2]))
                  {
                    ++itemAt.Hole6Level;
                    itemAt.Hole6Exp = 0;
                    val2 = true;
                    break;
                  }
                  break;
                case 3:
                  if (itemAt.Hole6Exp >= int.Parse(GameProperties.HoleLevelUpExpList.Split('|')[3]))
                  {
                    ++itemAt.Hole6Level;
                    itemAt.Hole6Exp = 0;
                    val2 = true;
                    break;
                  }
                  break;
                case 4:
                  if (itemAt.Hole6Exp >= int.Parse(GameProperties.HoleLevelUpExpList.Split('|')[4]))
                  {
                    ++itemAt.Hole6Level;
                    itemAt.Hole6Exp = 0;
                    val2 = true;
                    break;
                  }
                  break;
              }
            }
            else
            {
              client.Player.SendMessage("Nível de perfuração não é adequado.");
              return 0;
            }
            break;
          default:
            Console.WriteLine("no have hole " + (object) val1);
            return 0;
        }
        client.Player.StoreBag.UpdateItem(itemAt);
        GSPacketIn pkg = new GSPacketIn((short) 217);
        pkg.WriteByte((byte) 0);
        pkg.WriteBoolean(val2);
        pkg.WriteInt(val1);
        client.Player.SendTCP(pkg);
      }
      else
        client.Player.SendMessage("Không thể đục lỗ");
      return 0;
    }
  }
}
