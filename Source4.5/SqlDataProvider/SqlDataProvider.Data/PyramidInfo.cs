// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.PyramidInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9CAC39CD-91A1-49E6-9D63-7507287C2536
// Assembly location: C:\Users\Administrador.OMINIHOST\Desktop\DLL\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
    public class PyramidInfo : DataObject
    {
        private int _ID;
        private int _userID;
        private int _currentLayer;
        private int _maxLayer;
        private int _totalPoint;
        private int _turnPoint;
        private int _pointRatio;
        private int _currentFreeCount;
        private bool _isPyramidStart;
        private string _LayerItems;
        private int _currentReviveCount;

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

        public int currentLayer
        {
            get => this._currentLayer;
            set
            {
                this._currentLayer = value;
                this._isDirty = true;
            }
        }

        public int maxLayer
        {
            get => this._maxLayer;
            set
            {
                this._maxLayer = value;
                this._isDirty = true;
            }
        }

        public int totalPoint
        {
            get => this._totalPoint;
            set
            {
                this._totalPoint = value;
                this._isDirty = true;
            }
        }

        public int turnPoint
        {
            get => this._turnPoint;
            set
            {
                this._turnPoint = value;
                this._isDirty = true;
            }
        }

        public int pointRatio
        {
            get => this._pointRatio;
            set
            {
                this._pointRatio = value;
                this._isDirty = true;
            }
        }

        public int currentFreeCount
        {
            get => this._currentFreeCount;
            set
            {
                this._currentFreeCount = value;
                this._isDirty = true;
            }
        }

        public bool isPyramidStart
        {
            get => this._isPyramidStart;
            set
            {
                this._isPyramidStart = value;
                this._isDirty = true;
            }
        }

        public string LayerItems
        {
            get => this._LayerItems;
            set
            {
                this._LayerItems = value;
                this._isDirty = true;
            }
        }

        public int currentReviveCount
        {
            get => this._currentReviveCount;
            set
            {
                this._currentReviveCount = value;
                this._isDirty = true;
            }
        }
    }
}
