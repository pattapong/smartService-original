using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using SmartService.Utils;
using SmartService.CheckBill;

namespace SmartService.Printer
{
	/// <summary>
	/// Summary description for PrintReceipt.
	/// </summary>
	public class PrintReceipt
	{
		private static bool showGratuity = true;
		private static bool showMenuKeyID = true;

		public static void Print(int orderBillID)
		{
			if (orderBillID != 0)
			{
				GetConfig();
				SqlConnection connection = ConnectDB.GetConnection();
				SqlCommand selectCommand = new SqlCommand("getOrderBillReceipt", connection);
				selectCommand.CommandType = CommandType.StoredProcedure;
				selectCommand.Parameters.Add("@orderBillID", SqlDbType.Int).Value = orderBillID;
				SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
				DataTable dataTable = new DataTable();
				adapter.Fill(dataTable);
				if (dataTable.Rows.Count == 0)
				{
					connection.Close();
				}
				else
				{
					selectCommand.CommandText = "getOrderBillSummary";
					adapter = new SqlDataAdapter(selectCommand);
					DataTable table2 = new DataTable();
					adapter.Fill(table2);
					if (table2.Rows.Count == 0)
					{
						connection.Close();
					}
					else
					{
						double totalPrice;
						double num4;
						double num5;
						double num6;
						string str11;
						connection.Close();
						int num = (int) table2.Rows[0]["TableID"];
						string str = (string) table2.Rows[0]["TableName"];
						int num2 = (int) table2.Rows[0]["BillID"];
						string str2 = (string) table2.Rows[0]["EmpName"];
						string str3 = (string) table2.Rows[0]["CustName"];
						string text = (string) table2.Rows[0]["CustAddress"];
						string str5 = (string) table2.Rows[0]["CustTel"];
						string str6 = (string) table2.Rows[0]["CustDescription"];
						string str7 = (string) table2.Rows[0]["CustRoad"];
						string str8 = (string) table2.Rows[0]["CustArea"];
						DateTime time = (table2.Rows[0]["InvoiceTime"] is DBNull) ? DateTime.MinValue : ((DateTime) table2.Rows[0]["InvoiceTime"]);
						try
						{
							num6 = double.Parse(table2.Rows[0]["TotalDiscount"].ToString());
							double.Parse(table2.Rows[0]["TotalReceive"].ToString());
						}
						catch (Exception)
						{
							totalPrice = 0.0;
							num4 = num5 = 0.0;
							num6 = 0.0;
						}
						BillPrice price = CheckBillService.ComputeBillPrice(orderBillID);
						totalPrice = price.totalPrice;
						num4 = price.totalTax1;
						num5 = price.totalTax2;
						string descriptionByID = CheckBillService.GetDescriptionByID("TAX1");
						string str10 = CheckBillService.GetDescriptionByID("TAX2");
						if (descriptionByID == null)
						{
							descriptionByID = "Tax1";
						}
						if (str10 == null)
						{
							str10 = "Tax2";
						}
						PrintSlip slip = new PrintSlip("RCP");
						StringBuilder builder = new StringBuilder();
						for (int i = 0; i < dataTable.Rows.Count; i++)
						{
							string str14;
							int num1 = (int) dataTable.Rows[i]["MenuID"];
							string str12 = dataTable.Rows[i]["MenuKeyID"].ToString();
							string str13 = (string) dataTable.Rows[i]["MenuShortName"];
							int num8 = (int) dataTable.Rows[i]["Unit"];
							double num9 = double.Parse(dataTable.Rows[i]["Price"].ToString());
							double num10 = double.Parse(dataTable.Rows[i]["NetPrice"].ToString());
							double.Parse(dataTable.Rows[i]["Tax1"].ToString());
							double.Parse(dataTable.Rows[i]["Tax2"].ToString());
							int num11 = (int) dataTable.Rows[i]["Status"];
							if (i == 0)
							{
								if (str3 != string.Empty)
								{
									builder.Length = 0;
									builder.Append("Cust: ");
									builder.Append(str3);
									slip.Add(builder.ToString(), 0, 1);
									if (text != string.Empty)
									{
										slip.Add(text, 0, 1);
									}
									if (str5 != string.Empty)
									{
										builder.Length = 0;
										builder.Append("Tel: ");
										builder.Append(str5);
										slip.Add(builder.ToString(), 0, 1);
									}
									if (str7 != string.Empty)
									{
										builder.Length = 0;
										builder.Append(str7);
										if (str8 != string.Empty)
										{
											builder.Append(" (");
											builder.Append(str8);
											builder.Append(")");
										}
										slip.Add(builder.ToString(), 0, 1);
									}
									if (str6 != string.Empty)
									{
										slip.Add(str6, 1, 1);
									}
									slip.Add("", 1, 0);
								}
								slip.AddHeader();
								builder.Length = 0;
								if (num != 0)
								{
									builder.Append("Table: ");
									builder.Append(str);
									builder.Append("-");
									builder.Append(num2);
								}
								else
								{
									builder.Append(str);
								}
								slip.Add(builder.ToString(), 0, 1);
								builder.Length = 0;
								if (time.CompareTo(DateTime.MinValue) > 0)
								{
									builder.Append(time.ToString("HH:mm     yyyy/MM/dd"));
								}
								else
								{
									builder.Append(DateTime.Now.ToString("HH:mm     yyyy/MM/dd"));
								}
								slip.Add(builder.ToString(), 0, 1);
								builder.Length = 0;
								builder.Append("Emp: " + str2);
								slip.Add(builder.ToString(), 1, 1);
								slip.Add("", 1, 0);
							}
							builder.Length = 0;
							if (showMenuKeyID)
							{
								builder.Append(str12);
								builder.Append(".");
								str14 = " ";
							}
							else
							{
								str14 = "    ";
							}
							builder.Append(str13);
							if (builder.Length < 13)
							{
								builder.Append(new string(' ', 13 - builder.Length));
							}
							if (builder.Length > 13)
							{
								builder.Length = 13;
							}
							builder.Append(str14);
							if (num8 < 10)
							{
								builder.Append("  ");
							}
							else if (num8 < 100)
							{
								builder.Append(" ");
							}
							builder.Append(num8.ToString());
							builder.Append(" ");
							if (num11 > 0)
							{
								str11 = num9.ToString("f");
								if (str11.Length < 6)
								{
									builder.Append(new string(' ', 6 - str11.Length));
								}
								builder.Append(str11);
								builder.Append(" ");
								str11 = num10.ToString("f");
								if (str11.Length < 6)
								{
									builder.Append(new string(' ', 6 - str11.Length));
								}
								builder.Append(str11);
							}
							else
							{
								builder.Append("         N/C ");
							}
							slip.Add(builder.ToString(), 1, 0);
						}
						builder.Length = 0;
						builder.Append("Sub Total               ");
						str11 = totalPrice.ToString("f");
						if (str11.Length < 7)
						{
							builder.Append(new string(' ', 7 - str11.Length));
						}
						builder.Append(str11);
						slip.Add(builder.ToString(), 2, 0);
						if ((str10 != "") || (num5 > 0.0))
						{
							builder.Length = 0;
							builder.Append(str10);
							if (str10.Length < 20)
							{
								builder.Append(new string(' ', 20 - str10.Length));
							}
							builder.Append("    ");
							str11 = num5.ToString("f");
							if (str11.Length < 7)
							{
								builder.Append(new string(' ', 7 - str11.Length));
							}
							builder.Append(str11);
							slip.Add(builder.ToString(), 2, 0);
						}
						if ((descriptionByID != "") || (num4 > 0.0))
						{
							builder.Length = 0;
							builder.Append(descriptionByID);
							if (descriptionByID.Length < 20)
							{
								builder.Append(new string(' ', 20 - descriptionByID.Length));
							}
							builder.Append("    ");
							str11 = num4.ToString("f");
							if (str11.Length < 7)
							{
								builder.Append(new string(' ', 7 - str11.Length));
							}
							builder.Append(str11);
							slip.Add(builder.ToString(), 2, 0);
						}
						if (num6 > 0.0)
						{
							builder.Length = 0;
							builder.Append("Discount                ");
							str11 = num6.ToString("f");
							if (str11.Length < 7)
							{
								builder.Append(new string(' ', 7 - str11.Length));
							}
							builder.Append(str11);
							slip.Add(builder.ToString(), 2, 0);
						}
						builder.Length = 0;
						builder.Append("Total                   ");
						str11 = (((totalPrice + num4) + num5) - num6).ToString("f");
						if (str11.Length < 7)
						{
							builder.Append(new string(' ', 7 - str11.Length));
						}
						builder.Append(str11);
						slip.Add(builder.ToString(), 3, 0);
						if (!AppParameter.IsDemo() && showGratuity)
						{
							builder.Length = 0;
							builder.Append("Gratuity                _______");
							slip.Add(builder.ToString(), 3, 0);
							builder.Length = 0;
							builder.Append("Grand Total             _______");
							slip.Add(builder.ToString(), 3, 0);
						}
						slip.AddFooter();
						slip.Print();
					}
				}
			}
		}

		private static void GetConfig()
		{
			SqlConnection connection = ConnectDB.GetConnection();
			SqlCommand command = new SqlCommand("getConfig", connection);
			command.CommandType = CommandType.StoredProcedure;
			command.Parameters.Add("@id", SqlDbType.VarChar).Value = "8,9";
			SqlDataReader reader = command.ExecuteReader();
			while (reader.Read())
			{
				int num = (int) reader["configid"];
				string str = reader["configvalue"].ToString();
				switch (num)
				{
					case 8:
						showMenuKeyID = str == "1";
						break;

					case 9:
						showGratuity = str == "1";
						break;
				}
			}
			connection.Close();
		}

	}
}
