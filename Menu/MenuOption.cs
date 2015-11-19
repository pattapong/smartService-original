using System;

namespace SmartService.Menu
{
	/// <summary>
	/// Summary description for MenuOption.
	/// </summary>
	public class MenuOption
	{
		// Fields
		public OptionChoice[] OptionChoices;
		public int OptionID;
		public string OptionName;

		// Methods
		public MenuOption()
		{
		}

		public MenuOption(int optionID, string optionName)
		{
			this.OptionID = optionID;
			this.OptionName = optionName;
			this.OptionChoices = null;
		}
	}

 

}
