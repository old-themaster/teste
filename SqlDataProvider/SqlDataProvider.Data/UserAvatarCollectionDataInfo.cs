namespace SqlDataProvider.Data
{
    public class UserAvatarCollectionDataInfo
	{
		private int _TemplateID;
		private int _Sex;
		public int TemplateID
		{
			get
			{
				return this._TemplateID;
			}
			set
			{
				this._TemplateID = value;
			}
		}
		public int Sex
		{
			get
			{
				return this._Sex;
			}
			set
			{
				this._Sex = value;
			}
		}
		public UserAvatarCollectionDataInfo()
		{
		}
		public UserAvatarCollectionDataInfo(int templateid, int sex)
		{
			this._TemplateID = templateid;
			this._Sex = sex;
		}
	}
}
