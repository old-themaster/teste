// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.ActivityQuestDataInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AC3D7047-E12B-4BAE-B268-8017031FB40B
// Assembly location: C:\server\fight\SqlDataProvider.dll

using System;

namespace SqlDataProvider.Data
{
    public class ActivityQuestDataInfo : DataObject
    {
        public int GetConditionValue(int index)
        {
            switch (index)
            {
                case 1:
                    return this.Condition1;
                case 2:
                    return this.Condition2;
                case 3:
                    return this.Condition3;
                case 4:
                    return this.Condition4;
                default:
                    throw new Exception("Quest condition index out of range.");
            }
        }

        public void SaveConditionValue(int index, int value)
        {
            switch (index)
            {
                case 1:
                    this.Condition1 = value;
                    break;
                case 2:
                    this.Condition2 = value;
                    break;
                case 3:
                    this.Condition3 = value;
                    break;
                case 4:
                    this.Condition4 = value;
                    break;
                default:
                    throw new Exception("Quest condition index out of range.");
            }
        }

        public int UserID { get; set; }

        public int QuestID { get; set; }

        public bool IsFinished { get; set; }

        public int Condition1 { get; set; }

        public int Condition2 { get; set; }

        public int Condition3 { get; set; }

        public int Condition4 { get; set; }

        public bool IsGetAwards { get; set; }

        public DateTime QuestFinishDateTime { get; set; }

        public bool IsExist { get; set; }
    }
}
