using System;
using System.Collections.Generic;
using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.Managers;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
	[PacketHandler(258, "Novice Activity")]
	public class NoviceActivityGetAward : IPacketHandler
	{
		public int HandlePacket(GameClient client, GSPacketIn packet)
		{
			int num = packet.ReadInt();
			int num2 = packet.ReadInt();
			if (DateTime.Compare(client.Player.LastOpenCard.AddSeconds(1.5), DateTime.Now) > 0)
			{
				return 0;
			}
			bool isPlus = false;
			//string translateId = "Evento Coletado!";
			string message = "Receba presentes de eventos de sucesso!";
			ProduceBussiness produceBussiness = new ProduceBussiness();
			EventRewardProcessInfo eventProcess = client.Player.Extra.GetEventProcess(num);
			List<ItemInfo> list = new List<ItemInfo>();
			int num3 = ((num2 == 1) ? 1 : (eventProcess.AwardGot * 2 + 1));
			switch (num3)
			{
				case 1:
					num2 = 1;
					break;
				case 3:
					num2 = 2;
					break;
				case 7:
					num2 = 3;
					break;
				case 15:
					num2 = 4;
					break;
				case 31:
					num2 = 5;
					break;
				case 63:
					num2 = 6;
					break;
				case 127:
					num2 = 7;
					break;
				case 255:
					num2 = 8;
					break;
				case 511:
					num2 = 9;
					break;
			}
			EventRewardInfo[] eventRewardInfoByType = produceBussiness.GetEventRewardInfoByType(num, num2);
			EventRewardGoodsInfo[] eventRewardGoodsByType = produceBussiness.GetEventRewardGoodsByType(num, num2);
			foreach (EventRewardGoodsInfo eventRewardGoodsInfo in eventRewardGoodsByType)
			{
				ItemInfo itemInfo = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(eventRewardGoodsInfo.TemplateId), 1, 104);
				itemInfo.StrengthenLevel = eventRewardGoodsInfo.StrengthLevel;
				itemInfo.AttackCompose = eventRewardGoodsInfo.AttackCompose;
				itemInfo.DefendCompose = eventRewardGoodsInfo.DefendCompose;
				itemInfo.AgilityCompose = eventRewardGoodsInfo.AgilityCompose;
				itemInfo.LuckCompose = eventRewardGoodsInfo.LuckCompose;
				itemInfo.IsBinds = eventRewardGoodsInfo.IsBind;
				itemInfo.Count = eventRewardGoodsInfo.Count;
				itemInfo.ValidDate = eventRewardGoodsInfo.ValidDate;
				list.Add(itemInfo);
			}
			string noviceActivityName = client.Player.Extra.GetNoviceActivityName((NoviceActiveType)num);
			EventRewardInfo[] array = eventRewardInfoByType;
			foreach (EventRewardInfo eventRewardInfo in array)
			{
				if (eventRewardInfo.Condition >= eventProcess.Conditions + 1)
				{
					client.Player.SendMessage("Requisito para coleta não cumprido.");
					client.Player.AddLog("Hack/Cheat", "Hack Novice Activity: " + client.Player.PlayerCharacter.NickName);
					GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
					foreach (GamePlayer gamePlayer in allPlayers)
					{
						if (ComandosMgr.checkStaff(gamePlayer.PlayerCharacter.ID))
						{
							GSPacketIn pkg = WorldMgr.SendStaffHelp("O jogador: [" + client.Player.PlayerCharacter.NickName + "] está tentando usar HACK Novice Activity");
							gamePlayer.SendTCP(pkg);
						}
					}
					return 0;
				}
				if (num3 != 999)
				{
					client.Player.Extra.UpdateEventCondition(num, eventRewardInfo.Condition, isPlus, num3);
					//client.Player.SendItemsToMail(list, "O evento de meta foi concluído aqui estão suas recompensas.", "Meta concluída", eMailType.Manage);
					client.Player.SendItemsToMail(list, string.Format("Este é um e-mail automático do presente de abertura do servidor! por favor não responda."), LanguageMgr.GetTranslation("Presente de abertura do servidor"), eMailType.Manage);
				}
			}
			client.Player.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation(message));
			client.Player.LastOpenCard = DateTime.Now;
			GSPacketIn packet2 = WorldMgr.SendSysTipNotice(string.Format("Parabéns [{0}]  acaba de receber o <{1}> em presentes de abertura do servidor.", client.Player.PlayerCharacter.NickName, noviceActivityName));
			GameServer.Instance.LoginServer.SendPacket(packet2);
			return 1;
		}
	}
}
