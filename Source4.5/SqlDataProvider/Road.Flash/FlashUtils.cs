using SqlDataProvider.Data;
using System;
using System.Xml.Linq;

namespace Road.Flash
{
  public class FlashUtils
  {
    public static XElement CreateEdictum(EdictumInfo info) => new XElement((XName) "Item", new object[8]
    {
      (object) new XAttribute((XName) "ID", (object) info.ID),
      (object) new XAttribute((XName) "Title", string.IsNullOrEmpty(info.Title) ? (object) "" : (object) info.Title),
      (object) new XAttribute((XName) "BeginDate", (object) info.BeginDate.ToString("yyyy-MM-dd HH:mm:ss")),
      (object) new XAttribute((XName) "BeginTime", (object) info.BeginTime.ToString("yyyy-MM-dd HH:mm:ss")),
      (object) new XAttribute((XName) "EndDate", (object) info.EndDate.ToString("yyyy-MM-dd HH:mm:ss")),
      (object) new XAttribute((XName) "EndTime", (object) info.EndTime.ToString("yyyy-MM-dd HH:mm:ss")),
      (object) new XAttribute((XName) "Text", string.IsNullOrEmpty(info.Text) ? (object) "" : (object) info.Text),
      (object) new XAttribute((XName) "IsExist", (object) info.IsExist)
    });

    public static XElement CreateServerInfo(
      int id,
      string name,
      string ip,
      int port,
      int state,
      int mustLevel,
      int lowestLevel,
      int online) => new XElement((XName) "Item", new object[9]
    {
      (object) new XAttribute((XName) "ID", (object) id),
      (object) new XAttribute((XName) "Name", (object) name),
      (object) new XAttribute((XName) "IP", (object) ip),
      (object) new XAttribute((XName) "Port", (object) port),
      (object) new XAttribute((XName) "State", (object) state),
      (object) new XAttribute((XName) "MustLevel", (object) mustLevel),
      (object) new XAttribute((XName) "LowestLevel", (object) lowestLevel),
      (object) new XAttribute((XName) "Online", (object) online),
      (object) new XAttribute((XName) "Remark", (object) "")
    });


        public static XElement CreateAchievementInfo(AchievementInfo info)
        {
            return new XElement("Item", new object[]
            {
                new XAttribute("ID", info.ID),
                new XAttribute("PlaceID", info.PlaceID),
                new XAttribute("Title", info.Title),
                new XAttribute("Detail", info.Detail),
                new XAttribute("NeedMinLevel", info.NeedMinLevel),
                new XAttribute("NeedMaxLevel", info.NeedMaxLevel),
                new XAttribute("PreAchievementID", info.PreAchievementID),
                new XAttribute("IsOther", info.IsOther),
                new XAttribute("AchievementType", info.AchievementType),
                new XAttribute("CanHide", info.CanHide),
                new XAttribute("StartDate", info.StartDate),
                new XAttribute("EndDate", info.EndDate),
                new XAttribute("AchievementPoint", info.AchievementPoint),
                new XAttribute("IsActive", info.IsActive),
                new XAttribute("PicID", info.PicID),
                new XAttribute("IsShare", info.IsShare)
            });
        }

        public static XElement CreateAchievementCondiction(AchievementCondictionInfo info)
        {
            return new XElement("Item_Condiction", new object[]
            {
                new XAttribute("AchievementID", info.AchievementID),
                new XAttribute("CondictionID", info.CondictionID),
                new XAttribute("CondictionType", info.CondictionType),
                new XAttribute("Condiction_Para1", info.Condiction_Para1),
                new XAttribute("Condiction_Para2", info.Condiction_Para2)
            });
        }

        public static XElement CreateAchievementGoods(AchievementGoodsInfo info)
        {
            return new XElement("Item_Reward", new object[]
            {
                new XAttribute("AchievementID", info.AchievementID),
                new XAttribute("RewardType", info.RewardType),
                new XAttribute("RewardPara", info.RewardPara),
                new XAttribute("RewardValueId", info.RewardValueId),
                new XAttribute("RewardCount", info.RewardCount)
            });
        }


        public static XElement CreateMapInfo(MapInfo m) => new XElement((XName) "Item", new object[18]
    {
      (object) new XAttribute((XName) "ID", (object) m.ID),
      (object) new XAttribute((XName) "Name", m.Name == null ? (object) "" : (object) m.Name),
      (object) new XAttribute((XName) "Description", m.Description == null ? (object) "" : (object) m.Description),
      (object) new XAttribute((XName) "ForegroundWidth", (object) m.ForegroundWidth),
      (object) new XAttribute((XName) "ForegroundHeight", (object) m.ForegroundHeight),
      (object) new XAttribute((XName) "BackroundWidht", (object) m.BackroundWidht),
      (object) new XAttribute((XName) "BackroundHeight", (object) m.BackroundHeight),
      (object) new XAttribute((XName) "DeadWidth", (object) m.DeadWidth),
      (object) new XAttribute((XName) "DeadHeight", (object) m.DeadHeight),
      (object) new XAttribute((XName) "Weight", (object) m.Weight),
      (object) new XAttribute((XName) "DragIndex", (object) m.DragIndex),
      (object) new XAttribute((XName) "ForePic", m.ForePic == null ? (object) "" : (object) m.ForePic),
      (object) new XAttribute((XName) "BackPic", m.BackPic == null ? (object) "" : (object) m.BackPic),
      (object) new XAttribute((XName) "DeadPic", m.DeadPic == null ? (object) "" : (object) m.DeadPic),
      (object) new XAttribute((XName) "Pic", m.Pic == null ? (object) "" : (object) m.Pic),
      (object) new XAttribute((XName) "BackMusic", m.BackMusic == null ? (object) "" : (object) m.BackMusic),
      (object) new XAttribute((XName) "Remark", m.Remark == null ? (object) "" : (object) m.Remark),
      (object) new XAttribute((XName) "Type", (object) m.Type)
    });

    public static XElement CreatePveInfo(PveInfo m) => new XElement((XName) "Item", new object[12]
    {
      (object) new XAttribute((XName) "ID", (object) m.ID),
      (object) new XAttribute((XName) "Name", m.Name == null ? (object) "" : (object) m.Name),
      (object) new XAttribute((XName) "Type", (object) m.Type),
      (object) new XAttribute((XName) "LevelLimits", (object) m.LevelLimits),
      (object) new XAttribute((XName) "SimpleTemplateIds", m.SimpleTemplateIds == null ? (object) "" : (object) m.SimpleTemplateIds),
      (object) new XAttribute((XName) "NormalTemplateIds", m.NormalTemplateIds == null ? (object) "" : (object) m.NormalTemplateIds),
      (object) new XAttribute((XName) "HardTemplateIds", m.HardTemplateIds == null ? (object) "" : (object) m.HardTemplateIds),
      (object) new XAttribute((XName) "TerrorTemplateIds", m.TerrorTemplateIds == null ? (object) "" : (object) m.TerrorTemplateIds),
      (object) new XAttribute((XName) "Pic", m.Pic == null ? (object) "" : (object) m.Pic),
      (object) new XAttribute((XName) "Description", m.Description == null ? (object) "" : (object) m.Description),
      (object) new XAttribute((XName) "Ordering", (object) m.Ordering),
      (object) new XAttribute((XName) "AdviceTips", m.AdviceTips == null ? (object) "" : (object) m.AdviceTips)
    });

    public static XElement CreateStrengthenInfo(StrengthenInfo info) => new XElement((XName) "Item", new object[2]
    {
      (object) new XAttribute((XName) "StrengthenLevel", (object) info.StrengthenLevel),
      (object) new XAttribute((XName) "Rock", (object) info.Rock)
    });

    public static XElement CreateItemInfo(ItemTemplateInfo info) => new XElement((XName) "Item", new object[42]
    {
      (object) new XAttribute((XName) "AddTime", (object) info.AddTime),
      (object) new XAttribute((XName) "Agility", (object) info.Agility),
      (object) new XAttribute((XName) "Attack", (object) info.Attack),
      (object) new XAttribute((XName) "CanCompose", (object) info.CanCompose),
      (object) new XAttribute((XName) "CanDelete", (object) info.CanDelete),
      (object) new XAttribute((XName) "CanDrop", (object) info.CanDrop),
      (object) new XAttribute((XName) "CanEquip", (object) info.CanEquip),
      (object) new XAttribute((XName) "CanStrengthen", (object) info.CanStrengthen),
      (object) new XAttribute((XName) "CanUse", (object) info.CanUse),
      (object) new XAttribute((XName) "CategoryID", (object) info.CategoryID),
      (object) new XAttribute((XName) "Colors", (object) info.Colors),
      (object) new XAttribute((XName) "Defence", (object) info.Defence),
      (object) new XAttribute((XName) "Description", info.Description == null ? (object) "" : (object) info.Description),
      (object) new XAttribute((XName) "Level", (object) info.Level),
      (object) new XAttribute((XName) "Luck", (object) info.Luck),
      (object) new XAttribute((XName) "MaxCount", (object) info.MaxCount),
      (object) new XAttribute((XName) "Name", info.Name == null ? (object) "" : (object) info.Name),
      (object) new XAttribute((XName) "NeedLevel", (object) info.NeedLevel),
      (object) new XAttribute((XName) "NeedSex", (object) info.NeedSex),
      (object) new XAttribute((XName) "Pic", info.Pic == null ? (object) "" : (object) info.Pic),
      (object) new XAttribute((XName) "Data", info.Data == null ? (object) "" : (object) info.Data),
      (object) new XAttribute((XName) "Property1", (object) info.Property1),
      (object) new XAttribute((XName) "Property2", (object) info.Property2),
      (object) new XAttribute((XName) "Property3", (object) info.Property3),
      (object) new XAttribute((XName) "Property4", (object) info.Property4),
      (object) new XAttribute((XName) "Property5", (object) info.Property5),
      (object) new XAttribute((XName) "Property6", (object) info.Property6),
      (object) new XAttribute((XName) "Property7", (object) info.Property7),
      (object) new XAttribute((XName) "Property8", (object) info.Property8),
      (object) new XAttribute((XName) "Quality", (object) info.Quality),
      (object) new XAttribute((XName) "Script", info.Script == null ? (object) "" : (object) info.Script),
      (object) new XAttribute((XName) "BindType", (object) info.BindType),
      (object) new XAttribute((XName) "FusionType", (object) info.FusionType),
      (object) new XAttribute((XName) "FusionRate", (object) info.FusionRate),
      (object) new XAttribute((XName) "FusionNeedRate", (object) info.FusionNeedRate),
      (object) new XAttribute((XName) "TemplateID", (object) info.TemplateID),
      (object) new XAttribute((XName) "RefineryLevel", (object) info.RefineryLevel),
      (object) new XAttribute((XName) "Hole", (object) info.Hole),
      (object) new XAttribute((XName) "ReclaimValue", (object) info.ReclaimValue),
      (object) new XAttribute((XName) "ReclaimType", (object) info.ReclaimType),
      (object) new XAttribute((XName) "CanRecycle", (object) info.CanRecycle),
      (object) new XAttribute((XName) "SuitId", (object) info.SuitId)
    });

