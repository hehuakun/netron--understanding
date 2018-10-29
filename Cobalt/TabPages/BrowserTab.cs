using System;
using System.Drawing;
using System.Windows.Forms;
using Netron.Neon;

namespace Netron.Cobalt
{
	/// <summary>
	/// Summary description for CodeTab.
	/// </summary>
	public class BrowserTab : DockContent, ICobaltTab
	{
		#region Fields

		private Mediator mediator;
		private NBrowser browser;
		private string identifier;
	
		#endregion

		#region Properties

//		public TabTypes TabType
//		{
//			get{return TabTypes.Browser;}
//		}

		public string TabIdentifier
		{
			get{return this.identifier;}
			set{identifier = value;}
		}

		public NBrowser Browser
		{
			get{return browser;}
			set{browser = value;}
		}

		
		public TabTypes TabType
		{
			get{return TabTypes.Browser;}
		}
		

		#endregion

		#region Constructor
		public BrowserTab(Mediator mediator)
		{
			this.mediator = mediator;
			Init();			
		
		}
		public BrowserTab(Mediator mediator, object scriptObject)
		{
			this.mediator = mediator;			
			Init();			
			//this.browser.ScriptObject = scriptObject;
		}
		#endregion

		#region Methods
		protected override void Dispose(bool disposing)
		{
			//Marshal.Release(browser.Handle);

			//			browser.Dispose();
			//			browser.ContainingControl = null;

			base.Dispose (disposing);
		}


		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BrowserTab));
            this.browser = new Netron.Neon.NBrowser();
            this.SuspendLayout();
            // 
            // browser
            // 
            this.browser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.browser.Location = new System.Drawing.Point(0, 0);
            this.browser.Name = "browser";
            this.browser.ShowNavigation = true;
            this.browser.Size = new System.Drawing.Size(292, 266);
            this.browser.TabIndex = 0;
            this.browser.ToolbarColor = System.Drawing.Color.WhiteSmoke;
            // 
            // BrowserTab
            // 
            this.AccessibleDescription = "This browser panel allows you to browse the internet and depends on IE.";
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.browser);
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BrowserTab";
            this.TabText = "Browser";
            this.ResumeLayout(false);

		}

		
		private void Init()
		{
			InitializeComponent();
			//this allows to use this application from inside an HTML page
			browser.AxWebBrowser.ScriptObject = this;
			browser.AxWebBrowser.ScriptEnabled = true;
			browser.Show();
			//go by default to the Netron hoempage
			browser.Navigate("http://netron.sf.net");
			return;
	
		}

	
		#endregion

		

	
	}
}
