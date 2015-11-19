using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using SmartService.UserProfile;
using SmartService.Utils;

namespace SmartService
{
	/// <summary>
	/// Summary description for UserAuthorizeService.
	/// </summary>
	[WebService(Namespace="http://ws.smartRestaurant.net")]
	public class UserAuthorizeService : System.Web.Services.WebService
	{
		public UserAuthorizeService()
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
		public SmartService.UserProfile.UserProfile CheckLogin(int employeeID, string password)
		{
			SqlConnection connection = ConnectDB.GetConnection();
			SqlCommand selectCommand = new SqlCommand("checkLogin", connection);
			selectCommand.CommandType = CommandType.StoredProcedure;
			selectCommand.Parameters.Add("@employeeid", SqlDbType.Int).Value = employeeID;
			selectCommand.Parameters.Add("@password", SqlDbType.VarChar).Value = password;
			object obj2 = selectCommand.ExecuteScalar();
			if (((obj2 == null) || (obj2.ToString() == "")) || (obj2 is DBNull))
			{
				connection.Close();
				return null;
			}
			SmartService.UserProfile.UserProfile profile = new SmartService.UserProfile.UserProfile();
			profile.Name = obj2.ToString();
			selectCommand.CommandText = "getEmployee";
			selectCommand.Parameters.Clear();
			selectCommand.Parameters.Add("@empid", SqlDbType.Int).Value = employeeID;
			DataTable dataTable = new DataTable();
			new SqlDataAdapter(selectCommand).Fill(dataTable);
			connection.Close();
			profile.EmployeeTypeID = (int) dataTable.Rows[0]["EmployeeTypeID"];
			return profile;
		}


		[WebMethod]
		public string CheckLogout(int employeeID)
		{
			SqlConnection connection = ConnectDB.GetConnection();
			SqlCommand command = new SqlCommand("checkLogout", connection);
			command.CommandType = CommandType.StoredProcedure;
			command.Parameters.Add("@employeeid", SqlDbType.Int).Value = employeeID;
			command.ExecuteScalar();
			connection.Close();
			return null;
		}

	}
}