    public static XElement CreateGoodsInfo(SqlDataProvider.Data.ItemInfo info) => new XElement((XName) "Item", new object[24]
    {
      (object) new XAttribute((XName) "AgilityCompose", (object) info.AgilityCompose),
      (object) new XAttribute((XName) "AttackCompose", (object) info.AttackCompose),
      (object) new XAttribute((XName) "BeginDate", (object) info.BeginDate.ToString("yyyy-MM-dd HH:mm:ss")),
      (object) new XAttribute((XName) "Color", info.Color == null ? (object) "" : (object) info.Color),
      (object) new XAttribute((XName) "Skin", info.Skin == null ? (object) "" : (object) info.Skin),
      (object) new XAttribute((XName) "Count", (object) info.Count),
      (object) new XAttribute((XName) "DefendCompose", (object) info.DefendCompose),
      (object) new XAttribute((XName) "IsBinds", (object) info.IsBinds),
      (object) new XAttribute((XName) "IsUsed", (object) info.IsUsed),
      (object) new XAttribute((XName) "IsJudge", (object) info.IsJudge),
      (object) new XAttribute((XName) "ItemID", (object) info.ItemID),
      (object) new XAttribute((XName) "LuckCompose", (object) info.LuckCompose),
      (object) new XAttribute((XName) "Place", (object) info.Place),
      (object) new XAttribute((XName) "StrengthenLevel", (object) info.StrengthenLevel),
      (object) new XAttribute((XName) "TemplateID", (object) info.TemplateID),
      (object) new XAttribute((XName) "UserID", (object) info.UserID),
      (object) new XAttribute((XName) "BagType", (object) info.BagType),
      (object) new XAttribute((XName) "ValidDate", (object) info.ValidDate),
      (object) new XAttribute((XName) "Hole1", (object) info.Hole1),
      (object) new XAttribute((XName) "Hole2", (object) info.Hole2),
      (object) new XAttribute((XName) "Hole3", (object) info.Hole3),
      (object) new XAttribute((XName) "Hole4", (object) info.Hole4),
      (object) new XAttribute((XName) "Hole5", (object) info.Hole5),
      (object) new XAttribute((XName) "Hole6", (object) info.Hole6)
    });

    public static XElement CreateShopInfo(ShopItemInfo shop) => new XElement((XName) "Item", new object[35]
    {
      (object) new XAttribute((XName) "ID", (object) shop.ID),
      (object) new XAttribute((XName) "ShopID", (object) shop.ShopID),
      (object) new XAttribute((XName) "GroupID", (object) shop.GroupID),
      (object) new XAttribute((XName) "TemplateID", (object) shop.TemplateID),
      (object) new XAttribute((XName) "BuyType", (object) shop.BuyType),
      (object) new XAttribute((XName) "IsContinue", (object) shop.IsContinue),
      (object) new XAttribute((XName) "IsBind", (object) shop.IsBind),
      (object) new XAttribute((XName) "IsVouch", (object) shop.IsVouch),
      (object) new XAttribute((XName) "Label", (object) shop.Label),
      (object) new XAttribute((XName) "Beat", (object) shop.Beat),
      (object) new XAttribute((XName) "AUnit", (object) shop.AUnit),
      (object) new XAttribute((XName) "APrice1", (object) shop.APrice1),
      (object) new XAttribute((XName) "AValue1", (object) shop.AValue1),
      (object) new XAttribute((XName) "APrice2", (object) shop.APrice2),
      (object) new XAttribute((XName) "AValue2", (object) shop.AValue2),
      (object) new XAttribute((XName) "APrice3", (object) shop.APrice3),
      (object) new XAttribute((XName) "AValue3", (object) shop.AValue3),
      (object) new XAttribute((XName) "BUnit", (object) shop.BUnit),
      (object) new XAttribute((XName) "BPrice1", (object) shop.BPrice1),
      (object) new XAttribute((XName) "BValue1", (object) shop.BValue1),
      (object) new XAttribute((XName) "BPrice2", (object) shop.BPrice2),
      (object) new XAttribute((XName) "BValue2", (object) shop.BValue2),
      (object) new XAttribute((XName) "BPrice3", (object) shop.BPrice3),
      (object) new XAttribute((XName) "BValue3", (object) shop.BValue3),
      (object) new XAttribute((XName) "CUnit", (object) shop.CUnit),
      (object) new XAttribute((XName) "CPrice1", (object) shop.CPrice1),
      (object) new XAttribute((XName) "CValue1", (object) shop.CValue1),
      (object) new XAttribute((XName) "CPrice2", (object) shop.CPrice2),
      (object) new XAttribute((XName) "CValue2", (object) shop.CValue2),
      (object) new XAttribute((XName) "CPrice3", (object) shop.CPrice3),
      (object) new XAttribute((XName) "CValue3", (object) shop.CValue3),
      (object) new XAttribute((XName) "IsCheap", (object) shop.IsCheap),
      (object) new XAttribute((XName) "LimitCount", (object) shop.LimitCount),
      (object) new XAttribute((XName) "StartDate", (object) shop.StartDate),
      (object) new XAttribute((XName) "EndDate", (object) shop.EndDate)
    });

    public static XElement CreateShopShowInfo(ShopGoodsShowListInfo shop) => new XElement((XName) "Item", new object[2]
    {
      (object) new XAttribute((XName) "Type", (object) shop.Type),
      (object) new XAttribute((XName) "ShopId", (object) shop.ShopId)
    });

    public static XElement CreateItemBoxInfo(ItemBoxInfo box) => new XElement((XName) "Item", new object[11]
    {
      (object) new XAttribute((XName) "ID", (object) box.ID),
      (object) new XAttribute((XName) "TemplateId", (object) box.TemplateId),
      (object) new XAttribute((XName) "StrengthenLevel", (object) box.StrengthenLevel),
      (object) new XAttribute((XName) "IsBind", (object) box.IsBind),
      (object) new XAttribute((XName) "ItemCount", (object) box.ItemCount),
      (object) new XAttribute((XName) "LuckCompose", (object) box.LuckCompose),
      (object) new XAttribute((XName) "DefendCompose", (object) box.DefendCompose),
      (object) new XAttribute((XName) "AttackCompose", (object) box.AttackCompose),
      (object) new XAttribute((XName) "AgilityCompose", (object) box.AgilityCompose),
      (object) new XAttribute((XName) "ItemValid", (object) box.ItemValid),
      (object) new XAttribute((XName) "IsTips", (object) box.IsTips)
    });

    public static XElement CreateUserBoxInfo(UserBoxInfo box) => new XElement((XName) "Item", new object[5]
    {
      (object) new XAttribute((XName) "ID", (object) box.ID),
      (object) new XAttribute((XName) "Type", (object) box.Type),
      (object) new XAttribute((XName) "Level", (object) box.Level),
      (object) new XAttribute((XName) "Condition", (object) box.Condition),
      (object) new XAttribute((XName) "TemplateID", (object) box.TemplateID)
    });

    public static XElement CreateBallInfo(BallInfo b) => new XElement((XName) "Item", new object[19]
    {
      (object) new XAttribute((XName) "ID", (object) b.ID),
      (object) new XAttribute((XName) "Power", (object) b.Power),
      (object) new XAttribute((XName) "Radii", (object) b.Radii),
      (object) new XAttribute((XName) "FlyingPartical", b.FlyingPartical == null ? (object) "" : (object) b.FlyingPartical),
      (object) new XAttribute((XName) "BombPartical", b.BombPartical == null ? (object) "" : (object) b.BombPartical),
      (object) new XAttribute((XName) "Crater", b.Crater == null ? (object) "" : (object) b.Crater),
      (object) new XAttribute((XName) "AttackResponse", (object) b.AttackResponse),
      (object) new XAttribute((XName) "IsSpin", (object) b.IsSpin),
      (object) new XAttribute((XName) "SpinV", (object) b.SpinV),
      (object) new XAttribute((XName) "SpinVA", (object) b.SpinVA),
      (object) new XAttribute((XName) "Amount", (object) b.Amount),
      (object) new XAttribute((XName) "Wind", (object) b.Wind),
      (object) new XAttribute((XName) "DragIndex", (object) b.DragIndex),
      (object) new XAttribute((XName) "Weight", (object) b.Weight),
      (object) new XAttribute((XName) "Shake", (object) b.Shake),
      (object) new XAttribute((XName) "ShootSound", b.ShootSound == null ? (object) "" : (object) b.ShootSound),
      (object) new XAttribute((XName) "BombSound", b.BombSound == null ? (object) "" : (object) b.BombSound),
      (object) new XAttribute((XName) "ActionType", (object) b.ActionType),
      (object) new XAttribute((XName) "Mass", (object) b.Mass)
    });

    public static XElement CreateBallConfigInfo(BallConfigInfo b) => new XElement((XName) "Item", new object[5]
    {
      (object) new XAttribute((XName) "TemplateID", (object) b.TemplateID),
      (object) new XAttribute((XName) "Common", (object) b.Common),
      (object) new XAttribute((XName) "CommonAddWound", (object) b.CommonAddWound),
      (object) new XAttribute((XName) "CommonMultiBall", (object) b.CommonMultiBall),
      (object) new XAttribute((XName) "Special", (object) b.Special)
    });

    public static XElement CreateRuneTemplateInfo(RuneTemplateInfo b) => new XElement((XName) "Rune", new object[17]
    {
      (object) new XAttribute((XName) "TemplateID", (object) b.TemplateID),
      (object) new XAttribute((XName) "NextTemplateID", (object) b.NextTemplateID),
      (object) new XAttribute((XName) "Name", (object) b.Name),
      (object) new XAttribute((XName) "BaseLevel", (object) b.BaseLevel),
      (object) new XAttribute((XName) "MaxLevel", (object) b.MaxLevel),
      (object) new XAttribute((XName) "Type1", (object) b.Type1),
      (object) new XAttribute((XName) "Attribute1", (object) b.Attribute1),
      (object) new XAttribute((XName) "Turn1", (object) b.Turn1),
      (object) new XAttribute((XName) "Rate1", (object) b.Rate1),
      (object) new XAttribute((XName) "Type2", (object) b.Type2),
      (object) new XAttribute((XName) "Attribute2", (object) b.Attribute2),
      (object) new XAttribute((XName) "Turn2", (object) b.Turn2),
      (object) new XAttribute((XName) "Rate2", (object) b.Rate2),
      (object) new XAttribute((XName) "Type3", (object) b.Type3),
      (object) new XAttribute((XName) "Attribute3", (object) b.Attribute3),
      (object) new XAttribute((XName) "Turn3", (object) b.Turn3),
      (object) new XAttribute((XName) "Rate3", (object) b.Rate3)
    });

