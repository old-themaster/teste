// Decompiled with JetBrains decompiler
// Type: Game.Server.Quests.ItemStrengthenCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using SqlDataProvider.Data;

namespace Game.Server.Quests
{
    public class ItemStrengthenCondition : BaseCondition
    {
        public ItemStrengthenCondition(BaseQuest quest, QuestConditionInfo info, int value)
            : base(quest, info, value)
        {
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.ItemStrengthen += player_ItemStrengthen;
        }

        public override bool IsCompleted(GamePlayer player)
        {
            if (base.m_info.Para1 == 1 && player.EquipBag.GetItemAt(0) != null && player.EquipBag.GetItemAt(0).StrengthenLevel >= m_info.Para2)
            {
                return true;
            }
            if (base.m_info.Para1 == 5 && player.EquipBag.GetItemAt(4) != null && player.EquipBag.GetItemAt(4).StrengthenLevel >= m_info.Para2)
            {
                return true;
            }
            if (base.m_info.Para1 == 7 && player.EquipBag.GetItemAt(6) != null && player.EquipBag.GetItemAt(6).StrengthenLevel >= m_info.Para2)
            {
                return true;
            }
            if (base.m_info.Para1 == 17 && player.EquipBag.GetItemAt(15) != null && player.EquipBag.GetItemAt(15).StrengthenLevel >= m_info.Para2)
            {
                return true;
            }
            return false;
        }

        private void player_ItemStrengthen(int categoryID, int level)
        {
            if (m_info.Para1 == categoryID && m_info.Para2 <= level)
            {
                base.Value = 0;
            }
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.ItemStrengthen -= player_ItemStrengthen;
        }
    }
}
