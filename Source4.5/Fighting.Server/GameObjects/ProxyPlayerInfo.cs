// Decompiled with JetBrains decompiler
// Type: Fighting.Server.GameObjects.ProxyPlayerInfo
// Assembly: Fighting.Server, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DD84ADA-AC24-47D6-83C2-9107959F6D72
// Assembly location: C:\Users\Administrador.OMINIHOST\Downloads\DDBTank4.1\Emulador\Fight\Fighting.Server.dll

using Bussiness.Managers;
using System;

namespace Fighting.Server.GameObjects
{
    public class ProxyPlayerInfo
    {
        public SqlDataProvider.Data.ItemInfo GetHealstone()
        {
            SqlDataProvider.Data.ItemInfo itemInfo = (SqlDataProvider.Data.ItemInfo)null;
            if ((uint)this.Healstone > 0U)
            {
                itemInfo = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(this.Healstone), 1, 1);
                itemInfo.Count = this.HealstoneCount;
            }
            return itemInfo;
        }

        public SqlDataProvider.Data.ItemInfo GetItemInfo()
        {
            SqlDataProvider.Data.ItemInfo itemInfo = (SqlDataProvider.Data.ItemInfo)null;
            if ((uint)this.SecondWeapon > 0U)
            {
                itemInfo = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(this.SecondWeapon), 1, 1);
                itemInfo.StrengthenLevel = this.StrengthLevel;
            }
            return itemInfo;
        }

        public int WeaponStrengthLevel { get; set; }

        public SqlDataProvider.Data.ItemInfo GetItemTemplateInfo()
        {
            SqlDataProvider.Data.ItemInfo itemInfo = (SqlDataProvider.Data.ItemInfo)null;
            if ((uint)this.TemplateId > 0U)
            {
                itemInfo = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(this.TemplateId), 1, 1);
                itemInfo.StrengthenLevel = this.WeaponStrengthLevel;
            }
            if ((uint)this.GoldTemplateId > 0U)
            {
                itemInfo.GoldEquip = ItemMgr.FindItemTemplate(this.GoldTemplateId);
                itemInfo.goldBeginTime = this.goldBeginTime;
                itemInfo.goldValidDate = this.goldValidDate;
            }
            return itemInfo;
        }

        public double AntiAddictionRate { get; set; }

        public float AuncherExperienceRate { get; set; }

        public float AuncherOfferRate { get; set; }

        public float AuncherRichesRate { get; set; }

        public double BaseAgility { get; set; }

        public double BaseAttack { get; set; }

        public double BaseBlood { get; set; }

        public double BaseDefence { get; set; }

        public bool CanUserProp { get; set; }

        public bool CanX2Exp { get; set; }

        public bool CanX3Exp { get; set; }

        public string FightFootballStyle { get; set; }

        public float GMExperienceRate { get; set; }

        public float GMOfferRate { get; set; }

        public float GMRichesRate { get; set; }

        public double GPAddPlus { get; set; }

        public int Healstone { get; set; }

        public int HealstoneCount { get; set; }

        public double OfferAddPlus { get; set; }

        public int SecondWeapon { get; set; }

        public int ServerId { get; set; }

        public int StrengthLevel { get; set; }

        public int TemplateId { get; set; }

        public int ZoneId { get; set; }

        public string ZoneName { get; set; }

        public int GoldTemplateId { get; set; }

        public DateTime goldBeginTime { get; set; }

        public int goldValidDate { get; set; }
    }
}