    public static XElement CreateCategoryInfo(CategoryInfo info) => new XElement((XName) "Item", new object[4]
    {
      (object) new XAttribute((XName) "ID", (object) info.ID),
      (object) new XAttribute((XName) "Name", info.Name == null ? (object) "" : (object) info.Name),
      (object) new XAttribute((XName) "Place", (object) info.Place),
      (object) new XAttribute((XName) "Remark", info.Remark == null ? (object) "" : (object) info.Remark)
    });

        public static XElement CreateUserLoginList(PlayerInfo info)
        {
            return new XElement("Item", new object[]
            {
                new XAttribute("ID", info.ID),
                new XAttribute("UserName", (info.UserName == null) ? "" : info.UserName),
                new XAttribute("NickName", (info.NickName == null) ? "" : info.NickName),
                new XAttribute("Grade", info.Grade),
                new XAttribute("Repute", info.Repute),
                new XAttribute("Sex", info.Sex),
                new XAttribute("WinCount", info.Win),
                new XAttribute("TotalCount", info.Total),
                new XAttribute("ConsortiaName", info.ConsortiaName),
                new XAttribute("Rename", info.Rename),
                new XAttribute("IsVIP", info.typeVIP > 0),
                new XAttribute("VIPLevel", info.VIPLevel),
                new XAttribute("ConsortiaRename", info.ConsortiaRename ? (info.NickName == info.ChairmanName) : info.ConsortiaRename),
                new XAttribute("EscapeCount", info.Escape),
                new XAttribute("IsFirst", info.IsFirst),
                new XAttribute("LastDate", DateTime.Now.AddDays(-1.0))
            });
        }

        public static XElement CreateQuestInfo(QuestInfo info) => new XElement((XName) "Item", new object[32]
    {
      (object) new XAttribute((XName) "ID", (object) info.ID),
      (object) new XAttribute((XName) "QuestID", (object) info.QuestID),
      (object) new XAttribute((XName) "Title", (object) info.Title),
      (object) new XAttribute((XName) "Detail", (object) info.Detail),
      (object) new XAttribute((XName) "Objective", (object) info.Objective),
      (object) new XAttribute((XName) "NeedMinLevel", (object) info.NeedMinLevel),
      (object) new XAttribute((XName) "NeedMaxLevel", (object) info.NeedMaxLevel),
      (object) new XAttribute((XName) "PreQuestID", (object) info.PreQuestID),
      (object) new XAttribute((XName) "NextQuestID", (object) info.NextQuestID),
      (object) new XAttribute((XName) "IsOther", (object) info.IsOther),
      (object) new XAttribute((XName) "CanRepeat", (object) info.CanRepeat),
      (object) new XAttribute((XName) "RepeatInterval", (object) info.RepeatInterval),
      (object) new XAttribute((XName) "RepeatMax", (object) info.RepeatMax),
      (object) new XAttribute((XName) "RewardGP", (object) info.RewardGP),
      (object) new XAttribute((XName) "RewardGold", (object) info.RewardGold),
      (object) new XAttribute((XName) "RewardGiftToken", (object) info.RewardGiftToken),
      (object) new XAttribute((XName) "RewardOffer", (object) info.RewardOffer),
      (object) new XAttribute((XName) "RewardRiches", (object) info.RewardRiches),
      (object) new XAttribute((XName) "RewardBuffID", (object) info.RewardBuffID),
      (object) new XAttribute((XName) "RewardBuffDate", (object) info.RewardBuffDate),
      (object) new XAttribute((XName) "RewardMoney", (object) info.RewardMoney),
      (object) new XAttribute((XName) "Rands", (object) info.Rands),
      (object) new XAttribute((XName) "RandDouble", (object) info.RandDouble),
      (object) new XAttribute((XName) "TimeMode", (object) info.TimeMode),
      (object) new XAttribute((XName) "StartDate", (object) info.StartDate),
      (object) new XAttribute((XName) "EndDate", (object) info.EndDate),
      (object) new XAttribute((XName) "MapID", (object) info.MapID),
      (object) new XAttribute((XName) "AutoEquip", (object) info.AutoEquip),
      (object) new XAttribute((XName) "RewardMedal", (object) info.RewardMedal),
      (object) new XAttribute((XName) "Rank", (object) info.Rank),
      (object) new XAttribute((XName) "StarLev", (object) info.StarLev),
      (object) new XAttribute((XName) "NotMustCount", (object) info.NotMustCount)
    });

    public static XElement CreateQuestCondiction(QuestConditionInfo info) => new XElement((XName) "Item_Condiction", new object[7]
    {
      (object) new XAttribute((XName) "QuestID", (object) info.QuestID),
      (object) new XAttribute((XName) "CondictionID", (object) info.CondictionID),
      (object) new XAttribute((XName) "CondictionTitle", (object) info.CondictionTitle),
      (object) new XAttribute((XName) "CondictionType", (object) info.CondictionType),
      (object) new XAttribute((XName) "Para1", (object) info.Para1),
      (object) new XAttribute((XName) "Para2", (object) info.Para2),
      (object) new XAttribute((XName) "isOpitional", (object) info.isOpitional)
    });

    public static XElement CreateQuestGoods(QuestAwardInfo info) => new XElement((XName) "Item_Good", new object[12]
    {
      (object) new XAttribute((XName) "QuestID", (object) info.QuestID),
      (object) new XAttribute((XName) "RewardItemID", (object) info.RewardItemID),
      (object) new XAttribute((XName) "IsSelect", (object) info.IsSelect),
      (object) new XAttribute((XName) "RewardItemValid", (object) info.RewardItemValid),
      (object) new XAttribute((XName) "RewardItemCount", (object) info.RewardItemCount),
      (object) new XAttribute((XName) "StrengthenLevel", (object) info.StrengthenLevel),
      (object) new XAttribute((XName) "AttackCompose", (object) info.AttackCompose),
      (object) new XAttribute((XName) "DefendCompose", (object) info.DefendCompose),
      (object) new XAttribute((XName) "AgilityCompose", (object) info.AgilityCompose),
      (object) new XAttribute((XName) "LuckCompose", (object) info.LuckCompose),
      (object) new XAttribute((XName) "IsCount", (object) info.IsCount),
      (object) new XAttribute((XName) "IsBind", (object) info.IsBind)
    });

    public static XElement CreateQuestDataInfo(QuestDataInfo info) => new XElement((XName) "Item", new object[9]
    {
      (object) new XAttribute((XName) "CompletedDate", (object) info.CompletedDate),
      (object) new XAttribute((XName) "IsComplete", (object) info.IsComplete),
      (object) new XAttribute((XName) "Condition1", (object) info.Condition1),
      (object) new XAttribute((XName) "Condition2", (object) info.Condition2),
      (object) new XAttribute((XName) "Condition3", (object) info.Condition3),
      (object) new XAttribute((XName) "Condition4", (object) info.Condition4),
      (object) new XAttribute((XName) "QuestID", (object) info.QuestID),
      (object) new XAttribute((XName) "UserID", (object) info.UserID),
      (object) new XAttribute((XName) "RepeatFinish", (object) info.RepeatFinish)
    });

        public static XElement CreateLuckstarActivityRank(LuckstarActivityRankInfo info)
        {
            return new XElement("myRank", new object[] { new XAttribute("rank", info.rank), new XAttribute("useStarNum", info.useStarNum), new XAttribute("nickName", info.nickName) }); ;
        }

        public static XElement CreateMapServer(ServerMapInfo info) => new XElement((XName) "Item", new object[3]
    {
      (object) new XAttribute((XName) "ServerID", (object) info.ServerID),
      (object) new XAttribute((XName) "OpenMap", (object) info.OpenMap),
      (object) new XAttribute((XName) "IsSpecial", (object) info.IsSpecial)
    });

    public static XElement CreateActiveInfo(ActiveInfo info)
    {
      XName name1 = (XName) "Item";
      object[] objArray1 = new object[17];
      objArray1[0] = (object) new XAttribute((XName) "ActiveID", (object) info.ActiveID);
      objArray1[1] = (object) new XAttribute((XName) "Description", info.Description == null ? (object) "" : (object) info.Description);
      objArray1[2] = (object) new XAttribute((XName) "Content", info.Content == null ? (object) "" : (object) info.Content);
      objArray1[3] = (object) new XAttribute((XName) "AwardContent", info.AwardContent == null ? (object) "" : (object) info.AwardContent);
      objArray1[4] = (object) new XAttribute((XName) "HasKey", (object) info.HasKey);
      int index1 = 5;
      XName name2 = (XName) "EndDate";
      DateTime startDate;
      string str1;
      if (info.EndDate.HasValue)
      {
        startDate = info.EndDate.Value;
        str1 = startDate.ToString("yyyy-MM-dd HH:mm:ss");
      }
      else
        str1 = "";
      string str2 = str1;
      XAttribute xattribute1 = new XAttribute(name2, (object) str2);
      objArray1[index1] = (object) xattribute1;
      objArray1[6] = (object) new XAttribute((XName) "IsOnly", (object) info.IsOnly);
      int index2 = 7;
      XName name3 = (XName) "StartDate";
      startDate = info.StartDate;
      string str3;
      if (!string.IsNullOrEmpty(startDate.ToString()))
      {
        startDate = info.StartDate;
        str3 = startDate.ToString("yyyy-MM-dd HH:mm:ss");
      }
      else
        str3 = "";
      string str4 = str3;
      XAttribute xattribute2 = new XAttribute(name3, (object) str4);
      objArray1[index2] = (object) xattribute2;
      objArray1[8] = (object) new XAttribute((XName) "Title", info.Title == null ? (object) "" : (object) info.Title);
      objArray1[9] = (object) new XAttribute((XName) "Type", (object) info.Type);
      objArray1[10] = (object) new XAttribute((XName) "ActiveType", (object) "0");
      objArray1[11] = (object) new XAttribute((XName) "IsAdvance", (object) false);
      objArray1[12] = (object) new XAttribute((XName) "GoodsExchangeTypes", (object) "");
      objArray1[13] = (object) new XAttribute((XName) "GoodsExchangeNum", (object) "");
      objArray1[14] = (object) new XAttribute((XName) "limitType", (object) "");
      objArray1[15] = (object) new XAttribute((XName) "limitValue", (object) "");
      objArray1[16] = (object) new XAttribute((XName) "ActionTimeContent", info.ActionTimeContent == null ? (object) "" : (object) info.ActionTimeContent);
      object[] objArray2 = objArray1;
      return new XElement(name1, objArray2);
    }

