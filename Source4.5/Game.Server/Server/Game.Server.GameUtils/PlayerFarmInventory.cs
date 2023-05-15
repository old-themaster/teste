// Decompiled with JetBrains decompiler
// Type: Game.Server.GameUtils.PlayerFarmInventory
// Assembly: Game.Server, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 867CF6A2-58A6-429A-A438-35AE88F781E9
// Assembly location: C:\5.3\Game.Server.dll

using Bussiness.Managers;
using Game.Logic;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Game.Server.GameUtils
{
    public abstract class PlayerFarmInventory
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected object m_lock = new object();
        private int m_capalility;
        private int m_beginSlot;
        protected UserFarmInfo m_farm;
        protected UserFieldInfo[] m_fields;
        protected UserFarmInfo m_otherFarm;
        protected UserFieldInfo[] m_otherFields;
        protected int m_farmstatus;

        public int Status => this.m_farmstatus;

        public int BeginSlot => this.m_beginSlot;

        public int Capalility
        {
            get => this.m_capalility;
            set => this.m_capalility = value < 0 ? 0 : (value > this.m_fields.Length ? this.m_fields.Length : value);
        }

        public bool IsEmpty(int slot) => slot < 0 || slot >= this.m_capalility || this.m_fields[slot] == null;

        public PlayerFarmInventory(int capability, int beginSlot)
        {
            this.m_capalility = capability;
            this.m_beginSlot = beginSlot;
            this.m_fields = new UserFieldInfo[capability];
            this.m_farm = new UserFarmInfo();
            this.m_otherFields = new UserFieldInfo[capability];
            this.m_otherFarm = new UserFarmInfo();
            this.m_farmstatus = 0;
        }

        public bool AddField(UserFieldInfo item) => this.AddField(item, this.m_beginSlot);

        public bool AddField(UserFieldInfo item, int minSlot)
        {
            if (item == null)
                return false;
            int firstEmptySlot = this.FindFirstEmptySlot(minSlot);
            return this.AddFieldTo(item, firstEmptySlot);
        }

        public virtual bool AddFieldTo(UserFieldInfo item, int place)
        {
            if (item == null || place >= this.m_capalility || place < 0)
                return false;
            lock (this.m_lock)
            {
                this.m_fields[place] = item;
                if (this.m_fields[place] != null)
                {
                    place = -1;
                }
                else
                {
                    this.m_fields[place] = item;
                    item.FieldID = place;
                }
            }
            return place != -1;
        }

        public virtual bool AddOtherFieldTo(UserFieldInfo item, int place)
        {
            if (item == null || place >= this.m_capalility || place < 0)
                return false;
            lock (this.m_lock)
            {
                this.m_otherFields[place] = item;
                if (this.m_otherFields[place] != null)
                {
                    place = -1;
                }
                else
                {
                    this.m_otherFields[place] = item;
                    item.FieldID = place;
                }
            }
            return place != -1;
        }

        public virtual bool RemoveField(UserFieldInfo item)
        {
            if (item == null)
                return false;
            int num = -1;
            lock (this.m_lock)
            {
                for (int index = 0; index < this.m_capalility; ++index)
                {
                    if (this.m_fields[index] == item)
                    {
                        num = index;
                        this.m_fields[index] = (UserFieldInfo)null;
                        break;
                    }
                }
            }
            return num != -1;
        }

        public virtual bool RemoveOtherField(UserFieldInfo item)
        {
            if (item == null)
                return false;
            int num = -1;
            lock (this.m_lock)
            {
                for (int index = 0; index < this.m_capalility; ++index)
                {
                    if (this.m_otherFields[index] == item)
                    {
                        num = index;
                        this.m_otherFields[index] = (UserFieldInfo)null;
                        break;
                    }
                }
            }
            return num != -1;
        }

        public bool RemoveFieldAt(int place) => this.RemoveField(this.GetFieldAt(place));

        public virtual UserFieldInfo GetFieldAt(int slot) => slot < 0 || slot >= this.m_capalility ? (UserFieldInfo)null : this.m_fields[slot];

        public int FindFirstEmptySlot() => this.FindFirstEmptySlot(this.m_beginSlot);

        public int FindFirstEmptySlot(int minSlot)
        {
            if (minSlot >= this.m_capalility)
                return -1;
            int num;
            lock (this.m_lock)
            {
                for (int index = minSlot; index < this.m_capalility; ++index)
                {
                    if (this.m_fields[index] == null)
                    {
                        num = index;
                        return num;
                    }
                }
                num = -1;
            }
            return num;
        }

        public void ClearFields()
        {
            lock (this.m_lock)
            {
                for (int beginSlot = this.m_beginSlot; beginSlot < this.m_capalility; ++beginSlot)
                {
                    if (this.m_fields[beginSlot] != null)
                        this.RemoveField(this.m_fields[beginSlot]);
                }
            }
        }

        public void ClearOtherFields()
        {
            lock (this.m_lock)
            {
                for (int beginSlot = this.m_beginSlot; beginSlot < this.m_capalility; ++beginSlot)
                {
                    if (this.m_otherFields[beginSlot] != null)
                        this.RemoveOtherField(this.m_otherFields[beginSlot]);
                }
            }
        }

        public int FindLastEmptySlot()
        {
            int num;
            lock (this.m_lock)
            {
                for (int index = this.m_capalility - 1; index >= 0; --index)
                {
                    if (this.m_fields[index] == null)
                    {
                        num = index;
                        return num;
                    }
                }
                num = -1;
            }
            return num;
        }

        public virtual void ClearFarm()
        {
            lock (this.m_lock)
                this.m_farm = (UserFarmInfo)null;
        }

        public virtual void ResetFarmProp()
        {
            lock (this.m_lock)
            {
                if (this.m_farm == null)
                    return;
                this.m_farm.isArrange = false;
                this.m_farm.buyExpRemainNum = 20;
            }
        }

        public virtual void ClearIsArrange()
        {
            lock (this.m_lock)
                this.m_farm.isArrange = true;
        }

        public virtual void UpdateGainCount(int fieldId, int count)
        {
            lock (this.m_lock)
                this.m_fields[fieldId].GainCount = count;
        }

        public virtual void UpdateFarm(UserFarmInfo farm)
        {
            lock (this.m_lock)
                this.m_farm = farm;
        }

        public virtual void UpdateOtherFarm(UserFarmInfo farm)
        {
            lock (this.m_lock)
                this.m_otherFarm = farm;
        }

        public virtual bool GrowField(int fieldId, int templateID)
        {
            ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(templateID);
            lock (this.m_lock)
            {
                this.m_fields[fieldId].SeedID = itemTemplate.TemplateID;
                this.m_fields[fieldId].PlantTime = DateTime.Now;
                this.m_fields[fieldId].GainCount = itemTemplate.Property2;
                this.m_fields[fieldId].FieldValidDate = itemTemplate.Property3;
            }
            return true;
        }

        public virtual bool killCropField(int fieldId)
        {
            lock (this.m_lock)
            {
                if (this.m_fields[fieldId] != null)
                {
                    this.m_fields[fieldId].SeedID = 0;
                    this.m_fields[fieldId].FieldValidDate = 1;
                    this.m_fields[fieldId].AccelerateTime = 0;
                    this.m_fields[fieldId].GainCount = 0;
                }
            }
            return true;
        }

        public virtual void StopHelperSwitchField()
        {
            lock (this.m_lock)
            {
                this.m_farm.isFarmHelper = false;
                this.m_farm.isAutoId = 0;
                this.m_farm.AutoPayTime = DateTime.Now;
                this.m_farm.AutoValidDate = 0;
                this.m_farm.GainFieldId = 0;
                this.m_farm.KillCropId = 0;
            }
        }

        public virtual void CreateFarm(int ID, string name)
        {
            string str1 = PetMgr.FindConfig("PayFieldMoney").Value;
            string str2 = PetMgr.FindConfig("PayAutoMoney").Value;
            this.UpdateFarm(new UserFarmInfo()
            {
                ID = 0,
                FarmID = ID,
                FarmerName = name,
                isFarmHelper = false,
                isAutoId = 0,
                AutoPayTime = DateTime.Now,
                AutoValidDate = 1,
                GainFieldId = 0,
                KillCropId = 0,
                PayAutoMoney = str2,
                PayFieldMoney = str1,
                buyExpRemainNum = 20,
                isArrange = false
            });
            this.CreateNewField(ID, 0, 8);
        }

        public virtual bool HelperSwitchFields(
          bool isHelper,
          int seedID,
          int seedTime,
          int haveCount,
          int getCount)
        {
            if (isHelper)
            {
                lock (this.m_lock)
                {
                    for (int index = 0; index < this.m_fields.Length; ++index)
                    {
                        if (this.m_fields[index] != null)
                        {
                            this.m_fields[index].SeedID = 0;
                            this.m_fields[index].FieldValidDate = 1;
                            this.m_fields[index].AccelerateTime = 0;
                            this.m_fields[index].GainCount = 0;
                        }
                    }
                }
            }
            lock (this.m_lock)
            {
                this.m_farm.isFarmHelper = isHelper;
                this.m_farm.isAutoId = seedID;
                this.m_farm.AutoPayTime = DateTime.Now;
                this.m_farm.AutoValidDate = seedTime;
                this.m_farm.GainFieldId = getCount / 10;
                this.m_farm.KillCropId = getCount;
            }
            return true;
        }

        public virtual void CreateNewField(int ID, int minslot, int maxslot)
        {
            for (int place = minslot; place < maxslot; ++place)
                this.AddFieldTo(new UserFieldInfo()
                {
                    ID = 0,
                    FarmID = ID,
                    FieldID = place,
                    SeedID = 0,
                    PayTime = DateTime.Now.AddYears(100),
                    payFieldTime = 876000,
                    PlantTime = DateTime.Now,
                    GainCount = 0,
                    FieldValidDate = 1,
                    AccelerateTime = 0,
                    AutomaticTime = DateTime.Now,
                    IsExit = true
                }, place);
        }

        public virtual bool CreateField(int ID, List<int> fieldIds, int payFieldTime)
        {
            for (int index = 0; index < fieldIds.Count; ++index)
            {
                int fieldId = fieldIds[index];
                DateTime now;
                if (this.m_fields[fieldId] == null)
                {
                    UserFieldInfo userFieldInfo1 = new UserFieldInfo();
                    userFieldInfo1.FarmID = ID;
                    userFieldInfo1.FieldID = fieldId;
                    userFieldInfo1.SeedID = 0;
                    UserFieldInfo userFieldInfo2 = userFieldInfo1;
                    now = DateTime.Now;
                    DateTime dateTime = now.AddDays((double)(payFieldTime / 24));
                    userFieldInfo2.PayTime = dateTime;
                    userFieldInfo1.payFieldTime = payFieldTime;
                    userFieldInfo1.PlantTime = DateTime.Now;
                    userFieldInfo1.GainCount = 0;
                    userFieldInfo1.FieldValidDate = 1;
                    userFieldInfo1.AccelerateTime = 0;
                    userFieldInfo1.AutomaticTime = DateTime.Now;
                    userFieldInfo1.IsExit = true;
                    this.AddFieldTo(userFieldInfo1, fieldId);
                }
                else
                {
                    UserFieldInfo field = this.m_fields[fieldId];
                    now = DateTime.Now;
                    DateTime dateTime = now.AddDays((double)(payFieldTime / 24));
                    field.PayTime = dateTime;
                    this.m_fields[fieldId].payFieldTime = payFieldTime;
                }
            }
            return true;
        }

        public virtual int AccelerateTimeFields(DateTime PlantTime, int FieldValidDate)
        {
            DateTime now = DateTime.Now;
            int num1 = now.Hour - PlantTime.Hour;
            int num2 = now.Minute - PlantTime.Minute;
            if (num1 < 0)
                num1 = 24 + num1;
            if (num2 < 0)
                num2 = 60 + num2;
            int num3 = num1 * 60 + num2;
            if (num3 > FieldValidDate)
                num3 = FieldValidDate;
            return num3;
        }

        public virtual bool AccelerateTimeFields()
        {
            lock (this.m_lock)
            {
                for (int index = 0; index < this.m_capalility; ++index)
                {
                    if (this.m_fields[index] != null && this.m_fields[index].SeedID > 0)
                    {
                        DateTime plantTime = this.m_fields[index].PlantTime;
                        int fieldValidDate = this.m_fields[index].FieldValidDate;
                        this.m_fields[index].AccelerateTime = this.AccelerateTimeFields(plantTime, fieldValidDate);
                    }
                }
            }
            return true;
        }

        public virtual bool AccelerateOtherTimeFields()
        {
            lock (this.m_lock)
            {
                for (int index = 0; index < this.m_capalility; ++index)
                {
                    if (this.m_otherFields[index] != null && this.m_otherFields[index].SeedID > 0)
                    {
                        DateTime plantTime = this.m_otherFields[index].PlantTime;
                        int fieldValidDate = this.m_otherFields[index].FieldValidDate;
                        this.m_otherFields[index].AccelerateTime = this.AccelerateTimeFields(plantTime, fieldValidDate);
                    }
                }
            }
            return true;
        }

        public virtual UserFieldInfo[] GetFields()
        {
            List<UserFieldInfo> userFieldInfoList = new List<UserFieldInfo>();
            lock (this.m_lock)
            {
                for (int index = 0; index < this.m_capalility; ++index)
                {
                    if (this.m_fields[index] != null && this.m_fields[index].IsValidField())
                        userFieldInfoList.Add(this.m_fields[index]);
                }
            }
            return userFieldInfoList.ToArray();
        }

        public virtual UserFieldInfo[] GetOtherFields()
        {
            List<UserFieldInfo> userFieldInfoList = new List<UserFieldInfo>();
            lock (this.m_lock)
            {
                for (int index = 0; index < this.m_capalility; ++index)
                {
                    if (this.m_otherFields[index] != null && this.m_otherFields[index].IsValidField())
                        userFieldInfoList.Add(this.m_otherFields[index]);
                }
            }
            return userFieldInfoList.ToArray();
        }

        public virtual UserFieldInfo GetOtherFieldAt(int slot) => slot < 0 || slot >= this.m_capalility ? (UserFieldInfo)null : this.m_otherFields[slot];

        public int GetEmptyCount() => this.GetEmptyCount(this.m_beginSlot);

        public virtual int GetEmptyCount(int minSlot)
        {
            if (minSlot < 0 || minSlot > this.m_capalility - 1)
                return 0;
            int num = 0;
            lock (this.m_lock)
            {
                for (int index = minSlot; index < this.m_capalility; ++index)
                {
                    if (this.m_fields[index] == null)
                        ++num;
                }
            }
            return num;
        }

        public virtual int payFieldMoneyToWeek() => int.Parse(this.m_farm.PayFieldMoney.Split('|')[0].Split(',')[1]);

        public virtual int payFieldTimeToWeek() => int.Parse(this.m_farm.PayFieldMoney.Split('|')[0].Split(',')[0]);

        public virtual int payFieldMoneyToMonth() => int.Parse(this.m_farm.PayFieldMoney.Split('|')[1].Split(',')[1]);

        public virtual int payFieldTimeToMonth() => int.Parse(this.m_farm.PayFieldMoney.Split('|')[1].Split(',')[0]);

        public virtual UserFarmInfo CreateFarmForNulll(int ID) => new UserFarmInfo()
        {
            FarmID = ID,
            FarmerName = "Null",
            isFarmHelper = false,
            isAutoId = 0,
            AutoPayTime = DateTime.Now,
            AutoValidDate = 1,
            GainFieldId = 0,
            KillCropId = 0,
            isArrange = true
        };

        public virtual UserFieldInfo[] CreateFieldsForNull(int ID)
        {
            List<UserFieldInfo> userFieldInfoList = new List<UserFieldInfo>();
            for (int index = 0; index < 8; ++index)
                userFieldInfoList.Add(new UserFieldInfo()
                {
                    FarmID = ID,
                    FieldID = index,
                    SeedID = 0,
                    PayTime = DateTime.Now,
                    payFieldTime = 365000,
                    PlantTime = DateTime.Now,
                    GainCount = 0,
                    FieldValidDate = 1,
                    AccelerateTime = 0,
                    AutomaticTime = DateTime.Now
                });
            return userFieldInfoList.ToArray();
        }
    }
}
