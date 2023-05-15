
// Type: Game.Logic.PetEffects.BasePetEffect
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Coded by Wonder Games Team | Copyright 2020 - 2021Game.Logic.dll

using Game.Logic.Phy.Object;

namespace Game.Logic.PetEffects
{
    public class BasePetEffect : AbstractPetEffect
    {
        public object ElementInfo { get; internal set; }

        public BasePetEffect(ePetEffectType type, string elementId)
          : base(type, elementId)
        {
        }

        public override sealed void OnAttached(Living living)
        {
            if (!(living is Player))
                return;
            this.OnAttachedToPlayer(living as Player);
        }

        protected virtual void OnAttachedToPlayer(Player player)
        {
        }

        protected virtual void OnPausedOnPlayer(Player player)
        {
        }

        public override sealed void OnRemoved(Living living)
        {
            if (!(living is Player))
                return;
            this.OnRemovedFromPlayer(living as Player);
        }

        protected virtual void OnRemovedFromPlayer(Player player)
        {
        }

        public override bool Start(Living living)
        {
            if (living is Player)
                return base.Start(living);
            return false;
        }
    }
}