    public static XElement CreateAuctionInfo(AuctionInfo info) => new XElement((XName) "Item", new object[12]
    {
      (object) new XAttribute((XName) "AuctionID", (object) info.AuctionID),
      (object) new XAttribute((XName) "AuctioneerID", (object) info.AuctioneerID),
      (object) new XAttribute((XName) "AuctioneerName", info.AuctioneerName == null ? (object) "" : (object) info.AuctioneerName),
      (object) new XAttribute((XName) "BeginDate", (object) info.BeginDate.ToString("yyyy-MM-dd HH:mm:ss")),
      (object) new XAttribute((XName) "BuyerID", (object) info.BuyerID),
      (object) new XAttribute((XName) "BuyerName", info.BuyerName == null ? (object) "" : (object) info.BuyerName),
      (object) new XAttribute((XName) "ItemID", (object) info.ItemID),
      (object) new XAttribute((XName) "Mouthful", (object) info.Mouthful),
      (object) new XAttribute((XName) "PayType", (object) info.PayType),
      (object) new XAttribute((XName) "Price", (object) info.Price),
      (object) new XAttribute((XName) "Rise", (object) info.Rise),
      (object) new XAttribute((XName) "ValidDate", (object) info.ValidDate)
    });

    public static XElement CreateConsortiaInfo(ConsortiaInfo info) => new XElement((XName) "Item", new object[41]
    {
      (object) new XAttribute((XName) "ConsortiaID", (object) info.ConsortiaID),
      (object) new XAttribute((XName) "BuildDate", (object) info.BuildDate.ToString("yyyy-MM-dd HH:mm:ss")),
      (object) new XAttribute((XName) "CelebCount", (object) info.CelebCount),
      (object) new XAttribute((XName) "ChairmanID", (object) info.ChairmanID),
      (object) new XAttribute((XName) "ChairmanName", info.ChairmanName == null ? (object) "" : (object) info.ChairmanName),
      (object) new XAttribute((XName) "ChairmanTypeVIP", (object) 0),
      (object) new XAttribute((XName) "ChairmanVIPLevel", (object) 0),
      (object) new XAttribute((XName) "ConsortiaName", info.ConsortiaName == null ? (object) "" : (object) info.ConsortiaName),
      (object) new XAttribute((XName) "CreatorID", (object) info.CreatorID),
      (object) new XAttribute((XName) "CreatorName", info.CreatorName == null ? (object) "" : (object) info.CreatorName),
      (object) new XAttribute((XName) "Description", info.Description == null ? (object) "" : (object) info.Description),
      (object) new XAttribute((XName) "Honor", (object) info.Honor),
      (object) new XAttribute((XName) "IP", (object) info.IP),
      (object) new XAttribute((XName) "Level", (object) info.Level),
      (object) new XAttribute((XName) "MaxCount", (object) info.MaxCount),
      (object) new XAttribute((XName) "Placard", info.Placard == null ? (object) "" : (object) info.Placard),
      (object) new XAttribute((XName) "Repute", (object) info.Repute),
      (object) new XAttribute((XName) "Count", (object) info.Count),
      (object) new XAttribute((XName) "Riches", (object) info.Riches),
      (object) new XAttribute((XName) "FightPower", (object) info.FightPower),
      (object) new XAttribute((XName) "DeductDate", (object) info.DeductDate.ToString("yyyy-MM-dd HH:mm:ss")),
      (object) new XAttribute((XName) "AddDayHonor", (object) info.AddDayHonor),
      (object) new XAttribute((XName) "AddDayRiches", (object) info.AddDayRiches),
      (object) new XAttribute((XName) "AddWeekHonor", (object) info.AddWeekHonor),
      (object) new XAttribute((XName) "AddWeekRiches", (object) info.AddWeekRiches),
      (object) new XAttribute((XName) "LastDayRiches", (object) info.LastDayRiches),
      (object) new XAttribute((XName) "OpenApply", (object) info.OpenApply),
      (object) new XAttribute((XName) "StoreLevel", (object) info.StoreLevel),
      (object) new XAttribute((XName) "SmithLevel", (object) info.SmithLevel),
      (object) new XAttribute((XName) "ShopLevel", (object) info.ShopLevel),
      (object) new XAttribute((XName) "BufferLevel", (object) info.SkillLevel),
      (object) new XAttribute((XName) "ConsortiaGiftGp", (object) 0),
      (object) new XAttribute((XName) "ConsortiaAddDayGiftGp", (object) 0),
      (object) new XAttribute((XName) "ConsortiaAddWeekGiftGp", (object) 0),
      (object) new XAttribute((XName) "Port", (object) info.Port),
      (object) new XAttribute((XName) "IsVoting", (object) false),
      (object) new XAttribute((XName) "VoteRemainDay", (object) 3),
      (object) new XAttribute((XName) "CharmGP", (object) 0),
      (object) new XAttribute((XName) "BadgeBuyTime", (object) info.BadgeBuyTime),
      (object) new XAttribute((XName) "BadgeID", (object) info.BadgeID),
      (object) new XAttribute((XName) "ValidDate", (object) info.ValidDate)
    });

    public static XElement CreateConsortiaApplyUserInfo(ConsortiaApplyUserInfo info) => new XElement((XName) "Item", new object[11]
    {
      (object) new XAttribute((XName) "ID", (object) info.ID),
      (object) new XAttribute((XName) "ApplyDate", (object) info.ApplyDate.ToString("yyyy-MM-dd HH:mm:ss")),
      (object) new XAttribute((XName) "ConsortiaID", (object) info.ConsortiaID),
      (object) new XAttribute((XName) "ConsortiaName", info.ConsortiaName == null ? (object) "" : (object) info.ConsortiaName),
      (object) new XAttribute((XName) "Remark", (object) info.Remark),
      (object) new XAttribute((XName) "UserID", (object) info.UserID),
      (object) new XAttribute((XName) "UserName", info.UserName == null ? (object) "" : (object) info.UserName),
      (object) new XAttribute((XName) "UserLevel", (object) info.UserLevel),
      (object) new XAttribute((XName) "Win", (object) info.Win),
      (object) new XAttribute((XName) "Total", (object) info.Total),
      (object) new XAttribute((XName) "Repute", (object) info.Repute)
    });

    public static XElement CreateConsortiaInviteUserInfo(ConsortiaInviteUserInfo info) => new XElement((XName) "Item", new object[14]
    {
      (object) new XAttribute((XName) "ID", (object) info.ID),
      (object) new XAttribute((XName) "CelebCount", (object) info.CelebCount),
      (object) new XAttribute((XName) "ChairmanName", info.ChairmanName == null ? (object) "" : (object) info.ChairmanName),
      (object) new XAttribute((XName) "ConsortiaID", (object) info.ConsortiaID),
      (object) new XAttribute((XName) "ConsortiaName", info.ConsortiaName == null ? (object) "" : (object) info.ConsortiaName),
      (object) new XAttribute((XName) "Count", (object) info.Count),
      (object) new XAttribute((XName) "Honor", (object) info.Honor),
      (object) new XAttribute((XName) "InviteDate", (object) info.InviteDate),
      (object) new XAttribute((XName) "InviteID", (object) info.InviteID),
      (object) new XAttribute((XName) "InviteName", info.InviteName == null ? (object) "" : (object) info.InviteName),
      (object) new XAttribute((XName) "Remark", info.Remark == null ? (object) "" : (object) info.Remark),
      (object) new XAttribute((XName) "Repute", (object) info.Repute),
      (object) new XAttribute((XName) "UserID", (object) info.UserID),
      (object) new XAttribute((XName) "UserName", info.UserName == null ? (object) "" : (object) info.UserName)
    });

    public static XElement CreateConsortiaUserInfo(ConsortiaUserInfo info) => new XElement((XName) "Item", new object[48]
    {
      (object) new XAttribute((XName) "ID", (object) info.ID),
      (object) new XAttribute((XName) "ConsortiaID", (object) info.ConsortiaID),
      (object) new XAttribute((XName) "DutyID", (object) info.DutyID),
      (object) new XAttribute((XName) "DutyName", info.DutyName == null ? (object) "" : (object) info.DutyName),
      (object) new XAttribute((XName) "GP", (object) info.GP),
      (object) new XAttribute((XName) "Level", (object) info.Level),
      (object) new XAttribute((XName) "Grade", (object) info.Grade),
      (object) new XAttribute((XName) "Right", (object) info.Right),
      (object) new XAttribute((XName) "DutyLevel", (object) info.Level),
      (object) new XAttribute((XName) "Offer", (object) info.Offer),
      (object) new XAttribute((XName) "RatifierID", (object) info.RatifierID),
      (object) new XAttribute((XName) "RatifierName", info.RatifierName == null ? (object) "" : (object) info.RatifierName),
      (object) new XAttribute((XName) "Remark", info.Remark == null ? (object) "" : (object) info.Remark),
      (object) new XAttribute((XName) "Repute", (object) info.Repute),
      (object) new XAttribute((XName) "State", (object) (info.State == 1 ? 1 : 0)),
      (object) new XAttribute((XName) "UserID", (object) info.UserID),
      (object) new XAttribute((XName) "Hide", (object) info.Hide),
      (object) new XAttribute((XName) "Colors", info.Colors == null ? (object) "" : (object) info.Colors),
      (object) new XAttribute((XName) "Skin", info.Skin == null ? (object) "" : (object) info.Skin),
      (object) new XAttribute((XName) "Style", (object) info.Style),
      (object) new XAttribute((XName) "LastDate", (object) info.LastDate.ToString("yyyy-MM-dd HH:mm:ss")),
      (object) new XAttribute((XName) "Sex", (object) info.Sex),
      (object) new XAttribute((XName) "IsBanChat", (object) info.IsBanChat),
      (object) new XAttribute((XName) "WinCount", (object) info.Win),
      (object) new XAttribute((XName) "TotalCount", (object) info.Total),
      (object) new XAttribute((XName) "EscapeCount", (object) info.Escape),
      (object) new XAttribute((XName) "RichesOffer", (object) info.RichesOffer),
      (object) new XAttribute((XName) "RichesRob", (object) info.RichesRob),
      (object) new XAttribute((XName) "Nimbus", (object) info.Nimbus),
      (object) new XAttribute((XName) "LoginName", info.LoginName == null ? (object) "" : (object) info.LoginName),
      (object) new XAttribute((XName) "UserName", info.UserName == null ? (object) "" : (object) info.UserName),
      (object) new XAttribute((XName) "FightPower", (object) info.FightPower),
      (object) new XAttribute((XName) "Rank", (object) info.honor),
      (object) new XAttribute((XName) "AchievementPoint", (object) info.AchievementPoint),
      (object) new XAttribute((XName) "IsDiplomatism", (object) true),
      (object) new XAttribute((XName) "IsDownGrade", (object) true),
      (object) new XAttribute((XName) "IsEditorPlacard", (object) true),
      (object) new XAttribute((XName) "IsEditorDescription", (object) true),
      (object) new XAttribute((XName) "IsExpel", (object) true),
      (object) new XAttribute((XName) "IsEditorUser", (object) true),
      (object) new XAttribute((XName) "IsInvite", (object) false),
      (object) new XAttribute((XName) "IsManageDuty", (object) true),
      (object) new XAttribute((XName) "IsUpGrade", (object) false),
      (object) new XAttribute((XName) "typeVIP", (object) info.typeVIP),
      (object) new XAttribute((XName) "VIPLevel", (object) info.VIPLevel),
      (object) new XAttribute((XName) "IsRatify", (object) true),
      (object) new XAttribute((XName) "IsChat", (object) true),
      (object) new XAttribute((XName) "TotalRichesOffer", (object) info.UseOffer)
    });

