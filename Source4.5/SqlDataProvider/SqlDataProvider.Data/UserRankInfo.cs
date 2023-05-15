using System;

namespace SqlDataProvider.Data
{
	public class UserRankInfo : DataObject
	{
		private bool bool_0;

		private int int_0;

		private NewTitleInfo newTitleInfo_0;

		private DateTime dateTime_0;

		private DateTime dateTime_1;
		private string _userRank;

		private int int_1;

		private int int_2;

		private string string_0;

		private int int_3;

		private int int_4;

		private int int_5;

		private int int_6;

		private int int_7;

		private int int_8;

		private int int_9;

		private int int_10;

		public int Agility
		{
			get
			{
				return this.int_7;
			}
			set
			{
				this.int_7 = value;
				this._isDirty = true;
			}
		}

		public int Attack
		{
			get
			{
				return this.int_4;
			}
			set
			{
				this.int_4 = value;
				this._isDirty = true;
			}
		}

		public DateTime BeginDate
		{
			get
			{
				return this.dateTime_1;
			}
			set
			{
				this.dateTime_1 = value;
				this._isDirty = true;
			}
		}

		public int Damage
		{
			get
			{
				return this.int_9;
			}
			set
			{
				this.int_9 = value;
				this._isDirty = true;
			}
		}

		public int Defence
		{
			get
			{
				return this.int_5;
			}
			set
			{
				this.int_5 = value;
				this._isDirty = true;
			}
		}

		public DateTime EndDate
		{
			get
			{
				return this.dateTime_0;
			}
			set
			{
				this.dateTime_0 = value;
				this._isDirty = true;
			}
		}
		public string UserRank
		{
			get
			{
				return this._userRank;
			}
			set
			{
				this._userRank = value;
				this._isDirty = true;
			}
		}
		public int Guard
		{
			get
			{
				return this.int_10;
			}
			set
			{
				this.int_10 = value;
				this._isDirty = true;
			}
		}

		public int HP
		{
			get
			{
				return this.int_8;
			}
			set
			{
				this.int_8 = value;
				this._isDirty = true;
			}
		}

		public int ID
		{
			get
			{
				return this.int_1;
			}
			set
			{
				this.int_1 = value;
				this._isDirty = true;
			}
		}

		public NewTitleInfo Info
		{
			get
			{
				return this.newTitleInfo_0;
			}
			set
			{
				this.newTitleInfo_0 = value;
			}
		}

		public bool IsExit
		{
			get
			{
				return this.bool_0;
			}
			set
			{
				this.bool_0 = value;
				this._isDirty = true;
			}
		}

		public int Luck
		{
			get
			{
				return this.int_6;
			}
			set
			{
				this.int_6 = value;
				this._isDirty = true;
			}
		}

		public string Name
		{
			get
			{
				return this.string_0;
			}
			set
			{
				this.string_0 = value;
				this._isDirty = true;
			}
		}

		public int NewTitleID
		{
			get
			{
				return this.int_2;
			}
			set
			{
				this.int_2 = value;
				this._isDirty = true;
			}
		}

		public int UserID
		{
			get
			{
				return this.int_0;
			}
			set
			{
				this.int_0 = value;
				this._isDirty = true;
			}
		}

		public int Validate
		{
			get
			{
				return this.int_3;
			}
			set
			{
				this.int_3 = value;
				this._isDirty = true;
			}
		}

		public UserRankInfo()
		{


		}

		public bool IsValidRank()
		{
			if (this.int_3 == 0)
			{
				return true;
			}
			return DateTime.Compare(this.dateTime_1.AddDays((double)this.int_3), DateTime.Now) > 0;
		}
	}
}