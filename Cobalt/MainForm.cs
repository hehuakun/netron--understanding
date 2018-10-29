using System;
using System.Drawing.Printing;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Netron.GraphLib;
using System.Diagnostics;
using System.IO;
using Netron.Neon;
using System.Xml;
using System.Reflection;
namespace Netron.Cobalt
{
	/// <summary>
	/// The MainForm collects a wide spectrum of features offered by the Netron graph control
	/// in one application.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		#region Constants
		public static readonly Color LightLightColor  = Color.LightGray;
		public static readonly string CaptionPrefix = "Cobalt [Graph Library 2.2]";
		#endregion

		#region Delegates and events
		/// <summary>
		/// Transfers a notification to the splashscreen about the module being loaded
		/// </summary>
		public delegate void LoadingInfo(string moduleName);
		/// <summary>
		/// Occurs when a module is being loaded (related to the splashscreen)
		/// </summary>
		public event LoadingInfo Loading;
		#endregion

		#region Fields


		/* though I'm a big fan of code documentation I haven't commented these fields
		 * for the rather obvious reason that it wouldn't be more enlighting than what you see
		 * in the form-designer of Visual Studio
		 */

		
		private bool m_bLayoutCalled = false;
		bool mLoaded = false;
		private DateTime m_dt;
		private DeserializeDockContent deserializeDockContent;
		internal System.Windows.Forms.StatusBar sb;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem mnuSaveDiagram;
		private System.Windows.Forms.MenuItem mnuOpenDiagram;
		private System.Windows.Forms.MenuItem mnuPrintPreview;
		private System.Windows.Forms.MenuItem mnuSaveImage;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem mnuStartLayout;
		private System.Windows.Forms.MenuItem mnuStopLayout;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem mnuZoom;
		private System.Windows.Forms.MenuItem mnuZoom100;
		private System.Windows.Forms.MenuItem mnuZoom50;
		private System.Windows.Forms.MenuItem mnuZoom200;
		private System.Windows.Forms.ImageList TabImages;
		private System.Windows.Forms.MenuItem mnuLayoutExample;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem mnuNoNewConnections;
		private System.Windows.Forms.MenuItem mnuNoNewShapes;
		private System.Windows.Forms.MenuItem mnuBackground;
		private System.Windows.Forms.MenuItem mnuSnap;
		private System.Windows.Forms.MenuItem mnuHelp;
		private System.Windows.Forms.MenuItem mnuAbout;
		private System.Windows.Forms.MenuItem mnuGenericHelp;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem mnuSave2GraphML;
		private System.Windows.Forms.MenuItem mnuNewDiagram;
		private System.Windows.Forms.MenuItem menuItem9;
		private System.Windows.Forms.MenuItem mnuFancyConnections;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.MenuItem mnuMultiPrint;
		private System.Windows.Forms.MenuItem mnuOpenGraphML;
		private System.Windows.Forms.MenuItem mnuGraphProperties;
		private System.Windows.Forms.MenuItem menuItem11;
		private System.Windows.Forms.MenuItem menuItem10;
		private System.Windows.Forms.MenuItem mnuSpringEmbedder;
		private System.Windows.Forms.MenuItem mnuTreeLayout;
		private System.Windows.Forms.MenuItem mnuRandomizerLayout;
		private System.Windows.Forms.ContextMenu outputMenu;
		private System.Windows.Forms.MenuItem mnuClearAll;
		private System.Windows.Forms.MenuItem menuItem12;
		private System.Windows.Forms.MenuItem mnuAnalysis;
		private System.Windows.Forms.MenuItem mnuSpanningTree;
		private System.Windows.Forms.MenuItem mnuLayers;
		private System.Windows.Forms.MenuItem menuItem13;
		private System.Windows.Forms.MenuItem mnuZorderExample;
		private System.Windows.Forms.MenuItem mnuLayeringExample;
		private System.Windows.Forms.ImageList ButtonImages;
		private System.Windows.Forms.MenuItem mnuTrees;
		private System.Windows.Forms.MenuItem mnuTreeAsCode;
		private System.Windows.Forms.MenuItem mnuRandomTree;
		private System.Windows.Forms.MenuItem mnuRandomNodeAddition;
		private System.Windows.Forms.MenuItem mnuNoLinkFrom;
		private System.Windows.Forms.MenuItem mnuItemCannotMove;
		private System.Windows.Forms.MenuItem mnuContextMenu;
		private System.Windows.Forms.MenuItem mnuShapeEvents;
		private Mediator mediator;
		private System.Windows.Forms.MenuItem mnuWindows;
		private System.Windows.Forms.MenuItem mnuWindowDiagram;
		private System.Windows.Forms.MenuItem mnuWindowZoom;
		private System.Windows.Forms.MenuItem mnuWindowShapes;
		private System.Windows.Forms.MenuItem mnuWindowFavs;
		private System.Windows.Forms.MenuItem mnuBrowser;
		private System.Windows.Forms.MenuItem menuItem14;
		private System.Windows.Forms.MenuItem mnuZoomIn;
		private System.Windows.Forms.MenuItem mnuZoomOut;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.MenuItem mnuWindowProperties;
		private System.Windows.Forms.MenuItem mnuOutput;
		private System.Windows.Forms.MenuItem mnuEditor;
		private System.Windows.Forms.MenuItem mnuEditorMenu;
		private System.Windows.Forms.MenuItem mnuHighlighting;
		private System.Windows.Forms.MenuItem mnuHighlightXML;
		private System.Windows.Forms.MenuItem mnuExamples;
		private System.Windows.Forms.MenuItem mnuDiagram;
		private System.Windows.Forms.MenuItem mnuApplication;
		private System.Windows.Forms.MenuItem mnuExit;
		private System.Windows.Forms.MenuItem mnuHighlightHTML;
		private System.Windows.Forms.MenuItem mnuHighlighCsharp;
		private System.Windows.Forms.MenuItem mnuHighlightVBNet;
		private System.Windows.Forms.MenuItem mnuHighlightNone;
		private System.Windows.Forms.MenuItem mnuOpenTextFile;
		private System.Windows.Forms.MenuItem mnuSaveTextFile;
		private System.Windows.Forms.MenuItem menuItem20;
		private System.Windows.Forms.MenuItem menuItem21;
		private System.Windows.Forms.MenuItem menuItem26;
		private System.Windows.Forms.MenuItem mnuNMLToEditor;
		internal System.Windows.Forms.MenuItem mnuShowNMLInDiagram;
		private System.Windows.Forms.MenuItem mnuEditorCut;
		private System.Windows.Forms.MenuItem mnuEditorCopy;
		private System.Windows.Forms.MenuItem mnuEditorPaste;
		private System.Windows.Forms.MenuItem mnuEditorDelete;
		private System.Windows.Forms.MenuItem mnuColorVisit;
		private System.Windows.Forms.MenuItem mnuTest;
		private System.Windows.Forms.MenuItem mnuDiagramBrowser;
		private System.Windows.Forms.MenuItem mnuCreateTemplate;
		private System.Windows.Forms.MenuItem menuItem15;
		private System.Windows.Forms.MenuItem mnuCreateSVG;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem mnuShowMode;
		private System.Windows.Forms.MenuItem menuItem17;
		private System.Windows.Forms.MenuItem mnuSave2HTML;
		private System.Windows.Forms.MenuItem mnuCreateImageShape;
		private System.Windows.Forms.MenuItem mnuEdit;
		private System.Windows.Forms.MenuItem mnuCut;
		private System.Windows.Forms.MenuItem mnuCopy;
		private System.Windows.Forms.MenuItem mnuPaste;
		private System.Windows.Forms.MenuItem mnuDelete;
		private System.Windows.Forms.MenuItem mnuUndo;
		private System.Windows.Forms.MenuItem mnuRedo;
		private System.Windows.Forms.MenuItem menuItem22;
		private System.Windows.Forms.MenuItem mnuCopyAsImage;
		private System.Windows.Forms.MenuItem menuItem18;
		private System.Windows.Forms.MenuItem mnuClassInheritance;
		private System.Windows.Forms.MenuItem mnuSelectAll;
		private System.Windows.Forms.MenuItem menuItem19;
		private System.Windows.Forms.MenuItem mnuChangeLog;
		private System.Windows.Forms.MenuItem mnuSampleDiagrams;
		private System.Windows.Forms.MenuItem mnuReadme;
		private Netron.Neon.DockPanel dockPanel;

		#endregion

		#region Constructor
		/// <summary>
		/// Default constructor
		/// </summary>
		public MainForm()
		{
			//this gets the state of the docking as it was the last run
			deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
			//part of the default Visual Studio setup
			InitializeComponent();
			//the root of the whole application connecting the different parts;
			mediator = new Mediator(this);
			mediator.DockPanel = dockPanel;

			#region Docking extender
			//Commenting out these lines will give you the traditional style.
			//If you use the Netron.Neon.Docking.Extenders.Blue.Extender you'll get another style.
			//The VS2005 mimics the Visual Studio tabs and colors
			dockPanel.Extender.AutoHideTabFactory = new  Netron.Neon.Docking.Extenders.VS2005.Extender.AutoHideTabFromBaseFactory();
			dockPanel.Extender.DockPaneTabFactory = new Netron.Neon.Docking.Extenders.VS2005.Extender.DockPaneTabFromBaseFactory();
			dockPanel.Extender.AutoHideStripFactory = new Netron.Neon.Docking.Extenders.VS2005.Extender.AutoHideStripFromBaseFactory();
			dockPanel.Extender.DockPaneCaptionFactory = new Netron.Neon.Docking.Extenders.VS2005.Extender.DockPaneCaptionFromBaseFactory();
			dockPanel.Extender.DockPaneStripFactory = new Netron.Neon.Docking.Extenders.VS2005.Extender.DockPaneStripFromBaseFactory();
				
			#endregion
			//if deserialization of the docking didn't work, this will set the default menu state
			SetContentMenu(TabTypes.Unknown, false);
			
		
		}
		#endregion

