using System;
using System.Data;
using System.Data.SqlClient;

namespace SmartService.Order
{
	/// <summary>
	/// Summary description for OrderBillItem.
	/// </summary>
	public class OrderBillItem
	{
		public int BillDetailID;
		public int CancelReasonID;
		public bool ChangeFlag;
		public bool DefaultOption;
		public int EmployeeID;
		public OrderItemChoice[] ItemChoices;
		public int MenuID;
		public string Message;
		public DateTime ServeTime;
		public byte Status;
		public int Unit;


		public OrderBillItem()
		{
			this.BillDetailID = 0;
			this.DefaultOption = true;
			this.Status = 1;
			this.Message = null;
			this.ServeTime = DateTime.MinValue;
			this.CancelReasonID = 0;
			this.EmployeeID = 0;
			this.ChangeFlag = false;
		}

		public OrderBillItem(int menuID, int unit)
		{
			this.BillDetailID = 0;
			this.MenuID = menuID;
			this.Unit = unit;
			this.DefaultOption = true;
			this.Status = 1;
			this.Message = null;
			this.ServeTime = DateTime.MinValue;
			this.CancelReasonID = 0;
			this.EmployeeID = 0;
			this.ChangeFlag = false;
			this.ItemChoices = null;
		}

		public int OrderBillItemStatus(string BillDetailIDList,Byte status)
		{
			SqlConnection conn = SmartService.Utils.ConnectDB.GetConnection();
			SqlCommand command = new SqlCommand();
			command.Connection = conn;
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "updateOrderBillDetailStatus";
			command.Parameters.Add("@billDetailIDList", SqlDbType.VarChar).Value = BillDetailIDList;
			command.Parameters.Add("@status", SqlDbType.TinyInt).Value = status;
			SqlParameter parameter3 = command.Parameters.Add("@result", SqlDbType.Int);
			parameter3.Direction = ParameterDirection.Output;
			command.ExecuteNonQuery();
			return (int) parameter3.Value;
		}
	}
}
