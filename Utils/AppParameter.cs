using System;
using System.Configuration;

namespace SmartService.Utils
{
	/// <summary>
	/// Summary description for AppParameter.
	/// </summary>
	public class AppParameter
	{
		private static DateTime minDateTime;
		private static bool setMinDateTime = false;
		public static bool IsDemo()
		{
			string str = ConfigurationSettings.AppSettings["Demo"];
			return ((str != null) && (str == "1"));
		}

		public static float GetPaperWidth()
		{
			try
			{
				return float.Parse(ConfigurationSettings.AppSettings["PaperWidth"].ToString());
			}
			catch (Exception)
			{
				return 275f;
			}
		}
		public static DateTime MinDateTime
		{
			get
			{
				if (!setMinDateTime)
				{
					minDateTime = new DateTime(0x76c, 1, 1, 0, 0, 0, 0);
				}
				return minDateTime;
			}
		}

	}
}
