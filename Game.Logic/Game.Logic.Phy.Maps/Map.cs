using Game.Logic.Phy.Object;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Game.Logic.Phy.Maps
{
	public class Map
	{
		private MapInfo mapInfo_0;

		private float float_0;

		private HashSet<Physics> hashSet_0;

		protected Tile _layer1;

		protected Tile _layer2;

		protected Rectangle _bound;

		private Random random_0;

		public float wind
		{
			get
			{
				return float_0;
			}
			set
			{
				float_0 = value;
			}
		}

		public float gravity => mapInfo_0.Weight;

		public float airResistance => mapInfo_0.DragIndex;

		public Tile Ground => _layer1;

		public Tile DeadTile => _layer2;

		public MapInfo Info => mapInfo_0;

		public Rectangle Bound => _bound;

		public Map(MapInfo info, Tile layer1, Tile layer2)
		{
			mapInfo_0 = info;
			hashSet_0 = new HashSet<Physics>();
			random_0 = new Random();
			_layer1 = layer1;
			_layer2 = layer2;
			if (_layer1 != null)
			{
				_bound = new Rectangle(0, 0, _layer1.Width, _layer1.Height);
			}
			else
			{
				_bound = new Rectangle(0, 0, _layer2.Width, _layer2.Height);
			}
		}

		public void Dig(int cx, int cy, Tile surface, Tile border)
		{
			if (_layer1 != null)
			{
				_layer1.Dig(cx, cy, surface, border);
			}
			if (_layer2 != null)
			{
				_layer2.Dig(cx, cy, surface, border);
			}
		}

		public bool IsEmpty(int x, int y)
		{
			if (_layer1 != null && !_layer1.IsEmpty(x, y))
			{
				return false;
			}
			if (_layer2 != null)
			{
				return _layer2.IsEmpty(x, y);
			}
			return true;
		}

		public bool IsRectangleEmpty(Rectangle rect)
		{
			if (_layer1 != null && !_layer1.IsRectangleEmptyQuick(rect))
			{
				return false;
			}
			if (_layer2 != null)
			{
				return _layer2.IsRectangleEmptyQuick(rect);
			}
			return true;
		}

		public Point FindYLineNotEmptyPointDown(int x, int y, int h)
		{
			x = ((x >= 0) ? ((x >= _bound.Width) ? (_bound.Width - 1) : x) : 0);
			y = ((y >= 0) ? y : 0);
			h = ((y + h >= _bound.Height) ? (_bound.Height - y - 1) : h);
			for (int i = 0; i < h; i++)
			{
				if (!IsEmpty(x - 1, y) || !IsEmpty(x + 1, y))
				{
					return new Point(x, y);
				}
				y++;
			}
			return Point.Empty;
		}

		public Point FindYLineNotEmptyPointDown(int x, int y)
		{
			return FindYLineNotEmptyPointDown(x, y, _bound.Height);
		}

		public Point FindYLineNotEmptyPointUp(int x, int y, int h)
		{
			x = ((x >= 0) ? ((x >= _bound.Width) ? _bound.Width : x) : 0);
			y = ((y >= 0) ? y : 0);
			h = ((y + h >= _bound.Height) ? (_bound.Height - y) : h);
			for (int i = 0; i < h; i++)
			{
				if (!IsEmpty(x - 1, y) || !IsEmpty(x + 1, y))
				{
					return new Point(x, y);
				}
				y--;
			}
			return Point.Empty;
		}

		public Point FindNextWalkPoint(int x, int y, int direction, int stepX, int stepY)
		{
			if (direction != 1 && direction != -1)
			{
				return Point.Empty;
			}
			int num = x + direction * stepX;
			if (num < 0 || num > _bound.Width)
			{
				return Point.Empty;
			}
			Point point = FindYLineNotEmptyPointDown(num, y - stepY - 1, _bound.Width);
			if (point != Point.Empty && Math.Abs(point.Y - y) > stepY)
			{
				point = Point.Empty;
			}
			return point;
		}

		public List<Living> FindLiving(int fx, int tx, List<Living> exceptLivings)
		{
			List<Living> list = new List<Living>();
			lock (hashSet_0)
			{
				foreach (Physics item in hashSet_0)
				{
					bool flag = true;
					if (item is Living && item.IsLiving && item.X > fx && item.X < tx)
					{
						if (exceptLivings != null && exceptLivings.Count != 0)
						{
							foreach (Living exceptLiving in exceptLivings)
							{
								if (((Living)item).Id == exceptLiving.Id)
								{
									flag = false;
									break;
								}
							}
							if (flag)
							{
								list.Add(item as Living);
							}
						}
						else
						{
							list.Add(item as Living);
						}
					}
				}
				return list;
			}
		}

		public Point FindNextWalkPointDown(int x, int y, int direction, int stepX, int stepY)
		{
			if (direction != 1 && direction != -1)
			{
				return Point.Empty;
			}
			int num = x + direction * stepX;
			if (num < 0 || num > _bound.Width)
			{
				return Point.Empty;
			}
			Point point = FindYLineNotEmptyPointDown(num, y - stepY - 1);
			if (point != Point.Empty && Math.Abs(point.Y - y) > stepY)
			{
				point = Point.Empty;
			}
			return point;
		}

		public List<Living> FindRandomPlayer(int fx, int tx, List<Player> exceptPlayers)
		{
			List<Living> list = new List<Living>();
			lock (hashSet_0)
			{
				foreach (Physics item in hashSet_0)
				{
					if (item is Player && item.IsLiving && item.X > fx && item.X < tx)
					{
						foreach (Player exceptPlayer in exceptPlayers)
						{
							if (((Player)item).PlayerDetail == exceptPlayer.PlayerDetail)
							{
								list.Add(item as Living);
							}
						}
					}
				}
			}
			List<Living> list2 = new List<Living>();
			if (list.Count > 0)
			{
				list2.Add(list[random_0.Next(list.Count)]);
			}
			return list2;
		}

		public List<Living> FindRandomLiving(int fx, int tx)
		{
			List<Living> list = new List<Living>();
			lock (hashSet_0)
			{
				foreach (Physics item in hashSet_0)
				{
					if ((item is SimpleNpc || item is SimpleBoss) && item.IsLiving && item.X > fx && item.X < tx)
					{
						list.Add(item as Living);
					}
				}
			}
			List<Living> list2 = new List<Living>();
			if (list.Count > 0)
			{
				list2.Add(list[random_0.Next(list.Count)]);
			}
			return list2;
		}

		public bool canMove(int x, int y)
		{
			if (IsEmpty(x, y))
			{
				return !IsOutMap(x, y);
			}
			return false;
		}

		public bool IsOutMap(int x, int y)
		{
			if (x >= _bound.X && x <= _bound.Width)
			{
				return y > _bound.Height;
			}
			return true;
		}

		public void AddPhysical(Physics phy)
		{
			phy.SetMap(this);
			lock (hashSet_0)
			{
				hashSet_0.Add(phy);
			}
		}

		public void RemovePhysical(Physics phy)
		{
			phy.SetMap(null);
			lock (hashSet_0)
			{
				hashSet_0.Remove(phy);
			}
		}

		public List<Physics> GetAllPhysicalSafe()
		{
			List<Physics> list = new List<Physics>();
			lock (hashSet_0)
			{
				foreach (Physics item in hashSet_0)
				{
					list.Add(item);
				}
				return list;
			}
		}

		public List<PhysicalObj> GetAllPhysicalObjSafe()
		{
			List<PhysicalObj> list = new List<PhysicalObj>();
			lock (hashSet_0)
			{
				foreach (Physics item in hashSet_0)
				{
					if (item is PhysicalObj)
					{
						list.Add(item as PhysicalObj);
					}
				}
				return list;
			}
		}

		public Physics[] FindPhysicalObjects(Rectangle rect, Physics except)
		{
			List<Physics> list = new List<Physics>();
			lock (hashSet_0)
			{
				foreach (Physics item in hashSet_0)
				{
					if (item.IsLiving && item != except)
					{
						Rectangle bound = item.Bound;
						Rectangle bound2 = item.Bound1;
						bound.Offset(item.X, item.Y);
						bound2.Offset(item.X, item.Y);
						if (bound.IntersectsWith(rect) || bound2.IntersectsWith(rect))
						{
							list.Add(item);
						}
					}
				}
			}
			return list.ToArray();
		}

		public bool FindPlayers(Point p, int radius)
		{
			int num = 0;
			lock (hashSet_0)
			{
				foreach (Physics item in hashSet_0)
				{
					if (item is Player && item.IsLiving && (item as Player).BoundDistance(p) < (double)radius)
					{
						num++;
					}
					if (num >= 2)
					{
						return true;
					}
				}
			}
			return false;
		}

		public List<Player> FindPlayers(int x, int y, int radius)
		{
			List<Player> list = new List<Player>();
			lock (hashSet_0)
			{
				foreach (Physics item in hashSet_0)
				{
					if (item is Player && item.IsLiving && item.Distance(x, y) < (double)radius)
					{
						list.Add(item as Player);
					}
				}
				return list;
			}
		}

		public List<Living> FindLivings(int x, int y, int radius)
		{
			List<Living> list = new List<Living>();
			lock (hashSet_0)
			{
				foreach (Physics item in hashSet_0)
				{
					if (item is Living && item.IsLiving && item.Distance(x, y) < (double)radius)
					{
						list.Add(item as Living);
					}
				}
				return list;
			}
		}

		public List<Living> FindPlayers(int fx, int tx, List<Player> exceptPlayers)
		{
			List<Living> list = new List<Living>();
			lock (hashSet_0)
			{
				foreach (Physics item in hashSet_0)
				{
					if ((item is Player || (item is Living && (item as Living).Config.IsHelper)) && item.IsLiving && item.X > fx && item.X < tx && (!(item is Player) || (item as Player).IsActive))
					{
						if (exceptPlayers != null)
						{
							foreach (Player exceptPlayer in exceptPlayers)
							{
								if (item is Player && ((TurnedLiving)item).DefaultDelay != exceptPlayer.DefaultDelay)
								{
									list.Add(item as Living);
								}
							}
						}
						else
						{
							list.Add(item as Living);
						}
					}
				}
				return list;
			}
		}

		public List<Living> FindHitByHitPiont(Point p, int radius)
		{
			List<Living> list = new List<Living>();
			lock (hashSet_0)
			{
				foreach (Physics item in hashSet_0)
				{
					if (item is Living && item.IsLiving && (item as Living).BoundDistance(p) < (double)radius)
					{
						list.Add(item as Living);
					}
				}
				return list;
			}
		}

		public Living FindNearestEnemy(int x, int y, double maxdistance, Living except)
		{
			Living result = null;
			lock (hashSet_0)
			{
				foreach (Physics item in hashSet_0)
				{
					if (item is Living && item != except && item.IsLiving && ((Living)item).Team != except.Team)
					{
						double num = item.Distance(x, y);
						if (num < maxdistance)
						{
							result = (item as Living);
							maxdistance = num;
						}
					}
				}
				return result;
			}
		}

		public List<Living> FindAllNearestEnemy(int x, int y, double maxdistance, Living except)
		{
			List<Living> list = new List<Living>();
			lock (hashSet_0)
			{
				foreach (Physics item in hashSet_0)
				{
					if (item is Living && item != except && item.IsLiving && ((Living)item).Team != except.Team)
					{
						double num = item.Distance(x, y);
						if (num < maxdistance)
						{
							list.Add(item as Living);
							maxdistance = num;
						}
					}
				}
				return list;
			}
		}

		public List<Living> FindAllNearestSameTeam(int x, int y, double maxdistance, Living except)
		{
			List<Living> list = new List<Living>();
			lock (hashSet_0)
			{
				foreach (Physics item in hashSet_0)
				{
					if (item is Living && item != except && item.IsLiving && ((Living)item).Team == except.Team)
					{
						double num = item.Distance(x, y);
						if (num < maxdistance)
						{
							list.Add(item as Living);
							maxdistance = num;
						}
					}
				}
				return list;
			}
		}

		public void Dispose()
		{
			foreach (Physics item in hashSet_0)
			{
				item.Dispose();
			}
		}

		public Map Clone()
		{
			return new Map(mapInfo_0, (_layer1 != null) ? _layer1.Clone() : null, (_layer2 != null) ? _layer2.Clone() : null);
		}
	}
}
