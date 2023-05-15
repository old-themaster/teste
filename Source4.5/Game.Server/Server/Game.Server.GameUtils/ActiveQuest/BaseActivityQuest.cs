using Bussiness.Managers;
using SqlDataProvider.Data;
using System.Collections.Generic;
using System.Linq;

namespace Game.Server.GameUtils.ActiveQuest
{
    public class BaseActivityQuest
    {
        private ActivityQuestInfo m_quest;
        private ActiveQuestUserData m_data;
        private GamePlayer m_player;
        List<BaseActiveQuestCondition> m_conditions;

        public ActivityQuestInfo Quest => m_quest;

        public BaseActivityQuest(ActivityQuestInfo quest, ActiveQuestUserData data, GamePlayer player)
        {
            this.m_quest = quest;
            this.m_data = data;
            m_player = player;
            m_conditions = new List<BaseActiveQuestCondition>();
            foreach (var cond in ActivityQuestMngr.GetCondtionsByQuestID(quest.ID))
                m_conditions.Add(BaseActiveQuestCondition.CreateQuestCondition(this, cond, m_data.GetIndex(cond.CondictionID)));

            foreach (var cond in m_conditions)
                cond.OnAttached();
        }

        public ActivityQuestInfo Info
        {
            get
            {
                return this.m_quest;
            }
        }

        public ActiveQuestUserData Data
        {
            get
            {
                return this.m_data;
            }
        }

        public GamePlayer Owner
        {
            get
            {
                return this.m_player;
            }
        }

        public bool CanComplete()
        {
            if (this.Data.IsCompleted)
                return this.Data.IsCompleted;
            else
            {
                UpdateConditions();
                bool data = m_conditions.Count(s => s.IsCompleted()) == m_conditions.Count();
                if (data)
                    this.Data.IsCompleted = true;
                return data;
            }

        }

        public void UpdateConditions()
        {
            foreach (var cond in m_conditions)
                if (this.m_data.GetIndex(cond.info.CondictionID) != cond.Value)
                    this.m_data.SetIndex(cond.info.CondictionID, cond.Value);
        }
    }
}
