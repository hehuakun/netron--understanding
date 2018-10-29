using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Netron.Neon;
namespace WebBrowser
{
	/// <summary>
	/// Summary description for BrowserTab.
	/// </summary>
	public class BrowserTab : DockContent, IBrowserTab
	{
		public delegate void BrowserTabInfo(BrowserTab tab);
		public event BrowserTabInfo OnDocumentComplete;

		private Netron.Neon.NBrowser browser;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private string identifier;

		public AxWebBrowser AxWebBrowser
		{
			get{return this.browser.AxWebBrowser;}
		}
		public NBrowser Browser
		{
			get{return browser;}
		}

		public BrowserTab()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			this.Browser.OnDocumentComplete+=new DWebBrowserEvents2_DocumentCompleteEventHandler(Browser_OnDocumentComplete);
		}

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(BrowserTab));
			this.browser = new Netron.Neon.NBrowser();
			this.SuspendLayout();
			// 
			// browser
			// 
			this.browser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.browser.Location = new System.Drawing.Point(0, 0);
			this.browser.Name = "browser";
			this.browser.ShowNavigation = true;
			this.browser.Size = new System.Drawing.Size(648, 509);
			this.browser.TabIndex = 1;
			this.browser.ToolbarColor = System.Drawing.Color.WhiteSmoke;
			// 
			// BrowserTab
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(648, 509);
			this.Controls.Add(this.browser);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "BrowserTab";
			this.Text = "Browser";
			this.ResumeLayout(false);

		}
		#endregion

		public void Navigate(string url)
		{
			this.browser.Navigate(url);
		}
		

		public string Identifier
		{
			get
			{	
				return identifier;
			}
			set
			{
				identifier = value;
			}
		}

		public TabTypes TabType
		{
			get{return TabTypes.Browser;}
		}

		private void Browser_OnDocumentComplete(object sender, DWebBrowserEvents2_DocumentCompleteEvent e)
		{
			
			if(sender==null) return;
			AxWebBrowser browser =  sender as AxWebBrowser;
			if(browser==null) return;

			mshtml.IHTMLDocument2 document =  browser.Document as mshtml.IHTMLDocument2;
			string title = document.title;
			if(title==string.Empty)
			{				
				this.Text = "[Empty]";
			}
			else				
				this.Text = title;

			if(OnDocumentComplete!=null)
				OnDocumentComplete(this);
		}
	}
}
