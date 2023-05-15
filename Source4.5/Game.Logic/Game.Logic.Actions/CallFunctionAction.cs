using Game.Logic.Phy.Object;

namespace Game.Logic.Actions
{
    public class CallFunctionAction : BaseAction
    {
        private LivingCallBack m_func;

        public CallFunctionAction(LivingCallBack func, int delay)
            : base(delay)
        {
            m_func = func;
        }

        protected override void ExecuteImp(BaseGame game, long tick)
        {
            try
            {
                m_func();
            }
            finally
            {
                Finish(tick);
            }
        }
    }
}
