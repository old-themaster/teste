﻿using Game.Logic.AI;

namespace GameServerScript.AI.NPC
{
    public class NewTrainingNpc23001 : ABrain
    {
        private int dis = 0;
        public override void OnCreated()
        {
            base.OnCreated();
        }

        public override void OnStartAttacking()
        {
            base.OnStartAttacking();
            int[] direction = { 1, -1 };
            dis = Game.Random.Next(30, 90);

            Body.MoveTo(Body.X + dis * direction[Game.Random.Next(0, 1)], Body.Y, "walk", 3000, "", 3);

        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
        }
        
    }
}
