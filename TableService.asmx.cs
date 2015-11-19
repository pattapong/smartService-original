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
	/// Summary description for TableService.
	/// </summary>
	[WebService(Namespace="http://ws.smartRestaurant.net")]
	public class TableService : System.Web.Services.WebService
	{
		public TableService()
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
		public TableInformation[] GetTableList()
		{
			SqlConnection connection = ConnectDB.GetConnection();
			SqlCommand selectCommand = new SqlCommand("getTable", connection);
			selectCommand.CommandType = CommandType.StoredProcedure;
			selectCommand.Parameters.Add("@tableid", SqlDbType.Int).Value = 0;
			SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
			DataTable dataTable = new DataTable();
			adapter.Fill(dataTable);
			connection.Close();
			if (dataTable.Rows.Count <= 0)
			{
				return null;
			}
			TableInformation[] informationArray = new TableInformation[dataTable.Rows.Count];
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				DataRow row = dataTable.Rows[i];
				informationArray[i] = new TableInformation((int) row["tableid"], (int) row["numberofseat"], row["tablename"].ToString());
			}
			return informationArray;
		}


		[WebMethod]
		public TableInformation GetTableInformation(int tableID)
		{
			SqlConnection connection = ConnectDB.GetConnection();
			SqlCommand selectCommand = new SqlCommand("getTable", connection);
			selectCommand.CommandType = CommandType.StoredProcedure;
			selectCommand.Parameters.Add("@tableid", SqlDbType.Int).Value = tableID;
			SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
			DataTable dataTable = new DataTable();
			adapter.Fill(dataTable);
			connection.Close();
			if (dataTable.Rows.Count <= 0)
			{
				return null;
			}
			DataRow row = dataTable.Rows[0];
			return new TableInformation((int) row["tableid"], (int) row["numberofseat"], row["tablename"].ToString());
		}

		[WebMethod]
		public TableStatus[] GetTableStatus()
		{
			SqlConnection connection = ConnectDB.GetConnection();
			SqlCommand selectCommand = new SqlCommand("getTableStatus", connection);
			selectCommand.CommandType = CommandType.StoredProcedure;
			SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
			DataTable dataTable = new DataTable();
			adapter.Fill(dataTable);
			connection.Close();
			if (dataTable.Rows.Count <= 0)
			{
				return null;
			}
			TableStatus[] statusArray = new TableStatus[dataTable.Rows.Count];
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				DataRow row = dataTable.Rows[i];
				statusArray[i] = new TableStatus((int) row["tableid"], row["tablename"].ToString(), ((int) row["inuse"]) > 0, ((int) row["printbillcnt"]) > 0, ((int) row["waitingcnt"]) > 0, ((int) row["lockinuse"]) > 0);
			}
			return statusArray;
		}

		[WebMethod]
		public int UpdateTableLockInuse(int tableID, bool lockInUse)
		{
			SqlConnection connection = null;
			int num = 0;
			try
			{
				connection = ConnectDB.GetConnection();
				SqlCommand command = new SqlCommand();
				command.Connection = connection;
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "updateTableLockInuse";
				command.Parameters.Add("@tableID", SqlDbType.Int).Value = tableID;
				SqlParameter parameter2 = command.Parameters.Add("@lockinuse", SqlDbType.Char);
				if (lockInUse)
				{
					parameter2.Value = "1";
				}
				else
				{
					parameter2.Value = "0";
				}
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
