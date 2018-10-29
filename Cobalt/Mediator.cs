using System;
using GL = Netron.GraphLib;
using System.Windows.Forms;
using Netron.Neon;
using System.Drawing;
using Microsoft.Win32;
using Netron.Neon.HtmlHelp.ChmDecoding;
using Netron.Neon.HtmlHelp;
using Netron.Neon.HtmlHelp.UIComponents;
using Netron.Neon.TextEditor;
using System.Xml;
namespace Netron.Cobalt
{
	/// <summary>
	/// Summary description for Mediator.
	/// </summary>
	public class Mediator
	{

		
		#region Fields

		#region Help system fields
		private DumpingInfo _dmpInfo=null;
		private HtmlHelpSystem _reader = null;
		string _prefURLPrefix = "mk:@MSITStore:";
		bool _prefUseHH2TreePics = false;
		private string LM_Key = @"Software\Netron\HtmlHelpViewer\";

		string _prefDumpOutput="";
		DumpCompression _prefDumpCompression = DumpCompression.Medium;
		private InfoTypeCategoryFilter _filter = new InfoTypeCategoryFilter();
		DumpingFlags _prefDumpFlags = DumpingFlags.DumpBinaryTOC | DumpingFlags.DumpTextTOC | 
			DumpingFlags.DumpTextIndex | DumpingFlags.DumpBinaryIndex | 
			DumpingFlags.DumpUrlStr | DumpingFlags.DumpStrings;

		#endregion

		/// <summary>
		/// General purpose randomizer for the graph examples
		/// </summary>
		protected Random rnd;
		private TabFactory tabFactory;

		private SampleFactory samplesFactory;
		internal MainForm parent;
		private DockContent lastAdded;
	
		private DockPanel dockPanel;
		

		#endregion

		#region Properties

		public DockPanel DockPanel
		{
			get{return dockPanel;}
			set{dockPanel = value;}
		}


		#region Tab controls
		public Netron.GraphLib.UI.GraphControl GraphControl
		{
			get{
				if(tabFactory.GraphTab==null)
					return null;
				else
					return tabFactory.GraphTab.GraphControl;
			}
		}


		public TextEditorControl Editor
		{
			get{
				if(tabFactory.CodeTab==null)
					return null;
				else
					return tabFactory.CodeTab.Editor;
			}
		}

		public PropertyGrid Properties
		{
			get
			{
				if(tabFactory.PropertiesTab==null)
					return null;
				else
					return tabFactory.PropertiesTab.PropertyGrid;
			}
		}


		public NBrowser Browser
		{
			get
			{
				if(tabFactory.BrowserTab==null)
					return null;
				else
					return tabFactory.BrowserTab.Browser;
			}
		}


		

		public TocTree TocTree
		{
			get{
				if(tabFactory.ChmTocTab==null)
					return null;
				else
					return tabFactory.ChmTocTab.TocTree;
				}
		}

	

	
		public NOutput OutputBox
		{
			get{return tabFactory.OutputTab.Output;}
		}


		#endregion

		#region Tabs

		public BrowserTab BrowserTab
		{
			get{return tabFactory.BrowserTab;}
		}

//		public BugTab BugTab
//		{
//			get{return tabFactory.BugTab;}
//		}

		public CodeTab CodeTab
		{
			get{return tabFactory.CodeTab;}
		}

		public DiagramBrowserTab DiagramBrowserTab
		{
			get{return tabFactory.DiagramBrowserTab;}
		}

		public GraphTab GraphTab
		{
			get{return tabFactory.GraphTab;}
		}
		public ChmTocTab ChmTocTab
		{
			get{return tabFactory.ChmTocTab;}
		}

		public FavTab FavTab
		{
			get{return tabFactory.ShapeFavoritesTab;}
		}

		public OutputExTab OutputTab 
		{
			get{return tabFactory.OutputTab;}
		}
		public ZoomTab ZoomTab
		{
			get{return tabFactory.ZoomTab;}
		}

		public PropertiesTab PropertiesTab
		{
			get{return tabFactory.PropertiesTab;}
		}

		public ShapeViewerTab ShapeViewerTab
		{
			get{return tabFactory.ShapeViewerTab;}
		}

		#endregion

		public StatusBar StatusBar
		{
			get{return parent.sb;}
		}

		public Random Randomizer
		{
			get{return rnd;}
		}
		#endregion

