using System;

namespace SmartService.Payment
{
	/// <summary>
	/// Summary description for PromotionCard.
	/// </summary>
	public class PromotionCard
	{
		public int		promotionID;
		public string	description;
		public double	amount;

		public PromotionCard()
		{
		}

		public PromotionCard(int proID, string desc, double amount)
		{
			this.promotionID = proID;
			this.description = desc;
			this.amount = amount;
		}
	}
}
