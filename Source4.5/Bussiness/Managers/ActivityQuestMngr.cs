using log4net;
using SqlDataProvider.Data;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bussiness.Managers
{
    public static class ActivityQuestMngr
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<int, ActivityQuestInfo> _quests = new Dictionary<int, ActivityQuestInfo>();
        private static readonly Dictionary<int, Dictionary<int, AtivityQuestConditionInfo>> _condtions = new Dictionary<int, Dictionary<int, AtivityQuestConditionInfo>>();

        private static readonly Dictionary<int, AtivityQuestGoodsInfo> _rewards = new Dictionary<int, AtivityQuestGoodsInfo>();


        public static bool Init()
        {
            try
            {
                using (var bss = new ActiveBussiness())
                {
                    foreach (var Info in bss.GetAllActivitysQuestInfos())
                        if (!_quests.ContainsKey(Info.ID))
                        {
                            _quests.Add(Info.ID, Info);
                            _condtions.Add(Info.ID, new Dictionary<int, AtivityQuestConditionInfo>());
                        }

                    foreach (var Info in bss.GetAllActivitysQuestConditions())
                    {
                        if (!_condtions.ContainsKey(Info.QuestID) || _condtions[Info.QuestID].ContainsKey(Info.CondictionID))
                            continue;

                        _condtions[Info.QuestID].Add(Info.CondictionID, Info);
                    }

                    foreach (var Info in bss.GetAllActivitysQuestGoods())
                    {
                        if (!_rewards.ContainsKey(Info.ID))
                        {
                            _rewards.Add(Info.ID, Info);
                        }
                    }

                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static ActivityQuestInfo[] GetQuestsByType(int Type)
        {
            return _quests.Values.Where((ActivityQuestInfo s) => s.QuestType == Type).ToArray();
        }
        public static ActivityQuestInfo GetQuestsByID(int ID)
        {
            if (_quests.ContainsKey(ID))
            {
                return _quests[ID];
            }
            return null;
        }

        public static ActivityQuestInfo[] GetActivitysByType(int Type)
        {
            return _quests.Values.Where(s => s.QuestType == Type).ToArray();
        }

        public static ActivityQuestInfo[] GetAllActivitys()
        {
            return _quests.Values.ToArray();
        }

        public static ActivityQuestInfo GetActivityByID(int ID)
        {
            if (_quests.ContainsKey(ID))
                return _quests[ID];
            return null;
        }

        public static AtivityQuestConditionInfo GetCondtionByQuestID(int QuestId, int ConditionID)
        {
            if (_condtions.ContainsKey(QuestId) && _condtions[QuestId].ContainsKey(ConditionID))
                return _condtions[QuestId][ConditionID];
            return null;
        }

        public static AtivityQuestConditionInfo[] GetCondtionsByQuestID(int QuestId)
        {
            if (_condtions.ContainsKey(QuestId))
                return _condtions[QuestId].Values.ToArray();
            return null;
        }

        public static AtivityQuestGoodsInfo[] GetAllActivitysQuestGoodsByQuestID(int QuestId)
        {
            return _rewards.Values.Where(s => s.QuestID == QuestId).ToArray();
        }

        public static AtivityQuestGoodsInfo[] GetAllActivitysQuestGoods(int Type, int Period)
        {
            return _rewards.Values.Where(s => s.QuestID == 0 && s.QuestType == Type && s.Period == Period).ToArray();
        }

        public static AtivityQuestGoodsInfo[] GetAllActivitysQuestGoods()
        {
            return _rewards.Values.ToArray();
        }

    }
}
