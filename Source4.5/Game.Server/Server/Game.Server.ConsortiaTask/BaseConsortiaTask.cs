using System;
using System.Collections.Generic;
using System.Linq;
using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.ConsortiaTask.Data;
using Game.Server.Managers;
using SqlDataProvider.Data;

namespace Game.Server.ConsortiaTask
{
    public class BaseConsortiaTask
	{
		private ConsortiaTaskDataInfo consortiaTaskDataInfo_0;

		private Dictionary<int, ConsortiaTaskUserDataInfo> dictionary_0;

		private Dictionary<int, ConsortiaTaskInfo> dictionary_1;

		private object object_0;

		public Dictionary<int, ConsortiaTaskInfo> ConditionList => dictionary_1;

		public ConsortiaTaskDataInfo Info => consortiaTaskDataInfo_0;

		public Dictionary<int, ConsortiaTaskUserDataInfo> ListUsers => dictionary_0;

		public BaseConsortiaTask(ConsortiaTaskDataInfo info, List<ConsortiaTaskInfo> listCondition)
		{
			object_0 = new object();
			dictionary_0 = new Dictionary<int, ConsortiaTaskUserDataInfo>();
			dictionary_1 = new Dictionary<int, ConsortiaTaskInfo>();
			consortiaTaskDataInfo_0 = info;
			int key = 0;
			foreach (ConsortiaTaskInfo consortiaTaskInfo in listCondition)
			{
				dictionary_1.Add(key, consortiaTaskInfo);
				key++;
			}
		}

		private void method_0(ConsortiaTaskUserDataInfo consortiaTaskUserDataInfo_0)
		{
			foreach (KeyValuePair<int, ConsortiaTaskInfo> keyValuePair in dictionary_1)
			{
				BaseConsortiaTaskCondition condition = BaseConsortiaTaskCondition.CreateCondition(consortiaTaskUserDataInfo_0, this, keyValuePair.Value, consortiaTaskUserDataInfo_0.GetConditionValue(keyValuePair.Key));
				if (condition != null)
				{
					condition.AddTrigger(consortiaTaskUserDataInfo_0);
					consortiaTaskUserDataInfo_0.ConditionList.Add(condition);
				}
			}
		}

		private void method_1(ConsortiaTaskUserDataInfo consortiaTaskUserDataInfo_0)
		{
			foreach (BaseConsortiaTaskCondition condition in consortiaTaskUserDataInfo_0.ConditionList)
			{
				condition.RemoveTrigger(consortiaTaskUserDataInfo_0);
			}
		}

		public ConsortiaTaskInfo GetPlaceCondtion(int conditionId, ref int place)
		{
			ConsortiaTaskInfo consortiaTaskInfo = null;
			foreach (KeyValuePair<int, ConsortiaTaskInfo> keyValuePair in dictionary_1)
			{
				if (keyValuePair.Value.ID == conditionId)
				{
					place = keyValuePair.Key;
					return keyValuePair.Value;
				}
			}
			return consortiaTaskInfo;
		}

		public int GetTotalValueByConditionPlace(int place)
		{
			switch (place)
			{
			case 0:
				return consortiaTaskDataInfo_0.Condition1;
			case 1:
				return consortiaTaskDataInfo_0.Condition2;
			case 2:
				return consortiaTaskDataInfo_0.Condition3;
			default:
				return 0;
			}
		}

		public int GetValueByConditionPlace(int userid, int place)
		{
			ConsortiaTaskUserDataInfo singlePlayer = GetSinglePlayer(userid);
			if (singlePlayer == null)
			{
				return 0;
			}
			switch (place)
			{
			case 0:
				return singlePlayer.Condition1;
			case 1:
				return singlePlayer.Condition2;
			case 2:
				return singlePlayer.Condition3;
			default:
				return 0;
			}
		}

		public void SaveData(ConsortiaTaskUserDataInfo player)
		{
			int index = 0;
			foreach (BaseConsortiaTaskCondition condition in player.ConditionList)
			{
				player.SaveConditionValue(index, condition.Value);
				index++;
			}
		}

		public void RemakeValue(int conditionId, ref int valueAdd)
		{
			int place = -1;
			ConsortiaTaskInfo placeCondtion = GetPlaceCondtion(conditionId, ref place);
			lock (object_0)
			{
				if (placeCondtion != null && consortiaTaskDataInfo_0.IsVaildDate() && !consortiaTaskDataInfo_0.IsComplete)
				{
					switch (place)
					{
					case 0:
						if (consortiaTaskDataInfo_0.Condition1 + valueAdd > placeCondtion.Para2)
						{
							valueAdd = placeCondtion.Para2 - consortiaTaskDataInfo_0.Condition1;
						}
						break;
					case 1:
						if (consortiaTaskDataInfo_0.Condition2 + valueAdd > placeCondtion.Para2)
						{
							valueAdd = placeCondtion.Para2 - consortiaTaskDataInfo_0.Condition2;
						}
						break;
					case 2:
						if (consortiaTaskDataInfo_0.Condition3 + valueAdd > placeCondtion.Para2)
						{
							valueAdd = placeCondtion.Para2 - consortiaTaskDataInfo_0.Condition3;
						}
						break;
					}
				}
				else
				{
					valueAdd = 0;
				}
			}
		}

