using System;
using Netron.Neon;
namespace WebBrowser
{
	/// <summary>
	/// Tags the tabs
	/// </summary>
	public interface ITab
	{
		/// <summary>
		/// Gets or sets the unique identifier of the tab
		/// </summary>
		string Identifier {get; set;}
		/// <summary>
		/// Gets the tab type
		/// </summary>
		TabTypes TabType {get;}
		/// <summary>
		/// Shows the panel
		/// </summary>
		/// <param name="dockPanel">The DockPanel on the parent</param>
		/// <param name="dockState">The docking state</param>
		void Show(DockPanel dockPanel, DockState dockState);
		/// <summary>
		/// Shows the panel in the last known state
		/// </summary>
		/// <param name="dockPanel">The DockPanel on the parent</param>
		void Show(DockPanel dockPanel);

		void Close();

		string Text {get; set;}

	}

	/// <summary>
	/// The interface of the fav tab
	/// </summary>
	public interface IFavTab : ITab
	{
		/// <summary>
		/// When a favorite is clicked
		/// </summary>
		event URLHandler OnFavClick;	
	}

	public interface IBrowserTab : ITab
	{
		/// <summary>
		/// Get the AxBrowser object of the tab
		/// </summary>
		AxWebBrowser AxWebBrowser{get;}
		/// <summary>
		/// Navigates to the given url
		/// </summary>
		/// <param name="url">internet location</param>
		void Navigate(string url);
	}

}