    public static XElement CreateConsortiaIMInfo(ConsortiaUserInfo info) => new XElement((XName) "Item", new object[20]
    {
      (object) new XAttribute((XName) "ID", (object) info.ID),
      (object) new XAttribute((XName) "ConsortiaID", (object) info.ConsortiaID),
      (object) new XAttribute((XName) "DutyID", (object) info.DutyID),
      (object) new XAttribute((XName) "DutyName", info.DutyName == null ? (object) "" : (object) info.DutyName),
      (object) new XAttribute((XName) "GP", (object) info.GP),
      (object) new XAttribute((XName) "Grade", (object) info.Grade),
      (object) new XAttribute((XName) "Level", (object) info.Level),
      (object) new XAttribute((XName) "Offer", (object) info.Offer),
      (object) new XAttribute((XName) "Remark", info.Remark == null ? (object) "" : (object) info.Remark),
      (object) new XAttribute((XName) "Repute", (object) info.Repute),
      (object) new XAttribute((XName) "State", (object) (info.State == 1 ? 1 : 0)),
      (object) new XAttribute((XName) "UserID", (object) info.UserID),
      (object) new XAttribute((XName) "Hide", (object) info.Hide),
      (object) new XAttribute((XName) "Colors", info.Colors == null ? (object) "" : (object) info.Colors),
      (object) new XAttribute((XName) "Skin", info.Skin == null ? (object) "" : (object) info.Skin),
      (object) new XAttribute((XName) "Style", (object) info.Style),
      (object) new XAttribute((XName) "LastDate", (object) info.LastDate.ToString("yyyy-MM-dd HH:mm:ss")),
      (object) new XAttribute((XName) "Sex", (object) info.Sex),
      (object) new XAttribute((XName) "LoginName", (object) info.LoginName),
      (object) new XAttribute((XName) "NickName", info.UserName == null ? (object) "" : (object) info.UserName)
    });

    public static XElement CreateConsortiaDutyInfo(ConsortiaDutyInfo info) => new XElement((XName) "Item", new object[5]
    {
      (object) new XAttribute((XName) "DutyID", (object) info.DutyID),
      (object) new XAttribute((XName) "ConsortiaID", (object) info.ConsortiaID),
      (object) new XAttribute((XName) "DutyName", info.DutyName == null ? (object) "" : (object) info.DutyName),
      (object) new XAttribute((XName) "Right", (object) info.Right),
      (object) new XAttribute((XName) "Level", (object) info.Level)
    });

    public static XElement CreateConsortiaApplyAllyInfo(ConsortiaApplyAllyInfo info) => new XElement((XName) "Item", new object[12]
    {
      (object) new XAttribute((XName) "ID", (object) info.ID),
      (object) new XAttribute((XName) "CelebCount", (object) info.CelebCount),
      (object) new XAttribute((XName) "ChairmanName", info.ChairmanName == null ? (object) "" : (object) info.ChairmanName),
      (object) new XAttribute((XName) "ConsortiaID", (object) info.Consortia1ID),
      (object) new XAttribute((XName) "ConsortiaName", info.ConsortiaName == null ? (object) "" : (object) info.ConsortiaName),
      (object) new XAttribute((XName) "Count", (object) info.Count),
      (object) new XAttribute((XName) "Date", (object) info.Date.ToString("yyyy-MM-dd HH:mm:ss")),
      (object) new XAttribute((XName) "Honor", (object) info.Honor),
      (object) new XAttribute((XName) "Remark", info.Remark == null ? (object) "" : (object) info.Remark),
      (object) new XAttribute((XName) "Level", (object) info.Level),
      (object) new XAttribute((XName) "Description", info.Description == null ? (object) "" : (object) info.Description),
      (object) new XAttribute((XName) "Repute", (object) info.Repute)
    });

    public static XElement CreateConsortiaAllyInfo(ConsortiaAllyInfo info) => new XElement((XName) "Item", new object[13]
    {
      (object) new XAttribute((XName) "ID", (object) info.ID),
      (object) new XAttribute((XName) "ChairmanName", info.ChairmanName1 == null ? (object) "" : (object) info.ChairmanName1),
      (object) new XAttribute((XName) "ConsortiaID", (object) info.Consortia1ID),
      (object) new XAttribute((XName) "ConsortiaName", info.ConsortiaName1 == null ? (object) "" : (object) info.ConsortiaName1),
      (object) new XAttribute((XName) "Count", (object) info.Count1),
      (object) new XAttribute((XName) "Honor", (object) info.Honor1),
      (object) new XAttribute((XName) "State", (object) info.State),
      (object) new XAttribute((XName) "Date", (object) info.Date.ToString("yyyy-MM-dd HH:mm:ss")),
      (object) new XAttribute((XName) "Level", (object) info.Level1),
      (object) new XAttribute((XName) "IsApply", (object) info.IsApply),
      (object) new XAttribute((XName) "Description", (object) info.Description1),
      (object) new XAttribute((XName) "Riches", (object) info.Riches1),
      (object) new XAttribute((XName) "Repute", (object) info.Repute1)
    });

    public static XElement CreateConsortiaEventInfo(ConsortiaEventInfo info) => new XElement((XName) "Item", new object[8]
    {
      (object) new XAttribute((XName) "ID", (object) info.ID),
      (object) new XAttribute((XName) "ConsortiaID", (object) info.ConsortiaID),
      (object) new XAttribute((XName) "Date", (object) info.Date.ToString("yyyy-MM-dd HH:mm:ss")),
      (object) new XAttribute((XName) "Type", (object) info.Type),
      (object) new XAttribute((XName) "Remark", (object) DateTime.Now.ToString()),
      (object) new XAttribute((XName) "NickName", (object) info.NickName),
      (object) new XAttribute((XName) "EventValue", (object) info.EventValue),
      (object) new XAttribute((XName) "ManagerName", (object) info.ManagerName)
    });

    public static XElement CreateConsortiLevelInfo(ConsortiaLevelInfo info) => new XElement((XName) "Item", new object[11]
    {
      (object) new XAttribute((XName) "Level", (object) info.Level),
      (object) new XAttribute((XName) "Count", (object) info.Count),
      (object) new XAttribute((XName) "Deduct", (object) info.Deduct),
      (object) new XAttribute((XName) "NeedGold", (object) info.NeedGold),
      (object) new XAttribute((XName) "NeedItem", (object) info.NeedItem),
      (object) new XAttribute((XName) "Reward", (object) info.Reward),
      (object) new XAttribute((XName) "ShopRiches", (object) info.ShopRiches),
      (object) new XAttribute((XName) "SmithRiches", (object) info.SmithRiches),
      (object) new XAttribute((XName) "StoreRiches", (object) info.StoreRiches),
      (object) new XAttribute((XName) "BufferRiches", (object) info.BufferRiches),
      (object) new XAttribute((XName) "Riches", (object) info.Riches)
    });

    public static XElement CreateEliteMatchPlayersList(PlayerInfo info, int rank) => new XElement((XName) "Item", new object[4]
    {
      (object) new XAttribute((XName) "PlayerID", (object) info.ID),
      (object) new XAttribute((XName) "PlayerName", info.NickName == null ? (object) "" : (object) info.NickName),
      (object) new XAttribute((XName) "PlayerScore", (object) info.EliteScore),
      (object) new XAttribute((XName) "PlayerRank", (object) rank)
    });

    public static XElement CreateCelebInfo(PlayerInfo info) => new XElement((XName) "Item", new object[42]
    {
      (object) new XAttribute((XName) "ID", (object) info.ID),
      (object) new XAttribute((XName) "UserName", info.UserName == null ? (object) "" : (object) info.UserName),
      (object) new XAttribute((XName) "NickName", info.NickName == null ? (object) "" : (object) info.NickName),
      (object) new XAttribute((XName) "typeVIP", (object) info.typeVIP),
      (object) new XAttribute((XName) "VIPLevel", (object) info.VIPLevel),
      (object) new XAttribute((XName) "Grade", (object) info.Grade),
      (object) new XAttribute((XName) "Colors", info.Colors == null ? (object) "" : (object) info.Colors),
      (object) new XAttribute((XName) "Skin", info.Skin == null ? (object) "" : (object) info.Skin),
      (object) new XAttribute((XName) "Sex", (object) info.Sex),
      (object) new XAttribute((XName) "Style", info.Style == null ? (object) "" : (object) info.Style),
      (object) new XAttribute((XName) "ConsortiaName", info.ConsortiaName == null ? (object) "" : (object) info.ConsortiaName),
      (object) new XAttribute((XName) "Hide", (object) info.Hide),
      (object) new XAttribute((XName) "Offer", (object) info.Offer),
      (object) new XAttribute((XName) "ReputeOffer", (object) info.ReputeOffer),
      (object) new XAttribute((XName) "ConsortiaHonor", (object) info.ConsortiaHonor),
      (object) new XAttribute((XName) "ConsortiaLevel", (object) info.ConsortiaLevel),
      (object) new XAttribute((XName) "StoreLevel", (object) info.StoreLevel),
      (object) new XAttribute((XName) "ShopLevel", (object) info.ShopLevel),
      (object) new XAttribute((XName) "SmithLevel", (object) info.SmithLevel),
      (object) new XAttribute((XName) "ConsortiaRepute", (object) info.ConsortiaRepute),
      (object) new XAttribute((XName) "WinCount", (object) info.Win),
      (object) new XAttribute((XName) "TotalCount", (object) info.Total),
      (object) new XAttribute((XName) "EscapeCount", (object) info.Escape),
      (object) new XAttribute((XName) "Repute", (object) info.Repute),
      (object) new XAttribute((XName) "AddDayGP", (object) info.AddDayGP),
      (object) new XAttribute((XName) "AddDayOffer", (object) info.AddDayOffer),
      (object) new XAttribute((XName) "AddWeekGP", (object) info.AddWeekGP),
      (object) new XAttribute((XName) "AddWeekOffer", (object) info.AddWeekOffer),
      (object) new XAttribute((XName) "ConsortiaRiches", (object) info.ConsortiaRiches),
      (object) new XAttribute((XName) "Nimbus", (object) info.Nimbus),
      (object) new XAttribute((XName) "GP", (object) info.GP),
      (object) new XAttribute((XName) "FightPower", (object) info.FightPower),
      (object) new XAttribute((XName) "AchievementPoint", (object) info.AchievementPoint),
      (object) new XAttribute((XName) "Rank", (object) ""),
      (object) new XAttribute((XName) "AddDayAchievementPoint", (object) 0),
      (object) new XAttribute((XName) "AddWeekAchievementPoint", (object) 0),
      (object) new XAttribute((XName) "GiftGp", (object) 0),
      (object) new XAttribute((XName) "GiftLevel", (object) 1),
      (object) new XAttribute((XName) "AddDayGiftGp", (object) 0),
      (object) new XAttribute((XName) "AddWeekGiftGp", (object) 0),
      (object) new XAttribute((XName) "ApprenticeshipState", (object) 0),
      (object) new XAttribute((XName) "AddWeekLeagueScore", (object) info.AddWeekLeagueScore)
    });

