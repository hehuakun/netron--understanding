using System;
using System.Windows.Forms;
namespace WebBrowser
{
	/// <summary>
	/// 
	/// </summary>
	public class MenuItemEx : MenuItem
	{
		ITab tab;

		public ITab Tab
		{
			get{return tab;}
			set{tab = value;}
		}

		public MenuItemEx(string text, EventHandler handler) : base(text, handler)
		{
			
		}
	}
}
