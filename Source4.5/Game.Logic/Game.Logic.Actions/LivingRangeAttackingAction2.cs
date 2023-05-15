using Game.Base.Packets;
using Game.Logic.Phy.Object;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Game.Logic.Actions
{
	public class LivingRangeAttackingAction2 : BaseAction
	{
		private bool bool_0;

		private bool bool_1;

		private int int_x;

		private int int_y;

		private List<Player> list_player;

		private Living living_0;

		private string string_0;

		public LivingRangeAttackingAction2(Living living, int fx, int tx, string action, int delay, bool removeFrost, bool directDamage, List<Player> players)
			: base(delay, 1000)
		{
			living_0 = living;
			list_player = players;
			int_x = fx;
			int_y = tx;
			string_0 = action;
			bool_0 = removeFrost;
			bool_1 = directDamage;
		}

		protected override void ExecuteImp(BaseGame game, long tick)
		{
			GSPacketIn gSPacketIn = new GSPacketIn(91, living_0.Id)
			{
				Parameter1 = living_0.Id
			};
			gSPacketIn.WriteByte(61);
			List<Living> list = game.Map.FindPlayers(int_x, int_y, list_player);
			int num = list.Count;
			foreach (Living item in list)
			{
				if (living_0.IsFriendly(item) || ((item is SimpleBoss || item is SimpleNpc) && !item.Config.CanTakeDamage))
				{
					num--;
				}
			}
			gSPacketIn.WriteInt(num);
			living_0.SyncAtTime = false;
			try
			{
				foreach (Living item2 in list)
				{
					item2.SyncAtTime = false;
					if ((!living_0.IsFriendly(item2) && ((!(item2 is SimpleBoss) && !(item2 is SimpleNpc)) || item2.Config.CanTakeDamage)) || item2.Config.IsHelper)
					{
						int val = 0;
						if (item2.IsFrost)
						{
							item2.IsFrost = false;
							game.method_30(item2);
							Finish(tick);
							return;
						}
						int damageAmount = method_0(item2);
						int criticalAmount = method_1(item2, damageAmount);
						int val2 = 0;
						if (item2.TakeDamage(living_0, ref damageAmount, ref criticalAmount, "范围攻击", 0))
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
						gSPacketIn.WriteInt(item2.IsLiving ? 1 : 6);
					}
				}
				game.SendToAll(gSPacketIn);
				Finish(tick);
			}
			finally
			{
				living_0.SyncAtTime = true;
				foreach (Living item3 in list)
				{
					item3.SyncAtTime = true;
				}
			}
		}

		private int method_0(Living living_1)
		{
			double baseGuard = living_1.BaseGuard;
			double defence = living_1.Defence;
			double attack = living_0.Attack;
			if (living_1.AddArmor && (living_1 as Player).DeputyWeapon != null)
			{
				int num = (living_1 as Player).DeputyWeapon.Template.Property7 + (int)Math.Pow(1.1, (living_1 as Player).DeputyWeapon.StrengthenLevel);
				baseGuard += (double)num;
				defence += (double)num;
			}
			if (living_0.IgnoreArmor)
			{
				baseGuard = 0.0;
				defence = 0.0;
			}
			float currentDamagePlus = living_0.CurrentDamagePlus;
			float currentShootMinus = living_0.CurrentShootMinus;
			double num2 = 0.95 * (living_1.BaseGuard - (double)(3 * living_0.Grade)) / (500.0 + living_1.BaseGuard - (double)(3 * living_0.Grade));
			double num3 = 0.0;
			num3 = ((!(living_1.Defence - living_0.Lucky < 0.0)) ? (0.95 * (living_1.Defence - living_0.Lucky) / (600.0 + living_1.Defence - living_0.Lucky)) : 0.0);
			double num4 = living_0.BaseDamage * (1.0 + attack * 0.001) * (1.0 - (num2 + num3 - num2 * num3)) * (double)currentDamagePlus * (double)currentShootMinus;
			if (!bool_1)
			{
				Rectangle directDemageRect = living_1.GetDirectDemageRect();
				double num5 = Math.Sqrt((directDemageRect.X - living_0.X) * (directDemageRect.X - living_0.X) + (directDemageRect.Y - living_0.Y) * (directDemageRect.Y - living_0.Y));
				num4 *= 1.0 - num5 / (double)Math.Abs(int_y - int_x) / 4.0;
			}
			if (num4 <= 0.0)
			{
				return 1;
			}
			return (int)num4;
		}

		private int method_1(Living living_1, int int_2)
		{
			double lucky = living_0.Lucky;
			Random random = new Random();
			if (75000.0 * lucky / (lucky + 800.0) > (double)random.Next(100000))
			{
				int num = 0;
				return (int)((0.5 + lucky * 0.0003) * (double)int_2) * (100 - num) / 100;
			}
			return 0;
		}
	}
}
