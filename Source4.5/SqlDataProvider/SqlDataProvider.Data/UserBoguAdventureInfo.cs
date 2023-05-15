// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.UserBoguAdventureInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9CAC39CD-91A1-49E6-9D63-7507287C2536
// Assembly location: C:\Users\Administrador.OMINIHOST\Desktop\DLL\SqlDataProvider.dll

using System.Collections.Generic;

namespace SqlDataProvider.Data
{
    public class UserBoguAdventureInfo : DataObject
    {
        private int _UserID;
        private int _CurrentPostion;
        private int _HP;
        private int _OpenCount;
        private int _ResetCount;
        private string _Map;
        private List<BoguCeilInfo> _MapData;
        private string _Award;

        public int UserID
        {
            get => this._UserID;
            set
            {
                this._UserID = value;
                this._isDirty = true;
            }
        }

        public int CurrentPostion
        {
            get => this._CurrentPostion;
            set
            {
                this._CurrentPostion = value;
                this._isDirty = true;
            }
        }

        public int HP
        {
            get => this._HP;
            set => this._HP = value;
        }

        public int OpenCount
        {
            get => this._OpenCount;
            set
            {
                this._OpenCount = value;
                this._isDirty = true;
            }
        }

        public int ResetCount
        {
            get => this._ResetCount;
            set
            {
                this._ResetCount = value;
                this._isDirty = true;
            }
        }

        public string Map
        {
            get => this._Map;
            set
            {
                this._Map = value;
                this._isDirty = true;
            }
        }

        public List<BoguCeilInfo> MapData
        {
            get => this._MapData;
            set => this._MapData = value;
        }

        public string Award
        {
            get => this._Award;
            set
            {
                this._Award = value;
                this._isDirty = true;
            }
        }

        public string[] GetAward() => this._Award.Split(',');

        public void SetAward(int slot, int value)
        {
            string[] award = this.GetAward();
            award[slot] = value.ToString();
            this._Award = string.Join(",", award);
        }
    }
}
