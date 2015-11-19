using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;

namespace SmartService.Utils
{
	/// <summary>
	/// Summary description for ConnectDB.
	/// </summary>
	public class ConnectDB
	{
		private static string oldCustFullName = null;
		private static string oldCustFName;
		private static string oldCustMName;
		private static string oldCustLName;

		public static SqlConnection GetConnection()
		{
			string connectionString = ConfigurationSettings.AppSettings["smartConnection"];
			SqlConnection connection = new SqlConnection(connectionString);
			connection.Open();
			return connection;
		}

		public static string ToDB(string data)
		{
			return data.Replace("'", "''");
		}


		private static void SplitCustName(string CustFullName)
		{
			if (CustFullName != oldCustFullName)
			{
				string[] strArray = CustFullName.Split(new char[] { ' ' });
				oldCustFName = oldCustMName = oldCustLName = "";
				if (strArray.Length >= 1)
				{
					oldCustFName = strArray[0];
				}
				if (strArray.Length == 2)
				{
					oldCustLName = strArray[1];
				}
				if (strArray.Length == 3)
				{
					oldCustMName = strArray[1];
					oldCustLName = strArray[2];
				}
			}
		}


		public static string SplitCustFName(string CustFullName)
		{
			SplitCustName(CustFullName);
			return oldCustFName;
		}

		public static string SplitCustMName(string CustFullName)
		{
			SplitCustName(CustFullName);
			return oldCustMName;
		}

		public static string SplitCustLName(string CustFullName)
		{
			SplitCustName(CustFullName);
			return oldCustLName;
		}
	}
}
