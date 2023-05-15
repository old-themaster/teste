using SqlDataProvider.Data;

namespace Game.Server.GameUtils.ActiveQuest.Types
{
    public class GradeActivityCondition : BaseActiveQuestCondition
    {
        public GradeActivityCondition(BaseActivityQuest quest, AtivityQuestConditionInfo Data, int value) : base(quest, Data, value)
        {
            if (quest.Owner.PlayerCharacter.Grade > Data.Para2)
                this.Value = Data.Para2;
            else
                this.Value = quest.Owner.PlayerCharacter.Grade;
        }

        public override void OnAttached()
        {
            this.m_quest.Owner.LevelUp += onlevelup;
        }

        private void onlevelup(GamePlayer player)
        {
            if (m_quest.Owner.PlayerCharacter.Grade > this.info.Para2)
                this.Value = this.info.Para2;
            else
                this.Value = m_quest.Owner.PlayerCharacter.Grade;
        }

        public override void OnRemoved()
        {
            this.m_quest.Owner.LevelUp -= onlevelup;
        }

        public override bool IsCompleted()
        {
            return this.Value >= this.info.Para2 ;
        }
    }
}
