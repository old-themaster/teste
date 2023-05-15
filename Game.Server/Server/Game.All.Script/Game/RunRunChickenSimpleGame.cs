﻿using Game.Logic.AI;
namespace GameServerScript.AI.Game
{
    class RunRunChickenSimpleGame : APVEGameControl
    {
        public override void OnCreated()
        {
            base.OnCreated();
            Game.SetupMissions("7001,7004");
            Game.TotalMissionCount = 2;
        }

        public override void OnPrepated()
        {
            base.OnPrepated();
        }

        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);

            if (score > 900)
            {
                return 3;
            }
            else if (score > 825)
            {
                return 2;
            }
            else if (score > 725)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public override void OnGameOverAllSession()
        {
            base.OnGameOverAllSession();
        }
    }
}
