using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.Packets;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Game.Server.GameUtils.ActiveQuest
{
    public class ActiveQuestInventory
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private GamePlayer _owner;
        List<BaseActivityQuest> quests;


        private int Level
        {
            get
            {
                if (_owner.PlayerCharacter.Grade < 15)
                    return 1;
                if (_owner.PlayerCharacter.Grade < 25)
                    return 2;
                if (_owner.PlayerCharacter.Grade < 35)
                    return 3;
                if (_owner.PlayerCharacter.Grade < 45)
                    return 4;
                if (_owner.PlayerCharacter.Grade < 55)
                    return 5;
                if (_owner.PlayerCharacter.Grade < 60)
                    return 6;
                else
                    return 7;
            }
        }

        public ActiveQuestInventory(GamePlayer player)
        {
            this._owner = player;
            quests = new List<BaseActivityQuest>();
        }

        public void GetRewardActivityQuest(GSPacketIn packet, int eventType)
        {
            var pkg = new GSPacketIn((short)ePackageType.ACTIVITY_SYSTEM);
            pkg.WriteByte(88);

            int rewardType = packet.ReadInt(); // reward type
            int questID = packet.ReadInt(); // quest id
            if (rewardType == 1)
            {
                if (this.quests.Any(s => s.Data.QuestID == questID))
                {
                    var quest = this.quests.First(s => s.Data.QuestID == questID);
                    if (quest.CanComplete() && !quest.Data.IsFinished)
                    {
                        List<ItemInfo> itens = new List<ItemInfo>();
                        foreach (var reward in ActivityQuestMngr.GetAllActivitysQuestGoodsByQuestID(questID))
                        {
                            var _item = ItemMgr.FindItemTemplate(reward.TemplateID);
                            if (_item != null)
                            {
                                var newItem = ItemInfo.CreateFromTemplate(_item, reward.Count, (int)eItemAddType.Other);
                                newItem.AttackCompose = reward.AttackCompose;
                                newItem.DefendCompose = reward.DefendCompose;
                                newItem.AgilityCompose = reward.AgilityCompose;
                                newItem.LuckCompose = reward.LuckCompose;
                                newItem.StrengthenLevel = reward.StrengthenLevel;
                                newItem.IsBinds = reward.IsBinds;
                                newItem.ValidDate = reward.ValidDate;
                                itens.Add(newItem);
                            }

                        }

                        this._owner.AddItem(itens.ToArray());
                        quest.Data.IsFinished = true;
                        pkg.WriteBoolean(true);
                        //this._owner.SendMessage(LanguageMgr.GetTranslation("Retirou prêmios c/êxito"));
                    }
                    else
                    {
                        pkg.WriteBoolean(false);
                    }
                }
            }
            else if (rewardType == 2 && questID >= 1 && questID <= 7)
            {
                if (!this._owner.PlayerCharacter.GodsRoadLevelData[questID - 1])
                {
                    this._owner.PlayerCharacter.GodsRoadLevelData[questID - 1] = true;

                    List<ItemInfo> itens = new List<ItemInfo>();
                    foreach (var reward in ActivityQuestMngr.GetAllActivitysQuestGoods(2, questID))
                    {
                        var _item = ItemMgr.FindItemTemplate(reward.TemplateID);
                        if (_item != null)
                        {
                            var newItem = ItemInfo.CreateFromTemplate(_item, reward.Count, (int)eItemAddType.Other);
                            newItem.AttackCompose = reward.AttackCompose;
                            newItem.DefendCompose = reward.DefendCompose;
                            newItem.AgilityCompose = reward.AgilityCompose;
                            newItem.LuckCompose = reward.LuckCompose;
                            newItem.StrengthenLevel = reward.StrengthenLevel;
                            newItem.IsBinds = reward.IsBinds;
                            newItem.ValidDate = reward.ValidDate;
                            itens.Add(newItem);
                        }

                    }

                    this._owner.AddItem(itens.ToArray());
                    pkg.WriteBoolean(true);
                }

            }
            else
            {
                this._owner.SendMessage(LanguageMgr.GetTranslation("Recompensas Coletadas"));
                pkg.WriteBoolean(false);
            }
            this._owner.SendTCP(pkg);
        }

        public void GetRewardSevenDayTarget(GSPacketIn packet)
        {
            var pkg = new GSPacketIn((short)ePackageType.ACTIVITY_SYSTEM);
            pkg.WriteByte(82);

            int questID = packet.ReadInt(); // quest id
            Console.WriteLine($"Carreguei {questID}");


            if (this.quests.Any(s => s.Data.QuestID == questID))
            {
                var quest = this.quests.First(s => s.Data.QuestID == questID);
                if (quest.CanComplete() && !quest.Data.IsFinished)
                {
                    List<ItemInfo> itens = new List<ItemInfo>();
                    foreach (var reward in ActivityQuestMngr.GetAllActivitysQuestGoodsByQuestID(questID))
                    {
                        var _item = ItemMgr.FindItemTemplate(reward.TemplateID);
                        if (_item != null)
                        {
                            var newItem = ItemInfo.CreateFromTemplate(_item, reward.Count, (int)eItemAddType.Other);
                            newItem.AttackCompose = reward.AttackCompose;
                            newItem.DefendCompose = reward.DefendCompose;
                            newItem.AgilityCompose = reward.AgilityCompose;
                            newItem.LuckCompose = reward.LuckCompose;
                            newItem.StrengthenLevel = reward.StrengthenLevel;
                            newItem.IsBinds = reward.IsBinds;
                            newItem.ValidDate = reward.ValidDate;
                            itens.Add(newItem);
                        }

                    }

                    this._owner.AddItem(itens.ToArray());
                    quest.Data.IsFinished = true;
                    pkg.WriteBoolean(true);
                    this._owner.SendMessage(LanguageMgr.GetTranslation("Retirou prêmios c/êxito"));
                }
                else
                {
                    this._owner.SendMessage(LanguageMgr.GetTranslation("Erro ao retirar prêmios."));
                    pkg.WriteBoolean(false);
                }
            }
            this._owner.SendTCP(pkg);
        }

        public void GetRewardNewPlayerActivity(GSPacketIn packet)
        {
            var pkg = new GSPacketIn((short)ePackageType.ACTIVITY_SYSTEM);
            pkg.WriteByte(98);

            int questID = packet.ReadInt(); // quest id
            Console.WriteLine($"Carreguei {questID}");


            if (this.quests.Any(s => s.Data.QuestID == questID))
            {
                var quest = this.quests.First(s => s.Data.QuestID == questID);
                if (quest.CanComplete() && !quest.Data.IsFinished)
                {
                    List<ItemInfo> itens = new List<ItemInfo>();
                    foreach (var reward in ActivityQuestMngr.GetAllActivitysQuestGoodsByQuestID(questID))
                    {
                        var _item = ItemMgr.FindItemTemplate(reward.TemplateID);
                        if (_item != null)
                        {
                            var newItem = ItemInfo.CreateFromTemplate(_item, reward.Count, (int)eItemAddType.Other);
                            newItem.AttackCompose = reward.AttackCompose;
                            newItem.DefendCompose = reward.DefendCompose;
                            newItem.AgilityCompose = reward.AgilityCompose;
                            newItem.LuckCompose = reward.LuckCompose;
                            newItem.StrengthenLevel = reward.StrengthenLevel;
                            newItem.IsBinds = reward.IsBinds;
                            newItem.ValidDate = reward.ValidDate;
                            itens.Add(newItem);
                        }

                    }

                    this._owner.AddItem(itens.ToArray());
                    quest.Data.IsFinished = true;
                    pkg.WriteBoolean(true);
                    this._owner.SendMessage(LanguageMgr.GetTranslation("Retirou prêmios c/êxito"));
                }
                else
                {
                    this._owner.SendMessage(LanguageMgr.GetTranslation("Erro ao retirar prêmios."));
                    pkg.WriteBoolean(false);
                }
            }
            this._owner.SendTCP(pkg);
        }

        public void SyncSevenDayTarget()
        {
            int Days = DateTime.Now.AddDays(-1).Day;
            // var Days = (DateTime.Now - this._owner.PlayerCharacter.CreateDate.AddDays(-1)).Days;
            var pkg = new GSPacketIn((short)ePackageType.SEVENDAYTARGET_GODSROADS);
            pkg.WriteByte(81);
            pkg.WriteInt(7);//Dias
            int currentDay = (Days > 7) ? 7 : Days;
            pkg.WriteInt(currentDay);//Dia Atual
            for (int x = 1; x <= 7; x++)
            {

                pkg.WriteBoolean(false);
                var quests = this.quests.Where(s => s.Info.QuestType == 1 && s.Info.Period == x);
                pkg.WriteInt(quests.Count());
                foreach (var Quest in quests)
                {
                    pkg.WriteInt(Quest.Data.QuestID);
                    pkg.WriteBoolean((currentDay >= x) ? Quest.CanComplete() : false);
                    pkg.WriteInt(Quest.Data.Condiction1);
                    pkg.WriteInt(Quest.Data.Condiction2);
                    pkg.WriteInt(Quest.Data.Condiction3);
                    pkg.WriteInt(Quest.Data.Condiction4);
                    pkg.WriteBoolean(Quest.Data.IsFinished);

                    //Recompensa
                    var rewards = ActivityQuestMngr.GetAllActivitysQuestGoodsByQuestID(Quest.Data.QuestID);

                    if (rewards.Count() >= 1)
                    {
                        pkg.WriteInt(rewards.Count());//quantidade

                        foreach (var reward in rewards)
                        {
                            pkg.WriteInt(reward.TemplateID); //ItemID
                            pkg.WriteInt(reward.StrengthenLevel);
                            pkg.WriteInt(reward.Count); // count
                            pkg.WriteInt(reward.ValidDate);
                            pkg.WriteInt(reward.AttackCompose);
                            pkg.WriteInt(reward.DefendCompose);
                            pkg.WriteInt(reward.AgilityCompose);
                            pkg.WriteInt(reward.LuckCompose);
                            pkg.WriteBoolean(reward.IsBinds);
                        }
                    }
                    else
                    {
                        pkg.WriteInt(1);//quantidade

                        pkg.WriteInt(11025); //ItemID
                        pkg.WriteInt(0);
                        pkg.WriteInt(15); // count
                        pkg.WriteInt(0);
                        pkg.WriteInt(0);
                        pkg.WriteInt(0);
                        pkg.WriteInt(0);
                        pkg.WriteInt(0);
                        pkg.WriteBoolean(true);
                    }


                }

                pkg.WriteInt(1);
            }
            this._owner.SendTCP(pkg);
        }

        public void SyncActivityQuest()
        {
            var pkg = new GSPacketIn((short)ePackageType.SEVENDAYTARGET_GODSROADS);
            pkg.WriteByte(87);
            pkg.WriteInt(7);//Nivel
            pkg.WriteInt(Level);//Nivel Atual

            for (int x = 1; x <= 7; x++)
            {
                var quests = this.quests.Where(s => s.Info.QuestType == 2 && s.Info.Period == x);

                if (quests.Count(s => s.CanComplete()) == quests.Count())
                    pkg.WriteBoolean(Level >= x && this._owner.PlayerCharacter.GodsRoadLevelData[x - 1]);
                else
                    pkg.WriteBoolean(true);
                pkg.WriteInt(quests.Count());
                foreach (var Quest in quests)
                {
                    pkg.WriteInt(Quest.Data.QuestID);
                    pkg.WriteBoolean((Level >= x) ? Quest.CanComplete() : false);
                    pkg.WriteInt(Quest.Data.Condiction1);
                    pkg.WriteInt(Quest.Data.Condiction2);
                    pkg.WriteInt(Quest.Data.Condiction3);
                    pkg.WriteInt(Quest.Data.Condiction4);
                    pkg.WriteBoolean(Quest.Data.IsFinished);

                    //Recompensa
                    var rewards = ActivityQuestMngr.GetAllActivitysQuestGoodsByQuestID(Quest.Data.QuestID);

                    pkg.WriteInt(rewards.Count());//quantidade
                    foreach (var reward in rewards)
                    {
                        pkg.WriteInt(reward.TemplateID); //ItemID
                        pkg.WriteInt(reward.StrengthenLevel);
                        pkg.WriteInt(reward.Count); // count
                        pkg.WriteInt(reward.ValidDate);
                        pkg.WriteInt(reward.AttackCompose);
                        pkg.WriteInt(reward.DefendCompose);
                        pkg.WriteInt(reward.AgilityCompose);
                        pkg.WriteInt(reward.LuckCompose);
                        pkg.WriteBoolean(reward.IsBinds);
                    }

                }

                //Recompensa
                var rewardsLevel = (ActivityQuestMngr.GetAllActivitysQuestGoods()).Where(s => s.QuestType == 2 && s.Period == x && s.QuestID == 0);

                pkg.WriteInt(rewardsLevel.Count());//quantidade
                foreach (var reward in rewardsLevel)
                {
                    pkg.WriteInt(reward.TemplateID); //ItemID
                    pkg.WriteInt(reward.StrengthenLevel);
                    pkg.WriteInt(reward.Count); // count
                    pkg.WriteInt(reward.ValidDate);
                    pkg.WriteInt(reward.AttackCompose);
                    pkg.WriteInt(reward.DefendCompose);
                    pkg.WriteInt(reward.AgilityCompose);
                    pkg.WriteInt(reward.LuckCompose);
                    pkg.WriteBoolean(reward.IsBinds);
                }
            }
            this._owner.SendTCP(pkg);
        }

        public void SyncNewPlayerActivity()
        {
            var pkg = new GSPacketIn((short)ePackageType.SEVENDAYTARGET_GODSROADS);
            pkg.WriteByte((byte)SevenDayTargetPackageType.NEWPLAYERREWARD_ENTER);

            for (int x = 1; x <= 3; x++)
            {
                var quests = this.quests.Where(s => s.Info.QuestType == 3 && s.Info.Period == x);
                pkg.WriteInt(quests.Count());
                foreach (var Quest in quests)
                {
                    var Condiction = ActivityQuestMngr.GetCondtionsByQuestID(Quest.Info.ID)[0];
                    pkg.WriteInt(Quest.Data.QuestID);
                    pkg.WriteInt(Condiction.Para2); //puxar o para2 
                    pkg.WriteBoolean(Quest.CanComplete());
                    pkg.WriteBoolean(Quest.Data.IsFinished);

                    //Recompensa
                    var rewards = ActivityQuestMngr.GetAllActivitysQuestGoodsByQuestID(Quest.Data.QuestID);

                    if (rewards.Count() >= 1)
                    {
                        pkg.WriteInt(rewards.Count());
                        foreach (var reward in rewards)
                        {
                            pkg.WriteInt(reward.TemplateID);
                            pkg.WriteInt(reward.Count);
                        }
                    }
                    else
                    {
                        pkg.WriteInt(1);
                        pkg.WriteInt(11025);
                        pkg.WriteInt(1);
                    }
                }
            }
            this._owner.SendTCP(pkg);
        }

        public bool LoadFromDatabase()
        {
            try
            {
                // foreach (var Quest in ActivityQuestMngr.GetActivitysByType(2))
                foreach (var Quest in ActivityQuestMngr.GetAllActivitys())
                {
                    using (PlayerBussiness pb = new PlayerBussiness())
                    {
                        var data = pb.GetUserActivityQuest(this._owner.PlayerCharacter.ID, Quest.ID);
                        if (data == null)
                            quests.Add(new BaseActivityQuest(Quest, new ActiveQuestUserData()
                            {
                                IsDirty = true,
                                QuestID = Quest.ID,
                                UserID = this._owner.PlayerCharacter.ID,
                                Condiction1 = 0,
                                Condiction2 = 0,
                                Condiction3 = 0,
                                Condiction4 = 0,
                                IsCompleted = false,
                                IsFinished = false,
                                // ConditionID = -1
                            }, _owner));
                        else
                            quests.Add(new BaseActivityQuest(Quest, data, _owner));


                    }


                }
            }
            catch (Exception)
            {
                log.Error("erro aqui 11");
            }
            return false;
        }

        public bool SaveToDatabase()
        {


            try
            {
                using (PlayerBussiness bussines = new PlayerBussiness())
                {
                    foreach (var quest in quests.ToArray())
                    {
                        quest.UpdateConditions();
                        if (quest.Data.IsDirty) ;
                        // bussines.UpdateOrSaveActivityQuestData(quest.Data);

                    }
                }
                return true;

            }
            catch (Exception)
            {
                log.Error("erro aqui 222");
                return false;
            }
        }
    }
}
