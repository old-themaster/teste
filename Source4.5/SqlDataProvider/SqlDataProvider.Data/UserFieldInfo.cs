// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.UserFieldInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 066F1F62-A5A7-41DE-A744-392643F34731
// Assembly location: C:\5.3\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
    public class UserFieldInfo : DataObject
    {
        private int _id;
        private int _farmID;
        private int _fieldID;
        private int _seedID;
        private DateTime _plantTime;
        private int _accelerateTime;
        private int _fieldValidDate;
        private DateTime _payTime;
        private int _payFieldTime;
        private int _gainCount;
        private int _gainFieldId;
        private int _autoSeedID;
        private int _autoFertilizerID;
        private int _autoSeedIDCount;
        private int _autoFertilizerCount;
        private bool _isAutomatic;
        private bool _isExist;
        private DateTime _automaticTime;

        public int ID
        {
            get => this._id;
            set
            {
                this._id = value;
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

        public int FieldID
        {
            get => this._fieldID;
            set
            {
                this._fieldID = value;
                this._isDirty = true;
            }
        }

        public int SeedID
        {
            get => this._seedID;
            set
            {
                this._seedID = value;
                this._isDirty = true;
            }
        }

        public DateTime PlantTime
        {
            get => this._plantTime;
            set
            {
                this._plantTime = value;
                this._isDirty = true;
            }
        }

        public int AccelerateTime
        {
            get => this._accelerateTime;
            set
            {
                this._accelerateTime = value;
                this._isDirty = true;
            }
        }

        public int FieldValidDate
        {
            get => this._fieldValidDate;
            set
            {
                this._fieldValidDate = value;
                this._isDirty = true;
            }
        }

        public DateTime PayTime
        {
            get => this._payTime;
            set
            {
                this._payTime = value;
                this._isDirty = true;
            }
        }

        public int payFieldTime
        {
            get => this._payFieldTime;
            set
            {
                this._payFieldTime = value;
                this._isDirty = true;
            }
        }

        public bool IsValidField() => this._payFieldTime == 0 || DateTime.Compare(this._payTime.AddDays((double)this._payFieldTime), DateTime.Now) > 0;

        public int GainCount
        {
            get => this._gainCount;
            set
            {
                this._gainCount = value;
                this._isDirty = true;
            }
        }

        public int gainFieldId
        {
            get => this._gainFieldId;
            set
            {
                this._gainFieldId = value;
                this._isDirty = true;
            }
        }

        public int AutoSeedID
        {
            get => this._autoSeedID;
            set
            {
                this._autoSeedID = value;
                this._isDirty = true;
            }
        }

        public int AutoFertilizerID
        {
            get => this._autoFertilizerID;
            set
            {
                this._autoFertilizerID = value;
                this._isDirty = true;
            }
        }

        public int AutoSeedIDCount
        {
            get => this._autoSeedIDCount;
            set
            {
                this._autoSeedIDCount = value;
                this._isDirty = true;
            }
        }

        public int AutoFertilizerCount
        {
            get => this._autoFertilizerCount;
            set
            {
                this._autoFertilizerCount = value;
                this._isDirty = true;
            }
        }

        public bool isAutomatic
        {
            get => this._isAutomatic;
            set
            {
                this._isAutomatic = value;
                this._isDirty = true;
            }
        }

        public bool IsExit
        {
            get => this._isExist;
            set
            {
                this._isExist = value;
                this._isDirty = true;
            }
        }

        public DateTime AutomaticTime
        {
            get => this._automaticTime;
            set
            {
                this._automaticTime = value;
                this._isDirty = true;
            }
        }

        public bool isDig() => this._fieldValidDate - (int)(DateTime.Now - this._plantTime).TotalMinutes > 0;
    }
}
