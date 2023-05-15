// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.DDQiYuanInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17193778-438D-49B4-8ECB-C1483D8CE3C1
// Assembly location: C:\DDtank 4.5\baixar flash\Emulator\SqlDataProvider.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlDataProvider.Data
{
    public class DDQiYuanInfo
    {
        private int iD;
        private int userID;
        private string nickName;
        private int myOfferTimes;
        private string hasGetGoodArr;
        private int hasGainTreasureBoxNum;
        private int hasGainJoinRewardCount;
        private string taskReward;
        private int areaId;
        private string areaName;

        public int ID
        {
            get => this.iD;
            set => this.iD = value;
        }

        public int UserID
        {
            get => this.userID;
            set => this.userID = value;
        }

        public string NickName
        {
            get => this.nickName;
            set => this.nickName = value;
        }

        public int MyOfferTimes
        {
            get => this.myOfferTimes;
            set => this.myOfferTimes = value;
        }

        public string HasGetGoodArr
        {
            get => this.hasGetGoodArr;
            set => this.hasGetGoodArr = value;
        }

        public int HasGainTreasureBoxNum
        {
            get => this.hasGainTreasureBoxNum;
            set => this.hasGainTreasureBoxNum = value;
        }

        public int HasGainJoinRewardCount
        {
            get => this.hasGainJoinRewardCount;
            set => this.hasGainJoinRewardCount = value;
        }

        public string TaskReward
        {
            get => this.taskReward;
            set => this.taskReward = value;
        }

        public int AreaId
        {
            get => this.areaId;
            set => this.areaId = value;
        }

        public string AreaName
        {
            get => this.areaName;
            set => this.areaName = value;
        }

        public void SaveTaskReward(int id)
        {
            List<string> list = ((IEnumerable<string>)this.taskReward.Split(new string[1]
            {
        "|"
            }, StringSplitOptions.RemoveEmptyEntries)).ToList<string>();
            list.Add(id.ToString());
            this.taskReward = string.Join("|", list.ToArray());
        }

        public bool CheckTaskReward(int id)
        {
            string taskReward = this.taskReward;
            string[] separator = new string[1] { "|" };
            int num = 1;
            foreach (string s in taskReward.Split(separator, (StringSplitOptions)num))
            {
                if (int.Parse(s) == id)
                    return true;
            }
            return false;
        }

        private bool CheckHasGetGoodArr(int id)
        {
            string hasGetGoodArr = this.hasGetGoodArr;
            string[] separator1 = new string[1] { "|" };
            int num1 = 1;
            foreach (string str in hasGetGoodArr.Split(separator1, (StringSplitOptions)num1))
            {
                string[] separator2 = new string[1] { "," };
                int num2 = 1;
                if (int.Parse(str.Split(separator2, (StringSplitOptions)num2)[0]) == id)
                    return true;
            }
            return false;
        }

        public void SaveHasGetGoodArr(int itemId, int count)
        {
            List<string> list = ((IEnumerable<string>)this.hasGetGoodArr.Split(new string[1]
            {
        "|"
            }, StringSplitOptions.RemoveEmptyEntries)).ToList<string>();
            if (this.CheckHasGetGoodArr(itemId))
            {
                List<string> stringList = new List<string>();
                foreach (string str in list)
                {
                    string[] separator = new string[1] { "," };
                    int num = 1;
                    string[] strArray = str.Split(separator, (StringSplitOptions)num);
                    if (itemId == int.Parse(strArray[0]))
                        stringList.Add(strArray[0] + "," + (object)(int.Parse(strArray[1]) + count));
                    else
                        stringList.Add(strArray[0] + "," + strArray[1]);
                }
                this.HasGetGoodArr = string.Join("|", stringList.ToArray());
            }
            else
            {
                list.Add(itemId.ToString() + "," + (object)count);
                this.HasGetGoodArr = string.Join("|", list.ToArray());
            }
        }

        public int GetCountHasGetGoodArr(int itemID)
        {
            string hasGetGoodArr = this.hasGetGoodArr;
            string[] separator1 = new string[1] { "|" };
            int num1 = 1;
            foreach (string str in ((IEnumerable<string>)hasGetGoodArr.Split(separator1, (StringSplitOptions)num1)).ToList<string>())
            {
                string[] separator2 = new string[1] { "," };
                int num2 = 1;
                string[] strArray = str.Split(separator2, (StringSplitOptions)num2);
                if (itemID == int.Parse(strArray[0]))
                    return int.Parse(strArray[1]);
            }
            return 0;
        }
    }
}
