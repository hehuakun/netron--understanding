using System;
using System.Collections;
using System.Windows.Forms;
using System.Configuration;
using Netron.Neon;
namespace Netron.Cobalt
{
	/// <summary>
	/// Tab factory, handles the creation and uniqueness of tabs (docking forms)
	/// </summary>
	public class TabFactory
	{

		#region Events
		public event TabInfo OnTabCreation;
		#endregion
		
		#region Fields
		private Mediator mediator;

		private PropertiesTab propsTab;

		private OutputExTab outputTab;

		private BrowserTab browserTab;

		private GraphTab graphTab;

		private ZoomTab zoomTab;

		private ShapeViewerTab shapesTab;

		private FavTab favTab;

		private ChmTocTab chmTocTab;

		private CodeTab codeTab;

//		private BugTab bugTab;

		private DiagramBrowserTab diagramBrowserTab;
		
		#endregion

		#region Properties

		#region TabAccess

		public DiagramBrowserTab DiagramBrowserTab
		{
			get
			{
				if(diagramBrowserTab==null)
					CreateDiagramBrowserTab(new TabCodon("Diagram browser","Diagram browser", TabTypes.DiagramBrowser));
				return diagramBrowserTab;

			}
		}

		public ChmTocTab ChmTocTab
		{
			get
			{
				if(chmTocTab==null)
					CreateChmTocTab(new TabCodon("Help content","Help content", TabTypes.ChmToc));
				return chmTocTab;

			}
		}

		public CodeTab CodeTab
		{
			get
			{
				if(codeTab==null)
					CreateCodeTab(new TabCodon("Editor","Editor", TabTypes.Code));
				return codeTab;

			}
		}

//		public BugTab BugTab
//		{
//			get
//			{
//				if(bugTab==null)
//					CreateBugLoggerTab(new TabCodon("Bug reporting","Bug reporting", TabTypes.BugTab));
//				return bugTab;
//
//			}
//		}
		
		public BrowserTab BrowserTab
		{
			get
			{
				if(browserTab==null)
					CreateBrowserTab(new TabCodon("Browser","Browser", TabTypes.Browser));
				return browserTab;
			}
		}


		public PropertiesTab PropertiesTab
		{
			get{
				if(propsTab==null)
					CreatePropertiesTab(new TabCodon("Properties","Properties", TabTypes.PropertyGrid));
				return propsTab;}
		}

		public OutputExTab OutputTab
		{
			get{
				if(outputTab==null)
					CreateOutputTab(new TabCodon("Output", "Output",TabTypes.Output));
				return outputTab;}
		}

		public GraphTab GraphTab
		{
			get
			{
				if(graphTab==null)
					CreateGraphTab(new TabCodon("Diagram", "Diagram", TabTypes.NetronDiagram));
				return graphTab;}
		}

		public FavTab ShapeFavoritesTab
		{
			get
			{
				if(favTab==null)
					CreateShapeFavoritesTab(new TabCodon("Templates",TabTypes.ShapeFavorites));
				return favTab;}
		}

		public ShapeViewerTab ShapeViewerTab
		{
			get
			{
				if(shapesTab==null)
					CreateShapesTab(new TabCodon("Shapes", "Shapes", TabTypes.ShapesViewer));
				return shapesTab;
			}
		}

		public ZoomTab ZoomTab
		{
			get
			{
				if(this.zoomTab==null)
					CreateZoomTab(new TabCodon("Zoom", "Zoom", TabTypes.DiagramZoom));
				return zoomTab;
			}
		}
		
		#endregion

		#region Control Access

	
		#endregion


		
		#endregion

		#region Constructor
		public TabFactory(Mediator mediator)
		{
			this.mediator = mediator;
		}
		#endregion

		#region Methods

		public object GetTab(TabCodon codon)
		{
			switch(codon.TabType)
			{			
				case TabTypes.DiagramBrowser:
					if(diagramBrowserTab==null)
						CreateDiagramBrowserTab(codon);
					return diagramBrowserTab;	
				case TabTypes.Browser:
					if(browserTab==null)
						CreateBrowserTab(codon);
					return browserTab;	
				case TabTypes.ChmToc:
					if(chmTocTab==null)
						CreateBrowserTab(codon);
					return chmTocTab;
				case TabTypes.Output:
					if(outputTab==null)
						CreateOutputTab(codon);
					return outputTab;				
				case TabTypes.PropertyGrid:
					if(propsTab==null)
						CreatePropertiesTab(codon);
					return propsTab;
				case TabTypes.NetronDiagram:
					if(graphTab==null)
						CreateGraphTab(codon);
					return graphTab;
				case TabTypes.DiagramZoom:
					if(zoomTab==null)
						CreateZoomTab(codon);
					return zoomTab;
				case TabTypes.ShapesViewer:
					if(shapesTab == null)
						CreateShapesTab(codon);
					return shapesTab;
				case TabTypes.ShapeFavorites:
					if(favTab == null)
						CreateShapeFavoritesTab(codon);
					return favTab;
//				case TabTypes.BugTab:
//					if(bugTab == null)
//						CreateBugLoggerTab(codon);
//					return bugTab;
				case TabTypes.Code:
					if(codeTab == null)
						CreateCodeTab(codon);
					return codeTab;
			}
			return null;
		}

