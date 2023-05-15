using Bussiness;
using Game.Base.Packets;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
namespace Game.Server.Buffer
{
    public class BufferList
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private object m_lock;
		protected List<AbstractBuffer> m_buffers;
		protected ArrayList m_clearList;
		protected volatile sbyte m_changesCount;
		private GamePlayer m_player;
		protected ArrayList m_changedBuffers = new ArrayList();
		private int m_changeCount;
		public BufferList(GamePlayer player)
		{
			this.m_player = player;
			this.m_lock = new object();
			this.m_buffers = new List<AbstractBuffer>();
			this.m_clearList = new ArrayList();
		}
		public void LoadFromDatabase(int playerId)
		{
			object @lock;
			Monitor.Enter(@lock = this.m_lock);
			try
			{
				using (PlayerBussiness playerBussiness = new PlayerBussiness())
				{
					BufferInfo[] userBuffer = playerBussiness.GetUserBuffer(playerId);
					this.BeginChanges();
					BufferInfo[] array = userBuffer;
					for (int i = 0; i < array.Length; i++)
					{
						BufferInfo info = array[i];
						AbstractBuffer abstractBuffer = BufferList.CreateBuffer(info);
						if (abstractBuffer != null)
						{
							abstractBuffer.Start(this.m_player);
						}
					}
					ConsortiaBufferInfo[] userConsortiaBuffer = playerBussiness.GetUserConsortiaBuffer(this.m_player.PlayerCharacter.ConsortiaID);
					ConsortiaBufferInfo[] array2 = userConsortiaBuffer;
					for (int j = 0; j < array2.Length; j++)
					{
						ConsortiaBufferInfo info2 = array2[j];
						AbstractBuffer abstractBuffer2 = BufferList.CreateConsortiaBuffer(info2);
						if (abstractBuffer2 != null)
						{
							abstractBuffer2.Start(this.m_player);
						}
					}
					this.CommitChanges();
				}
				this.Update();
				this.m_player.ClearFightBuffOneMatch();
			}
			finally
			{
				Monitor.Exit(@lock);
			}
		}
		public void SaveToDatabase()
		{
			object @lock;
			Monitor.Enter(@lock = this.m_lock);
			try
			{
				using (PlayerBussiness playerBussiness = new PlayerBussiness())
				{
					foreach (AbstractBuffer current in this.m_buffers)
					{
						playerBussiness.SaveBuffer(current.Info);
					}
					foreach (BufferInfo info in this.m_clearList)
					{
						playerBussiness.SaveBuffer(info);
					}
					this.m_clearList.Clear();
				}
			}
			finally
			{
				Monitor.Exit(@lock);
			}
		}
		public bool AddBuffer(AbstractBuffer buffer)
		{
			List<AbstractBuffer> buffers;
			Monitor.Enter(buffers = this.m_buffers);
			try
			{
				this.m_buffers.Add(buffer);
			}
			finally
			{
				Monitor.Exit(buffers);
			}
			this.OnBuffersChanged(buffer);
			return true;
		}
		public bool RemoveBuffer(AbstractBuffer buffer)
		{
			List<AbstractBuffer> buffers;
			Monitor.Enter(buffers = this.m_buffers);
			try
			{
				if (this.m_buffers.Remove(buffer))
				{
					this.m_clearList.Add(buffer.Info);
				}
			}
			finally
			{
				Monitor.Exit(buffers);
			}
			this.OnBuffersChanged(buffer);
			return true;
		}
		public void UpdateBuffer(AbstractBuffer buffer)
		{
			this.OnBuffersChanged(buffer);
		}
		protected void OnBuffersChanged(AbstractBuffer buffer)
		{
			if (!this.m_changedBuffers.Contains(buffer))
			{
				this.m_changedBuffers.Add(buffer);
			}
			if (this.m_changeCount <= 0 && this.m_changedBuffers.Count > 0)
			{
				this.UpdateChangedBuffers();
			}
		}
		public void BeginChanges()
		{
			Interlocked.Increment(ref this.m_changeCount);
		}
		public void CommitChanges()
		{
			int num = Interlocked.Decrement(ref this.m_changeCount);
			if (num < 0)
			{
				if (BufferList.log.IsErrorEnabled)
				{
					BufferList.log.Error("Inventory changes counter is bellow zero (forgot to use BeginChanges?)!\n\n" + Environment.StackTrace);
				}
				Thread.VolatileWrite(ref this.m_changeCount, 0);
			}
			if (num <= 0 && this.m_changedBuffers.Count > 0)
			{
				this.UpdateChangedBuffers();
			}
		}
		public void UpdateChangedBuffers()
		{
			List<BufferInfo> list = new List<BufferInfo>();
			Dictionary<int, BufferInfo> dictionary = new Dictionary<int, BufferInfo>();
			foreach (AbstractBuffer abstractBuffer in this.m_changedBuffers)
			{
				if (abstractBuffer.Info.TemplateID > 100)
				{
					list.Add(abstractBuffer.Info);
				}
			}
			List<AbstractBuffer> allBuffers = this.GetAllBuffers();
			foreach (AbstractBuffer current in allBuffers)
			{
				if (this.IsConsortiaBuff(current.Info.Type) && this.m_player.IsConsortia())
				{
					dictionary.Add(current.Info.TemplateID, current.Info);
				}
			}
			BufferInfo[] infos = list.ToArray();
			GSPacketIn pkg = this.m_player.Out.SendUpdateBuffer(this.m_player, infos);
			if (this.m_player.CurrentRoom != null)
			{
				this.m_player.CurrentRoom.SendToAll(pkg, this.m_player);
			}
			this.m_player.Out.SendUpdateConsotiaBuffer(this.m_player, dictionary);
			this.m_changedBuffers.Clear();
			dictionary.Clear();
		}
		public bool IsConsortiaBuff(int type)
		{
			return type > 100 && type < 115;
		}
		public bool UserSaveLifeBuff()
		{
			List<AbstractBuffer> buffers;
			Monitor.Enter(buffers = this.m_buffers);
			try
			{
				for (int i = 0; i < this.m_buffers.Count; i++)
				{
					if (this.m_buffers[i].Info.Type == 51 && this.m_buffers[i].Info.ValidCount > 0)
					{
						this.m_buffers[i].Info.ValidCount--;
						this.OnBuffersChanged(this.m_buffers[i]);
						return true;
					}
				}
			}
			finally
			{
				Monitor.Exit(buffers);
			}
			return false;
		}
		public virtual AbstractBuffer GetOfType(Type bufferType)
		{
			List<AbstractBuffer> buffers;
			Monitor.Enter(buffers = this.m_buffers);
			try
			{
				foreach (AbstractBuffer current in this.m_buffers)
				{
					if (current.GetType().Equals(bufferType))
					{
						return current;
					}
				}
			}
			finally
			{
				Monitor.Exit(buffers);
			}
			return null;
		}
		public List<AbstractBuffer> GetAllBufferByTemplate()
		{
			List<AbstractBuffer> list = new List<AbstractBuffer>();
			object @lock;
			Monitor.Enter(@lock = this.m_lock);
			try
			{
				foreach (AbstractBuffer current in this.m_buffers)
				{
					if (current.Info.TemplateID > 100)
					{
						list.Add(current);
					}
				}
			}
			finally
			{
				Monitor.Exit(@lock);
			}
			return list;
		}
		public List<AbstractBuffer> GetAllBuffers()
		{
			List<AbstractBuffer> list = new List<AbstractBuffer>();
			object @lock;
			Monitor.Enter(@lock = this.m_lock);
			try
			{
				foreach (AbstractBuffer current in this.m_buffers)
				{
					list.Add(current);
				}
			}
			finally
			{
				Monitor.Exit(@lock);
			}
			return list;
		}
		public List<AbstractBuffer> GetAllBuffer()
		{
			List<AbstractBuffer> abstractBufferList = new List<AbstractBuffer>();		
			{
				foreach (AbstractBuffer buffer in this.m_buffers)
					abstractBufferList.Add(buffer);
			}
			return abstractBufferList;
		}
		public void Update()
		{
			List<AbstractBuffer> allBuffers = this.GetAllBuffers();
			foreach (AbstractBuffer current in allBuffers)
			{
				try
				{
					if (!current.Check())
					{
						current.Stop();
					}
				}
				catch (Exception message)
				{
					BufferList.log.Error(message);
				}
			}
		}
		public static AbstractBuffer CreateConsortiaBuffer(ConsortiaBufferInfo info)
		{
			return BufferList.CreateBuffer(new BufferInfo
			{
				TemplateID = info.BufferID,
				BeginDate = info.BeginDate,
				ValidDate = info.ValidDate,
				Value = info.Value,
				Type = info.Type,
				ValidCount = 1,
				IsExist = true
			});
		}
		public static AbstractBuffer CreateBuffer(ItemTemplateInfo template, int ValidDate)
		{
			return BufferList.CreateBuffer(new BufferInfo
			{
				TemplateID = template.TemplateID,
				BeginDate = DateTime.Now,
				ValidDate = ValidDate * 24 * 60,
				Value = template.Property2,
				Type = template.Property1,
				ValidCount = 1,
				IsExist = true
			});
		}
		public static AbstractBuffer CreateBufferHour(ItemTemplateInfo template, int ValidHour)
		{
			return BufferList.CreateBuffer(new BufferInfo
			{
				TemplateID = template.TemplateID,
				BeginDate = DateTime.Now,
				ValidDate = ValidHour * 60,
				Value = template.Property2,
				Type = template.Property1,
				ValidCount = 1,
				IsExist = true
			});
		}
		public static AbstractBuffer CreateBufferMinutes(ItemTemplateInfo template, int ValidMinutes)
		{
			return BufferList.CreateBuffer(new BufferInfo
			{
				TemplateID = template.TemplateID,
				BeginDate = DateTime.Now,
				ValidDate = ValidMinutes,
				Value = template.Property2,
				Type = template.Property1,
				ValidCount = 1,
				IsExist = true
			});
		}
		public static AbstractBuffer CreateSaveLifeBuffer(int ValidCount)
		{
			return BufferList.CreateBuffer(new BufferInfo
			{
				TemplateID = 11919,
				BeginDate = DateTime.Now,
				ValidDate = 1440,
				Value = 30,
				Type = 51,
				ValidCount = ValidCount,
				IsExist = true
			});
		}
		public static AbstractBuffer CreatePayBuffer(int type, int Value, int ValidMinutes)
		{
			return BufferList.CreateBuffer(new BufferInfo
			{
				TemplateID = 0,
				BeginDate = DateTime.Now,
				ValidDate = ValidMinutes,
				Value = Value,
				Type = type,
				ValidCount = 1,
				IsExist = true
			});
		}
		public static AbstractBuffer CreatePayBuffer(int type, int Value, int ValidMinutes, int id)
		{
			return BufferList.CreateBuffer(new BufferInfo
			{
				TemplateID = id,
				BeginDate = DateTime.Now,
				ValidDate = ValidMinutes,
				Value = Value,
				Type = type,
				ValidCount = 1,
				IsExist = true
			});
		}
		

