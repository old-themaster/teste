using Game.Logic;
using SqlDataProvider.Data;

namespace Game.Server.GameUtils.ActiveQuest.Types
{
    public class GameMonsterCondition : BaseActiveQuestCondition
    {

        public GameMonsterCondition(BaseActivityQuest quest, AtivityQuestConditionInfo Data, int value) : base(quest, Data, value)
        {
        }

        public override void OnAttached()
        {
            this.m_quest.Owner.AfterKillingLiving += new GamePlayer.PlayerGameKillEventHandel(this.onKillLiving);
        }

        public override void OnRemoved()
        {
            this.m_quest.Owner.AfterKillingLiving -= new GamePlayer.PlayerGameKillEventHandel(this.onKillLiving);
        }

        private void onKillLiving(AbstractGame game, int type, int id, bool isLiving, int demage, bool isSpanArea)
        {
            if (type != 2 || id != this.info.Para1 || isLiving)
            {
                return;
            }

            if (this.Value >= this.info.Para2)
            {
                this.Value = this.info.Para2;
                return;
            }
                
            ++this.Value;
        }

        public override bool IsCompleted()
        {
            return this.Value >= this.info.Para2;
        }
    }
}
