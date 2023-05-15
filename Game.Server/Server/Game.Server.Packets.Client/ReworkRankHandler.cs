using Bussiness;
using Game.Base.Packets;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
    [PacketHandler(189, "RefreshHonor")]
    public class ReworkRankHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            string honor = packet.ReadString();
            if (!string.IsNullOrEmpty(honor))
            {
                client.Player.UpdateHonor(honor);
                new PlayerBussiness().AddDailyRecord(new DailyRecordInfo()
                {
                    UserID = client.Player.PlayerCharacter.ID,
                    Type = 7,
                    Value = honor
                });
            }
            return 0;
        }
    }
}
