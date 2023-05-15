// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.DiceDataInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9CAC39CD-91A1-49E6-9D63-7507287C2536
// Assembly location: C:\Users\Administrador.OMINIHOST\Desktop\DLL\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
    public class DiceDataInfo : DataObject
    {
        private int _ID;
        private int _userID;
        private int _luckIntegral;
        private int _luckIntegralLevel;
        private int _level;
        private int _freeCount;
        private int _currentPosition;
        private bool _userFirstCell;
        private string _awardArray;

        public int ID
        {
            get => this._ID;
            set
            {
                this._ID = value;
                this._isDirty = true;
            }
        }

        public int UserID
        {
            get => this._userID;
            set
            {
                this._userID = value;
                this._isDirty = true;
            }
        }

        public int LuckIntegral
        {
            get => this._luckIntegral;
            set
            {
                this._luckIntegral = value;
                this._isDirty = true;
            }
        }

        public int LuckIntegralLevel
        {
            get => this._luckIntegralLevel;
            set
            {
                this._luckIntegralLevel = value;
                this._isDirty = true;
            }
        }

        public int Level
        {
            get => this._level;
            set
            {
                this._level = value;
                this._isDirty = true;
            }
        }

        public int FreeCount
        {
            get => this._freeCount;
            set
            {
                this._freeCount = value;
                this._isDirty = true;
            }
        }

        public int CurrentPosition
        {
            get => this._currentPosition;
            set
            {
                this._currentPosition = value;
                this._isDirty = true;
            }
        }

        public bool UserFirstCell
        {
            get => this._userFirstCell;
            set
            {
                this._userFirstCell = value;
                this._isDirty = true;
            }
        }

        public string AwardArray
        {
            get => this._awardArray;
            set
            {
                this._awardArray = value;
                this._isDirty = true;
            }
        }
    }
}
