// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.DailyRecordHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
  [PacketHandler(103, "DailyRecord")]
  public class DailyRecordHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn pkg)
    {
      PlayerBussiness playerBussiness = new PlayerBussiness();
      DailyRecordInfo[] dailyRecord = playerBussiness.GetDailyRecord(client.Player.PlayerCharacter.ID);
      int length = dailyRecord.Length;
      GSPacketIn packet = new GSPacketIn((short) 103, client.Player.PlayerId);
      packet.WriteInt(length);
      for (int index = 0; index < length; ++index)
      {
        DailyRecordInfo dailyRecordInfo = dailyRecord[index];
        packet.WriteInt(dailyRecordInfo.Type);
        packet.WriteString(dailyRecordInfo.Value);
        playerBussiness.DeleteDailyRecord(client.Player.PlayerId, dailyRecordInfo.Type);
      }
      client.Out.SendTCP(packet);
      return 1;
    }

    private bool isUpdate(int type)
    {
      switch (type)
      {
        case 10:
        case 11:
        case 12:
        case 13:
        case 14:
        case 15:
        case 16:
        case 17:
        case 18:
        case 19:
        case 20:
          return true;
        default:
          return false;
      }
    }
  }
}
