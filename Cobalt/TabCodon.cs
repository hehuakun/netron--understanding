using System;

namespace Netron.Cobalt
{
	/// <summary>
	/// Summary description for TabCodon.
	/// </summary>
	public class TabCodon : CodonBase
	{

		private TabTypes tabType;
		private string text;

		public string Text
		{
			get{return text;}
			set{text = value;}
		}
		public TabTypes TabType
		{
			get{return tabType;}
			set{tabType = value;}
		}


		public TabCodon(string name, TabTypes type) : base(name)
		{
			tabType = type;
		}

		public TabCodon(string name,string text, TabTypes type) : base(name)
		{
			tabType = type;
			this.text = text;
		}
	}

	/// <summary>
	/// The various tab types used in this project
	/// </summary>
	public enum TabTypes
	{
		DataGrid,
		World,
		BugTab,
		Browser,
		ChmToc,
		Code,
		NetronDiagram,
		EngineSettings,
		Chart,
		Project,
		PropertyGrid, 
		Output, 
		Trace,
		DiagramZoom,
		ShapesViewer,
		ShapeFavorites,
		DiagramBrowser,
		Unknown
	}
}
