using Game.Logic;
using Game.Logic.Phy.Object;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;

namespace Game.Server.GameUtils.ActiveQuest.Types
{
    public class PlayerGameOverCondition : BaseActiveQuestCondition
    {

        public PlayerGameOverCondition(BaseActivityQuest quest, AtivityQuestConditionInfo Data, int value) : base(quest, Data, value)
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

        private void onGameOver(AbstractGame game, bool isWin, int gainXp, bool isSpanArea , bool isCouple)
        {
            bool Is2x2 = false;
            if (game is PVPGame p)
            {
              //  Is2x2 = p.GameIs(2);
            }

            switch (game.RoomType)
            {
                case eRoomType.Match:

                    if (this.info.Para1 == 0 && isWin && this.info.CondictionType == 6)
                    {
                        this.Value++;
                    }

                    if (this.info.Para1 == 2 && Is2x2 && this.info.CondictionType == 34)
                    {
                        this.Value++;
                    }

                    break;
            }


            if (this.Value >= this.info.Para2)
            {
                this.Value = this.info.Para2;
            }
        }

        public override bool IsCompleted()
        {
            return this.Value >= this.info.Para2;
        }
    }
}
