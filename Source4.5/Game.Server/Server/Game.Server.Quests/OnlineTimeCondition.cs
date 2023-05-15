// Decompiled with JetBrains decompiler
// Type: Game.Server.Quests.DirectFinishCondition
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.Quests
{
    public class OnlineTimeCondition : BaseCondition
    {
        public OnlineTimeCondition(BaseQuest quest, QuestConditionInfo info, int value)
          : base(quest, info, value)
        {
        }

        public override bool IsCompleted(GamePlayer player) => true;
    }
}
