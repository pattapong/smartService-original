using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Services;
using SmartService.Order;
using SmartService.Utils;
using SmartService.Customer;
using SmartService.Printer;

namespace SmartService
{
	/// <summary>
	/// Summary description for OrderService.
	/// </summary>
	[WebService(Namespace="http://ws.smartRestaurant.net")]
	public class OrderService : System.Web.Services.WebService
	{
		public OrderService()
		{
			//CODEGEN: This call is required by the ASP.NET Web Services Designer
			InitializeComponent();
		}

		#region Component Designer generated code
		
		//Required by the Web Services Designer 
		private IContainer components = null;
				
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);		
		}
		
		#endregion
		/// <summary>
		/// Take Order
		/// </summary>
		[WebMethod]
		public OrderInformation GetOrder(int tableID)
		{
			SqlConnection connection = null;
			OrderInformation information2;
			try
			{
				int num;
				int num2;
				int num3;
				connection = ConnectDB.GetConnection();
				SqlCommand selectCommand = new SqlCommand("getOrderInformation", connection);
				selectCommand.CommandType = CommandType.StoredProcedure;
				selectCommand.Parameters.Add("@tableID", SqlDbType.Int).Value = tableID;
				SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
				DataSet dataSet = new DataSet();
				adapter.Fill(dataSet);
				DataTable table = dataSet.Tables[0];
				if (table.Rows.Count <= 0)
				{
					return null;
				}
				OrderInformation information = new OrderInformation();
				DataRow row = table.Rows[0];
				information.OrderID = (int) row["OrderID"];
				information.OrderTime = (DateTime) row["OrderTime"];
				information.TableID = (int) row["TableID"];
				information.EmployeeID = (int) row["EmployeeID"];
				information.NumberOfGuest = (int) row["NumberOfGuest"];
				information.CloseOrderDate = (row["CloseOrderDate"] is DBNull) ? AppParameter.MinDateTime : ((DateTime) row["CloseOrderDate"]);
				information.CreateDate = (DateTime) row["CreateDate"];
				table = dataSet.Tables[1];
				if (table.Rows.Count <= 0)
				{
					information.Bills = null;
					return information;
				}
				information.Bills = new OrderBill[table.Rows.Count];
				for (num = 0; num < table.Rows.Count; num++)
				{
					information.Bills[num] = new OrderBill();
					row = table.Rows[num];
					information.Bills[num].OrderBillID = (int) row["OrderBillID"];
					information.Bills[num].BillID = (int) row["BillID"];
					information.Bills[num].EmployeeID = (int) row["EmployeeID"];
					information.Bills[num].Items = null;
					information.Bills[num].CloseBillDate = (row["CloseBillDate"] is DBNull) ? AppParameter.MinDateTime : ((DateTime) row["CloseBillDate"]);
				}
				table = dataSet.Tables[2];
				if (table.Rows.Count <= 0)
				{
					return information;
				}
				for (num = 0; num < table.Rows.Count; num++)
				{
					row = table.Rows[num];
					int num5 = (int) row["OrderBillID"];
					num2 = 0;
					while (num2 < information.Bills.Length)
					{
						if (information.Bills[num2].OrderBillID == num5)
						{
							OrderBill bill = information.Bills[num2];
							if (bill.Items == null)
							{
								bill.Items = new OrderBillItem[1];
							}
							else
							{
								OrderBillItem[] items = bill.Items;
								bill.Items = new OrderBillItem[bill.Items.Length + 1];
								num3 = 0;
								while (num3 < items.Length)
								{
									bill.Items[num3] = items[num3];
									num3++;
								}
							}
							OrderBillItem item = new OrderBillItem();
							item.BillDetailID = (int) row["BillDetailID"];
							item.MenuID = (int) row["MenuID"];
							item.Unit = (int) row["Unit"];
							item.Status = (byte) row["Status"];
							item.Message = (row["Message"] is DBNull) ? null : ((string) row["Message"]);
							item.ServeTime = (row["ServeTime"] is DBNull) ? AppParameter.MinDateTime : ((DateTime) row["ServeTime"]);
							item.CancelReasonID = (int) row["CancelReasonID"];
							item.EmployeeID = (int) row["EmployeeID"];
							item.ChangeFlag = false;
							item.ItemChoices = null;
							bill.Items[bill.Items.Length - 1] = item;
							break;
						}
						num2++;
					}
				}
				table = dataSet.Tables[3];
				if (table.Rows.Count <= 0)
				{
					return information;
				}
				for (num = 0; num < table.Rows.Count; num++)
				{
					row = table.Rows[num];
					int num6 = (int) row["BillDetailID"];
					for (num2 = 0; num2 < information.Bills.Length; num2++)
					{
						if ((information.Bills[num2] != null) && (information.Bills[num2].Items != null))
						{
							for (num3 = 0; num3 < information.Bills[num2].Items.Length; num3++)
							{
								if (information.Bills[num2].Items[num3].BillDetailID == num6)
								{
									OrderBillItem item2 = information.Bills[num2].Items[num3];
									if (item2.ItemChoices == null)
									{
										item2.ItemChoices = new OrderItemChoice[1];
										item2.DefaultOption = false;
									}
									else
									{
										OrderItemChoice[] itemChoices = item2.ItemChoices;
										item2.ItemChoices = new OrderItemChoice[item2.ItemChoices.Length + 1];
										for (int i = 0; i < itemChoices.Length; i++)
										{
											item2.ItemChoices[i] = itemChoices[i];
										}
									}
									OrderItemChoice choice = new OrderItemChoice();
									choice.OptionID = (int) row["OptionID"];
									choice.ChoiceID = (int) row["ChoiceID"];
									item2.ItemChoices[item2.ItemChoices.Length - 1] = choice;
									break;
								}
							}
						}
					}
				}
				information2 = information;
			}
			catch (Exception)
			{
				information2 = null;
			}
			finally
			{
				if (connection != null)
				{
					connection.Close();
				}
			}
			return information2;
		}

		[WebMethod]
		public OrderInformation GetOrderByOrderID(int OrderID)
		{
			SqlConnection connection = null;
			OrderInformation information2;
			try
			{
				int num;
				int num2;
				int num3;
				connection = ConnectDB.GetConnection();
				SqlCommand selectCommand = new SqlCommand("getOrderInformationByOrderID", connection);
				selectCommand.CommandType = CommandType.StoredProcedure;
				selectCommand.Parameters.Add("@orderID", SqlDbType.Int).Value = OrderID;
				SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
				DataSet dataSet = new DataSet();
				adapter.Fill(dataSet);
				DataTable table = dataSet.Tables[0];
				if (table.Rows.Count <= 0)
				{
					return null;
				}
				OrderInformation information = new OrderInformation();
				DataRow row = table.Rows[0];
				information.OrderID = (int) row["OrderID"];
				information.OrderTime = (DateTime) row["OrderTime"];
				information.TableID = (int) row["TableID"];
				information.EmployeeID = (int) row["EmployeeID"];
				information.NumberOfGuest = (int) row["NumberOfGuest"];
				information.CloseOrderDate = (row["CloseOrderDate"] is DBNull) ? AppParameter.MinDateTime : ((DateTime) row["CloseOrderDate"]);
				information.CreateDate = (DateTime) row["CreateDate"];
				table = dataSet.Tables[1];
				if (table.Rows.Count <= 0)
				{
					information.Bills = null;
					return information;
				}
				information.Bills = new OrderBill[table.Rows.Count];
				for (num = 0; num < table.Rows.Count; num++)
				{
					information.Bills[num] = new OrderBill();
					row = table.Rows[num];
					information.Bills[num].OrderBillID = (int) row["OrderBillID"];
					information.Bills[num].BillID = (int) row["BillID"];
					information.Bills[num].EmployeeID = (int) row["EmployeeID"];
					information.Bills[num].Items = null;
					information.Bills[num].CloseBillDate = (row["CloseBillDate"] is DBNull) ? AppParameter.MinDateTime : ((DateTime) row["CloseBillDate"]);
				}
				table = dataSet.Tables[2];
				if (table.Rows.Count <= 0)
				{
					return information;
				}
				for (num = 0; num < table.Rows.Count; num++)
				{
					row = table.Rows[num];
					int num5 = (int) row["OrderBillID"];
					num2 = 0;
					while (num2 < information.Bills.Length)
					{
						if (information.Bills[num2].OrderBillID == num5)
						{
							OrderBill bill = information.Bills[num2];
							if (bill.Items == null)
							{
								bill.Items = new OrderBillItem[1];
							}
							else
							{
								OrderBillItem[] items = bill.Items;
								bill.Items = new OrderBillItem[bill.Items.Length + 1];
								num3 = 0;
								while (num3 < items.Length)
								{
									bill.Items[num3] = items[num3];
									num3++;
								}
							}
							OrderBillItem item = new OrderBillItem();
							item.BillDetailID = (int) row["BillDetailID"];
							item.MenuID = (int) row["MenuID"];
							item.Unit = (int) row["Unit"];
							item.Status = (byte) row["Status"];
							item.Message = (row["Message"] is DBNull) ? null : ((string) row["Message"]);
							item.ServeTime = (row["ServeTime"] is DBNull) ? AppParameter.MinDateTime : ((DateTime) row["ServeTime"]);
							item.CancelReasonID = (int) row["CancelReasonID"];
							item.EmployeeID = (int) row["EmployeeID"];
							item.ChangeFlag = false;
							item.ItemChoices = null;
							bill.Items[bill.Items.Length - 1] = item;
							break;
						}
						num2++;
					}
				}
				table = dataSet.Tables[3];
				if (table.Rows.Count <= 0)
				{
					return information;
				}
				for (num = 0; num < table.Rows.Count; num++)
				{
					row = table.Rows[num];
					int num6 = (int) row["BillDetailID"];
					for (num2 = 0; num2 < information.Bills.Length; num2++)
					{
						if ((information.Bills[num2] != null) && (information.Bills[num2].Items != null))
						{
							for (num3 = 0; num3 < information.Bills[num2].Items.Length; num3++)
							{
								if (information.Bills[num2].Items[num3].BillDetailID == num6)
								{
									OrderBillItem item2 = information.Bills[num2].Items[num3];
									if (item2.ItemChoices == null)
									{
										item2.ItemChoices = new OrderItemChoice[1];
										item2.DefaultOption = false;
									}
									else
									{
										OrderItemChoice[] itemChoices = item2.ItemChoices;
										item2.ItemChoices = new OrderItemChoice[item2.ItemChoices.Length + 1];
										for (int i = 0; i < itemChoices.Length; i++)
										{
											item2.ItemChoices[i] = itemChoices[i];
										}
									}
									OrderItemChoice choice = new OrderItemChoice();
									choice.OptionID = (int) row["OptionID"];
									choice.ChoiceID = (int) row["ChoiceID"];
									item2.ItemChoices[item2.ItemChoices.Length - 1] = choice;
									break;
								}
							}
						}
					}
				}
				information2 = information;
			}
			catch (Exception)
			{
				information2 = null;
			}
			finally
			{
				if (connection != null)
				{
					connection.Close();
				}
			}
			return information2;
		}

		[WebMethod]
		public int[] GetTableReference(int orderID)
		{
			SqlConnection connection = null;
			int[] numArray;
			try
			{
				connection = ConnectDB.GetConnection();
				SqlCommand selectCommand = new SqlCommand();
				selectCommand.Connection = connection;
				selectCommand.CommandType = CommandType.StoredProcedure;
				selectCommand.CommandText = "getTableRef";
				selectCommand.Parameters.Add("@orderID", SqlDbType.Int).Value = orderID;
				SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
				DataTable dataTable = new DataTable();
				adapter.Fill(dataTable);
				if (dataTable.Rows.Count == 0)
				{
					return null;
				}
				numArray = new int[dataTable.Rows.Count];
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					numArray[i] = (int) dataTable.Rows[i]["TableID"];
				}
			}
			catch (Exception)
			{
				return null;
			}
			finally
			{
				if (connection != null)
				{
					connection.Close();
				}
			}
			return numArray;
		}

		[WebMethod]
		public int SetTableReference(int orderID, int[] tableIDList)
		{
			SqlConnection connection = null;
			int num = 0;
			try
			{
				connection = ConnectDB.GetConnection();
				SqlCommand command = new SqlCommand();
				command.Connection = connection;
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "insertTableRef";
				string str = "";
				if (tableIDList != null)
				{
					for (int i = 0; i < tableIDList.Length; i++)
					{
						if (str != "")
						{
							str = str + ",";
						}
						str = str + tableIDList[i].ToString();
					}
				}
				command.Parameters.Add("@orderID", SqlDbType.Int).Value = orderID;
				command.Parameters.Add("@TableList", SqlDbType.VarChar).Value = str;
				SqlParameter parameter3 = command.Parameters.Add("@result", SqlDbType.Int);
				parameter3.Direction = ParameterDirection.Output;
				command.ExecuteNonQuery();
				num = (int) parameter3.Value;
			}
			catch (Exception)
			{
				return -1;
			}
			finally
			{
				if (connection != null)
				{
					connection.Close();
				}
			}
			return num;
		}

		[WebMethod]
		public string SendOrder(OrderInformation orderInfo, int CustID, string CustFullName)
		{
			return this.SendOrderPrint(orderInfo, CustID, CustFullName, true);
		}

		[WebMethod]
		public string SendOrderPrint(OrderInformation orderInfo, int CustID, string CustFullName, bool print)
		{
			int num = 0;
			ArrayList list = new ArrayList();
			ArrayList list2 = new ArrayList();
			string[] billDetailID = null;
			if (orderInfo == null)
			{
				return "No data";
			}
			SqlConnection connection = null;
			SqlTransaction transaction = null;
			try
			{
				string str;
				connection = ConnectDB.GetConnection();
				transaction = connection.BeginTransaction();
				SqlCommand command = new SqlCommand();
				command.Connection = connection;
				command.Transaction = transaction;
				command.CommandType = CommandType.StoredProcedure;
				if (orderInfo.OrderID == 0)
				{
					command.CommandText = "insertOrder";
					num = 1;
				}
				else
				{
					command.CommandText = "updateOrder";
					command.Parameters.Add("@orderID", SqlDbType.Int).Value = orderInfo.OrderID;
				}
				command.Parameters.Add("@orderTime", SqlDbType.DateTime).Value = orderInfo.OrderTime;
				command.Parameters.Add("@tableID", SqlDbType.Int).Value = orderInfo.TableID;
				command.Parameters.Add("@employeeID", SqlDbType.Int).Value = orderInfo.EmployeeID;
				command.Parameters.Add("@numberOfGuest", SqlDbType.Int).Value = orderInfo.NumberOfGuest;
				SqlParameter parameter6 = command.Parameters.Add("@closeOrderDate", SqlDbType.DateTime);
				if (orderInfo.CloseOrderDate.CompareTo(AppParameter.MinDateTime) <= 0)
				{
					parameter6.Value = DBNull.Value;
				}
				else
				{
					parameter6.Value = orderInfo.CloseOrderDate;
				}
				SqlParameter parameter7 = command.Parameters.Add("@result", SqlDbType.Int);
				parameter7.Direction = ParameterDirection.Output;
				command.ExecuteNonQuery();
				orderInfo.OrderID = (int) parameter7.Value;
				if (orderInfo.OrderID == 0)
				{
					transaction.Rollback();
					return "Can't insert order info";
				}
				list2.Add(orderInfo.OrderID);
				if (orderInfo.Bills != null)
				{
					command.Parameters.Clear();
					SqlParameter parameter8 = command.Parameters.Add("@orderBillID", SqlDbType.Int);
					SqlParameter parameter9 = command.Parameters.Add("@billID", SqlDbType.Int);
					SqlParameter parameter = command.Parameters.Add("@orderID", SqlDbType.Int);
					SqlParameter parameter4 = command.Parameters.Add("@employeeID", SqlDbType.Int);
					SqlParameter parameter10 = command.Parameters.Add("@closeBillDate", SqlDbType.DateTime);
					parameter7 = command.Parameters.Add("@result", SqlDbType.Int);
					parameter7.Direction = ParameterDirection.Output;
					SqlCommand command2 = new SqlCommand();
					command2.Connection = connection;
					command2.Transaction = transaction;
					command2.CommandType = CommandType.StoredProcedure;
					SqlParameter parameter11 = command2.Parameters.Add("@billDetailID", SqlDbType.Int);
					SqlParameter parameter12 = command2.Parameters.Add("@orderBillID", SqlDbType.Int);
					SqlParameter parameter13 = command2.Parameters.Add("@menuID", SqlDbType.Int);
					SqlParameter parameter14 = command2.Parameters.Add("@unit", SqlDbType.Int);
					SqlParameter parameter15 = command2.Parameters.Add("@status", SqlDbType.TinyInt);
					SqlParameter parameter16 = command2.Parameters.Add("@message", SqlDbType.NText);
					SqlParameter parameter17 = command2.Parameters.Add("@serveTime", SqlDbType.DateTime);
					SqlParameter parameter18 = command2.Parameters.Add("@cancelReasonID", SqlDbType.Int);
					SqlParameter parameter19 = command2.Parameters.Add("@employeeID", SqlDbType.Int);
					SqlParameter parameter20 = command2.Parameters.Add("@result", SqlDbType.Int);
					parameter20.Direction = ParameterDirection.Output;
					SqlCommand command3 = new SqlCommand();
					command3.Connection = connection;
					command3.Transaction = transaction;
					command3.CommandType = CommandType.StoredProcedure;
					SqlParameter parameter21 = command3.Parameters.Add("@billDetailID", SqlDbType.Int);
					SqlParameter parameter22 = command3.Parameters.Add("@choiceID", SqlDbType.Int);
					command3.Parameters.Add("@result", SqlDbType.Int).Direction = ParameterDirection.Output;
					for (int i = 0; i < orderInfo.Bills.Length; i++)
					{
						OrderBill bill = orderInfo.Bills[i];
						if (bill.OrderBillID == 0)
						{
							command.CommandText = "insertOrderBill";
							if (command.Parameters.Contains(parameter8))
							{
								command.Parameters.Remove(parameter8);
							}
							if (command.Parameters.Contains(parameter10))
							{
								command.Parameters.Remove(parameter10);
							}
							if (!command.Parameters.Contains(parameter))
							{
								command.Parameters.Add(parameter);
							}
							parameter.Value = orderInfo.OrderID;
							if (num != 1)
							{
								num = 2;
							}
						}
						else
						{
							command.CommandText = "updateOrderBill";
							if (!command.Parameters.Contains(parameter8))
							{
								command.Parameters.Add(parameter8);
							}
							if (!command.Parameters.Contains(parameter10))
							{
								command.Parameters.Add(parameter10);
							}
							if (command.Parameters.Contains(parameter))
							{
								command.Parameters.Remove(parameter);
							}
							parameter8.Value = bill.OrderBillID;
							if (bill.CloseBillDate.CompareTo(AppParameter.MinDateTime) <= 0)
							{
								parameter10.Value = DBNull.Value;
							}
							else
							{
								parameter10.Value = bill.CloseBillDate;
							}
						}
						parameter4.Value = bill.EmployeeID;
						parameter9.Value = bill.BillID;
						command.ExecuteNonQuery();
						bill.OrderBillID = (int) parameter7.Value;
						if (bill.OrderBillID == 0)
						{
							transaction.Rollback();
							return "Can't insert bill";
						}
						if (bill.Items != null)
						{
							parameter12.Value = bill.OrderBillID;
							for (int j = 0; j < bill.Items.Length; j++)
							{
								OrderBillItem item = bill.Items[j];
								if (item.BillDetailID == 0)
								{
									command2.CommandText = "insertOrderBillDetail";
									if (command2.Parameters.Contains(parameter11))
									{
										command2.Parameters.Remove(parameter11);
									}
									if (command2.Parameters.Contains(parameter15))
									{
										command2.Parameters.Remove(parameter15);
									}
									if (command2.Parameters.Contains(parameter17))
									{
										command2.Parameters.Remove(parameter17);
									}
									if (command2.Parameters.Contains(parameter18))
									{
										command2.Parameters.Remove(parameter18);
									}
									if ((num != 1) && (num != 2))
									{
										num = 3;
									}
								}
								else
								{
									command2.CommandText = "updateOrderBillDetail";
									if (!command2.Parameters.Contains(parameter11))
									{
										command2.Parameters.Add(parameter11);
									}
									if (!command2.Parameters.Contains(parameter15))
									{
										command2.Parameters.Add(parameter15);
									}
									if (!command2.Parameters.Contains(parameter17))
									{
										command2.Parameters.Add(parameter17);
									}
									if (!command2.Parameters.Contains(parameter18))
									{
										command2.Parameters.Add(parameter18);
									}
									parameter11.Value = item.BillDetailID;
									parameter15.Value = item.Status;
									if (item.ServeTime.CompareTo(AppParameter.MinDateTime) <= 0)
									{
										parameter17.Value = DBNull.Value;
									}
									else
									{
										parameter17.Value = item.ServeTime;
									}
									if (item.CancelReasonID == 0)
									{
										parameter18.Value = DBNull.Value;
									}
									else
									{
										parameter18.Value = item.CancelReasonID;
									}
								}
								parameter13.Value = item.MenuID;
								parameter14.Value = item.Unit;
								if ((item.Message == null) || (item.Message.Length == 0))
								{
									parameter16.Value = DBNull.Value;
								}
								else
								{
									parameter16.Value = item.Message;
								}
								parameter19.Value = item.EmployeeID;
								command2.ExecuteNonQuery();
								item.BillDetailID = (int) parameter20.Value;
								if (item.BillDetailID == 0)
								{
									transaction.Rollback();
									return "Can't insert item";
								}
								switch (num)
								{
									case 1:
									case 2:
									case 3:
										if ((item.Status != 0) || AppParameter.IsDemo())
										{
											list.Add(item.BillDetailID.ToString());
										}
										if (num == 3)
										{
											num = 0;
										}
										break;

									default:
										if (((command2.CommandText == "updateOrderBillDetail") && item.ChangeFlag) && ((item.Status != 0) || AppParameter.IsDemo()))
										{
											list.Add(item.BillDetailID.ToString());
										}
										break;
								}
								command3.CommandText = "deleteOrderBillOption";
								if (command3.Parameters.Contains(parameter22))
								{
									command3.Parameters.Remove(parameter22);
								}
								parameter21.Value = item.BillDetailID;
								command3.ExecuteNonQuery();
								if (!item.DefaultOption && (item.ItemChoices != null))
								{
									command3.Parameters.Add(parameter22);
									for (int k = 0; k < item.ItemChoices.Length; k++)
									{
										OrderItemChoice choice = item.ItemChoices[k];
										command3.CommandText = "insertOrderBillOption";
										parameter22.Value = choice.ChoiceID;
										command3.ExecuteNonQuery();
										if (((int) parameter20.Value) == 0)
										{
											transaction.Rollback();
											return "Can't insert choice";
										}
									}
								}
							}
						}
						if (num == 2)
						{
							num = 0;
						}
					}
				}
				transaction.Commit();
				transaction = null;
				if ((CustFullName != null) && (CustFullName != ""))
				{
					str = this.SendTakeOut(orderInfo.OrderID, CustID, CustFullName);
					if (str != null)
					{
						return str;
					}
				}
				if (print)
				{
					billDetailID = (string[]) list.ToArray(typeof(string));
					this.PrintKitchen(billDetailID);
					for (int m = 0; m < list2.Count; m++)
					{
						OrderInformation.OrderPrintKitchen((int) list2[m]);
					}
				}
				str = this.CheckOrderClose(orderInfo.OrderID);
				if (str != null)
				{
					return str;
				}
			}
			catch (Exception exception)
			{
				if (transaction != null)
				{
					transaction.Rollback();
				}
				return exception.ToString();
			}
			finally
			{
				if (connection != null)
				{
					connection.Close();
				}
			}
			return orderInfo.OrderID.ToString();
		}

		[WebMethod]
		public string SendOrderBill(OrderBill Bill)
		{
			if ((Bill == null) || Bill.BillID.Equals(null))
			{
				return "No data";
			}
			SqlConnection connection = null;
			SqlTransaction transaction = null;
			try
			{
				connection = ConnectDB.GetConnection();
				transaction = connection.BeginTransaction();
				SqlCommand command = new SqlCommand();
				command.Connection = connection;
				command.Transaction = transaction;
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "updateOrderBill";
				command.Parameters.Add("@orderBillID", SqlDbType.Int).Value = Bill.OrderBillID;
				command.Parameters.Add("@billID", SqlDbType.Int).Value = Bill.BillID;
				command.Parameters.Add("@employeeID", SqlDbType.Int).Value = Bill.EmployeeID;
				SqlParameter parameter4 = command.Parameters.Add("@closeBillDate", SqlDbType.DateTime);
				if (Bill.CloseBillDate.CompareTo(AppParameter.MinDateTime) <= 0)
				{
					parameter4.Value = DBNull.Value;
				}
				else
				{
					parameter4.Value = Bill.CloseBillDate;
				}
				SqlParameter parameter5 = command.Parameters.Add("@result", SqlDbType.Int);
				parameter5.Direction = ParameterDirection.Output;
				command.ExecuteNonQuery();
				if (((int) parameter5.Value) == 0)
				{
					transaction.Rollback();
					return "Can't update order bill";
				}
				if (Bill.Items != null)
				{
					SqlCommand command2 = new SqlCommand();
					command2.Connection = connection;
					command2.Transaction = transaction;
					command2.CommandType = CommandType.StoredProcedure;
					SqlParameter parameter6 = command2.Parameters.Add("@billDetailID", SqlDbType.Int);
					SqlParameter parameter7 = command2.Parameters.Add("@orderBillID", SqlDbType.Int);
					SqlParameter parameter8 = command2.Parameters.Add("@menuID", SqlDbType.Int);
					SqlParameter parameter9 = command2.Parameters.Add("@unit", SqlDbType.Int);
					SqlParameter parameter10 = command2.Parameters.Add("@status", SqlDbType.TinyInt);
					SqlParameter parameter11 = command2.Parameters.Add("@message", SqlDbType.NText);
					SqlParameter parameter12 = command2.Parameters.Add("@serveTime", SqlDbType.DateTime);
					SqlParameter parameter13 = command2.Parameters.Add("@cancelReasonID", SqlDbType.Int);
					SqlParameter parameter14 = command2.Parameters.Add("@employeeID", SqlDbType.Int);
					SqlParameter parameter15 = command2.Parameters.Add("@result", SqlDbType.Int);
					parameter15.Direction = ParameterDirection.Output;
					SqlCommand command3 = new SqlCommand();
					command3.Connection = connection;
					command3.Transaction = transaction;
					command3.CommandType = CommandType.StoredProcedure;
					SqlParameter parameter16 = command3.Parameters.Add("@billDetailID", SqlDbType.Int);
					SqlParameter parameter17 = command3.Parameters.Add("@choiceID", SqlDbType.Int);
					command3.Parameters.Add("@result", SqlDbType.Int).Direction = ParameterDirection.Output;
					parameter7.Value = Bill.OrderBillID;
					for (int i = 0; i < Bill.Items.Length; i++)
					{
						OrderBillItem item = Bill.Items[i];
						command2.CommandText = "updateOrderBillDetail";
						parameter6.Value = item.BillDetailID;
						parameter10.Value = item.Status;
						if (item.ServeTime.CompareTo(AppParameter.MinDateTime) <= 0)
						{
							parameter12.Value = DBNull.Value;
						}
						else
						{
							parameter12.Value = item.ServeTime;
						}
						if (item.CancelReasonID == 0)
						{
							parameter13.Value = DBNull.Value;
						}
						else
						{
							parameter13.Value = item.CancelReasonID;
						}
						parameter8.Value = item.MenuID;
						parameter9.Value = item.Unit;
						if (item.Message == null)
						{
							parameter11.Value = DBNull.Value;
						}
						else
						{
							parameter11.Value = item.Message;
						}
						parameter14.Value = item.EmployeeID;
						command2.ExecuteNonQuery();
						item.BillDetailID = (int) parameter15.Value;
						if (item.BillDetailID == 0)
						{
							transaction.Rollback();
							return "Can't update item";
						}
						command3.CommandText = "deleteOrderBillOption";
						if (command3.Parameters.Contains(parameter17))
						{
							command3.Parameters.Remove(parameter17);
						}
						parameter16.Value = item.BillDetailID;
						command3.ExecuteNonQuery();
						if (!item.DefaultOption && (item.ItemChoices != null))
						{
							command3.Parameters.Add(parameter17);
							for (int j = 0; j < item.ItemChoices.Length; j++)
							{
								OrderItemChoice choice = item.ItemChoices[j];
								command3.CommandText = "insertOrderBillOption";
								parameter17.Value = choice.ChoiceID;
								command3.ExecuteNonQuery();
								if (((int) parameter15.Value) == 0)
								{
									transaction.Rollback();
									return "Can't insert choice";
								}
							}
						}
					}
				}
				transaction.Commit();
				transaction = null;
			}
			catch (Exception exception)
			{
				if (transaction != null)
				{
					transaction.Rollback();
				}
				return exception.ToString();
			}
			finally
			{
				if (connection != null)
				{
					connection.Close();
				}
			}
			return null;
		}

		/// <summary>
		/// TakeOut
		/// </summary>
		[WebMethod]
		public TakeOutInformation[] GetTakeOutList()
		{
			try
			{
				SqlConnection connection = ConnectDB.GetConnection();
				SqlCommand selectCommand = new SqlCommand("getTakeOutList", connection);
				selectCommand.CommandType = CommandType.StoredProcedure;
				SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
				DataTable dataTable = new DataTable();
				adapter.Fill(dataTable);
				connection.Close();
				if (dataTable.Rows.Count <= 0)
				{
					return null;
				}
				TakeOutInformation[] informationArray = new TakeOutInformation[dataTable.Rows.Count];
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					int orderID = (int) dataTable.Rows[i]["orderID"];
					DateTime orderDate = (dataTable.Rows[i]["createdate"] is DBNull) ? AppParameter.MinDateTime : ((DateTime) dataTable.Rows[i]["createdate"]);
					CustomerInformation custInfo = new CustomerInformation();
					custInfo.CustID = (int) dataTable.Rows[i]["CustID"];
					custInfo.FirstName = dataTable.Rows[i]["CustFirstName"].ToString();
					custInfo.MiddleName = dataTable.Rows[i]["CustMiddleName"].ToString();
					custInfo.LastName = dataTable.Rows[i]["CustLastName"].ToString();
					custInfo.Telephone = dataTable.Rows[i]["CustTel"].ToString();
					custInfo.Address = dataTable.Rows[i]["CustAddress"].ToString();
					custInfo.Description = dataTable.Rows[i]["CustDescription"].ToString();
					informationArray[i] = new TakeOutInformation(orderID, orderDate, custInfo);
				}
				return informationArray;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public string SendTakeOut(int OrderID, int CustID, string CustFullName)
		{
			try
			{
				string str = ConnectDB.SplitCustFName(CustFullName);
				string str2 = ConnectDB.SplitCustMName(CustFullName);
				string str3 = ConnectDB.SplitCustLName(CustFullName);
				SqlConnection connection = ConnectDB.GetConnection();
				SqlCommand command = new SqlCommand("insertTakeOut", connection);
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.Add("@orderID", SqlDbType.Int).Value = OrderID;
				command.Parameters.Add("@firstName", SqlDbType.NVarChar).Value = str;
				command.Parameters.Add("@middleName", SqlDbType.NVarChar).Value = str2;
				command.Parameters.Add("@lastName", SqlDbType.NVarChar).Value = str3;
				command.Parameters.Add("@custID", SqlDbType.Int).Value = CustID;
				SqlParameter parameter6 = command.Parameters.Add("@result", SqlDbType.Int);
				parameter6.Direction = ParameterDirection.Output;
				command.ExecuteNonQuery();
				connection.Close();
				parameter6.Value.ToString();
				return null;
			}
			catch (Exception exception)
			{
				return exception.Message.ToString();
			}
		}

		public string CheckOrderClose(int OrderID)
		{
			try
			{
				SqlConnection connection = ConnectDB.GetConnection();
				SqlCommand command = new SqlCommand("checkOrderClose", connection);
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.Add("@orderID", SqlDbType.Int).Value = OrderID;
				SqlParameter parameter2 = command.Parameters.Add("@result", SqlDbType.Int);
				parameter2.Direction = ParameterDirection.Output;
				command.ExecuteNonQuery();
				connection.Close();
				parameter2.Value.ToString();
				return null;
			}
			catch (Exception exception)
			{
				return exception.Message.ToString();
			}
		}


		[WebMethod]
		public int PrintReceipt(int OrderBillID)
		{
			Printer.PrintReceipt.Print(OrderBillID);
			OrderBill bill = new OrderBill();
			return bill.OrderBillPrint(OrderBillID);
		}


		[WebMethod]
		public int PrintBill(int OrderBillID)
		{
			Printer.PrintBill.Print(OrderBillID);
			OrderBill bill = new OrderBill();
			return bill.OrderBillPrint(OrderBillID);
		}


		[WebMethod]
		public int ReprintBill(int OrderBillID)
		{
			Printer.PrintBill.Print(OrderBillID, true);
			OrderBill bill = new OrderBill();
			return bill.OrderBillPrint(OrderBillID);
		}


		private int PrintKitchen(string[] BillDetailID)
		{
			string billDetailID = string.Join(",", BillDetailID);
			Printer.PrintKitchen.Print(billDetailID);
			byte status = 2;
			OrderBillItem item = new OrderBillItem();
			return item.OrderBillItemStatus(billDetailID, status);
		}


		/*private int ServeCustomer(string[] BillDetailID)
		{
			string BillDetailList=string.Join(",",BillDetailID);
			//update status
			Byte StatusServe=3;
			OrderBillItem BillItem = new OrderBillItem();
			int result=BillItem.OrderBillItemStatus(BillDetailList,StatusServe);
			return result;
		}*/

		[WebMethod]
		public CancelReason[] GetCancelReason()
		{
			SqlConnection connection = null;
			CancelReason[] reasonArray2;
			try
			{
				connection = ConnectDB.GetConnection();
				SqlCommand selectCommand = new SqlCommand("getCancelReason", connection);
				selectCommand.CommandType = CommandType.StoredProcedure;
				selectCommand.Parameters.Add("@cancelReasonID", SqlDbType.Int).Value = 0;
				SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
				DataTable dataTable = new DataTable();
				adapter.Fill(dataTable);
				if (dataTable.Rows.Count <= 0)
				{
					return null;
				}
				CancelReason[] reasonArray = new CancelReason[dataTable.Rows.Count];
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					DataRow row = dataTable.Rows[i];
					reasonArray[i] = new CancelReason((int) row["cancelreasonid"], row["reason"].ToString());
				}
				reasonArray2 = reasonArray;
			}
			catch (Exception)
			{
				reasonArray2 = null;
			}
			finally
			{
				if (connection != null)
				{
					connection.Close();
				}
			}
			return reasonArray2;
		}

		[WebMethod]
		public OrderWaiting[] GetBillDetailWaitingList()
		{
			SqlConnection connection = null;
			OrderWaiting[] waitingArray;
			try
			{
				connection = ConnectDB.GetConnection();
				SqlCommand selectCommand = new SqlCommand("getBillDetailWaitingList", connection);
				selectCommand.CommandType = CommandType.StoredProcedure;
				SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
				DataTable dataTable = new DataTable();
				adapter.Fill(dataTable);
				if (dataTable.Rows.Count <= 0)
				{
					return null;
				}
				ArrayList list = new ArrayList();
				ArrayList list2 = new ArrayList();
				OrderWaiting waiting = null;
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					DataRow row = dataTable.Rows[i];
					int num2 = (int) row["orderid"];
					string str = (string) row["tablename"];
					int num3 = (int) row["billdetailid"];
					string str2 = (string) row["menukeyidtext"];
					int num4 = (int) row["unit"];
					string str3 = (row["choicename"] is DBNull) ? null : ((string) row["choicename"]);
					if ((waiting == null) || (waiting.OrderID != num2))
					{
						if ((waiting != null) && (list2.Count > 0))
						{
							waiting.Items = (OrderBillItemWaiting[]) list2.ToArray(typeof(OrderBillItemWaiting));
							list.Add(waiting);
						}
						waiting = new OrderWaiting();
						waiting.OrderID = num2;
						waiting.TableName = str;
						waiting.Items = null;
						list2.Clear();
					}
					OrderBillItemWaiting waiting2 = new OrderBillItemWaiting();
					waiting2.BillDetailID = num3;
					waiting2.MenuKeyID = str2;
					waiting2.Unit = num4;
					waiting2.Choice = str3;
					list2.Add(waiting2);
				}
				if ((waiting != null) && (list2.Count > 0))
				{
					waiting.Items = (OrderBillItemWaiting[]) list2.ToArray(typeof(OrderBillItemWaiting));
					list.Add(waiting);
				}
				waitingArray = (OrderWaiting[]) list.ToArray(typeof(OrderWaiting));
			}
			catch (Exception)
			{
				waitingArray = null;
			}
			finally
			{
				if (connection != null)
				{
					connection.Close();
				}
			}
			return waitingArray;
		}

		[WebMethod]
		public OrderWaiting[] ServeWaitingOrder(int orderID, int billDetailID)
		{
			SqlConnection connection = null;
			try
			{
				connection = ConnectDB.GetConnection();
				SqlCommand command = new SqlCommand("updateOrderServe", connection);
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.Add("@orderID", SqlDbType.Int).Value = orderID;
				command.Parameters.Add("@billDetailID", SqlDbType.Int).Value = billDetailID;
				command.ExecuteNonQuery();
			}
			catch (Exception)
			{
			}
			finally
			{
				if (connection != null)
				{
					connection.Close();
				}
			}
			return this.GetBillDetailWaitingList();
		}

	}
}
