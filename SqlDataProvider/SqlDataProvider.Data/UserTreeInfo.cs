using System;

namespace SqlDataProvider.Data
{
    public class UserTreeInfo : DataObject
    {
        private int _CostExp;
        private DateTime _DateUpExp;
        private int _Exp;
        private int _Level;
        private int _LoveNum;
        private int _MosterExp;
        private int _MosterLevel;
        private int _StateMoster;
        private int _userId;

        public int CostExp
        {
            get => this._CostExp;
            set
            {
                this._CostExp = value;
                this._isDirty = true;
            }
        }

        public DateTime DateUpExp
        {
            get => this._DateUpExp;
            set
            {
                this._DateUpExp = value;
                this._isDirty = true;
            }
        }

        public int Exp
        {
            get => this._Exp;
            set
            {
                this._Exp = value;
                this._isDirty = true;
            }
        }

        public int Level
        {
            get => this._Level;
            set
            {
                this._Level = value;
                this._isDirty = true;
            }
        }

        public int LoveNum
        {
            get => this._LoveNum;
            set => this._LoveNum = value;
        }

        public int MosterExp
        {
            get => this._MosterExp;
            set
            {
                this._MosterExp = value;
                this._isDirty = true;
            }
        }

        public int MosterLevel
        {
            get => this._MosterLevel;
            set
            {
                this._MosterLevel = value;
                this._isDirty = true;
            }
        }

        public int StateMoster
        {
            get => this._StateMoster;
            set
            {
                this._StateMoster = value;
                this._isDirty = true;
            }
        }

        public bool StatingFight { get; set; }

        public int UserID
        {
            get => this._userId;
            set
            {
                this._userId = value;
                this._isDirty = true;
            }
        }

        public void ResetMoster()
        {
            this.MosterLevel = 0;
            this.MosterExp = 0;
            this.StateMoster = 0;
            this.StatingFight = false;
        }
    }
}
