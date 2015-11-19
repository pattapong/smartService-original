using System;
using System.Data.SqlClient;
using System.Data;
using SmartService.Utils;

namespace SmartService.Customer
{
	/// <summary>
	/// Summary description for CustomerInformation.
	/// </summary>
	public class CustomerInformation
	{
		public string Address;
		public int CustID;
		public string Description;
		public string FirstName;
		public string LastName;
		public string MiddleName;
		public string OtherRoadName;
		public int RoadID;
		public string Telephone;


		public CustomerInformation()
		{
			this.CustID = 0;
			this.FirstName = null;
			this.MiddleName = null;
			this.LastName = null;
			this.Telephone = null;
			this.Address = null;
			this.Description = null;
			this.RoadID = 0;
			this.OtherRoadName = null;

		}

		public CustomerInformation(int custID, string firstName, string middleName, string lastName, string telephone, string address, string description, int roadID, string otherRoadName)
		{
			this.CustID = custID;
			this.FirstName = firstName;
			this.MiddleName = middleName;
			this.LastName = lastName;
			this.Telephone = telephone;
			this.Address = address;
			this.Description = description;
			this.RoadID = roadID;
			this.OtherRoadName = otherRoadName;
		}


		public static string CustomerInsert(CustomerInformation custInfo)
		{
			try
			{
				SqlConnection connection = ConnectDB.GetConnection();
				SqlCommand command = new SqlCommand("insertCustomer", connection);
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.Add("@firstName", SqlDbType.NVarChar).Value = custInfo.FirstName;
				command.Parameters.Add("@middleName", SqlDbType.NVarChar).Value = custInfo.MiddleName;
				command.Parameters.Add("@lastName", SqlDbType.NVarChar).Value = custInfo.LastName;
				command.Parameters.Add("@telephone", SqlDbType.VarChar).Value = custInfo.Telephone;
				command.Parameters.Add("@address", SqlDbType.NVarChar).Value = custInfo.Address;
				command.Parameters.Add("@description", SqlDbType.NVarChar).Value = custInfo.Description;
				SqlParameter parameter7 = command.Parameters.Add("@roadid", SqlDbType.Int);
				if (custInfo.RoadID > 0)
				{
					parameter7.Value = custInfo.RoadID;
				}
				else
				{
					parameter7.Value = DBNull.Value;
				}
				SqlParameter parameter8 = command.Parameters.Add("@otherroadname", SqlDbType.NVarChar);
				if (custInfo.OtherRoadName != null)
				{
					parameter8.Value = custInfo.OtherRoadName;
				}
				else
				{
					parameter8.Value = DBNull.Value;
				}
				SqlParameter parameter9 = command.Parameters.Add("@result", SqlDbType.Int);
				parameter9.Direction = ParameterDirection.Output;
				command.ExecuteNonQuery();
				connection.Close();
				parameter9.Value.ToString();
				return null;
			}
			catch (Exception exception)
			{
				return exception.Message.ToString();
			}
		}

		public static string CustomerUpdate(CustomerInformation custInfo)
		{
			try
			{
				SqlConnection connection = ConnectDB.GetConnection();
				SqlCommand command = new SqlCommand("updateCustomer", connection);
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.Add("@custID", SqlDbType.Int).Value = custInfo.CustID;
				command.Parameters.Add("@firstName", SqlDbType.NVarChar).Value = custInfo.FirstName;
				command.Parameters.Add("@middleName", SqlDbType.NVarChar).Value = custInfo.MiddleName;
				command.Parameters.Add("@lastName", SqlDbType.NVarChar).Value = custInfo.LastName;
				command.Parameters.Add("@telephone", SqlDbType.VarChar).Value = custInfo.Telephone;
				command.Parameters.Add("@address", SqlDbType.NVarChar).Value = custInfo.Address;
				command.Parameters.Add("@description", SqlDbType.NVarChar).Value = custInfo.Description;
				SqlParameter parameter8 = command.Parameters.Add("@roadid", SqlDbType.Int);
				if (custInfo.RoadID > 0)
				{
					parameter8.Value = custInfo.RoadID;
				}
				else
				{
					parameter8.Value = DBNull.Value;
				}
				SqlParameter parameter9 = command.Parameters.Add("@otherroadname", SqlDbType.NVarChar);
				if (custInfo.OtherRoadName != null)
				{
					parameter9.Value = custInfo.OtherRoadName;
				}
				else
				{
					parameter9.Value = DBNull.Value;
				}
				SqlParameter parameter10 = command.Parameters.Add("@result", SqlDbType.Int);
				parameter10.Direction = ParameterDirection.Output;
				command.ExecuteNonQuery();
				connection.Close();
				parameter10.Value.ToString();
				return null;
			}
			catch (Exception exception)
			{
				return exception.Message.ToString();
			}
		}

		public static string CustomerDelete(int CustID)
		{
			try
			{
				SqlConnection connection = ConnectDB.GetConnection();
				SqlCommand command = new SqlCommand("deleteCustomer", connection);
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.Add("@custID", SqlDbType.Int).Value = CustID;
				SqlParameter parameter2 = command.Parameters.Add("@result", SqlDbType.Int);
				parameter2.Direction = ParameterDirection.Output;
				command.ExecuteNonQuery();
				connection.Close();
				parameter2.Value.ToString();
				return null;
			}
			catch (Exception exception)
			{
				return exception.Message.ToString();
			}
		}

	}
}
