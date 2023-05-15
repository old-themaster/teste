using Bussiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Match.Splush
{
    //
    public class GuildBalancemode
    {

        public static bool Reload()
        {
            return true;
        }

        public static bool CheckStartTime()
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday || DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
            {
                return false;
            }

            bool flag = false;
            List<string> startTimes = new List<string> { "09:00","21:30"};
            List<string> endTimes = new List<string> { "10:00","22:30"};

            for (int i = 0; i < startTimes.Count; i++)
            {
                var startTime = startTimes[i].Split(':');
                var endTime = endTimes[i].Split(':');

                int startHour = int.Parse(startTime[0]); //Hora inicial
                int startMinute = int.Parse(startTime[1]); //Minuto inicial
                int endHour = int.Parse(endTime[0]); //Hora final
                int endMinute = int.Parse(endTime[1]);

                if (DateTime.Now.Hour >= startHour && DateTime.Now.Hour < endHour)
                {
                    if (DateTime.Now.Hour == startHour)
                    {
                        if (DateTime.Now.Minute >= startMinute)
                        {
                            flag = true;
                            break;
                        }
                    }
                    else if (DateTime.Now.Hour > startHour && DateTime.Now.Hour < endHour)
                    {
                        flag = true;
                        break;
                    }
                }
            }
            return flag;
        }
    }
}