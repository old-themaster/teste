﻿using Game.Logic.AI;
namespace GameServerScript.AI.Game
{
    public class WarriorsArenaTerrorGame : APVEGameControl
    {
        public override void OnCreated()
        {
            base.OnCreated();
            Game.SetupMissions("13301,13302,13303,13304");
            Game.TotalMissionCount = 4;
        }

        public override void OnPrepated()
        {
            base.OnPrepated();
            Game.SessionId = 0;
        }

        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);

            if (score > 800)
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