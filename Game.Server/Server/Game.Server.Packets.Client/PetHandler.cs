// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.PetHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using System;

namespace Game.Server.Packets.Client
{
  [PacketHandler(68, "添加好友")]
  public class PetHandler : IPacketHandler
  {
    public static Random random = new Random();

    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      if (client.Player.PlayerCharacter.Grade < 25)
      {
        client.Player.SendMessage(LanguageMgr.GetTranslation("PetHandler.Msg23"));
        return 0;
      }

      if (client.Player.PetHandler == null)
        return 0;
      client.Player.PetHandler.ProcessData(client.Player, packet);
      return 1;
    }
  }
}
