using System;
using System.Data;

namespace SmartService.Report
{
	/// <summary>
	/// Summary description for DataStream.
	/// </summary>
	public class DataStream
	{
		public string[] Column;

		public DataStream()
		{
		}

		public DataStream(int col)
		{
			Column = new string[col];
		}

		public static DataStream[] Convert(DataTable dTable)
		{
			if (dTable == null)
			{
				return null;
			}
			DataStream[] streamArray = new DataStream[dTable.Rows.Count + 1];
			DataStream stream = new DataStream(dTable.Columns.Count);
			for (int i = 0; i < dTable.Columns.Count; i++)
			{
				stream.Column[i] = dTable.Columns[i].ColumnName;
			}
			streamArray[0] = stream;
			for (int j = 0; j < dTable.Rows.Count; j++)
			{
				DataRow row = dTable.Rows[j];
				stream = new DataStream(dTable.Columns.Count);
				for (int k = 0; k < dTable.Columns.Count; k++)
				{
					stream.Column[k] = row[k].ToString();
				}
				streamArray[j + 1] = stream;
			}
			return streamArray;
		}

	}
}
