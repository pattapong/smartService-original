using System;

namespace SmartService.Order
{
	/// <summary>
	/// Summary description for OrderWaiting.
	/// </summary>
	public class OrderWaiting
	{
		public OrderBillItemWaiting[] Items;
		public int OrderID;
		public string TableName;
	}
}
