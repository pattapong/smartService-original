using System;

namespace SmartService.Payment
{
	/// <summary>
	/// Summary description for PaymentMethod.
	/// </summary>
	public class PaymentMethod
	{
		public int paymentMethodID;
		public string paymentMethodName;

		public PaymentMethod()
		{
		}

		public PaymentMethod(int id, string name)
		{
			this.paymentMethodID = id;
			this.paymentMethodName = name;
		}
	}
}
