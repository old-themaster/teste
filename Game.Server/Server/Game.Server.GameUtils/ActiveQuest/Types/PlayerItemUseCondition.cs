using SqlDataProvider.Data;

namespace Game.Server.GameUtils.ActiveQuest.Types
{
    public class PlayerItemUseCondition : BaseActiveQuestCondition
    {
        public PlayerItemUseCondition(BaseActivityQuest quest, AtivityQuestConditionInfo Data, int value) : base(quest, Data, value)
        {
        }

        public override void OnAttached()
        {
            this.m_quest.Owner.AfterUsingItem += onitemuse;
        }

        public override void OnRemoved()
        {
            this.m_quest.Owner.AfterUsingItem -= onitemuse;
        }

        private void onitemuse(int templateID, int count)
        {
            if (templateID != this.info.Para1)
            {
                return;
            }
                
            this.Value = this.info.Para2;
        }

        public override bool IsCompleted()
        {
            return this.Value >= this.info.Para2;
        }
    }
}
