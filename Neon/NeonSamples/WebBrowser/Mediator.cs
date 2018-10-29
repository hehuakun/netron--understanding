using System;
using System.Windows.Forms;
using Netron.Neon;
using DockingExtenders = Netron.Neon.Docking.Extenders;
namespace WebBrowser
{
	/// <summary>
	/// 
	/// </summary>
	public class Mediator
	{
		internal MainForm parent;
		TabFactory tabFactory;
		public Mediator(MainForm parent)
		{
			this.parent = parent;
			tabFactory = new TabFactory(this);
			tabFactory.OnNewTab+=new TabInfo(tabFactory_OnNewTab);
			tabFactory.OnShowTab+=new TabInfo(tabFactory_OnShowTab);
		}


		public ITab GetTab(TabTypes type, string identifier)
		{
			return tabFactory.Get(type, identifier);
		}


		private void tabFactory_OnNewTab(ITab tab)
		{
			switch(tab.TabType)
			{
				case TabTypes.Favorites:
					tab.Show(parent.dockPanel,DockState.DockRightAutoHide);
					break;
				case TabTypes.Browser:
					tab.Show(parent.dockPanel,DockState.Document);
					BrowserTab fulltab = tab as BrowserTab;
					fulltab.OnDocumentComplete+=new WebBrowser.BrowserTab.BrowserTabInfo(OnBrowserDocumentComplete);
					break;
			}
			
		}

		private void WindowsClick(object sender, EventArgs e)
		{
			MenuItemEx item = sender as MenuItemEx;
			item.Tab.Show(parent.dockPanel);
		}

		private void tabFactory_OnShowTab(ITab tab)
		{
			tab.Show(parent.dockPanel);
		}

		/// <summary>
		/// A custom method called by an Html page from within JavaScript
		/// </summary>
		public void CustomMethod()
		{
			tabFactory.Get(TabTypes.Browser, "http://netron.sf.net");
		}

		public void CloseAll()
		{
			while(tabFactory.Tabs.Count>0)			
			{				
				tabFactory.Tabs[0].Close();				
			}
			tabFactory.Tabs.Clear();
		}
		public void ChangeUI(string type)
		{
			
			CloseAll();
			
			

			if(type.ToLower()=="neon")
			{
				parent.dockPanel.Extender.AutoHideTabFactory = new DockingExtenders.Blue.Extender.AutoHideTabFromBaseFactory();
				parent.dockPanel.Extender.DockPaneTabFactory = new DockingExtenders.Blue.Extender.DockPaneTabFromBaseFactory();
				parent.dockPanel.Extender.AutoHideStripFactory = new DockingExtenders.Blue.Extender.AutoHideStripFromBaseFactory();
				parent.dockPanel.Extender.DockPaneCaptionFactory = new DockingExtenders.Blue.Extender.DockPaneCaptionFromBaseFactory();
				parent.dockPanel.Extender.DockPaneStripFactory = new DockingExtenders.Blue.Extender.DockPaneStripFromBaseFactory();
				parent.Refresh();
			}
			else
			{
				parent.dockPanel.Extender.AutoHideTabFactory = null;
				parent.dockPanel.Extender.DockPaneTabFactory = null;
				parent.dockPanel.Extender.AutoHideStripFactory = null;
				parent.dockPanel.Extender.DockPaneCaptionFactory = null;
				parent.dockPanel.Extender.DockPaneStripFactory = null;
				parent.Refresh();
			}
			this.tabFactory.Get(TabTypes.Favorites,"Favorites");
			this.tabFactory.Get(TabTypes.Browser,"");


		}

		private void OnBrowserDocumentComplete(BrowserTab tab)
		{
			//check if already in the list
			for(int k =0; k<parent.mnuWindows.MenuItems.Count;k++)
			{
				if(parent.mnuWindows.MenuItems[k].Text== tab.Text)
					return;
			}

			MenuItemEx item = new MenuItemEx(tab.Text, new EventHandler(WindowsClick));
			item.MergeType = MenuMerge.MergeItems;
			item.Tab = tab;
			parent.mnuWindows.MenuItems.Add(item);
		}
	}

	
	



	public enum TabTypes
	{
		Browser,
		Favorites
	}

	public delegate void TabInfo(ITab tab);
}