		#region Constructor
		public Mediator(MainForm main)
		{
			this.parent = main;
		
			tabFactory = new TabFactory(this);
			tabFactory.OnTabCreation+=new TabInfo(OnTabCall);

			samplesFactory = new SampleFactory(this);

			//init the randomizer for the placement of shapes on the canvas
			rnd = new Random();


		}
		#endregion

		#region Methods

		public void SetCaption(string text)
		{
			parent.SetCaption(text);
		}


		public void SetLayoutAlgorithm(GL.GraphLayoutAlgorithms algorithm)
		{
			//change the UI
			parent.SetLayoutAlgorithmUIState(algorithm);
			//set the control
			this.GraphControl.GraphLayoutAlgorithm = algorithm;
		}
		
		/// <summary>
		/// Outputs messages from the graphcontrol to the output-tab
		/// </summary>
		/// <param name="msg"></param>
		public void Output(string msg)
		{
			//tabFactory.OutputTab.Show();
			OutputBox.WriteLine("Default", msg);
		}
		/// <summary>
		/// Outputs messages from the graphcontrol to the output-tab
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="level"></param>
		public void Output(string msg, GL.OutputInfoLevels level)
		{
			//tabFactory.OutputTab.Show();
			OutputBox.WriteLine(level.ToString(), msg);
		}
		
		#region Opening of the various tabs


		public void OpenOuputTab()
		{
			lastAdded = tabFactory.GetTab(new TabCodon("Output", "Output", TabTypes.Output)) as OutputExTab;
			OnShowTab(lastAdded);
		}

		public void OpenGraphTab()
		{
			lastAdded = tabFactory.GetTab(new TabCodon("Diagram", "Diagram", TabTypes.NetronDiagram)) as GraphTab;
			OnShowTab(lastAdded);
		}

//		public void OpenBugTab()
//		{
//			lastAdded = tabFactory.GetTab(new TabCodon("Bug reporting", "Bug reporting", TabTypes.BugTab)) as BugTab;
//			OnShowTab(lastAdded);
//		}

		public void OpenBrowserTab()
		{
			lastAdded = tabFactory.GetTab(new TabCodon("Browser", "Browser", TabTypes.Browser)) as BrowserTab;
			OnShowTab(lastAdded);
		}
		public void OpenZoomTab()
		{
			lastAdded = tabFactory.GetTab(new TabCodon("Zoom", "Zoom", TabTypes.DiagramZoom)) as ZoomTab;
			OnShowTab(lastAdded);
		}
		public void OpenShapesTab()
		{
			lastAdded = tabFactory.GetTab(new TabCodon("Shapes", "Shapes", TabTypes.ShapesViewer)) as ShapeViewerTab;
			OnShowTab(lastAdded);
		}

		public void OpenFavsTab()
		{
			lastAdded = tabFactory.GetTab(new TabCodon("Templates", "Templates", TabTypes.ShapeFavorites)) as FavTab;
			OnShowTab(lastAdded);
		}

		public void OpenDiagramBrowserTab()
		{
			lastAdded = tabFactory.GetTab(new TabCodon("Diagram browser", "Diagram browser", TabTypes.DiagramBrowser)) as DiagramBrowserTab;
			OnShowTab(lastAdded);
		}

		public void OpenChmTocTab()
		{
			lastAdded = tabFactory.GetTab(new TabCodon("Help content", "Help content", TabTypes.ChmToc)) as ChmTocTab;
			OnShowTab(lastAdded);
		}

		public void OpenCodeTab()
		{
			lastAdded = tabFactory.GetTab(new TabCodon("Editor", "Editor", TabTypes.Code)) as CodeTab;
			OnShowTab(lastAdded);
		}
	

		public void OpenPropsTab()
		{
			lastAdded = tabFactory.GetTab(new TabCodon("Properties", "Properties",TabTypes.PropertyGrid)) as PropertiesTab;
			(lastAdded as PropertiesTab).PropertyGrid.LineColor = MainForm.LightLightColor;
			(lastAdded as PropertiesTab).PropertyGrid.CommandsBackColor = MainForm.LightLightColor;
			(lastAdded as PropertiesTab).PropertyGrid.ViewBackColor = MainForm.LightLightColor;
			OnShowTab(lastAdded);
		}
		




		#endregion

