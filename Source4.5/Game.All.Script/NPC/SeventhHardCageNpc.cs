﻿using Game.Logic.AI;
using Bussiness;

namespace GameServerScript.AI.NPC
{
    public class SeventhHardCageNpc : ABrain
    {
        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();

            Body.CurrentDamagePlus = 1;
            Body.CurrentShootMinus = 1;
        }

        public override void OnCreated()
        {
            base.OnCreated();
        }

        public override void OnStartAttacking()
        {
            base.OnStartAttacking();
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }

        public override void OnDie()
        {
            base.OnDie();
            Body.Say(LanguageMgr.GetTranslation("GameServerScript.AI.NPC.SeventhCageNpc.msg1"), 0, 1000);
        }
    }
}