    public static XElement CreateBestEquipInfo(BestEquipInfo info) => new XElement((XName) "Item", new object[8]
    {
      (object) new XAttribute((XName) "Date", (object) info.Date.ToString("yyyy-MM-dd HH:mm:ss")),
      (object) new XAttribute((XName) "GP", (object) info.GP),
      (object) new XAttribute((XName) "Grade", (object) info.Grade),
      (object) new XAttribute((XName) "ItemName", info.ItemName == null ? (object) "" : (object) info.ItemName),
      (object) new XAttribute((XName) "NickName", info.NickName == null ? (object) "" : (object) info.NickName),
      (object) new XAttribute((XName) "Sex", (object) info.Sex),
      (object) new XAttribute((XName) "Strengthenlevel", (object) info.Strengthenlevel),
      (object) new XAttribute((XName) "Type", info.UserName == null ? (object) "" : (object) info.UserName)
    });

    public static XElement CreateMailInfo(MailInfo info, string nodeName)
    {
      DateTime.Now.Subtract(info.SendTime);
      return new XElement((XName) nodeName, new object[22]
      {
        (object) new XAttribute((XName) "ID", (object) info.ID),
        (object) new XAttribute((XName) "Title", (object) info.Title),
        (object) new XAttribute((XName) "Content", (object) info.Content),
        (object) new XAttribute((XName) "Sender", (object) info.Sender),
        (object) new XAttribute((XName) "Receiver", (object) info.Receiver),
        (object) new XAttribute((XName) "SendTime", (object) info.SendTime.ToString("yyyy-MM-dd HH:mm:ss")),
        (object) new XAttribute((XName) "ValidDate", (object) info.ValidDate),
        (object) new XAttribute((XName) "Gold", (object) info.Gold),
        (object) new XAttribute((XName) "Money", (object) info.Money),
        (object) new XAttribute((XName) "Annex1ID", info.Annex1 == null ? (object) "" : (object) info.Annex1),
        (object) new XAttribute((XName) "Annex2ID", info.Annex2 == null ? (object) "" : (object) info.Annex2),
        (object) new XAttribute((XName) "Annex3ID", info.Annex3 == null ? (object) "" : (object) info.Annex3),
        (object) new XAttribute((XName) "Annex4ID", info.Annex4 == null ? (object) "" : (object) info.Annex4),
        (object) new XAttribute((XName) "Annex5ID", info.Annex5 == null ? (object) "" : (object) info.Annex5),
        (object) new XAttribute((XName) "Annex1Name", info.Annex1Name == null ? (object) "" : (object) info.Annex1Name),
        (object) new XAttribute((XName) "Annex2Name", info.Annex2Name == null ? (object) "" : (object) info.Annex2Name),
        (object) new XAttribute((XName) "Annex3Name", info.Annex3Name == null ? (object) "" : (object) info.Annex3Name),
        (object) new XAttribute((XName) "Annex4Name", info.Annex4Name == null ? (object) "" : (object) info.Annex4Name),
        (object) new XAttribute((XName) "Annex5Name", info.Annex5Name == null ? (object) "" : (object) info.Annex5Name),
        (object) new XAttribute((XName) "AnnexRemark", info.AnnexRemark == null ? (object) "" : (object) info.AnnexRemark),
        (object) new XAttribute((XName) "Type", (object) info.Type),
        (object) new XAttribute((XName) "IsRead", (object) info.IsRead)
      });
    }

    public static XElement CreateBuffInfo(BufferInfo info) => new XElement((XName) "Item", new object[7]
    {
      (object) new XAttribute((XName) "BeginDate", (object) info.BeginDate.ToString("yyyy-MM-dd HH:mm:ss")),
      (object) new XAttribute((XName) "Data", info.Data == null ? (object) "" : (object) info.Data),
      (object) new XAttribute((XName) "IsExist", (object) info.IsExist),
      (object) new XAttribute((XName) "Type", (object) info.Type),
      (object) new XAttribute((XName) "UserID", (object) info.UserID),
      (object) new XAttribute((XName) "ValidDate", (object) info.ValidDate),
      (object) new XAttribute((XName) "Value", (object) info.Value)
    });

    public static XElement CreateMarryInfo(MarryInfo info) => new XElement((XName) "Info", new object[26]
    {
      (object) new XAttribute((XName) "ID", (object) info.ID),
      (object) new XAttribute((XName) "UserID", (object) info.UserID),
      (object) new XAttribute((XName) "IsPublishEquip", (object) info.IsPublishEquip),
      (object) new XAttribute((XName) "Introduction", (object) info.Introduction),
      (object) new XAttribute((XName) "NickName", (object) info.NickName),
      (object) new XAttribute((XName) "IsConsortia", (object) info.IsConsortia),
      (object) new XAttribute((XName) "ConsortiaID", (object) info.ConsortiaID),
      (object) new XAttribute((XName) "Sex", (object) info.Sex),
      (object) new XAttribute((XName) "Win", (object) info.Win),
      (object) new XAttribute((XName) "Total", (object) info.Total),
      (object) new XAttribute((XName) "Escape", (object) info.Escape),
      (object) new XAttribute((XName) "GP", (object) info.GP),
      (object) new XAttribute((XName) "Honor", (object) info.Honor),
      (object) new XAttribute((XName) "Style", (object) info.Style),
      (object) new XAttribute((XName) "Colors", (object) info.Colors),
      (object) new XAttribute((XName) "Hide", (object) info.Hide),
      (object) new XAttribute((XName) "Grade", (object) info.Grade),
      (object) new XAttribute((XName) "State", (object) info.State),
      (object) new XAttribute((XName) "Repute", (object) info.Repute),
      (object) new XAttribute((XName) "Skin", (object) info.Skin),
      (object) new XAttribute((XName) "Offer", (object) info.Offer),
      (object) new XAttribute((XName) "IsMarried", (object) info.IsMarried),
      (object) new XAttribute((XName) "ConsortiaName", (object) info.ConsortiaName),
      (object) new XAttribute((XName) "DutyName", (object) info.DutyName),
      (object) new XAttribute((XName) "Nimbus", (object) info.Nimbus),
      (object) new XAttribute((XName) "FightPower", (object) info.FightPower)
    });

