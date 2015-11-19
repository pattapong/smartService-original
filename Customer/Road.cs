using System;
using System.Data.SqlClient;
using System.Data;
using SmartService.Utils;

namespace SmartService.Customer
{
	/// <summary>
	/// Summary description for CustomerInformation.
	/// </summary>
	public class Road
	{
		// Fields
		public string AreaName;
		public string AreaTypeName;
		public int RoadID;
		public string RoadName;
		public string RoadTypeName;

		// Methods
		public Road()
		{
			this.RoadID = 0;
			this.RoadName = null;
			this.RoadTypeName = null;
			this.AreaName = null;
			this.AreaTypeName = null;
		}

		public Road(int roadID, string roadName, string roadTypeName, string areaName, string areaTypeName)
		{
			this.RoadID = roadID;
			this.RoadName = roadName;
			this.RoadTypeName = roadTypeName;
			this.AreaName = areaName;
			this.AreaTypeName = areaTypeName;
		}
	}


}
