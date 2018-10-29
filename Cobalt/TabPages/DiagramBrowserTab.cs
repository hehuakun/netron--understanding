using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Netron.Neon;
using System.IO;
using Netron.GraphLib.IO.Reporting;
using Netron.GraphLib;
namespace Netron.Cobalt
{
	/// <summary>
	/// Summary description for TestTab.
	/// </summary>
	public class DiagramBrowserTab : DockContent, ICobaltTab
	{
		private System.ComponentModel.IContainer components;
		#region Fields

		private System.Windows.Forms.TextBox browsePath;
		private System.Windows.Forms.Button browseButton;
		private string identifier;
		private Netron.Neon.NeonBaseList baseList;
		private Mediator mediator;
		private readonly Color DefaultCardColor = Color.SteelBlue;
		private readonly Color SelectedCardColor = Color.Orange;
		private DiagramReport currentCard;
		private System.Windows.Forms.Button SmallBig;
		private System.Windows.Forms.Button RefreshButton;
		private System.Windows.Forms.ToolTip toolTip1;
		private string currentDirectory;

		#endregion
		
		#region Properties
		public string TabIdentifier
		{
			get
			{
				
				return this.identifier;
			}
			set
			{
				this.identifier = value;
			}
		}
		public bool Thumbnails
		{
			get{return (this.baseList.ControlSize.Width != 720);}
			set{
				if(value)					
						this.baseList.ControlSize = new Size(165,165);
					else
						this.baseList.ControlSize= new Size(720,165);
			}
		}
		#endregion

		#region Constructor
		public DiagramBrowserTab(Mediator mediator)
		{
			
			InitializeComponent();

			this.mediator = mediator;
			this.baseList.ControlPaddings.Left = 15;

		}

		#endregion

