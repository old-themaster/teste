// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.UserChristmasInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9CAC39CD-91A1-49E6-9D63-7507287C2536
// Assembly location: C:\Users\Administrador.OMINIHOST\Desktop\DLL\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
    public class UserChristmasInfo : DataObject
    {
        private int _ID;
        private int _userID;
        private int _exp;
        private int _awardState;
        private int _count;
        private int _packsNumber;
        private int _lastPacks;
        private int _dayPacks;
        private DateTime _gameBeginTime;
        private DateTime _gameEndTime;
        private bool _isEnter;
        private int _AvailTime;

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

        public int exp
        {
            get => this._exp;
            set
            {
                this._exp = value;
                this._isDirty = true;
            }
        }

        public int awardState
        {
            get => this._awardState;
            set
            {
                this._awardState = value;
                this._isDirty = true;
            }
        }

        public int count
        {
            get => this._count;
            set
            {
                this._count = value;
                this._isDirty = true;
            }
        }

        public int packsNumber
        {
            get => this._packsNumber;
            set
            {
                this._packsNumber = value;
                this._isDirty = true;
            }
        }

        public int lastPacks
        {
            get => this._lastPacks;
            set
            {
                this._lastPacks = value;
                this._isDirty = true;
            }
        }

        public int dayPacks
        {
            get => this._dayPacks;
            set
            {
                this._dayPacks = value;
                this._isDirty = true;
            }
        }

        public DateTime gameBeginTime
        {
            get => this._gameBeginTime;
            set
            {
                this._gameBeginTime = value;
                this._isDirty = true;
            }
        }

        public DateTime gameEndTime
        {
            get => this._gameEndTime;
            set
            {
                this._gameEndTime = value;
                this._isDirty = true;
            }
        }

        public bool isEnter
        {
            get => this._isEnter;
            set
            {
                this._isEnter = value;
                this._isDirty = true;
            }
        }

        public int AvailTime
        {
            get => this._AvailTime;
            set
            {
                this._AvailTime = value;
                this._isDirty = true;
            }
        }
    }
}
