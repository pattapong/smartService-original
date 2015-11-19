using System;

namespace SmartService.Table
{
	/// <summary>
	/// Summary description for TableStatus.
	/// </summary>
	public class TableStatus
	{
		public bool InUse;
		public bool IsPrintBill;
		public bool IsWaitingItem;
		public bool LockInUse;
		public int TableID;
		public string TableName;


		public TableStatus()
		{
			this.TableID = 0;
			this.TableName = null;
			this.InUse = false;
			this.IsPrintBill = false;
			this.IsWaitingItem = false;
			this.LockInUse = false;

		}

		public TableStatus(int tableID, string tableName, bool inUse, bool isPrintBill, bool isWaitingItem, bool isLockInUse)
		{
			this.TableID = tableID;
			this.TableName = tableName;
			this.InUse = inUse;
			this.IsPrintBill = isPrintBill;
			this.IsWaitingItem = isWaitingItem;
			this.LockInUse = isLockInUse;
		}

	}
}
