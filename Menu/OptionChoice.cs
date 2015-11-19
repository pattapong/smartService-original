using System;

namespace SmartService.Menu
{
	/// <summary>
	/// Summary description for OptionChoice.
	/// </summary>
	public class OptionChoice
	{
		// Fields
		public int ChoiceID;
		public string ChoiceName;
		public int OptionID;

		// Methods
		public OptionChoice()
		{
		}

		public OptionChoice(MenuOption menuOption, int choiceID, string choiceName)
		{
			this.OptionID = menuOption.OptionID;
			this.ChoiceID = choiceID;
			this.ChoiceName = choiceName;
		}
	}
}
