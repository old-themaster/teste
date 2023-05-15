using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDataProvider.Data
{
    public class AtivityQuestGoodsInfo
    {
        public int ID { get; set; }

        public int QuestID { get; set; }

        public int QuestType { get; set; }

        public int Period { get; set; }

        public int TemplateID { get; set; }

        public int StrengthenLevel { get; set; }

        public int Count { get; set; }

        public int ValidDate { get; set; }

        public int AttackCompose { get; set; }

        public int DefendCompose { get; set; }

        public int AgilityCompose { get; set; }

        public int LuckCompose { get; set; }

        public bool IsBinds { get; set; }
    }
}
