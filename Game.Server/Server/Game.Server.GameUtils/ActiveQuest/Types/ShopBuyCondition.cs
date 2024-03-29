﻿using SqlDataProvider.Data;

namespace Game.Server.GameUtils.ActiveQuest.Types
{
    public class ShopBuyCondition : BaseActiveQuestCondition
    {

        public ShopBuyCondition(BaseActivityQuest quest, AtivityQuestConditionInfo Data, int value) : base(quest, Data, value)
        {
        }

        public override void OnAttached()
        {
            this.m_quest.Owner.Paid += new GamePlayer.PlayerShopEventHandle(this.onPlayerShop);
        }

        public override void OnRemoved()
        {
            this.m_quest.Owner.Paid -= new GamePlayer.PlayerShopEventHandle(this.onPlayerShop);
        }

        private void onPlayerShop(int money, int gold, int offer, int gifttoken, int medal, int Ascension, string payGoods)
        {
            foreach (string str in payGoods.Split(new char[] { ',' }))
            {
                if (str == this.info.Para1.ToString())
                {
                    this.Value++;
                }
            }

            if (this.Value >= this.info.Para2)
            {
                this.Value = this.info.Para2;
            }
        }

        public override bool IsCompleted()
        {
            return this.Value >= this.info.Para2;
        }
    }
}
