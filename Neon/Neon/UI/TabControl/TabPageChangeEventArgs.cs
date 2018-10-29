using System;
using System.Windows.Forms;
namespace Netron.Neon
{

	public class TabPageChangeEventArgs : EventArgs
	{
		private TabPage _Selected = null;
		private TabPage _PreSelected = null;
		public bool Cancel = false;

		public TabPage CurrentTab
		{
			get
			{
				return _Selected;
			}
		}


		public  TabPage NextTab
		{
			get
			{
				return _PreSelected;
			}
		}


		public TabPageChangeEventArgs(TabPage CurrentTab, TabPage NextTab)
		{
			_Selected = CurrentTab;
			_PreSelected = NextTab;
		}

	



	}
}
