using System;
using Netron.Neon;
using System.IO;
using System.Reflection;
namespace WebBrowser
{
	/// <summary>
	/// Organizes the tabs and keeps track of the already opened ones so that
	/// a Url is opened only once.
	/// </summary>
	public class TabFactory
	{

		#region Fields
		public event TabInfo OnNewTab;
		public event TabInfo OnShowTab;
		private Mediator mediator;
		private TabCollection tabs;
		private string defaultHtml = "<font color=Red>Could not find a required resource in the assembly.</font>";
		
		#endregion

		#region Properties

		public TabCollection Tabs
		{
			get{return tabs;}
		}

		#endregion

		#region Constructor
		public TabFactory(Mediator mediator)
		{
			this.mediator = mediator;
			tabs = new TabCollection();
			Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("WebBrowser.Default.htm");
			StreamReader reader = new StreamReader(stream,System.Text.Encoding.ASCII);
			defaultHtml= reader.ReadToEnd();
			reader.Close();
			stream.Close();
			stream=null;
		}

		#endregion

		#region Methods
		/// <summary>
		/// Returns a tab page of a certain kind
		/// </summary>
		/// <param name="type">The type of tab (browser or favs)</param>
		/// <param name="identifier">the unique key</param>
		/// <returns></returns>
		public ITab Get(TabTypes type, string identifier)
		{
			ITab tab = null;
			if((tab=tabs[identifier])==null)
			{
				switch(type)
				{
					case TabTypes.Browser:
						return CreateBrowser(identifier);
					case TabTypes.Favorites:
						return CreateFavorites("Favorites");
					default:
						return null;
				}


			}
			else
			{
				if(OnShowTab!=null)
					OnShowTab(tab);
				return tab;
			}


		}

		/// <summary>
		/// Creates a new browser tab
		/// 
		/// </summary>
		/// <param name="identifier">The unique identifier of the browser tab is the url</param>
		/// <returns></returns>
		private ITab CreateBrowser(string identifier)
		{
			BrowserTab tab = new BrowserTab();
			tab.AxWebBrowser.NewWindow3+=new DWebBrowserEvents2_NewWindow3EventHandler(AxWebBrowser_NewWindow3);
			tab.AxWebBrowser.NewWindow2+=new DWebBrowserEvents2_NewWindow2EventHandler(AxWebBrowser_NewWindow2);			
			//you don't want to pile up unaccessible browsers!
			tab.Closing+=new System.ComponentModel.CancelEventHandler(tab_Closing);

			if(identifier=="Home")
			{
				tab.Identifier = "Home";
				tab.Navigate("about:blank");//this one seems really necessary
				tab.AxWebBrowser.Html = defaultHtml;
				tab.AxWebBrowser.ScriptObject = this.mediator;
			}
			else if(identifier==string.Empty)
			{
				tab.Identifier = "Empty";
				tab.Navigate("about:blank");//this one seems really necessary
			}
			else
			{
				tab.Identifier = identifier;
				tab.Navigate(identifier);
				
				
				

			}			
			tabs.Add(tab);
			if(OnNewTab!=null)
				OnNewTab(tab);
			return tab;
		}

		




		/// <summary>
		/// Creates the favs tab
		/// </summary>
		/// <param name="identifier"></param>
		/// <returns></returns>
		private ITab CreateFavorites(string identifier)
		{
			FavoritesTab tab = new FavoritesTab();
			tab.Identifier = identifier;
			tabs.Add(tab);
			tab.Closing+=new System.ComponentModel.CancelEventHandler(tab_Closing);
			tab.OnFavClick+=new URLHandler(tab_OnFavClick);
			tab.Text = "Favorites";
			tab.TabText = "Favorites";
			if(OnNewTab!=null)
				OnNewTab(tab);
			return tab;
		}

		

		/// <summary>
		/// On closing a tab it has to be removed from the collection
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void tab_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if(sender!=null)
				tabs.Remove(sender as ITab);
			MenuItemEx item=null;
			for(int k=0;k<mediator.parent.mnuWindows.MenuItems.Count;k++)
			{
				item = mediator.parent.mnuWindows.MenuItems[k] as MenuItemEx;
				if(item==null) continue;
				if(item.Tab!=null)
				{
					if(item.Tab==sender)
					{
						mediator.parent.mnuWindows.MenuItems.RemoveAt(k);
						break;
					}

				}
			}
		}

		/// <summary>
		/// Occurs when a new browser window is requested (either via shift-click or via target=_blank)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AxWebBrowser_NewWindow2(object sender, DWebBrowserEvents2_NewWindow2Event e)
		{
			if(e==null) return;
			IBrowserTab tab=	CreateBrowser("") as IBrowserTab;				
			//e.cancel = true; //doesn't seem to be necessary
			e.ppDisp = tab.AxWebBrowser.Application;			
			if(Environment.OSVersion.Version.Major > 4 	&& Environment.OSVersion.Version.Minor > 0 )
				e.cancel = true; //necessary on WinXP since the NewWindow3 is raised as well in that case
		
			/*
				PlatformID			MajorVersion  Minor Version  Operating System 
				Win32Windows      >= 4				0						Win95 
				Win32Windows      >= 4				 > 0 && < 90       Win98 
				Win32Windows      >= 4				 > 0 && >= 90     WinMe 
				Win32NT				  <= 4			 0						WinNT 
				Win32NT  5           5					0						Win2K 
				Win32NT  5           5					> 0					WinXP 
			*/

		}

		/// <summary>
		/// Occurs only on WinXP
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AxWebBrowser_NewWindow3(object sender, DWebBrowserEvents2_NewWindow3Event e)
		{
			if(e==null) return;
			if(e.url ==null) return;
			if(this.tabs[e.url]==null)
			{
				CreateBrowser(e.url);
				e.cancel = true;
			}
			
		}
		#endregion

		/// <summary>
		/// Opens a new browser tab when a favorite is clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void tab_OnFavClick(object sender, URLInfo e)
		{
			this.Get(TabTypes.Browser,e.URL);
		}
	}

}