    public static XElement CreateUserApprenticeshipInfo(PlayerInfo info) => new XElement((XName) "Item", new object[26]
    {
      (object) new XAttribute((XName) "UserID", (object) info.ID),
      (object) new XAttribute((XName) "NickName", (object) info.NickName),
      (object) new XAttribute((XName) "typeVIP", (object) info.typeVIP),
      (object) new XAttribute((XName) "VIPLevel", (object) info.VIPLevel),
      (object) new XAttribute((XName) "Skin", (object) info.Skin),
      (object) new XAttribute((XName) "Sex", (object) info.Sex),
      (object) new XAttribute((XName) "Grade", (object) info.Grade),
      (object) new XAttribute((XName) "Hide", (object) info.Hide),
      (object) new XAttribute((XName) "ConsortiaName", (object) info.ConsortiaName),
      (object) new XAttribute((XName) "WinCount", (object) info.Win),
      (object) new XAttribute((XName) "TotalCount", (object) info.Total),
      (object) new XAttribute((XName) "EscapeCount", (object) info.Escape),
      (object) new XAttribute((XName) "Offer", (object) info.Offer),
      (object) new XAttribute((XName) "State", (object) info.State),
      (object) new XAttribute((XName) "Repute", (object) info.Repute),
      (object) new XAttribute((XName) "DutyName", (object) info.DutyName),
      (object) new XAttribute((XName) "AchievementPoint", (object) info.AchievementPoint),
      (object) new XAttribute((XName) "Rank", (object) info.Honor),
      (object) new XAttribute((XName) "FightPower", (object) info.FightPower),
      (object) new XAttribute((XName) "ApprenticeshipState", (object) info.apprenticeshipState),
      (object) new XAttribute((XName) "GraduatesCount", (object) info.graduatesCount),
      (object) new XAttribute((XName) "IsMarried", (object) info.IsMarried),
      (object) new XAttribute((XName) "HonourOfMaster", (object) info.honourOfMaster),
      (object) new XAttribute((XName) "Style", (object) info.Style),
      (object) new XAttribute((XName) "Colors", (object) info.Colors),
      (object) new XAttribute((XName) "LastDate", (object) info.LastDate.ToString())
    });
        public static XElement CreateApprenticeShipInfo2(PlayerInfo info)
        {
            XElement xelement = new XElement(XName.Get("Info"));
            xelement.SetAttributeValue(XName.Get("UserID"), (object)info.ID);
            xelement.SetAttributeValue(XName.Get("ApplyFor"), (object)false);
            xelement.SetAttributeValue(XName.Get("IsPublishEquip"), (object)true);
            xelement.SetAttributeValue(XName.Get("NickName"), (object)info.NickName);
            xelement.SetAttributeValue(XName.Get("typeVIP"), (object)info.typeVIP);
            xelement.SetAttributeValue(XName.Get("VIPLevel"), (object)info.VIPLevel);
            xelement.SetAttributeValue(XName.Get("IsConsortia"), (object)info.IsConsortia);
            xelement.SetAttributeValue(XName.Get("ConsortiaID"), (object)info.ConsortiaID);
            xelement.SetAttributeValue(XName.Get("Sex"), (object)info.Sex);
            xelement.SetAttributeValue(XName.Get("Win"), (object)info.Win);
            xelement.SetAttributeValue(XName.Get("Total"), (object)info.Total);
            xelement.SetAttributeValue(XName.Get("Escape"), (object)info.Escape);
            xelement.SetAttributeValue(XName.Get("GP"), (object)info.GP);
            xelement.SetAttributeValue(XName.Get("Honor"), (object)info.Honor);
            xelement.SetAttributeValue(XName.Get("Style"), (object)info.Style);
            xelement.SetAttributeValue(XName.Get("Colors"), (object)info.Colors);
            xelement.SetAttributeValue(XName.Get("Hide"), (object)info.Hide);
            xelement.SetAttributeValue(XName.Get("Grade"), (object)info.Grade);
            xelement.SetAttributeValue(XName.Get("State"), (object)info.State);
            xelement.SetAttributeValue(XName.Get("Repute"), (object)info.Repute);
            xelement.SetAttributeValue(XName.Get("Skin"), (object)info.Skin);
            xelement.SetAttributeValue(XName.Get("Offer"), (object)info.Offer);
            xelement.SetAttributeValue(XName.Get("IsMarried"), (object)info.IsMarried);
            xelement.SetAttributeValue(XName.Get("ConsortiaName"), (object)info.ConsortiaName);
            xelement.SetAttributeValue(XName.Get("DutyName"), (object)info.DutyName);
            xelement.SetAttributeValue(XName.Get("Nimbus"), (object)info.Nimbus);
            xelement.SetAttributeValue(XName.Get("FightPower"), (object)info.FightPower);
            xelement.SetAttributeValue(XName.Get("AchievementPoint"), (object)info.AchievementPoint);
            xelement.SetAttributeValue(XName.Get("Rank"), (object)info.Honor);
            xelement.SetAttributeValue(XName.Get("ApprenticeshipState"), (object)info.apprenticeshipState);
            xelement.SetAttributeValue(XName.Get("GraduatesCount"), (object)info.graduatesCount);
            xelement.SetAttributeValue(XName.Get("HonourOfMaster"), (object)info.honourOfMaster);
            xelement.SetAttributeValue(XName.Get("SpouseID"), (object)info.SpouseID);
            xelement.SetAttributeValue(XName.Get("SpouseName"), (object)info.SpouseName);
            xelement.SetAttributeValue(XName.Get("BadgeID"), (object)info.badgeID);
            xelement.SetAttributeValue(XName.Get("BadgeBuyTime"), (object)DateTime.Now.ToString());
            xelement.SetAttributeValue(XName.Get("ValidDate"), (object)0);
            return xelement;
        }
        public static XElement CreateApprenticeShipInfo(PlayerInfo info) => new XElement((XName) "Info", new object[37]
    {
      (object) new XAttribute((XName) "UserID", (object) info.ID),
      (object) new XAttribute((XName) "ApplyFor", (object) false),
      (object) new XAttribute((XName) "IsPublishEquip", (object) true),
      (object) new XAttribute((XName) "NickName", (object) info.NickName),
      (object) new XAttribute((XName) "typeVIP", (object) info.typeVIP),
      (object) new XAttribute((XName) "VIPLevel", (object) info.VIPLevel),
      (object) new XAttribute((XName) "IsConsortia", (object) info.IsConsortia),
      (object) new XAttribute((XName) "ConsortiaID", (object) info.ConsortiaID),
      (object) new XAttribute((XName) "Sex", (object) info.Sex),
      (object) new XAttribute((XName) "Win", (object) info.Win),
      (object) new XAttribute((XName) "Total", (object) info.Total),
      (object) new XAttribute((XName) "Escape", (object) info.Escape),
      (object) new XAttribute((XName) "GP", (object) info.GP),
      (object) new XAttribute((XName) "Honor", (object) info.Honor),
      (object) new XAttribute((XName) "Style", (object) info.Style),
      (object) new XAttribute((XName) "Colors", (object) info.Colors),
      (object) new XAttribute((XName) "Hide", (object) info.Hide),
      (object) new XAttribute((XName) "Grade", (object) info.Grade),
      (object) new XAttribute((XName) "State", (object) info.State),
      (object) new XAttribute((XName) "Repute", (object) info.Repute),
      (object) new XAttribute((XName) "Skin", (object) info.Skin),
      (object) new XAttribute((XName) "Offer", (object) info.Offer),
      (object) new XAttribute((XName) "IsMarried", (object) info.IsMarried),
      (object) new XAttribute((XName) "ConsortiaName", (object) info.ConsortiaName),
      (object) new XAttribute((XName) "DutyName", (object) info.DutyName),
      (object) new XAttribute((XName) "Nimbus", (object) info.Nimbus),
      (object) new XAttribute((XName) "FightPower", (object) info.FightPower),
      (object) new XAttribute((XName) "AchievementPoint", (object) info.AchievementPoint),
      (object) new XAttribute((XName) "Rank", (object) info.Honor),
      (object) new XAttribute((XName) "ApprenticeshipState", (object) info.apprenticeshipState),
      (object) new XAttribute((XName) "GraduatesCount", (object) info.graduatesCount),
      (object) new XAttribute((XName) "HonourOfMaster", (object) info.honourOfMaster),
      (object) new XAttribute((XName) "SpouseID", (object) info.SpouseID),
      (object) new XAttribute((XName) "SpouseName", (object) info.SpouseName),
      (object) new XAttribute((XName) "BadgeID", (object) info.badgeID),
      (object) new XAttribute((XName) "BadgeBuyTime", (object) DateTime.Now.ToString()),
      (object) new XAttribute((XName) "ValidDate", (object) 0)
    });

    public static XElement CreateActiveInfo(DailyAwardInfo info) => new XElement((XName) "Item", new object[11]
    {
      (object) new XAttribute((XName) "ID", (object) info.ID),
      (object) new XAttribute((XName) "Count", (object) info.Count),
      (object) new XAttribute((XName) "CountRemark", info.CountRemark == null ? (object) "" : (object) info.CountRemark),
      (object) new XAttribute((XName) "IsBinds", (object) info.IsBinds),
      (object) new XAttribute((XName) "Remark", info.Remark == null ? (object) "" : (object) info.Remark),
      (object) new XAttribute((XName) "Sex", (object) info.Sex),
      (object) new XAttribute((XName) "TemplateID", (object) info.TemplateID),
      (object) new XAttribute((XName) "Type", (object) info.Type),
      (object) new XAttribute((XName) "ValidDate", (object) info.ValidDate),
      (object) new XAttribute((XName) "GetWay", (object) info.GetWay),
      (object) new XAttribute((XName) "AwardDays", (object) info.AwardDays)
    });

    public static XElement CreateConsortiaEquipControlInfo(ConsortiaEquipControlInfo info) => new XElement((XName) "Item", new object[4]
    {
      (object) new XAttribute((XName) "ConsortiaID", (object) info.ConsortiaID),
      (object) new XAttribute((XName) "Level", (object) info.Level),
      (object) new XAttribute((XName) "Riches", (object) info.Riches),
      (object) new XAttribute((XName) "Type", (object) info.Type)
    });

    public static XElement CreatNPCInfo(NpcInfo info) => new XElement((XName) "Item", new object[28]
    {
      (object) new XAttribute((XName) "ID", (object) info.ID),
      (object) new XAttribute((XName) "Name", (object) info.Name),
      (object) new XAttribute((XName) "Level", (object) info.Level),
      (object) new XAttribute((XName) "Camp", (object) info.Camp),
      (object) new XAttribute((XName) "Type", (object) info.Type),
      (object) new XAttribute((XName) "Blood", (object) info.Blood),
      (object) new XAttribute((XName) "MoveMin", (object) info.MoveMin),
      (object) new XAttribute((XName) "MoveMax", (object) info.MoveMax),
      (object) new XAttribute((XName) "BaseDamage", (object) info.BaseDamage),
      (object) new XAttribute((XName) "BaseGuard", (object) info.BaseGuard),
      (object) new XAttribute((XName) "Defence", (object) info.Defence),
      (object) new XAttribute((XName) "Agility", (object) info.Agility),
      (object) new XAttribute((XName) "Lucky", (object) info.Lucky),
      (object) new XAttribute((XName) "ModelID", (object) info.ModelID),
      (object) new XAttribute((XName) "ResourcesPath", (object) info.ResourcesPath),
      (object) new XAttribute((XName) "DropRate", (object) info.DropRate),
      (object) new XAttribute((XName) "Experience", (object) info.Experience),
      (object) new XAttribute((XName) "Delay", (object) info.Delay),
      (object) new XAttribute((XName) "Immunity", (object) info.Immunity),
      (object) new XAttribute((XName) "Alert", (object) info.Alert),
      (object) new XAttribute((XName) "Range", (object) info.Range),
      (object) new XAttribute((XName) "Preserve", (object) info.Preserve),
      (object) new XAttribute((XName) "Script", (object) info.Script),
      (object) new XAttribute((XName) "FireX", (object) info.FireX),
      (object) new XAttribute((XName) "FireY", (object) info.FireY),
      (object) new XAttribute((XName) "DropId", (object) info.DropId),
      (object) new XAttribute((XName) "MagicAttack", (object) 0),
      (object) new XAttribute((XName) "MagicDefence", (object) 0)
    });

    public static XElement CreateCardUpdateInfo(CardUpdateInfo info) => new XElement((XName) "Item", new object[8]
    {
      (object) new XAttribute((XName) "Id", (object) info.Id),
      (object) new XAttribute((XName) "Level", (object) info.Level),
      (object) new XAttribute((XName) "Attack", (object) info.Attack),
      (object) new XAttribute((XName) "Defend", (object) info.Defend),
      (object) new XAttribute((XName) "Agility", (object) info.Agility),
      (object) new XAttribute((XName) "Lucky", (object) info.Lucky),
      (object) new XAttribute((XName) "Guard", (object) info.Guard),
      (object) new XAttribute((XName) "Damage", (object) info.Damage)
    });

