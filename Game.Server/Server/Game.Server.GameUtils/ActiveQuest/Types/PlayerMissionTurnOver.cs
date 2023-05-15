using Game.Logic;
using SqlDataProvider.Data;

namespace Game.Server.GameUtils.ActiveQuest.Types
{
    public class PlayerMissionTurnOver : BaseActiveQuestCondition
    {

        public PlayerMissionTurnOver(BaseActivityQuest quest, AtivityQuestConditionInfo Data, int value) : base(quest, Data, value)
        {
        }

        public override void OnAttached()
        {
            this.m_quest.Owner.MissionTurnOver += new GamePlayer.PlayerMissionTurnOverEventHandle(this.missionTurnOver);
        }

        public override void OnRemoved()
        {
            this.m_quest.Owner.MissionTurnOver -= new GamePlayer.PlayerMissionTurnOverEventHandle(this.missionTurnOver);
        }

        private void missionTurnOver(AbstractGame game, int missionId, int turnCount)
        {
            if (missionId == this.info.Para1)
            {
                this.Value = 1;
            }
        }

        public override bool IsCompleted()
        {
            return this.Value >= this.info.Para2;
        }
    }
}
