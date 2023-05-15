using SqlDataProvider.Data;

namespace Game.Server.GameUtils.ActiveQuest.Types
{
    public class PlayerBeadUpCondition : BaseActiveQuestCondition
    {
        public PlayerBeadUpCondition(BaseActivityQuest quest, AtivityQuestConditionInfo Data, int value) : base(quest, Data, value)
        {
        }

        public override void OnAttached()
        {
            this.m_quest.Owner.BeadUpEvent += onbeadup;
        }

        public override void OnRemoved()
        {
            this.m_quest.Owner.BeadUpEvent -= onbeadup;
        }

        private void onbeadup(int level)
        {
            if (level >= this.info.Para2)
                this.Value = this.info.Para2;
            else
                this.Value = level;
        }

        public override bool IsCompleted()
        {
            return this.Value >= this.info.Para2;
        }
    }
}
