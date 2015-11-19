using System;

namespace SmartService.Table
{
	/// <summary>
	/// Summary description for TableInformation.
	/// </summary>
	public class TableInformation
	{
		public int TableID;
		public int NumberOfSeat;
		public string TableName;

		public TableInformation()
		{
		}

		public TableInformation(int tableID, int noOfSeat, string tableName)
		{
			this.TableID = tableID;
			this.NumberOfSeat = noOfSeat;
			this.TableName = tableName;
		}
	}
}
