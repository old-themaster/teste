using Game.Logic;
using SqlDataProvider.Data;

namespace Game.Server.GameUtils.ActiveQuest.Types
{
    public class PlayerTexpCondition : BaseActiveQuestCondition
    {
        public PlayerTexpCondition(BaseActivityQuest quest, AtivityQuestConditionInfo Data, int value) : base(quest, Data, value)
        {
        }

        public override void OnAttached()
        {
            this.m_quest.Owner.AfterUsingItem += onusetexp;
        }

        public override void OnRemoved()
        {
            this.m_quest.Owner.AfterUsingItem -= onusetexp;
        }

        private void onusetexp(int templateID, int count)
        {
            var level = 0;
            if (this.info.Para1 == 1 && templateID == 45005)
                level = ExerciseMgr.GetExerciseinfoByLevelExp(this.m_quest.Owner.PlayerCharacter.Texp.hpTexpExp).Grage;
            if (this.info.Para1 == 2 && templateID == 45001)
                level = ExerciseMgr.GetExerciseinfoByLevelExp(this.m_quest.Owner.PlayerCharacter.Texp.attTexpExp).Grage;
            if (this.info.Para1 == 3 && templateID == 45002)
               level = ExerciseMgr.GetExerciseinfoByLevelExp(this.m_quest.Owner.PlayerCharacter.Texp.defTexpExp).Grage;
            if (this.info.Para1 == 4 && templateID == 45003)
               level = ExerciseMgr.GetExerciseinfoByLevelExp(this.m_quest.Owner.PlayerCharacter.Texp.spdTexpExp).Grage;
            if (this.info.Para1 == 5 && templateID == 45004)
               level = ExerciseMgr.GetExerciseinfoByLevelExp(this.m_quest.Owner.PlayerCharacter.Texp.lukTexpExp).Grage;

            if(level != 0)
            {
                                    if (level < this.info.Para2)
                                        this.Value = level;
                                    else
                                        this.Value = this.info.Para2;
            }

        }

        public override bool IsCompleted()
        {
            return this.Value >= this.info.Para2;
        }
    }
}
