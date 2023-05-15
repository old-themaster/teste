// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.UserFarmInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 066F1F62-A5A7-41DE-A744-392643F34731
// Assembly location: C:\5.3\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
    public class UserFarmInfo : DataObject
    {
        private UserFieldInfo _field;
        private int _ID;
        private int _farmID;
        private string _payFieldMoney;
        private string _payAutoMoney;
        private DateTime _autoPayTime;
        private int _autoValidDate;
        private int _vipLimitLevel;
        private string _farmerName;
        private int _gainFieldId;
        private int _matureId;
        private int _killCropId;
        private int _isAutoId;
        private bool _isFarmHelper;
        private int _buyExpRemainNum;
        private bool _isArrange;

        public UserFieldInfo Field
        {
            get => this._field;
            set
            {
                this._field = value;
                this._isDirty = true;
            }
        }

        public int ID
        {
            get => this._ID;
            set
            {
                this._ID = value;
                this._isDirty = true;
            }
        }

        public int FarmID
        {
            get => this._farmID;
            set
            {
                this._farmID = value;
                this._isDirty = true;
            }
        }

        public string PayFieldMoney
        {
            get => this._payFieldMoney;
            set
            {
                this._payFieldMoney = value;
                this._isDirty = true;
            }
        }

        public string PayAutoMoney
        {
            get => this._payAutoMoney;
            set
            {
                this._payAutoMoney = value;
                this._isDirty = true;
            }
        }

        public DateTime AutoPayTime
        {
            get => this._autoPayTime;
            set
            {
                this._autoPayTime = value;
                this._isDirty = true;
            }
        }

        public int AutoValidDate
        {
            get => this._autoValidDate;
            set
            {
                this._autoValidDate = value;
                this._isDirty = true;
            }
        }

        public int VipLimitLevel
        {
            get => this._vipLimitLevel;
            set
            {
                this._vipLimitLevel = value;
                this._isDirty = true;
            }
        }

        public string FarmerName
        {
            get => this._farmerName;
            set
            {
                this._farmerName = value;
                this._isDirty = true;
            }
        }

        public int GainFieldId
        {
            get => this._gainFieldId;
            set
            {
                this._gainFieldId = value;
                this._isDirty = true;
            }
        }

        public int MatureId
        {
            get => this._matureId;
            set
            {
                this._matureId = value;
                this._isDirty = true;
            }
        }

        public int KillCropId
        {
            get => this._killCropId;
            set
            {
                this._killCropId = value;
                this._isDirty = true;
            }
        }

        public int isAutoId
        {
            get => this._isAutoId;
            set
            {
                this._isAutoId = value;
                this._isDirty = true;
            }
        }

        public bool isFarmHelper
        {
            get => this._isFarmHelper;
            set
            {
                this._isFarmHelper = value;
                this._isDirty = true;
            }
        }

        public int buyExpRemainNum
        {
            get => this._buyExpRemainNum;
            set
            {
                this._buyExpRemainNum = value;
                this._isDirty = true;
            }
        }

        public bool isArrange
        {
            get => this._isArrange;
            set
            {
                this._isArrange = value;
                this._isDirty = true;
            }
        }
    }
}
