﻿namespace SqlDataProvider.Data
{
    public class ClothGroupTemplateInfo
	{
		private int _ItemID;
		private int _id;
		private int _TemplateID;
		private int _sex;
		private int _Description;
		private int _Cost;
		public int ItemID
		{
			get
			{
				return this._ItemID;
			}
			set
			{
				this._ItemID = value;
			}
		}
		public int ID
		{
			get
			{
				return this._id;
			}
			set
			{
				this._id = value;
			}
		}
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
				return this._sex;
			}
			set
			{
				this._sex = value;
			}
		}
		public int Description
		{
			get
			{
				return this._Description;
			}
			set
			{
				this._Description = value;
			}
		}
		public int Cost
		{
			get
			{
				return this._Cost;
			}
			set
			{
				this._Cost = value;
			}
		}
	}
}
