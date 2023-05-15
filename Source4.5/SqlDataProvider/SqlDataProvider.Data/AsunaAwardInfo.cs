namespace SqlDataProvider.Data
{
    public class AsunaAwardInfo
	{
		public int ID;
		public int ActivityType;
		public int TemplateID;
		public int Count;
		public int ValidDate;
		public bool IsBinds;
		public int StrengthenLevel;
		public int AttackCompose;
		public int DefendCompose;
		public int AgilityCompose;
		public int LuckCompose;
		public int Random;
		public int Position
		{
			get;
			set;
		}
	}
}
