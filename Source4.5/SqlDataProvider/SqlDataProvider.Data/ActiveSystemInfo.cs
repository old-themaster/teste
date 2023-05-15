// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.ActiveSystemInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EA1EF4C9-8E90-4CDB-9B2B-6C3CF3595312
// Assembly location: C:\DDTank 6.1\Emuladores\Fight\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
    public class ActiveSystemInfo : DataObject
    {
        private int _ID;
        private int _userID;
        private int _myRank;
        private string _nickName;
        private int _totalScore;
        private int _useableScore;
        private int _dayScore;
        private bool _CanGetGift;
        private int _AvailTime;
        private int _canOpenCounts;
        private int _canEagleEyeCounts;
        private DateTime _lastFlushTime;
        private bool _isShowAll;
        private int _isBuy;
        private int _activeMoney;
        private int _activityTanabataNum;
        private int _challengeNum;
        private int _buyBuffNum;
        private int _damageNum;
        private int _luckystarCoins;
        private DateTime _lastEnterYearMonter;
        private string _boxState;
        private string _PuzzleAwardGet;

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

        public int myRank
        {
            get
            {
                return this._myRank;
            }
            set
            {
                this._myRank = value;
                this._isDirty = true;
            }
        }

        public string NickName
        {
            get
            {
                return this._nickName;
            }
            set
            {
                this._nickName = value;
                this._isDirty = true;
            }
        }

        public int totalScore
        {
            get
            {
                return this._totalScore;
            }
            set
            {
                this._totalScore = value;
                this._isDirty = true;
            }
        }

        public int useableScore
        {
            get
            {
                return this._useableScore;
            }
            set
            {
                this._useableScore = value;
                this._isDirty = true;
            }
        }

        public int dayScore
        {
            get
            {
                return this._dayScore;
            }
            set
            {
                this._dayScore = value;
                this._isDirty = true;
            }
        }

        public bool CanGetGift
        {
            get
            {
                return this._CanGetGift;
            }
            set
            {
                this._CanGetGift = value;
                this._isDirty = true;
            }
        }

        public int AvailTime
        {
            get
            {
                return this._AvailTime;
            }
            set
            {
                this._AvailTime = value;
                this._isDirty = true;
            }
        }

        public int canOpenCounts
        {
            get
            {
                return this._canOpenCounts;
            }
            set
            {
                this._canOpenCounts = value;
                this._isDirty = true;
            }
        }

        public int canEagleEyeCounts
        {
            get
            {
                return this._canEagleEyeCounts;
            }
            set
            {
                this._canEagleEyeCounts = value;
                this._isDirty = true;
            }
        }

        public DateTime lastFlushTime
        {
            get
            {
                return this._lastFlushTime;
            }
            set
            {
                this._lastFlushTime = value;
                this._isDirty = true;
            }
        }

        public bool isShowAll
        {
            get
            {
                return this._isShowAll;
            }
            set
            {
                this._isShowAll = value;
                this._isDirty = true;
            }
        }

        public int isBuy
        {
            get
            {
                return this._isBuy;
            }
            set
            {
                this._isBuy = value;
                this._isDirty = true;
            }
        }

        public int ActiveMoney
        {
            get
            {
                return this._activeMoney;
            }
            set
            {
                this._activeMoney = value;
                this._isDirty = true;
            }
        }

        public int activityTanabataNum
        {
            get
            {
                return this._activityTanabataNum;
            }
            set
            {
                this._activityTanabataNum = value;
                this._isDirty = true;
            }
        }

        public int ChallengeNum
        {
            get
            {
                return this._challengeNum;
            }
            set
            {
                this._challengeNum = value;
                this._isDirty = true;
            }
        }

        public int BuyBuffNum
        {
            get
            {
                return this._buyBuffNum;
            }
            set
            {
                this._buyBuffNum = value;
                this._isDirty = true;
            }
        }

        public int DamageNum
        {
            get
            {
                return this._damageNum;
            }
            set
            {
                this._damageNum = value;
                this._isDirty = true;
            }
        }

        public int LuckystarCoins
        {
            get
            {
                return this._luckystarCoins;
            }
            set
            {
                this._luckystarCoins = value;
                this._isDirty = true;
            }
        }

        public DateTime lastEnterYearMonter
        {
            get
            {
                return this._lastEnterYearMonter;
            }
            set
            {
                this._lastEnterYearMonter = value;
                this._isDirty = true;
            }
        }

        public string BoxState
        {
            get
            {
                return this._boxState;
            }
            set
            {
                this._boxState = value;
                this._isDirty = true;
            }
        }
        private DateTime _lastRefresh;

        public DateTime LastRefresh
        {
            get { return _lastRefresh; }
            set
            {
                _lastRefresh = value;
                _isDirty = true;
            }
        }

        private int _EntertamentPoint;
        public int EntertamentPoint
        {
            get
            {
                return this._EntertamentPoint;
            }
            set
            {
                this._EntertamentPoint = value;
                this._isDirty = true;
            }
        }

        private int _curRefreshedTimes;

        public int CurRefreshedTimes
        {
            get { return _curRefreshedTimes; }
            set
            {
                _curRefreshedTimes = value;
                _isDirty = true;
            }
        }

        public string PuzzleAwardGet
        {
            get
            {
                return this._PuzzleAwardGet;
            }
            set
            {
                this._PuzzleAwardGet = value;
                this._isDirty = true;
            }
        }

        public bool IsPuzzleAwardGet(int slot)
        {
            if (slot <= 0 || slot > 3)
                return false;
            string[] strArray = this._PuzzleAwardGet.Split(',');
            return strArray.Length >= 3 && !(strArray[slot - 1] == "0");
        }

        public void SetGetPuzzleAward(int slot)
        {
            if (slot <= 0 || slot > 3)
                return;
            string[] strArray = this._PuzzleAwardGet.Split(',');
            if (strArray.Length < 3)
                return;
            for (int index = 0; index < strArray.Length; ++index)
            {
                if (index == slot - 1)
                    strArray[index] = "1";
            }
            this.PuzzleAwardGet = string.Join(",", strArray);
        }
    }
}
