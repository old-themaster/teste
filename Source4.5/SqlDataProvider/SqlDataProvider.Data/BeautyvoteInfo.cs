// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.BeautyvoteInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 17193778-438D-49B4-8ECB-C1483D8CE3C1
// Assembly location: C:\DDtank 4.5\baixar flash\Emulator\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
    public class BeautyvoteInfo
    {
        private int id;
        private int count;
        private int userID;
        private string nickName;
        private string style;
        private string color;

        public int ID
        {
            get => this.id;
            set => this.id = value;
        }

        public int Count
        {
            get => this.count;
            set => this.count = value;
        }

        public int UserID
        {
            get => this.userID;
            set => this.userID = value;
        }

        public string NickName
        {
            get => this.nickName;
            set => this.nickName = value;
        }

        public string Style
        {
            get => this.style;
            set => this.style = value;
        }

        public string Color
        {
            get => this.color;
            set => this.color = value;
        }
    }
}
