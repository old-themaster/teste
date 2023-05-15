
using System;
using System.Collections.Generic;

namespace SqlDataProvider.Data
{
    public class UserGodcardInfo
    {
        private int[] _ArraySanXiao = new int[49];
        private int _stepRemain = 10;
        private DateTime _DateTimes = DateTime.Now.AddHours(1.0);
        private string _ListMemoryGameInfo = "";
        private string TmpMemoryGame_ = "0";
        private int _score;
        private int _chipCount;
        private int _freeCount;
        private string _ListCard;
        private string _ListAward;
        private string _ListExchange;
        private int _UserID;
        private int _ID;
        private string _RewardsData;
        private string _StoreData;
        private int _scoreSan;
        private int _crystalNum;
        private int _MemoryGameCount;
        private int _MemoryGameScore;
        private string MemoryGameRewardList_;

        public int ID
        {
            get => this._ID;
            set => this._ID = value;
        }

        public int UserID
        {
            get => this._UserID;
            set => this._UserID = value;
        }

        public int score
        {
            get => this._score;
            set => this._score = value;
        }

        public int chipCount
        {
            get => this._chipCount;
            set => this._chipCount = value;
        }

        public int freeCount
        {
            get => this._freeCount;
            set => this._freeCount = value;
        }

        public string ListCard
        {
            get => this._ListCard;
            set => this._ListCard = value;
        }

        public string ListAward
        {
            get => this._ListAward;
            set => this._ListAward = value;
        }

        public string ListExchange
        {
            get => this._ListExchange;
            set => this._ListExchange = value;
        }

        public int[] ArraySanXiao
        {
            get => this._ArraySanXiao;
            set => this._ArraySanXiao = value;
        }

        public string SV_ArraySanXiao
        {
            get => string.Join<int>("|", (IEnumerable<int>)this._ArraySanXiao);
            set => this._ArraySanXiao = UserGodcardInfo.IntParseFast(value);
        }

        public string RewardsData
        {
            get => this._RewardsData;
            set => this._RewardsData = value;
        }

        public string StoreData
        {
            get => this._StoreData;
            set => this._StoreData = value;
        }

        public int scoreSan
        {
            get => this._scoreSan;
            set => this._scoreSan = value;
        }

        public int stepRemain
        {
            get => this._stepRemain;
            set => this._stepRemain = value;
        }

        public int crystalNum
        {
            get => this._crystalNum;
            set => this._crystalNum = value;
        }

        public DateTime DateTimes
        {
            get => this._DateTimes;
            set => this._DateTimes = value;
        }

        public int MemoryGameCount
        {
            get => this._MemoryGameCount;
            set => this._MemoryGameCount = value;
        }

        public int MemoryGameScore
        {
            get => this._MemoryGameScore;
            set => this._MemoryGameScore = value;
        }

        public string ListMemoryGameInfo
        {
            get => this._ListMemoryGameInfo;
            set => this._ListMemoryGameInfo = value;
        }

        public string TmpMemoryGame
        {
            get => this.TmpMemoryGame_;
            set => this.TmpMemoryGame_ = value;
        }

        public bool ReloadMemoryGame => this.checkReload();

        public string MemoryGameRewardList
        {
            get => this.MemoryGameRewardList_;
            set => this.MemoryGameRewardList_ = value;
        }

