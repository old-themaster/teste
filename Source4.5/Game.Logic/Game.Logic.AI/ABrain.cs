using Game.Logic.Phy.Object;

namespace Game.Logic.AI
{
    public abstract class ABrain
    {
        protected Living m_body;

        protected BaseGame m_game;

        public Living Body
        {
            get
            {
                return m_body;
            }
            set
            {
                m_body = value;
            }
        }

        public BaseGame Game
        {
            get
            {
                return m_game;
            }
            set
            {
                m_game = value;
            }
        }

        public virtual void Dispose()
        {
        }

        public virtual void OnAfterTakedBomb()
        {
        }

        public virtual void OnAfterTakedFrozen()
        {
        }

        public virtual void OnBeginNewTurn()
        {
        }

        public virtual void OnBeginSelfTurn()
        {
        }

        public virtual void OnCreated()
        {
        }

        public virtual void OnDie()
        {
        }

        public virtual void OnDiedSay()
        {
        }

        public virtual void OnKillPlayerSay()
        {
        }

        public virtual void OnShootedSay(int delay)
        {
        }

        public virtual void OnStartAttacking()
        {
        }

        public virtual void OnStopAttacking()
        {
        }

        public virtual void OnShootedSay()
        {
        }        
        public virtual void OnDiedEvent()
        {
        }
    }
}
