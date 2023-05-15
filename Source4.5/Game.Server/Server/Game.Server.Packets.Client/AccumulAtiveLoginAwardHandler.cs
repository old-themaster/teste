using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using SqlDataProvider.Data;
using System.Collections.Generic;

namespace Game.Server.Packets.Client
{
    [PacketHandler(238, "Entrada de Acumulação")]
	public class AccumulAtiveLoginAwardHandler : IPacketHandler
	{
		public AccumulAtiveLoginAwardHandler()
		{


		}

		public int HandlePacket(GameClient client, GSPacketIn packet)
		{
			int num = packet.ReadInt();
			GSPacketIn gSPacketIn = new GSPacketIn(238, client.Player.PlayerCharacter.ID);
			string translation = LanguageMgr.GetTranslation("CONTATE O OFICIAL DE ERROS!");
			if (client.Player.PlayerCharacter.accumulativeAwardDays < client.Player.PlayerCharacter.accumulativeLoginDays)
			{
				for (int i = client.Player.PlayerCharacter.accumulativeAwardDays; i < client.Player.PlayerCharacter.accumulativeLoginDays; i++)
				{
					int num1 = i + 1;
					List<ItemInfo> itemInfos = new List<ItemInfo>();
					itemInfos = (num1 < 8 ? AccumulActiveLoginMgr.GetAllAccumulAtiveLoginAward(num1) : AccumulActiveLoginMgr.GetSelecedAccumulAtiveLoginAward(num));
					if (itemInfos.Count <= 0)
					{
						client.Player.SendMessage(translation);
					}
					else
					{
						translation = LanguageMgr.GetTranslation("Prêmio de inscrição acumulado recebido com sucesso!", new object[] { num1 });
						WorldEventMgr.SendItemsToMail(itemInfos, client.Player.PlayerCharacter.ID, client.Player.PlayerCharacter.NickName, translation);
						PlayerInfo playerCharacter = client.Player.PlayerCharacter;
						playerCharacter.accumulativeAwardDays = playerCharacter.accumulativeAwardDays + 1;
						client.Player.SendMessage(translation);
					}
				}
			}
			gSPacketIn.WriteInt(client.Player.PlayerCharacter.accumulativeLoginDays);
			gSPacketIn.WriteInt(client.Player.PlayerCharacter.accumulativeAwardDays);
			client.Player.SendTCP(gSPacketIn);
			return 0;
		}
	}
}