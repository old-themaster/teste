using Game.Base.Packets;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Game.Logic.Actions
{
	public class LivingRangeAttackingAction : BaseAction
	{
		private string m_action;

		private int m_fx;

		private Living m_living;

		private List<Player> m_players;

		private int m_tx;

		public LivingRangeAttackingAction(Living living, int fx, int tx, string action, int delay, List<Player> players)
			: base(delay, 1000)
		{
			m_living = living;
			m_players = players;
			m_fx = fx;
			m_tx = tx;
			m_action = action;
		}

		protected override void ExecuteImp(BaseGame game, long tick)
		{
			GSPacketIn gSPacketIn = new GSPacketIn(91, m_living.Id)
			{
				Parameter1 = m_living.Id
			};
			gSPacketIn.WriteByte(61);
			List<Living> list = game.Map.FindPlayers(m_fx, m_tx, m_players);
			int num = list.Count;
			foreach (Living item in list)
			{
				if (m_living.IsFriendly(item) || ((item is SimpleBoss || item is SimpleNpc) && !item.Config.CanTakeDamage))
				{
					num--;
				}
			}
			gSPacketIn.WriteInt(num);
			m_living.SyncAtTime = false;
			try
			{
				foreach (Living item2 in list)
				{
					item2.SyncAtTime = false;
					if (!m_living.IsFriendly(item2) && ((!(item2 is SimpleBoss) && !(item2 is SimpleNpc)) || item2.Config.CanTakeDamage))
					{
						int val = 0;
						item2.IsFrost = false;
						game.SendGameUpdateFrozenState(item2);
						int damageAmount = MakeDamage(item2);
						int criticalAmount = MakeCriticalDamage(item2, damageAmount);
						int val2 = 0;
						if (item2.TakeDamage(m_living, ref damageAmount, ref criticalAmount, "范围攻击", 0))
						{
							val2 = damageAmount + criticalAmount;
							if (item2 is Player)
							{
								val = (item2 as Player).Dander;
							}
						}
						gSPacketIn.WriteInt(item2.Id);
						gSPacketIn.WriteInt(val2);
						gSPacketIn.WriteInt(item2.Blood);
						gSPacketIn.WriteInt(val);
						gSPacketIn.WriteInt(1);
					}
				}
				game.SendToAll(gSPacketIn);
				Finish(tick);
			}
			finally
			{
				m_living.SyncAtTime = true;
				foreach (Living item3 in list)
				{
					item3.SyncAtTime = true;
				}
			}
		}

		private int MakeCriticalDamage(Living p, int baseDamage)
		{
			double lucky = m_living.Lucky;
			Random random = new Random();
			if (75000.0 * lucky / (lucky + 800.0) > (double)random.Next(100000))
			{
				int num = 0;
				return (int)((0.5 + lucky * 0.0003) * (double)baseDamage) * (100 - num) / 100;
			}
			return 0;
		}

		private int MakeDamage(Living p)
		{
			double baseDamage = m_living.BaseDamage;
			double num = p.BaseGuard;
			double num2 = p.Defence;
			double attack = m_living.Attack;
			if (p.AddArmor && (p as Player).DeputyWeapon != null)
			{
				int num3 = (p as Player).DeputyWeapon.Template.Property7 + (int)Math.Pow(1.1, (p as Player).DeputyWeapon.StrengthenLevel);
				num += (double)num3;
				num2 += (double)num3;
			}
			if (m_living.IgnoreArmor)
			{
				num = 0.0;
				num2 = 0.0;
			}
			float currentDamagePlus = m_living.CurrentDamagePlus;
			float currentShootMinus = m_living.CurrentShootMinus;
			double num4 = 0.95 * (num - (double)(3 * m_living.Grade)) / (500.0 + num - (double)(3 * m_living.Grade));
			double num5 = (!(num2 - m_living.Lucky < 0.0)) ? (0.95 * (num2 - m_living.Lucky) / (600.0 + num2 - m_living.Lucky)) : 0.0;
			double num6 = baseDamage * (1.0 + attack * 0.001) * (1.0 - (num4 + num5 - num4 * num5)) * (double)currentDamagePlus * (double)currentShootMinus;
			Rectangle directDemageRect = p.GetDirectDemageRect();
			double num7 = Math.Sqrt((directDemageRect.X - m_living.X) * (directDemageRect.X - m_living.X) + (directDemageRect.Y - m_living.Y) * (directDemageRect.Y - m_living.Y));
			num6 *= 1.0 - num7 / (double)Math.Abs(m_tx - m_fx) / 4.0;
			if (num6 < 0.0)
			{
				return 1;
			}
			return (int)num6;
		}
	}
}
