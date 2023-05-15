using SqlDataProvider.Data;

namespace Game.Server.GameUtils.ActiveQuest.Types
{
    public class ItemComposeCondition : BaseActiveQuestCondition
    {
        public ItemComposeCondition(BaseActivityQuest quest, AtivityQuestConditionInfo Data, int value) : base(quest, Data, value)
        {
        }

        public override void OnAttached()
        {
            this.m_quest.Owner.ItemCompose += onCompose;
        }

        public override void OnRemoved()
        {
            this.m_quest.Owner.ItemCompose -= onCompose;
        }

        private void onCompose(int itemID)
        {
            this.Value = (itemID == this.info.Para1) ? 1 : 0;
        }

        public override bool IsCompleted()
        {
            return this.Value >= this.info.Para2;
        }
    }
}