		#region Sample handling

		
		public void LoadSample(Samples sample)
		{
			try
			{
				ISample spl = samplesFactory.GetSample(sample);
				if(spl==null)
					return;
				ResetDefault();
				spl.Run();
			}
			catch(Exception exc)
			{
				Output(exc.Message,GL.OutputInfoLevels.Exception);
			}
		}

		
		/// <summary>
		/// Resets the default state of the graph control
		/// </summary>
		private void ResetDefault()
		{
			this.OpenGraphTab();
			GraphControl.NewDiagram(true);
			GraphControl.Layers.Clear();
			GraphControl.AllowAddConnection = true;
			GraphControl.AllowAddShape = true;
			GraphControl.AllowDeleteShape = true;
			GraphControl.AllowMoveShape =true;
			GraphControl.BackgroundType = GL.CanvasBackgroundType.FlatColor;
			GraphControl.BackColor = Color.WhiteSmoke;
			GraphControl.Snap = false;
			GraphControl.ShowGrid = false;
			GraphControl.GraphLayoutAlgorithm = GraphLib.GraphLayoutAlgorithms.SpringEmbedder;
			GraphControl.DefaultConnectionPath = "Default";

			GraphControl.GraphLayoutAlgorithm = GL.GraphLayoutAlgorithms.SpringEmbedder;
			parent.SetLayoutAlgorithmUIState(GL.GraphLayoutAlgorithms.SpringEmbedder);
		}
		/// <summary>
		/// Adds a certain amount of random nodes
		/// </summary>
		/// <param name="amount"></param>
		public void AddRandomNodes(int amount)
		{
			if(amount<1) return;
			GL.Shape shape1, shape2;
			if(GraphControl.Shapes.Count==0)
			{
				shape1 = GraphControl.AddBasicShape("Root", new Point(100,100));
				amount--;
			}
			
			for(int k=0; k<amount;k++)
			{
				shape1 = GraphControl.Shapes[rnd.Next(0,GraphControl.Shapes.Count-1)];
				shape2 = GraphControl.AddBasicShape("Random " + k, new Point(rnd.Next(20,GraphControl.Width-70),rnd.Next(20,GraphControl.Height-30)));
				Connect(shape1, shape2);
			}
		}

		#endregion
		#region Shape creation utils
		public void SetShape(GL.Shape shape)
		{
			shape.X = Randomizer.Next(50,GraphControl.Width-100);
			shape.Y = Randomizer.Next(50,GraphControl.Height-20);
		}

		public GL.Connection Connect(GL.Shape s1, GL.Shape s2)
		{
			return GraphControl.AddConnection(s1.Connectors[0], s2.Connectors[0]);	
		}
		public GL.Connection Connect(GL.Shape s1, GL.Shape s2, GL.ConnectionEnd lineEnd)
		{
			GL.Connection con = GraphControl.AddConnection(s1.Connectors[0], s2.Connectors[0]);	
			con.LineEnd = lineEnd;
			return con;
		}
		public GL.Connection Connect(GL.Shape s1, GL.Shape s2, string linePath)
		{
			GL.Connection con = GraphControl.AddConnection(s1.Connectors[0], s2.Connectors[0]);	
			con.LinePath = linePath;
			return con;
		}

		public GL.Connection Connect(GL.Shape s1, GL.Shape s2, string linePath, GL.ConnectionEnd lineEnd)
		{
			GL.Connection con = GraphControl.AddConnection(s1.Connectors[0], s2.Connectors[0]);	
			con.LinePath = linePath;
			con.LineEnd = lineEnd;
			return con;
		}

		#endregion

		private void OnTabCall(DockContent tab)
		{
			TabTypes type = (tab as ICobaltTab).TabType;
			
			switch(type)
			{
				case TabTypes.Code: case TabTypes.NetronDiagram: case TabTypes.DataGrid: case TabTypes.World: case TabTypes.Browser:
				case TabTypes.Chart: case TabTypes.DiagramBrowser:
					tab.Show(dockPanel,DockState.Document);
					break;
				case TabTypes.Project: case TabTypes.PropertyGrid: 
					tab.Show(dockPanel,DockState.DockRight);
					break;
				case TabTypes.Trace: case TabTypes.Output:
					tab.Show(dockPanel,DockState.DockBottom);
					break;
				case TabTypes.ShapesViewer: case TabTypes.ShapeFavorites:
					tab.Show(dockPanel, DockState.DockLeft);
					break;
			}
		}
		private void OnShowTab(DockContent tab)
		{
			if(tab==null) return;
			TabTypes type = (tab as ICobaltTab).TabType;
			tab.Show(dockPanel);
			//tab.MdiParent = this.parent;
			
		}
		public void ShowProperties(object obj)
		{
			if(Properties==null)
				OpenPropsTab();

			this.Properties.SelectedObject = obj;
		}
		public void ShowProperties(object[] obj)
		{
			if(Properties==null)
				OpenPropsTab();

			this.Properties.SelectedObjects = obj;
		}

