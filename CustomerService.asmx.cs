using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using SmartService.Customer;
using SmartService.Utils;

namespace SmartService
{
	/// <summary>
	/// Summary description for CustomerService.
	/// </summary>
	[WebService(Namespace="http://ws.smartRestaurant.net")]
	public class CustomerService : System.Web.Services.WebService
	{

		public CustomerService()
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
		public CustomerInformation[] GetCustomers()
		{
			return this.SearchCustomers(string.Empty, string.Empty, string.Empty, string.Empty);
		}
		[WebMethod]
		public Road[] GetRoads()
		{
			try
			{
				SqlConnection connection = ConnectDB.GetConnection();
				SqlCommand selectCommand = new SqlCommand("getCustomerRoad", connection);
				selectCommand.CommandType = CommandType.StoredProcedure;
				selectCommand.Parameters.Add("@roadID", SqlDbType.Int).Value = 0;
				SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
				DataTable dataTable = new DataTable();
				adapter.Fill(dataTable);
				connection.Close();
				if (dataTable.Rows.Count <= 0)
				{
					return null;
				}
				Road[] roadArray = new Road[dataTable.Rows.Count];
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					int roadID = (int) dataTable.Rows[i]["RoadID"];
					string roadName = dataTable.Rows[i]["roadname"].ToString();
					string roadTypeName = dataTable.Rows[i]["roadtypename"].ToString();
					string areaName = dataTable.Rows[i]["areaname"].ToString();
					string areaTypeName = dataTable.Rows[i]["areatypename"].ToString();
					roadArray[i] = new Road(roadID, roadName, roadTypeName, areaName, areaTypeName);
				}
				return roadArray;
			}
			catch (Exception)
			{
				return null;
			}
		}


		[WebMethod]
		public CustomerInformation[] SearchCustomers(string telephone, string fname, string mname, string lname)
		{
			try
			{
				SqlConnection connection = ConnectDB.GetConnection();
				SqlCommand selectCommand = new SqlCommand("getCustomerInformation", connection);
				selectCommand.CommandType = CommandType.StoredProcedure;
				selectCommand.Parameters.Add("@tel", SqlDbType.VarChar).Value = telephone;
				selectCommand.Parameters.Add("@fname", SqlDbType.NVarChar).Value = fname;
				selectCommand.Parameters.Add("@mname", SqlDbType.NVarChar).Value = mname;
				selectCommand.Parameters.Add("@lname", SqlDbType.NVarChar).Value = lname;
				SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
				DataTable dataTable = new DataTable();
				adapter.Fill(dataTable);
				connection.Close();
				if (dataTable.Rows.Count <= 0)
				{
					return null;
				}
				CustomerInformation[] informationArray = new CustomerInformation[dataTable.Rows.Count];
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					int custID = (int) dataTable.Rows[i]["CustID"];
					string firstName = dataTable.Rows[i]["CustFirstName"].ToString();
					string middleName = dataTable.Rows[i]["CustMiddleName"].ToString();
					string lastName = dataTable.Rows[i]["CustLastName"].ToString();
					string str4 = dataTable.Rows[i]["CustTel"].ToString();
					string address = dataTable.Rows[i]["CustAddress"].ToString();
					string description = dataTable.Rows[i]["CustDescription"].ToString();
					int roadID = (dataTable.Rows[i]["RoadID"] is DBNull) ? 0 : ((int) dataTable.Rows[i]["RoadID"]);
					string otherRoadName = (dataTable.Rows[i]["OtherRoadName"] is DBNull) ? null : dataTable.Rows[i]["OtherRoadName"].ToString();
					informationArray[i] = new CustomerInformation(custID, firstName, middleName, lastName, str4, address, description, roadID, otherRoadName);
				}
				return informationArray;
			}
			catch (Exception)
			{
				return null;
			}
		}

		[WebMethod]
		public string SetCustomer(CustomerInformation custInfo)
		{
			if (custInfo.CustID == 0)
			{
				return CustomerInformation.CustomerInsert(custInfo);
			}
			return CustomerInformation.CustomerUpdate(custInfo);
		}

	}
}
