﻿using Game.Logic.AI;

namespace GameServerScript.AI.Game
{
    public class WarriorsArenaSimpleGame : APVEGameControl
    {
        public override void OnCreated()
        {
             Game.SetupMissions("13001, 13002, 13003, 13004");
        //    Game.SetupMissions("13003");
            Game.TotalMissionCount = 4;
        }

        public override void OnPrepated()
        {
            base.OnPrepated();
        }

        public override int CalculateScoreGrade(int score)
        {
            if (score > 800)
            {
                return 3;
            }
            else if (score > 725)
            {
                return 2;
            }
            else if (score > 650)
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
        }
    }
}
