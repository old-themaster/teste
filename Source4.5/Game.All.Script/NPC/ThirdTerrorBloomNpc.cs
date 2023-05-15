﻿using System;
using Game.Logic.AI;
using Bussiness;

namespace GameServerScript.AI.NPC
{
    public class ThirdTerrorBloomNpc : ABrain
    {
        public override void OnBeginSelfTurn()
        {
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
        }

        public override void OnStartAttacking()
        {
        }

        public override void OnStopAttacking()
        {
        }

        public override void OnKillPlayerSay()
        {
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
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorBloomNpc.msg1"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorBloomNpc.msg2"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorBloomNpc.msg3"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorBloomNpc.msg4"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorBloomNpc.msg5"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorBloomNpc.msg6"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorBloomNpc.msg7"),
            LanguageMgr.GetTranslation("GameServerScript.AI.NPC.ThirdTerrorBloomNpc.msg8"),

        };
        #endregion
    }
}
