using System;

namespace SmartService.Menu
{
	/// <summary>
	/// Summary description for MenuItem.
	/// </summary>
	public class MenuItem
	{
		public string Description;
		public int ID;
		public int KeyID;
		public string KeyIDText;
		public MenuDefault[] MenuDefaults;
		public string Name;
		public double Price;
		public int TypeID;


		public MenuItem()
		{
		}

		public MenuItem(MenuType type, int id, int keyID, string keyIDTxt, string name, string desc, double price)
		{
			this.TypeID = type.ID;
			this.ID = id;
			this.KeyID = keyID;
			this.KeyIDText = keyIDTxt;
			this.Name = name;
			this.Description = desc;
			this.Price = price;
			this.MenuDefaults = null;
		}

	}
}
