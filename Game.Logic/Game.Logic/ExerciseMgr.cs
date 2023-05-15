using Bussiness;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Game.Logic
{
	public class ExerciseMgr
	{
		private static Dictionary<int, ExerciseInfo> _exercises;

		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private static ReaderWriterLock m_lock;

		private static ThreadSafeRandom rand;

		private static List<EliteGameRoundInfo> list_0 = new List<EliteGameRoundInfo>();

		private static Dictionary<int, PlayerEliteGameInfo> dictionary_1 = new Dictionary<int, PlayerEliteGameInfo>();

		public static int EliteStatus
		{
			get;
			set;
		}

		public static Dictionary<int, PlayerEliteGameInfo> EliteGameChampionPlayersList => dictionary_1;

		public static ExerciseInfo FindExercise(int Grage)
		{
			if (Grage == 0)
			{
				Grage = 1;
			}
			m_lock.AcquireReaderLock(10000);
			try
			{
				if (_exercises.ContainsKey(Grage))
				{
					return _exercises[Grage];
				}
			}
			catch
			{
			}
			finally
			{
				m_lock.ReleaseReaderLock();
			}
			return null;
		}

		public static int GetExercise(int GP, string type)
		{
			int result = 0;
			for (int i = 1; i <= GetMaxLevel(); i++)
			{
				if (FindExercise(i).GP >= GP)
				{
					return result;
				}
				if (type == null)
				{
					continue;
				}
				if (type != "A")
				{
					if (type != "AG")
					{
						if (type != "D")
						{
							if (type != "H")
							{
								if (type == "L")
								{
									result = FindExercise(i).ExerciseL;
								}
							}
							else
							{
								result = FindExercise(i).ExerciseH;
							}
						}
						else
						{
							result = FindExercise(i).ExerciseD;
						}
					}
					else
					{
						result = FindExercise(i).ExerciseAG;
					}
				}
				else
				{
					result = FindExercise(i).ExerciseA;
				}
			}
			return result;
		}

		public static int GetMaxLevel()
		{
			if (_exercises == null)
			{
				Init();
			}
			return _exercises.Values.Count;
		}

		public static int getLv(int exp)
		{
			int result = 0;
			ExerciseInfo exerciseInfo = null;
			int num;
			for (num = 1; num <= GetMaxLevel(); num++)
			{
				exerciseInfo = _exercises[num];
				if (exp < exerciseInfo.GP)
				{
					break;
				}
				result = num;
				num++;
			}
			return result;
		}

		public static int getExp(int type, TexpInfo self)
		{
			switch (type)
			{
				case 0:
					return self.hpTexpExp;
				case 1:
					return self.attTexpExp;
				case 2:
					return self.defTexpExp;
				case 3:
					return self.spdTexpExp;
				case 4:
					return self.lukTexpExp;
				default:
					return 0;
			}
		}

		public static ExerciseInfo GetExerciseinfoByLevelExp(int Exp)
		{
			ExerciseInfo info = null;
			m_lock.AcquireReaderLock(10000);
			foreach (ExerciseInfo data in _exercises.Values)
			{
				if (Exp >= data.GP)
				{
					info = data;
				}
			}
			m_lock.ReleaseReaderLock();
			return info;
		}
		public static bool isUp(int type, int oldExp, TexpInfo self)
		{
			if (getLv(getExp(type, self)) > getLv(oldExp))
			{
				return true;
			}
			return false;
		}

		public static bool Init()
		{
			try
			{
				m_lock = new ReaderWriterLock();
				_exercises = new Dictionary<int, ExerciseInfo>();
				rand = new ThreadSafeRandom();
				EliteStatus = 0;
				return LoadExercise(_exercises);
			}
			catch (Exception exception)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("ExercisesMgr", exception);
				}
				return false;
			}
		}

		private static bool LoadExercise(Dictionary<int, ExerciseInfo> Exercise)
		{
			using (ProduceBussiness produceBussiness = new ProduceBussiness())
			{
				ExerciseInfo[] allExercise = produceBussiness.GetAllExercise();
				foreach (ExerciseInfo exerciseInfo in allExercise)
				{
					if (!Exercise.ContainsKey(exerciseInfo.Grage))
					{
						Exercise.Add(exerciseInfo.Grage, exerciseInfo);
					}
				}
			}
			return true;
		}

		public static bool ReLoad()
		{
			try
			{
				Dictionary<int, ExerciseInfo> dictionary = new Dictionary<int, ExerciseInfo>();
				if (LoadExercise(dictionary))
				{
					m_lock.AcquireWriterLock(-1);
					try
					{
						_exercises = dictionary;
						return true;
					}
					catch
					{
					}
					finally
					{
						m_lock.ReleaseWriterLock();
					}
				}
			}
			catch (Exception exception)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("ExerciseMgr", exception);
				}
			}
			return false;
		}

		public static void ResetEliteGame()
		{
			list_0 = new List<EliteGameRoundInfo>();
			dictionary_1 = new Dictionary<int, PlayerEliteGameInfo>();
		}

		public static void SynEliteGameChampionPlayerList(Dictionary<int, PlayerEliteGameInfo> tempPlayerList)
		{
			m_lock.AcquireReaderLock(-1);
			try
			{
				dictionary_1 = tempPlayerList;
			}
			catch
			{
			}
			finally
			{
				m_lock.ReleaseReaderLock();
			}
		}

		public static void UpdateEliteGameChapionPlayerList(PlayerEliteGameInfo p)
		{
			m_lock.AcquireReaderLock(-1);
			try
			{
				if (dictionary_1.ContainsKey(p.UserID))
				{
					dictionary_1[p.UserID] = p;
				}
				else
				{
					dictionary_1.Add(p.UserID, p);
				}
			}
			catch
			{
			}
			finally
			{
				m_lock.ReleaseReaderLock();
			}
		}

		public static EliteGameRoundInfo FindEliteRoundByUser(int userId)
		{
			m_lock.AcquireReaderLock(-1);
			try
			{
				foreach (EliteGameRoundInfo item in (from a in list_0
													 orderby a.RoundType descending
													 select a).ToList())
				{
					if (item.PlayerOne.UserID == userId || item.PlayerTwo.UserID == userId)
					{
						return item;
					}
				}
			}
			catch (Exception ex)
			{
				log.Error(ex.ToString());
			}
			finally
			{
				m_lock.ReleaseReaderLock();
			}
			return null;
		}

		public static void AddEliteRound(EliteGameRoundInfo elite)
		{
			m_lock.AcquireReaderLock(-1);
			try
			{
				list_0.Add(elite);
			}
			catch
			{
			}
			finally
			{
				m_lock.ReleaseReaderLock();
			}
		}

		public static void RemoveEliteRound(EliteGameRoundInfo elite)
		{
			m_lock.AcquireReaderLock(-1);
			try
			{
				if (list_0.Contains(elite))
				{
					list_0.Remove(elite);
				}
			}
			catch
			{
			}
			finally
			{
				m_lock.ReleaseReaderLock();
			}
		}

		public static bool IsBlockWeapon(int templateid)
		{
			bool result = false;
			if (GameProperties.EliteGameBlockWeapon.Split('|').Contains(templateid.ToString()))
			{
				result = true;
			}
			return result;
		}
	}
}
