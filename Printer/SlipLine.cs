using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using SmartService.Utils;

namespace SmartService.Printer
{
	public class SlipLine
	{
		// Fields
		public int FontSize;
		public string Text;
		public int TextAlignment;

		// Methods
		public SlipLine(string text, int fontSize, int textAlignment)
		{
			this.Text = text;
			this.FontSize = fontSize;
			this.TextAlignment = textAlignment;
		}
	}


}
