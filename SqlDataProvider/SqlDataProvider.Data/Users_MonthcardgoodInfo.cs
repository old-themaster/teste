
using System;

namespace SqlDataProvider.Data
{
    public class Users_MonthcardgoodInfo
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        public int MonthcardgoodID { get; set; }

        public bool IsActive { get; set; }

        public DateTime LastUpdate { get; set; }

        public bool IsDirty { get; set; }
    }
}
