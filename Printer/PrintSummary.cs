using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using SmartService.Utils;

namespace SmartService.Printer
{
	/// <summary>
	/// Summary description for PrintSummary.
	/// </summary>
	public class PrintSummary
	{
		private const int BY_MENUTYPE	= 1;
		private const int BY_RECEIVE	= 2;

		public static void PrintMenuType(DateTime date)
		{
			Print(1, date);
		}

		public static void PrintReceive(DateTime date)
		{
			Print(2, date);
		}

		private static void Print(int type, DateTime date)
		{
			SqlConnection connection = ConnectDB.GetConnection();
			SqlCommand selectCommand = new SqlCommand();
			switch (type)
			{
				case 1:
					selectCommand.CommandText = "getSummaryMenuType";
					break;

				case 2:
					selectCommand.CommandText = "getSummaryReceive";
					break;

				default:
					return;
			}
			selectCommand.Connection = connection;
			selectCommand.CommandType = CommandType.StoredProcedure;
			selectCommand.Parameters.Add("@date", SqlDbType.DateTime).Value = date;
			SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
			DataTable dataTable = new DataTable();
			adapter.Fill(dataTable);
			connection.Close();
			if (dataTable.Rows.Count != 0)
			{
				PrintSlip slip = new PrintSlip("RCP");
				StringBuilder builder = new StringBuilder();
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					string text = dataTable.Rows[i]["Message"].ToString();
					double num2 = double.Parse(dataTable.Rows[i]["Price"].ToString());
					bool flag = ((int) dataTable.Rows[i]["Bold"]) == 1;
					switch (text)
					{
						case "-":
						case "=":
							slip.Add(text, 1, 0);
							break;

						default:
						{
							builder.Length = 0;
							builder.Append(text);
							if (builder.Length < 0x15)
							{
								builder.Append(new string(' ', 0x15 - builder.Length));
							}
							string str = num2.ToString("f");
							if (str.Length < 10)
							{
								builder.Append(new string(' ', 10 - str.Length));
							}
							builder.Append(str);
							if (flag)
							{
								slip.Add(builder.ToString(), 2, 0);
							}
							else
							{
								slip.Add(builder.ToString(), 1, 0);
							}
							break;
						}
					}
				}
				slip.Print();
			}
		}
	}
}
