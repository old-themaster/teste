// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.UseReworkNameHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameUtils;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
  [PacketHandler(171, "场景用户离开")]
  public class UseReworkNameHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      byte num = packet.ReadByte();
      int slot = packet.ReadInt();
      string newNickName = packet.ReadString();
      string msg = "";
      PlayerInventory inventory = client.Player.GetInventory((eBageType) num);
      ItemInfo itemAt = inventory.GetItemAt(slot);
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        if (playerBussiness.RenameNick(client.Player.PlayerCharacter.UserName, client.Player.PlayerCharacter.NickName, newNickName))
          inventory.RemoveCountFromStack(itemAt, 1);
        else
          msg = "Não foi possível renomear o apelido.";
      }
      if (msg != "")
        client.Player.SendMessage(msg);
      return 0;
    }
  }
}
