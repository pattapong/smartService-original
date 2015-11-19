using System;

namespace SmartService.CheckBill
{
	/// <summary>
	/// Summary description for Invoice.
	/// </summary>
	public class Invoice
	{
		public InvoiceDiscount[] discounts;
		public int employeeID;
		public int invoiceID;
		public string invoiceNote;
		public int orderBillID;
		public int paymentMethodID;
		public InvoicePayment[] payments;
		public int point;
		public int refInvoice;
		public double tax1;
		public double tax2;
		public double totalDiscount;
		public double totalPrice;
		public double totalReceive;


		public Invoice()
		{
			this.invoiceID = 0;
			this.paymentMethodID = 0;
			this.orderBillID = 0;
			this.point = 0;
			this.totalPrice = this.tax1 = this.tax2 = this.totalDiscount = this.totalReceive = 0.0;
			this.employeeID = 0;
			this.refInvoice = 0;
			this.invoiceNote = string.Empty;
			this.discounts = null;
			this.payments = null;
		}


		public Invoice(int invoiceID, int paymentMethodID, int orderBillID, int employeeID)
		{
			this.invoiceID = invoiceID;
			this.paymentMethodID = paymentMethodID;
			this.orderBillID = orderBillID;
			this.point = 0;
			this.totalPrice = this.tax1 = this.tax2 = this.totalDiscount = this.totalReceive = 0.0;
			this.employeeID = employeeID;
			this.refInvoice = 0;
		}

	}
}
