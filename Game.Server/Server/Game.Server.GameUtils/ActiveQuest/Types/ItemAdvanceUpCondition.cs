using SqlDataProvider.Data;

namespace Game.Server.GameUtils.ActiveQuest.Types
{
    public class ItemAdvanceUpCondition : BaseActiveQuestCondition
    {
        public ItemAdvanceUpCondition(BaseActivityQuest quest, AtivityQuestConditionInfo Data, int value) : base(quest, Data, value)
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
            int levelCn = level - 12;

            int currentLevel = (levelCn > 0) ? levelCn : 0;

            if (categoryID == this.info.Para1)
            {
                if (currentLevel >= this.info.Para2)
                    this.Value = this.info.Para2;
                else
                    this.Value = currentLevel;
            }
        }

        public override bool IsCompleted()
        {
            return this.Value >= this.info.Para2;
        }
    }
}