		public void Navigate(string url)
		{
			if(Browser==null)
				OpenBrowserTab();
			this.Browser.Navigate(url);
		}

		#endregion

		/// <summary>
		/// Hides all tha tabpages from the interface
		/// </summary>
		public void HideAllTabs()
		{
			foreach(DockContent dp in dockPanel.Contents)
			{
				dp.Hide();
			}
		}
		/// <summary>
		/// Opens a file in the editor
		/// </summary>
		public void OpenTextFile()
		{
			OpenFileDialog fileChooser= new OpenFileDialog();
			fileChooser.Filter = "All files (*.*)|*.*| NML files (*.NML)|*.NML";
			fileChooser.FilterIndex = 2;//choose NML by default
			DialogResult result =fileChooser.ShowDialog();
			string filename;			
			if(result==DialogResult.Cancel) return;
			filename=fileChooser.FileName;
			if (filename=="" || filename==null)
				MessageBox.Show("Invalid file name","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
			else
			{
				Editor.LoadFile(filename);
				if(filename.ToLower().EndsWith("nml"))				
					Editor.SetHighlighting("XML");				
				else if(filename.ToLower().EndsWith("cs"))
					Editor.SetHighlighting("C#");
				else if(filename.ToLower().EndsWith(".vb"))
					Editor.SetHighlighting("VBNET");
				else if(filename.ToLower().EndsWith(".htm"))
					Editor.SetHighlighting("HTML");
				else if(filename.ToLower().EndsWith(".html"))
					Editor.SetHighlighting("HTML");
				else
					Editor.SetDefaultHighlighting();
				
				TestTextForNML();
			}
		}

		/// <summary>
		/// Saves the content of the editor
		/// </summary>
		public void SaveTextFile()
		{
			SaveFileDialog fileChooser=new SaveFileDialog();
			fileChooser.CheckFileExists=false;
			fileChooser.Filter = "All files (*.*)|*.*| NML files (*.NML)|*.NML";
			fileChooser.InitialDirectory = "\\c:";
			fileChooser.RestoreDirectory = true;
			DialogResult result =fileChooser.ShowDialog();
			string filename;
			
			if(result==DialogResult.Cancel) return;
			filename=fileChooser.FileName;
			if (filename=="" || filename==null)
				MessageBox.Show("Invalid file name","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
			else
			{
				Editor.SaveFile(filename);				
			}
		}

		/// <summary>
		/// Tests if the loaded text is an XML file with NML-root
		/// </summary>
		internal void TestTextForNML()
		{
			try
			{
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(Editor.Text);
				if(doc.DocumentElement.Name.ToLower()=="nml")
					parent.mnuShowNMLInDiagram.Enabled = true;
			}
			catch
			{
				parent.mnuShowNMLInDiagram.Enabled = false;
			}
		}


		private void ChmInit()
		{
			_reader = new HtmlHelpSystem();
			HtmlHelpSystem.UrlPrefix = "mk:@MSITStore:";
			// use temporary folder for data dumping
			string sTemp = System.Environment.GetEnvironmentVariable("TEMP");
			if(sTemp.Length <= 0)
				sTemp = System.Environment.GetEnvironmentVariable("TMP");

			_prefDumpOutput = sTemp;

			// create a dump info instance used for dumping data
			_dmpInfo = 
				new DumpingInfo(DumpingFlags.DumpBinaryTOC | DumpingFlags.DumpTextTOC | 
				DumpingFlags.DumpTextIndex | DumpingFlags.DumpBinaryIndex | 
				DumpingFlags.DumpUrlStr | DumpingFlags.DumpStrings,
				sTemp, DumpCompression.Medium);

			LoadRegistryPreferences();

			HtmlHelpSystem.UrlPrefix = _prefURLPrefix;
			HtmlHelpSystem.UseHH2TreePics = _prefUseHH2TreePics;
		}

		public void TransferNMLToEditor()
		{
			this.Editor.Text = this.GraphControl.GetNML();
			this.Editor.SetHighlighting("XML");
			this.OpenCodeTab();
		}

		public void LoadCHM(string filePath)
		{
			
				ChmInit();

			try
			{
				//make sure the TOC is ready
				if(TocTree==null) OpenChmTocTab();

				// clear current items
				 TocTree.ClearContents();
				//					helpIndex1.ClearContents();
				//					helpSearch2.ClearContents();

				// open the chm-file selected in the OpenFileDialog
				_reader.OpenFile( filePath, _dmpInfo );			
				_reader.MergeFile("test.chm");
				// Enable the toc-tree pane if the opened file has a table of contents
				TocTree.Enabled = _reader.HasTableOfContents;
				// Enable the index pane if the opened file has an index
				//					helpIndex1.Enabled = _reader.HasIndex;
				// Enable the full-text search pane if the opened file supports full-text searching
				//					helpSearch2.Enabled = _reader.FullTextSearch;

				//					btnContents.Enabled = _reader.HasTableOfContents;
				//					btnIndex.Enabled = _reader.HasIndex;
				//					btnSearch.Enabled = _reader.FullTextSearch;
				//
				//					miContents.Enabled = _reader.HasTableOfContents;
				//					miContents1.Enabled = _reader.HasTableOfContents;
				//					miIndex.Enabled = _reader.HasIndex;
				//					miIndex1.Enabled = _reader.HasIndex;
				//					miSearch.Enabled = _reader.FullTextSearch;
				//					miSearch1.Enabled = _reader.FullTextSearch;
				//					btnSynch.Enabled = _reader.HasTableOfContents;
				//
				//					tabControl1.SelectedIndex = 0;
				//
				//					btnRefresh.Enabled = true;
				//					if( _reader.DefaultTopic.Length > 0)
				//					{
				//						btnHome.Enabled = true;
				//						miHome.Enabled = true;
				//					}

				// Build the table of contents tree view in the classlibrary control
				TocTree.BuildTOC( _reader.TableOfContents, _filter );

				// Build the index entries in the classlibrary control
				//					if( _reader.HasKLinks )
				//						helpIndex1.BuildIndex( _reader.Index, IndexType.KeywordLinks, _filter );
				//					else if( _reader.HasALinks )
				//						helpIndex1.BuildIndex( _reader.Index, IndexType.AssiciativeLinks, _filter );

				// Navigate the embedded browser to the default help topic
				//					NavigateBrowser( _reader.DefaultTopic );

				//					miMerge.Enabled = true;
				//					miCloseFile.Enabled = true;
				//
				//					this.Text = _reader.FileList[0].FileInfo.HelpWindowTitle + " - HtmlHelp - Viewer";
				//
				//					miCustomize.Enabled = ( _reader.HasInformationTypes || _reader.HasCategories);

				// Force garbage collection to free memory
				GC.Collect();
			}
			finally
			{
				
			}

				
			
		}
		#region Registry preferences
		/// <summary>
		/// Loads viewer preferences from registry
		/// </summary>
		private void LoadRegistryPreferences()
		{
			RegistryKey regKey = Registry.LocalMachine.CreateSubKey(LM_Key);
			
			bool bEnable = bool.Parse(regKey.GetValue("EnableDumping", true).ToString());

			_prefDumpOutput = (string) regKey.GetValue("DumpOutputDir", _prefDumpOutput);
			_prefDumpCompression = (DumpCompression) ((int)regKey.GetValue("CompressionLevel", _prefDumpCompression));
			_prefDumpFlags = (DumpingFlags) ((int)regKey.GetValue("DumpingFlags", _prefDumpFlags));

			if(bEnable)
				_dmpInfo = new DumpingInfo(_prefDumpFlags, _prefDumpOutput, _prefDumpCompression);
			else
				_dmpInfo = null;

			_prefURLPrefix = (string) regKey.GetValue("ITSUrlPrefix", _prefURLPrefix);
			_prefUseHH2TreePics = bool.Parse(regKey.GetValue("UseHH2TreePics", _prefUseHH2TreePics).ToString());
		}
		/// <summary>
		/// Saves viewer preferences to registry
		/// </summary>
		private void SaveRegistryPreferences()
		{
			RegistryKey regKey = Registry.LocalMachine.CreateSubKey(LM_Key);

			regKey.SetValue("EnableDumping", (_dmpInfo!=null));
			regKey.SetValue("DumpOutputDir", _prefDumpOutput);
			regKey.SetValue("CompressionLevel", (int)_prefDumpCompression);
			regKey.SetValue("DumpingFlags", (int)_prefDumpFlags);

			regKey.SetValue("ITSUrlPrefix", _prefURLPrefix);
			regKey.SetValue("UseHH2TreePics", _prefUseHH2TreePics);
		}

		#endregion

	}
}
