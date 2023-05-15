using Game.Server.GameUtils.ActiveQuest.Types;
using SqlDataProvider.Data;
using System;

namespace Game.Server.GameUtils.ActiveQuest
{
    public class BaseActiveQuestCondition
    {
        internal int Value;
        internal BaseActivityQuest m_quest;
        internal AtivityQuestConditionInfo info;
        internal AtivityQuestConditionInfo _cond;

        public BaseActiveQuestCondition(BaseActivityQuest quest, AtivityQuestConditionInfo Data, int value)
        {
            m_quest = quest;
            info = Data;
            Value = value;
        }

        public virtual void OnRemoved()
        {

        }

        public virtual void OnAttached()
        {

        }
        public bool CanComplete()
        {
            if (m_quest.Quest.NeedMinLevel > m_quest.Owner.PlayerCharacter.Grade)
            {
                return false;
            }
            return Value >= _cond.Para2;
        }
        public virtual bool IsCompleted()
        {
            if (m_quest.Quest.NeedMinLevel > m_quest.Owner.PlayerCharacter.Grade)
            {
                return false;
            }
            return false;
        }


        public static BaseActiveQuestCondition CreateQuestCondition(BaseActivityQuest quest, AtivityQuestConditionInfo Data, int ConditionValue)
        {
            switch (Data.CondictionType)
            {
                case -1:
                    return new MoneyAccumulateCondition(quest, Data, ConditionValue);
                case -2:
                    return new PlayerBeadUpCondition(quest, Data, ConditionValue);
                case -3:
                    return new PlayerTexpCondition(quest, Data, ConditionValue);
                case -4:
                    return new ItemAdvanceUpCondition(quest, Data, ConditionValue);
                case 3:
                    return new PlayerItemUseCondition(quest, Data, ConditionValue);
                case 6:
                    return new PlayerGameOverCondition(quest, Data, ConditionValue);
                case 9:
                    return new ItemStrengthenupCondition(quest, Data, ConditionValue);
                case 10:
                    return new ShopConsumeCondition(quest, Data, ConditionValue);
                case 13:
                    return new GameMonsterCondition(quest, Data, ConditionValue);
                case 14:
                    return new ShopBuyCondition(quest, Data, ConditionValue);
                case 17:
                    return new OwnMarryCondition(quest, Data, ConditionValue);
                case 18:
                    return new OwnConsortiaCondition(quest, Data, ConditionValue);
                case 19:
                    return new ItemComposeCondition(quest, Data, ConditionValue);                   
                case 21:
                    return new PlayerMissionTurnOver(quest, Data, ConditionValue);
                case 23:
                    return new GameFightByGameCondition(quest, Data, ConditionValue);
                case 30:
                    return new PlayerRichesConsortiaCondition(quest, Data, ConditionValue);
                case 34:
                    return new PlayerGameOverCondition(quest, Data, ConditionValue);
                case 38:
                    return new MoneyAccumulateCondition(quest, Data, ConditionValue);
                case 50:
                    return new PlayerPetUpCondition(quest, Data, ConditionValue);
                case 61:
                    return new GradeActivityCondition(quest, Data, ConditionValue);
            }

            Console.WriteLine($"Condição Não Encontrada {Data.CondictionType}");
            return new BaseActiveQuestCondition(quest, Data, ConditionValue);
        }
    }
}
