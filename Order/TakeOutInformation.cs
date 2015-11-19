using System;
using SmartService.Customer;

namespace SmartService.Order
{
	/// <summary>
	/// Summary description for TakeOutInformation.
	/// </summary>
	public class TakeOutInformation
	{
		public CustomerInformation CustInfo;
		public DateTime OrderDate;
		public int OrderID;

		public TakeOutInformation()
		{
			this.OrderID = 0;
			this.OrderDate = DateTime.MinValue;
			this.CustInfo = null;
		}

		public TakeOutInformation(int orderID, DateTime orderDate, CustomerInformation custInfo)
		{
			this.OrderID = orderID;
			this.OrderDate = orderDate;
			this.CustInfo = custInfo;
		}
	}
}
