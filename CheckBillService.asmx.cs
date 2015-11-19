using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Web;
using System.Web.Services;
using SmartService.CheckBill;
using SmartService.Payment;
using SmartService.Utils;


namespace SmartService
{
	/// <summary>
	/// Summary description for CheckBillService.
	/// </summary>
	[WebService(Namespace="http://ws.smartRestaurant.net")]
	public class CheckBillService : System.Web.Services.WebService
	{
		private IContainer components = null;
		public CheckBillService()
		{
			//CODEGEN: This call is required by the ASP.NET Web Services Designer
			InitializeComponent();
		}

		#region Component Designer generated code
		
		//Required by the Web Services Designer 

				
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
		protected override void Dispose(bool disposing)
		{
			if (disposing && (this.components != null))
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		
		#endregion

		[WebMethod]
		public Invoice GetInvoice(int orderBillID)
		{
			SqlConnection connection = null;
			Invoice invoice = null;
			try
			{
				connection = ConnectDB.GetConnection();
				SqlCommand selectCommand = new SqlCommand("getInvoice", connection);
				selectCommand.CommandType = CommandType.StoredProcedure;
				selectCommand.Parameters.Add("@orderBillID", SqlDbType.Int).Value = orderBillID;
				SqlDataReader reader = selectCommand.ExecuteReader();
				if (reader.Read())
				{
					invoice = new Invoice();
					invoice.invoiceID = (int) reader["InvoiceID"];
					invoice.paymentMethodID = (int) reader["PaymentMethodID"];
					invoice.orderBillID = orderBillID;
					invoice.totalPrice = double.Parse(reader["TotalPrice"].ToString());
					invoice.tax1 = double.Parse(reader["Tax1"].ToString());
					invoice.tax2 = double.Parse(reader["Tax2"].ToString());
					invoice.totalDiscount = double.Parse(reader["TotalDiscount"].ToString());
					invoice.totalReceive = double.Parse(reader["TotalReceive"].ToString());
					invoice.employeeID = (int) reader["EmployeeID"];
					invoice.point = (int) reader["Point"];
					invoice.refInvoice = (reader["RefInvoice"] is DBNull) ? 0 : ((int) reader["RefInvoice"]);
					invoice.invoiceNote = (reader["InvoiceNote"] is DBNull) ? string.Empty : ((string) reader["InvoiceNote"]);
					reader.Close();
					selectCommand.CommandText = "getInvoiceDiscount";
					selectCommand.Parameters.Clear();
					selectCommand.Parameters.Add("@invoiceID", SqlDbType.Int).Value = invoice.invoiceID;
					SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
					DataTable dataTable = new DataTable();
					adapter.Fill(dataTable);
					if (dataTable.Rows.Count > 0)
					{
						invoice.discounts = new InvoiceDiscount[dataTable.Rows.Count];
						for (int i = 0; i < dataTable.Rows.Count; i++)
						{
							invoice.discounts[i] = new InvoiceDiscount();
							invoice.discounts[i].invoiceDiscountID = (int) dataTable.Rows[i]["invoiceDiscountID"];
							invoice.discounts[i].invoiceID = (int) dataTable.Rows[i]["invoiceID"];
							invoice.discounts[i].promotionID = (int) dataTable.Rows[i]["proID"];
						}
					}
					else
					{
						invoice.discounts = null;
					}
					selectCommand.CommandText = "getInvoicePayment";
					adapter = new SqlDataAdapter(selectCommand);
					dataTable.Clear();
					adapter.Fill(dataTable);
					if (dataTable.Rows.Count > 0)
					{
						invoice.payments = new InvoicePayment[dataTable.Rows.Count];
						for (int j = 0; j < dataTable.Rows.Count; j++)
						{
							invoice.payments[j] = new InvoicePayment();
							invoice.payments[j].paymentMethodID = (int) dataTable.Rows[j]["paymentMethodID"];
							invoice.payments[j].receive = double.Parse(dataTable.Rows[j]["receive"].ToString());
						}
						return invoice;
					}
					invoice.payments = null;
					return invoice;
				}
				reader.Close();
			}
			catch (Exception)
			{
				return new Invoice(0, 0, 0, 0);
			}
			finally
			{
				if (connection != null)
				{
					connection.Close();
				}
			}
			return invoice;
		}

		[WebMethod]
		public Invoice GetInvoiceByID(int invoiceID)
		{
			SqlConnection connection = null;
			Invoice invoice = null;
			try
			{
				connection = ConnectDB.GetConnection();
				SqlCommand selectCommand = new SqlCommand("getInvoiceByID", connection);
				selectCommand.CommandType = CommandType.StoredProcedure;
				selectCommand.Parameters.Add("@invoiceID", SqlDbType.Int).Value = invoiceID;
				SqlDataReader reader = selectCommand.ExecuteReader();
				if (reader.Read())
				{
					invoice = new Invoice();
					invoice.invoiceID = (int) reader["InvoiceID"];
					invoice.paymentMethodID = (int) reader["PaymentMethodID"];
					invoice.orderBillID = (int) reader["OrderBillID"];
					invoice.totalPrice = double.Parse(reader["TotalPrice"].ToString());
					invoice.tax1 = double.Parse(reader["Tax1"].ToString());
					invoice.tax2 = double.Parse(reader["Tax2"].ToString());
					invoice.totalDiscount = double.Parse(reader["TotalDiscount"].ToString());
					invoice.totalReceive = double.Parse(reader["TotalReceive"].ToString());
					invoice.employeeID = (int) reader["EmployeeID"];
					invoice.point = (int) reader["Point"];
					invoice.refInvoice = (reader["RefInvoice"] is DBNull) ? 0 : ((int) reader["RefInvoice"]);
					invoice.invoiceNote = (reader["InvoiceNote"] is DBNull) ? string.Empty : ((string) reader["InvoiceNote"]);
					reader.Close();
					selectCommand.CommandText = "getInvoiceDiscount";
					selectCommand.Parameters.Clear();
					selectCommand.Parameters.Add("@invoiceID", SqlDbType.Int).Value = invoice.invoiceID;
					SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
					DataTable dataTable = new DataTable();
					adapter.Fill(dataTable);
					if (dataTable.Rows.Count > 0)
					{
						invoice.discounts = new InvoiceDiscount[dataTable.Rows.Count];
						for (int i = 0; i < dataTable.Rows.Count; i++)
						{
							invoice.discounts[i] = new InvoiceDiscount();
							invoice.discounts[i].invoiceDiscountID = (int) dataTable.Rows[i]["invoiceDiscountID"];
							invoice.discounts[i].invoiceID = (int) dataTable.Rows[i]["invoiceID"];
							invoice.discounts[i].promotionID = (int) dataTable.Rows[i]["proID"];
						}
					}
					else
					{
						invoice.discounts = null;
					}
					selectCommand.CommandText = "getInvoicePayment";
					adapter = new SqlDataAdapter(selectCommand);
					dataTable.Clear();
					adapter.Fill(dataTable);
					if (dataTable.Rows.Count > 0)
					{
						invoice.payments = new InvoicePayment[dataTable.Rows.Count];
						for (int j = 0; j < dataTable.Rows.Count; j++)
						{
							invoice.payments[j] = new InvoicePayment();
							invoice.payments[j].paymentMethodID = (int) dataTable.Rows[j]["paymentMethodID"];
							invoice.payments[j].receive = double.Parse(dataTable.Rows[j]["receive"].ToString());
						}
						return invoice;
					}
					invoice.payments = null;
					return invoice;
				}
				reader.Close();
			}
			catch (Exception)
			{
				return new Invoice(0, 0, 0, 0);
			}
			finally
			{
				if (connection != null)
				{
					connection.Close();
				}
			}
			return invoice;
		}

		[WebMethod]
		public string SendInvoice(Invoice invoice)
		{
			if (invoice == null)
			{
				return "Wrong invoice";
			}
			int num = 0;
			SqlConnection connection = null;
			SqlTransaction transaction = null;
			try
			{
				connection = ConnectDB.GetConnection();
				transaction = connection.BeginTransaction();
				SqlCommand selectCommand = new SqlCommand();
				selectCommand.Connection = connection;
				selectCommand.Transaction = transaction;
				selectCommand.CommandType = CommandType.StoredProcedure;
				if (invoice.invoiceID == 0)
				{
					selectCommand.CommandText = "insertInvoice";
				}
				else
				{
					selectCommand.CommandText = "updateInvoice";
					selectCommand.Parameters.Add("@invoiceID", SqlDbType.Int).Value = invoice.invoiceID;
				}
				selectCommand.Parameters.Add("@methodID", SqlDbType.Int).Value = invoice.paymentMethodID;
				selectCommand.Parameters.Add("@orderBillID", SqlDbType.Int).Value = invoice.orderBillID;
				selectCommand.Parameters.Add("@totalPrice", SqlDbType.Decimal).Value = invoice.totalPrice;
				selectCommand.Parameters.Add("@tax1", SqlDbType.Decimal).Value = invoice.tax1;
				selectCommand.Parameters.Add("@tax2", SqlDbType.Decimal).Value = invoice.tax2;
				selectCommand.Parameters.Add("@totalDiscount", SqlDbType.Decimal).Value = invoice.totalDiscount;
				selectCommand.Parameters.Add("@otherDiscount", SqlDbType.Decimal).Value = 0.0;
				selectCommand.Parameters.Add("@totalReceive", SqlDbType.Decimal).Value = invoice.totalReceive;
				selectCommand.Parameters.Add("@employeeID", SqlDbType.Int).Value = invoice.employeeID;
				selectCommand.Parameters.Add("@point", SqlDbType.Int).Value = invoice.point;
				selectCommand.Parameters.Add("@refInvoice", SqlDbType.Int).Value = (invoice.refInvoice <= 0) ? ((object) DBNull.Value) : ((object) invoice.refInvoice);
				selectCommand.Parameters.Add("@invoiceNote", SqlDbType.NVarChar).Value = (invoice.invoiceNote == null) ? ((object) DBNull.Value) : ((object) invoice.invoiceNote);
				SqlParameter parameter14 = selectCommand.Parameters.Add("@result", SqlDbType.Int);
				parameter14.Direction = ParameterDirection.Output;
				selectCommand.ExecuteNonQuery();
				num = (int) parameter14.Value;
				if (num <= 0)
				{
					transaction.Rollback();
					return "Can't create invoice";
				}
				string str = null;
				string str2 = null;
				if ((invoice.discounts != null) && (invoice.discounts.Length > 0))
				{
					selectCommand.CommandText = "getInvoiceDiscount";
					selectCommand.Parameters.Clear();
					selectCommand.Parameters.Add("@invoiceID", SqlDbType.Int).Value = num;
					SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
					DataTable dataTable = new DataTable();
					adapter.Fill(dataTable);
					if (dataTable.Rows.Count > 0)
					{
						for (int j = 0; j < dataTable.Rows.Count; j++)
						{
							bool flag = false;
							for (int k = 0; k < invoice.discounts.Length; k++)
							{
								if (invoice.discounts[k].promotionID == ((int) dataTable.Rows[j]["proID"]))
								{
									invoice.discounts[k].promotionID = -1;
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								if (str != null)
								{
									str = str + ",";
								}
								str = str + dataTable.Rows[j]["invoiceDiscountID"];
							}
						}
					}
					for (int i = 0; i < invoice.discounts.Length; i++)
					{
						if (invoice.discounts[i].promotionID >= 0)
						{
							if (str2 != null)
							{
								str2 = str2 + ",";
							}
							str2 = str2 + invoice.discounts[i].promotionID;
						}
					}
				}
				else
				{
					str = "0";
				}
				if (str != null)
				{
					selectCommand.CommandText = "deleteInvoiceDiscount";
					selectCommand.Parameters.Clear();
					selectCommand.Parameters.Add("@invoiceID", SqlDbType.Int).Value = num;
					selectCommand.Parameters.Add("@idList", SqlDbType.VarChar).Value = str;
					selectCommand.ExecuteNonQuery();
				}
				if (str2 != null)
				{
					selectCommand.CommandText = "insertInvoiceDiscount";
					selectCommand.Parameters.Clear();
					selectCommand.Parameters.Add("@invoiceID", SqlDbType.Int).Value = num;
					selectCommand.Parameters.Add("@idList", SqlDbType.VarChar).Value = str2;
					selectCommand.ExecuteNonQuery();
				}
				selectCommand.CommandText = "deleteInvoicePayment";
				selectCommand.Parameters.Clear();
				selectCommand.Parameters.Add("@invoiceID", SqlDbType.Int).Value = num;
				selectCommand.ExecuteNonQuery();
				if (invoice.payments != null)
				{
					selectCommand.CommandText = "insertInvoicePayment";
					selectCommand.Parameters.Clear();
					selectCommand.Parameters.Add("@invoiceID", SqlDbType.Int).Value = num;
					SqlParameter parameter16 = selectCommand.Parameters.Add("@paymentMethodID", SqlDbType.Int);
					SqlParameter parameter17 = selectCommand.Parameters.Add("@receive", SqlDbType.Decimal);
					for (int m = 0; m < invoice.payments.Length; m++)
					{
						parameter16.Value = invoice.payments[m].paymentMethodID;
						parameter17.Value = invoice.payments[m].receive;
						selectCommand.ExecuteNonQuery();
					}
				}
				if ((invoice.totalPrice > 0.0) && (((int) ((((invoice.totalPrice + invoice.tax1) + invoice.tax2) - invoice.totalDiscount) * 100.0)) <= ((int) (invoice.totalReceive * 100.0))))
				{
					selectCommand.CommandText = "updateOrderBillClose";
					selectCommand.Parameters.Clear();
					selectCommand.Parameters.Add("@orderBillID", SqlDbType.Int).Value = invoice.orderBillID;
					parameter14 = selectCommand.Parameters.Add("@result", SqlDbType.Int);
					parameter14.Direction = ParameterDirection.Output;
					selectCommand.ExecuteNonQuery();
					switch (((int) parameter14.Value))
					{
						case 0:
							transaction.Rollback();
							return "Can't update order bill status";

						case -1:
							num = -1;
							break;
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
			return num.ToString();
		}

		[WebMethod]
		public BillPrice GetComputeBillPrice(int orderBillID)
		{
			return ComputeBillPrice(orderBillID);
		}

		public static BillPrice ComputeBillPrice(int orderBillID)
		{
			SqlConnection connection = ConnectDB.GetConnection();
			SqlCommand selectCommand = new SqlCommand("getOrderBillComputePrice", connection);
			selectCommand.CommandType = CommandType.StoredProcedure;
			selectCommand.Parameters.Add("@orderBillID", SqlDbType.Int).Value = orderBillID;
			SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
			DataTable dataTable = new DataTable();
			adapter.Fill(dataTable);
			connection.Close();
			if (dataTable.Rows.Count == 0)
			{
				return null;
			}
			double totalPrice = double.Parse(dataTable.Rows[0]["totalPrice"].ToString());
			double discount = double.Parse(dataTable.Rows[0]["totalDiscount"].ToString());
			double num3 = double.Parse(dataTable.Rows[0]["totalTax1"].ToString());
			return new BillPrice(totalPrice, discount, num3, double.Parse(dataTable.Rows[0]["totalTax2"].ToString()));
		}

		[WebMethod]
		public double GetTodayTip()
		{
			SqlConnection connection = ConnectDB.GetConnection();
			SqlCommand command = new SqlCommand("getTipByDate", connection);
			command.CommandType = CommandType.StoredProcedure;
			command.Parameters.Add("@tipDate", SqlDbType.VarChar).Value = DateTime.Today.ToString("yyyy-MM-dd");
			object obj2 = command.ExecuteScalar();
			connection.Close();
			if ((obj2 != null) && !(obj2 is DBNull))
			{
				return double.Parse(obj2.ToString());
			}
			return 0.0;
		}



		[WebMethod]
		public string GetDescription(string id)
		{
			return GetDescriptionByID(id);
		}

		public static string GetDescriptionByID(string id)
		{
			SqlConnection connection = ConnectDB.GetConnection();
			SqlCommand command = new SqlCommand("getDescription", connection);
			command.CommandType = CommandType.StoredProcedure;
			command.Parameters.Add("@descriptionID", SqlDbType.VarChar).Value = id;
			object obj2 = command.ExecuteScalar();
			connection.Close();
			if ((obj2 != null) && !(obj2 is DBNull))
			{
				return obj2.ToString();
			}
			return null;
		}


	}
}
