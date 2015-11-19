using System;
using System.Data;
using System.Data.SqlClient;

namespace SmartService.Order
{
	/// <summary>
	/// Summary description for OrderInformation.
	/// </summary>
	public class OrderInformation
	{
		public OrderBill[] Bills;
		public DateTime CloseOrderDate;
		public DateTime CreateDate;
		public int EmployeeID;
		public int NumberOfGuest;
		public int OrderID;
		public DateTime OrderTime;
		public int TableID;

		public OrderInformation()
		{
			this.Bills = null;
			this.CloseOrderDate = DateTime.MinValue;
		}

		public OrderInformation(int orderID, DateTime orderTime, int tableID, int employeeID, int guest, DateTime closeOrderDate, DateTime createDate, int bill)
		{
			this.OrderID = orderID;
			this.OrderTime = orderTime;
			this.TableID = tableID;
			this.EmployeeID = employeeID;
			this.NumberOfGuest = guest;
			this.CloseOrderDate = closeOrderDate;
			this.CreateDate = createDate;
			if (bill > 0)
			{
				this.Bills = new OrderBill[bill];
				for (int i = 0; i < bill; i++)
				{
					this.Bills[i] = new OrderBill();
				}
			}
			else
			{
				this.Bills = null;
			}
		}

		public void SetNumberOfBill(int bill)
		{
			if ((this.Bills == null) || (this.Bills.Length != bill))
			{
				if (bill == 0)
				{
					this.Bills = null;
				}
				else
				{
					OrderBill[] bills = this.Bills;
					this.Bills = new OrderBill[bill];
					for (int i = 0; i < bill; i++)
					{
						if ((bills != null) && (i < bills.Length))
						{
							this.Bills[i] = bills[i];
						}
						else
						{
							this.Bills[i] = new OrderBill();
						}
					}
				}
			}
		}

		public static int OrderPrintKitchen(int orderID)
		{
			SqlConnection conn = SmartService.Utils.ConnectDB.GetConnection();
			SqlCommand command = new SqlCommand();
			command.Connection = conn;
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "updateOrderPrintKitchen";
			command.Parameters.Add("@orderID", SqlDbType.Int).Value = orderID;
			SqlParameter parameter2 = command.Parameters.Add("@result", SqlDbType.Int);
			parameter2.Direction = ParameterDirection.Output;
			command.ExecuteNonQuery();
			return (int) parameter2.Value;
		}
	}
}