		private void RaiseNewTab(DockContent tab)
		{
			if(OnTabCreation!=null)
				OnTabCreation(tab);
		}

		

		private void CreatePropertiesTab(TabCodon codon)
		{
			propsTab = new PropertiesTab(this.mediator);
			propsTab.Text = codon.Text;
			propsTab.TabIdentifier = codon.CodonName;
			RaiseNewTab(propsTab);

		}

		private void CreateGraphTab(TabCodon codon)
		{
			graphTab = new GraphTab(this.mediator);
			graphTab.Text = codon.Text;
			graphTab.TabIdentifier = codon.CodonName;
			RaiseNewTab(graphTab);
			//create the stamper and couple it
			if(zoomTab==null)
				CreateZoomTab(new TabCodon("Zoom", "Zoom", TabTypes.DiagramZoom));
			//connect the stamper to the graph control
			zoomTab.GraphStamp.GraphControl = 
				zoomTab.GraphControl=graphTab.GraphControl;	
		}

		private void CreateZoomTab(TabCodon codon)
		{
			zoomTab = new ZoomTab(this.mediator);
			zoomTab.Text = codon.Text;
			zoomTab.TabIdentifier = codon.CodonName;
			RaiseNewTab(zoomTab);
			
		}
		private void CreateShapesTab(TabCodon codon)
		{
			shapesTab = new ShapeViewerTab(this.mediator);
			shapesTab.Text = codon.Text;
			shapesTab.TabIdentifier = codon.CodonName;
			RaiseNewTab(shapesTab);
			
		}

		private void CreateShapeFavoritesTab(TabCodon codon)
		{
			favTab = new FavTab(this.mediator);
			favTab.Text = codon.Text;
			favTab.TabIdentifier = codon.CodonName;
			string defaultFolder = ConfigurationSettings.AppSettings.Get("TemplateBrowserInitialDirectory");
			if(defaultFolder!=string.Empty)
			{
					favTab.LoadDirectory(defaultFolder);
					favTab.DefaultDirectory = defaultFolder;
			}
			RaiseNewTab(favTab);
			
		}

	

		private void CreateOutputTab(TabCodon codon)
		{
			outputTab = new OutputExTab(this.mediator);
			outputTab.TabIdentifier = codon.CodonName;
			outputTab.Text = codon.Text;			
			RaiseNewTab(outputTab);
		}
//		private void CreateBugLoggerTab(TabCodon codon)
//		{
//			bugTab = new BugTab(this.mediator);
//			bugTab.TabIdentifier = codon.CodonName;
//			bugTab.Text = codon.Text;			
//			RaiseNewTab(bugTab);
//		}
		private void CreateBrowserTab(TabCodon codon)
		{
			browserTab = new BrowserTab(this.mediator);
			browserTab.TabIdentifier = codon.CodonName;
			browserTab.Text = codon.Text;			
			RaiseNewTab(browserTab);
		}

		private void CreateDiagramBrowserTab(TabCodon codon)
		{
			diagramBrowserTab = new DiagramBrowserTab(mediator);
			diagramBrowserTab.TabIdentifier = codon.CodonName;
			diagramBrowserTab.Text = codon.Text;
			string initialPath = ConfigurationSettings.AppSettings.Get("DiagramBrowserInitialDirectory");
			if(initialPath!=string.Empty)
				diagramBrowserTab.LoadDirectory(initialPath, false);
			diagramBrowserTab.Thumbnails = true;//set to small size initially
			RaiseNewTab(diagramBrowserTab);
		}

		private void CreateChmTocTab(TabCodon codon)
		{
			chmTocTab = new ChmTocTab(mediator);
			chmTocTab.TabIdentifier = codon.CodonName;
			chmTocTab.Text = codon.Text;			
			RaiseNewTab(chmTocTab);
		}

		private void CreateCodeTab(TabCodon codon)
		{
			codeTab = new CodeTab(mediator);
			codeTab.TabIdentifier = codon.CodonName;
			codeTab.Text = codon.Text;			
			RaiseNewTab(codeTab);
		}


		#endregion
	
	}

	/// <summary>
	/// Passes the tab created, to be added on a higher level in a TabControl
	/// </summary>
	public delegate void TabInfo(DockContent tab );


}
