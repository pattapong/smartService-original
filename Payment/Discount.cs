using System;

namespace SmartService.Payment
{
	/// <summary>
	/// Summary description for Discount.
	/// </summary>
	public class Discount
	{
		public double amount;
		public string description;
		public double discountAmount;
		public double discountPercent;
		public int promotionID;
		public int promotionType;

		public Discount()
		{
		}

		public Discount(int promotionID, int promotionType, string description, double amount, double discountPercent, double discountAmount)
		{
			this.promotionID = promotionID;
			this.promotionType = promotionType;
			this.description = description;
			this.amount = amount;
			this.discountPercent = discountPercent;
			this.discountAmount = discountAmount;
		}

	}
}
