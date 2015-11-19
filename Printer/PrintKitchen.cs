using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using SmartService.Utils;

namespace SmartService.Printer
{
	/// <summary>
	/// Summary description for PrintKitchen.
	/// </summary>
	public class PrintKitchen
	{
		private static bool showCustName = true;
		private static bool showDate = true;
		private static bool showShortName = true;
		private static bool showTakeOutBar = true;
		private static bool showTime = true;
		private static bool showTotal = true;

		public static void Print(string billDetailID)
		{
			if (billDetailID != null)
			{
				GetConfig();
				SqlConnection connection = ConnectDB.GetConnection();
				SqlCommand selectCommand = new SqlCommand("getBillDetailList", connection);
				selectCommand.CommandType = CommandType.StoredProcedure;
				selectCommand.Parameters.Add("@billDetailIDList", SqlDbType.NVarChar).Value = billDetailID;
				SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
				DataTable dataTable = new DataTable();
				adapter.Fill(dataTable);
				connection.Close();
				if (dataTable.Rows.Count != 0)
				{
					PrintSlip slip = new PrintSlip("KIT");
					StringBuilder builder = new StringBuilder();
					int num = -1;
					int num2 = -1;
					string str = "0";
					int num3 = 0;
					for (int i = 0; i < dataTable.Rows.Count; i++)
					{
						string str2 = dataTable.Rows[i]["printline"].ToString();
						int num5 = (int) dataTable.Rows[i]["menutype"];
						int num6 = (int) dataTable.Rows[i]["tableid"];
						string str3 = dataTable.Rows[i]["tablename"].ToString();
						string str4 = dataTable.Rows[i]["menukeyid"].ToString();
						string str5 = dataTable.Rows[i]["menushortname"].ToString();
						int num7 = (int) dataTable.Rows[i]["unit"];
						string str6 = dataTable.Rows[i]["message"].ToString();
						string str7 = dataTable.Rows[i]["choicename"].ToString();
						string str8 = dataTable.Rows[i]["custfirstname"].ToString();
						string str9 = dataTable.Rows[i]["custmiddlename"].ToString();
						string str10 = dataTable.Rows[i]["custlastname"].ToString();
						short num8 = (short) dataTable.Rows[i]["status"];
						if (num != num6)
						{
							slip.Add("", 0, 1);
							slip.Add("", 0, 1);
							slip.Add("", 0, 1);
							slip.Add("", 0, 1);
							if (num6 == 0)
							{
								if (showTakeOutBar)
								{
									slip.Add("-XXX-", 0, 1);
								}
								else
								{
									slip.Add("", 0, 1);
								}
								builder.Length = 0;
								builder.Append(str3);
								slip.Add(builder.ToString(), 0, 1);
								if (showCustName)
								{
									builder.Length = 0;
									if (str8.Length > 0)
									{
										builder.Append(str8);
									}
									if (str9.Length > 0)
									{
										builder.Append(" ");
										builder.Append(str9);
									}
									if (str10.Length > 0)
									{
										builder.Append(" ");
										builder.Append(str10);
									}
									slip.Add(builder.ToString(), 0, 1);
								}
							}
							else
							{
								slip.Add("", 0, 1);
								builder.Length = 0;
								builder.Append("Table:");
								builder.Append(str3);
								slip.Add(builder.ToString(), 0, 1);
							}
							builder.Length = 0;
							if (showTime)
							{
								builder.Append(DateTime.Now.ToString("HH:mm"));
							}
							if (showDate)
							{
								if (builder.Length == 0)
								{
									builder.Append(DateTime.Now.ToString("yyyy/MM/dd"));
								}
								else
								{
									builder.Append(DateTime.Now.ToString("     yyyy/MM/dd"));
								}
							}
							if (builder.Length > 0)
							{
								slip.Add(builder.ToString(), 0, 1);
							}
							num = num6;
							num3 = 0;
						}
						if ((num2 != num5) && (str == "1"))
						{
							slip.Add("-", 1, 0);
						}
						num2 = num5;
						str = str2;
						builder.Length = 0;
						builder.Append(str4);
						if (num7 != 1)
						{
							builder.Append(" (");
							builder.Append(num7);
							builder.Append(")");
						}
						else
						{
							builder.Append(" ");
						}
						if (showShortName)
						{
							builder.Append(str5);
							if (num8 == 0)
							{
								builder.Append(" (Cancel)");
							}
							else if (num8 < 0)
							{
								builder.Append(" (Change)");
							}
							slip.Add(builder.ToString(), 1, 0);
							if (str7 != "")
							{
								string[] strArray = str7.Split(new char[] { ',' });
								for (int j = 0; j < strArray.Length; j++)
								{
									builder.Length = 0;
									builder.Append("  - ");
									builder.Append(strArray[j]);
									slip.Add(builder.ToString(), 2, 0);
								}
							}
						}
						else
						{
							if (num8 == 0)
							{
								builder.Append("(Can) ");
							}
							else if (num8 < 0)
							{
								builder.Append("(Chg) ");
							}
							if (str7 != "")
							{
								builder.Append(str7);
							}
							slip.Add(builder.ToString(), 1, 0);
						}
						if (str6 != "")
						{
							builder.Length = 0;
							builder.Append("  - ");
							builder.Append(str6);
							slip.Add(builder.ToString(), 3, 0);
						}
						num3 += num7;
					}
					if ((num == 0) && showTotal)
					{
						connection = ConnectDB.GetConnection();
						selectCommand = new SqlCommand("getOrderBillComputePriceByBillDetailID", connection);
						selectCommand.CommandType = CommandType.StoredProcedure;
						selectCommand.Parameters.Add("@billDetailID", SqlDbType.Int).Value = billDetailID.Split(new char[] { ',' })[0];
						adapter = new SqlDataAdapter(selectCommand);
						dataTable = new DataTable();
						adapter.Fill(dataTable);
						connection.Close();
						if (dataTable.Rows.Count > 0)
						{
							double num10 = double.Parse(dataTable.Rows[0]["totalprice"].ToString());
							double num11 = double.Parse(dataTable.Rows[0]["totaltax1"].ToString());
							double num12 = double.Parse(dataTable.Rows[0]["totaltax2"].ToString());
							num10 += num11 + num12;
							builder.Length = 0;
							builder.Append("Total: ");
							builder.Append(num10.ToString("f"));
							slip.Add(builder.ToString(), 1, 0);
						}
					}
					slip.Print();
				}
			}
		}


		private static void GetConfig()
		{
			SqlConnection connection = ConnectDB.GetConnection();
			SqlCommand command = new SqlCommand("getConfig", connection);
			command.CommandType = CommandType.StoredProcedure;
			command.Parameters.Add("@id", SqlDbType.VarChar).Value = "2,3,4,5,6,7";
			SqlDataReader reader = command.ExecuteReader();
			while (reader.Read())
			{
				int num = (int) reader["configid"];
				string str = reader["configvalue"].ToString();
				switch (num)
				{
					case 2:
						showDate = str == "1";
						break;

					case 3:
						showTime = str == "1";
						break;

					case 4:
						showTotal = str == "1";
						break;

					case 5:
						showCustName = str == "1";
						break;

					case 6:
						showShortName = str == "1";
						break;

					case 7:
						showTakeOutBar = str == "1";
						break;
				}
			}
			connection.Close();
		}

	}
}
