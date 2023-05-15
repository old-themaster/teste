﻿using System;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Bussiness;

namespace GameServerScript.AI.NPC
{
    public class ThirdNormalFixureLongNpc : ABrain
    {
        protected Living attack;
        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            if (m_body.IsSay)
            {
                string msg;
                int index = Game.Random.Next(0, AntChat.Length);
                msg = AntChat[index];
                m_body.Say(msg, 0, 2000);
            }
        }

        public override void OnCreated()
        {
            base.OnCreated();
        }


        public override void OnStartAttacking()
        {
            base.OnStartAttacking();
            attack = Game.FindRandomPlayer();
            NextAttack();
        }

        private void NextAttack()
        {
            if (attack != null)
            {
                if (attack.X > Body.X)
                {
                    Body.ChangeDirection(1, 800);
                }
                else
                {
                    Body.ChangeDirection(-1, 800);
                }

                int mtX = Game.Random.Next(attack.X - 10, attack.X + 10);

                Body.ShootPoint(mtX, attack.Y, 58, 1000, 10000, 1, 2.0f, 2300);
            }
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }

        #region NPC 小怪说话

        private static Random random = new Random();

        private static string[] AntChat = new string[] {
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdNormalFixureLongNpc.msg1"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdNormalFixureLongNpc.msg2"),
        };
        #endregion
    }
}
