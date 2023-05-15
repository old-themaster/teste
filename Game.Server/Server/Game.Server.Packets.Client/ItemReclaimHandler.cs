using Game.Base.Packets;
using Game.Server.GameUtils;
using Game.Server.Managers;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
    [PacketHandler(127, "物品比较")]
	public class ItemReclaimHandler : IPacketHandler
	{
		public int HandlePacket(GameClient client, GSPacketIn packet)
		{
			eBageType bageType = (eBageType)packet.ReadByte();
			int slot = packet.ReadInt();
			int count = packet.ReadInt();
			PlayerInventory inventory = client.Player.GetInventory(bageType);
			if (inventory != null && inventory.GetItemAt(slot) != null)
			{
				if (inventory.GetItemAt(slot).Count <= count)
				{
					count = inventory.GetItemAt(slot).Count;
				}
				ItemTemplateInfo template = inventory.GetItemAt(slot).Template;
				int num3 = count * template.ReclaimValue;
				if (template.ReclaimType == 3)
				{
					client.Player.AddMoney(num3);
					client.Out.SendMessage(eMessageType.GM_NOTICE, string.Format("Você ganha {0} moedas.", num3));
					GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
					for (int i = 0; i < allPlayers.Length; i++)
					{
						allPlayers[i].Out.SendMessage(eMessageType.ChatNormal, string.Format("Parabéns ao jogador [{0}] que acabou de vender o item {1} e ganhe {2} moedas.", client.Player.PlayerCharacter.NickName, template.Name, num3));
					}
				}
				if (template.ReclaimType == 2)
				{
					client.Player.AddGiftToken(num3);
					client.Out.SendMessage(eMessageType.GM_NOTICE, string.Format("Você recebeu {0} moedas de ouro.", num3));
				}
				if (template.ReclaimType == 1)
				{
					client.Player.AddGold(num3);
					client.Out.SendMessage(eMessageType.GM_NOTICE, string.Format("Você ganha {0} ouro.", num3));
				}
				if (template.TemplateID == 11408)
				{
					client.Player.RemoveMedal(count);
				}
				if (template.TemplateID == 11183)
				{
					client.Player.RemoveAscension(count);
				}
				inventory.RemoveItemAt(slot);
				return 0;
			}
			client.Out.SendMessage(eMessageType.GM_NOTICE, string.Format("A venda do item falhou."));
			return 1;
		}
	}
}
