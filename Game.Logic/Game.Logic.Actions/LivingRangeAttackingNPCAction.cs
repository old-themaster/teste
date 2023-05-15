using Game.Base.Packets;
using Game.Logic.Phy.Object;
using System.Drawing;
namespace Game.Logic.Actions
{
    public class LivingRangeAttackingNPCAction : BaseAction
    {
        private Living living_0;
        private System.Collections.Generic.List<Living> list_0;
        private string string_0;
        public LivingRangeAttackingNPCAction(Living living, string action, int delay, System.Collections.Generic.List<Living> livings) : base(delay, 1000)
        {
            living_0 = living;
            list_0 = livings;
            string_0 = action;
        }
        private int method_0(Living living_1)
        {
            double baseDamage = living_0.BaseDamage;
            double baseGuard = living_1.BaseGuard;
            double defence = living_1.Defence;
            double attack = living_0.Attack;
            float currentDamagePlus = living_0.CurrentDamagePlus;
            float currentShootMinus = living_0.CurrentShootMinus;
            double num = 0.95 * (baseGuard - 3 * living_0.Grade) / (500.0 + baseGuard - 3 * living_0.Grade);
            double num2;
            if (defence - living_0.Lucky < 0.0)
            {
                num2 = 0.0;
            }
            else
            {
                num2 = 0.95 * (defence - living_0.Lucky) / (600.0 + defence - living_0.Lucky);
            }
            double num3 = baseDamage * (1.0 + attack * 0.001) * (1.0 - (num + num2 - num * num2)) * currentDamagePlus * currentShootMinus;
            Rectangle directDemageRect = living_1.GetDirectDemageRect();
            //System.Math.Sqrt((directDemageRect.X - living_0.X) * (directDemageRect.X - living_0.X) + (directDemageRect.Y - living_0.Y) * (directDemageRect.Y - living_0.Y));
            if (num3 < 0.0)
            {
                return 1;
            }
            return (int)num3;
        }
        protected override void ExecuteImp(BaseGame game, long tick)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(91, living_0.Id) {Parameter1 = (living_0.Id)};
            gSPacketIn.WriteByte(61);
            int count = list_0.Count;
            gSPacketIn.WriteInt(count);
            foreach (Living current in list_0)
            {
                if (current is SimpleNpc && string_0 != "movie")
                {
                    game.method_27(current, string_0);
                }
                int num = method_0(current);
                int num2 = 0;
                int num3 = 0;
                if (current.TakeDamage(living_0, ref num, ref num2, "范围攻击"))
                {
                    num3 = num + num2;
                }
                gSPacketIn.WriteInt(current.Id);
                gSPacketIn.WriteInt(num3);
                gSPacketIn.WriteInt(current.Blood);
                gSPacketIn.WriteInt(0);
                gSPacketIn.WriteInt(1);
            }
            game.SendToAll(gSPacketIn);
            Finish(tick);
        }
    }
}
