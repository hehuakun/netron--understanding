using System;
using System.Windows.Forms;
using Netron.Neon;
using Netron.Neon.TextEditor.Document;

namespace Netron.Cobalt
{
	/// <summary>
	/// Summary description for CodeTab.
	/// </summary>
	public class CodeTab : DockContent, ICobaltTab
	{
		#region Fields
		private Mediator mediator;
		private Netron.Neon.TextEditor.TextEditorControl editor;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem mnuHighlighting;
		private System.Windows.Forms.MenuItem mnuHighlightXML;
		private System.Windows.Forms.MenuItem mnuHighlightCS;
		private System.Windows.Forms.MenuItem mnuHighlightVBNet;
		private System.Windows.Forms.MenuItem mnuHighlightNone;
		private System.Windows.Forms.MenuItem mnuHighlightHTML;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem mnuClearAll;
		private System.Windows.Forms.MenuItem mnuOpenFile;
		private System.Windows.Forms.MenuItem mnuSaveFile;
		private string identifier;
		#endregion

		#region Properties

		public TabTypes TabType
		{
			get{return TabTypes.Code;}
		}

		public string TabIdentifier
		{
			get{return this.identifier;}
			set{identifier = value;}
		}


		public string EditorText
		{
			get
			{
				return this.editor.Text;
			}
			set{this.editor.Text = value;}
		}

		public Netron.Neon.TextEditor.TextEditorControl Editor
		{
			get{
				return editor;
			}
		}

		#endregion

		#region Constructor
		public CodeTab(Mediator mediator)
		{
			InitializeComponent();

			this.mediator = mediator;

			this.editor.ShowEOLMarkers = false;
			this.editor.ShowHRuler = false;
			this.editor.ShowSpaces = false;
			this.editor.ShowTabs = false;
			this.editor.ShowLineNumbers = true;
			this.editor.ShowMatchingBracket = true;

			AddContextMenu();
			
		}
		#endregion

		#region Methods

		private void AddContextMenu()
		{
			
		}
		

		public void GotoLine(int line)
		{
			

			if (line != 0)
			{
//				i = 1;
//				pos = 0;
//				while (i < l)
//				{
//					pos = this.editor.Text.IndexOf(Environment.NewLine, pos + 1);
//					i++;
//				}
				this.editor.ActiveTextAreaControl.Caret.Line = line-1;
				
				//				editor.SelectionStart = pos;
				//				editor.SelectionLength = editor.Text.IndexOf(Environment.NewLine, pos + 1) - pos;
			}
		}

		public void LoadFile(string path)
		{
			this.editor.LoadFile(path);
			this.editor.EnableFolding = true;
		}
		public void SaveFile(string path)
		{
			this.editor.SaveFile(path);
		}

