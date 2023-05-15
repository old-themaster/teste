using Bussiness;
using Game.Logic.Phy.Maps;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Game.Logic
{
	public class MapMgr
	{
		private static Dictionary<int, Map> _mapInfos;

		private static Dictionary<int, MapPoint> _maps;

		private static Dictionary<int, List<int>> _serverMap;

		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private static ReaderWriterLock m_lock;

		private static ThreadSafeRandom random;

		public static int GetWeekDay
		{
			get
			{
				int num = Convert.ToInt32(DateTime.Now.DayOfWeek);
				if (num != 0)
				{
					return num;
				}
				return 7;
			}
		}

		public static Map CloneMap(int index)
		{
			if (_mapInfos.ContainsKey(index))
			{
				return _mapInfos[index].Clone();
			}
			return null;
		}

		public static MapInfo FindMapInfo(int index)
		{
			if (_mapInfos.ContainsKey(index))
			{
				return _mapInfos[index].Info;
			}
			return null;
		}

		public static int GetMapIndex(int index, byte type, int serverId)
		{
			if (index != 0 && !_maps.Keys.Contains(index))
			{
				index = 0;
			}
			if (index != 0)
			{
				return index;
			}
			List<int> list = new List<int>();
			foreach (int item in _serverMap[serverId])
			{
				MapInfo mapInfo = FindMapInfo(item);
				if ((type & mapInfo.Type) != 0)
				{
					list.Add(item);
				}
			}
			if (list.Count == 0)
			{
				int count = _serverMap[serverId].Count;
				return _serverMap[serverId][random.Next(count)];
			}
			int count2 = list.Count;
			return list[random.Next(count2)];
		}

		public static MapPoint GetMapRandomPos(int index)
		{
			MapPoint mapPoint = new MapPoint();
			if (index != 0 && !_maps.Keys.Contains(index))
			{
				index = 0;
			}
			MapPoint mapPoint2;
			if (index == 0)
			{
				int[] array = _maps.Keys.ToArray();
				mapPoint2 = _maps[array[random.Next(array.Length)]];
			}
			else
			{
				mapPoint2 = _maps[index];
			}
			if (random.Next(2) == 1)
			{
				mapPoint.PosX.AddRange(mapPoint2.PosX);
				mapPoint.PosX1.AddRange(mapPoint2.PosX1);
				return mapPoint;
			}
			mapPoint.PosX.AddRange(mapPoint2.PosX1);
			mapPoint.PosX1.AddRange(mapPoint2.PosX);
			return mapPoint;
		}

		public static MapPoint GetPVEMapRandomPos(int index)
		{
			MapPoint mapPoint = new MapPoint();
			if (index != 0 && !_maps.Keys.Contains(index))
			{
				index = 0;
			}
			MapPoint mapPoint2;
			if (index == 0)
			{
				int[] array = _maps.Keys.ToArray();
				mapPoint2 = _maps[array[random.Next(array.Length)]];
			}
			else
			{
				mapPoint2 = _maps[index];
			}
			mapPoint.PosX.AddRange(mapPoint2.PosX);
			mapPoint.PosX1.AddRange(mapPoint2.PosX1);
			return mapPoint;
		}

		public static bool Init()
		{
			try
			{
				random = new ThreadSafeRandom();
				m_lock = new ReaderWriterLock();
				_maps = new Dictionary<int, MapPoint>();
				_mapInfos = new Dictionary<int, Map>();
				if (!LoadMap(_maps, _mapInfos))
				{
					return false;
				}
				_serverMap = new Dictionary<int, List<int>>();
				if (!InitServerMap(_serverMap))
				{
					return false;
				}
			}
			catch (Exception exception)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("MapMgr", exception);
				}
				return false;
			}
			return true;
		}

		public static bool InitServerMap(Dictionary<int, List<int>> servermap)
		{
			ServerMapInfo[] allServerMap = new MapBussiness().GetAllServerMap();
			try
			{
				int result = 0;
				ServerMapInfo[] array = allServerMap;
				foreach (ServerMapInfo serverMapInfo in array)
				{
					if (!servermap.Keys.Contains(serverMapInfo.ServerID))
					{
						string[] array2 = serverMapInfo.OpenMap.Split(',');
						List<int> list = new List<int>();
						string[] array3 = array2;
						foreach (string text in array3)
						{
							if (!string.IsNullOrEmpty(text) && int.TryParse(text, out result))
							{
								list.Add(result);
							}
						}
						servermap.Add(serverMapInfo.ServerID, list);
					}
				}
			}
			catch (Exception ex)
			{
				log.Error(ex.ToString());
			}
			return true;
		}

		public static bool LoadMap(Dictionary<int, MapPoint> maps, Dictionary<int, Map> mapInfos)
		{
			try
			{
				MapInfo[] allMap = new MapBussiness().GetAllMap();
				foreach (MapInfo mapInfo in allMap)
				{
					if (!string.IsNullOrEmpty(mapInfo.PosX))
					{
						if (!maps.Keys.Contains(mapInfo.ID))
						{
							string[] array = mapInfo.PosX.Split('|');
							string[] array2 = mapInfo.PosX1.Split('|');
							MapPoint mapPoint = new MapPoint();
							string[] array3 = array;
							foreach (string text in array3)
							{
								if (!string.IsNullOrEmpty(text.Trim()))
								{
									string[] array4 = text.Split(',');
									mapPoint.PosX.Add(new Point(int.Parse(array4[0]), int.Parse(array4[1])));
								}
							}
							array3 = array2;
							foreach (string text2 in array3)
							{
								if (!string.IsNullOrEmpty(text2.Trim()))
								{
									string[] array5 = text2.Split(',');
									mapPoint.PosX1.Add(new Point(int.Parse(array5[0]), int.Parse(array5[1])));
								}
							}
							maps.Add(mapInfo.ID, mapPoint);
						}
						if (!mapInfos.ContainsKey(mapInfo.ID))
						{
							Tile tile = null;
							string text3 = $"map\\{mapInfo.ID}\\fore.map";
							if (File.Exists(text3))
							{
								tile = new Tile(text3, digable: true);
							}
							Tile tile2 = null;
							text3 = $"map\\{mapInfo.ID}\\dead.map";
							if (File.Exists(text3))
							{
								tile2 = new Tile(text3, digable: false);
							}
							if (tile != null || tile2 != null)
							{
								mapInfos.Add(mapInfo.ID, new Map(mapInfo, tile, tile2));
							}
							else if (log.IsErrorEnabled)
							{
								log.Error("Map's file" + mapInfo.ID + " is not exist!");
							}
						}
					}
				}
				if (maps.Count == 0 || mapInfos.Count == 0)
				{
					if (log.IsErrorEnabled)
					{
						log.Error("maps:" + maps.Count + ",mapInfos:" + mapInfos.Count);
					}
					return false;
				}
			}
			catch (Exception exception)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("MapMgr", exception);
				}
				return false;
			}
			return true;
		}

		public static bool ReLoadMap()
		{
			try
			{
				Dictionary<int, MapPoint> maps = new Dictionary<int, MapPoint>();
				Dictionary<int, Map> mapInfos = new Dictionary<int, Map>();
				if (LoadMap(maps, mapInfos))
				{
					m_lock.AcquireWriterLock(-1);
					try
					{
						_maps = maps;
						_mapInfos = mapInfos;
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
					log.Error("ReLoadMap", exception);
				}
			}
			return false;
		}

		public static bool ReLoadMapServer()
		{
			try
			{
				Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
				if (InitServerMap(dictionary))
				{
					m_lock.AcquireWriterLock(-1);
					try
					{
						_serverMap = dictionary;
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
					log.Error("ReLoadMapWeek", exception);
				}
			}
			return false;
		}
	}
}
