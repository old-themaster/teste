using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Game.Logic;
using Game.Logic.Phy.Maps;
using log4net;
using SqlDataProvider.Data;

namespace Game.Server.Games
{
	public class GameMgr
	{
		private static readonly int CLEAR_GAME_INTERVAL = 10000;

		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private static int m_boxBroadcastLevel;

		private static long m_clearGamesTimer;

		private static int m_gameId;

		private static List<BaseGame> m_games;

		private static bool m_running;

		private static int m_serverId;

		private static Thread m_thread;

		public static readonly long THREAD_INTERVAL = 40L;

		public static int BoxBroadcastLevel => m_boxBroadcastLevel;

		private static void ClearStoppedGames(object state)
		{
			foreach (BaseGame game in state as ArrayList)
			{
				try
				{
					game.Dispose();
				}
				catch (Exception exception)
				{
					log.Error("game dispose error:", exception);
				}
			}
		}

		private static void GameThread()
		{
			Thread.CurrentThread.Priority = ThreadPriority.Highest;
			long num = 0L;
			m_clearGamesTimer = TickHelper.GetTickCount();
			while (m_running)
			{
				long tickCount = TickHelper.GetTickCount();
				int num2 = 0;
				try
				{
					num2 = UpdateGames(tickCount);
					if (m_clearGamesTimer <= tickCount)
					{
						m_clearGamesTimer += CLEAR_GAME_INTERVAL;
						ArrayList state = new ArrayList();
						foreach (BaseGame game in m_games)
						{
							if (game.GameState == eGameState.Stopped)
							{
								state.Add(game);
							}
						}
						foreach (BaseGame game2 in state)
						{
							m_games.Remove(game2);
						}
						ThreadPool.QueueUserWorkItem(ClearStoppedGames, state);
					}
				}
				catch (Exception exception)
				{
					log.Error("Game Mgr Thread Error:", exception);
				}
				long num3 = TickHelper.GetTickCount();
				num += THREAD_INTERVAL - (num3 - tickCount);
				if (num3 - tickCount > THREAD_INTERVAL * 2)
				{
					log.WarnFormat("Game Mgr spent too much times: {0} ms, count:{1}", num3 - tickCount, num2);
				}
				if (num > 0)
				{
					Thread.Sleep((int)num);
					num = 0L;
				}
				else if (num < -1000)
				{
					num += 1000;
				}

				m_synDate = DateTime.Now;
			}
		}

		public static List<BaseGame> GetAllGame()
		{
			List<BaseGame> list = new List<BaseGame>();
			lock (m_games)
			{
				list.AddRange(m_games);
				return list;
			}
		}

		private static DateTime m_synDate;

		public static int SynDate
		{
			get { return DateTime.Compare(m_synDate.AddSeconds(THREAD_INTERVAL), DateTime.Now); }
		}

		public static bool Setup(int serverId, int boxBroadcastLevel)
		{
			m_thread = new Thread(GameThread);
			m_games = new List<BaseGame>();
			m_serverId = serverId;
			m_boxBroadcastLevel = boxBroadcastLevel;
			m_gameId = 0;
			m_synDate = DateTime.Now;
			return true;
		}

		public static bool Start()
		{
			if (!m_running)
			{
				m_running = true;
				m_thread.Start();
			}
			return true;
		}

		public static BaseGame StartPVEGame(int roomId, List<IGamePlayer> players, int copyId, eRoomType roomType, eGameType gameType, int timeType, eHardLevel hardLevel, int levelLimits, int currentFloor)
		{
			try
			{
				PveInfo pveInfoByType = ((copyId != 0 && copyId != 100000) ? PveInfoMgr.GetPveInfoById(copyId) : PveInfoMgr.GetPveInfoByType(roomType, levelLimits));
				if (pveInfoByType != null)
				{
					PVEGame item = new PVEGame(m_gameId++, roomId, pveInfoByType, players, null, roomType, gameType, timeType, hardLevel, currentFloor);
					lock (m_games)
					{
						m_games.Add(item);
					}
					item.Prepare();
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("Instância do tipo {0} com {1} jogadores, Nome {4}, ID {2} iniciada com sucesso.", (object)item.RoomType, (object)item.PlayerCount, (object)item.Id, (object)copyId, (object)pveInfoByType.Name, (object)GameMgr.m_games.Count);
					Console.ResetColor();
					return (BaseGame)item;
					return item;
				}
				return null;
			}
			catch (Exception exception)
			{
				log.Error("Create game error:", exception);
				return null;
			}
		}

		public static BaseGame StartPVPGame(int roomId, List<IGamePlayer> red, List<IGamePlayer> blue, int mapIndex, eRoomType roomType, eGameType gameType, int timeType)
		{
			try
			{
				Map map = MapMgr.CloneMap(MapMgr.GetMapIndex(mapIndex, (byte)roomType, m_serverId));
				if (map != null)
				{
					PVPGame item = new PVPGame(m_gameId++, roomId, red, blue, map, roomType, gameType, timeType);
					lock (m_games)
					{
						m_games.Add(item);
					}
					item.Prepare();
					return item;
				}
				return null;
			}
			catch (Exception exception)
			{
				log.Error("Create game error:", exception);
				return null;
			}
		}

		public static void Stop()
		{
			if (m_running)
			{
				m_running = false;
				m_thread.Join();
			}
		}

		private static int UpdateGames(long tick)
		{
			IList allGame = GetAllGame();
			if (allGame != null)
			{
				foreach (BaseGame game in allGame)
				{
					try
					{
						game.Update(tick);
					}
					catch (Exception exception)
					{
						log.Error("Game  updated error:", exception);
					}
				}
				return allGame.Count;
			}
			return 0;
		}

		public static void ClearAllGames()
        {
			ArrayList temp = new ArrayList();
			lock (m_games)
            {
				foreach (BaseGame g in m_games)
                {
					temp.Add(g);
                }

				foreach (BaseGame g in temp)
                {
					m_games.Remove(g);
					try
                    {
						g.Dispose();
                    }
					catch (Exception ex)
                    {
						log.Error("game dispose error:", ex);
					}
                }
            }
        }
	}
}