		public virtual AbstractBuffer GetOfType(BuffType type)
		{
			lock (this.m_buffers)
			{
				foreach (AbstractBuffer buffer in this.m_buffers)
				{
					if ((BuffType)buffer.Info.Type == type)
						return buffer;
				}
			}
			return (AbstractBuffer)null;
		}
		public static AbstractBuffer CreateBuffer(BufferInfo info)
		{
			AbstractBuffer result = null;
			int type = info.Type;
			if (type <= 51)
			{
				switch (type)
				{
					case 11:
						result = new KickProtectBuffer(info);
						break;
					case 12:
						result = new OfferMultipleBuffer(info);
						break;
					case 13:
						result = new GPMultipleBuffer(info);
						break;
					case 14:
						break;
					case 15:
						result = new PropsBuffer(info);
						break;
					default:
						if (type != 26)
						{
							if (type == 51)
							{
								result = new SaveLifeBuffer(info);
							}
						}
						else
						{
							result = new HonorBuffer(info);
						}
						break;
				}
			}
			else
			{
				switch (type)
				{
					case 11:
						result = (AbstractBuffer)new KickProtectBuffer(info);
				        break;
					case 12:
						result = (AbstractBuffer)new OfferMultipleBuffer(info);
				        break;
					case 13:
						result = (AbstractBuffer)new GPMultipleBuffer(info);
				        break;
					case 15:
						result = (AbstractBuffer)new PropsBuffer(info);
						break;
					case 50:
						result = (AbstractBuffer)new AgiBuffer(info);
				        break;
					case 51:
						result = (AbstractBuffer)new SaveLifeBuffer(info);
						break;
					case 52:
						result = (AbstractBuffer)new ReHealthBuffer(info);
				        break;
					case 70:
						result = (AbstractBuffer)new CaddyGoodsBuffer(info);
				        break;
					case 71:
						result = (AbstractBuffer)new TrainGoodsBuffer(info);
				        break;
					case 73:
						result = (AbstractBuffer)new CardGetBuffer(info);				
				        break;
					case 74:
						result = new DefendBuffer(info);
						break;
					case 75:
						result = new AttackBuffer(info);
						break;
					case 76:
						result = new GuardBuffer(info);
						break;
					case 77:
						result = new AgiBuffer(info);
						break;
					case 78:
						result = new DameBuffer(info);
						break;
					case 79:
						result = new HpBuffer(info);
						break;
					case 80:
						result = new LuckBuffer(info);
						break;
					default:
						switch (type)
						{
							case 101:
								result = new ConsortionAddBloodGunCountBuffer(info);
								break;
							case 102:
								result = new ConsortionAddDamageBuffer(info);
								break;
							case 103:
								result = new ConsortionAddCriticalBuffer(info);
								break;
							case 104:
								result = new ConsortionAddMaxBloodBuffer(info);
								break;
							case 105:
								result = new ConsortionAddPropertyBuffer(info);
								break;
							case 106:
								result = new ConsortionReduceEnergyUseBuffer(info);
								break;
							case 107:
								result = new ConsortionAddEnergyBuffer(info);
								break;
							case 108:
								result = new ConsortionAddEffectTurnBuffer(info);
								break;
							case 109:
								result = new ConsortionAddOfferRateBuffer(info);
								break;
							case 110:
								result = new ConsortionAddPercentGoldOrGPBuffer(info);
								break;
							case 111:
								result = new ConsortionAddSpellCountBuffer(info);
								break;
							case 112:
								result = new ConsortionReduceDanderBuffer(info);
								break;
							case 113:
								break;
							case 114:
								result = new ActivityDungeonBubbleBuffer(info);
								break;
							case 115:
								result = new ActivityDungeonNetBuffer(info);
								break;
							default:
								switch (type)
								{
									case 400:
										result = new WorldBossHPBuffer(info);
										break;
									case 401:
										result = new WorldBossAttrackBuffer(info);
										break;
									case 402:
										result = new WorldBossHP_MoneyBuffBuffer(info);
										break;
									case 403:
										result = new WorldBossAttrack_MoneyBuffBuffer(info);
										break;
									case 404:
										result = new WorldBossMetalSlugBuffer(info);
										break;
									case 405:
										result = new WorldBossAncientBlessingsBuffer(info);
										break;
									case 406:
										result = new WorldBossAddDamageBuffer(info);
										break;
								}
								break;
						}
						break;
				}
			}
			return result;
		}
	}
}
