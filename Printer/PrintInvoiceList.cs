using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using SmartService.Utils;

namespace SmartService.Printer
{
	/// <summary>
	/// Summary description for PrintInvoiceList.
	/// </summary>
	public class PrintInvoiceList
	{
		public static void Print(DateTime date, int empType)
		{
			SqlConnection connection = ConnectDB.GetConnection();
			SqlCommand selectCommand = new SqlCommand("getRptInvoiceByDate", connection);
			selectCommand.CommandType = CommandType.StoredProcedure;
			selectCommand.Parameters.Add("@date", SqlDbType.DateTime).Value = date;
			selectCommand.Parameters.Add("@empType", SqlDbType.Int).Value = empType;
			SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
			DataTable dataTable = new DataTable();
			adapter.Fill(dataTable);
			if (dataTable.Rows.Count == 0)
			{
				connection.Close();
			}
			else
			{
				connection.Close();
				PrintSlip slip = new PrintSlip("BIL");
				StringBuilder builder = new StringBuilder();
				int num = 0;
				int num2 = 1;
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					int num4 = (int) dataTable.Rows[i]["invoiceid"];
					string str2 = dataTable.Rows[i]["Date"].ToString();
					string str3 = dataTable.Rows[i]["Time"].ToString();
					string str4 = dataTable.Rows[i]["Paid By"].ToString();
					double num5 = double.Parse(dataTable.Rows[i]["Total"].ToString());
					bool flag = (bool) dataTable.Rows[i]["hidden"];
					if (i == 0)
					{
						builder.Length = 0;
						builder.Append("Date: ");
						builder.Append(str2);
						slip.Add(builder.ToString(), 0, 1);
						slip.Add("", 1, 0);
					}
					builder.Length = 0;
					if (num != num4)
					{
						builder.Append(num2);
						if (builder.Length < 5)
						{
							builder.Append(new string(' ', 5 - builder.Length));
						}
						num2++;
					}
					else
					{
						builder.Append(new string(' ', 5));
					}
					if (flag)
					{
						builder.Append('*');
					}
					else
					{
						builder.Append(' ');
					}
					builder.Append(str3);
					builder.Append(' ');
					if (str4.Length > 10)
					{
						builder.Append(str4.Substring(0, 10));
					}
					else
					{
						builder.Append(str4);
						if (str4.Length < 10)
						{
							builder.Append(new string(' ', 10 - str4.Length));
						}
					}
					string str = num5.ToString("f");
					if (str.Length < 9)
					{
						builder.Append(new string(' ', 9 - str.Length));
					}
					builder.Append(str);
					slip.Add(builder.ToString(), 1, 0);
					num = num4;
				}
				slip.Print();
			}
		}
	}
}
