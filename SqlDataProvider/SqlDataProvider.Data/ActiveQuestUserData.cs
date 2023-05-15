using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDataProvider.Data
{
    public class ActiveQuestUserData : DataObject
    {
		private int _questID;

		private int _UserID;

		private int _Condiction1;

		private int _Condiction2;

		private int _Condiction3;

		private int _Condiction4;

		private bool _IsCompleted;

		private bool _IsFinished;

		public int QuestID
		{
			get
			{
				return _questID;
			}
			set
			{
				_questID = value;
				base.IsDirty = true;
			}
		}

		public int UserID
		{
			get
			{
				return _UserID;
			}
			set
			{
				_UserID = value;
				base.IsDirty = true;
			}
		}

		public int Condiction1
		{
			get
			{
				return _Condiction1;
			}
			set
			{
				_Condiction1 = value;
				base.IsDirty = true;
			}
		}

		public int Condiction2
		{
			get
			{
				return _Condiction2;
			}
			set
			{
				_Condiction2 = value;
				base.IsDirty = true;
			}
		}

		public int Condiction3
		{
			get
			{
				return _Condiction3;
			}
			set
			{
				_Condiction3 = value;
				base.IsDirty = true;
			}
		}

		public int Condiction4
		{
			get
			{
				return _Condiction4;
			}
			set
			{
				_Condiction4 = value;
				base.IsDirty = true;
			}
		}

		public bool IsFinished
		{
			get
			{
				return _IsFinished;
			}
			set
			{
				_IsFinished = value;
				base.IsDirty = true;
			}
		}

		public bool IsCompleted
		{
			get
			{
				return _IsCompleted;
			}
			set
			{
				_IsCompleted = value;
				base.IsDirty = true;
			}
		}

		public int GetIndex(int Index)
        {
            switch (Index)
            {
                case 2:
                    return Condiction2;
                case 3:
                    return Condiction3;
                case 4:
                    return Condiction4;
                default:
                    return Condiction1;
            }
        }

        public void SetIndex(int Index, int Value)
        {
            switch (Index)
            {
                case 2:
                    Condiction2 = Value;
                    break;
                case 3:
                    Condiction3 = Value;
                    break;
                case 4:
                    Condiction4 = Value;
                    break;

                default:
                    Condiction1 = Value;
                    break;
            }
        }

    }
}
