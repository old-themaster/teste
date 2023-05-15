using System;
using System.Collections.Generic;
namespace SqlDataProvider.Data
{
	public class UserAvatarCollectionInfo : DataObject
	{
		private int _ID;
		private int _userID;
		private int _AvatarID;
		private ClothPropertyTemplateInfo _clothProperty;
		private int _Sex;
		private bool _isActive;
		private string _data;
		private List<UserAvatarCollectionDataInfo> _items;
		private DateTime _timeStart;
		private DateTime _timeend;
		private bool _IsExit;
		public int ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				this._ID = value;
				this._isDirty = true;
			}
		}
		public int UserID
		{
			get
			{
				return this._userID;
			}
			set
			{
				this._userID = value;
				this._isDirty = true;
			}
		}
		public int AvatarID
		{
			get
			{
				return this._AvatarID;
			}
			set
			{
				this._AvatarID = value;
				this._isDirty = true;
			}
		}
		public ClothPropertyTemplateInfo ClothProperty
		{
			get
			{
				return this._clothProperty;
			}
			set
			{
				this._clothProperty = value;
			}
		}
		public int Sex
		{
			get
			{
				return this._Sex;
			}
			set
			{
				this._Sex = value;
				this._isDirty = true;
			}
		}
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				this._isActive = value;
				this._isDirty = true;
			}
		}
		public string Data
		{
			get
			{
				return this._data;
			}
			set
			{
				this._data = value;
				this._isDirty = true;
			}
		}
		public List<UserAvatarCollectionDataInfo> Items
		{
			get
			{
				if (this._items == null)
				{
					this._items = this.GetData();
				}
				return this._items;
			}
			set
			{
				this._items = value;
			}
		}
		public DateTime TimeStart
		{
			get
			{
				return this._timeStart;
			}
			set
			{
				this._timeStart = value;
				this._isDirty = true;
			}
		}
		public DateTime TimeEnd
		{
			get
			{
				return this._timeend;
			}
			set
			{
				this._timeend = value;
				this._isDirty = true;
			}
		}
		public bool IsExit
		{
			get
			{
				return this._IsExit;
			}
			set
			{
				this._IsExit = value;
				this._isDirty = true;
			}
		}
		public UserAvatarCollectionInfo()
		{
		}
		public UserAvatarCollectionInfo(int UserId, int AvatarID, int Sex, bool IsActive, DateTime timeend)
		{
			this._userID = UserId;
			this._AvatarID = AvatarID;
			this._Sex = Sex;
			this._isActive = IsActive;
			this._timeend = timeend;
			this._timeStart = DateTime.Now;
			this._data = "";
			this._IsExit = true;
		}
		public bool IsAvalible()
		{
			return this._isActive && this._timeend > DateTime.Now;
		}
		public bool ActiveAvatar(int days)
		{
			if (days <= 0)
			{
				return false;
			}
			if (this._isActive && this._timeend >= DateTime.Now)
			{
				this.TimeEnd = this.TimeEnd.AddDays((double)days);
				return true;
			}
			this.IsActive = true;
			this.TimeEnd = DateTime.Now.AddDays((double)days);
			return true;
		}
		public List<UserAvatarCollectionDataInfo> GetData()
		{
			List<UserAvatarCollectionDataInfo> data = new List<UserAvatarCollectionDataInfo>();
			if (this._data == null)
			{
				this._data = "";
			}
			if (this._data.Length > 0)
			{
				string[] allDataSplit = this._data.Split(new char[]
				{
					'|'
				});
				if (allDataSplit.Length > 0)
				{
					string[] array = allDataSplit;
					for (int i = 0; i < array.Length; i++)
					{
						string currData = array[i];
						string[] realDataSplit = currData.Split(new char[]
						{
							','
						});
						if (realDataSplit.Length >= 2)
						{
							data.Add(new UserAvatarCollectionDataInfo
							{
								TemplateID = int.Parse(realDataSplit[0]),
								Sex = int.Parse(realDataSplit[1])
							});
						}
					}
				}
			}
			return data;
		}
		public void UpdateItems()
		{
			if (this._items == null)
			{
				this._items = this.GetData();
			}
		}
		public bool SaveData()
		{
			bool isOkey = false;
			if (this._items == null)
			{
				this._items = this.GetData();
			}
			string[] arg = new string[2];
			List<string> arg2 = new List<string>();
			if (this._items.Count > 0)
			{
				foreach (UserAvatarCollectionDataInfo avt in this._items)
				{
					arg[0] = avt.TemplateID.ToString();
					arg[1] = avt.Sex.ToString();
					string joinArg = string.Join(",", arg);
					arg2.Add(joinArg);
				}
				if (arg2.Count > 0)
				{
					string joinArg2 = string.Join("|", arg2.ToArray());
					this.Data = joinArg2;
					isOkey = true;
				}
			}
			return isOkey;
		}
		public bool AddItem(UserAvatarCollectionDataInfo item)
		{
			if (this._items == null)
			{
				this._items = this.GetData();
			}
			if (this.GetItemWithTemplateID(item.TemplateID) == null)
			{
				this.Items.Add(item);
				this.SaveData();
				return true;
			}
			return false;
		}
		public bool RemoveItem(UserAvatarCollectionDataInfo item)
		{
			if (this._items == null)
			{
				this._items = this.GetData();
			}
			if (this.GetItemWithTemplateID(item.TemplateID) != null)
			{
				this.Items.Remove(item);
				this.SaveData();
				return true;
			}
			return false;
		}
		public UserAvatarCollectionDataInfo GetItemWithTemplateID(int ItemID)
		{
			if (this._items == null)
			{
				this._items = this.GetData();
			}
			if (this._items.Count > 0)
			{
				foreach (UserAvatarCollectionDataInfo item in this._items)
				{
					if (item.TemplateID == ItemID)
					{
						return item;
					}
				}
				return null;
			}
			return null;
		}
	}
}
