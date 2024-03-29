﻿using System;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Bussiness;

namespace GameServerScript.AI.NPC
{
    public class ThirdHardBloomNpcS : ABrain
    {
        private int addBloom = 800;

        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
            Body.PlayMovie("", 0, 1000);
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            int rand = Game.Random.Next(0, 5);
            if (rand == 0)
            {
                string msg;
                int index = Game.Random.Next(0, AntChat.Length);
                msg = AntChat[index];
                m_body.Say(msg, 0, 1000);
            }
        }

        public override void OnCreated()
        {
            base.OnCreated();
        }

        public override void OnStartAttacking()
        {
            base.OnStartAttacking();
            int count = 0;
            foreach (Player player in Game.GetAllFightPlayers())
            {
                if (player.IsLiving && player.X > Body.X - 100 && player.X < Body.X + 100)
                {
                    count++;
                }
            }
            Body.PlayMovie("renew", 0, 0);
            foreach (Player player in Game.GetAllFightPlayers())
            {
                if (player.IsLiving && player.X > Body.X - 100 && player.X < Body.X + 100)
                {
                    player.AddBlood(addBloom / count);
                }
            }
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }

        public override void OnKillPlayerSay()
        {
            base.OnKillPlayerSay();
        }

        public override void OnDiedSay()
        {
        }

        public override void OnShootedSay(int delay)
        {
        }

        #region NPC 小怪说话

        private static Random random = new Random();
        private static string[] AntChat = new string[] {
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdHardBloomNpcS.msg1"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdHardBloomNpcS.msg2"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdHardBloomNpcS.msg3"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdHardBloomNpcS.msg4"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdHardBloomNpcS.msg5"),
        };
        #endregion
    }
}