		#region Methods
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DiagramBrowserTab));
			this.baseList = new Netron.Neon.NeonBaseList();
			this.browsePath = new System.Windows.Forms.TextBox();
			this.browseButton = new System.Windows.Forms.Button();
			this.SmallBig = new System.Windows.Forms.Button();
			this.RefreshButton = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// baseList
			// 
			this.baseList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.baseList.AutoScroll = true;
			this.baseList.BackColor = System.Drawing.Color.WhiteSmoke;
			this.baseList.BorderColor = System.Drawing.Color.Black;
			this.baseList.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
			this.baseList.ControlSize = new System.Drawing.Size(200, 200);
			this.baseList.Location = new System.Drawing.Point(0, 64);
			this.baseList.Name = "baseList";
			this.baseList.Size = new System.Drawing.Size(500, 308);
			this.baseList.TabIndex = 0;
			this.baseList.SelectionChanged += new Netron.Neon.NeonBaseList.SelectionChangedHandler(this.baseList_SelectionChanged);
			// 
			// browsePath
			// 
			this.browsePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.browsePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.browsePath.Location = new System.Drawing.Point(6, 10);
			this.browsePath.Name = "browsePath";
			this.browsePath.Size = new System.Drawing.Size(457, 21);
			this.browsePath.TabIndex = 1;
			this.browsePath.Text = "";
			this.toolTip1.SetToolTip(this.browsePath, "Press enter to load the given directory");
			this.browsePath.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.browsePath_KeyPress);
			// 
			// browseButton
			// 
			this.browseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.browseButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.browseButton.Location = new System.Drawing.Point(469, 10);
			this.browseButton.Name = "browseButton";
			this.browseButton.Size = new System.Drawing.Size(23, 20);
			this.browseButton.TabIndex = 2;
			this.browseButton.Text = "...";
			this.toolTip1.SetToolTip(this.browseButton, "Select a directory");
			this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
			// 
			// SmallBig
			// 
			this.SmallBig.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.SmallBig.Image = ((System.Drawing.Image)(resources.GetObject("SmallBig.Image")));
			this.SmallBig.Location = new System.Drawing.Point(7, 38);
			this.SmallBig.Name = "SmallBig";
			this.SmallBig.Size = new System.Drawing.Size(23, 20);
			this.SmallBig.TabIndex = 3;
			this.toolTip1.SetToolTip(this.SmallBig, "Small/big cards");
			this.SmallBig.Click += new System.EventHandler(this.SmallBig_Click);
			// 
			// RefreshButton
			// 
			this.RefreshButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.RefreshButton.Image = ((System.Drawing.Image)(resources.GetObject("RefreshButton.Image")));
			this.RefreshButton.Location = new System.Drawing.Point(38, 39);
			this.RefreshButton.Name = "RefreshButton";
			this.RefreshButton.Size = new System.Drawing.Size(23, 20);
			this.RefreshButton.TabIndex = 4;
			this.toolTip1.SetToolTip(this.RefreshButton, "Refresh the view");
			this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
			// 
			// DiagramBrowserTab
			// 
			this.AccessibleDescription = "The diagram browser allows you to browse saved diagrams with ease. Click on the t" +
				"humbnail to open the diagram.";
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(500, 372);
			this.Controls.Add(this.RefreshButton);
			this.Controls.Add(this.SmallBig);
			this.Controls.Add(this.browseButton);
			this.Controls.Add(this.browsePath);
			this.Controls.Add(this.baseList);
			this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DiagramBrowserTab";
			this.Text = "Diagram browser";
			this.ResumeLayout(false);

		}
		
		#endregion

		private void browseButton_Click(object sender, System.EventArgs e)
		{
			SelectDirectory();
		}

		private void SelectDirectory()
		{
			Netron.Neon.FolderBrowser	 browser = new FolderBrowser(this);
			browser.Caption = "Select a folder wherein the diagrams are stored:";
			browser.Title = "Binary diagrams folder";
			string path = string.Empty;
			if((path=browser.BrowseForFolder())!=null)
			{
				browsePath.Text = path;
				currentDirectory = path;
				LoadDirectory(path, true);
			}
		}

		
		
		private void browsePath_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar==(char) Keys.Enter)
			{
				if(browsePath.Text.Trim()!=string.Empty)
				{
					currentDirectory = browsePath.Text.Trim();
					LoadDirectory(browsePath.Text.Trim(), true);
				}
			}
		}
		/// <summary>
		/// Loads the given directory in the browser
		/// </summary>
		/// <param name="path"></param>
		/// <param name="showGauge">whether the gauge should be shown</param>
		public void LoadDirectory(string path, bool showGauge)
		{
#if !DEBUG
	mediator.parent.RaiseLoading("Loading diagrams in the diagram browser...");
#endif
			try
			{
				
				this.Cursor = Cursors.WaitCursor;
				if(showGauge)
				{					
					SetUIState(false);
				}
				if(!Directory.Exists(path)) return;
				path = Path.GetFullPath(path);
				browsePath.Text = path;
				currentDirectory = path;
				baseList.Controls.Clear();
				baseList.Redraw();
				GraphLib.IO.Reporting.BinaryReporter reporter = new Netron.GraphLib.IO.Reporting.BinaryReporter(path);
				reporter.OnReport+=new InfoDelegate(OnReport);
				GraphLib.IO.Reporting.BinaryReportCollection col = reporter.Report() as GraphLib.IO.Reporting.BinaryReportCollection;
				DiagramReport card;
				BinaryReport report = null;
				//stop the layout for a moment until the list is filled
				
				baseList.BeginUpdate();
				for(int k = 0; k<col.Count; k++)
				{
					try
					{

						card = new DiagramReport();
						report = col[k];
						card.FileName.Text = new System.IO.FileInfo(report.Path).Name;
						card.BackColor = DefaultCardColor;					
						card.ThumbNail.Image = report.Thumbnail;
						card.Title = report.Title==""? "[Not set]": report.Title;
						card.FilePath = report.Path;						
						card.Author.Text = report.Author==""?"[Not set]": report.Author;
						card.Description.Text = report.Description==""?"[Not set]":report.Description;
						card.CreationDate.Text = report.CreationDate==""? "[Not set]":report.CreationDate;					
						#region Connect events
						card.LoadDiagram+=new Netron.Cobalt.DiagramReport.PathInfo(card_LoadDiagram);
						card.SelectDirectory+=new EventHandler(card_SelectDirectory);
						card.ReloadDirectory+=new EventHandler(card_ReloadDirectory);
						card.DeleteDiagram+=new Netron.Cobalt.DiagramReport.PathInfo(card_DeleteDiagram);
						#endregion
						baseList.Controls.Add(card);
						mediator.Output("Found '" + card.FileName.Text + "' in directory.",GraphLib.OutputInfoLevels.Info);
#if !DEBUG
						mediator.parent.RaiseLoading("...found diagram '" + card.FileName.Text +"'");
						System.Threading.Thread.Sleep(300);
#endif
					
					}
					catch
					{
						mediator.Output("File '" + new System.IO.FileInfo(report.Path).Name + "' seems not to be a valid diagram file.",GraphLib.OutputInfoLevels.Info);
						continue;
					}
					baseList.EndUpdate();
					
				}
			}
			catch(System.IO.DirectoryNotFoundException e1)
			{
				mediator.Output("The directory was not found",GraphLib.OutputInfoLevels.Exception);
				Trace.WriteLine(e1.Message,"DiagramBrowserTab.LoadDirectory");
			}
			catch(System.IO.FileNotFoundException e2)
			{
				mediator.Output("The file was not found",GraphLib.OutputInfoLevels.Exception);
				Trace.WriteLine(e2.Message,"DiagramBrowserTab.LoadDirectory");
			}
			catch(Exception e)
			{
				mediator.Output(e.Message,GraphLib.OutputInfoLevels.Exception);
				Trace.WriteLine(e.Message,"DiagramBrowserTab.LoadDirectory");
			}
			finally
			{
				this.Cursor = Cursors.Default;
				SetUIState(true);				
			}
			
		}

		private void OnReport(object sender, OutputInfoLevels level)
		{
			Trace.WriteLine("Found " + (sender as BinaryReport).Title);
		}

		private void SetUIState(bool value)
		{
			baseList.Enabled = value;
			this.RefreshButton.Enabled = value;
			this.SmallBig.Enabled = value;
			this.browseButton.Enabled = value;
			this.browsePath.Enabled = value;
		
		
		}

		public Netron.Cobalt.TabTypes TabType
		{
			get
			{
				return TabTypes.DiagramBrowser;
			}
		}

		
		private void card_LoadDiagram(string path)
		{
			mediator.GraphControl.NewDiagram(true);
			mediator.GraphControl.Open(path);
		}

		private void baseList_SelectionChanged(object sender, System.EventArgs e)
		{
			if(baseList.SelectedControl!=null)
			{
				if(currentCard!=null) currentCard.BackColor = DefaultCardColor;				
				currentCard = baseList.SelectedControl as DiagramReport;
				currentCard.BackColor = SelectedCardColor;				
			}
		}

		private void card_SelectDirectory(object sender, EventArgs e)
		{
			SelectDirectory();
		}

		private void card_ReloadDirectory(object sender, EventArgs e)
		{
			LoadDirectory(currentDirectory, true);
		}
		private void card_DeleteDiagram(string path)
		{
			DialogResult res = MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if(res==DialogResult.Yes)
			{
				
				try
				{
					File.Delete(path);
					System.Threading.Thread.Sleep(1300);	
					this.baseList.Controls.Remove(currentCard);
				}
				catch{}//could happen if the deletion doesn't occur fast enough and the directory is reloaded
				
				//LoadDirectory(currentDirectory);
			}
		}

		private void SmallBig_Click(object sender, System.EventArgs e)
		{
			if(this.baseList.ControlSize.Width == 720)
				this.baseList.ControlSize = new Size(165,165);
			else
				this.baseList.ControlSize= new Size(720,165);
			baseList.Redraw();
			//baseList.Invalidate(true);

		}
		
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged (e);
			baseList.Redraw();
		}



		private void RefreshButton_Click(object sender, System.EventArgs e)
		{
			LoadDirectory(currentDirectory, true);
		}

	
		#endregion
	}
}
