using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Netron.Neon;
namespace WebServer
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel panel1;
		internal Netron.Neon.NOutput nOutput1;
		private Netron.Neon.NSplitter nuiSplitter1;
		private System.Windows.Forms.Panel panel2;
		private Netron.Neon.NBrowser nBrowser1;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.MenuItem menuItem9;
		private System.Windows.Forms.MenuItem menuItem10;
		private System.Windows.Forms.MenuItem menuItem11;
		private System.Windows.Forms.MenuItem menuItem12;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
	
		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

		
			this.nBrowser1.Navigate("http://localhost:8080/default.htm");
			this.nBrowser1.AxWebBrowser.DocumentComplete+=new Netron.Neon.DWebBrowserEvents2_DocumentCompleteEventHandler(AxWebBrowser_DocumentComplete);
		}
	


		/// <summary>
		/// This is a custom method called via JavaScript from the AxBrowser
		/// </summary>
		/// <param name="msg"></param>
		public void CustomMethod(string msg)
		{
			this.nOutput1.WriteLine(msg);
		}


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
			this.panel1 = new System.Windows.Forms.Panel();
			this.nOutput1 = new Netron.Neon.NOutput();
			this.nuiSplitter1 = new Netron.Neon.NSplitter();
			this.panel2 = new System.Windows.Forms.Panel();
			this.nBrowser1 = new Netron.Neon.NBrowser();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.menuItem8 = new System.Windows.Forms.MenuItem();
			this.menuItem9 = new System.Windows.Forms.MenuItem();
			this.menuItem10 = new System.Windows.Forms.MenuItem();
			this.menuItem11 = new System.Windows.Forms.MenuItem();
			this.menuItem12 = new System.Windows.Forms.MenuItem();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.nOutput1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel1.Location = new System.Drawing.Point(568, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(192, 414);
			this.panel1.TabIndex = 2;
			// 
			// nOutput1
			// 
			this.nOutput1.BackColor = System.Drawing.Color.MediumPurple;
			this.nOutput1.Current = "Default";
			this.nOutput1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.nOutput1.Image = null;
			this.nOutput1.Label = "";
			this.nOutput1.Location = new System.Drawing.Point(0, 0);
			this.nOutput1.Name = "nOutput1";
			this.nOutput1.Root = null;
			this.nOutput1.Size = new System.Drawing.Size(192, 414);
			this.nOutput1.TabIndex = 1;
			// 
			// nuiSplitter1
			// 
			this.nuiSplitter1.BackColor = System.Drawing.Color.WhiteSmoke;
			this.nuiSplitter1.ColapsingPanel = this.panel1;
			this.nuiSplitter1.Dock = System.Windows.Forms.DockStyle.Right;
			this.nuiSplitter1.Location = new System.Drawing.Point(560, 0);
			this.nuiSplitter1.MinSize = 2;
			this.nuiSplitter1.Name = "nuiSplitter1";
			this.nuiSplitter1.Size = new System.Drawing.Size(8, 414);
			this.nuiSplitter1.TabIndex = 3;
			this.nuiSplitter1.TabStop = false;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.nBrowser1);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(560, 414);
			this.panel2.TabIndex = 4;
			// 
			// nBrowser1
			// 
			this.nBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.nBrowser1.Location = new System.Drawing.Point(0, 0);
			this.nBrowser1.Name = "nBrowser1";
			this.nBrowser1.ShowNavigation = true;
			this.nBrowser1.Size = new System.Drawing.Size(560, 414);
			this.nBrowser1.TabIndex = 0;
			this.nBrowser1.ToolbarColor = System.Drawing.Color.WhiteSmoke;
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1,
																					  this.menuItem3});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem2});
			this.menuItem1.Text = "File";
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 0;
			this.menuItem2.Text = "Exit";
			this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 1;
			this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem4,
																					  this.menuItem5,
																					  this.menuItem6,
																					  this.menuItem7,
																					  this.menuItem8,
																					  this.menuItem9,
																					  this.menuItem10,
																					  this.menuItem11,
																					  this.menuItem12});
			this.menuItem3.Text = "Windows";
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 0;
			this.menuItem4.Text = "Home";
			this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 1;
			this.menuItem5.Text = "Container method";
			this.menuItem5.Click += new System.EventHandler(this.menuItem5_Click);
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 2;
			this.menuItem6.Text = "Embedded resources";
			this.menuItem6.Click += new System.EventHandler(this.menuItem6_Click);
			// 
			// menuItem7
			// 
			this.menuItem7.Index = 3;
			this.menuItem7.Text = "Form posting";
			this.menuItem7.Click += new System.EventHandler(this.menuItem7_Click);
			// 
			// menuItem8
			// 
			this.menuItem8.Index = 4;
			this.menuItem8.Text = "Image browser";
			this.menuItem8.Click += new System.EventHandler(this.menuItem8_Click);
			// 
			// menuItem9
			// 
			this.menuItem9.Index = 5;
			this.menuItem9.Text = "XML files";
			this.menuItem9.Click += new System.EventHandler(this.menuItem9_Click);
			// 
			// menuItem10
			// 
			this.menuItem10.Index = 6;
			this.menuItem10.Text = "My computer";
			this.menuItem10.Click += new System.EventHandler(this.menuItem10_Click);
			// 
			// menuItem11
			// 
			this.menuItem11.Index = 7;
			this.menuItem11.Text = "Servlets";
			this.menuItem11.Click += new System.EventHandler(this.menuItem11_Click);
			// 
			// menuItem12
			// 
			this.menuItem12.Index = 8;
			this.menuItem12.Text = "Static pages";
			this.menuItem12.Click += new System.EventHandler(this.menuItem12_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(760, 414);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.nuiSplitter1);
			this.Controls.Add(this.panel1);
			this.Menu = this.mainMenu1;
			this.Name = "Form1";
			this.Text = "Xeon example";
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void AxWebBrowser_DocumentComplete(object sender, Netron.Neon.DWebBrowserEvents2_DocumentCompleteEvent e)
		{
			if(sender==null) return;
			AxWebBrowser browser =  sender as AxWebBrowser;
			if(browser==null) return;
			browser.ScriptObject = this;
//			mshtml.IHTMLDocument2 document =  browser.Document as mshtml.IHTMLDocument2;
//			string title = document.title;
		}

		private void menuItem2_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void menuItem4_Click(object sender, System.EventArgs e)
		{
			this.nBrowser1.Navigate("http://localhost:8080/default.htm");
		}

		private void menuItem5_Click(object sender, System.EventArgs e)
		{
			this.nBrowser1.Navigate("http://localhost:8080/ContainerMethod.htm");
		}

		private void menuItem6_Click(object sender, System.EventArgs e)
		{
			this.nBrowser1.Navigate("http://localhost:8080/EmbeddedLocation.htm");
		}

		private void menuItem7_Click(object sender, System.EventArgs e)
		{
			this.nBrowser1.Navigate("http://localhost:8080/FormPosting.htm");
		}

		private void menuItem8_Click(object sender, System.EventArgs e)
		{
			this.nBrowser1.Navigate("http://localhost:8080/ImageBrowser.htm");
		}

		private void menuItem9_Click(object sender, System.EventArgs e)
		{
			this.nBrowser1.Navigate("http://localhost:8080/MSHelp.htm");
		}

		private void menuItem10_Click(object sender, System.EventArgs e)
		{
			this.nBrowser1.Navigate("http://localhost:8080/Mycomputer.htm");
		}

		private void menuItem11_Click(object sender, System.EventArgs e)
		{
			this.nBrowser1.Navigate("http://localhost:8080/Servlets.htm");
		}

		private void menuItem12_Click(object sender, System.EventArgs e)
		{
			this.nBrowser1.Navigate("http://localhost:8080/StaticPages.htm");
		}
	}
}
