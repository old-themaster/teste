using System;
using System.Collections.Generic;
using Bussiness;
using Game.Base.Packets;
using Game.Server.ConsortiaTask;
using SqlDataProvider.Data;

namespace Game.Server.Consortia.Handle
{
    [global::Consortia(22)]
	public class CConsortiaTask : IConsortiaCommandHadler
	{
		public int CommandHandler(GamePlayer Player, GSPacketIn packet)
		{
			if (Player.PlayerCharacter.ConsortiaID == 0)
			{
				return 0;
			}
			switch (packet.ReadInt())
			{
			case 0:
			{
				using (ConsortiaBussiness consortiaBussiness1 = new ConsortiaBussiness())
				{
					ConsortiaInfo consortiaSingle = consortiaBussiness1.GetConsortiaSingle(Player.PlayerCharacter.ConsortiaID);
					if (consortiaSingle != null && consortiaSingle.ChairmanID == Player.PlayerCharacter.ID)
					{
						int riches = GameProperties.MissionRichesArr()[consortiaSingle.Level - 1];
						DateTime date4 = consortiaSingle.DateOpenTask.Date;
						DateTime date2 = DateTime.Now.Date;
						if (date4 != date2)
						{
							if (consortiaBussiness1.ConsortiaRichRemove(consortiaSingle.ConsortiaID, ref riches))
							{
								int consortiaId = consortiaSingle.ConsortiaID;
								DateTime date3 = DateTime.Now.Date;
								consortiaBussiness1.ConsortiaTaskUpdateDate(consortiaId, date3);
								if (ConsortiaTaskMgr.AddConsortiaTask(consortiaSingle.ConsortiaID, consortiaSingle.Level))
								{
									BaseConsortiaTask singleConsortiaTask = ConsortiaTaskMgr.GetSingleConsortiaTask(Player.PlayerCharacter.ConsortiaID);
									GSPacketIn pkg = new GSPacketIn(129);
									pkg.WriteByte(22);
									pkg.WriteByte(0);
									pkg.WriteInt(singleConsortiaTask.ConditionList.Count);
									foreach (KeyValuePair<int, ConsortiaTaskInfo> condition in singleConsortiaTask.ConditionList)
									{
										pkg.WriteInt(condition.Key);
										pkg.WriteString(condition.Value.CondictionTitle);
									}
									Player.SendTCP(pkg);
								}
								else
								{
									Player.SendMessage(LanguageMgr.GetTranslation("GameServer.ConsortiaTask.msg6"));
								}
							}
							else
							{
								Player.SendMessage(LanguageMgr.GetTranslation("GameServer.ConsortiaTask.msg8"));
							}
						}
						else
						{
							Player.SendMessage(LanguageMgr.GetTranslation("GameServer.ConsortiaTask.msg5"));
						}
					}
					else
					{
						Player.SendMessage(LanguageMgr.GetTranslation("GameServer.ConsortiaTask.msg7"));
					}
				}
				break;
			}
			case 2:
			{
				BaseConsortiaTask singleConsortiaTask2 = ConsortiaTaskMgr.GetSingleConsortiaTask(Player.PlayerCharacter.ConsortiaID);
				bool val = false;
				if (singleConsortiaTask2 != null && !singleConsortiaTask2.Info.IsActive && ConsortiaTaskMgr.ActiveTask(Player.PlayerCharacter.ConsortiaID))
				{
					val = true;
					Player.Out.SendConsortiaTaskInfo(singleConsortiaTask2);
				}
				GSPacketIn pkg2 = new GSPacketIn(129);
				pkg2.WriteByte(22);
				pkg2.WriteByte(2);
				pkg2.WriteBoolean(val);
				Player.SendTCP(pkg2);
				break;
			}
			case 3:
			{
				BaseConsortiaTask singleConsortiaTask3 = ConsortiaTaskMgr.GetSingleConsortiaTask(Player.PlayerCharacter.ConsortiaID);
				Player.Out.SendConsortiaTaskInfo(singleConsortiaTask3);
				break;
			}
			}
			return 0;
		}
	}
}
