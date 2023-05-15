using SqlDataProvider.Data;

namespace Game.Server.GameUtils.ActiveQuest.Types
{
    public class ItemStrengthenupCondition : BaseActiveQuestCondition
    {
        public ItemStrengthenupCondition(BaseActivityQuest quest, AtivityQuestConditionInfo Data, int value) : base(quest, Data, value)
        {
        }

        public override void OnAttached()
        {
            this.m_quest.Owner.ItemStrengthen += onlevelup;
        }

        public override void OnRemoved()
        {
            this.m_quest.Owner.ItemStrengthen -= onlevelup;
        }

        private void onlevelup(int categoryID, int level)
        {
            if (categoryID == this.info.Para1)
            {
                if (level > this.info.Para2)
                    this.Value = this.info.Para2;
                else
                    this.Value = level;
            }
 
        }

        public override bool IsCompleted()
        {
            return this.Value >= this.info.Para2;
        }
    }
}
