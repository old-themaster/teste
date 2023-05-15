﻿using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;
using Bussiness;

namespace GameServerScript.AI.NPC
{
    public class NewTrainingBoss22001 : ABrain
    {
        private int m_attackTurn = 0;

        public int currentCount = 0;

        public int Dander = 0;        

        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();

            Body.CurrentDamagePlus = 1;
            Body.CurrentShootMinus = 1;
            Body.SetRect(((SimpleBoss)Body).NpcInfo.X, ((SimpleBoss)Body).NpcInfo.Y, ((SimpleBoss)Body).NpcInfo.Width, ((SimpleBoss)Body).NpcInfo.Height);

            if (Body.Direction == -1)
            {
                Body.SetRect(((SimpleBoss)Body).NpcInfo.X, ((SimpleBoss)Body).NpcInfo.Y, ((SimpleBoss)Body).NpcInfo.Width, ((SimpleBoss)Body).NpcInfo.Height);
            }
            else
            {
                Body.SetRect(-((SimpleBoss)Body).NpcInfo.X - ((SimpleBoss)Body).NpcInfo.Width, ((SimpleBoss)Body).NpcInfo.Y, ((SimpleBoss)Body).NpcInfo.Width, ((SimpleBoss)Body).NpcInfo.Height);
            }

        }
        public override void OnDiedSay()
        {
            Body.Say(LanguageMgr.GetTranslation("GameServerScript.AI.NPC.NewTrainingBoss22001.died"), 1, 0, 1500);           

        }
        public override void OnCreated()
        {
            base.OnCreated();
        }

        public override void OnStartAttacking()
        {
            Body.Direction = Game.FindlivingbyDir(Body);
            if (m_attackTurn < 2)
            {
                NextAttack();
                m_attackTurn++;
            }
            else
            {
                PersonalAttack();
            }
        }
        private void PersonalAttack()
        {   
            int dis = Game.Random.Next(20, 50);
            //int direction = Body.Direction;
            int[] direction = { 1, -1 };
            if (dis > 30)
            {
                Body.MoveTo(Body.X + dis * direction[1], Body.Y, "happy", "", ((SimpleBoss)Body).NpcInfo.speed, 1000, new LivingCallBack(NextAttack));

            }
            else
            {
                Body.MoveTo(Body.X + dis * direction[0], Body.Y, "happy", "", ((SimpleBoss)Body).NpcInfo.speed, 1000, new LivingCallBack(NextAttack));
            }
            //Body.ChangeDirection(Game.FindlivingbyDir(Body), 5000);
        }
        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }
        
        private void NextAttack()
        {
            Player target = Game.FindRandomPlayer();

            Body.SetRect(0, 0, 0, 0);
            if (target.X > Body.Y)
            {
                Body.ChangeDirection(1, 500);
            }
            else
            {
                Body.ChangeDirection(-1, 500);
            }

            Body.CurrentDamagePlus = 1.8f;

            if (target != null)
            {
                int mtX = Game.Random.Next(target.X - 50, target.X + 50);

                if (Body.ShootPoint(mtX, target.Y, ((SimpleBoss)Body).NpcInfo.CurrentBallId, 1000, 10000, 1, 1, 2200))
                {
                    Body.PlayMovie("beat", 1700, 0);
                }
            }
        }
                

    }
}
