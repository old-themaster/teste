using Bussiness;
using Game.Base.Packets;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;

namespace Game.Logic.Actions
{
	public class LivingRangeAttackingAction3 : BaseAction
	{
		private Living m_living;

		private List<Living> m_Livings;

		private int m_fx;

		private int m_tx;

		private string m_action;

		private int m_type;

		private static ThreadSafeRandom random = new ThreadSafeRandom();

		public LivingRangeAttackingAction3(Living living, int fx, int tx, string action, int delay, List<Living> livings, int type)
			: base(delay, 1500)
		{
			m_living = living;
			m_Livings = livings;
			m_fx = fx;
			m_tx = tx;
			m_action = action;
			m_type = type;
		}

		private int MakeDamage(Living p)
		{
			double baseDamage = m_living.BaseDamage;
			double num = p.BaseGuard;
			double num2 = p.Defence;
			double attack = m_living.Attack;
			if (m_living.IgnoreArmor)
			{
				num = 0.0;
				num2 = 0.0;
			}
			float currentDamagePlus = m_living.CurrentDamagePlus;
			float currentShootMinus = m_living.CurrentShootMinus;
			double num3 = 0.95 * (num - (double)(3 * m_living.Grade)) / (500.0 + num - (double)(3 * m_living.Grade));
			double num4 = (!(num2 - m_living.Lucky < 0.0)) ? (0.95 * (num2 - m_living.Lucky) / (600.0 + num2 - m_living.Lucky)) : 0.0;
			double num5 = baseDamage * (1.0 + attack * 0.001) * (1.0 - (num3 + num4 - num3 * num4)) * (double)currentDamagePlus * (double)currentShootMinus;
			double num6 = Math.Abs(p.GetDirectDemageRect().X - m_living.X);
			num5 *= 1.1 - num6 / (double)Math.Abs(m_tx - m_fx) / 5.0;
			if (num5 < 0.0)
			{
				return 1;
			}
			return (int)num5;
		}

		private int MakeCriticalDamage(Living p, int baseDamage)
		{
			double lucky = m_living.Lucky;
			if (lucky * 70.0 / (2000.0 + lucky) > (double)random.Next(100))
			{
				return (int)((0.5 + lucky * 0.0003) * (double)baseDamage);
			}
			return 0;
		}

		protected override void ExecuteImp(BaseGame game, long tick)
		{
			GSPacketIn gSPacketIn = new GSPacketIn(91);
			gSPacketIn.Parameter1 = m_living.Id;
			gSPacketIn.WriteByte(61);
			List<Living> list = game.Map.FindLiving(m_fx, m_tx, m_Livings);
			List<Living> list2 = new List<Living>();
			foreach (Living item in list)
			{
				if (((PVEGame)game).CanCampAttack(m_living, item))
				{
					list2.Add(item);
				}
			}
			int num = list2.Count;
			foreach (Living item2 in list2)
			{
				if (m_living.IsFriendly(item2))
				{
					num--;
				}
			}
			gSPacketIn.WriteInt(num);
			m_living.SyncAtTime = false;
			try
			{
				foreach (Living item3 in list2)
				{
					item3.SyncAtTime = false;
					if (!m_living.IsFriendly(item3))
					{
						int val = 0;
						item3.IsFrost = false;
						game.SendGameUpdateFrozenState(item3);
						int damageAmount = MakeDamage(item3);
						int criticalAmount = MakeCriticalDamage(item3, damageAmount);
						int val2 = 0;
						if (item3.TakeDamage(m_living, ref damageAmount, ref criticalAmount, "", 1000))
						{
							val2 = damageAmount + criticalAmount;
							if (item3 is Player)
							{
								val = (item3 as Player).Dander;
							}
						}
						gSPacketIn.WriteInt(item3.Id);
						gSPacketIn.WriteInt(val2);
						gSPacketIn.WriteInt(item3.Blood);
						gSPacketIn.WriteInt(val);
						gSPacketIn.WriteInt((criticalAmount == 0) ? 1 : 2);
					}
				}
				game.SendToAll(gSPacketIn);
				Finish(tick);
			}
			finally
			{
				m_living.SyncAtTime = true;
				foreach (Living item4 in list2)
				{
					item4.SyncAtTime = true;
				}
			}
		}
	}
}
