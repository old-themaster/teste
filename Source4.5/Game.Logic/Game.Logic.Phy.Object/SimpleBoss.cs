using Game.Logic.AI;
using Game.Logic.AI.Npc;
using Game.Server.Managers;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace Game.Logic.Phy.Object
{
	public class SimpleBoss : TurnedLiving
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		protected NpcInfo m_npcInfo;

		private ABrain m_ai;

		private List<SimpleNpc> m_child = new List<SimpleNpc>();

		private List<SimpleBoss> m_childB = new List<SimpleBoss>();

		private Dictionary<Player, int> m_mostHateful;

		private List<SimpleNpc> m_fire;

		private List<SimpleBoss> m_boss;

		public NpcInfo NpcInfo => m_npcInfo;

		public List<SimpleNpc> Child => m_child;

		public List<SimpleBoss> ChildB => m_childB;

		public int CurrentLivingNpcNum
		{
			get
			{
				int num = 0;
				foreach (SimpleNpc item in Child)
				{
					if (item.IsLiving)
					{
						num++;
					}
				}
				foreach (SimpleBoss item2 in ChildB)
				{
					if (item2.IsLiving)
					{
						num++;
					}
				}
				return num;
			}
		}

		public List<SimpleBoss> Boss => m_boss;

		public List<SimpleNpc> Fire => m_fire;

		public SimpleBoss(int id, BaseGame game, NpcInfo npcInfo, int direction, int type, string actions)
			: base(id, game, npcInfo.Camp, npcInfo.Name, npcInfo.ModelID, npcInfo.Blood, npcInfo.Immunity, direction)
		{
			m_child = new List<SimpleNpc>();
			m_boss = new List<SimpleBoss>();
			m_fire = new List<SimpleNpc>();
			switch (type)
			{
			case 0:
				base.Type = eLivingType.SimpleBoss;
				break;
			case 1:
				base.Type = eLivingType.ClearEnemy;
				break;
			default:
				base.Type = (eLivingType)type;
				break;
			}
			base.ActionStr = actions;
			m_mostHateful = new Dictionary<Player, int>();
			m_npcInfo = npcInfo;
			m_ai = (ScriptMgr.CreateInstance(npcInfo.Script) as ABrain);
			if (m_ai == null)
			{
				log.ErrorFormat("Can't create abrain :{0}", npcInfo.Script);
				m_ai = SimpleBrain.Simple;
			}
			m_ai.Game = m_game;
			m_ai.Body = this;
			try
			{
				m_ai.OnCreated();
			}
			catch (Exception arg)
			{
				log.ErrorFormat("SimpleBoss Created error:{1}", arg);
			}
		}

		public override void Reset()
		{
			if (base.Config.IsWorldBoss)
			{
				this.m_maxBlood = 2147483647;
			}
			else
			{
				this.m_maxBlood = this.m_npcInfo.Blood;
			}
			m_maxBlood = m_npcInfo.Blood;
			BaseDamage = m_npcInfo.BaseDamage;
			BaseGuard = m_npcInfo.BaseGuard;
			Attack = m_npcInfo.Attack;
			Defence = m_npcInfo.Defence;
			Agility = m_npcInfo.Agility;
			Lucky = m_npcInfo.Lucky;
			Grade = m_npcInfo.Level;
			Experience = m_npcInfo.Experience;
			m_delay = m_npcInfo.Agility;
			SetRect(m_npcInfo.X, m_npcInfo.Y, m_npcInfo.Width, m_npcInfo.Height);
			SetRelateDemagemRect(m_npcInfo.X, m_npcInfo.Y, m_npcInfo.Width, m_npcInfo.Height);
			base.Reset();
		}

		public override void Die()
		{
			base.Die();
		}

		public override void Die(int delay)
		{
			base.Die(delay);
		}

		public override bool TakeDamage(Living source, ref int damageAmount, ref int criticalAmount, string msg, int delay)
		{
			bool flag = base.TakeDamage(source, ref damageAmount, ref criticalAmount, msg, delay);
			if (source is Player)
			{
				Player player = source as Player;
				int num = damageAmount + criticalAmount;
				if (m_mostHateful.ContainsKey(player))
				{
					Dictionary<Player, int> mostHateful = m_mostHateful;
					Player key = player;
					mostHateful[key] += num;
					return flag;
				}
				m_mostHateful.Add(player, num);
			}
			if (flag)
			{
				ShootedSay(delay);
			}
			return flag;
		}

		public void RandomSay(string[] msg, int type, int delay, int finishTime)
		{
			int num = base.Game.Random.Next(0, msg.Length);
			string msg2 = msg[num];
			Say(msg2, type, delay, finishTime);
		}

		public Player FindMostHatefulPlayer()
		{
			if (m_mostHateful.Count > 0)
			{
				KeyValuePair<Player, int> keyValuePair = m_mostHateful.ElementAt(0);
				foreach (KeyValuePair<Player, int> item in m_mostHateful)
				{
					if (keyValuePair.Value < item.Value)
					{
						keyValuePair = item;
					}
				}
				return keyValuePair.Key;
			}
			return null;
		}

		public void CreateChild(int id, int x, int y, int disToSecond, int maxCount, int direction)
		{
			if (CurrentLivingNpcNum < maxCount)
			{
				if (maxCount - CurrentLivingNpcNum >= 2)
				{
					Child.Add(((PVEGame)base.Game).CreateNpc(id, x + disToSecond, y, 1, direction));
					Child.Add(((PVEGame)base.Game).CreateNpc(id, x, y, 1, direction));
				}
				else if (maxCount - CurrentLivingNpcNum == 1)
				{
					Child.Add(((PVEGame)base.Game).CreateNpc(id, x, y, 1, direction));
				}
			}
		}

		public void CreateChild(int id, Point[] brithPoint, int maxCount, int maxCountForOnce, int type)
		{
			CreateChild(id, brithPoint, maxCount, maxCountForOnce, type, 0);
		}

		public void CreateChild(int id, Point[] brithPoint, int maxCount, int maxCountForOnce, int type, int objtype)
		{
			Point[] array = new Point[brithPoint.Length];
			if (CurrentLivingNpcNum >= maxCount)
			{
				return;
			}
			int num = (maxCount - CurrentLivingNpcNum < maxCountForOnce) ? (maxCount - CurrentLivingNpcNum) : maxCountForOnce;
			for (int i = 0; i < num; i++)
			{
				int num2 = 0;
				if (num <= brithPoint.Length)
				{
					int num3;
					for (num3 = 0; num3 < brithPoint.Length; num3++)
					{
						num2 = base.Game.Random.Next(0, brithPoint.Length);
						bool flag = false;
						for (int j = 0; j < array.Length; j++)
						{
							if (brithPoint[num2] == array[j])
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							array[num2] = brithPoint[num2];
							break;
						}
						num3--;
					}
				}
				if (objtype == 0)
				{
					Child.Add(((PVEGame)base.Game).CreateNpc(id, brithPoint[num2].X, brithPoint[num2].Y, type));
				}
				else
				{
					ChildB.Add(((PVEGame)base.Game).CreateBoss(id, brithPoint[num2].X, brithPoint[num2].Y, -1, type));
				}
			}
		}

		public SimpleNpc CreateChild(int id, int x, int y, bool showBlood, LivingConfig config)
		{
			return CreateChild(id, x, y, 0, -1, showBlood, config);
		}

		public SimpleNpc CreateChild(int id, int x, int y, int dir, bool showBlood, LivingConfig config)
		{
			return CreateChild(id, x, y, 0, dir, showBlood, config);
		}

		public SimpleNpc CreateChild(int id, int x, int y, int type, int dir, bool showBlood, LivingConfig config)
		{
			SimpleNpc simpleNpc = ((PVEGame)base.Game).CreateNpc(id, x, y, type, dir, config);
			Child.Add(simpleNpc);
			if (!showBlood)
			{
				base.Game.PedSuikAov(simpleNpc, 0);
			}
			return simpleNpc;
		}

		public void RandomConSay(string[] msg, int type, int delay, int finishTime)
		{
			if (base.Game.Random.Next(0, 2) == 1)
			{
				int num = base.Game.Random.Next(0, msg.Count());
				string msg2 = msg[num];
				Say(msg2, type, delay, finishTime);
			}
		}

		public void TowardsToPlayer(int playerX, int delay)
		{
			if (playerX > X)
			{
				ChangeDirection(1, delay);
			}
			else
			{
				ChangeDirection(-1, delay);
			}
		}

		public void RemoveAllChild()
		{
			foreach (SimpleNpc item in Child)
			{
				if (item.IsLiving)
				{
					item.Die();
				}
			}
			m_child = new List<SimpleNpc>();
		}

		public override void PrepareNewTurn()
		{
			base.PrepareNewTurn();
			try
			{
				m_ai.OnBeginNewTurn();
			}
			catch (Exception arg)
			{
				log.ErrorFormat("SimpleBoss BeginNewTurn error:{0}", arg);
			}
		}

		public override void PrepareSelfTurn()
		{
			base.PrepareSelfTurn();
			DefaultDelay = m_delay;
			AddDelay(m_npcInfo.Delay);
			try
			{
				m_ai.OnBeginSelfTurn();
			}
			catch (Exception arg)
			{
				log.ErrorFormat("SimpleBoss BeginSelfTurn error:{0}", arg);
			}
		}

		public override void StartAttacking()
		{
			base.StartAttacking();
			try
			{
				m_ai.OnStartAttacking();
			}
			catch (Exception arg)
			{
				log.ErrorFormat("SimpleBoss StartAttacking error:{0}", arg);
			}
			if (base.IsAttacking)
			{
				StopAttacking();
			}
		}

		public override void StopAttacking()
		{
			base.StopAttacking();
			try
			{
				m_ai.OnStopAttacking();
			}
			catch (Exception arg)
			{
				log.ErrorFormat("SimpleBoss StopAttacking error:{0}", arg);
			}
		}

		public override void Dispose()
		{
			base.Dispose();
			try
			{
				m_ai.Dispose();
			}
			catch (Exception arg)
			{
				log.ErrorFormat("SimpleBoss Dispose error:{0}", arg);
			}
		}

		public void KillPlayerSay()
		{
			try
			{
				m_ai.OnKillPlayerSay();
			}
			catch (Exception arg)
			{
				log.ErrorFormat("SimpleBoss Say error:{0}", arg);
			}
		}

		public void DiedSay()
		{
			try
			{
				m_ai.OnDiedSay();
			}
			catch (Exception arg)
			{
				log.ErrorFormat("SimpleBoss DiedSay error {0}", arg);
			}
		}

		public void DiedEvent()
		{
			try
			{
				m_ai.OnDiedEvent();
			}
			catch (Exception arg)
			{
				log.ErrorFormat("SimpleBoss DiedEvent error {0}", arg);
			}
		}

		public override void OnAfterTakedBomb()
		{
			try
			{
				m_ai.OnAfterTakedBomb();
			}
			catch (Exception arg)
			{
				log.ErrorFormat("SimpleBoss OnAfterTakedBomb error:{1}", arg);
			}
		}

		public void ShootedSay(int delay)
		{
			try
			{
				m_ai.OnShootedSay(delay);
			}
			catch (Exception arg)
			{
				log.ErrorFormat("SimpleBoss ShootedSay error {0}", arg);
			}
		}
	}
}
