using SqlDataProvider.Data;

namespace Game.Server.GameUtils.ActiveQuest.Types
{
    public class OwnMarryCondition : BaseActiveQuestCondition
    {

        public OwnMarryCondition(BaseActivityQuest quest, AtivityQuestConditionInfo Data, int value) : base(quest, Data, value)
        {
        }

        public override void OnAttached()
        {
        }

        public override void OnRemoved()
        {
        }

        public override bool IsCompleted()
        {
            if (this.Value == 0 && m_quest.Owner.PlayerCharacter.IsMarried)
                this.Value = this.info.Para2;

            return this.Value >= this.info.Para2;
        }
    }
}
