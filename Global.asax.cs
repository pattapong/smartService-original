using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;

namespace SmartService 
{
	/// <summary>
	/// Summary description for Global.
	/// </summary>
	public class Global : System.Web.HttpApplication
	{
		public Global()
		{
			this.InitializeComponent();
		}

		protected void Application_AuthenticateRequest(object sender, EventArgs e)
		{
		}

		protected void Application_BeginRequest(object sender, EventArgs e)
		{
		}

		protected void Application_End(object sender, EventArgs e)
		{
		}

		protected void Application_EndRequest(object sender, EventArgs e)
		{
		}

		protected void Application_Error(object sender, EventArgs e)
		{
		}

		protected void Application_Start(object sender, EventArgs e)
		{
		}

		private void InitializeComponent()
		{
		}

		protected void Session_End(object sender, EventArgs e)
		{
		}

		protected void Session_Start(object sender, EventArgs e)
		{
		}

	}
}

