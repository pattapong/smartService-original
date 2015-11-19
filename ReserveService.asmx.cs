using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using SmartService.Table;
using SmartService.Utils;

namespace SmartService
{
	/// <summary>
	/// Summary description for ReserveService.
	/// </summary>
	[WebService(Namespace="http://ws.smartRestaurant.net")]
	public class ReserveService : System.Web.Services.WebService
	{
		public ReserveService()
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
		[WebMethod]
		public TableReserve[] GetTableReserve(DateTime date)
		{
			SqlConnection connection = null;
			TableReserve[] reserveArray2;
			try
			{
				connection = ConnectDB.GetConnection();
				SqlCommand selectCommand = new SqlCommand("getTableReserveByDate", connection);
				selectCommand.CommandType = CommandType.StoredProcedure;
				selectCommand.Parameters.Add("@date", SqlDbType.DateTime).Value = date;
				SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
				DataTable dataTable = new DataTable();
				adapter.Fill(dataTable);
				if (dataTable.Rows.Count <= 0)
				{
					return null;
				}
				TableReserve[] reserveArray = new TableReserve[dataTable.Rows.Count];
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					reserveArray[i] = new TableReserve();
					reserveArray[i].reserveTableID = (int) dataTable.Rows[i]["TableReserveTransactionID"];
					reserveArray[i].customerID = (int) dataTable.Rows[i]["CustID"];
					reserveArray[i].tableID = (dataTable.Rows[i]["TableID"] is DBNull) ? 0 : ((int) dataTable.Rows[i]["TableID"]);
					reserveArray[i].seat = (int) dataTable.Rows[i]["Seat"];
					reserveArray[i].reserveDate = (DateTime) dataTable.Rows[i]["ReserveDate"];
					reserveArray[i].custFirstName = dataTable.Rows[i]["CustFirstName"].ToString();
					reserveArray[i].custMiddleName = dataTable.Rows[i]["CustMiddleName"].ToString();
					reserveArray[i].custLastName = dataTable.Rows[i]["CustLastName"].ToString();
				}
				reserveArray2 = reserveArray;
			}
			catch (Exception)
			{
				reserveArray2 = null;
			}
			finally
			{
				if (connection != null)
				{
					connection.Close();
				}
			}
			return reserveArray2;
		}

		[WebMethod]
		public string SetTableReserve(TableReserve reserve, string custFullName)
		{
			SqlConnection connection = null;
			string str4;
			try
			{
				SqlParameter parameter;
				string str = ConnectDB.SplitCustFName(custFullName);
				string str2 = ConnectDB.SplitCustMName(custFullName);
				string str3 = ConnectDB.SplitCustLName(custFullName);
				connection = ConnectDB.GetConnection();
				SqlCommand command = new SqlCommand("getCustomerInformation", connection);
				command.CommandType = CommandType.StoredProcedure;
				if (reserve.customerID == 0)
				{
					command.Parameters.Add("@tel", SqlDbType.VarChar).Value = "";
					command.Parameters.Add("@fname", SqlDbType.NVarChar).Value = str;
					command.Parameters.Add("@mname", SqlDbType.NVarChar).Value = str2;
					command.Parameters.Add("@lname", SqlDbType.NVarChar).Value = str3;
					SqlDataReader reader = command.ExecuteReader();
					if (reader.Read())
					{
						reserve.customerID = (int) reader["CustID"];
					}
					reader.Close();
					if (reserve.customerID == 0)
					{
						command.CommandText = "insertCustomer";
						command.Parameters.Clear();
						command.Parameters.Add("@firstname", SqlDbType.NVarChar).Value = str;
						command.Parameters.Add("@middlename", SqlDbType.NVarChar).Value = str2;
						command.Parameters.Add("@lastname", SqlDbType.NVarChar).Value = str3;
						command.Parameters.Add("@telephone", SqlDbType.VarChar).Value = "";
						command.Parameters.Add("@address", SqlDbType.NVarChar).Value = "";
						command.Parameters.Add("@description", SqlDbType.NVarChar).Value = "";
						parameter = command.Parameters.Add("@result", SqlDbType.Int);
						parameter.Direction = ParameterDirection.Output;
						command.ExecuteNonQuery();
						reserve.customerID = (int) parameter.Value;
					}
					if (reserve.customerID == 0)
					{
						return "Customer not found.";
					}
				}
				command.CommandText = "insertTableReserve";
				command.Parameters.Clear();
				SqlParameter parameter8 = command.Parameters.Add("@tableID", SqlDbType.Int);
				if (reserve.tableID > 0)
				{
					parameter8.Value = reserve.tableID;
				}
				else
				{
					parameter8.Value = DBNull.Value;
				}
				command.Parameters.Add("@custID", SqlDbType.Int).Value = reserve.customerID;
				command.Parameters.Add("@reserveDate", SqlDbType.DateTime).Value = reserve.reserveDate;
				command.Parameters.Add("@seat", SqlDbType.Int).Value = reserve.seat;
				parameter = command.Parameters.Add("@result", SqlDbType.Int);
				parameter.Direction = ParameterDirection.Output;
				command.ExecuteNonQuery();
				if (((int) parameter.Value) == 0)
				{
					return "Can't insert reserve table";
				}
				str4 = null;
			}
			catch (Exception exception)
			{
				str4 = exception.ToString();
			}
			finally
			{
				if (connection != null)
				{
					connection.Close();
				}
			}
			return str4;
		}

		[WebMethod]
		public int SetReserveCancel(string reserveID)
		{
			SqlConnection connection = null;
			int num = 0;
			try
			{
				connection = ConnectDB.GetConnection();
				SqlCommand command = new SqlCommand();
				command.Connection = connection;
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "updateReserveCancel";
				command.Parameters.Add("@reserveID", SqlDbType.Int).Value = reserveID;
				SqlParameter parameter2 = command.Parameters.Add("@result", SqlDbType.Int);
				parameter2.Direction = ParameterDirection.Output;
				command.ExecuteNonQuery();
				num = (int) parameter2.Value;
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
		public int SetReserveDinIn(string reserveID, string TableID)
		{
			SqlConnection connection = null;
			int num = 0;
			try
			{
				connection = ConnectDB.GetConnection();
				SqlCommand command = new SqlCommand();
				command.Connection = connection;
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "updateReserveDinIn";
				command.Parameters.Add("@reserveID", SqlDbType.Int).Value = reserveID;
				command.Parameters.Add("@TableID", SqlDbType.TinyInt).Value = TableID;
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

	}
}
