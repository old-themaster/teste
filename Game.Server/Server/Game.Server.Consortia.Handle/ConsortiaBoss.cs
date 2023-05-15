using Bussiness;
using Game.Base.Packets;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Consortia.Handle
{
	[global::Consortia(30)]

	public class ConsortiaBoss : IConsortiaCommandHadler
	{
		public int CommandHandler(GamePlayer Player, GSPacketIn packet)
		{			
			byte b2 = packet.ReadByte();
			int num20 = packet.ReadInt();
			int consortiaID2 = Player.PlayerCharacter.ConsortiaID;
			ConsortiaInfo consortiaInfo10 = ConsortiaBossMgr.GetConsortiaById(consortiaID2);
			if (consortiaInfo10 == null || consortiaInfo10.LastOpenBoss.Date < DateTime.Now.Date)
			{
				using (ConsortiaBussiness consortiaBussiness25 = new ConsortiaBussiness())
				{
					consortiaInfo10 = consortiaBussiness25.GetConsortiaSingle(consortiaID2);
				}
			}
			if (consortiaInfo10 == null)
			{
				Player.SendMessage("Guilda não encontrada, operação falhou!");
				return 0;
			}
			if (consortiaInfo10.Level < 3)
			{
			    Player.SendMessage("Guilda de nível 3 e superior pode convocar o chefe.");
				return 0;
			}
			int num21 = ConsortiaMgr.FindConsortiaBossBossMaxLevel(0, consortiaInfo10);
			ConsortiaBossConfigInfo consortiaBossConfigInfo = ConsortiaMgr.FindConsortiaBossConfig(num21);
			if (consortiaBossConfigInfo == null || (consortiaBossConfigInfo.Level != num20 && num20 > 1))
			{
				Player.SendMessage("Falha.. Aguarde e tente mais Tarde! ");
				return 0;
			}
			if (b2 == 0)
			{
				if (consortiaInfo10.Riches < consortiaBossConfigInfo.CostRich)
				{
					Player.SendMessage("Os recursos da guilda não são suficientes, a criação do chefe falhou!");
					return 0;
				}
				consortiaInfo10.bossState = 1;
				consortiaInfo10.endTime = DateTime.Now.AddMinutes(20.0);
				consortiaInfo10.LastOpenBoss = DateTime.Now;
				using (ConsortiaBussiness consortiaBussiness26 = new ConsortiaBussiness())
				{
					int costRich = consortiaBossConfigInfo.CostRich;
					if (consortiaBussiness26.ConsortiaRichRemove(Player.PlayerCharacter.ConsortiaID, ref costRich))
					{
						consortiaInfo10.Riches = costRich;
					}
				}
				ConsortiaBossMgr.CreateBoss(consortiaInfo10, consortiaBossConfigInfo.NpcID);
				return 0;
			}
			else
			{
				if (b2 == 2)
				{
					if (consortiaInfo10.Riches < consortiaBossConfigInfo.ProlongRich)
					{
						Player.SendMessage("Os recursos da guilda não são suficientes, tempo de falha extra!");
						return 0;
					}
					using (ConsortiaBussiness consortiaBussiness27 = new ConsortiaBussiness())
					{
						int prolongRich = consortiaBossConfigInfo.ProlongRich;
						if (consortiaBussiness27.ConsortiaRichRemove(Player.PlayerCharacter.ConsortiaID, ref prolongRich))
						{
							consortiaInfo10.Riches = prolongRich;
						}
					}
					ConsortiaBossMgr.ExtendAvailable(consortiaID2, consortiaInfo10.Riches);
					return 0;
				}
				else
				{
					if (ConsortiaBossMgr.GetConsortiaExit(consortiaID2))
					{
						ConsortiaBossMgr.reload(consortiaID2);
						return 0;
					}
					consortiaInfo10.bossState = 0;
					consortiaInfo10.endTime = DateTime.Now;
					consortiaInfo10.extendAvailableNum = 3;
					consortiaInfo10.callBossLevel = num21;
					ConsortiaBossMgr.AddConsortia(consortiaInfo10);
					return 0;
				}
			}

		}
	}
}

