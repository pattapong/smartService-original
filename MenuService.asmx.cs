using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using SmartService.Menu;
using SmartService.Utils;
using System.Configuration;
using SmartService.Printer;
namespace SmartService
{
	/// <summary>
	/// Summary description for Service1.
	/// </summary>
	[WebService(Namespace="http://ws.smartRestaurant.net")]
	public class MenuService : System.Web.Services.WebService
	{
		public MenuService()
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
		public MenuType[] GetMenus(string AppName)
		{
			string str3;
			MenuType[] typeArray2;
			string str = ConfigurationSettings.AppSettings["LANG"];
			string str2 = ConfigurationSettings.AppSettings[AppName];
			if (str2 != "")
			{
				str3 = "Menu" + ConfigurationSettings.AppSettings[AppName] + "Name";
			}
			else
			{
				str3 = "MenuName" + str;
			}
			new PrintSlip("KIT");
			SqlConnection connection = null;
			try
			{
				int num;
				int num2;
				DataRow row;
				connection = ConnectDB.GetConnection();
				SqlCommand selectCommand = new SqlCommand("GetMenus", connection);
				selectCommand.CommandType = CommandType.StoredProcedure;
				SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
				DataSet dataSet = new DataSet();
				adapter.Fill(dataSet);
				DataTable table = dataSet.Tables[0];
				MenuType[] typeArray = new MenuType[table.Rows.Count];
				for (num = 0; num < table.Rows.Count; num++)
				{
					row = table.Rows[num];
					int id = (int) row["MenuTypeID"];
					string name = (row["MenuTypeName"] is DBNull) ? null : ((string) row["MenuTypeName"]);
					string desc = (row["MenuTypeDescription"] is DBNull) ? null : ((string) row["MenuTypeDescription"]);
					double num5 = double.Parse(row["Tax1"].ToString());
					double num6 = double.Parse(row["Tax2"].ToString());
					int num7 = (int) row["ItemCount"];
					typeArray[num] = new MenuType(id, name, desc);
					typeArray[num].Tax1 = num5 / 100.0;
					typeArray[num].Tax2 = num6 / 100.0;
					if (num7 > 0)
					{
						typeArray[num].MenuItems = new MenuItem[num7];
					}
				}
				table = dataSet.Tables[1];
				int index = 0;
				int num9 = 0;
				int num10 = 0;
				for (num = 0; num < table.Rows.Count; num++)
				{
					row = table.Rows[num];
					int num11 = (int) row["MenuID"];
					int keyID = (int) row["MenuKeyID"];
					int num13 = (int) row["MenuTypeID"];
					string keyIDTxt = (row["MenuKeyIDText"] is DBNull) ? keyID.ToString() : row["MenuKeyIDText"].ToString();
					string str7 = row[str3].ToString();
					string str8 = (row["MenuDescription"] is DBNull) ? null : ((string) row["MenuDescription"]);
					double price = double.Parse(row["Price"].ToString());
					int num15 = (int) row["ItemCount"];
					switch (keyIDTxt)
					{
						case null:
						case "":
							keyIDTxt = keyID.ToString();
							break;
					}
					if (num10 != num13)
					{
						num2 = 0;
						while (num2 < typeArray.Length)
						{
							if (typeArray[num2].ID == num13)
							{
								index = num2;
								break;
							}
							num2++;
						}
						num9 = -1;
						if (typeArray[index].MenuItems != null)
						{
							num2 = 0;
							while (num2 < typeArray[index].MenuItems.Length)
							{
								if (typeArray[index].MenuItems[num2] == null)
								{
									num9 = num2;
									break;
								}
								num2++;
							}
						}
						num10 = num13;
					}
					if (num9 >= 0)
					{
						typeArray[index].MenuItems[num9] = new MenuItem(typeArray[index], num11, keyID, keyIDTxt, str7, str8, price);
						if (num15 > 0)
						{
							typeArray[index].MenuItems[num9].MenuDefaults = new MenuDefault[num15];
						}
						num9++;
					}
				}
				table = dataSet.Tables[2];
				index = 0;
				num9 = 0;
				int num16 = 0;
				int num17 = 0;
				for (num = 0; num < table.Rows.Count; num++)
				{
					row = table.Rows[num];
					int num18 = (int) row["MenuID"];
					int optionID = (int) row["OptionID"];
					int choiceID = (int) row["DefaultChoiceID"];
					if (num17 != num18)
					{
						index = -1;
						num9 = -1;
						num2 = 0;
						while (num2 < typeArray.Length)
						{
							if (typeArray[num2].MenuItems != null)
							{
								for (int i = 0; i < typeArray[num2].MenuItems.Length; i++)
								{
									if (typeArray[num2].MenuItems[i].ID == num18)
									{
										index = num2;
										num9 = i;
										break;
									}
								}
								if (index >= 0)
								{
									break;
								}
							}
							num2++;
						}
						num16 = -1;
						if (typeArray[index].MenuItems[num9].MenuDefaults != null)
						{
							for (num2 = 0; num2 < typeArray[index].MenuItems[num9].MenuDefaults.Length; num2++)
							{
								if (typeArray[index].MenuItems[num9].MenuDefaults[num2] == null)
								{
									num16 = num2;
									break;
								}
							}
						}
						num17 = num18;
					}
					if (num16 >= 0)
					{
						typeArray[index].MenuItems[num9].MenuDefaults[num16] = new MenuDefault(typeArray[index].MenuItems[num9], optionID, choiceID);
						num16++;
					}
				}
				typeArray2 = typeArray;
			}
			catch (Exception)
			{
				typeArray2 = null;
			}
			finally
			{
				if (connection != null)
				{
					connection.Close();
				}
			}
			return typeArray2;
		}

