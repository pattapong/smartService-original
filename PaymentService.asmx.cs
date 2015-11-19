using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using SmartService.Payment;
using SmartService.Utils;

namespace SmartService
{
	/// <summary>
	/// Summary description for PaymentService.
	/// </summary>
	[WebService(Namespace="http://ws.smartRestaurant.net")]
	public class PaymentService : System.Web.Services.WebService
	{
		public PaymentService()
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
		public PaymentMethod[] GetPaymentMethods()
		{
			SqlConnection connection = ConnectDB.GetConnection();
			SqlCommand selectCommand = new SqlCommand("getPaymentMethod", connection);
			selectCommand.CommandType = CommandType.StoredProcedure;
			selectCommand.Parameters.Add("@paymentMethodID", SqlDbType.Int).Value = 0;
			SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
			DataTable dataTable = new DataTable();
			adapter.Fill(dataTable);
			connection.Close();
			if (dataTable.Rows.Count == 0)
			{
				return null;
			}
			PaymentMethod[] methodArray = new PaymentMethod[dataTable.Rows.Count];
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				int id = (int) dataTable.Rows[i]["paymentMethodID"];
				string name = (string) dataTable.Rows[i]["paymentMethodName"];
				methodArray[i] = new PaymentMethod(id, name);
			}
			return methodArray;
		}

		[WebMethod]
		public PromotionCard[] GetCoupons()
		{
			SqlConnection connection = ConnectDB.GetConnection();
			SqlCommand selectCommand = new SqlCommand("getCoupons", connection);
			selectCommand.CommandType = CommandType.StoredProcedure;
			SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
			DataTable dataTable = new DataTable();
			adapter.Fill(dataTable);
			connection.Close();
			if (dataTable.Rows.Count == 0)
			{
				return null;
			}
			PromotionCard[] cardArray = new PromotionCard[dataTable.Rows.Count];
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				int proID = (int) dataTable.Rows[i]["proID"];
				string desc = (string) dataTable.Rows[i]["description"];
				double amount = double.Parse(dataTable.Rows[i]["amount"].ToString());
				cardArray[i] = new PromotionCard(proID, desc, amount);
			}
			return cardArray;
		}


		[WebMethod]
		public PromotionCard[] GetGiftVouchers()
		{
			SqlConnection connection = ConnectDB.GetConnection();
			SqlCommand selectCommand = new SqlCommand("getGiftVouchers", connection);
			selectCommand.CommandType = CommandType.StoredProcedure;
			SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
			DataTable dataTable = new DataTable();
			adapter.Fill(dataTable);
			connection.Close();
			if (dataTable.Rows.Count == 0)
			{
				return null;
			}
			PromotionCard[] cardArray = new PromotionCard[dataTable.Rows.Count];
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				int proID = (int) dataTable.Rows[i]["proID"];
				string desc = (string) dataTable.Rows[i]["description"];
				double amount = double.Parse(dataTable.Rows[i]["amount"].ToString());
				cardArray[i] = new PromotionCard(proID, desc, amount);
			}
			return cardArray;
		}


		[WebMethod]
		public Discount[] GetDiscounts()
		{
			SqlConnection connection = ConnectDB.GetConnection();
			SqlCommand selectCommand = new SqlCommand("getDiscounts", connection);
			selectCommand.CommandType = CommandType.StoredProcedure;
			SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
			DataTable dataTable = new DataTable();
			adapter.Fill(dataTable);
			connection.Close();
			if (dataTable.Rows.Count == 0)
			{
				return null;
			}
			Discount[] discountArray = new Discount[dataTable.Rows.Count];
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				int promotionID = (int) dataTable.Rows[i]["proID"];
				int promotionType = (int) dataTable.Rows[i]["proTypeID"];
				string description = (string) dataTable.Rows[i]["description"];
				double amount = (dataTable.Rows[i]["amount"] is DBNull) ? 0.0 : double.Parse(dataTable.Rows[i]["amount"].ToString());
				double discountPercent = (dataTable.Rows[i]["discountPercent"] is DBNull) ? 0.0 : double.Parse(dataTable.Rows[i]["discountPercent"].ToString());
				double discountAmount = (dataTable.Rows[i]["discountAmount"] is DBNull) ? 0.0 : double.Parse(dataTable.Rows[i]["discountAmount"].ToString());
				discountArray[i] = new Discount(promotionID, promotionType, description, amount, discountPercent, discountAmount);
			}
			return discountArray;
		}

	}
}
