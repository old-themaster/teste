﻿using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic.Effects;
using Game.Logic.Actions;
using System.Drawing;

namespace GameServerScript.AI.NPC
{
    public class ThirdHardLockNpc : ABrain
    {
        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            AddEffect();
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

        public override void OnKillPlayerSay()
        {
            base.OnKillPlayerSay();
        }

        public override void OnDiedEvent()
        {
            ClearEffect();
        }

        public override void OnDiedSay()
        {
            ClearEffect();
        }

        public override void OnShootedSay(int delay)
        {
        }


        private void AddEffect()
        {
            foreach (Player player in Game.GetAllFightPlayers())
            {
                if (player.IsLiving && player.X > Body.X - 2000 && player.X < Body.X + 2000)
                {
                    AbstractEffect effect = null;
                    effect = player.EffectList.GetOfType(eEffectType.LockDirectionEffect);
                    if (effect == null)
                        player.AddEffect(new LockDirectionEffect(10), 0);
                }
                else
                {
                    player.EffectList.StopEffect(typeof(LockDirectionEffect));
                }
            }
        }
        private void ClearEffect()
        {
            foreach (Player player in Game.GetAllFightPlayers())
            {
                player.EffectList.StopEffect(typeof(LockDirectionEffect));
            }
        }
    }
}
