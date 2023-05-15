using Game.Logic.AI;
using Game.Logic.AI.Npc;
using Game.Server.Managers;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Game.Logic.Phy.Object
{
	public class SimpleNpc : Living
	{
		private NpcInfo m_npcInfo;

		private ABrain m_ai;

		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private int m_rank;

		public NpcInfo NpcInfo => m_npcInfo;

		public int Rank => m_rank;

		public SimpleNpc(int id, BaseGame game, NpcInfo npcInfo, int type, int direction)
			: base(id, game, npcInfo.Camp, npcInfo.Name, npcInfo.ModelID, npcInfo.Blood, npcInfo.Immunity, direction)
		{
			switch (type)
			{
			case 0:
				base.Type = eLivingType.SimpleNpc;
				break;
			case 1:
				base.Type = eLivingType.ClearEnemy;
				break;
			default:
				base.Type = eLivingType.SimpleNpc;
				break;
			}
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
				log.ErrorFormat("SimpleNpc Created error:{1}", arg);
			}
		}

		public SimpleNpc(int id, BaseGame game, NpcInfo npcInfo, int type, int direction, string action)
			: base(id, game, npcInfo.Camp, npcInfo.Name, npcInfo.ModelID, npcInfo.Blood, npcInfo.Immunity, direction)
		{
			if (type == 0)
			{
				base.Type = eLivingType.SimpleNpc;
			}
			else
			{
				base.Type = eLivingType.SimpleNpc1;
			}
			m_npcInfo = npcInfo;
			base.ActionStr = action;
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
				log.ErrorFormat("SimpleNpc Created error:{1}", arg);
			}
		}

		public SimpleNpc(int id, BaseGame game, NpcInfo npcInfo, int type, int direction, int rank)
			: base(id, game, npcInfo.Camp, npcInfo.Name, npcInfo.ModelID, npcInfo.Blood, npcInfo.Immunity, direction)
		{
			if (type == 0)
			{
				base.Type = eLivingType.SimpleNpc;
			}
			else
			{
				base.Type = eLivingType.SimpleNpc1;
			}
			m_npcInfo = npcInfo;
			base.ActionStr = "";
			m_ai = (ScriptMgr.CreateInstance(npcInfo.Script) as ABrain);
			if (m_ai == null)
			{
				log.ErrorFormat("Can't create abrain :{0}", npcInfo.Script);
				m_ai = SimpleBrain.Simple;
			}
			m_ai.Game = m_game;
			m_ai.Body = this;
			m_rank = rank;
			try
			{
				m_ai.OnCreated();
			}
			catch (Exception arg)
			{
				log.ErrorFormat("SimpleNpc Created error:{1}", arg);
			}
		}

		public override void Reset()
		{
			Agility = m_npcInfo.Agility;
			Attack = m_npcInfo.Attack;
			BaseDamage = m_npcInfo.BaseDamage;
			BaseGuard = m_npcInfo.BaseGuard;
			Lucky = m_npcInfo.Lucky;
			Grade = m_npcInfo.Level;
			Experience = m_npcInfo.Experience;
			SetRect(m_npcInfo.X, m_npcInfo.Y, m_npcInfo.Width, m_npcInfo.Height);
			SetRelateDemagemRect(m_npcInfo.X, m_npcInfo.Y, m_npcInfo.Width, m_npcInfo.Height);
			base.Reset();
		}

		public void GetDropItemInfo()
		{
			if (m_game.CurrentLiving is Player)
			{
				Player player = m_game.CurrentLiving as Player;
				List<ItemInfo> info = null;
				int gold = 0;
				int money = 0;
				int giftToken = 0;
				DropInventory.NPCDrop(m_npcInfo.DropId, ref info);
				if (info != null)
				{
					foreach (ItemInfo item in info)
					{
						ItemInfo itemInfo = ItemInfo.FindSpecialItemInfo(item, ref gold, ref money, ref giftToken);
						if (itemInfo != null)
						{
							if (itemInfo.Template.CategoryID == 10)
							{
								player.PlayerDetail.AddTemplate(itemInfo, eBageType.FightBag, item.Count, eGameView.dungeonTypeGet);
							}
							else
							{
								player.PlayerDetail.AddTemplate(itemInfo, eBageType.TempBag, item.Count, eGameView.dungeonTypeGet);
							}
						}
					}
					player.PlayerDetail.AddGold(gold);
					player.PlayerDetail.AddMoney(money);
					player.PlayerDetail.AddGiftToken(giftToken);
				}
			}
		}

		public override void Die()
		{
			GetDropItemInfo();
			base.Die();
		}

		public override void Die(int delay)
		{
			GetDropItemInfo();
			base.Die(delay);
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
				log.ErrorFormat("SimpleNpc BeginNewTurn error:{0}", arg);
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
				log.ErrorFormat("SimpleNpc StartAttacking error:{0}", arg);
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
				log.ErrorFormat("SimpleNpc Dispose error:{0}", arg);
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
				log.ErrorFormat("SimpleNpc DiedSay error {0}", arg);
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
				log.ErrorFormat("SimpleNpc DiedEvent error {0}", arg);
			}
		}

		public void OnDie()
		{
			try
			{
				m_ai.OnDie();
			}
			catch (Exception arg)
			{
				log.ErrorFormat("SimpleNpc OnDie error {0}", arg);
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

		public override void OnAfterTakedFrozen()
		{
			try
			{
				m_ai.OnAfterTakedFrozen();
			}
			catch (Exception arg)
			{
				log.ErrorFormat("SimpleBoss OnAfterTakedFrozen error:{1}", arg);
			}
		}
	}
}
