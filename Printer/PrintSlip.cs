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
	public class PrintSlip
	{
		public const int ALIGN_CENTER = 1;
		public const int ALIGN_LEFT = 0;
		public const int ALIGN_RIGHT = 2;
		public const string CONTENT_FOOTER = "FOOTER";
		public const string CONTENT_HEADER = "HEADER";
		public const int FONT_BODY = 1;
		public const int FONT_HEADER = 0;
		public const int FONT_OPTION1 = 2;
		public const int FONT_OPTION2 = 3;
		private int fontSize;
		private int footerAlignment;
		private string footerText;
		private int headerAlignment;
		private string headerText;
		private ArrayList lines;
		private PrintDocument printDoc;
		private string printerName;
		private Font printFontBody;
		private Font printFontHeader;
		private Font printFontOption1;
		private Font printFontOption2;
		public const string STYLE_BILL = "BIL";
		public const string STYLE_KITCHEN = "KIT";
		public const string STYLE_RECEIPT = "RCP";
		private int textAlignment;

		private void LoadConfig(string style)
		{
			SqlConnection connection = null;
			SqlDataReader reader = null;
			bool flag = false;
			this.headerText = (string) (this.footerText = null);
			try
			{
				connection = ConnectDB.GetConnection();
				SqlCommand command = new SqlCommand("getSlipStyleFont", connection);
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.Add("@slipID", SqlDbType.VarChar).Value = style;
				reader = command.ExecuteReader();
				if (reader.Read())
				{
					string familyName = reader["HeaderFontName"].ToString();
					short num = (short) reader["HeaderFontSize"];
					FontStyle style2 = (reader["HeaderFontBold"].ToString() == "1") ? FontStyle.Bold : FontStyle.Regular;
					this.printFontHeader = new Font(familyName, (float) num, style2);
					familyName = reader["BodyFontName"].ToString();
					num = (short) reader["BodyFontSize"];
					style2 = (reader["BodyFontBold"].ToString() == "1") ? FontStyle.Bold : FontStyle.Regular;
					this.printFontBody = new Font(familyName, (float) num, style2);
					familyName = reader["Option1FontName"].ToString();
					num = (short) reader["Option1FontSize"];
					style2 = (reader["Option1FontBold"].ToString() == "1") ? FontStyle.Bold : FontStyle.Regular;
					this.printFontOption1 = new Font(familyName, (float) num, style2);
					familyName = reader["Option2FontName"].ToString();
					num = (short) reader["Option2FontSize"];
					style2 = (reader["Option2FontBold"].ToString() == "1") ? FontStyle.Bold : FontStyle.Regular;
					this.printFontOption2 = new Font(familyName, (float) num, style2);
				}
				reader.Close();
				reader = null;
				flag = true;
				command.CommandText = "getSlipContent";
				command.Parameters.Clear();
				command.Parameters.Add("@contentType", SqlDbType.VarChar).Value = "0";
				reader = command.ExecuteReader();
				while (reader.Read())
				{
					string str2 = reader["ContentType"].ToString();
					string str3 = reader["ContentText"].ToString();
					int num2 = (short) reader["Alignment"];
					switch (str2)
					{
						case "HEADER":
							this.headerText = str3.Replace("\r", "");
							this.headerAlignment = num2;
							break;

						case "FOOTER":
							this.footerText = str3.Replace("\r", "");
							this.footerAlignment = num2;
							break;
					}
				}
				reader.Close();
				reader = null;
			}
			catch (Exception)
			{
			}
			finally
			{
				if (reader != null)
				{
					reader.Close();
				}
				if (connection != null)
				{
					connection.Close();
				}
			}
			if (!flag)
			{
				this.printFontHeader = new Font("Courier New", 10f);
				this.printFontBody = new Font("Courier New", 10f);
				this.printFontOption1 = new Font("Courier New", 10f);
				this.printFontOption2 = new Font("Courier New", 10f);
			}
			this.printerName = CheckBillService.GetDescriptionByID(style + "_PRINTER");
		}

		public void Print()
		{
			printDoc.Print();
		}

		public void Clear()
		{
			lines.Clear();
		}

		public void Add(string text, int size, int alignment)
		{
			string[] strArray = text.Replace("\r", string.Empty).Split(new char[] { '\n' });
			for (int i = 0; i < strArray.Length; i++)
			{
				if ((strArray.Length <= 1) || (strArray[i].Trim() != string.Empty))
				{
					SlipLine line = new SlipLine(strArray[i], size, alignment);
					this.lines.Add(line);
				}
			}
			this.fontSize = size;
			this.textAlignment = alignment;
		}
		public PrintSlip(string style)
		{
			this.LoadConfig(style);
			this.printDoc = new PrintDocument();
			try
			{
				if ((this.printerName != null) && (this.printerName != ""))
				{
					this.printDoc.PrinterSettings.PrinterName = this.printerName;
				}
			}
			catch (Exception)
			{
			}
			this.printDoc.PrintPage += new PrintPageEventHandler(this.printDoc_PrintPage);
			this.lines = new ArrayList();
			this.fontSize = 1;
			this.textAlignment = 0;
		}

		public void Add(string text, int size)
		{
			this.Add(text, size, this.textAlignment);
		}

		public void Add(string text)
		{
			this.Add(text, this.fontSize, this.textAlignment);
		}

		public void AddHeader()
		{
			if (this.headerText != null)
			{
				string[] strArray = this.headerText.Split(new char[] { '\r', '\n' });
				for (int i = 0; i < strArray.Length; i++)
				{
					this.Add(strArray[i], 0, this.headerAlignment);
				}
			}
		}


		public void AddFooter()
		{
			if (this.footerText != null)
			{
				this.Add("", 0, this.footerAlignment);
				string[] strArray = this.footerText.Split(new char[] { '\r', '\n' });
				for (int i = 0; i < strArray.Length; i++)
				{
					this.Add(strArray[i], 0, this.footerAlignment);
				}
			}
		}


		public static float MeasureDisplayStringWidth(Graphics graphics, string text, Font font)
		{
			StringFormat stringFormat = new StringFormat();
			RectangleF layoutRect = new RectangleF(0f, 0f, 1000f, 1000f);
			CharacterRange[] ranges = new CharacterRange[] { new CharacterRange(0, text.Length) };
			Region[] regionArray = new Region[1];
			stringFormat.SetMeasurableCharacterRanges(ranges);
			return (graphics.MeasureCharacterRanges(text, font, layoutRect, stringFormat)[0].GetBounds(graphics).Right + 1f);
		}


		public static string[] TokenStringWidth(Graphics g, string text, Font font, float width)
		{
			ArrayList list = new ArrayList();
			list.Add(text);
			for (int i = 0; i < list.Count; i++)
			{
				string str = (string) list[i];
				int length = str.Length;
				while ((length > 0) && (MeasureDisplayStringWidth(g, str.Substring(0, length), font) > width))
				{
					length--;
				}
				if (length < str.Length)
				{
					list[i] = str.Substring(0, length);
					list.Add(str.Substring(length));
				}
			}
			return (string[]) list.ToArray(typeof(string));
		}

		public static string[] GetInstalledPrinter()
		{
			PrinterSettings.StringCollection installedPrinters = PrinterSettings.InstalledPrinters;
			if ((installedPrinters == null) || (installedPrinters.Count <= 0))
			{
				return null;
			}
			string[] strArray = new string[installedPrinters.Count];
			for (int i = 0; i < installedPrinters.Count; i++)
			{
				strArray[i] = installedPrinters[i];
			}
			return strArray;
		}

		private void printDoc_PrintPage(object sender, PrintPageEventArgs ev)
		{
			StreamWriter writer;
			Font printFontBody = this.printFontBody;
			StringFormat format = null;
			float x = 0f;
			float num2 = 0f;
			int num3 = 0;
			float num4 = 0.32f;
			int top = ev.MarginBounds.Top;
			float paperWidth = AppParameter.GetPaperWidth();
			try
			{
				writer = File.CreateText(@"D:\Projects\smartRestaurant\Output\print.txt");
			}
			catch (Exception)
			{
				writer = null;
			}
			for (num3 = 0; num3 < this.lines.Count; num3++)
			{
				SlipLine line = (SlipLine) this.lines[num3];
				switch (line.FontSize)
				{
					case 0:
						printFontBody = this.printFontHeader;
						break;

					case 1:
						printFontBody = this.printFontBody;
						break;

					case 2:
						printFontBody = this.printFontOption1;
						break;

					case 3:
						printFontBody = this.printFontOption2;
						break;
				}
				switch (line.TextAlignment)
				{
					case 0:
						x = num4;
						format = null;
						break;

					case 1:
						x = paperWidth / 2f;
						format = new StringFormat();
						format.Alignment = StringAlignment.Center;
						break;

					case 2:
						x = paperWidth;
						format = new StringFormat();
						format.Alignment = StringAlignment.Far;
						break;
				}
				if (line.Text == "-")
				{
					ev.Graphics.DrawLine(Pens.Black, 0f, num2, paperWidth, num2);
					if (writer != null)
					{
						writer.WriteLine("-------");
					}
				}
				else if (line.Text == "=")
				{
					ev.Graphics.DrawLine(Pens.Black, 0f, num2, paperWidth, num2);
					ev.Graphics.DrawLine(Pens.Black, 0f, num2 + 2f, paperWidth, num2 + 2f);
					if (writer != null)
					{
						writer.WriteLine("=======");
					}
				}
				else if (line.Text == "-XXX-")
				{
					ev.Graphics.FillRectangle(Brushes.Black, 0f, num2, paperWidth, printFontBody.GetHeight(ev.Graphics));
					if (writer != null)
					{
						writer.WriteLine("XXXXXXX");
					}
					num2 += printFontBody.GetHeight(ev.Graphics);
				}
				else
				{
					string[] strArray = TokenStringWidth(ev.Graphics, line.Text, printFontBody, paperWidth);
					for (int i = 0; i < strArray.Length; i++)
					{
						if (format == null)
						{
							ev.Graphics.DrawString(strArray[i], printFontBody, Brushes.Black, x, num2);
						}
						else
						{
							ev.Graphics.DrawString(strArray[i], printFontBody, Brushes.Black, x, num2, format);
						}
						if (writer != null)
						{
							writer.WriteLine(strArray[i]);
						}
						num2 += printFontBody.GetHeight(ev.Graphics);
					}
				}
			}
			if (writer != null)
			{
				writer.Close();
			}
			ev.HasMorePages = false;
		}

	}
}
