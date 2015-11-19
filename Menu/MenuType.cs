using System;

namespace SmartService.Menu
{
	/// <summary>
	/// Summary description for MenuType.
	/// </summary>
	public class MenuType
	{
		// Fields
		public string Description;
		public int ID;
		private MenuItem[] menuItems;
		public string Name;
		public double Tax1;
		public double Tax2;

		// Methods
		public MenuType()
		{
		}

		public MenuType(int id, string name, string desc)
		{
			this.ID = id;
			this.Name = name;
			this.Description = desc;
			this.Tax1 = 0.0;
			this.Tax2 = 0.0;
			this.MenuItems = null;
		}

		// Properties
		public MenuItem[] MenuItems
		{
			get
			{
				return this.menuItems;
			}
			set
			{
				this.menuItems = value;
			}
		}
	}
}
