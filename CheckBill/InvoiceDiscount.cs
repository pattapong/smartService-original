using System;

namespace SmartService.CheckBill
{
	/// <summary>
	/// Summary description for InvoiceDiscount.
	/// </summary>
	public class InvoiceDiscount
	{
		public int	invoiceDiscountID;
		public int	invoiceID;
		public int	promotionID;

		public InvoiceDiscount()
		{
			invoiceDiscountID = 0;
			invoiceID = 0;
			promotionID = 0;
		}

		public InvoiceDiscount(int discountID, int inID, int proID)
		{
			invoiceDiscountID = discountID;
			invoiceID = inID;
			promotionID = proID;
		}
	}
}
