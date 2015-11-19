using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using SmartService.Utils;
using SmartService.CheckBill;

namespace SmartService.Printer
{
	/// <summary>
	/// Summary description for PrintBill.
	/// </summary>
	public class PrintBill
	{
		private static bool printBill = true;
		private static bool showMenuKeyID = true;

		public PrintBill()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static void Print(int orderBillID)
		{
			Print(orderBillID, false);
		}

		public static void Print(int orderBillID, bool reprint)
		{
			if (orderBillID != 0)
			{
				GetConfig();
				if (reprint || printBill)
				{
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
							double num5;
							double num6;
							double num7;
							double num8;
							string str11;
							connection.Close();
							int num = (int) table2.Rows[0]["TableID"];
							string str = (string) table2.Rows[0]["TableName"];
							int num2 = (int) table2.Rows[0]["BillID"];
							int num3 = (int) table2.Rows[0]["InvoiceID"];
							string str2 = (string) table2.Rows[0]["EmpName"];
							string str3 = (string) table2.Rows[0]["CustName"];
							string text = (string) table2.Rows[0]["CustAddress"];
							string str5 = (string) table2.Rows[0]["CustTel"];
							string str6 = (string) table2.Rows[0]["CustDescription"];
							string str7 = (string) table2.Rows[0]["CustRoad"];
							string str8 = (string) table2.Rows[0]["CustArea"];
							DateTime time = (table2.Rows[0]["InvoiceTime"] is DBNull) ? DateTime.MinValue: ((DateTime) table2.Rows[0]["InvoiceTime"]);
							try
							{
								num7 = double.Parse(table2.Rows[0]["TotalDiscount"].ToString());
								num8 = double.Parse(table2.Rows[0]["TotalReceive"].ToString());
							}
							catch (Exception)
							{
								totalPrice = 0.0;
								num5 = num6 = 0.0;
								num7 = num8 = 0.0;
							}
							BillPrice price = CheckBillService.ComputeBillPrice(orderBillID);
							totalPrice = price.totalPrice;
							num5 = price.totalTax1;
							num6 = price.totalTax2;
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
							PrintSlip slip = new PrintSlip("BIL");
							StringBuilder builder = new StringBuilder();
							for (int i = 0; i < dataTable.Rows.Count; i++)
							{
								string str14;
								int num1 = (int) dataTable.Rows[i]["MenuID"];
								string str12 = dataTable.Rows[i]["MenuKeyID"].ToString();
								string str13 = (string) dataTable.Rows[i]["MenuShortName"];
								int num10 = (int) dataTable.Rows[i]["Unit"];
								double num11 = double.Parse(dataTable.Rows[i]["Price"].ToString());
								double num12 = double.Parse(dataTable.Rows[i]["NetPrice"].ToString());
								double.Parse(dataTable.Rows[i]["Tax1"].ToString());
								double.Parse(dataTable.Rows[i]["Tax2"].ToString());
								int num13 = (int) dataTable.Rows[i]["Status"];
								if (i == 0)
								{
									if (reprint)
									{
										slip.Add("COPY", 0, 2);
									}
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
								if (num10 < 10)
								{
									builder.Append("  ");
								}
								else if (num10 < 100)
								{
									builder.Append(" ");
								}
								builder.Append(num10);
								builder.Append(" ");
								if (num13 > 0)
								{
									str11 = num11.ToString("f");
									if (str11.Length < 6)
									{
										builder.Append(new string(' ', 6 - str11.Length));
									}
									builder.Append(str11);
									builder.Append(" ");
									str11 = num12.ToString("f");
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
							if ((str10 != "") || (num6 > 0.0))
							{
								builder.Length = 0;
								builder.Append(str10);
								if (str10.Length < 20)
								{
									builder.Append(new string(' ', 20 - str10.Length));
								}
								builder.Append("    ");
								str11 = num6.ToString("f");
								if (str11.Length < 7)
								{
									builder.Append(new string(' ', 7 - str11.Length));
								}
								builder.Append(str11);
								slip.Add(builder.ToString(), 2, 0);
							}
							if ((descriptionByID != "") || (num5 > 0.0))
							{
								builder.Length = 0;
								builder.Append(descriptionByID);
								if (descriptionByID.Length < 20)
								{
									builder.Append(new string(' ', 20 - descriptionByID.Length));
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
							if (num7 > 0.0)
							{
								builder.Length = 0;
								builder.Append("Discount                ");
								str11 = num7.ToString("f");
								if (str11.Length < 7)
								{
									builder.Append(new string(' ', 7 - str11.Length));
								}
								builder.Append(str11);
								slip.Add(builder.ToString(), 2, 0);
							}
							builder.Length = 0;
							builder.Append("Total                   ");
							double num14 = ((totalPrice + num5) + num6) - num7;
							if (num14 < 0.0)
							{
								num14 = 0.0;
							}
							str11 = num14.ToString("f");
							if (str11.Length < 7)
							{
								builder.Append(new string(' ', 7 - str11.Length));
							}
							builder.Append(str11);
							slip.Add(builder.ToString(), 3, 0);
							double num15 = num8;
							double num16 = num15 - num14;
							if (AppParameter.IsDemo())
							{
								builder.Length = 0;
								builder.Append("Receive                 ");
								str11 = num15.ToString("f");
								if (str11.Length < 7)
								{
									builder.Append(new string(' ', 7 - str11.Length));
								}
								builder.Append(str11);
								slip.Add(builder.ToString(), 3, 0);
								builder.Length = 0;
								builder.Append("Change                  ");
								str11 = num16.ToString("f");
								if (str11.Length < 7)
								{
									builder.Append(new string(' ', 7 - str11.Length));
								}
								builder.Append(str11);
								slip.Add(builder.ToString(), 3, 0);
							}
							else
							{
								selectCommand.CommandText = "getInvoicePayment";
								selectCommand.Parameters.Clear();
								selectCommand.Parameters.Add("@invoiceID", SqlDbType.Int).Value = num3;
								adapter = new SqlDataAdapter(selectCommand);
								DataTable table3 = new DataTable();
								adapter.Fill(table3);
								for (int j = 0; j < table3.Rows.Count; j++)
								{
									builder.Length = 0;
									builder.Append(table3.Rows[j]["PaymentMethodName"].ToString());
									if (builder.Length < 0x18)
									{
										builder.Append(new string(' ', 0x18 - builder.Length));
									}
									str11 = double.Parse(table3.Rows[j]["Receive"].ToString()).ToString("f");
									if (str11.Length < 7)
									{
										builder.Append(new string(' ', 7 - str11.Length));
									}
									builder.Append(str11);
									slip.Add(builder.ToString(), 3, 0);
								}
								builder.Length = 0;
								builder.Append("Gratuity                ");
								str11 = num16.ToString("f");
								if (str11.Length < 7)
								{
									builder.Append(new string(' ', 7 - str11.Length));
								}
								builder.Append(str11);
								slip.Add(builder.ToString(), 3, 0);
								builder.Length = 0;
								builder.Append("Grand Total             ");
								str11 = num15.ToString("f");
								if (str11.Length < 7)
								{
									builder.Append(new string(' ', 7 - str11.Length));
								}
								builder.Append(str11);
								slip.Add(builder.ToString(), 3, 0);
							}
							slip.AddFooter();
							slip.Print();
						}
					}
				}
			}
		}

		private static void GetConfig()
		{
			SqlConnection connection = ConnectDB.GetConnection();
			SqlCommand command = new SqlCommand("getConfig", connection);
			command.CommandType = CommandType.StoredProcedure;
			command.Parameters.Add("@id", SqlDbType.VarChar).Value = "1";
			SqlDataReader reader = command.ExecuteReader();
			while (reader.Read())
			{
				int num = (int) reader["configid"];
				string str = reader["configvalue"].ToString();
				switch (num)
				{
					case 1:
					{
						printBill = str == "1";
						continue;
					}
					case 8:
						break;

					default:
					{
						continue;
					}
				}
				showMenuKeyID = str == "1";
			}
			connection.Close();

		}
	}
}