		#region Methods

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
			this.sb = new System.Windows.Forms.StatusBar();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.mnuApplication = new System.Windows.Forms.MenuItem();
			this.mnuShowMode = new System.Windows.Forms.MenuItem();
			this.menuItem17 = new System.Windows.Forms.MenuItem();
			this.mnuExit = new System.Windows.Forms.MenuItem();
			this.mnuEdit = new System.Windows.Forms.MenuItem();
			this.mnuUndo = new System.Windows.Forms.MenuItem();
			this.mnuRedo = new System.Windows.Forms.MenuItem();
			this.menuItem22 = new System.Windows.Forms.MenuItem();
			this.mnuCut = new System.Windows.Forms.MenuItem();
			this.mnuCopy = new System.Windows.Forms.MenuItem();
			this.mnuPaste = new System.Windows.Forms.MenuItem();
			this.mnuDelete = new System.Windows.Forms.MenuItem();
			this.menuItem18 = new System.Windows.Forms.MenuItem();
			this.mnuCopyAsImage = new System.Windows.Forms.MenuItem();
			this.mnuDiagram = new System.Windows.Forms.MenuItem();
			this.mnuNewDiagram = new System.Windows.Forms.MenuItem();
			this.menuItem9 = new System.Windows.Forms.MenuItem();
			this.mnuSaveDiagram = new System.Windows.Forms.MenuItem();
			this.mnuOpenDiagram = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.mnuSave2GraphML = new System.Windows.Forms.MenuItem();
			this.mnuOpenGraphML = new System.Windows.Forms.MenuItem();
			this.mnuNMLToEditor = new System.Windows.Forms.MenuItem();
			this.menuItem11 = new System.Windows.Forms.MenuItem();
			this.mnuMultiPrint = new System.Windows.Forms.MenuItem();
			this.mnuPrintPreview = new System.Windows.Forms.MenuItem();
			this.mnuContextMenu = new System.Windows.Forms.MenuItem();
			this.mnuGraphProperties = new System.Windows.Forms.MenuItem();
			this.mnuSaveImage = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.mnuStartLayout = new System.Windows.Forms.MenuItem();
			this.mnuStopLayout = new System.Windows.Forms.MenuItem();
			this.menuItem10 = new System.Windows.Forms.MenuItem();
			this.mnuSpringEmbedder = new System.Windows.Forms.MenuItem();
			this.mnuTreeLayout = new System.Windows.Forms.MenuItem();
			this.mnuRandomizerLayout = new System.Windows.Forms.MenuItem();
			this.menuItem12 = new System.Windows.Forms.MenuItem();
			this.mnuAnalysis = new System.Windows.Forms.MenuItem();
			this.mnuSpanningTree = new System.Windows.Forms.MenuItem();
			this.mnuColorVisit = new System.Windows.Forms.MenuItem();
			this.menuItem8 = new System.Windows.Forms.MenuItem();
			this.mnuLayers = new System.Windows.Forms.MenuItem();
			this.menuItem13 = new System.Windows.Forms.MenuItem();
			this.mnuCreateTemplate = new System.Windows.Forms.MenuItem();
			this.mnuCreateImageShape = new System.Windows.Forms.MenuItem();
			this.menuItem15 = new System.Windows.Forms.MenuItem();
			this.mnuCreateSVG = new System.Windows.Forms.MenuItem();
			this.mnuSave2HTML = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.mnuTest = new System.Windows.Forms.MenuItem();
			this.mnuExamples = new System.Windows.Forms.MenuItem();
			this.mnuLayoutExample = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.mnuNoNewConnections = new System.Windows.Forms.MenuItem();
			this.mnuNoNewShapes = new System.Windows.Forms.MenuItem();
			this.mnuNoLinkFrom = new System.Windows.Forms.MenuItem();
			this.mnuItemCannotMove = new System.Windows.Forms.MenuItem();
			this.mnuBackground = new System.Windows.Forms.MenuItem();
			this.mnuSnap = new System.Windows.Forms.MenuItem();
			this.mnuFancyConnections = new System.Windows.Forms.MenuItem();
			this.mnuZorderExample = new System.Windows.Forms.MenuItem();
			this.mnuLayeringExample = new System.Windows.Forms.MenuItem();
			this.mnuTrees = new System.Windows.Forms.MenuItem();
			this.mnuTreeAsCode = new System.Windows.Forms.MenuItem();
			this.mnuRandomTree = new System.Windows.Forms.MenuItem();
			this.mnuRandomNodeAddition = new System.Windows.Forms.MenuItem();
			this.mnuShapeEvents = new System.Windows.Forms.MenuItem();
			this.mnuZoom = new System.Windows.Forms.MenuItem();
			this.mnuZoom200 = new System.Windows.Forms.MenuItem();
			this.mnuZoom100 = new System.Windows.Forms.MenuItem();
			this.mnuZoom50 = new System.Windows.Forms.MenuItem();
			this.menuItem14 = new System.Windows.Forms.MenuItem();
			this.mnuZoomIn = new System.Windows.Forms.MenuItem();
			this.mnuZoomOut = new System.Windows.Forms.MenuItem();
			this.mnuEditorMenu = new System.Windows.Forms.MenuItem();
			this.mnuOpenTextFile = new System.Windows.Forms.MenuItem();
			this.mnuSaveTextFile = new System.Windows.Forms.MenuItem();
			this.menuItem20 = new System.Windows.Forms.MenuItem();
			this.mnuHighlighting = new System.Windows.Forms.MenuItem();
			this.mnuHighlightXML = new System.Windows.Forms.MenuItem();
			this.mnuHighlightHTML = new System.Windows.Forms.MenuItem();
			this.mnuHighlighCsharp = new System.Windows.Forms.MenuItem();
			this.mnuHighlightVBNet = new System.Windows.Forms.MenuItem();
			this.mnuHighlightNone = new System.Windows.Forms.MenuItem();
			this.menuItem21 = new System.Windows.Forms.MenuItem();
			this.mnuEditorCut = new System.Windows.Forms.MenuItem();
			this.mnuEditorCopy = new System.Windows.Forms.MenuItem();
			this.mnuEditorPaste = new System.Windows.Forms.MenuItem();
			this.mnuEditorDelete = new System.Windows.Forms.MenuItem();
			this.menuItem26 = new System.Windows.Forms.MenuItem();
			this.mnuShowNMLInDiagram = new System.Windows.Forms.MenuItem();
			this.mnuWindows = new System.Windows.Forms.MenuItem();
			this.mnuWindowDiagram = new System.Windows.Forms.MenuItem();
			this.mnuWindowZoom = new System.Windows.Forms.MenuItem();
			this.mnuWindowShapes = new System.Windows.Forms.MenuItem();
			this.mnuWindowFavs = new System.Windows.Forms.MenuItem();
			this.mnuBrowser = new System.Windows.Forms.MenuItem();
			this.mnuWindowProperties = new System.Windows.Forms.MenuItem();
			this.mnuOutput = new System.Windows.Forms.MenuItem();
			this.mnuEditor = new System.Windows.Forms.MenuItem();
			this.mnuDiagramBrowser = new System.Windows.Forms.MenuItem();
			this.mnuHelp = new System.Windows.Forms.MenuItem();
			this.mnuGenericHelp = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.mnuAbout = new System.Windows.Forms.MenuItem();
			this.outputMenu = new System.Windows.Forms.ContextMenu();
			this.mnuClearAll = new System.Windows.Forms.MenuItem();
			this.TabImages = new System.Windows.Forms.ImageList(this.components);
			this.ButtonImages = new System.Windows.Forms.ImageList(this.components);
			this.dockPanel = new Netron.Neon.DockPanel();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.mnuClassInheritance = new System.Windows.Forms.MenuItem();
			this.mnuSelectAll = new System.Windows.Forms.MenuItem();
			this.menuItem19 = new System.Windows.Forms.MenuItem();
			this.mnuChangeLog = new System.Windows.Forms.MenuItem();
			this.mnuSampleDiagrams = new System.Windows.Forms.MenuItem();
			this.mnuReadme = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// sb
			// 
			this.sb.Location = new System.Drawing.Point(0, 471);
			this.sb.Name = "sb";
			this.sb.Size = new System.Drawing.Size(928, 22);
			this.sb.TabIndex = 11;
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuApplication,
																					  this.mnuEdit,
																					  this.mnuDiagram,
																					  this.mnuExamples,
																					  this.mnuZoom,
																					  this.mnuEditorMenu,
																					  this.mnuWindows,
																					  this.mnuHelp});
			// 
			// mnuApplication
			// 
			this.mnuApplication.Index = 0;
			this.mnuApplication.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						   this.mnuShowMode,
																						   this.menuItem17,
																						   this.mnuExit});
			this.mnuApplication.Text = "Application";
			// 
			// mnuShowMode
			// 
			this.mnuShowMode.Index = 0;
			this.mnuShowMode.Shortcut = System.Windows.Forms.Shortcut.F11;
			this.mnuShowMode.Text = "Presentation mode";
			this.mnuShowMode.Click += new System.EventHandler(this.mnuShowMode_Click);
			// 
			// menuItem17
			// 
			this.menuItem17.Index = 1;
			this.menuItem17.Text = "-";
			// 
			// mnuExit
			// 
			this.mnuExit.Index = 2;
			this.mnuExit.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftX;
			this.mnuExit.Text = "E&xit";
			this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
			// 
			// mnuEdit
			// 
			this.mnuEdit.Index = 1;
			this.mnuEdit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuUndo,
																					this.mnuRedo,
																					this.menuItem22,
																					this.mnuCut,
																					this.mnuCopy,
																					this.mnuPaste,
																					this.mnuDelete,
																					this.menuItem18,
																					this.mnuCopyAsImage,
																					this.menuItem19,
																					this.mnuSelectAll});
			this.mnuEdit.Text = "Edit";
			// 
			// mnuUndo
			// 
			this.mnuUndo.Enabled = false;
			this.mnuUndo.Index = 0;
			this.mnuUndo.Shortcut = System.Windows.Forms.Shortcut.CtrlZ;
			this.mnuUndo.Text = "Undo";
			// 
			// mnuRedo
			// 
			this.mnuRedo.Enabled = false;
			this.mnuRedo.Index = 1;
			this.mnuRedo.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftZ;
			this.mnuRedo.Text = "Redo";
			// 
			// menuItem22
			// 
			this.menuItem22.Index = 2;
			this.menuItem22.Text = "-";
			// 
			// mnuCut
			// 
			this.mnuCut.Index = 3;
			this.mnuCut.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
			this.mnuCut.Text = "Cut";
			this.mnuCut.Click += new System.EventHandler(this.mnuCut_Click);
			// 
			// mnuCopy
			// 
			this.mnuCopy.Index = 4;
			this.mnuCopy.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
			this.mnuCopy.Text = "Copy";
			this.mnuCopy.Click += new System.EventHandler(this.mnuCopy_Click);
			// 
			// mnuPaste
			// 
			this.mnuPaste.Index = 5;
			this.mnuPaste.Shortcut = System.Windows.Forms.Shortcut.CtrlV;
			this.mnuPaste.Text = "Paste";
			this.mnuPaste.Click += new System.EventHandler(this.mnuPaste_Click);
			// 
			// mnuDelete
			// 
			this.mnuDelete.Index = 6;
			this.mnuDelete.Shortcut = System.Windows.Forms.Shortcut.Del;
			this.mnuDelete.Text = "Delete";
			this.mnuDelete.Click += new System.EventHandler(this.mnuDelete_Click);
			// 
			// menuItem18
			// 
			this.menuItem18.Index = 7;
			this.menuItem18.Text = "-";
			// 
			// mnuCopyAsImage
			// 
			this.mnuCopyAsImage.Index = 8;
			this.mnuCopyAsImage.Text = "Copy as image";
			this.mnuCopyAsImage.Click += new System.EventHandler(this.mnuCopyAsImage_Click);
			// 
			// mnuDiagram
			// 
			this.mnuDiagram.Index = 2;
			this.mnuDiagram.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.mnuNewDiagram,
																					   this.menuItem9,
																					   this.mnuSaveDiagram,
																					   this.mnuOpenDiagram,
																					   this.menuItem3,
																					   this.mnuSave2GraphML,
																					   this.mnuOpenGraphML,
																					   this.mnuNMLToEditor,
																					   this.menuItem11,
																					   this.mnuMultiPrint,
																					   this.mnuPrintPreview,
																					   this.mnuContextMenu,
																					   this.mnuGraphProperties,
																					   this.mnuSaveImage,
																					   this.menuItem4,
																					   this.menuItem1,
																					   this.mnuAnalysis,
																					   this.menuItem8,
																					   this.mnuLayers,
																					   this.menuItem13,
																					   this.mnuCreateTemplate,
																					   this.mnuCreateImageShape,
																					   this.menuItem15,
																					   this.mnuCreateSVG,
																					   this.mnuSave2HTML,
																					   this.menuItem2,
																					   this.mnuTest});
			this.mnuDiagram.Text = "Diagram";
			// 
			// mnuNewDiagram
			// 
			this.mnuNewDiagram.Index = 0;
			this.mnuNewDiagram.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
			this.mnuNewDiagram.Text = "New diagram";
			this.mnuNewDiagram.Click += new System.EventHandler(this.mnuNewDiagram_Click);
			// 
			// menuItem9
			// 
			this.menuItem9.Index = 1;
			this.menuItem9.Text = "-";
			// 
			// mnuSaveDiagram
			// 
			this.mnuSaveDiagram.Index = 2;
			this.mnuSaveDiagram.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
			this.mnuSaveDiagram.Text = "Save diagram";
			this.mnuSaveDiagram.Click += new System.EventHandler(this.mnuSaveDiagram_Click);
			// 
			// mnuOpenDiagram
			// 
			this.mnuOpenDiagram.Index = 3;
			this.mnuOpenDiagram.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
			this.mnuOpenDiagram.Text = "Open diagram";
			this.mnuOpenDiagram.Click += new System.EventHandler(this.mnuOpenDiagram_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 4;
			this.menuItem3.Text = "-";
			// 
			// mnuSave2GraphML
			// 
			this.mnuSave2GraphML.Index = 5;
			this.mnuSave2GraphML.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftS;
			this.mnuSave2GraphML.Text = "Save to NML";
			this.mnuSave2GraphML.Click += new System.EventHandler(this.mnuSave2NML_Click);
			// 
			// mnuOpenGraphML
			// 
			this.mnuOpenGraphML.Index = 6;
			this.mnuOpenGraphML.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftO;
			this.mnuOpenGraphML.Text = "Open NML";
			this.mnuOpenGraphML.Click += new System.EventHandler(this.mnuOpenGraphML_Click);
			// 
			// mnuNMLToEditor
			// 
			this.mnuNMLToEditor.Index = 7;
			this.mnuNMLToEditor.Shortcut = System.Windows.Forms.Shortcut.ShiftF5;
			this.mnuNMLToEditor.Text = "NML to editor";
			this.mnuNMLToEditor.Click += new System.EventHandler(this.mnuNMLToEditor_Click);
			// 
			// menuItem11
			// 
			this.menuItem11.Index = 8;
			this.menuItem11.Text = "-";
			// 
			// mnuMultiPrint
			// 
			this.mnuMultiPrint.Index = 9;
			this.mnuMultiPrint.Text = "Multi-page print";
			this.mnuMultiPrint.Click += new System.EventHandler(this.mnuMultiPrint_Click);
			// 
			// mnuPrintPreview
			// 
			this.mnuPrintPreview.Index = 10;
			this.mnuPrintPreview.Shortcut = System.Windows.Forms.Shortcut.CtrlP;
			this.mnuPrintPreview.Text = "Print preview";
			this.mnuPrintPreview.Click += new System.EventHandler(this.mnuPrintPreview_Click);
			// 
			// mnuContextMenu
			// 
			this.mnuContextMenu.Checked = true;
			this.mnuContextMenu.Index = 11;
			this.mnuContextMenu.Text = "Context menu";
			this.mnuContextMenu.Click += new System.EventHandler(this.ContextMenuSwitch_Click);
			// 
			// mnuGraphProperties
			// 
			this.mnuGraphProperties.Index = 12;
			this.mnuGraphProperties.Shortcut = System.Windows.Forms.Shortcut.CtrlT;
			this.mnuGraphProperties.Text = "Graph properties";
			this.mnuGraphProperties.Click += new System.EventHandler(this.mnuGraphProperties_Click);
			// 
			// mnuSaveImage
			// 
			this.mnuSaveImage.Index = 13;
			this.mnuSaveImage.Shortcut = System.Windows.Forms.Shortcut.CtrlI;
			this.mnuSaveImage.Text = "Save graph image";
			this.mnuSaveImage.Click += new System.EventHandler(this.mnuSaveImage_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 14;
			this.menuItem4.Text = "-";
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 15;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuStartLayout,
																					  this.mnuStopLayout,
																					  this.menuItem10,
																					  this.mnuSpringEmbedder,
																					  this.mnuTreeLayout,
																					  this.mnuRandomizerLayout,
																					  this.menuItem12});
			this.menuItem1.Text = "Layout";
			// 
			// mnuStartLayout
			// 
			this.mnuStartLayout.Index = 0;
			this.mnuStartLayout.Shortcut = System.Windows.Forms.Shortcut.CtrlL;
			this.mnuStartLayout.Text = "Start layout";
			this.mnuStartLayout.Click += new System.EventHandler(this.mnuStartLayout_Click);
			// 
			// mnuStopLayout
			// 
			this.mnuStopLayout.Index = 1;
			this.mnuStopLayout.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftL;
			this.mnuStopLayout.Text = "Stop layout";
			this.mnuStopLayout.Click += new System.EventHandler(this.mnuStopLayout_Click);
			// 
			// menuItem10
			// 
			this.menuItem10.Index = 2;
			this.menuItem10.Text = "-";
			// 
			// mnuSpringEmbedder
			// 
			this.mnuSpringEmbedder.Checked = true;
			this.mnuSpringEmbedder.Index = 3;
			this.mnuSpringEmbedder.Text = "Spring-embedder";
			this.mnuSpringEmbedder.Click += new System.EventHandler(this.mnuSpringEmbedder_Click);
			// 
			// mnuTreeLayout
			// 
			this.mnuTreeLayout.Index = 4;
			this.mnuTreeLayout.Text = "Tree layout";
			this.mnuTreeLayout.Click += new System.EventHandler(this.mnuTreeLayout_Click);
			// 
			// mnuRandomizerLayout
			// 
			this.mnuRandomizerLayout.Index = 5;
			this.mnuRandomizerLayout.Text = "Randomizer";
			this.mnuRandomizerLayout.Click += new System.EventHandler(this.mnuRandomizerLayout_Click);
			// 
			// menuItem12
			// 
			this.menuItem12.Index = 6;
			this.menuItem12.Text = "-";
			// 
			// mnuAnalysis
			// 
			this.mnuAnalysis.Index = 16;
			this.mnuAnalysis.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						this.mnuSpanningTree,
																						this.mnuColorVisit});
			this.mnuAnalysis.Text = "Analysis";
			// 
			// mnuSpanningTree
			// 
			this.mnuSpanningTree.Index = 0;
			this.mnuSpanningTree.Text = "Spanning tree";
			this.mnuSpanningTree.Click += new System.EventHandler(this.mnuSpanningTree_Click);
			// 
			// mnuColorVisit
			// 
			this.mnuColorVisit.Index = 1;
			this.mnuColorVisit.Text = "Colored visit";
			this.mnuColorVisit.Click += new System.EventHandler(this.mnuColoredVisit_Click);
			// 
			// menuItem8
			// 
			this.menuItem8.Index = 17;
			this.menuItem8.Text = "-";
			// 
			// mnuLayers
			// 
			this.mnuLayers.Index = 18;
			this.mnuLayers.Shortcut = System.Windows.Forms.Shortcut.CtrlQ;
			this.mnuLayers.Text = "Layers";
			this.mnuLayers.Click += new System.EventHandler(this.mnuLayers_Click);
			// 
			// menuItem13
			// 
			this.menuItem13.Index = 19;
			this.menuItem13.Text = "-";
			// 
			// mnuCreateTemplate
			// 
			this.mnuCreateTemplate.Index = 20;
			this.mnuCreateTemplate.Text = "Create template";
			this.mnuCreateTemplate.Click += new System.EventHandler(this.mnuCreateTemplate_Click);
			// 
			// mnuCreateImageShape
			// 
			this.mnuCreateImageShape.Index = 21;
			this.mnuCreateImageShape.Text = "Create image shape";
			this.mnuCreateImageShape.Click += new System.EventHandler(this.mnuCreateImageShape_Click);
			// 
			// menuItem15
			// 
			this.menuItem15.Index = 22;
			this.menuItem15.Text = "-";
			// 
			// mnuCreateSVG
			// 
			this.mnuCreateSVG.Index = 23;
			this.mnuCreateSVG.Text = "Save to SVG";
			this.mnuCreateSVG.Click += new System.EventHandler(this.mnuCreateSVG_Click);
			// 
			// mnuSave2HTML
			// 
			this.mnuSave2HTML.Index = 24;
			this.mnuSave2HTML.Text = "Save to HTML";
			this.mnuSave2HTML.Click += new System.EventHandler(this.mnuSave2HTML_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 25;
			this.menuItem2.Text = "-";
			// 
			// mnuTest
			// 
			this.mnuTest.Index = 26;
			this.mnuTest.Text = "test";
			this.mnuTest.Click += new System.EventHandler(this.mnuTest_Click_1);
			// 
			// mnuExamples
			// 
			this.mnuExamples.Index = 3;
			this.mnuExamples.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						this.mnuLayoutExample,
																						this.menuItem5,
																						this.mnuBackground,
																						this.mnuSnap,
																						this.mnuFancyConnections,
																						this.mnuZorderExample,
																						this.mnuLayeringExample,
																						this.mnuTrees,
																						this.mnuShapeEvents,
																						this.mnuClassInheritance});
			this.mnuExamples.Text = "API Examples";
			// 
			// mnuLayoutExample
			// 
			this.mnuLayoutExample.Index = 0;
			this.mnuLayoutExample.Text = "Layout a graph";
			this.mnuLayoutExample.Click += new System.EventHandler(this.mnuLayoutExample_Click);
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 1;
			this.menuItem5.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuNoNewConnections,
																					  this.mnuNoNewShapes,
																					  this.mnuNoLinkFrom,
																					  this.mnuItemCannotMove});
			this.menuItem5.Text = "Constraints examples";
			// 
			// mnuNoNewConnections
			// 
			this.mnuNoNewConnections.Index = 0;
			this.mnuNoNewConnections.Text = "No new connections";
			this.mnuNoNewConnections.Click += new System.EventHandler(this.mnuNoNewConnections_Click);
			// 
			// mnuNoNewShapes
			// 
			this.mnuNoNewShapes.Index = 1;
			this.mnuNoNewShapes.Text = "No new shapes";
			this.mnuNoNewShapes.Click += new System.EventHandler(this.mnuNoNewShapes_Click);
			// 
			// mnuNoLinkFrom
			// 
			this.mnuNoLinkFrom.Index = 2;
			this.mnuNoLinkFrom.Text = "No connections from \'Item1\'";
			this.mnuNoLinkFrom.Click += new System.EventHandler(this.mnuNoLinkFrom_Click);
			// 
			// mnuItemCannotMove
			// 
			this.mnuItemCannotMove.Index = 3;
			this.mnuItemCannotMove.Text = "\'Item 1\' cannot move";
			this.mnuItemCannotMove.Click += new System.EventHandler(this.mnuItemCannotMove_Click);
			// 
			// mnuBackground
			// 
			this.mnuBackground.Index = 2;
			this.mnuBackground.Text = "Background";
			this.mnuBackground.Click += new System.EventHandler(this.mnuBackground_Click);
			// 
			// mnuSnap
			// 
			this.mnuSnap.Index = 3;
			this.mnuSnap.Text = "Grid and snap";
			this.mnuSnap.Click += new System.EventHandler(this.mnuSnap_Click);
			// 
			// mnuFancyConnections
			// 
			this.mnuFancyConnections.Index = 4;
			this.mnuFancyConnections.Text = "Fancy connections";
			this.mnuFancyConnections.Click += new System.EventHandler(this.mnuFancyConnections_Click);
			// 
			// mnuZorderExample
			// 
			this.mnuZorderExample.Index = 5;
			this.mnuZorderExample.Text = "Z-ordering";
			this.mnuZorderExample.Click += new System.EventHandler(this.mnuZorderExample_Click);
			// 
			// mnuLayeringExample
			// 
			this.mnuLayeringExample.Index = 6;
			this.mnuLayeringExample.Text = "Layering";
			this.mnuLayeringExample.Click += new System.EventHandler(this.mnuLayeringExample_Click);
			// 
			// mnuTrees
			// 
			this.mnuTrees.Index = 7;
			this.mnuTrees.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.mnuTreeAsCode,
																					 this.mnuRandomTree,
																					 this.mnuRandomNodeAddition});
			this.mnuTrees.Text = "Trees";
			// 
			// mnuTreeAsCode
			// 
			this.mnuTreeAsCode.Index = 0;
			this.mnuTreeAsCode.Text = "As code";
			this.mnuTreeAsCode.Click += new System.EventHandler(this.mnuTreeAsCode_Click);
			// 
			// mnuRandomTree
			// 
			this.mnuRandomTree.Index = 1;
			this.mnuRandomTree.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftT;
			this.mnuRandomTree.Text = "Random";
			this.mnuRandomTree.Click += new System.EventHandler(this.mnuRandomTree_Click);
			// 
			// mnuRandomNodeAddition
			// 
			this.mnuRandomNodeAddition.Index = 2;
			this.mnuRandomNodeAddition.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftA;
			this.mnuRandomNodeAddition.Text = "Add random node";
			this.mnuRandomNodeAddition.Click += new System.EventHandler(this.mnuRandomNodeAddition_Click);
			// 
			// mnuShapeEvents
			// 
			this.mnuShapeEvents.Index = 8;
			this.mnuShapeEvents.Text = "Shape events";
			this.mnuShapeEvents.Click += new System.EventHandler(this.mnuShapeEvents_Click);
			// 
			// mnuZoom
			// 
			this.mnuZoom.Index = 4;
			this.mnuZoom.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuZoom200,
																					this.mnuZoom100,
																					this.mnuZoom50,
																					this.menuItem14,
																					this.mnuZoomIn,
																					this.mnuZoomOut});
			this.mnuZoom.Text = "Zoom";
			// 
			// mnuZoom200
			// 
			this.mnuZoom200.Index = 0;
			this.mnuZoom200.Text = "200%";
			this.mnuZoom200.Click += new System.EventHandler(this.mnuZoom200_Click);
			// 
			// mnuZoom100
			// 
			this.mnuZoom100.Index = 1;
			this.mnuZoom100.Shortcut = System.Windows.Forms.Shortcut.CtrlR;
			this.mnuZoom100.Text = "100%";
			this.mnuZoom100.Click += new System.EventHandler(this.mnuZoom100_Click);
			// 
			// mnuZoom50
			// 
			this.mnuZoom50.Index = 2;
			this.mnuZoom50.Text = "50%";
			this.mnuZoom50.Click += new System.EventHandler(this.mnuZoom50_Click);
			// 
			// menuItem14
			// 
			this.menuItem14.Index = 3;
			this.menuItem14.Text = "-";
			// 
			// mnuZoomIn
			// 
			this.mnuZoomIn.Index = 4;
			this.mnuZoomIn.Shortcut = System.Windows.Forms.Shortcut.CtrlIns;
			this.mnuZoomIn.Text = "Zoom in";
			this.mnuZoomIn.Click += new System.EventHandler(this.mnuZoomIn_Click);
			// 
			// mnuZoomOut
			// 
			this.mnuZoomOut.Index = 5;
			this.mnuZoomOut.Shortcut = System.Windows.Forms.Shortcut.CtrlDel;
			this.mnuZoomOut.Text = "Zoom out";
			this.mnuZoomOut.Click += new System.EventHandler(this.mnuZoomOut_Click);
			// 
			// mnuEditorMenu
			// 
			this.mnuEditorMenu.Index = 5;
			this.mnuEditorMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						  this.mnuOpenTextFile,
																						  this.mnuSaveTextFile,
																						  this.menuItem20,
																						  this.mnuHighlighting,
																						  this.menuItem21,
																						  this.mnuEditorCut,
																						  this.mnuEditorCopy,
																						  this.mnuEditorPaste,
																						  this.mnuEditorDelete,
																						  this.menuItem26,
																						  this.mnuShowNMLInDiagram});
			this.mnuEditorMenu.Text = "Editor";
			// 
			// mnuOpenTextFile
			// 
			this.mnuOpenTextFile.Index = 0;
			this.mnuOpenTextFile.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
			this.mnuOpenTextFile.Text = "Open file...";
			this.mnuOpenTextFile.Click += new System.EventHandler(this.mnuOpenTextFile_Click);
			// 
			// mnuSaveTextFile
			// 
			this.mnuSaveTextFile.Index = 1;
			this.mnuSaveTextFile.Text = "Save file...";
			this.mnuSaveTextFile.Click += new System.EventHandler(this.mnuSaveTextFile_Click);
			// 
			// menuItem20
			// 
			this.menuItem20.Index = 2;
			this.menuItem20.Text = "-";
			// 
			// mnuHighlighting
			// 
			this.mnuHighlighting.Index = 3;
			this.mnuHighlighting.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							this.mnuHighlightXML,
																							this.mnuHighlightHTML,
																							this.mnuHighlighCsharp,
																							this.mnuHighlightVBNet,
																							this.mnuHighlightNone});
			this.mnuHighlighting.Text = "Highlighting";
			// 
			// mnuHighlightXML
			// 
			this.mnuHighlightXML.Index = 0;
			this.mnuHighlightXML.Text = "XML";
			this.mnuHighlightXML.Click += new System.EventHandler(this.mnuHighlightXML_Click);
			// 
			// mnuHighlightHTML
			// 
			this.mnuHighlightHTML.Index = 1;
			this.mnuHighlightHTML.Text = "HTML";
			this.mnuHighlightHTML.Click += new System.EventHandler(this.mnuHighlightHTML_Click);
			// 
			// mnuHighlighCsharp
			// 
			this.mnuHighlighCsharp.Index = 2;
			this.mnuHighlighCsharp.Text = "C#";
			this.mnuHighlighCsharp.Click += new System.EventHandler(this.mnuHighlighCsharp_Click);
			// 
			// mnuHighlightVBNet
			// 
			this.mnuHighlightVBNet.Index = 3;
			this.mnuHighlightVBNet.Text = "VB.Net";
			this.mnuHighlightVBNet.Click += new System.EventHandler(this.mnuHighlightVBNet_Click);
			// 
			// mnuHighlightNone
			// 
			this.mnuHighlightNone.Index = 4;
			this.mnuHighlightNone.Text = "None";
			this.mnuHighlightNone.Click += new System.EventHandler(this.mnuHighlightNone_Click);
			// 
			// menuItem21
			// 
			this.menuItem21.Index = 4;
			this.menuItem21.Text = "-";
			// 
			// mnuEditorCut
			// 
			this.mnuEditorCut.Index = 5;
			this.mnuEditorCut.Text = "Cut";
			this.mnuEditorCut.Click += new System.EventHandler(this.mnuEditorCut_Click);
			// 
			// mnuEditorCopy
			// 
			this.mnuEditorCopy.Index = 6;
			this.mnuEditorCopy.Text = "Copy";
			this.mnuEditorCopy.Click += new System.EventHandler(this.mnuEditorCopy_Click);
			// 
			// mnuEditorPaste
			// 
			this.mnuEditorPaste.Index = 7;
			this.mnuEditorPaste.Text = "Paste";
			this.mnuEditorPaste.Click += new System.EventHandler(this.mnuEditorPaste_Click);
			// 
			// mnuEditorDelete
			// 
			this.mnuEditorDelete.Index = 8;
			this.mnuEditorDelete.Text = "Delete";
			this.mnuEditorDelete.Click += new System.EventHandler(this.mnuEditorDelete_Click);
			// 
			// menuItem26
			// 
			this.menuItem26.Index = 9;
			this.menuItem26.Text = "-";
			// 
			// mnuShowNMLInDiagram
			// 
			this.mnuShowNMLInDiagram.Index = 10;
			this.mnuShowNMLInDiagram.Shortcut = System.Windows.Forms.Shortcut.F5;
			this.mnuShowNMLInDiagram.Text = "Show NML in diagram";
			this.mnuShowNMLInDiagram.Click += new System.EventHandler(this.mnuShowNMLInDiagram_Click);
			// 
			// mnuWindows
			// 
			this.mnuWindows.Index = 6;
			this.mnuWindows.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.mnuWindowDiagram,
																					   this.mnuWindowZoom,
																					   this.mnuWindowShapes,
																					   this.mnuWindowFavs,
																					   this.mnuBrowser,
																					   this.mnuWindowProperties,
																					   this.mnuOutput,
																					   this.mnuEditor,
																					   this.mnuDiagramBrowser});
			this.mnuWindows.Text = "Windows";
			// 
			// mnuWindowDiagram
			// 
			this.mnuWindowDiagram.Index = 0;
			this.mnuWindowDiagram.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftD;
			this.mnuWindowDiagram.Text = "Diagram";
			this.mnuWindowDiagram.Click += new System.EventHandler(this.mnuWindowDiagram_Click);
			// 
			// mnuWindowZoom
			// 
			this.mnuWindowZoom.Index = 1;
			this.mnuWindowZoom.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftZ;
			this.mnuWindowZoom.Text = "Zoom";
			this.mnuWindowZoom.Click += new System.EventHandler(this.mnuWindowZoom_Click);
			// 
			// mnuWindowShapes
			// 
			this.mnuWindowShapes.Index = 2;
			this.mnuWindowShapes.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftF;
			this.mnuWindowShapes.Text = "Shapes";
			this.mnuWindowShapes.Click += new System.EventHandler(this.mnuWindowShapes_Click);
			// 
			// mnuWindowFavs
			// 
			this.mnuWindowFavs.Index = 3;
			this.mnuWindowFavs.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftF;
			this.mnuWindowFavs.Text = "Templates";
			this.mnuWindowFavs.Click += new System.EventHandler(this.mnuWindowFavs_Click);
			// 
			// mnuBrowser
			// 
			this.mnuBrowser.Index = 4;
			this.mnuBrowser.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftB;
			this.mnuBrowser.Text = "Browser";
			this.mnuBrowser.Click += new System.EventHandler(this.mnuBrowser_Click);
			// 
			// mnuWindowProperties
			// 
			this.mnuWindowProperties.Index = 5;
			this.mnuWindowProperties.Shortcut = System.Windows.Forms.Shortcut.F6;
			this.mnuWindowProperties.Text = "Properties";
			this.mnuWindowProperties.Click += new System.EventHandler(this.mnuWindowProperties_Click);
			// 
			// mnuOutput
			// 
			this.mnuOutput.Index = 6;
			this.mnuOutput.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftJ;
			this.mnuOutput.Text = "Output";
			this.mnuOutput.Click += new System.EventHandler(this.mnuOutput_Click);
			// 
			// mnuEditor
			// 
			this.mnuEditor.Index = 7;
			this.mnuEditor.Shortcut = System.Windows.Forms.Shortcut.F3;
			this.mnuEditor.Text = "Editor";
			this.mnuEditor.Click += new System.EventHandler(this.mnuEditor_Click);
			// 
			// mnuDiagramBrowser
			// 
			this.mnuDiagramBrowser.Index = 8;
			this.mnuDiagramBrowser.Text = "Diagram browser";
			this.mnuDiagramBrowser.Click += new System.EventHandler(this.mnuDiagramBrowser_Click);
			// 
			// mnuHelp
			// 
			this.mnuHelp.Index = 7;
			this.mnuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuGenericHelp,
																					this.mnuChangeLog,
																					this.mnuSampleDiagrams,
																					this.mnuReadme,
																					this.menuItem6,
																					this.mnuAbout});
			this.mnuHelp.Text = "Help";
			// 
			// mnuGenericHelp
			// 
			this.mnuGenericHelp.Index = 0;
			this.mnuGenericHelp.Text = "Help online";
			this.mnuGenericHelp.Click += new System.EventHandler(this.mnuGenericHelp_Click);
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 4;
			this.menuItem6.Text = "-";
			// 
			// mnuAbout
			// 
			this.mnuAbout.Index = 5;
			this.mnuAbout.Text = "About";
			this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
			// 
			// outputMenu
			// 
			this.outputMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.mnuClearAll});
			// 
			// mnuClearAll
			// 
			this.mnuClearAll.Index = 0;
			this.mnuClearAll.Text = "Clear all";
			// 
			// TabImages
			// 
			this.TabImages.ImageSize = new System.Drawing.Size(16, 16);
			this.TabImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("TabImages.ImageStream")));
			this.TabImages.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// ButtonImages
			// 
			this.ButtonImages.ImageSize = new System.Drawing.Size(16, 16);
			this.ButtonImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ButtonImages.ImageStream")));
			this.ButtonImages.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// dockPanel
			// 
			this.dockPanel.ActiveAutoHideContent = null;
			this.dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dockPanel.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.dockPanel.Location = new System.Drawing.Point(0, 0);
			this.dockPanel.Name = "dockPanel";
			this.dockPanel.Size = new System.Drawing.Size(928, 471);
			this.dockPanel.TabIndex = 16;
			this.dockPanel.ActiveDocumentChanged += new System.EventHandler(this.dockPanel_ActiveDocumentChanged);
			this.dockPanel.ActiveContentChanged += new System.EventHandler(this.dockPanel_ActiveContentChanged);
			// 
			// timer1
			// 
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// mnuClassInheritance
			// 
			this.mnuClassInheritance.Index = 9;
			this.mnuClassInheritance.Text = "Class inheritance";
			this.mnuClassInheritance.Click += new System.EventHandler(this.mnuClassInheritance_Click);
			// 
			// mnuSelectAll
			// 
			this.mnuSelectAll.Index = 10;
			this.mnuSelectAll.Text = "Select all";
			this.mnuSelectAll.Click += new System.EventHandler(this.mnuSelectAll_Click);
			// 
			// menuItem19
			// 
			this.menuItem19.Index = 9;
			this.menuItem19.Text = "-";
			// 
			// mnuChangeLog
			// 
			this.mnuChangeLog.Index = 1;
			this.mnuChangeLog.Text = "Change log";
			this.mnuChangeLog.Click += new System.EventHandler(this.mnuChangeLog_Click);
			// 
			// mnuSampleDiagrams
			// 
			this.mnuSampleDiagrams.Index = 2;
			this.mnuSampleDiagrams.Text = "Sample diagrams";
			this.mnuSampleDiagrams.Click += new System.EventHandler(this.mnuSampleDiagrams_Click);
			// 
			// mnuReadme
			// 
			this.mnuReadme.Index = 3;
			this.mnuReadme.Text = "Readme";
			this.mnuReadme.Click += new System.EventHandler(this.mnuReadme_Click);
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(928, 493);
			this.Controls.Add(this.dockPanel);
			this.Controls.Add(this.sb);
			this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu1;
			this.MinimumSize = new System.Drawing.Size(840, 520);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Layout += new System.Windows.Forms.LayoutEventHandler(this.MainForm_Layout);
			this.ResumeLayout(false);

		}
		#endregion

		#region Saving/serialization related
		/// <summary>
		/// Opens an existing nml file
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuOpenGraphML_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog fileChooser= new OpenFileDialog();
			fileChooser.Filter = "NML files (*.nml)|*.nml";
			DialogResult result = fileChooser.ShowDialog();
			string filename;			
			if(result==DialogResult.Cancel) return;
			filename=fileChooser.FileName;
			if (filename=="" || filename==null)
				MessageBox.Show("Invalid file name","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
			else
			{
				try
				{
					mediator.GraphControl.NewDiagram(true);
					mediator.GraphControl.OpenNML(filename);
				}
				catch(Exception exc)
				{
					mediator.Output(exc.Message);
				}
			}
		}

		/// <summary>
		/// Saves the diagram to NML
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuSave2NML_Click(object sender, System.EventArgs e)
		{
			SaveFileDialog fileChooser=new SaveFileDialog();
			fileChooser.CheckFileExists=false;
			
			fileChooser.Filter = "NML files (*.nml)|*.nml";
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
				mediator.GraphControl.SaveNMLAs(filename);
				MessageBox.Show("The graph was saved to '" + filename  + "'","NML saved",MessageBoxButtons.OK,MessageBoxIcon.Information);
				
			}
			

		}

		/// <summary>
		/// Saves the diagram to a binary format using .Net's BinarySerializer
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuSaveDiagram_Click(object sender, System.EventArgs e)
		{
			SaveDiagramBinary();
			
		}

		private void SaveDiagramBinary()
		{
			SaveFileDialog fileChooser=new SaveFileDialog();
			fileChooser.CheckFileExists=false;
			fileChooser.Filter = "Netron Graphs (*.netron)|*.netron";
			fileChooser.InitialDirectory = "\\c:";
			fileChooser.RestoreDirectory = true;
			fileChooser.Title = "Save diagram to binary file";
			DialogResult result =fileChooser.ShowDialog();
			string filename;
			
			if(result==DialogResult.Cancel) return;
			filename=fileChooser.FileName;
			if (filename=="" || filename==null)
				MessageBox.Show("Invalid file name","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
			else
			{
				
				if(mediator.GraphControl.SaveAs(filename))
					MessageBox.Show("The diagram was saved in '"  + filename + "'" ,"Save info",MessageBoxButtons.OK,MessageBoxIcon.Information);
				else
					MessageBox.Show("The diagram was not saved." ,"Save info",MessageBoxButtons.OK,MessageBoxIcon.Information);
				
			}
		}

		/// <summary>
		/// Opens a binary serialized diagram
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuOpenDiagram_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog fileChooser= new OpenFileDialog();
			fileChooser.Filter = "Netron diagram (*.netron)|*.netron";
			DialogResult result =fileChooser.ShowDialog();
			string filename;			
			if(result==DialogResult.Cancel) return;
			filename=fileChooser.FileName;
			if (filename=="" || filename==null)
				MessageBox.Show("Invalid file name","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
			else
			{
				mediator.GraphControl.NewDiagram(true);
				mediator.GraphControl.Open(filename);
			}
			
		}

		/// <summary>
		/// Saves a screenshot of the canvas to your c:\ directory
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuSaveImage_Click(object sender, System.EventArgs e)
		{
			/* // Use the following code if you want to change the directory;
			SaveFileDialog fileChooser=new SaveFileDialog();
			fileChooser.CheckFileExists=false;
			
			fileChooser.Filter = "JPG files (*.jpg)|*.jpg";
			fileChooser.InitialDirectory = "\\c:";
			fileChooser.RestoreDirectory = true;
			DialogResult result =fileChooser.ShowDialog();
			string filename;
			
			if(result==DialogResult.Cancel) return;
			filename=fileChooser.FileName;
			if (filename=="" || filename==null)
				MessageBox.Show("Invalid file name","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
			else
			*/
			{
				string filename = "c:\\NetronDiagramCanvas.jpg";
				mediator.GraphControl.SaveImage(filename, true);
				MessageBox.Show(@"The diagram image was saved in '" + filename + "'","Save info",MessageBoxButtons.OK,MessageBoxIcon.Information);
				
			}
			
		}

		
		#endregion

		#region Printing
		/// <summary>
		/// Shows the print-preview dialog
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuPrintPreview_Click(object sender, System.EventArgs e)
		{
			PrintDocument p = new PrintDocument();
			p.PrintPage += new PrintPageEventHandler(mediator.GraphControl.PrintCanvas);

			PrintPreviewDialog prev = new PrintPreviewDialog();
			prev.Document=p;
			prev.ShowDialog(this);
			return;
			/* this is the print directly
			PrintDialog d = new PrintDialog();
			d.Document = p ;
			if (d.ShowDialog() == DialogResult.OK)
			{
				p.Print();
			}
			*/
		}

		/// <summary>
		/// Allows multi-page printing if the diagram spans multiple pages.
		/// Thanks to Fabio for this example.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuMultiPrint_Click(object sender, System.EventArgs e)
		{

			PrinterSettings pset = null;// new PrinterSettings();
			

			NetronPrinter p = new NetronPrinter(pset,mediator.GraphControl);
			//p.PrintPage += new PrintPageEventHandler(mediator.GraphControl.PrintCanvas);

			PrintPreviewDialog prev = new PrintPreviewDialog();
			prev.UseAntiAlias = true;
			prev.Document=p;
			prev.ShowDialog(this);
			return;
		}

		#endregion
		
		#region Zoom
		/// <summary>
		/// Magnifies the diagram
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuZoom200_Click(object sender, System.EventArgs e)
		{
			mediator.GraphControl.Zoom = 2F;
		}

		/// <summary>
		/// Magnifies the diagram
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuZoom100_Click(object sender, System.EventArgs e)
		{
			mediator.GraphControl.Zoom = 1F;
		}

		/// <summary>
		/// Shrinks the diagram
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuZoom50_Click(object sender, System.EventArgs e)
		{
			mediator.GraphControl.Zoom = 0.5F;
		}

	
		#endregion
	
		#region Layout
		/// <summary>
		/// Starts the layout
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuStartLayout_Click(object sender, System.EventArgs e)
		{
			if(mediator.GraphControl.Shapes.Count<1)
			{
				MessageBox.Show("Add first some shapes to the canvas before running the layout algorithm. \n You can use either the context menu or drag-drop from the shapes library.", "No shapes on the canvas",MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}

			if(mediator.GraphControl.Connections.Count<1)
			{
				MessageBox.Show(" Shapes are layed out if they are connected together. \n Drag connections between shapes by first clicking on a connector and dragging the line to another connector..", "No connections on the canvas",MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			mediator.GraphControl.StartLayout();
		}

		/// <summary>
		/// Stops the layout process
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuStopLayout_Click(object sender, System.EventArgs e)
		{
			if(mediator.GraphControl.Shapes.Count<1)
				MessageBox.Show("Add first some shapes to the canvas before running the layout algorithm. \n You can use either the context menu or drag-drop from the shapes library.", "No shapes on the canvas",MessageBoxButtons.OK, MessageBoxIcon.Hand);
			mediator.GraphControl.StopLayout();
		}

		/// <summary>
		/// Changes the algorithm to the spring-embedder
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuSpringEmbedder_Click(object sender, System.EventArgs e)
		{
			mnuTreeLayout.Checked = false;
			mnuSpringEmbedder.Checked = true;
			mnuRandomizerLayout.Checked = false;
			mediator.GraphControl.GraphLayoutAlgorithm = GraphLayoutAlgorithms.SpringEmbedder;
		}

		/// <summary>
		/// Changes the algorithm to the 'tree' type
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuTreeLayout_Click(object sender, System.EventArgs e)
		{
			mnuTreeLayout.Checked = true;
			mnuSpringEmbedder.Checked =false;
			mnuRandomizerLayout.Checked = false;
			mediator.GraphControl.GraphLayoutAlgorithm = GraphLayoutAlgorithms.Tree;
		}

		/// <summary>
		/// Changes the layout algorithm to the 'random' type
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuRandomizerLayout_Click(object sender, System.EventArgs e)
		{
			mnuTreeLayout.Checked = false;
			mnuSpringEmbedder.Checked =false;
			mnuRandomizerLayout.Checked = true;
			mediator.GraphControl.GraphLayoutAlgorithm = GraphLayoutAlgorithms.Randomizer;
		}

		
		#endregion
		
		#region Help links

		private void mnuAbout_Click(object sender, System.EventArgs e)
		{
			SplashForm frm = new SplashForm(false);
			frm.ShowDialog(this);

		}

		private void mnuCredits_Click(object sender, System.EventArgs e)
		{
			System.Diagnostics.Process.Start("http://netron.sourceforge.net/ewiki/netron.php?id=CreditsAndAknowledgements"); 

		}

		private void mnuGenericHelp_Click(object sender, System.EventArgs e)
		{			
			System.Diagnostics.Process.Start("http://netron.sourceforge.net/wp/"); 
		}

		#endregion

		#region Favorites
		private void Favorites_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			
			if ((e.AllowedEffect & DragDropEffects.Move) == DragDropEffects.Move )
			{
				// Show the standard Move icon.
				e.Effect = DragDropEffects.Move;
			}
			else
			{
				// Show the standard Copy icon.
				e.Effect = DragDropEffects.Copy;
			}
		}

		

		#endregion

		#region Outputter
		

		

		#endregion

		#region Layer interface display
		private void mnuLayers_Click(object sender, System.EventArgs e)
		{
			using(LayersDialog frm = new LayersDialog(mediator.GraphControl))
			{
				//GraphLayerCollection layers = mediator.GraphControl.Layers;
				frm.Manager.LoadLayers(mediator.GraphControl);
				DialogResult res = frm.ShowDialog(this);
				if(res==DialogResult.OK)
				{
					frm.Manager.UpdateLayerData();					

				}
				else
					return;
			}
		}
		#endregion

		#region Examples

		#region Class structures
		private void mnuBasicClases_Click(object sender, System.EventArgs e)
		{
			mediator.LoadSample(Samples.GraphLibClasses);
		}
		#endregion

		#region Interfaces
		private void mnuUMLInterface_Click(object sender, System.EventArgs e)
		{
			mediator.LoadSample(Samples.GraphLibInterfaces);
		}


		#endregion

		#region Custom bagrounds
		/// <summary>
		/// Sets a gradient background
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuBackground_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show("This example positions shapes at random positions and random z-depth and starts the spring-embedder algorithm. You can stop the layout by pressing CTRL-SHIFT-L and restart it again with CTRL-L. The background is a bitmap and only shows the possibility to use flat colors, gradients or images as a background.","API example", MessageBoxButtons.OK, MessageBoxIcon.Information);
			mediator.LoadSample(Samples.Background);
			
		}

		#endregion

		#region Grid & snap
		/// <summary>
		/// Shows how to set the snapping on
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuSnap_Click(object sender, System.EventArgs e)
		{
			
			MessageBox.Show("This example shows how you can constraint shape positions with the grid. The grid, grid-size and snap can be set in the canvas properties (right-click the canvas and select 'properties'). You can also change the location of shapes via the CTRL-arrow keys.","API example", MessageBoxButtons.OK, MessageBoxIcon.Information);
			mediator.LoadSample(Samples.Snap);

		}

		#endregion

		#region Utilities to generate the examples

		/// <summary>
		/// Sets the algorithm and changes the interface accordingly
		/// </summary>
		/// <param name="algorithm"></param>
		public void SetLayoutAlgorithmUIState(GraphLayoutAlgorithms algorithm)
		{
			
			switch(algorithm)
			{
				case GraphLayoutAlgorithms.Randomizer:
					this.mnuRandomizerLayout.Checked = true;
					this.mnuSpringEmbedder.Checked = false;
					this.mnuTreeLayout.Checked = false;
					break;
				case GraphLayoutAlgorithms.SpringEmbedder:
					this.mnuRandomizerLayout.Checked = false;
					this.mnuSpringEmbedder.Checked = true;
					this.mnuTreeLayout.Checked = false;
					break;
				case GraphLayoutAlgorithms.Tree:
					this.mnuRandomizerLayout.Checked = false;
					this.mnuSpringEmbedder.Checked = false;
					this.mnuTreeLayout.Checked = true;
					break;

			}
		}



		#endregion

		#region Specific item cannot move
		private void mnuItemCannotMove_Click(object sender, System.EventArgs e)
		{
		mediator.LoadSample(Samples.ItemCannotMove);		
		}

		#endregion

		#region No new connection
		private void mnuNoNewConnections_Click(object sender, System.EventArgs e)
		{
		mediator.LoadSample(Samples.NoNewConnections);
		}
		#endregion

		private void mnuAddSomeShapes_Click(object sender, System.EventArgs e)
		{
			//mediator.LoadSample(Samples.
		}

		#region Class inheritance

		private void mnuClassInheritance_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show("See the code of this example to see how you can access custom shapes (i.e. shapes not compiled in the graph library) via code.","API example", MessageBoxButtons.OK, MessageBoxIcon.Information);
			mediator.LoadSample(Samples.ClassInheritance);	
		}		
		#endregion
		

		#region Fancy connections

		private void mnuFancyConnections_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show("This example shows a sample of custom connections defined in the Entitology library. Note that the location of the shape is random and can sometimes be an unhappy one, simply restart this example again in that case.","API example", MessageBoxButtons.OK, MessageBoxIcon.Information);
			mediator.LoadSample(Samples.FancyConnections);
		}

		#endregion

		#region Z-order

		private void mnuZorderExample_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show("This example shows how the z-depth can help you achieve a 3D look in diagrams. The alpha color of the shapes and connections is in function of the z-order. You can stop/Start the layout at any time via CTRL-SHIFT-L and CTRL-L respectively.","API example", MessageBoxButtons.OK, MessageBoxIcon.Information);
			mediator.LoadSample(Samples.ZOrder);
		}

	

		#endregion

		#region Layering
		private void mnuLayeringExample_Click(object sender, System.EventArgs e)
		{
			mediator.LoadSample(Samples.Layering);
		}
		#endregion

		#region Spring-embedder layout 
		private void mnuLayoutExample_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show("This example positions shapes at random positions and starts the spring-embedder algorithm. You can stop the layout by pressing CTRL-SHIFT-L and restart it again with CTRL-L.","API example", MessageBoxButtons.OK, MessageBoxIcon.Information);
			mediator.LoadSample(Samples.Layout);
		}
		#endregion

		#region With controls
		private void mnuWithControls_Click(object sender, System.EventArgs e)
		{
			mediator.LoadSample(Samples.Controls);	
		}

		#endregion

		#region No linking from specific item
		private void mnuNoLinkFrom_Click(object sender, System.EventArgs e)
		{
			mediator.LoadSample(Samples.NoLinking);
		}
		#endregion

		#region Shape events
		/// <summary>
		/// Shows how you can attach mouse-event handler to specific shapes
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuShapeEvents_Click(object sender, System.EventArgs e)
		{
		mediator.LoadSample(Samples.ShapeEvents);


		
		}
		

		#endregion

		#region Trees
		/// <summary>
		/// Creates a random tree graph and applies the tree layout thereafter
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuRandomTree_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show("Restart this example to get a different tree. Use CTRL-SHIFT-A in this example to add a random node to the tree.","API example", MessageBoxButtons.OK, MessageBoxIcon.Information);
			mediator.LoadSample(Samples.RandomTree);		
		}

		/// <summary>
		/// Add a single random node to the graph
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuRandomNodeAddition_Click(object sender, System.EventArgs e)
		{
			mediator.GraphControl.Focus();
			mediator.AddRandomNodes(1);
			//SetLayoutAlgorithmUIState(GraphLayoutAlgorithms.Tree);

			mediator.GraphControl.StartLayout();

		
		}
		


		private void mnuTreeAsCode_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show("Use CTRL-SHIFT-A in this example to add a random node to the tree.","API example", MessageBoxButtons.OK, MessageBoxIcon.Information);
			mediator.LoadSample(Samples.TreeAsCode);
		}
		#endregion

		#region No new shapes
		/// <summary>
		/// Disallows the addition of new shapes on the control level
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuNoNewShapes_Click(object sender, System.EventArgs e)
		{
			mediator.LoadSample(Samples.NoNewShapes);		

		}

		#endregion
		
		#endregion

		#region Diverse elements

		/// <summary>
		/// Clears the canvas
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuNewDiagram_Click(object sender, System.EventArgs e)
		{
			CreateNewDiagram();
			
		}
		/// <summary>
		/// Creates a new diagram
		/// </summary>
		internal void CreateNewDiagram()
		{
			//AskForSaving();
			mediator.OpenGraphTab();
			mediator.GraphControl.NewDiagram(true);
		}
		/// <summary>
		/// Asks the user if he/she wants to save the current diagram.
		/// </summary>
		internal void AskForSaving()
		{
			if(mediator.GraphControl.Shapes.Count>0)
			{
				DialogResult res = MessageBox.Show("Do you want to save the diagram before the canvas is cleared?", "New diagram", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if(res==DialogResult.Yes)
				{
					SaveDiagramBinary();
				}
			}
		}

		/// <summary>
		/// Contextmenu switch of the graphcontrol
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ContextMenuSwitch_Click(object sender, System.EventArgs e)
		{
			mnuContextMenu.Checked = !mnuContextMenu.Checked;
			mediator.GraphControl.EnableContextMenu=!mediator.GraphControl.EnableContextMenu;
		}
	
		/// <summary>
		/// Quits the application
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuExit_Click(object sender, System.EventArgs e)
		{
			DialogResult res = MessageBox.Show("Are you sure you want to exit?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if(res==DialogResult.Yes) Application.Exit();
		}

		/// <summary>
		/// Opens the graph-info dialog
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuGraphProperties_Click(object sender, System.EventArgs e)
		{
			GraphPropertiesDialog props = new GraphPropertiesDialog(mediator.GraphControl.Abstract.GraphInformation);
			DialogResult res= props.ShowDialog();

			if(res==DialogResult.OK)
			{
				mediator.GraphControl.Abstract.GraphInformation = props.GraphInformation;
			}
			

		}

		/// <summary>
		/// Displays a spanning tree via Prim's algorithm.
		/// This show how to use the graph-analysis sub-library which was formerly delivered in the 
		/// NetronDataStructures library.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuSpanningTree_Click(object sender, System.EventArgs e)
		{
			GraphLib.Analysis.GraphAnalyzer analyzer = new Netron.GraphLib.Analysis.GraphAnalyzer(mediator.GraphControl.Abstract, true);
			mediator.Output( analyzer.MatrixForm());			
			mediator.Output("A spanning tree starting from the 0-th node:");
			//GraphLib.Analysis.IGraph g = GraphLib.Analysis.Algorithms.KruskalsAlgorithm(analyzer);
			GraphLib.Analysis.IGraph g = GraphLib.Analysis.Algorithms.PrimsAlgorithm(analyzer,0);
			int nodeCount = analyzer.Count;
			IEnumerator numer = g.Edges.GetEnumerator();
			
			while(numer.MoveNext())
			{
				try
				{
					GraphLib.Analysis.IEdge edge = numer.Current as GraphLib.Analysis.IEdge;//g.GetEdge(v,w);
					analyzer.GetShape(edge.V0.Number).ShapeColor = Color.Orange;
					analyzer.GetShape(edge.V1.Number).ShapeColor = Color.Orange;
					analyzer.GetConnection(edge.V0.Number,edge.V1.Number).LineColor = Color.Orange;
				}
				catch
				{continue;}
			}
			#region alternatively
			//			for(int v=0; v<nodeCount; v++)
			//			{
			//				
			//				for(int w=0; w<nodeCount; w++)
			//				{
			//					try
			//					{
			//						GraphLib.Analysis.IEdge edge = g.GetEdge(v,w);
			//						analyzer.GetShape(v).ShapeColor = Color.Orange;
			//						analyzer.GetShape(w).ShapeColor = Color.Orange;
			//						analyzer.GetConnection(v,w).LineColor = Color.Orange;
			//					}
			//					catch(Exception)
			//					{							
			//						continue;
			//					}
			//				}				
			//			}
			#endregion
		}


		/// <summary>
		/// Shows a simple splash-screen
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);	
			Trace.WriteLine(Environment.NewLine);
			Trace.WriteLine(Environment.NewLine);
			Trace.WriteLine("Cobalt v" + Assembly.GetExecutingAssembly().GetName().Version.ToString());
			Trace.WriteLine(DateTime.Now.ToLongTimeString() + ", loading the application.");
			Trace.WriteLine(Environment.NewLine);

			SetCaption("");
#if DEBUG
			try
			{
				string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
				if (File.Exists(configFile))
					dockPanel.LoadFromXml(configFile, deserializeDockContent);				
			}
			catch
			{
				Trace.WriteLine("The deserialization of the docking didn't work, probably a different version.");
			}
#endif

		}


		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			//Stop any layout-thread(s)
			mediator.GraphControl.StopLayout();
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}


		#endregion

		/// <summary>
		/// Serializes the docking state before exiting the application.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing (e);
			string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
			dockPanel.SaveAsXml(configFile);
		}


		public void SetCaption(string text)
		{
			if(text.Length==0)
				this.Text = CaptionPrefix ;
			else
				this.Text = CaptionPrefix + ": " + text;
		}
	

		/// <summary>
		/// Shows the diagram tab
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuWindowDiagram_Click(object sender, System.EventArgs e)
		{
			mediator.OpenGraphTab();
		}


		/// <summary>
		/// Shows the properties tab
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuWindowProperties_Click(object sender, System.EventArgs e)
		{
			mediator.OpenPropsTab();
		}

		/// <summary>
		/// Shows the output tab
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuOutput_Click(object sender, System.EventArgs e)
		{
			mediator.OpenOuputTab();
		}

		/// <summary>
		/// Shows the zoom tab
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuWindowZoom_Click(object sender, System.EventArgs e)
		{
			mediator.OpenZoomTab();
		}

		/// <summary>
		/// Shows the shape-viewer tab
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuWindowShapes_Click(object sender, System.EventArgs e)
		{
			mediator.OpenShapesTab();
		}

		/// <summary>
		/// Shows the shape favorites tab
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuWindowFavs_Click(object sender, System.EventArgs e)
		{
			mediator.OpenFavsTab();
		}

		
		/// <summary>
		/// Shows the browser tab
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuBrowser_Click(object sender, System.EventArgs e)
		{
			mediator.OpenBrowserTab();
		}

		/// <summary>
		/// Shows the help content tab
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuHelpContent_Click(object sender, System.EventArgs e)
		{
			//you can show the help inside the application;
			//mediator.OpenChmTocTab();
			Process.Start(Path.GetDirectoryName(Application.ExecutablePath) + "\\NetronGraphLibrary.chm" );
		}

		/// <summary>
		/// Part of the deserialization of the docking environment
		/// </summary>
		/// <param name="persistString"></param>
		/// <returns></returns>
		private DockContent GetContentFromPersistString(string persistString)
		{
			if(!mLoaded)
				RaiseLoading("Loading tab '" + persistString + "'...");
			if (persistString == typeof(BrowserTab).ToString())
				return mediator.BrowserTab;
			else if (persistString == typeof(ChmTocTab).ToString())
				return mediator.ChmTocTab;
			else if (persistString == typeof(FavTab).ToString())
				return mediator.FavTab;
			else if (persistString == typeof(OutputExTab).ToString())
				return mediator.OutputTab;
			else if (persistString == typeof(PropertiesTab).ToString())
				return mediator.PropertiesTab;
			else if (persistString == typeof(ShapeViewerTab).ToString())
				return mediator.ShapeViewerTab;
			else if (persistString == typeof(ZoomTab).ToString())
				return mediator.ZoomTab;
			else if (persistString == typeof(GraphTab).ToString())
				return mediator.GraphTab;
//			else if (persistString == typeof(BugTab).ToString())
//				return mediator.BugTab;
			else if (persistString == typeof(CodeTab).ToString())
				return mediator.CodeTab;
			else if (persistString == typeof(DiagramBrowserTab).ToString())
				return mediator.DiagramBrowserTab;
			else
			{
			
				return null;
			}
		}

		/// <summary>
		/// Show the bug reporter tab
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
//		private void mnuWindowBugReporter_Click(object sender, System.EventArgs e)
//		{
//			mediator.OpenBugTab();
//			mediator.BugTab.EnableEditing = true;
//		}

		/// <summary>
		/// Zoom in the diagram
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuZoomIn_Click(object sender, System.EventArgs e)
		{
			mediator.GraphControl.Zoom+=0.2f;
		}

		/// <summary>
		/// Zoom out the diagram
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuZoomOut_Click(object sender, System.EventArgs e)
		{
			mediator.GraphControl.Zoom-=0.2f;
		}

		

		private void RTFBox_LinkClicked(object sender, System.Windows.Forms.LinkClickedEventArgs e)
		{
			Process.Start(e.LinkText);
		}

		private void MainForm_Layout(object sender, System.Windows.Forms.LayoutEventArgs e)
		{
			if( m_bLayoutCalled == false )
			{
				m_bLayoutCalled = true;
				m_dt = DateTime.Now;
				if( SplashScreen.SplashForm != null )
					SplashScreen.SplashForm.Owner = this;
				this.Activate();
				System.Threading.Thread.Sleep(5000);
				SplashScreen.CloseForm();
				//timer1.Start();
			}
		}

		private void timer1_Tick(object sender, System.EventArgs e)
		{
			TimeSpan ts = DateTime.Now.Subtract(m_dt);
			if( ts.TotalSeconds > 12 )
				this.Close();
		}
		/// <summary>
		/// Raise the loading-event
		/// </summary>
		/// <param name="moduleName"></param>
		internal void RaiseLoading(string moduleName)
		{
			if(Loading!=null)
				Loading(moduleName);
		}
		/// <summary>
		/// Used together with the multithreaded splashscreen to give feedback of the 
		/// loaded modules.
		/// </summary>
		public void PreLoad()
		{
			if (mLoaded)
			{
				//	just return. this code can't execute twice!
				return;
			}
			RaiseLoading("Docking deserialization...");
			string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");

			if (File.Exists(configFile))
				dockPanel.LoadFromXml(configFile, deserializeDockContent);
	
			//new TabPages.TestTab().Show(dockPanel);
			
			//			mediator.OpenPropsTab();
			//			mediator.OpenGraphTab();
			//RaiseLoading("Loading preliminary documentation...");
			//mediator.LoadCHM("NetronGraphLib2.2beta.chm");
			/*
			SplashScreen.SetStatus("Loading module 1");
			System.Threading.Thread.Sleep(500);
			SplashScreen.SetStatus("Loading module 2");
			System.Threading.Thread.Sleep(300);
			SplashScreen.SetStatus("Loading module 3");
			System.Threading.Thread.Sleep(900);
			SplashScreen.SetStatus("Loading module 4");
			System.Threading.Thread.Sleep(100);
			
*/
			//	flag that we have loaded all we need.
			mLoaded = true;
		}

		#endregion

		/// <summary>
		/// Opens the text editor
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuEditor_Click(object sender, System.EventArgs e)
		{
			mediator.OpenCodeTab();			
		}

		/// <summary>
		/// Changes the main menu when the active content is changed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dockPanel_ActiveContentChanged(object sender, System.EventArgs e)
		{
			ContentFocusChanged();
		}

		/// <summary>
		///  Changes the main menu when the active document is changed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dockPanel_ActiveDocumentChanged(object sender, System.EventArgs e)
		{
			ContentFocusChanged();
		}

		/// <summary>
		/// Changes some ambient things like the menu in function of the current content/tab
		/// </summary>
		private void ContentFocusChanged()
		{
			if(mnuShowMode.Checked) return;

			DockContent dockContent = this.dockPanel.ActiveContent;
			if(dockContent==null) return;
			
			if(typeof(CodeTab).IsInstanceOfType(dockContent))
				SetContentMenu(TabTypes.Code, true);
			else
				SetContentMenu(TabTypes.Code, false);

			if(typeof(GraphTab).IsInstanceOfType(dockContent)  || typeof(ZoomTab).IsInstanceOfType(dockContent) || typeof(ShapeViewerTab).IsInstanceOfType(dockContent) || typeof(FavTab).IsInstanceOfType(dockContent))
				SetContentMenu(TabTypes.NetronDiagram, true);
			else
				SetContentMenu(TabTypes.NetronDiagram, false);

		}

		/// <summary>
		/// Changes the main menu in function of the given tab content
		/// </summary>
		/// <param name="tabType"></param>
		/// <param name="value"></param>
		private void SetContentMenu(TabTypes tabType, bool value)
		{
			switch(tabType)
			{
				case TabTypes.NetronDiagram:
					mnuExamples.Visible = value;
					mnuZoom.Visible = value;
					mnuDiagram.Visible = value;
					if(value) mediator.TestTextForNML();
					mnuEdit.Visible = value;
					break;
				case TabTypes.Code:
					mnuEditorMenu.Visible = value;
					break;
				case TabTypes.Unknown:
					mnuExamples.Visible = value;
					mnuZoom.Visible = value;
					mnuDiagram.Visible = value;
					mnuEditorMenu.Visible = value;
					break;
			}
		}

		
	
		/// <summary>
		/// Opens a text file in the editor
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuOpenTextFile_Click(object sender, System.EventArgs e)
		{
			OpenTextFile();
		}



		private void OpenTextFile()
		{
			mediator.OpenTextFile();
		}

		/// <summary>
		/// Saves the content of the editor to a file
		/// </summary>
		private void SaveTextFile()
		{
			mediator.SaveTextFile();
		}
		/// <summary>
		/// Transfer the presumed NML content to the diagram
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuShowNMLInDiagram_Click(object sender, System.EventArgs e)
		{
			mediator.GraphControl.NewDiagram(true);
			mediator.GraphControl.OpenNMLFragment(mediator.Editor.Text);
		}

		/// <summary>
		/// The presumed XML in the text-editor will be re-formatted and indented
		/// to something more readable. 
		/// However, for some reason this does not work...
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FormatXML()
		{
			
			try
			{
				
				

				MemoryStream stream = new MemoryStream();		
				XmlTextWriter writer = new XmlTextWriter(stream,System.Text.Encoding.ASCII);
				writer.Formatting = System.Xml.Formatting.Indented;
				//writer.WriteRaw(mediator.Editor.Text);

				writer.WriteString(mediator.Editor.Text);

				System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();

				//StringReader reader = new StringReader();
				int count = 0;
				stream.Seek(0, SeekOrigin.Begin);

				byte[] byteArray = new byte[stream.Length];

				while(count < stream.Length)
				{
					byteArray[count++] = Convert.ToByte(stream.ReadByte());
				}

				// Decode the byte array into a char array 
				// and write it to the console.
				char[] charArray = new char[asciiEncoding.GetCharCount(byteArray, 0, count)];
				asciiEncoding.GetDecoder().GetChars(byteArray, 0, count, charArray, 0);

				string s = new string(charArray);
				mediator.Editor.Text = s;
				stream.Close();
				writer.Close();
			}
			catch(Exception exc)
			{
				mediator.Output(exc.Message);
			}
		}

		/// <summary>
		/// Transfers the diagram'sNML to the editor
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuNMLToEditor_Click(object sender, System.EventArgs e)
		{
			this.mediator.TransferNMLToEditor();
		}


		private void mnuSaveTextFile_Click(object sender, System.EventArgs e)
		{
			SaveTextFile();
		}

		#region Cut, copy, paste, delete
		private void mnuEditorCut_Click(object sender, System.EventArgs e)
		{
			mediator.Editor.ActiveTextAreaControl.TextArea.ExecuteDialogKey(Keys.X | Keys.Control);
		}

		private void mnuEditorCopy_Click(object sender, System.EventArgs e)
		{
			mediator.Editor.ActiveTextAreaControl.TextArea.ExecuteDialogKey(Keys.C | Keys.Control);
		}

		private void mnuEditorPaste_Click(object sender, System.EventArgs e)
		{
			mediator.Editor.ActiveTextAreaControl.TextArea.ExecuteDialogKey(Keys.V | Keys.Control);
		}

		private void mnuEditorDelete_Click(object sender, System.EventArgs e)
		{
			mediator.Editor.ActiveTextAreaControl.TextArea.ExecuteDialogKey(Keys.D | Keys.Control);
		}
		#endregion

		#region Highlighting
		private void mnuHighlightHTML_Click(object sender, System.EventArgs e)
		{
			mediator.Editor.SetHighlighting("HTML");
		}
		

		private void mnuHighlightXML_Click(object sender, System.EventArgs e)
		{
			mediator.Editor.SetHighlighting("XML");
		}

		private void mnuHighlighCsharp_Click(object sender, System.EventArgs e)
		{
				mediator.Editor.SetHighlighting("C#");
		}

		private void mnuHighlightVBNet_Click(object sender, System.EventArgs e)
		{
				mediator.Editor.SetHighlighting("VBNET");
		}

		private void mnuHighlightNone_Click(object sender, System.EventArgs e)
		{
				mediator.Editor.SetDefaultHighlighting();
		}

		#endregion
		
		#region Colored visit
		/// <summary>
		/// Demonstrates the use of the analysis service
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuColoredVisit_Click(object sender, System.EventArgs e)
		{
			Netron.GraphLib.Analysis.GraphAnalyzer analyzer = new Netron.GraphLib.Analysis.GraphAnalyzer(this.mediator.GraphControl.Abstract,true);
			MyVisitor visitor = new MyVisitor(analyzer);
			analyzer.DepthFirstTraversal(visitor,0);
		}

		/// <summary>
		/// Sample IVisitor implementation
		/// </summary>
		public class MyVisitor : Netron.GraphLib.Analysis.AbstractPrePostVisitor
		{

			private GraphLib.Analysis.GraphAnalyzer analyzer;

			private Random rnd = new Random();

			public MyVisitor(GraphLib.Analysis.GraphAnalyzer analyzer)
			{
				this.analyzer = analyzer;
			}

			public override void Visit(object obj)
			{
				GraphLib.Analysis.IVertex vertex = obj as GraphLib.Analysis.IVertex;
				if(vertex !=null)
				{
					Shape shape = analyzer.GetShape(vertex.Number);
					shape.ShapeColor = Color.FromArgb(rnd.Next(10,200),rnd.Next(10,200),rnd.Next(10,200));
				}
				
			}


		}
		#endregion

		private void mnuTest_Click(object sender, System.EventArgs e)
		{
			//			mediator.OpenBrowserTab();
			//			mediator.Browser.Navigate("about:blank");
			//			mediator.Browser.AxWebBrowser.Html = "testje";
				
		}

		private void mnuTest_Click_1(object sender, System.EventArgs e)
		{
			
		}

		private void mnuDiagramBrowser_Click(object sender, System.EventArgs e)
		{
			mediator.OpenDiagramBrowserTab();
		}

		/// <summary>
		/// Creates a template from the selected shapes
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuCreateTemplate_Click(object sender, System.EventArgs e)
		{
			if(mediator.GraphControl.SelectedShapes.Count<1)
			{
				MessageBox.Show("Make first a selection in the diagram; a template is a subset of an existing diagram","No shapes selected",MessageBoxButtons.OK,MessageBoxIcon.Information);
				return;
			}
			using (TemplatePropertiesDialog dialog = new TemplatePropertiesDialog())
			{
				DialogResult res= dialog.ShowDialog(this);
				if(res==DialogResult.OK)
				{
					
					EntityBundle bundle= mediator.GraphControl.BundleSelection();		
					bundle.Name = dialog.TemplateName.Text;
					bundle.Description = dialog.Description.Text;
					mediator.FavTab.AddFavorite(bundle, dialog.TemplateName.Text.Trim(),dialog.Description.Text.Trim());
					mediator.Output("New template '" + dialog.TemplateName.Text.Trim() + "' was added. Double-click the template name in the favorites to load it in the diagram.",OutputInfoLevels.Info);
				}
			}
		}

		/// <summary>
		/// Output SVG
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuCreateSVG_Click(object sender, System.EventArgs e)
		{
			mediator.Output("SVG export is still in an experimental stadium, see the SVGSerializer class for more.", OutputInfoLevels.Info);
			string tmpFile = Path.GetTempFileName();
			tmpFile = Path.ChangeExtension(tmpFile,".svg");
			Netron.GraphLib.IO.SVG.SVGSerializer.SaveAs(tmpFile, mediator.GraphControl);			
			try
			{
				Process.Start(tmpFile);
			}
			catch
			{
				//probably the SVG plugin is not installed
				MessageBox.Show(@"The Cobalt application tried to launch the application associeted to SVG file types but this resulted in an exception. Probably you haven't installed a SVG-viewer. If you're using IE, see Adobe's SVG-site http://www.adobe.com/svg/viewer/install/ for a browser plugin. For FireFox, Opera and other browser you'll need a stand-alone application; see the SVG site for more http://svg.org/.", "Error; no SVG application?", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void mnuShowMode_Click(object sender, System.EventArgs e)
		{
			mnuShowMode.Checked = !mnuShowMode.Checked;
			ShowMode(mnuShowMode.Checked);
		}


		private void ShowMode(bool value)
		{			
			mediator.HideAllTabs();

			//set the menu
			this.mnuApplication.Visible = true;		
			this.mnuEditorMenu.Visible = !value;
			this.mnuWindows.Visible = !value;
			this.mnuHelp.Visible = !value;
			this.mnuDiagram.Visible = !value;
			this.mnuExamples.Visible = !value;
			this.mnuZoom.Visible = !value;

			
			mediator.OpenGraphTab();
			
			if(value)
			{
				this.WindowState = FormWindowState.Maximized;
				this.FormBorderStyle = FormBorderStyle.None;
				mediator.GraphTab.AllowRedocking = false;
			}
			else
			{
				this.FormBorderStyle = FormBorderStyle.Sizable;
				mediator.GraphTab.AllowRedocking = true;
			}
				

		}

		private void mnuSave2HTML_Click(object sender, System.EventArgs e)
		{
			SaveFileDialog fileChooser=new SaveFileDialog();
			fileChooser.CheckFileExists=false;
			
			fileChooser.Filter = "HTML files (*.htm; *.html)|*.htm; *.html";
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
				Netron.GraphLib.IO.HTML.HTMLExporter exporter = new Netron.GraphLib.IO.HTML.HTMLExporter(mediator.GraphControl);
				exporter.SaveAs(filename);	
			}
		}

		private void mnuCreateImageShape_Click(object sender, System.EventArgs e)
		{
			if(mediator.GraphControl.SelectedShapes.Count<1)
			{
				MessageBox.Show("Make first a selection in the diagram; a new ImageShape will be created on the basis of the selected shapes and connections (i.e. a screenshot). You can save the newly created ImageShape thereafter as a template to re-use it in other diagrams.","No shapes selected",MessageBoxButtons.OK,MessageBoxIcon.Information);
				return;
			}
			try
			{
				EntityBundle bundle = mediator.GraphControl.GroupSelection();
				Shape shape = mediator.GraphControl.AddShape("47D016B9-990A-436c-ADE8-B861714EBE5A", new PointF(bundle.Rectangle.X, bundle.Rectangle.Y));		
				if(shape==null)
				{
					MessageBox.Show("This feature depends on the reflected Entitology shape library. The library or the ImageShape could not be found however and the creation of the shape failed. Please check the configuration of the application and the outputted exception message for more details regarding this error.", "Shape or library not found.",MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				
			
				PropertyInfo info = shape.GetType().GetProperty("Image");
				info.SetValue(shape,bundle.BundleImage, null);
				shape.Invalidate();		
			}
			catch(Exception exc)
			{
				mediator.Output(exc.Message);
			}
		}

		private void mnuCopy_Click(object sender, System.EventArgs e)
		{
			mediator.GraphControl.Copy();
		}

		private void mnuPaste_Click(object sender, System.EventArgs e)
		{
			mediator.GraphControl.Paste();
		}

		private void mnuCut_Click(object sender, System.EventArgs e)
		{
			mediator.GraphControl.Cut();
		}

		private void mnuDelete_Click(object sender, System.EventArgs e)
		{
			mediator.GraphControl.Delete();
		}

		private void mnuCopyAsImage_Click(object sender, System.EventArgs e)
		{
			if(mediator.GraphControl.SelectedShapes.Count<1)
			{
				MessageBox.Show("Nothing selected: make a diagram selection first.","No shapes selected",MessageBoxButtons.OK,MessageBoxIcon.Information);
				return;
			}
			mediator.GraphControl.CopyAsImage();	
			
		
		}

		private void mnuSelectAll_Click(object sender, System.EventArgs e)
		{
			mediator.GraphControl.SelectAll(true);
		}

		private void mnuChangeLog_Click(object sender, System.EventArgs e)
		{
			Process.Start("ChangeLog.txt");
		}

		private void mnuSampleDiagrams_Click(object sender, System.EventArgs e)
		{
			Process.Start(@"..\..\..\HTML\default.htm");
		}

		private void mnuReadme_Click(object sender, System.EventArgs e)
		{
			Process.Start(@"..\..\..\HTML\Readme20050728.htm");
		}


	}
}