		[WebMethod]
		public MenuOption[] GetOptions(string AppName)
		{
			MenuOption[] optionArray2;
			string str = "Option" + ConfigurationSettings.AppSettings[AppName] + "Name";
			string str2 = "Choice" + ConfigurationSettings.AppSettings[AppName] + "Name";
			SqlConnection connection = null;
			try
			{
				int num;
				DataRow row;
				connection = ConnectDB.GetConnection();
				SqlCommand selectCommand = new SqlCommand("GetMenuOptions", connection);
				selectCommand.CommandType = CommandType.StoredProcedure;
				SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
				DataSet dataSet = new DataSet();
				adapter.Fill(dataSet);
				DataTable table = dataSet.Tables[0];
				MenuOption[] optionArray = new MenuOption[table.Rows.Count];
				for (num = 0; num < table.Rows.Count; num++)
				{
					row = table.Rows[num];
					int optionID = (int) row["OptionID"];
					string optionName = row[str].ToString();
					int num4 = (int) row["Cnt"];
					optionArray[num] = new MenuOption(optionID, optionName);
					if (num4 > 0)
					{
						optionArray[num].OptionChoices = new OptionChoice[num4];
					}
				}
				table = dataSet.Tables[1];
				int index = 0;
				int num6 = 0;
				int num7 = 0;
				for (num = 0; num < table.Rows.Count; num++)
				{
					row = table.Rows[num];
					int choiceID = (int) row["ChoiceID"];
					int num9 = (int) row["OptionID"];
					string choiceName = row[str2].ToString();
					if (num7 != num9)
					{
						int num2 = 0;
						while (num2 < optionArray.Length)
						{
							if (optionArray[num2].OptionID == num9)
							{
								index = num2;
								break;
							}
							num2++;
						}
						num6 = -1;
						if (optionArray[index].OptionChoices != null)
						{
							for (num2 = 0; num2 < optionArray[index].OptionChoices.Length; num2++)
							{
								if (optionArray[index].OptionChoices[num2] == null)
								{
									num6 = num2;
									break;
								}
							}
						}
						num7 = num9;
					}
					if (num6 >= 0)
					{
						optionArray[index].OptionChoices[num6] = new OptionChoice(optionArray[index], choiceID, choiceName);
						num6++;
					}
				}
				optionArray2 = optionArray;
			}
			catch (Exception)
			{
				optionArray2 = null;
			}
			finally
			{
				if (connection != null)
				{
					connection.Close();
				}
			}
			return optionArray2;
		}
	}
}
