using Game.Logic;
using SqlDataProvider.Data;

namespace Game.Server.GameUtils.ActiveQuest.Types
{
    public class GameFightByGameCondition : BaseActiveQuestCondition
    {

        public GameFightByGameCondition(BaseActivityQuest quest, AtivityQuestConditionInfo Data, int value) : base(quest, Data, value)
        {
        }

        public override void OnAttached()
        {
            this.m_quest.Owner.GameOver += new GamePlayer.PlayerGameOverEventHandle(this.onGameOver);
        }

        public override void OnRemoved()
        {
            this.m_quest.Owner.GameOver -= new GamePlayer.PlayerGameOverEventHandle(this.onGameOver);
        }

        private void onGameOver(AbstractGame game, bool isWin, int gainXp, bool isSpanArea, bool isCouple)
        {
            switch (game.GameType)
            {
                case eGameType.Free:
                    if ((this.info.Para1 == 0 || this.info.Para1 == -1) && this.Value < this.info.Para2)
                    {
                        ++this.Value;
                        break;
                    }
                    break;
                case eGameType.Guild:
                    if ((this.info.Para1 == 1 || this.info.Para1 == -1) && this.Value < this.info.Para2)
                    {
                        ++this.Value;
                        break;
                    }
                    break;
                case eGameType.Training:
                    if ((this.info.Para1 == 2 || this.info.Para1 == -1) && this.Value < this.info.Para2)
                    {
                        ++this.Value;
                        break;
                    }
                    break;
                case eGameType.ALL:
                    if ((this.info.Para1 == 4 || this.info.Para1 == -1) && this.Value < this.info.Para2)
                    {
                        ++this.Value;
                        break;
                    }
                    break;
                case eGameType.Exploration:
                    if ((this.info.Para1 == 5 || this.info.Para1 == -1) && this.Value < this.info.Para2)
                    {
                        ++this.Value;
                        break;
                    }
                    break;
                case eGameType.FightLib:
                    if ((this.info.Para1 == 8 || this.info.Para1 == -1) && this.Value < this.info.Para2)
                    {
                        ++this.Value;
                        break;
                    }
                    break;
                case eGameType.matchNpc:
                    if ((this.info.Para1 == 9 || this.info.Para1 == -1) && this.Value < this.info.Para2)
                    {
                        ++this.Value;
                        break;
                    }
                    break;
            }
            if (this.Value < this.info.Para2)
                return;
            this.Value = this.info.Para2;
        }

        public override bool IsCompleted()
        {
            return this.Value >= this.info.Para2;
        }
    }
}
