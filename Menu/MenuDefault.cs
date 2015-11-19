using System;

namespace SmartService.Menu
{
	/// <summary>
	/// Summary description for MenuDefault.
	/// </summary>
	public class MenuDefault
	{
		public int MenuID;
		public int OptionID;
		public int DefaultChoiceID;

		public MenuDefault()
		{
		}

		public MenuDefault(MenuItem menu, int optionID, int choiceID)
		{
			this.MenuID = menu.ID;
			this.OptionID = optionID;
			this.DefaultChoiceID = choiceID;
		}
	}
}