		public void Update(ConsortiaTaskUserDataInfo player)
		{
			SaveData(player);
			UpdateTotalCondition();
			Finish();
		}

		public void UpdateTotalCondition()
		{
			List<ConsortiaTaskUserDataInfo> allPlayers = GetAllPlayers();
			lock (object_0)
			{
				consortiaTaskDataInfo_0.Condition1 = 0;
				consortiaTaskDataInfo_0.Condition2 = 0;
				consortiaTaskDataInfo_0.Condition3 = 0;
				foreach (ConsortiaTaskUserDataInfo taskUserDataInfo in allPlayers)
				{
					consortiaTaskDataInfo_0.Condition1 += taskUserDataInfo.Condition1;
					consortiaTaskDataInfo_0.Condition2 += taskUserDataInfo.Condition2;
					consortiaTaskDataInfo_0.Condition3 += taskUserDataInfo.Condition3;
				}
			}
		}

		public bool CanCompleted()
		{
			if (!consortiaTaskDataInfo_0.IsActive)
			{
				return false;
			}
			lock (object_0)
			{
				foreach (KeyValuePair<int, ConsortiaTaskInfo> keyValuePair in dictionary_1)
				{
					switch (keyValuePair.Key)
					{
					case 0:
						if (keyValuePair.Value.Para2 > consortiaTaskDataInfo_0.Condition1)
						{
							return false;
						}
						break;
					case 1:
						if (keyValuePair.Value.Para2 > consortiaTaskDataInfo_0.Condition2)
						{
							return false;
						}
						break;
					case 2:
						if (keyValuePair.Value.Para2 > consortiaTaskDataInfo_0.Condition3)
						{
							return false;
						}
						break;
					}
				}
				return true;
			}
		}

		public bool Finish()
		{
			if (consortiaTaskDataInfo_0.IsVaildDate() && CanCompleted())
			{
				DisableTriggle();
				consortiaTaskDataInfo_0.IsComplete = true;
				List<ConsortiaTaskUserDataInfo> taskUserDataInfoList = new List<ConsortiaTaskUserDataInfo>();
				lock (dictionary_0)
				{
					taskUserDataInfoList = dictionary_0.Values.OrderByDescending((ConsortiaTaskUserDataInfo a) => a.GetTotalConditionCompleted()).ToList();
				}
				int conditionCompleted = consortiaTaskDataInfo_0.GetTotalConditionCompleted();
				if (conditionCompleted > 0)
				{
					using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
					{
						foreach (ConsortiaTaskUserDataInfo taskUserDataInfo in taskUserDataInfoList)
						{
							float num1 = (float)((double)taskUserDataInfo.GetTotalConditionCompleted() / (double)conditionCompleted * 100.0);
							if ((double)num1 > 0.0)
							{
								int gp = (int)Math.Floor((double)(consortiaTaskDataInfo_0.TotalExp / 100) * (double)num1);
								int num2 = (int)Math.Floor((double)(consortiaTaskDataInfo_0.TotalOffer / 100) * (double)num1);
								int riches = (int)Math.Floor((double)(consortiaTaskDataInfo_0.TotalRiches / 100) * (double)num1);
								if (taskUserDataInfo.Player != null && taskUserDataInfo.Player.IsActive)
								{
									taskUserDataInfo.Player.AddGP(gp);
									taskUserDataInfo.Player.AddOffer(num2);
									consortiaBussiness.ConsortiaRichAdd(taskUserDataInfo.Player.PlayerCharacter.ConsortiaID, ref riches);
									taskUserDataInfo.Player.SendMessage(LanguageMgr.GetTranslation("GameServer.ConsortiaTask.msg3", num1, gp, num2));
								}
								else
								{
									ConsortiaTaskMgr.AddConsortiaTaskUserTemp(new ConsortiaTaskUserTempInfo
									{
										Total = (int)num1,
										Exp = gp,
										Offer = num2
									});
								}
							}
						}
						if (consortiaTaskDataInfo_0.BuffID > 0)
						{
							ConsortiaBuffTempInfo consortiaBuffInfo = ConsortiaExtraMgr.FindConsortiaBuffInfo(consortiaTaskDataInfo_0.BuffID);
							if (consortiaBuffInfo != null)
							{
								ConsortiaMgr.AddBuffConsortia(null, consortiaBuffInfo, consortiaTaskDataInfo_0.ConsortiaID, consortiaTaskDataInfo_0.BuffID, 1440);
							}
						}
					}
				}
				SendSystemConsortiaChat(LanguageMgr.GetTranslation("GameServer.ConsortiaTask.msg2"));
				consortiaTaskDataInfo_0.CanRemove = true;
				return true;
			}
			if (consortiaTaskDataInfo_0.IsVaildDate() || !consortiaTaskDataInfo_0.IsActive)
			{
				return false;
			}
			SendSystemConsortiaChat(LanguageMgr.GetTranslation("GameServer.ConsortiaTask.msg1"));
			DisableTriggle();
			consortiaTaskDataInfo_0.IsComplete = true;
			consortiaTaskDataInfo_0.CanRemove = true;
			return true;
		}

