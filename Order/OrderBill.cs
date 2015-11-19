using System;
using System.Data;
using System.Data.SqlClient;

namespace SmartService.Order
{
	/// <summary>
	/// Summary description for OrderBill.
	/// </summary>
	public class OrderBill
	{
		public int OrderBillID;
		public int BillID;
		public int EmployeeID;
		public DateTime CloseBillDate;
		public OrderBillItem[] Items;

		public OrderBill()
		{
			this.Items = null;
			this.OrderBillID = 0;
			this.BillID = 0;
			this.EmployeeID = 0;
			this.CloseBillDate = DateTime.MinValue;
		}

		public void AddItem(OrderBillItem item)
		{
			OrderBillItem[] items = this.Items;
			if (items != null)
			{
				this.Items = new OrderBillItem[items.Length + 1];
				for (int i = 0; i < items.Length; i++)
				{
					this.Items[i] = items[i];
				}
			}
			else
			{
				this.Items = new OrderBillItem[1];
			}
			this.Items[this.Items.Length - 1] = item;
		}
		public int OrderBillPrint(int orderBillID)
		{
			SqlConnection conn = SmartService.Utils.ConnectDB.GetConnection();
			SqlCommand command = new SqlCommand();
			command.Connection = conn;
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "updateOrderBillPrint";
			command.Parameters.Add("@orderBillID", SqlDbType.Int).Value = orderBillID;
			SqlParameter parameter2 = command.Parameters.Add("@result", SqlDbType.Int);
			parameter2.Direction = ParameterDirection.Output;
			command.ExecuteNonQuery();
			return (int) parameter2.Value;
		}


	}
}
