using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDataProvider.Data
{
    public class AtivityQuestConditionInfo
    {
        public int QuestID { get; set; }

        public int CondictionID { get; set; }

        public string CondictionTitle { get; set; }

        public int CondictionType { get; set; }

        public int Para1 { get; set; }

        public int Para2 { get; set; }

        public int IndexType { get; set; }
    }
}
