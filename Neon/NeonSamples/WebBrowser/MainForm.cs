using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Netron.Neon;
namespace WebBrowser
{
	/// <summary>
	/// A multi-tabbed browser in a coupld of lines
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		#region Fields
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem mnuExit;
		private System.Windows.Forms.MenuItem mnuFavorites;
		private System.Windows.Forms.MenuItem mnuHome;
		internal System.Windows.Forms.MenuItem mnuWindows;
		private System.Windows.Forms.MenuItem mnuFile;
		private System.Windows.Forms.StatusBar statusBar1;
		internal Netron.Neon.DockPanel dockPanel;
		private System.Windows.Forms.MenuItem mnuCloseAll;
		Mediator mediator;
		#endregion

		#region Constructor
		public MainForm()
		{
			
			InitializeComponent();

			mediator = new Mediator(this);
			mediator.GetTab(TabTypes.Favorites,"") ;			
		
			//Load an empty browser with the custom Html and external handler
			mediator.GetTab(TabTypes.Browser,"Home");
			
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
				if (components != null) 
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.mnuFile = new System.Windows.Forms.MenuItem();
			this.mnuExit = new System.Windows.Forms.MenuItem();
			this.mnuWindows = new System.Windows.Forms.MenuItem();
			this.mnuFavorites = new System.Windows.Forms.MenuItem();
			this.mnuHome = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.statusBar1 = new System.Windows.Forms.StatusBar();
			this.dockPanel = new Netron.Neon.DockPanel();
			this.mnuCloseAll = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuFile,
																					  this.mnuWindows});
			// 
			// mnuFile
			// 
			this.mnuFile.Index = 0;
			this.mnuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuExit});
			this.mnuFile.Text = "File";
			// 
			// mnuExit
			// 
			this.mnuExit.Index = 0;
			this.mnuExit.Text = "Exit";
			this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
			// 
			// mnuWindows
			// 
			this.mnuWindows.Index = 1;
			this.mnuWindows.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.mnuFavorites,
																					   this.mnuHome,
																					   this.mnuCloseAll,
																					   this.menuItem3});
			this.mnuWindows.Text = "Windows";
			// 
			// mnuFavorites
			// 
			this.mnuFavorites.Index = 0;
			this.mnuFavorites.Text = "Favorites";
			this.mnuFavorites.Click += new System.EventHandler(this.mnuFavorites_Click);
			// 
			// mnuHome
			// 
			this.mnuHome.Index = 1;
			this.mnuHome.Text = "Home";
			this.mnuHome.Click += new System.EventHandler(this.mnuHome_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 3;
			this.menuItem3.Text = "-";
			// 
			// statusBar1
			// 
			this.statusBar1.Location = new System.Drawing.Point(0, 431);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Size = new System.Drawing.Size(608, 22);
			this.statusBar1.TabIndex = 0;
			// 
			// dockPanel
			// 
			this.dockPanel.ActiveAutoHideContent = null;
			this.dockPanel.BackColor = System.Drawing.Color.SteelBlue;
			this.dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dockPanel.Font = new System.Drawing.Font("Verdana", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World, ((System.Byte)(0)));
			this.dockPanel.Location = new System.Drawing.Point(0, 0);
			this.dockPanel.Name = "dockPanel";
			this.dockPanel.Size = new System.Drawing.Size(608, 431);
			this.dockPanel.TabIndex = 1;
			// 
			// mnuCloseAll
			// 
			this.mnuCloseAll.Index = 2;
			this.mnuCloseAll.Text = "Close all";
			this.mnuCloseAll.Click += new System.EventHandler(this.mnuCloseAll_Click);
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(608, 453);
			this.Controls.Add(this.dockPanel);
			this.Controls.Add(this.statusBar1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu1;
			this.Name = "MainForm";
			this.Text = "Neon browser";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new MainForm());
		}

	
		#endregion

		private void mnuExit_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		private void mnuFavorites_Click(object sender, System.EventArgs e)
		{
			mediator.GetTab(TabTypes.Favorites,"Favorites");
		}

		private void mnuHome_Click(object sender, System.EventArgs e)
		{
			mediator.GetTab(TabTypes.Browser,"Home");
		}

		private void mnuCloseAll_Click(object sender, System.EventArgs e)
		{
			mediator.CloseAll();			
		}
	}
}
