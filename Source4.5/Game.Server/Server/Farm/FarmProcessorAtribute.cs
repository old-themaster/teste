using System;

namespace Game.Server.Farm
{
	public class FarmProcessorAtribute : Attribute
	{
		private byte byte_0;

		private string string_0;

		public byte Code
		{
			get
			{
				return this.byte_0;
			}
		}

		public string Description
		{
			get
			{
				return this.string_0;
			}
		}

		public FarmProcessorAtribute(byte code, string description)
		{
			
			
			this.byte_0 = code;
			this.string_0 = description;
		}
	}
}