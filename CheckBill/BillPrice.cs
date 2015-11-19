using System;

namespace SmartService.CheckBill
{
	/// <summary>
	/// Summary description for BillPrice.
	/// </summary>
	public class BillPrice
	{
		public double totalPrice;
		public double totalDiscount;
		public double totalTax1;
		public double totalTax2;

		public BillPrice()
		{
			this.totalPrice = 0.0;
			this.totalDiscount = 0.0;
			this.totalTax1 = 0.0;
			this.totalTax2 = 0.0;
		}

		public BillPrice(double totalPrice, double discount, double totalTax1, double totalTax2)
		{
			this.totalPrice = totalPrice;
			this.totalDiscount = discount;
			this.totalTax1 = totalTax1;
			this.totalTax2 = totalTax2;
		}
	}
}