    public static XElement CreateCardUpdateCondition(CardUpdateConditionInfo info) => new XElement((XName) "Item", new object[7]
    {
      (object) new XAttribute((XName) "Level", (object) info.Level),
      (object) new XAttribute((XName) "Exp", (object) info.Exp),
      (object) new XAttribute((XName) "MinExp", (object) info.MinExp),
      (object) new XAttribute((XName) "MaxExp", (object) info.MaxExp),
      (object) new XAttribute((XName) "UpdateCardCount", (object) info.UpdateCardCount),
      (object) new XAttribute((XName) "ResetCardCount", (object) info.ResetCardCount),
      (object) new XAttribute((XName) "ResetMoney", (object) info.ResetMoney)
    });

    public static XElement CreatePetSkillTemplate(PetSkillTemplateInfo info) => new XElement((XName) "item", new object[7]
    {
      (object) new XAttribute((XName) "PetTemplateID", (object) info.PetTemplateID),
      (object) new XAttribute((XName) "KindID", (object) info.KindID),
      (object) new XAttribute((XName) "GetType", (object) 1),
      (object) new XAttribute((XName) "SkillID", (object) info.SkillID),
      (object) new XAttribute((XName) "SkillBookID", (object) info.SkillBookID),
      (object) new XAttribute((XName) "MinLevel", (object) info.MinLevel),
      (object) new XAttribute((XName) "DeleteSkillIDs", (object) info.DeleteSkillIDs)
    });

    public static XElement CreatePetSkillElement(PetSkillElementInfo info) => new XElement((XName) "item", new object[5]
    {
      (object) new XAttribute((XName) "ID", (object) info.ID),
      (object) new XAttribute((XName) "Name", (object) info.Name),
      (object) new XAttribute((XName) "EffectPic", (object) info.EffectPic),
      (object) new XAttribute((XName) "Description", (object) info.Description),
      (object) new XAttribute((XName) "Pic", (object) info.Pic)
    });

    public static XElement CreatePetSkillInfo(PetSkillInfo info) => new XElement((XName) "item", new object[14]
    {
      (object) new XAttribute((XName) "ID", (object) info.ID),
      (object) new XAttribute((XName) "Name", (object) info.Name),
      (object) new XAttribute((XName) "ElementIDs", (object) info.ElementIDs),
      (object) new XAttribute((XName) "Description", (object) info.Description),
      (object) new XAttribute((XName) "BallType", (object) info.BallType),
      (object) new XAttribute((XName) "NewBallID", (object) info.NewBallID),
      (object) new XAttribute((XName) "CostMP", (object) info.CostMP),
      (object) new XAttribute((XName) "Pic", (object) info.Pic),
      (object) new XAttribute((XName) "Action", (object) info.Action),
      (object) new XAttribute((XName) "EffectPic", (object) info.EffectPic),
      (object) new XAttribute((XName) "Delay", (object) info.Delay),
      (object) new XAttribute((XName) "ColdDown", (object) info.ColdDown),
      (object) new XAttribute((XName) "GameType", (object) info.GameType),
      (object) new XAttribute((XName) "Probability", (object) info.Probability)
    });

        public static XElement CreateNewTitleItems(NewTitleInfo info) => new XElement((XName)"Item", new object[8]
   {
      (object) new XAttribute((XName) "ID", (object) info.ID),
      (object) new XAttribute((XName) "Show", (object) info.Show),
      (object) new XAttribute((XName) "Name", (object) (info.Name ?? "")),
      (object) new XAttribute((XName) "Pic", (object) info.Pic),
      (object) new XAttribute((XName) "Att", (object) info.Att),
      (object) new XAttribute((XName) "Def", (object) info.Def),
      (object) new XAttribute((XName) "Agi", (object) info.Agi),
      (object) new XAttribute((XName) "Luck", (object) info.Luck)
   });

        public static XElement CreateActivitySystemItems(ActivitySystemItemInfo info) => new XElement((XName)"Item", new object[11]
    {
      (object) new XAttribute((XName) "ActivityType", (object) info.ActivityType),
      (object) new XAttribute((XName) "Quality", (object) info.Quality),
      (object) new XAttribute((XName) "TemplateID", (object) info.TemplateID),
      (object) new XAttribute((XName) "ValidDate", (object) info.ValidDate),
      (object) new XAttribute((XName) "Count", (object) info.Count),
      (object) new XAttribute((XName) "IsBinds", (object) info.IsBinds),
      (object) new XAttribute((XName) "StrengthenLevel", (object) info.StrengthenLevel),
      (object) new XAttribute((XName) "AttackCompose", (object) info.AttackCompose),
      (object) new XAttribute((XName) "DefendCompose", (object) info.DefendCompose),
      (object) new XAttribute((XName) "AgilityCompose", (object) info.AgilityCompose),
      (object) new XAttribute((XName) "LuckCompose", (object) info.LuckCompose)
    });
        public static XElement CreatePetTemplate(PetTemplateInfo info) => new XElement((XName) "item", new object[23]
    {
      (object) new XAttribute((XName) "TemplateID", (object) info.TemplateID),
      (object) new XAttribute((XName) "Name", (object) info.Name),
      (object) new XAttribute((XName) "KindID", (object) info.KindID),
      (object) new XAttribute((XName) "Description", (object) info.Description),
      (object) new XAttribute((XName) "Pic", (object) info.Pic),
      (object) new XAttribute((XName) "RareLevel", (object) info.RareLevel),
      (object) new XAttribute((XName) "MP", (object) info.MP),
      (object) new XAttribute((XName) "StarLevel", (object) info.StarLevel),
      (object) new XAttribute((XName) "GameAssetUrl", (object) info.GameAssetUrl),
      (object) new XAttribute((XName) "HighAgility", (object) info.HighAgility),
      (object) new XAttribute((XName) "HighAgilityGrow", (object) info.HighAgilityGrow),
      (object) new XAttribute((XName) "HighAttack", (object) info.HighAttack),
      (object) new XAttribute((XName) "HighAttackGrow", (object) info.HighAttackGrow),
      (object) new XAttribute((XName) "HighBlood", (object) info.HighBlood),
      (object) new XAttribute((XName) "HighBloodGrow", (object) info.HighBloodGrow),
      (object) new XAttribute((XName) "HighDamage", (object) info.HighDamage),
      (object) new XAttribute((XName) "HighDamageGrow", (object) info.HighDamageGrow),
      (object) new XAttribute((XName) "HighDefence", (object) info.HighDefence),
      (object) new XAttribute((XName) "HighDefenceGrow", (object) info.HighDefenceGrow),
      (object) new XAttribute((XName) "HighGuard", (object) info.HighGuard),
      (object) new XAttribute((XName) "HighGuardGrow", (object) info.HighGuardGrow),
      (object) new XAttribute((XName) "HighLuck", (object) info.HighLuck),
      (object) new XAttribute((XName) "HighLuckGrow", (object) info.HighLuckGrow)
    });

        public static XElement CreateGMActivityInfo(GmActivityInfo gmActivityInfo) => new XElement((XName)"Activity", new object[16]
    {
      (object) new XAttribute((XName) "activityId", (object) gmActivityInfo.activityId),
      (object) new XAttribute((XName) "activityName", (object) gmActivityInfo.activityName),
      (object) new XAttribute((XName) "activityType", (object) gmActivityInfo.activityType),
      (object) new XAttribute((XName) "activityChildType", (object) gmActivityInfo.activityChildType),
      (object) new XAttribute((XName) "getWay", (object) gmActivityInfo.getWay),
      (object) new XAttribute((XName) "desc", (object) (gmActivityInfo.desc ?? "")),
      (object) new XAttribute((XName) "rewardDesc", (object) (gmActivityInfo.rewardDesc ?? "")),
      (object) new XAttribute((XName) "beginTime", (object) gmActivityInfo.beginTime.ToString("yyyy-MM-dd HH:mm:ss")),
      (object) new XAttribute((XName) "beginShowTime", (object) gmActivityInfo.beginShowTime.ToString("yyyy-MM-dd HH:mm:ss")),
      (object) new XAttribute((XName) "endTime", (object) gmActivityInfo.endTime.ToString("yyyy-MM-dd HH:mm:ss")),
      (object) new XAttribute((XName) "endShowTime", (object) gmActivityInfo.endShowTime.ToString("yyyy-MM-dd HH:mm:ss")),
      (object) new XAttribute((XName) "icon", (object) gmActivityInfo.icon),
      (object) new XAttribute((XName) "isContinue", (object) gmActivityInfo.isContinue),
      (object) new XAttribute((XName) "status", (object) gmActivityInfo.status),
      (object) new XAttribute((XName) "remain1", (object) gmActivityInfo.remain1),
      (object) new XAttribute((XName) "remain2", (object) (gmActivityInfo.remain2 ?? ""))
    });

        public static XElement CreateGMGiftInfo(GmGiftInfo gmGiftInfo) => new XElement((XName)"Gift", new object[4]
        {
      (object) new XAttribute((XName) "giftbagId", (object) gmGiftInfo.giftbagId),
      (object) new XAttribute((XName) "activityId", (object) gmGiftInfo.activityId),
      (object) new XAttribute((XName) "rewardMark", (object) gmGiftInfo.rewardMark),
      (object) new XAttribute((XName) "giftbagOrder", (object) gmGiftInfo.giftbagOrder)
        });

        public static XElement CreateGMConditionInfo(GmActiveConditionInfo gmConditionInfo) => new XElement((XName)"Condition", new object[5]
        {
      (object) new XAttribute((XName) "giftbagId", (object) gmConditionInfo.giftbagId),
      (object) new XAttribute((XName) "conditionIndex", (object) gmConditionInfo.conditionIndex),
      (object) new XAttribute((XName) "conditionValue", (object) gmConditionInfo.conditionValue),
      (object) new XAttribute((XName) "remain1", (object) gmConditionInfo.remain1),
      (object) new XAttribute((XName) "remain2", (object) (gmConditionInfo.remain2 ?? ""))
        });

        public static XElement CreateGMRewardInfo(GmActiveRewardInfo gmRewardInfo) => new XElement((XName)"Reward", new object[9]
        {
      (object) new XAttribute((XName) "giftId", (object) gmRewardInfo.giftId),
      (object) new XAttribute((XName) "templateId", (object) gmRewardInfo.templateId),
      (object) new XAttribute((XName) "count", (object) gmRewardInfo.count),
      (object) new XAttribute((XName) "isBind", (object) gmRewardInfo.isBind),
      (object) new XAttribute((XName) "occupationOrSex", (object) gmRewardInfo.occupationOrSex),
      (object) new XAttribute((XName) "rewardType", (object) gmRewardInfo.rewardType),
      (object) new XAttribute((XName) "validDate", (object) gmRewardInfo.validDate),
      (object) new XAttribute((XName) "property", (object) gmRewardInfo.property),
      (object) new XAttribute((XName) "remain1", (object) (gmRewardInfo.remain1 ?? ""))
        });
    }
}
