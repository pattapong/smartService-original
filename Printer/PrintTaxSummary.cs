using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Globalization;
using SmartService.Utils;
using SmartService.CheckBill;
using SmartService.Printer;


namespace SmartService.Printer
{
	public class PrintTaxSummary
	{
		private static CultureInfo ci = new CultureInfo("en-US");
		public static void Print(int month, int year)
		{
			SqlConnection connection = ConnectDB.GetConnection();
			SqlCommand selectCommand = new SqlCommand("getTaxSummary", connection);
			selectCommand.CommandType = CommandType.StoredProcedure;
			selectCommand.Parameters.Add("@month", SqlDbType.Int).Value = month;
			selectCommand.Parameters.Add("@year", SqlDbType.Int).Value = year;
			SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
			DataTable dataTable = new DataTable();
			adapter.Fill(dataTable);
			if (dataTable.Rows.Count == 0)
			{
				connection.Close();
			}
			else
			{
				string str3;
				double num2;
				connection.Close();
				string descriptionByID = CheckBillService.GetDescriptionByID("TAX1");
				string str2 = CheckBillService.GetDescriptionByID("TAX2");
				PrintSlip slip = new PrintSlip("BIL");
				StringBuilder builder = new StringBuilder();
				slip.Add("Tax Monthly Report", 0, 1);
				slip.Add(new DateTime(year, month, 1).ToString("MMMM yyyy", ci), 1, 1);
				slip.Add("", 1, 0);
				builder.Length = 0;
				builder.Append("Day  ");
				if (descriptionByID.Length < 9)
				{
					builder.Append(new string(' ', 9 - descriptionByID.Length));
				}
				builder.Append(descriptionByID);
				if (str2.Length < 9)
				{
					builder.Append(new string(' ', 9 - str2.Length));
				}
				builder.Append(str2);
				slip.Add(builder.ToString(), 1, 0);
				slip.Add("-", 1, 0);
				double num = num2 = 0.0;
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					int num4 = (int) dataTable.Rows[i]["invoiceday"];
					double num5 = double.Parse(dataTable.Rows[i]["tax1"].ToString());
					double num6 = double.Parse(dataTable.Rows[i]["tax2"].ToString());
					builder.Length = 0;
					builder.Append(num4);
					if (builder.Length < 5)
					{
						builder.Append(new string(' ', 5 - builder.Length));
					}
					str3 = num5.ToString("f");
					if (str3.Length < 9)
					{
						builder.Append(new string(' ', 9 - str3.Length));
					}
					builder.Append(str3);
					str3 = num6.ToString("f");
					if (str3.Length < 9)
					{
						builder.Append(new string(' ', 9 - str3.Length));
					}
					builder.Append(str3);
					slip.Add(builder.ToString(), 1, 0);
					num += num5;
					num2 += num6;
				}
				slip.Add("-", 1, 0);
				builder.Length = 0;
				builder.Append("Total");
				str3 = num.ToString("f");
				if (str3.Length < 9)
				{
					builder.Append(new string(' ', 9 - str3.Length));
				}
				builder.Append(str3);
				str3 = num2.ToString("f");
				if (str3.Length < 9)
				{
					builder.Append(new string(' ', 9 - str3.Length));
				}
				builder.Append(str3);
				slip.Add(builder.ToString(), 1, 0);
				slip.Add("=", 1, 0);
				slip.Print();
			}
		}
	}
}
