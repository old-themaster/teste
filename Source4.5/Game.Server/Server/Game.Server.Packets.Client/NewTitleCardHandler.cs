using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
	[PacketHandler(265, "Obter-Cartão")]
	public class NewTitleCardHandler : IPacketHandler
	{
		public int HandlePacket(GameClient client, GSPacketIn packet)
		{
			eBageType bagType = (eBageType)packet.ReadByte();
			int place = packet.ReadInt();
			ItemInfo itemAt = client.Player.GetItemAt(bagType, place);
			if (itemAt != null)
			{
				NewTitleInfo newTitleInfo = NewTitleMgr.FindNewTitle(itemAt.Template.Property1);
				if (newTitleInfo == null)
				{
					client.Player.SendMessage(LanguageMgr.GetTranslation("NewTitleCardHandler.TitleNotFound"));
				}
				else if (client.Player.RemoveCountFromStack(itemAt, 1))
				{
					client.Player.Rank.AddNewRank(newTitleInfo.ID, itemAt.Template.Property2);
					client.Player.SendMessage(LanguageMgr.GetTranslation("NewTitleCardHandler.Success", newTitleInfo.Name));
				}
				else
				{
					client.Player.SendMessage(LanguageMgr.GetTranslation("NewTitleCardHandler.RemoveItemError"));
				}
			}
			return 0;
		}
	}
}