		public void AddToPlayer(GamePlayer player)
		{
			if (!CheckCanAddPlayer(player.PlayerId))
			{
				return;
			}
			ConsortiaTaskUserDataInfo consortiaTaskUserDataInfo_0 = GetSinglePlayer(player.PlayerId);
			if (consortiaTaskUserDataInfo_0 == null)
			{
				consortiaTaskUserDataInfo_0 = new ConsortiaTaskUserDataInfo();
				consortiaTaskUserDataInfo_0.UserID = player.PlayerId;
				lock (dictionary_0)
				{
					dictionary_0.Add(player.PlayerId, consortiaTaskUserDataInfo_0);
				}
			}
			if (consortiaTaskUserDataInfo_0.Player == null)
			{
				consortiaTaskUserDataInfo_0.Player = player;
			}
			consortiaTaskUserDataInfo_0.ConditionList = new List<BaseConsortiaTaskCondition>();
			method_0(consortiaTaskUserDataInfo_0);
		}

		public void RemoveToPlayer(GamePlayer player)
		{
			ConsortiaTaskUserDataInfo singlePlayer = GetSinglePlayer(player.PlayerId);
			if (singlePlayer != null && singlePlayer.ConditionList != null)
			{
				method_1(singlePlayer);
				singlePlayer.Player = null;
				singlePlayer.ConditionList = null;
			}
		}

		public bool CheckCanAddPlayer(int userid)
		{
			bool flag = false;
			if (consortiaTaskDataInfo_0.IsActive && !consortiaTaskDataInfo_0.IsComplete && consortiaTaskDataInfo_0.IsVaildDate())
			{
				lock (dictionary_0)
				{
					if (!dictionary_0.ContainsKey(userid) || dictionary_0[userid].Player == null)
					{
						return true;
					}
					return flag;
				}
			}
			return flag;
		}

		public ConsortiaTaskUserDataInfo GetSinglePlayer(int userId)
		{
			ConsortiaTaskUserDataInfo taskUserDataInfo = null;
			lock (dictionary_0)
			{
				if (dictionary_0.ContainsKey(userId))
				{
					return dictionary_0[userId];
				}
				return taskUserDataInfo;
			}
		}

		public List<ConsortiaTaskUserDataInfo> GetAllPlayers()
		{
			List<ConsortiaTaskUserDataInfo> taskUserDataInfoList = new List<ConsortiaTaskUserDataInfo>();
			lock (dictionary_0)
			{
				foreach (ConsortiaTaskUserDataInfo taskUserDataInfo in dictionary_0.Values)
				{
					taskUserDataInfoList.Add(taskUserDataInfo);
				}
				return taskUserDataInfoList;
			}
		}

		public void DisableTriggle()
		{
			lock (dictionary_0)
			{
				foreach (ConsortiaTaskUserDataInfo consortiaTaskUserDataInfo_0 in dictionary_0.Values)
				{
					if (consortiaTaskUserDataInfo_0.Player != null && consortiaTaskUserDataInfo_0.ConditionList != null)
					{
						method_1(consortiaTaskUserDataInfo_0);
					}
				}
			}
		}

		public void ClearAllUserData()
		{
			DisableTriggle();
			lock (dictionary_0)
			{
				dictionary_0.Clear();
			}
		}

		public void SendSystemConsortiaChat(string content)
		{
			GamePlayer[] allPlayersWithConsortia = WorldMgr.GetAllPlayersWithConsortia(consortiaTaskDataInfo_0.ConsortiaID);
			for (int i = 0; i < allPlayersWithConsortia.Length; i++)
			{
				allPlayersWithConsortia[i].Out.SendSystemConsortiaChat(content, sendToSelf: true);
			}
		}

		public void SendToAll(GSPacketIn pkg, GamePlayer ext)
		{
			GamePlayer[] allPlayersWithConsortia = WorldMgr.GetAllPlayersWithConsortia(consortiaTaskDataInfo_0.ConsortiaID);
			foreach (GamePlayer playersWithConsortium in allPlayersWithConsortia)
			{
				if (playersWithConsortium.IsActive && playersWithConsortium != ext)
				{
					playersWithConsortium.SendTCP(pkg);
				}
			}
		}
	}
}
