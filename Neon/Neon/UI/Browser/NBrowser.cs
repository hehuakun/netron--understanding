using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Drawing.Printing;

namespace Netron.Neon
{
	/// <summary>
	/// Browser user control
	/// Originally from IC#
	/// It contains a browser toolbar on top which can be disabled
	/// </summary>
	public class NBrowser : UserControl
	{

		public event DWebBrowserEvents2_NewWindow2EventHandler OnNewWindow;
		public event DWebBrowserEvents2_DocumentCompleteEventHandler OnDocumentComplete;

		#region Fields
		AxWebBrowser axWebBrowser = null;
		private bool showNavigation = true;		
		Panel   topPanel   = new Panel();
		ToolBar toolBar    = new ToolBar();
		TextBox urlTextBox = new TextBox();

		Color toolbarColor = Color.WhiteSmoke;
		bool   isHandleCreated  = false;
		string lastUrl     = null;
		string newWindowUrl;
		#endregion

		#region Properties
		public Color ToolbarColor
		{
			get{return toolbarColor;}
			set{toolbarColor = value;}
		}

		public AxWebBrowser AxWebBrowser 
		{
			get 
			{
				return axWebBrowser;
			}
		}
		public bool ShowNavigation
		{
			get{return showNavigation;}
			set{showNavigation = value;}
		}
		
		#endregion		
		
		#region Constructor
		public NBrowser()
		{
			
			Dock = DockStyle.Fill;
			Size = new Size(500, 500);
			
			if (showNavigation) 
			{
				#region Toolbar construction
				topPanel.Size = new Size(Width, 25);
				topPanel.Dock = DockStyle.Top;
				
				Controls.Add(topPanel);
				
				toolBar.Dock = DockStyle.None;
				
				
				for (int i = 0; i < 5; ++i) 
				{
					ToolBarButton toolBarButton = new ToolBarButton();
					toolBarButton.ImageIndex    = i;
					toolBar.Buttons.Add(toolBarButton);
				}

				

			
				
				toolBar.ImageList = new ImageList();
				toolBar.ImageList.ColorDepth = ColorDepth.Depth16Bit;
				toolBar.ImageList.Images.Add(GetImage("BackNavigationArrow.gif"));
				toolBar.ImageList.Images.Add(GetImage("ForwardNavigationArrow.gif"));
				toolBar.ImageList.Images.Add(GetImage("CancelNavigation.gif"));
				toolBar.ImageList.Images.Add(GetImage("RefreshNavigation.gif"));
				toolBar.ImageList.Images.Add(GetImage("HomeNavigation.gif"));


				
				toolBar.Appearance = ToolBarAppearance.Flat;
				toolBar.Divider = false;
				toolBar.ButtonClick += new ToolBarButtonClickEventHandler(ToolBarClick);
				
				toolBar.Location = new Point(0, 0);
				toolBar.Size = new Size(5*toolBar.ButtonSize.Width-50, 25);
				//toolBar.BorderStyle= BorderStyle.Fixed3D;
				topPanel.Controls.Add(toolBar);
				
				urlTextBox.Location  = new Point(5*toolBar.ButtonSize.Width+10, 2);
				urlTextBox.Size      = new Size(Width - (5*toolBar.ButtonSize.Width) - 20, 21);
				urlTextBox.Anchor    = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
				urlTextBox.KeyPress += new KeyPressEventHandler(KeyPressEvent);
				urlTextBox.BorderStyle = BorderStyle.FixedSingle;
				topPanel.Controls.Add(urlTextBox);
				topPanel.BackColor = toolbarColor;

				Label label = new Label();
				label.Text = "URL:";
				label.Location = new Point(5*toolBar.ButtonSize.Width - 30,6);
				label.Size = new Size(40,25);

				topPanel.Controls.Add(label);
				#endregion
			} 
			
			axWebBrowser = new AxWebBrowser();
			axWebBrowser.BeginInit();			
			axWebBrowser.Dock = DockStyle.Fill;			
			axWebBrowser.HandleCreated += new EventHandler(this.CreatedWebBrowserHandle);
			axWebBrowser.TitleChange   += new DWebBrowserEvents2_TitleChangeEventHandler(TitleChange);
			axWebBrowser.NewWindow2+=new DWebBrowserEvents2_NewWindow2EventHandler(axWebBrowser_NewWindow2);
			axWebBrowser.BeforeNavigate2+=new DWebBrowserEvents2_BeforeNavigate2EventHandler(axWebBrowser_BeforeNavigate2);
			axWebBrowser.DocumentComplete+=new DWebBrowserEvents2_DocumentCompleteEventHandler(axWebBrowser_DocumentComplete);
			Controls.Add(axWebBrowser);
			
			if (showNavigation) 
			{
				Controls.Add(topPanel);
			}
			
			axWebBrowser.EndInit();
		
		}


		#endregion

		#region Methods		
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing) 
			{
				axWebBrowser.Dispose();
			}
		}

		void TitleChange(object sender, DWebBrowserEvents2_TitleChangeEvent e)
		{
			urlTextBox.Text = axWebBrowser.LocationURL;
		}
		
		void KeyPressEvent(object sender, KeyPressEventArgs ex)
		{
			if (ex.KeyChar == '\r') 
			{
				Navigate(urlTextBox.Text);
			}
		}
		
		void ToolBarClick(object sender, ToolBarButtonClickEventArgs e)
		{
			try 
			{
				switch(toolBar.Buttons.IndexOf(e.Button)) 
				{
					case 0:
						axWebBrowser.GoBack();
						break;
					case 1:
						axWebBrowser.GoForward();
						break;
					case 2:
						axWebBrowser.Stop();
						break;
					case 3:
						axWebBrowser.CtlRefresh();
						break;
					case 4:
						axWebBrowser.GoHome();
						break;
				}
			} 
			catch (Exception) 
			{
			}
		}
		
		public void CreatedWebBrowserHandle(object sender, EventArgs evArgs) 
		{
			isHandleCreated = true;
			if (lastUrl != null) 
			{
				Navigate(lastUrl);
			}
		}
		
		public void Navigate(string name)
		{
			if (!isHandleCreated) 
			{
				lastUrl = name;
				return;
			}
			urlTextBox.Text = name;
			object arg1 = 0; 
			object arg2 = ""; 
			object arg3 = ""; 
			object arg4 = "";
			try 
			{
				axWebBrowser.Navigate(name, ref arg1, ref arg2, ref arg3, ref arg4);
			} 
			catch (Exception e) 
			{
				Console.WriteLine(e.ToString());
			}
		}
		public  Bitmap GetImage(string name)
		{
			Bitmap bmp=null;
			try
			{
				Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.Neon.Resources." + name);
					
				bmp= Bitmap.FromStream(stream) as Bitmap;
				stream.Close();
				stream=null;
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.Message);
			}
			return bmp;
		}
		#endregion

		private void axWebBrowser_NewWindow2(object sender, DWebBrowserEvents2_NewWindow2Event e)
		{
			//e.cancel = true;			
		
			if(OnNewWindow!=null)
				OnNewWindow(sender,e);
		}

		private void axWebBrowser_BeforeNavigate2(object sender, DWebBrowserEvents2_BeforeNavigate2Event e)
		{
			newWindowUrl = (string) e.uRL;
		}

		private void axWebBrowser_DocumentComplete(object sender, DWebBrowserEvents2_DocumentCompleteEvent e)
		{
			if(OnDocumentComplete!=null)
				OnDocumentComplete(sender,e);
		}
	}
}
