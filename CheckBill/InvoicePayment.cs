using System;

namespace SmartService.CheckBill
{
	/// <summary>
	/// Summary description for InvoicePayment.
	/// </summary>
	public class InvoicePayment
	{
		public int		paymentMethodID;
		public double	receive;

		public InvoicePayment()
		{
			paymentMethodID = 0;
			receive = 0;
		}

		public InvoicePayment(int paymentMethodID, int receiev)
		{
			this.paymentMethodID = paymentMethodID;
			this.receive = receiev;
		}
	}
}
