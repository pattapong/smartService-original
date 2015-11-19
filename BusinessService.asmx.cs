using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Web;
using System.Web.Services;
using SmartService.Printer;
using SmartService.Utils;
using SmartService.Report;

namespace SmartService
{
	/// <summary>
	/// Summary description for BusinessService.
	/// </summary>
	[WebService(Namespace="http://ws.smartRestaurant.net")]
	public class BusinessService : System.Web.Services.WebService
	{
		private static System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo ("en-US");

		public BusinessService()
		{
			//CODEGEN: This call is required by the ASP.NET Web Services Designer
			InitializeComponent();
		}
		[WebMethod]
		public void BackupDatabase()
		{
			SqlConnection connection = ConnectDB.GetConnection();
			SqlCommand command = new SqlCommand("backupDatabase", connection);
			command.CommandType = CommandType.StoredProcedure;
			command.ExecuteNonQuery();
			connection.Close();
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
		public DataStream[] GetSalesReport(DateTime date, int empType)
		{
			SqlConnection connection = ConnectDB.GetConnection();
			SqlCommand selectCommand = new SqlCommand("getRptSalesByPaymentTime", connection);
			selectCommand.CommandType = CommandType.StoredProcedure;
			selectCommand.Parameters.Add("@datefrom", SqlDbType.VarChar).Value = date.ToString("dd/MM/yyyy", ci);
			selectCommand.Parameters.Add("@dateto", SqlDbType.VarChar).Value = date.ToString("dd/MM/yyyy", ci);
			selectCommand.Parameters.Add("@empType", SqlDbType.Int).Value = empType;
			SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
			DataTable dataTable = new DataTable();
			adapter.Fill(dataTable);
			connection.Close();
			return DataStream.Convert(dataTable);
		}


		[WebMethod]
		public DataStream[] GetInvoiceReport(DateTime date, int empType)
		{
			SqlConnection connection = ConnectDB.GetConnection();
			SqlCommand selectCommand = new SqlCommand("getRptInvoiceByDate", connection);
			selectCommand.CommandType = CommandType.StoredProcedure;
			selectCommand.Parameters.Add("@date", SqlDbType.DateTime).Value = date;
			selectCommand.Parameters.Add("@empType", SqlDbType.Int).Value = empType;
			SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
			DataTable dataTable = new DataTable();
			adapter.Fill(dataTable);
			connection.Close();
			return DataStream.Convert(dataTable);
		}


		[WebMethod]
		public void PrintInvoiceReport(DateTime date, int empType)
		{
			PrintInvoiceList.Print(date, empType);
		}


		[WebMethod]
		public DataStream[] GetInvoiceSummaryReport(DateTime date, int empType)
		{
			SqlConnection connection = ConnectDB.GetConnection();
			SqlCommand selectCommand = new SqlCommand("getRptInvoiceSummary", connection);
			selectCommand.CommandType = CommandType.StoredProcedure;
			selectCommand.Parameters.Add("@date", SqlDbType.DateTime).Value = date;
			selectCommand.Parameters.Add("@empType", SqlDbType.Int).Value = empType;
			SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
			DataTable dataTable = new DataTable();
			adapter.Fill(dataTable);
			connection.Close();
			return DataStream.Convert(dataTable);
		}
		[WebMethod]
		public string GetKitchenPrinter()
		{
			return CheckBillService.GetDescriptionByID("KIT_PRINTER");
		}


		[WebMethod]
		public string GetReceiptPrinter()
		{
			return CheckBillService.GetDescriptionByID("RCP_PRINTER");
		}


		[WebMethod]
		public void UpdateInvoiceHidden(int invoiceID, bool hidden)
		{
			SqlConnection conn = ConnectDB.GetConnection();
			SqlCommand command = new SqlCommand("updateInvoiceHidden", conn);
			command.CommandType = CommandType.StoredProcedure;
			SqlParameter paramInvoiceID = command.Parameters.Add("@invoiceID", SqlDbType.Int);
			paramInvoiceID.Value = invoiceID;
			SqlParameter paramHidden = command.Parameters.Add("@hidden", SqlDbType.Bit);
			paramHidden.Value = hidden?1:0;
			command.ExecuteNonQuery();
			conn.Close();
		}

		[WebMethod]
		public void PrintSummaryMenuType(DateTime date)
		{
			Printer.PrintSummary.PrintMenuType(date);
		}

		[WebMethod]
		public void PrintSummaryReceive(DateTime date)
		{
			Printer.PrintSummary.PrintReceive(date);
		}
		[WebMethod]
		public void PrintTaxSummary(int month, int year)
		{
			Printer.PrintTaxSummary.Print(month, year);
		}
		[WebMethod]
		public void SetBillPrinter(string printerName)
		{
			this.SetPrinter("BIL", printerName);
		}
		[WebMethod]
		public void SetReceiptPrinter(string printerName)
		{
			this.SetPrinter("RCP", printerName);
		}


		[WebMethod]
		public void SetKitchenPrinter(string printerName)
		{
			this.SetPrinter("KIT", printerName);
		}
		private void SetPrinter(string style, string printerName)
		{
			SqlConnection connection = ConnectDB.GetConnection();
			SqlCommand command = new SqlCommand("updateDescription", connection);
			command.CommandType = CommandType.StoredProcedure;
			command.Parameters.Add("@descriptionID", SqlDbType.NVarChar).Value = style + "_PRINTER";
			command.Parameters.Add("@descriptionText", SqlDbType.NVarChar).Value = printerName;
			command.Parameters.Add("@descriptionLinkId", SqlDbType.Int).Value = 0;
			command.Parameters.Add("@result", SqlDbType.Int).Direction = ParameterDirection.Output;
			command.ExecuteNonQuery();
			connection.Close();
		}

		[WebMethod]
		public DataStream[] ExportInvoice(DateTime fromDate, DateTime toDate)
		{
			SqlConnection connection = ConnectDB.GetConnection();
			SqlCommand selectCommand = new SqlCommand("exportInvoice", connection);
			selectCommand.CommandType = CommandType.StoredProcedure;
			selectCommand.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = fromDate;
			selectCommand.Parameters.Add("@toDate", SqlDbType.DateTime).Value = toDate;
			SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
			DataTable dataTable = new DataTable();
			adapter.Fill(dataTable);
			connection.Close();
			return DataStream.Convert(dataTable);
		}


		[WebMethod]
		public void DeleteSelected()
		{
			SqlConnection connection = ConnectDB.GetConnection();
			SqlCommand command = new SqlCommand("deleteSelected", connection);
			command.CommandType = CommandType.StoredProcedure;
			command.ExecuteNonQuery();
			connection.Close();
		}
		[WebMethod]
		public string GetBillPrinter()
		{
			return CheckBillService.GetDescriptionByID("BIL_PRINTER");
		}
		[WebMethod]
		public string[] GetInstalledPrinter()
		{
			return PrintSlip.GetInstalledPrinter();
		}

	}
}