        public static int[] IntParseFast(string value)
        {
            int[] numArray1;
            if (value == null)
            {
                numArray1 = new int[49];
            }
            else
            {
                string[] strArray = value.Split(new string[1] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                int[] numArray2 = new int[49];
                for (int index = 0; index < strArray.Length; ++index)
                    numArray2[index] = int.Parse(strArray[index]);
                numArray1 = numArray2;
            }
            return numArray1;
        }

        public void SaveListCard(int ID, int count, ref int Count)
        {
            string[] strArray1 = this._ListCard.Split(new string[1]
            {
        "|"
            }, StringSplitOptions.RemoveEmptyEntries);
            if (strArray1.Length == 0)
            {
                Count = count;
                this.ListCard = ID.ToString() + "," + (object)count;
            }
            else
            {
                string str1 = "";
                bool flag = false;
                for (int index = 0; index < strArray1.Length; ++index)
                {
                    string str2 = strArray1[index];
                    string[] strArray2 = str2.Split(new string[1]
                    {
            ","
                    }, StringSplitOptions.RemoveEmptyEntries);
                    if (int.Parse(strArray2[0]) == ID)
                    {
                        if (str1 == "")
                        {
                            str1 = ID.ToString() + "," + (object)(Count = int.Parse(strArray2[1]) + count);
                            flag = true;
                        }
                        else
                        {
                            str1 = str1 + "|" + (object)ID + "," + (object)(Count = int.Parse(strArray2[1]) + count);
                            flag = true;
                        }
                    }
                    else
                        str1 = !(str1 == "") ? str1 + "|" + str2 : str2;
                }
                if (!flag)
                {
                    if (str1 == "")
                    {
                        Count = count;
                        str1 = this.ListCard = ID.ToString() + "," + (object)count;
                    }
                    else
                    {
                        object[] objArray1 = new object[5]
                        {
              (object) str1,
              (object) "|",
              (object) ID,
              (object) ",",
              null
                        };
                        object[] objArray2 = objArray1;
                        int index = 4;
                        Count = count;
                        objArray2[index] = (object)count;
                        str1 = string.Concat(objArray1);
                    }
                }
                this._ListCard = str1;
            }
        }

        public bool RemoveListCard(int ID, int count, ref int Count)
        {
            string[] strArray1 = this._ListCard.Split(new string[1]
            {
        "|"
            }, StringSplitOptions.RemoveEmptyEntries);
            bool flag1;
            if (strArray1.Length == 0)
            {
                flag1 = false;
            }
            else
            {
                string str1 = "";
                bool flag2 = false;
                for (int index = 0; index < strArray1.Length; ++index)
                {
                    string str2 = strArray1[index];
                    string[] strArray2 = str2.Split(new string[1]
                    {
            ","
                    }, StringSplitOptions.RemoveEmptyEntries);
                    if (int.Parse(strArray2[0]) == ID)
                    {
                        if (int.Parse(strArray2[1]) > count)
                        {
                            if (str1 == "")
                            {
                                str1 = ID.ToString() + "," + (object)(Count = int.Parse(strArray2[1]) - count);
                                flag2 = true;
                            }
                            else
                            {
                                str1 = str1 + "|" + (object)ID + "," + (object)(Count = int.Parse(strArray2[1]) - count);
                                flag2 = true;
                            }
                        }
                        else
                        {
                            if (int.Parse(strArray2[1]) != count)
                                return false;
                            flag2 = true;
                        }
                    }
                    else
                        str1 = !(str1 == "") ? str1 + "|" + str2 : str2;
                }
                this._ListCard = str1;
                flag1 = flag2;
            }
            return flag1;
        }

        public void SaveListExchange(int ID, int count, ref int total)
        {
            string[] strArray1 = this._ListExchange.Split(new string[1]
            {
        "|"
            }, StringSplitOptions.RemoveEmptyEntries);
            if (strArray1.Length == 0)
            {
                this._ListExchange = ID.ToString() + "," + (object)count;
            }
            else
            {
                string str1 = "";
                bool flag = false;
                for (int index = 0; index < strArray1.Length; ++index)
                {
                    string str2 = strArray1[index];
                    string[] strArray2 = str2.Split(new string[1]
                    {
            ","
                    }, StringSplitOptions.RemoveEmptyEntries);
                    if (int.Parse(strArray2[0]) == ID)
                    {
                        if (str1 == "")
                        {
                            str1 = ID.ToString() + "," + (object)(total = int.Parse(strArray2[1]) + count);
                            flag = true;
                        }
                        else
                        {
                            str1 = str1 + "|" + (object)ID + "," + (object)(total = int.Parse(strArray2[1]) + count);
                            flag = true;
                        }
                    }
                    else
                        str1 = !(str1 == "") ? str1 + "|" + str2 : str2;
                }
                if (!flag)
                {
                    if (str1 == "")
                    {
                        total = count;
                        str1 = this.ListCard = "ID," + (object)count;
                    }
                    else
                    {
                        object[] objArray1 = new object[5]
                        {
              (object) str1,
              (object) "|",
              (object) ID,
              (object) ",",
              null
                        };
                        object[] objArray2 = objArray1;
                        int index = 4;
                        total = count;
                        objArray2[index] = (object)count;
                        str1 = string.Concat(objArray1);
                    }
                }
                this._ListExchange = str1;
            }
        }

        public void SaveListAward(int ID)
        {
            if (this._ListAward.Split(new string[1] { "|" }, StringSplitOptions.RemoveEmptyEntries).Length == 0)
                this._ListAward = ID.ToString();
            else
                this._ListAward = this._ListAward + "|" + (object)ID;
        }

        public void SaveRewardsData(int ID)
        {
            if (this._RewardsData.Split(new string[1] { "|" }, StringSplitOptions.RemoveEmptyEntries).Length == 0)
                this._RewardsData = ID.ToString();
            else
                this._RewardsData = this._RewardsData + "|" + (object)ID;
        }

        public void SaveStoreData(int ID, int count)
        {
            string[] strArray1 = this._StoreData.Split(new string[1]
            {
        "|"
            }, StringSplitOptions.RemoveEmptyEntries);
            if (strArray1.Length == 0)
            {
                this._StoreData = ID.ToString() + "," + (object)count;
            }
            else
            {
                string str1 = "";
                bool flag = false;
                for (int index = 0; index < strArray1.Length; ++index)
                {
                    string str2 = strArray1[index];
                    string[] strArray2 = str2.Split(new string[1]
                    {
            ","
                    }, StringSplitOptions.RemoveEmptyEntries);
                    if (int.Parse(strArray2[0]) == ID)
                    {
                        if (str1 == "")
                        {
                            str1 = ID.ToString() + "," + (object)(int.Parse(strArray2[1]) + count);
                            flag = true;
                        }
                        else
                        {
                            str1 = str1 + "|" + (object)ID + "," + (object)(int.Parse(strArray2[1]) + count);
                            flag = true;
                        }
                    }
                    else
                        str1 = !(str1 == "") ? str1 + "|" + str2 : str2;
                }
                if (!flag)
                {
                    if (str1 == "")
                        str1 = ID.ToString() + "," + (object)count;
                    else
                        str1 = str1 + "|" + (object)ID + "," + (object)count;
                }
                this._StoreData = str1;
            }
        }

        public bool SaveMemoryGameInfo(
          int place,
          ref int ItemID,
          ref int count,
          ref bool isGet,
          ref bool place2,
          ref int Valid)
        {
            string[] strArray1 = this._ListMemoryGameInfo.Split(new string[1]
            {
        "|"
            }, StringSplitOptions.RemoveEmptyEntries);
            List<string> stringList = new List<string>();
            for (int index = 0; index < strArray1.Length; ++index)
            {
                string[] strArray2 = strArray1[index].Split(new string[1]
                {
          ","
                }, StringSplitOptions.RemoveEmptyEntries);
                if (int.Parse(strArray2[2]) == place)
                {
                    strArray2[5] = "1";
                    ItemID = int.Parse(strArray2[0]);
                    count = int.Parse(strArray2[1]);
                    Valid = int.Parse(strArray2[7]);
                }
                else if (int.Parse(strArray2[3]) == place)
                {
                    strArray2[6] = "1";
                    ItemID = int.Parse(strArray2[0]);
                    count = int.Parse(strArray2[1]);
                    Valid = int.Parse(strArray2[7]);
                }
                if (int.Parse(strArray2[5]) == 1 && int.Parse(strArray2[6]) == 1)
                {
                    if (int.Parse(strArray2[4]) == 0)
                    {
                        isGet = true;
                        place2 = true;
                    }
                    strArray2[4] = "1";
                }
                else
                {
                    int num;
                    if (this.TmpMemoryGame_.Length > 0)
                        num = int.Parse(this.TmpMemoryGame_.Split(new string[1]
                        {
              "|"
                        }, StringSplitOptions.RemoveEmptyEntries)[0]) != 1 ? 1 : 0;
                    else
                        num = 1;
                    if (num == 0)
                    {
                        strArray2[5] = "0";
                        strArray2[6] = "0";
                    }
                }
                stringList.Add(string.Join(",", strArray2));
            }
            this._ListMemoryGameInfo = string.Join("|", stringList.ToArray());
            return true;
        }

        private bool checkReload()
        {
            string[] strArray = this._ListMemoryGameInfo.Split(new string[1]
            {
        "|"
            }, StringSplitOptions.RemoveEmptyEntries);
            List<string> stringList = new List<string>();
            for (int index = 0; index < strArray.Length; ++index)
            {
                if (int.Parse(strArray[index].Split(new string[1]
                {
          ","
                }, StringSplitOptions.RemoveEmptyEntries)[4]) == 0)
                    return false;
            }
            return true;
        }

        public void SaveMemoryGameRewardList(int ID)
        {
            if (this.MemoryGameRewardList_.Split(new string[1]
            {
        "|"
            }, StringSplitOptions.RemoveEmptyEntries).Length == 0)
                this.MemoryGameRewardList_ = ID.ToString();
            else
                this.MemoryGameRewardList_ = this.MemoryGameRewardList_ + "|" + (object)ID;
        }
    }
}
