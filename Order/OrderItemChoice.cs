using System;

namespace SmartService.Order
{
	/// <summary>
	/// Summary description for OrderItemChoice.
	/// </summary>
	public class OrderItemChoice
	{
		public int OptionID;
		public int ChoiceID;
		public OrderItemChoice()
		{
		}

		public OrderItemChoice(int optionID, int choiceID)
		{
			this.OptionID = optionID;
			this.ChoiceID = choiceID;
		}
	}
}
