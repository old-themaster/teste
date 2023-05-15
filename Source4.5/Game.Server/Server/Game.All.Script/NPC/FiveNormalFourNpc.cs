﻿using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.NPC
{
    public class FiveNormalFourNpc : ABrain
    {
        protected Player m_targer;

        private static string[] AttackChat = new string[] {
                    ""
                };
        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            m_body.CurrentDamagePlus = 1;
            m_body.CurrentShootMinus = 1;
        }

        public override void OnCreated()
        {
            base.OnCreated();
            Body.MaxBeatDis = 200;
        }

        public override void OnStartAttacking()
        {
            base.OnStartAttacking();
            m_targer = Game.FindNearestPlayer(Body.X, Body.Y);

            if(m_targer != null)
            {
                Body.ChangeDirection(m_targer, 500);
                Body.MoveTo(m_targer.X, m_targer.Y - 20, "fly", 1000, BeatTarget, 5);
            }
        }

        private void BeatTarget()
        {
            Body.Beat(m_targer, "beatA", 100, 0, 500);
        }

        public override void OnDie()
        {
            base.OnDie();
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }
    }
}
