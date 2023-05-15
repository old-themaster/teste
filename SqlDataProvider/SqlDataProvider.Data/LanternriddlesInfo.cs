// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.LanternriddlesInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9CAC39CD-91A1-49E6-9D63-7507287C2536
// Assembly location: C:\arquivos 4.1\5.9\SERVIDOR\Emulador\Road\SqlDataProvider.dll

using System;
using System.Collections.Generic;

namespace SqlDataProvider.Data
{
    public class LanternriddlesInfo
    {
        private int m_playerID;
        private int m_questionIndex;
        private int m_questionView;
        private int m_doubleFreeCount;
        private int m_doublePrice;
        private int m_hitFreeCount;
        private int m_hitPrice;
        private int m_myInteger;
        private int m_questionNum;
        private int m_option;
        private bool m_isHint;
        private bool m_isDouble;
        private DateTime m_endDate;
        private Dictionary<int, LightriddleQuestInfo> m_questViews;

        public int PlayerID
        {
            get => this.m_playerID;
            set => this.m_playerID = value;
        }

        public int QuestionIndex
        {
            get => this.m_questionIndex;
            set => this.m_questionIndex = value;
        }

        public int QuestionView
        {
            get => this.m_questionView;
            set => this.m_questionView = value;
        }

        public int DoubleFreeCount
        {
            get => this.m_doubleFreeCount;
            set => this.m_doubleFreeCount = value;
        }

        public int DoublePrice
        {
            get => this.m_doublePrice;
            set => this.m_doublePrice = value;
        }

        public int HitFreeCount
        {
            get => this.m_hitFreeCount;
            set => this.m_hitFreeCount = value;
        }

        public int HitPrice
        {
            get => this.m_hitPrice;
            set => this.m_hitPrice = value;
        }

        public int MyInteger
        {
            get => this.m_myInteger;
            set => this.m_myInteger = value;
        }

        public int QuestionNum
        {
            get => this.m_questionNum;
            set => this.m_questionNum = value;
        }

        public int Option
        {
            get => this.m_option;
            set => this.m_option = value;
        }

        public bool IsHint
        {
            get => this.m_isHint;
            set => this.m_isHint = value;
        }

        public bool IsDouble
        {
            get => this.m_isDouble;
            set => this.m_isDouble = value;
        }

        public DateTime EndDate
        {
            get => this.m_endDate;
            set => this.m_endDate = value;
        }

        public Dictionary<int, LightriddleQuestInfo> QuestViews
        {
            get => this.m_questViews;
            set => this.m_questViews = value;
        }

        public int GetQuestionID => this.m_questViews != null ? this.m_questViews[this.m_questionIndex].QuestionID : 1;

        public LightriddleQuestInfo GetCurrentQuestion => this.m_questViews != null ? this.m_questViews[this.m_questionIndex] : this.m_questViews[1];

        public bool CanNextQuest => this.m_questionIndex <= this.m_questionView - 1;
    }
}
