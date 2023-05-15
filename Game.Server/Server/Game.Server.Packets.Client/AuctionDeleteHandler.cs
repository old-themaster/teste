using Bussiness;
using Game.Base.Packets;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
    [PacketHandler(194, "撤消拍卖")]
    public class AuctionDeleteHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            int auctionID = packet.ReadInt();
            string translation = LanguageMgr.GetTranslation("AuctionDeleteHandler.Fail");
            using (PlayerBussiness playerBussiness = new PlayerBussiness())
            {
                if (playerBussiness.DeleteAuction(auctionID, client.Player.PlayerCharacter.ID, ref translation))
                {
                    client.Out.SendMailResponse(client.Player.PlayerCharacter.ID, eMailRespose.Receiver);
                    client.Out.SendAuctionRefresh((AuctionInfo)null, auctionID, false, (ItemInfo)null);
                }
                else
                {
                    AuctionInfo auctionSingle = playerBussiness.GetAuctionSingle(auctionID);
                    client.Out.SendAuctionRefresh(auctionSingle, auctionID, auctionSingle != null, (ItemInfo)null);
                }
                client.Out.SendMessage(eMessageType.GM_NOTICE, translation);
            }
            return 0;
        }
    }
}
