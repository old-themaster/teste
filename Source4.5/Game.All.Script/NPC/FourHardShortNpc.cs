﻿using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;
using Bussiness;
using GameServerScript.AI.Messions;

namespace GameServerScript.AI.NPC
{
    public class FourHardShortNpc : ABrain
    {
        private string[] NormalSay =
        {
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourHardShortNpc.msg1"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourHardShortNpc.msg2"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourHardShortNpc.msg3"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourHardShortNpc.msg4"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.FourHardShortNpc.msg5")
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

            if (Game.Random.Next(100) < 50)
            {
                int place = Game.Random.Next(0, NormalSay.Length);
                Body.Say(NormalSay[place], 1, 0);
            }
        }

        public override void OnCreated()
        {
            base.OnCreated();
        }

        public override void OnStartAttacking()
        {
            base.OnStartAttacking();

            SimpleBoss enemy = (((PVEGame)Game).MissionAI as DKH4201).Helper;

            int randX = Game.Random.Next(-2000, 200);

            Body.MoveTo(enemy.X + randX, enemy.Y, "walk", 1500);
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }
    }
}