		private void InitializeComponent()
		{
//		    AddDocument();
		    System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CodeTab));
            this.editor = new Netron.Neon.TextEditor.TextEditorControl();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.mnuHighlighting = new System.Windows.Forms.MenuItem();
            this.mnuHighlightXML = new System.Windows.Forms.MenuItem();
            this.mnuHighlightCS = new System.Windows.Forms.MenuItem();
            this.mnuHighlightVBNet = new System.Windows.Forms.MenuItem();
            this.mnuHighlightHTML = new System.Windows.Forms.MenuItem();
            this.mnuHighlightNone = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.mnuClearAll = new System.Windows.Forms.MenuItem();
            this.mnuOpenFile = new System.Windows.Forms.MenuItem();
            this.mnuSaveFile = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // editor
            // 
            this.editor.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.editor.Document = defaultDocument1;
            this.editor.Encoding = ((System.Text.Encoding)(resources.GetObject("editor.Encoding")));
            this.editor.IndentStyle = Netron.Neon.TextEditor.Document.IndentStyle.None;
            this.editor.LineViewerStyle = Netron.Neon.TextEditor.Document.LineViewerStyle.FullRow;
            this.editor.Location = new System.Drawing.Point(0, 0);
            this.editor.Name = "editor";
            this.editor.Root = null;
            this.editor.ShowEOLMarkers = true;
            this.editor.ShowInvalidLines = false;
            this.editor.ShowMatchingBracket = false;
            this.editor.ShowSpaces = true;
            this.editor.ShowTabs = true;
            this.editor.ShowVRuler = true;
            this.editor.Size = new System.Drawing.Size(292, 266);
            this.editor.Tab = null;
            this.editor.TabIndex = 0;
//            this.editor.TextEditorProperties = defaultTextEditorProperties1;
            // 
            // contextMenu1
            // 
            this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuHighlighting,
            this.menuItem7,
            this.mnuClearAll,
            this.mnuOpenFile,
            this.mnuSaveFile});
            // 
            // mnuHighlighting
            // 
            this.mnuHighlighting.Index = 0;
            this.mnuHighlighting.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuHighlightXML,
            this.mnuHighlightCS,
            this.mnuHighlightVBNet,
            this.mnuHighlightHTML,
            this.mnuHighlightNone});
            this.mnuHighlighting.Text = "HighLighting";
            // 
            // mnuHighlightXML
            // 
            this.mnuHighlightXML.Index = 0;
            this.mnuHighlightXML.Text = "XML";
            this.mnuHighlightXML.Click += new System.EventHandler(this.mnuHighlightXML_Click);
            // 
            // mnuHighlightCS
            // 
            this.mnuHighlightCS.Index = 1;
            this.mnuHighlightCS.Text = "C#";
            this.mnuHighlightCS.Click += new System.EventHandler(this.mnuHighlightCS_Click);
            // 
            // mnuHighlightVBNet
            // 
            this.mnuHighlightVBNet.Index = 2;
            this.mnuHighlightVBNet.Text = "VB.Net";
            this.mnuHighlightVBNet.Click += new System.EventHandler(this.mnuHighlightVBNet_Click);
            // 
            // mnuHighlightHTML
            // 
            this.mnuHighlightHTML.Index = 3;
            this.mnuHighlightHTML.Text = "HTML";
            this.mnuHighlightHTML.Click += new System.EventHandler(this.mnuHighlightHTML_Click);
            // 
            // mnuHighlightNone
            // 
            this.mnuHighlightNone.Index = 4;
            this.mnuHighlightNone.Text = "None";
            this.mnuHighlightNone.Click += new System.EventHandler(this.mnuHighlightNone_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 1;
            this.menuItem7.Text = "-";
            // 
            // mnuClearAll
            // 
            this.mnuClearAll.Index = 2;
            this.mnuClearAll.Text = "Clear all";
            this.mnuClearAll.Click += new System.EventHandler(this.mnuClearAll_Click);
            // 
            // mnuOpenFile
            // 
            this.mnuOpenFile.Index = 3;
            this.mnuOpenFile.Text = "Open file...";
            this.mnuOpenFile.Click += new System.EventHandler(this.mnuOpenFile_Click);
            // 
            // mnuSaveFile
            // 
            this.mnuSaveFile.Index = 4;
            this.mnuSaveFile.Text = "Save file...";
            this.mnuSaveFile.Click += new System.EventHandler(this.mnuSaveFile_Click);
            // 
            // CodeTab
            // 
            this.AccessibleDescription = "The code panel allows you to edit any text and NML in particular. You can go back" +
    " and forth between the diagram and NML by pressing F5 and SHIFT-F5.";
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.editor);
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CodeTab";
            this.ShowHint = Netron.Neon.DockState.Document;
            this.TabPageContextMenu = this.contextMenu1;
            this.TabText = "Code";
            this.ResumeLayout(false);

		}

	    private static void AddDocument()
	    {
	        Netron.Neon.TextEditor.Document.DefaultDocument defaultDocument1 =
	            new Netron.Neon.TextEditor.Document.DefaultDocument();
	        Netron.Neon.TextEditor.Document.DefaultFormattingStrategy defaultFormattingStrategy1 =
	            new Netron.Neon.TextEditor.Document.DefaultFormattingStrategy();
	        Netron.Neon.TextEditor.Document.DefaultHighlightingStrategy defaultHighlightingStrategy1 =
	            new Netron.Neon.TextEditor.Document.DefaultHighlightingStrategy();
	        Netron.Neon.TextEditor.Document.GapTextBufferStrategy gapTextBufferStrategy1 =
	            new Netron.Neon.TextEditor.Document.GapTextBufferStrategy();
	        Netron.Neon.TextEditor.Document.DefaultTextEditorProperties defaultTextEditorProperties1 =
	            new Netron.Neon.TextEditor.Document.DefaultTextEditorProperties();
            defaultDocument1.FormattingStrategy = defaultFormattingStrategy1;
            defaultHighlightingStrategy1.Extensions = new string[0];
            defaultDocument1.HighlightingStrategy = defaultHighlightingStrategy1;
            defaultDocument1.ReadOnly = false;
            defaultDocument1.TextBufferStrategy = gapTextBufferStrategy1;
            defaultDocument1.TextContent = "";
            defaultTextEditorProperties1.AllowCaretBeyondEOL = false;
            defaultTextEditorProperties1.AutoInsertCurlyBracket = true;
            defaultTextEditorProperties1.BracketMatchingStyle = Netron.Neon.TextEditor.Document.BracketMatchingStyle.After;
            defaultTextEditorProperties1.ConvertTabsToSpaces = false;
            defaultTextEditorProperties1.CreateBackupCopy = false;
            defaultTextEditorProperties1.DocumentSelectionMode = Netron.Neon.TextEditor.Document.DocumentSelectionMode.Normal;
            defaultTextEditorProperties1.EnableFolding = true;
//            defaultTextEditorProperties1.Encoding = ((System.Text.Encoding)(resources.GetObject("defaultTextEditorProperties1.Encoding")));
            defaultTextEditorProperties1.Font = new System.Drawing.Font("Courier New", 10F);
            defaultTextEditorProperties1.HideMouseCursor = false;
            defaultTextEditorProperties1.IndentStyle = Netron.Neon.TextEditor.Document.IndentStyle.None;
            defaultTextEditorProperties1.IsIconBarVisible = true;
            defaultTextEditorProperties1.LineTerminator = "\r\n";
            defaultTextEditorProperties1.LineViewerStyle = Netron.Neon.TextEditor.Document.LineViewerStyle.FullRow;
            defaultTextEditorProperties1.MouseWheelScrollDown = true;
            defaultTextEditorProperties1.MouseWheelTextZoom = true;
            defaultTextEditorProperties1.ShowEOLMarker = true;
            defaultTextEditorProperties1.ShowHorizontalRuler = false;
            defaultTextEditorProperties1.ShowInvalidLines = false;
            defaultTextEditorProperties1.ShowLineNumbers = true;
            defaultTextEditorProperties1.ShowMatchingBracket = false;
            defaultTextEditorProperties1.ShowSpaces = true;
            defaultTextEditorProperties1.ShowTabs = true;
            defaultTextEditorProperties1.ShowVerticalRuler = true;
            defaultTextEditorProperties1.TabIndent = 4;
            defaultTextEditorProperties1.UseAntiAliasedFont = false;
            defaultTextEditorProperties1.VerticalRulerRow = 80;
            defaultDocument1.TextEditorProperties = defaultTextEditorProperties1;

        }

        public void SetHighlighting(string light)
		{
			this.editor.SetHighlighting(light);
		}
		#endregion

		

		private void mnuHighlightXML_Click(object sender, System.EventArgs e)
		{
			mediator.Editor.SetHighlighting("XML");
		}

		private void mnuHighlightCS_Click(object sender, System.EventArgs e)
		{
			mediator.Editor.SetHighlighting("C#");
		}

		private void mnuHighlightVBNet_Click(object sender, System.EventArgs e)
		{
			mediator.Editor.SetHighlighting("VBNET");
		}

		private void mnuHighlightHTML_Click(object sender, System.EventArgs e)
		{
			mediator.Editor.SetHighlighting("HTML");
		}

		private void mnuHighlightNone_Click(object sender, System.EventArgs e)
		{
			mediator.Editor.SetDefaultHighlighting();
		}

		private void mnuClearAll_Click(object sender, System.EventArgs e)
		{
			mediator.Editor.Text = "";
		}

		private void mnuOpenFile_Click(object sender, System.EventArgs e)
		{
			mediator.OpenTextFile();
		}

		private void mnuSaveFile_Click(object sender, System.EventArgs e)
		{
			mediator.SaveTextFile();
		}

	
	}
}
