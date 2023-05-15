
using System;

namespace SqlDataProvider.Data
{
    public class PyramidConfigInfo
    {
        private int _userID;
        private bool _isOpen;
        private bool _isScoreExchange;
        private DateTime _beginTime;
        private DateTime _endTime;
        private int _freeCount;
        private int _turnCardPrice;
        private int[] _revivePrice;

        public int UserID
        {
            get => this._userID;
            set => this._userID = value;
        }

        public bool isOpen
        {
            get => this._isOpen;
            set => this._isOpen = value;
        }

        public bool isScoreExchange
        {
            get => this._isScoreExchange;
            set => this._isScoreExchange = value;
        }

        public DateTime beginTime
        {
            get => this._beginTime;
            set => this._beginTime = value;
        }

        public DateTime endTime
        {
            get => this._endTime;
            set => this._endTime = value;
        }

        public int freeCount
        {
            get => this._freeCount;
            set => this._freeCount = value;
        }

        public int turnCardPrice
        {
            get => this._turnCardPrice;
            set => this._turnCardPrice = value;
        }

        public int[] revivePrice
        {
            get => this._revivePrice;
            set => this._revivePrice = value;
        }
    }
}
