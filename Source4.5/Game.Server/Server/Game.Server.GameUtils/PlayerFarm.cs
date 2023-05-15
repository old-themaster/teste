// Decompiled with JetBrains decompiler
// Type: Game.Server.GameUtils.PlayerFarm
// Assembly: Game.Server, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 867CF6A2-58A6-429A-A438-35AE88F781E9
// Assembly location: C:\5.3\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Server.Managers;
using Game.Server.Packets;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Game.Server.GameUtils
{
    public class PlayerFarm : PlayerFarmInventory
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected GamePlayer m_player;
        private bool m_saveToDb;
        private List<UserFieldInfo> m_removedList = new List<UserFieldInfo>();

        public GamePlayer Player => this.m_player;

        public UserFarmInfo OtherFarm => this.m_otherFarm;

        public UserFieldInfo[] OtherFields => this.m_otherFields;

        public UserFarmInfo CurrentFarm => this.m_farm;

        public UserFieldInfo[] CurrentFields => this.m_fields;

        public PlayerFarm(GamePlayer player, bool saveTodb, int capibility, int beginSlot)
          : base(capibility, beginSlot)
        {
            this.m_player = player;
            this.m_saveToDb = saveTodb;
        }

        public virtual void LoadFromDatabase()
        {
            if (!this.m_saveToDb)
                return;
            using (PlayerBussiness playerBussiness = new PlayerBussiness())
            {
                UserFarmInfo singleFarm = playerBussiness.GetSingleFarm(this.m_player.PlayerCharacter.ID);
                UserFieldInfo[] singleFields = playerBussiness.GetSingleFields(this.m_player.PlayerCharacter.ID);
                this.UpdateFarm(singleFarm);
                foreach (UserFieldInfo userFieldInfo in singleFields)
                    this.AddFieldTo(userFieldInfo, userFieldInfo.FieldID, singleFarm.FarmID);
            }
        }

        public virtual void SaveToDatabase()
        {
            if (!this.m_saveToDb)
                return;
            using (PlayerBussiness playerBussiness = new PlayerBussiness())
            {
                lock (this.m_lock)
                {
                    if (this.m_farm != null && this.m_farm.IsDirty)
                    {
                        if (this.m_farm.ID > 0)
                            playerBussiness.UpdateFarm(this.m_farm);
                        else
                            playerBussiness.AddFarm(this.m_farm);
                        for (int index = 0; index < this.m_fields.Length; ++index)
                        {
                            UserFieldInfo field = this.m_fields[index];
                            if (field != null && field.IsDirty)
                            {
                                if (field.ID > 0)
                                    playerBussiness.UpdateFields(field);
                                else
                                    playerBussiness.AddFields(field);
                            }
                        }
                    }
                }
            }
        }

        public bool AddFieldTo(UserFieldInfo item, int place, int farmId)
        {
            if (!this.AddFieldTo(item, place))
                return false;
            item.FarmID = farmId;
            return true;
        }

        public bool AddOtherFieldTo(UserFieldInfo item, int place, int farmId)
        {
            if (!this.AddOtherFieldTo(item, place))
                return false;
            item.FarmID = farmId;
            return true;
        }

        public void EnterFarm()
        {
            this.CropHelperSwitchField(false);
            if (this.m_farm == null)
                this.CreateFarm(this.m_player.PlayerCharacter.ID, this.m_player.PlayerCharacter.NickName);
            if (!this.AccelerateTimeFields())
                return;
            this.m_player.Out.SendEnterFarm(this.m_player.PlayerCharacter, this.CurrentFarm, this.GetFields());
            this.m_farmstatus = 1;
        }

        public void CropHelperSwitchField(bool isStopFarmHelper)
        {
            if (this.m_farm == null || !this.m_farm.isFarmHelper)
                return;
            SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(ItemMgr.FindItemTemplate(this.m_farm.isAutoId).Property4), 1, 102);
            int num1 = this.m_farm.AutoValidDate * 60;
            TimeSpan timeSpan = DateTime.Now - this.m_farm.AutoPayTime;
            int killCropId = this.m_farm.KillCropId;
            int num2 = (1 - (timeSpan.TotalMilliseconds >= 0.0 ? num1 - (int)timeSpan.TotalMilliseconds : num1) / num1) * killCropId / 1000;
            if (num2 > killCropId)
            {
                num2 = killCropId;
                isStopFarmHelper = true;
            }
            if (isStopFarmHelper)
            {
                fromTemplate.Count = num2;
                if (num2 > 0)
                {
                    string content = "Kết thúc trợ thủ, bạn nhận được " + (object)num2 + " " + fromTemplate.Template.Name;
                    string title = "Kết thúc trợ thủ, nhận được thức ăn thú cưng!";
                    this.m_player.SendItemToMail(fromTemplate, content, title, eMailType.ItemOverdue);
                    this.m_player.Out.SendMailResponse(this.m_player.PlayerCharacter.ID, eMailRespose.Receiver);
                }
                lock (this.m_lock)
                {
                    this.m_farm.isFarmHelper = false;
                    this.m_farm.isAutoId = 0;
                    this.m_farm.AutoPayTime = DateTime.Now;
                    this.m_farm.AutoValidDate = 0;
                    this.m_farm.GainFieldId = 0;
                    this.m_farm.KillCropId = 0;
                }
                this.m_player.Out.SendHelperSwitchField(this.m_player.PlayerCharacter, this.m_farm);
            }
        }

        public void ExitFarm() => this.m_farmstatus = 0;

        public virtual void HelperSwitchField(
          bool isHelper,
          int seedID,
          int seedTime,
          int haveCount,
          int getCount)
        {
            if (!this.HelperSwitchFields(isHelper, seedID, seedTime, haveCount, getCount))
                return;
            this.m_player.Out.SendHelperSwitchField(this.m_player.PlayerCharacter, this.m_farm);
        }

        public virtual bool GainFriendFields(int userId, int fieldId)
        {
            GamePlayer playerById = WorldMgr.GetPlayerById(userId);
            UserFieldInfo otherField1 = this.m_otherFields[fieldId];
            if (otherField1 == null)
                return false;
            SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(ItemMgr.FindItemTemplate(otherField1.SeedID).Property4), 1, 102);
            List<SqlDataProvider.Data.ItemInfo> items = new List<SqlDataProvider.Data.ItemInfo>();
            this.AccelerateTimeFields();
            if (this.GetOtherFieldAt(fieldId).isDig())
                return false;
            lock (this.m_lock)
            {
                if (this.m_otherFields[fieldId].GainCount <= 9)
                    return false;
                --this.m_otherFields[fieldId].GainCount;
            }
            if (!this.m_player.PropBag.StackItemToAnother(fromTemplate) && !this.m_player.PropBag.AddItem(fromTemplate))
                items.Add(fromTemplate);
            if (playerById == null)
            {
                using (PlayerBussiness playerBussiness = new PlayerBussiness())
                {
                    for (int index = 0; index < this.m_otherFields.Length; ++index)
                    {
                        UserFieldInfo otherField2 = this.m_otherFields[index];
                        if (otherField2 != null)
                            playerBussiness.UpdateFields(otherField2);
                    }
                }
            }
            else if (playerById.Farm.Status == 1)
            {
                playerById.Farm.UpdateGainCount(fieldId, this.m_otherFields[fieldId].GainCount);
                playerById.Out.SendtoGather(playerById.PlayerCharacter, this.m_otherFields[fieldId]);
            }
            this.m_player.Out.SendtoGather(this.m_player.PlayerCharacter, this.m_otherFields[fieldId]);
            if (items.Count > 0)
            {
                this.m_player.SendItemsToMail(items, "Bagfull trả về thư!", "Bagfull trả về thư!", eMailType.ItemOverdue);
                this.m_player.Out.SendMailResponse(this.m_player.PlayerCharacter.ID, eMailRespose.Receiver);
            }
            return true;
        }

        public void EnterFriendFarm(int userId)
        {
            GamePlayer playerById = WorldMgr.GetPlayerById(userId);
            UserFarmInfo farm;
            UserFieldInfo[] userFieldInfoArray;
            if (playerById == null)
            {
                using (PlayerBussiness playerBussiness = new PlayerBussiness())
                {
                    farm = playerBussiness.GetSingleFarm(userId);
                    userFieldInfoArray = playerBussiness.GetSingleFields(userId);
                }
            }
            else
            {
                farm = playerById.Farm.CurrentFarm;
                userFieldInfoArray = playerById.Farm.CurrentFields;
                playerById.ViFarmsAdd(this.m_player.PlayerCharacter.ID);
            }
            if (farm == null)
            {
                farm = this.CreateFarmForNulll(userId);
                userFieldInfoArray = this.CreateFieldsForNull(userId);
            }
            this.m_farmstatus = this.m_player.PlayerCharacter.ID;
            this.UpdateOtherFarm(farm);
            this.ClearOtherFields();
            foreach (UserFieldInfo userFieldInfo in userFieldInfoArray)
            {
                if (userFieldInfo != null)
                    this.AddOtherFieldTo(userFieldInfo, userFieldInfo.FieldID, farm.FarmID);
            }
            if (!this.AccelerateOtherTimeFields())
                return;
            this.m_player.Out.SendEnterFarm(this.m_player.PlayerCharacter, this.OtherFarm, this.GetOtherFields());
        }

        public virtual void PayField(List<int> fieldIds, int payFieldTime)
        {
            if (!this.CreateField(this.m_player.PlayerCharacter.ID, fieldIds, payFieldTime))
                return;
            foreach (int viFarm in this.m_player.ViFarms)
            {
                GamePlayer playerById = WorldMgr.GetPlayerById(viFarm);
                if (playerById != null && playerById.Farm.Status == viFarm)
                    playerById.Out.SendPayFields(this.m_player, fieldIds);
            }
            this.m_player.Out.SendPayFields(this.m_player, fieldIds);
        }

        public override bool GrowField(int fieldId, int templateID)
        {
            if (!base.GrowField(fieldId, templateID))
                return false;
            foreach (int viFarm in this.m_player.ViFarms)
            {
                GamePlayer playerById = WorldMgr.GetPlayerById(viFarm);
                if (playerById != null && playerById.Farm.Status == viFarm)
                    playerById.Out.SendSeeding(this.m_player.PlayerCharacter, this.m_fields[fieldId]);
            }
            this.m_player.Out.SendSeeding(this.m_player.PlayerCharacter, this.m_fields[fieldId]);
            return true;
        }

        public override bool killCropField(int fieldId)
        {
            if (!base.killCropField(fieldId))
                return false;
            this.m_player.Out.SendKillCropField(this.m_player.PlayerCharacter, this.m_fields[fieldId]);
            return true;
        }

        public virtual bool GainField(int fieldId)
        {
            this.AccelerateTimeFields();
            if (this.GetFieldAt(fieldId).isDig() || fieldId < 0 || fieldId > ((IEnumerable<UserFieldInfo>)this.GetFields()).Count<UserFieldInfo>())
                return false;
            ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(this.m_fields[fieldId].SeedID);
            if (itemTemplate == null)
                return false;
            SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(itemTemplate.Property4), 1, 102);
            List<SqlDataProvider.Data.ItemInfo> items = new List<SqlDataProvider.Data.ItemInfo>();
            fromTemplate.Count = this.m_fields[fieldId].GainCount;
            if (!base.killCropField(fieldId))
                return false;
            if (!this.m_player.PropBag.StackItemToAnother(fromTemplate) && !this.m_player.PropBag.AddItem(fromTemplate))
                items.Add(fromTemplate);
            this.m_player.Out.SendtoGather(this.m_player.PlayerCharacter, this.m_fields[fieldId]);
            foreach (int viFarm in this.m_player.ViFarms)
            {
                GamePlayer playerById = WorldMgr.GetPlayerById(viFarm);
                if (playerById != null && playerById.Farm.Status == viFarm)
                    playerById.Out.SendtoGather(this.m_player.PlayerCharacter, this.m_fields[fieldId]);
            }
            this.m_player.OnCropPrimaryEvent();
            if (items.Count > 0)
            {
                this.m_player.SendItemsToMail(items, "Bagfull trả về thư!", "Bagfull trả về thư!", eMailType.ItemOverdue);
                this.m_player.Out.SendMailResponse(this.m_player.PlayerCharacter.ID, eMailRespose.Receiver);
            }
            return true;
        }
    }
}
