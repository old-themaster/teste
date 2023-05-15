// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.UserUpdateMailHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Packets.Client
{
  [PacketHandler(114, "修改邮件的已读未读标志")]
  public class UserUpdateMailHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      GSPacketIn packet1 = new GSPacketIn((short) 114, client.Player.PlayerCharacter.ID);
      int mailID = packet.ReadInt();
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        MailInfo mailSingle = playerBussiness.GetMailSingle(client.Player.PlayerCharacter.ID, mailID);
        if (mailSingle != null && !mailSingle.IsRead)
        {
          mailSingle.IsRead = true;
          if (mailSingle.Type < 100)
          {
            mailSingle.ValidDate = 72;
            mailSingle.SendTime = DateTime.Now;
          }
          playerBussiness.UpdateMail(mailSingle, mailSingle.Money);
          packet1.WriteBoolean(true);
        }
        else
          packet1.WriteBoolean(false);
      }
      client.Out.SendTCP(packet1);
      return 0;
    }
  }
}
