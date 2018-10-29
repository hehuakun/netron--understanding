
using SHDocVw;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
namespace Netron.Neon
{
	/// <summary>
	/// Wrapper around the IE browser activeX COM with a lot of interop
	/// http://www.rendelmann.info/blog/
	/// http://www.codeproject.com/books/0764549146_8.asp?print=true
	/// http://www.icsharpcode.net/
	/// </summary>
	[System.Windows.Forms.AxHost.ClsidAttribute("{8856f961-340a-11d0-a96b-00c04fd705a2}")]
	[System.ComponentModel.DesignTimeVisibleAttribute(true)]
	[System.ComponentModel.DefaultProperty("Name")]	
	public class AxWebBrowser : System.Windows.Forms.AxHost
	{
		IWebBrowser2 ocx;
		AxWebBrowserEventMulticaster eventMulticaster;
		System.Windows.Forms.AxHost.ConnectionPointCookie cookie;

		#region Added from IEControl

		public delegate void BrowserNewWindowEventHandler(object sender, BrowserNewWindowEvent e);
		///<summary>
		///</summary>
		public event BrowserNewWindowEventHandler NewWindow;

		/// <summary>
		/// </summary>
		public class BrowserNewWindowEvent : CancelEventArgs 
		{
			string _url;
			public BrowserNewWindowEvent(string url, bool cancel)	: base(cancel)	{ this._url = url;	}
			public string url { get { return this._url;	} 	}
		} 

		internal void RaiseOnNewBrowserWindow(string URL, int Flags, string TargetFrameName, ref object PostData, string Headers, ref bool Processed) 
		{
			Processed = false;
			if (this.NewWindow != null) 
			{
				BrowserNewWindowEvent newwindowEvent = new BrowserNewWindowEvent(URL, Processed);
				this.NewWindow(this, newwindowEvent);
				Processed = newwindowEvent.Cancel;
			}
		}
		private SHDocVw.WebBrowser_V1 ocx_v1 = null;
		/// <summary>
		/// Set/Get HTML.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string Html 
		{
			get { return this.html;	}
			set 
			{ 
				this.html = value;	if (this.html == null) this.html = String.Empty; 
				this.body = String.Empty;
			}
		}

		private mshtml.IHTMLDocument2 document;

		protected void SetHtmlText(string text) 
		{

			if (text == null) 
				text = String.Empty;

			if (!IsHandleCreated) 
				return;

			if (ocx != null) 
			{
				
				document = ocx.Document as mshtml.IHTMLDocument2;
				if (document != null) 
				{

					if (activate) DoVerb(Interop.OLEIVERB_UIACTIVATE);
					// this way we can provide the FULL HTML incl. <head><style> etc.
					document.open("", null, null, null);
					object[] a = new object[]{text};
					document.write(a);
					document.close();
					//_document = document;
					// Without the "Refresh" command, the browser control doesn't
					// react to links containing the # character in the hyperlink.
					try 
					{
						if (text.IndexOf("#") > 0)
							document.execCommand("Refresh", false, null);
					} 
					catch {}
				}
			}
		}

		// called from SelfNavigateComplete()
		protected void SetBodyText(string text) 
		{
			
			if (text == null) 
				text = String.Empty;

			if (!IsHandleCreated) 
				return;

			if (ocx != null) 
			{
				document = ocx.Document as mshtml.IHTMLDocument2;
				if (document != null) 
				{
					//_document = document;
					mshtml.IHTMLElement body = document.body;
					if (body != null) 
					{
						if (activate) DoVerb(Interop.OLEIVERB_UIACTIVATE);
						body.innerHTML = text;
						return;
					}
				}
			}
		}
		/// <summary>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SelfNavigateComplete(object sender, DWebBrowserEvents2_NavigateComplete2Event e) 
		{
			if (activate) DoVerb(Interop.OLEIVERB_UIACTIVATE);

			if (this.html != String.Empty) 
			{
				this.SetHtmlText(html);
				RaiseOnDocumentComplete(this, new DWebBrowserEvents2_DocumentCompleteEvent(e.pDisp, e.uRL));
				this.html = String.Empty;
			}

			if (this.body != String.Empty) 
			{
				SetBodyText(body);
				RaiseOnDocumentComplete(this, new DWebBrowserEvents2_DocumentCompleteEvent(e.pDisp, e.uRL));
				this.body = String.Empty;
			}
		}

		///<summary>
		///Static ctor
		///</summary>
		static AxWebBrowser() 
		{
			try 
			{
				//_UseCurrentDll();
				lowSecurity = (Environment.OSVersion.Platform != PlatformID.Win32NT);
				return;
			}
			catch{}
			finally{}

			try 
			{
				//SHDocVwAssembly = Interop.GetAssemblyForTypeLib( "SHDocVw.DLL" );
				//AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler( RedirectSHDocAssembly );
			}
			catch{}
		}

		void SelfHandleCreated(object s, EventArgs e) 
		{

			HandleCreated -= new EventHandler(SelfHandleCreated);
			if (DesignMode)
				return;

			if (url == null)		url = String.Empty;
			if (html == null)	html = String.Empty;
			if (body == null)	body = String.Empty;

			if (body == String.Empty && html == String.Empty)
				url = "about:blank";

			// attach to the Browser.V1 new window event
			ocx_v1 = ocx as WebBrowser_V1;
			if (ocx_v1 != null) 
			{
				ocx_v1.NewWindow += new DWebBrowserEvents_NewWindowEventHandler(this.RaiseOnNewBrowserWindow);
			}

			if (!lowSecurity) 
			{
				// need to SetClientSite() to enable set properties in GetHostInfo() and setFlags():
				Interop.IOleObject oleObj = ocx as Interop.IOleObject;
				if (oleObj != null) 
				{
					uiHandler = new DocHostUIHandler(this);
					// next line causes problems on Win98 :(
					oleObj.SetClientSite(uiHandler);
				}
			}
		}

		// this can be called multiple times in the lifetime of the control!!!
		void SelfHandleDestroyed(object s, EventArgs e) 
		{
			if (ocx_v1 != null) 
			{
				ocx_v1.NewWindow -=  new DWebBrowserEvents_NewWindowEventHandler(this.RaiseOnNewBrowserWindow);
			}
			ocx_v1 = null;
			uiHandler = null;
		}

		/// <summary>
		/// </summary>
		public event BrowserTranslateUrlEventHandler TranslateUrl;

		/// <summary>
		/// </summary>
		/// <param name="e"></param>
		internal void RaiseOnTranslateUrl(DWebBrowserEvents2_TranslateUrlEventArgs e) 
		{
			if (TranslateUrl != null)
				TranslateUrl(this, e);
		}

		/// <summary>
		/// </summary>
		public event KeyEventHandler TranslateAccelerator;

		/// <summary>
		/// </summary>
		/// <param name="e"></param>
		internal void RaiseOnTranslateAccelerator(KeyEventArgs e) 
		{
			if (TranslateAccelerator!= null)
				TranslateAccelerator(this, e);
		}

		/// <summary>
		/// Set/Get a bool to cotnrol the visual style of the boder
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Border3d 
		{
			get {	return this.border3d;}
			set { this.border3d = value;}
		}
		/// <summary>
		/// Set/Get a bool to control the style of the scrollbars
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool FlatScrollBars 
		{
			get { return this.flatScrollBars;	}
			set { this.flatScrollBars = value;}
		}

		/// <summary>
		/// Set/Get a bool to allow in place navigation
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool AllowInPlaceNavigation 
		{
			get {	return this.allowInPlaceNavigation; }
			set {	this.allowInPlaceNavigation = value;	}
		}


		/// <summary>
		/// </summary>
		/// <param name="e"></param>
		internal void RaiseOnShowContextMenu(DWebBrowserEvents2_MenuCancelEventArgs e) 
		{
			if (ShowContextMenu != null)
				ShowContextMenu(this, e);
		}

		/// <summary>
		/// </summary>
		public event DWebBrowserEvents2_MenuCancelEventHandler ShowContextMenu;

		private DocHostUIHandler uiHandler;
		private object scriptObject;
		private static bool lowSecurity;

		string url = String.Empty;
		string html = String.Empty;
		string body = String.Empty;
		private bool 
			activate,
			allowInPlaceNavigation,
			border3d, 
			flatScrollBars,
			scriptEnabled,
			activeXEnabled,
			javaEnabled,
			bgSoundEnabled,
			imagesDownloadEnabled,
			videoEnabled,
			scrollBarsEnabled;

		/// <summary>
		/// Set/Get a bool to allow exec. of JavaScript
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool ScriptEnabled 
		{
			get {	return this.scriptEnabled;		}
			set { this.scriptEnabled = value;	}
		}

		/// <summary>
		/// Set/Get a bool to allow to excecute ActiveX controls
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool ActiveXEnabled 
		{
			get {	return this.activeXEnabled;		}
			set { this.activeXEnabled = value;	}
		}
		/// <summary>
		/// Set/Get a bool to allow Java applets
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool JavaEnabled 
		{
			get {	return this.javaEnabled;	}
			set { this.javaEnabled = value;	}
		}

		/// <summary>
		/// Set/Get a bool to control the playback of background sound
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool BackroundSoundEnabled 
		{
			get {	return this.bgSoundEnabled;		}
			set { this.bgSoundEnabled = value;	}
		}

		/// <summary>
		/// Set/Get a bool to control the display of any images
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool ImagesDownloadEnabled 
		{
			get {	return this.imagesDownloadEnabled;	}
			set { this.imagesDownloadEnabled = value;	}
		}

		/// <summary>
		/// Set/Get a bool to control the display of embedded video plugins
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool VideoEnabled 
		{
			get {	return this.videoEnabled;	}
			set { this.videoEnabled = value;	}
		}

		/// <summary>
		/// Set/Get the script object to be used from within JavaScript via 
		/// <c>window.getExternal</c> call
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object ScriptObject 
		{
			get { return this.scriptObject; }
			set { this.scriptObject = value;}
		}

		/// <summary>
		/// Set/Get a bool that control the scrollbar dislpay
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool ScrollBarsEnabled 
		{
			get {	return this.scrollBarsEnabled;	}
			set { this.scrollBarsEnabled = value;	}
		}

		/// <summary>
		/// Get a bool that informs about low security browser state.
		/// Win98/Me does not support all we need and causes exceptions.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public static bool LowSecurity 
		{
			get {	return lowSecurity;	}
		}

		/// <summary>
		/// Enables the parent to test, if there is already a listener attached to the
		/// BeforeNavigate event. Without that, the parent isn't able to test, because
		/// delegates are only allowed to have += and -= operators.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool AnyBeforeNavigateEventListener 
		{
			get { return (BeforeNavigate2 != null);}
		}

		#endregion

		
		public AxWebBrowser() : base("8856f961-340a-11d0-a96b-00c04fd705a2") 
		{
			this.scrollBarsEnabled = true;
			this.imagesDownloadEnabled = true;
			this.scriptEnabled = true;
			this.javaEnabled = true;
			this.border3d = false;
			this.flatScrollBars = true;

			HandleCreated += new EventHandler(SelfHandleCreated);
			HandleDestroyed += new EventHandler(SelfHandleDestroyed);
			NavigateComplete2+=new DWebBrowserEvents2_NavigateComplete2EventHandler(SelfNavigateComplete);
		}
		
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		[System.Runtime.InteropServices.DispIdAttribute(200)]
		public virtual object Application {
			get {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("Application", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.Application;
			}
		}
		
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		[System.Runtime.InteropServices.DispIdAttribute(201)]
		public virtual object CtlParent {
			get {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("CtlParent", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.Parent;
			}
		}
		
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		[System.Runtime.InteropServices.DispIdAttribute(202)]
		public virtual object CtlContainer {
			get {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("CtlContainer", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.Container;
			}
		}
		
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		[System.Runtime.InteropServices.DispIdAttribute(203)]
		public virtual object Document {
			get {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("Document", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.Document;
			}
		}
		
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		[System.Runtime.InteropServices.DispIdAttribute(204)]
		public virtual bool TopLevelContainer {
			get {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("TopLevelContainer", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.TopLevelContainer;
			}
		}
		
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		[System.Runtime.InteropServices.DispIdAttribute(205)]
		public virtual string Type {
			get {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("Type", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.Type;
			}
		}
		
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		[System.Runtime.InteropServices.DispIdAttribute(206)]
		public virtual int CtlLeft {
			get {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("CtlLeft", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.Left;
			}
			set {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("CtlLeft", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertySet);
				}
				this.ocx.Left = value;
			}
		}
		
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		[System.Runtime.InteropServices.DispIdAttribute(207)]
		public virtual int CtlTop {
			get {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("CtlTop", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.Top;
			}
			set {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("CtlTop", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertySet);
				}
				this.ocx.Top = value;
			}
		}
		
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		[System.Runtime.InteropServices.DispIdAttribute(208)]
		public virtual int CtlWidth {
			get {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("CtlWidth", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.Width;
			}
			set {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("CtlWidth", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertySet);
				}
				this.ocx.Width = value;
			}
		}
		
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		[System.Runtime.InteropServices.DispIdAttribute(209)]
		public virtual int CtlHeight {
			get {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("CtlHeight", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.Height;
			}
			set {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("CtlHeight", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertySet);
				}
				this.ocx.Height = value;
			}
		}
		
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		[System.Runtime.InteropServices.DispIdAttribute(210)]
		public virtual string LocationName {
			get {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("LocationName", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.LocationName;
			}
		}
		
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		[System.Runtime.InteropServices.DispIdAttribute(211)]
		public virtual string LocationURL {
			get {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("LocationURL", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.LocationURL;
			}
		}
		
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		[System.Runtime.InteropServices.DispIdAttribute(212)]
		public virtual bool Busy {
			get {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("Busy", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.Busy;
			}
		}
		
		[System.ComponentModel.Browsable(true)]
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		[System.Runtime.InteropServices.DispIdAttribute(0)]
		public new virtual string Name {
			get {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("Name", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.Name;
			}
		}
		
		[System.ComponentModel.Browsable(false)]
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		[System.Runtime.InteropServices.DispIdAttribute(-515)]
		[System.Runtime.InteropServices.ComAliasNameAttribute("System.Int32")]
		public virtual int HWND {
			get {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("HWND", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return (this.ocx.HWND);
			}
		}
		
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		[System.Runtime.InteropServices.DispIdAttribute(400)]
		public virtual string FullName {
			get {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("FullName", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.FullName;
			}
		}
		
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		[System.Runtime.InteropServices.DispIdAttribute(401)]
		public virtual string Path {
			get {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("Path", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.Path;
			}
		}
		
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		[System.Runtime.InteropServices.DispIdAttribute(402)]
		public virtual bool CtlVisible {
			get {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("CtlVisible", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.Visible;
			}
			set {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("CtlVisible", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertySet);
				}
				this.ocx.Visible = value;
			}
		}
		
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		[System.Runtime.InteropServices.DispIdAttribute(403)]
		public virtual bool StatusBar {
			get {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("StatusBar", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.StatusBar;
			}
			set {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("StatusBar", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertySet);
				}
				this.ocx.StatusBar = value;
			}
		}
		
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		[System.Runtime.InteropServices.DispIdAttribute(404)]
		public virtual string StatusText {
			get {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("StatusText", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.StatusText;
			}
			set {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("StatusText", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertySet);
				}
				this.ocx.StatusText = value;
			}
		}
		
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		[System.Runtime.InteropServices.DispIdAttribute(405)]
		public virtual int ToolBar {
			get {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("ToolBar", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.ToolBar;
			}
			set {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("ToolBar", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertySet);
				}
				this.ocx.ToolBar = value;
			}
		}
		
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		[System.Runtime.InteropServices.DispIdAttribute(406)]
		public virtual bool MenuBar {
			get {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("MenuBar", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.MenuBar;
			}
			set {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("MenuBar", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertySet);
				}
				this.ocx.MenuBar = value;
			}
		}
		
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		[System.Runtime.InteropServices.DispIdAttribute(407)]
		public virtual bool FullScreen {
			get {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("FullScreen", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.FullScreen;
			}
			set {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("FullScreen", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertySet);
				}
				this.ocx.FullScreen = value;
			}
		}
		
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		[System.Runtime.InteropServices.DispIdAttribute(-525)]
		[System.ComponentModel.Bindable(System.ComponentModel.BindableSupport.Yes)]
		public virtual tagREADYSTATE ReadyState {
			get {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("ReadyState", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.ReadyState;
			}
		}
		
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		[System.Runtime.InteropServices.DispIdAttribute(550)]
		public virtual bool Offline {
			get {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("Offline", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.Offline;
			}
			set {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("Offline", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertySet);
				}
				this.ocx.Offline = value;
			}
		}
		
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		[System.Runtime.InteropServices.DispIdAttribute(551)]
		public virtual bool Silent {
			get {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("Silent", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.Silent;
			}
			set {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("Silent", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertySet);
				}
				this.ocx.Silent = value;
			}
		}
		
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		[System.Runtime.InteropServices.DispIdAttribute(552)]
		public virtual bool RegisterAsBrowser {
			get {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("RegisterAsBrowser", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.RegisterAsBrowser;
			}
			set {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("RegisterAsBrowser", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertySet);
				}
				this.ocx.RegisterAsBrowser = value;
			}
		}
		
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		[System.Runtime.InteropServices.DispIdAttribute(553)]
		public virtual bool RegisterAsDropTarget {
			get {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("RegisterAsDropTarget", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.RegisterAsDropTarget;
			}
			set {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("RegisterAsDropTarget", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertySet);
				}
				this.ocx.RegisterAsDropTarget = value;
			}
		}
		
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		[System.Runtime.InteropServices.DispIdAttribute(554)]
		public virtual bool TheaterMode {
			get {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("TheaterMode", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.TheaterMode;
			}
			set {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("TheaterMode", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertySet);
				}
				this.ocx.TheaterMode = value;
			}
		}
		
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		[System.Runtime.InteropServices.DispIdAttribute(555)]
		public virtual bool AddressBar {
			get {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("AddressBar", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.AddressBar;
			}
			set {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("AddressBar", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertySet);
				}
				this.ocx.AddressBar = value;
			}
		}
		
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		[System.Runtime.InteropServices.DispIdAttribute(556)]
		public virtual bool Resizable {
			get {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("Resizable", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertyGet);
				}
				return this.ocx.Resizable;
			}
			set {
				if ((this.ocx == null)) {
					throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("Resizable", System.Windows.Forms.AxHost.ActiveXInvokeKind.PropertySet);
				}
				this.ocx.Resizable = value;
			}
		}
		
		public event DWebBrowserEvents2_PrivacyImpactedStateChangeEventHandler PrivacyImpactedStateChange;
		
		public event DWebBrowserEvents2_UpdatePageStatusEventHandler UpdatePageStatus;
		
		public event DWebBrowserEvents2_PrintTemplateTeardownEventHandler PrintTemplateTeardown;
		
		public event DWebBrowserEvents2_PrintTemplateInstantiationEventHandler PrintTemplateInstantiation;
		
		public event DWebBrowserEvents2_NavigateErrorEventHandler NavigateError;
		
		public event DWebBrowserEvents2_FileDownloadEventHandler FileDownload;
		
		public event DWebBrowserEvents2_SetSecureLockIconEventHandler SetSecureLockIcon;
		
		public event DWebBrowserEvents2_ClientToHostWindowEventHandler ClientToHostWindow;
		
		public event DWebBrowserEvents2_WindowClosingEventHandler WindowClosing;
		
		public event DWebBrowserEvents2_WindowSetHeightEventHandler WindowSetHeight;
		
		public event DWebBrowserEvents2_WindowSetWidthEventHandler WindowSetWidth;
		
		public event DWebBrowserEvents2_WindowSetTopEventHandler WindowSetTop;
		
		public event DWebBrowserEvents2_WindowSetLeftEventHandler WindowSetLeft;
		
		public event DWebBrowserEvents2_WindowSetResizableEventHandler WindowSetResizable;
		
		public event DWebBrowserEvents2_OnTheaterModeEventHandler OnTheaterMode;
		
		public event DWebBrowserEvents2_OnFullScreenEventHandler OnFullScreen;
		
		public event DWebBrowserEvents2_OnStatusBarEventHandler OnStatusBar;
		
		public event DWebBrowserEvents2_OnMenuBarEventHandler OnMenuBar;
		
		public event DWebBrowserEvents2_OnToolBarEventHandler OnToolBar;
		
		public event DWebBrowserEvents2_OnVisibleEventHandler OnVisible;
		
		public event System.EventHandler OnQuit;
		
		public event DWebBrowserEvents2_DocumentCompleteEventHandler DocumentComplete;
		
		public event DWebBrowserEvents2_NavigateComplete2EventHandler NavigateComplete2;
		
		public event DWebBrowserEvents2_NewWindow2EventHandler NewWindow2;

		public event DWebBrowserEvents2_NewWindow3EventHandler NewWindow3;
		
		public event DWebBrowserEvents2_BeforeNavigate2EventHandler BeforeNavigate2;
		
		public event DWebBrowserEvents2_PropertyChangeEventHandler PropertyChange;
		
		public event DWebBrowserEvents2_TitleChangeEventHandler TitleChange;
		
		public event System.EventHandler DownloadComplete;
		
		public event System.EventHandler DownloadBegin;
		
		public event DWebBrowserEvents2_CommandStateChangeEventHandler CommandStateChange;
		
		public event DWebBrowserEvents2_ProgressChangeEventHandler ProgressChange;
		
		public event DWebBrowserEvents2_StatusTextChangeEventHandler StatusTextChange;
		
		public virtual void ShowBrowserBar(ref object pvaClsid, [System.Runtime.InteropServices.Optional()] ref object pvarShow, [System.Runtime.InteropServices.Optional()] ref object pvarSize) {
			if ((this.ocx == null)) {
				throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("ShowBrowserBar", System.Windows.Forms.AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.ShowBrowserBar(ref pvaClsid, ref pvarShow, ref pvarSize);
		}
		
		public virtual void ExecWB(OLECMDID cmdID, OLECMDEXECOPT cmdexecopt, [System.Runtime.InteropServices.Optional()] ref object pvaIn, [System.Runtime.InteropServices.Optional()] ref object pvaOut) {
			if ((this.ocx == null)) {
				throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("ExecWB", System.Windows.Forms.AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.ExecWB(cmdID, cmdexecopt, ref pvaIn, ref pvaOut);
		}
		
		public virtual OLECMDF QueryStatusWB(OLECMDID cmdID) {
			if ((this.ocx == null)) {
				throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("QueryStatusWB", System.Windows.Forms.AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.QueryStatusWB(cmdID);
		}
		
		public virtual void Navigate2(ref object uRL, [System.Runtime.InteropServices.Optional()] ref object flags, [System.Runtime.InteropServices.Optional()] ref object targetFrameName, [System.Runtime.InteropServices.Optional()] ref object postData, [System.Runtime.InteropServices.Optional()] ref object headers) {
			if ((this.ocx == null)) {
				throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("Navigate2", System.Windows.Forms.AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Navigate2(ref uRL, ref flags, ref targetFrameName, ref postData, ref headers);
		}
		
		public virtual object GetProperty(string property) {
			if ((this.ocx == null)) {
				throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("GetProperty", System.Windows.Forms.AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			return this.ocx.GetProperty(property);
		}
		
		public virtual void PutProperty(string property, object vtValue) {
			if ((this.ocx == null)) {
				throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("PutProperty", System.Windows.Forms.AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.PutProperty(property, vtValue);
		}
		
		public virtual void ClientToWindow(ref int pcx, ref int pcy) {
			if ((this.ocx == null)) {
				throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("ClientToWindow", System.Windows.Forms.AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.ClientToWindow(ref pcx, ref pcy);
		}
		
		public virtual void Quit() {
			if ((this.ocx == null)) {
				throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("Quit", System.Windows.Forms.AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Quit();
		}
		
		public virtual void Stop() {
			if ((this.ocx == null)) {
				throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("Stop", System.Windows.Forms.AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Stop();
		}
		
		public virtual void Refresh2([System.Runtime.InteropServices.Optional()] ref object level) {
			if ((this.ocx == null)) {
				throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("Refresh2", System.Windows.Forms.AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Refresh2(ref level);
		}
		
		public virtual void CtlRefresh() {
			if ((this.ocx == null)) {
				throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("CtlRefresh", System.Windows.Forms.AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Refresh();
		}
		
		public virtual void Navigate(string uRL, [System.Runtime.InteropServices.Optional()] ref object flags, [System.Runtime.InteropServices.Optional()] ref object targetFrameName, [System.Runtime.InteropServices.Optional()] ref object postData, [System.Runtime.InteropServices.Optional()] ref object headers) {
			if ((this.ocx == null)) {
				throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("Navigate", System.Windows.Forms.AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.Navigate(uRL, ref flags, ref targetFrameName, ref postData, ref headers);
		}
		
		public virtual void GoSearch() {
			if ((this.ocx == null)) {
				throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("GoSearch", System.Windows.Forms.AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.GoSearch();
		}
		
		public virtual void GoHome() {
			if ((this.ocx == null)) {
				throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("GoHome", System.Windows.Forms.AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.GoHome();
		}
		
		public virtual void GoForward() {
			if ((this.ocx == null)) {
				throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("GoForward", System.Windows.Forms.AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.GoForward();
		}
		
		public virtual void GoBack() {
			if ((this.ocx == null)) {
				throw new System.Windows.Forms.AxHost.InvalidActiveXStateException("GoBack", System.Windows.Forms.AxHost.ActiveXInvokeKind.MethodInvoke);
			}
			this.ocx.GoBack();
		}
		
		protected override void CreateSink() {
			try {
				this.eventMulticaster = new AxWebBrowserEventMulticaster(this);
				this.cookie = new System.Windows.Forms.AxHost.ConnectionPointCookie(this.ocx, this.eventMulticaster, typeof(DWebBrowserEvents2));
			}
			catch (System.Exception ) {
			}
		}
		
		protected override void DetachSink() {
			try {
				this.cookie.Disconnect();
			}
			catch (System.Exception ) {
			}
		}
		
		protected override void AttachInterfaces() {
			try {
				this.ocx = ((IWebBrowser2)(this.GetOcx())); //gets the COM interface
			}
			catch (System.Exception ) {
			}
		}
		
		internal void RaiseOnPrivacyImpactedStateChange(object sender, DWebBrowserEvents2_PrivacyImpactedStateChangeEvent e) {
			if ((this.PrivacyImpactedStateChange != null)) {
				this.PrivacyImpactedStateChange(sender, e);
			}
		}
		
		internal void RaiseOnUpdatePageStatus(object sender, DWebBrowserEvents2_UpdatePageStatusEvent e) {
			if ((this.UpdatePageStatus != null)) {
				this.UpdatePageStatus(sender, e);
			}
		}
		
		internal void RaiseOnPrintTemplateTeardown(object sender, DWebBrowserEvents2_PrintTemplateTeardownEvent e) {
			if ((this.PrintTemplateTeardown != null)) {
				this.PrintTemplateTeardown(sender, e);
			}
		}
		
		internal void RaiseOnPrintTemplateInstantiation(object sender, DWebBrowserEvents2_PrintTemplateInstantiationEvent e) {
			if ((this.PrintTemplateInstantiation != null)) {
				this.PrintTemplateInstantiation(sender, e);
			}
		}
		
		internal void RaiseOnNavigateError(object sender, DWebBrowserEvents2_NavigateErrorEvent e) {
			if ((this.NavigateError != null)) {
				this.NavigateError(sender, e);
			}
		}
		
		internal void RaiseOnFileDownload(object sender, DWebBrowserEvents2_FileDownloadEvent e) {
			if ((this.FileDownload != null)) {
				this.FileDownload(sender, e);
			}
		}
		
		internal void RaiseOnSetSecureLockIcon(object sender, DWebBrowserEvents2_SetSecureLockIconEvent e) {
			if ((this.SetSecureLockIcon != null)) {
				this.SetSecureLockIcon(sender, e);
			}
		}
		
		internal void RaiseOnClientToHostWindow(object sender, DWebBrowserEvents2_ClientToHostWindowEvent e) {
			if ((this.ClientToHostWindow != null)) {
				this.ClientToHostWindow(sender, e);
			}
		}
		
		internal void RaiseOnWindowClosing(object sender, DWebBrowserEvents2_WindowClosingEvent e) {
			if ((this.WindowClosing != null)) {
				this.WindowClosing(sender, e);
			}
		}
		
		internal void RaiseOnWindowSetHeight(object sender, DWebBrowserEvents2_WindowSetHeightEvent e) {
			if ((this.WindowSetHeight != null)) {
				this.WindowSetHeight(sender, e);
			}
		}
		
		internal void RaiseOnWindowSetWidth(object sender, DWebBrowserEvents2_WindowSetWidthEvent e) {
			if ((this.WindowSetWidth != null)) {
				this.WindowSetWidth(sender, e);
			}
		}
		
		internal void RaiseOnWindowSetTop(object sender, DWebBrowserEvents2_WindowSetTopEvent e) {
			if ((this.WindowSetTop != null)) {
				this.WindowSetTop(sender, e);
			}
		}
		
		internal void RaiseOnWindowSetLeft(object sender, DWebBrowserEvents2_WindowSetLeftEvent e) {
			if ((this.WindowSetLeft != null)) {
				this.WindowSetLeft(sender, e);
			}
		}
		
		internal void RaiseOnWindowSetResizable(object sender, DWebBrowserEvents2_WindowSetResizableEvent e) {
			if ((this.WindowSetResizable != null)) {
				this.WindowSetResizable(sender, e);
			}
		}
		
		internal void RaiseOnOnTheaterMode(object sender, DWebBrowserEvents2_OnTheaterModeEvent e) {
			if ((this.OnTheaterMode != null)) {
				this.OnTheaterMode(sender, e);
			}
		}
		
		internal void RaiseOnOnFullScreen(object sender, DWebBrowserEvents2_OnFullScreenEvent e) {
			if ((this.OnFullScreen != null)) {
				this.OnFullScreen(sender, e);
			}
		}
		
		internal void RaiseOnOnStatusBar(object sender, DWebBrowserEvents2_OnStatusBarEvent e) {
			if ((this.OnStatusBar != null)) {
				this.OnStatusBar(sender, e);
			}
		}
		
		internal void RaiseOnOnMenuBar(object sender, DWebBrowserEvents2_OnMenuBarEvent e) {
			if ((this.OnMenuBar != null)) {
				this.OnMenuBar(sender, e);
			}
		}
		
		internal void RaiseOnOnToolBar(object sender, DWebBrowserEvents2_OnToolBarEvent e) {
			if ((this.OnToolBar != null)) {
				this.OnToolBar(sender, e);
			}
		}
		
		internal void RaiseOnOnVisible(object sender, DWebBrowserEvents2_OnVisibleEvent e) {
			if ((this.OnVisible != null)) {
				this.OnVisible(sender, e);
			}
		}
		
		internal void RaiseOnOnQuit(object sender, System.EventArgs e) {
			if ((this.OnQuit != null)) {
				this.OnQuit(sender, e);
			}
		}
		
		internal void RaiseOnDocumentComplete(object sender, DWebBrowserEvents2_DocumentCompleteEvent e) {
			if ((this.DocumentComplete != null)) {
				this.DocumentComplete(sender, e);
			}
		}
		
		internal void RaiseOnNavigateComplete2(object sender, DWebBrowserEvents2_NavigateComplete2Event e) {
			if ((this.NavigateComplete2 != null)) {
				this.NavigateComplete2(sender, e);
			}
		}
		
		internal void RaiseOnNewWindow2(object sender, DWebBrowserEvents2_NewWindow2Event e) {
			if ((this.NewWindow2 != null)) {
				this.NewWindow2(sender, e);
			}
		}
		
		internal void RaiseOnNewWindow3(object sender, DWebBrowserEvents2_NewWindow3Event e) 
		{
			if ((this.NewWindow3 != null)) 
			{
				this.NewWindow3(sender, e);
			}
		}
		
		internal void RaiseOnBeforeNavigate2(object sender, DWebBrowserEvents2_BeforeNavigate2Event e) {
			if ((this.BeforeNavigate2 != null)) {
				this.BeforeNavigate2(sender, e);
			}
		}
		
		internal void RaiseOnPropertyChange(object sender, DWebBrowserEvents2_PropertyChangeEvent e) {
			if ((this.PropertyChange != null)) {
				this.PropertyChange(sender, e);
			}
		}
		
		internal void RaiseOnTitleChange(object sender, DWebBrowserEvents2_TitleChangeEvent e) {
			if ((this.TitleChange != null)) {
				this.TitleChange(sender, e);
			}
		}
		
		internal void RaiseOnDownloadComplete(object sender, System.EventArgs e) {
			if ((this.DownloadComplete != null)) {
				this.DownloadComplete(sender, e);
			}
		}
		
		internal void RaiseOnDownloadBegin(object sender, System.EventArgs e) {
			if ((this.DownloadBegin != null)) {
				this.DownloadBegin(sender, e);
			}
		}
		
		internal void RaiseOnCommandStateChange(object sender, DWebBrowserEvents2_CommandStateChangeEvent e) {
			if ((this.CommandStateChange != null)) {
				this.CommandStateChange(sender, e);
			}
		}
		
		internal void RaiseOnProgressChange(object sender, DWebBrowserEvents2_ProgressChangeEvent e) {
			if ((this.ProgressChange != null)) {
				this.ProgressChange(sender, e);
			}
		}
		
		internal void RaiseOnStatusTextChange(object sender, DWebBrowserEvents2_StatusTextChangeEvent e) {
			if ((this.StatusTextChange != null)) {
				this.StatusTextChange(sender, e);
			}
		}
	}

	/// <summary>
	/// </summary>
	public delegate void DWebBrowserEvents2_MenuCancelEventHandler(object sender, DWebBrowserEvents2_MenuCancelEventArgs e);

	/// <summary>
	/// </summary>
	public class DWebBrowserEvents2_MenuCancelEventArgs : CancelEventArgs 
	{
		Point  location;
		public DWebBrowserEvents2_MenuCancelEventArgs(Point loaction, bool cancel)	: base(cancel)	{ this.location = location;	}
		public Point Location { get { return this.location;	} 	}
	} 

	/// <summary>
	/// </summary>
	public class DWebBrowserEvents2_TranslateUrlEventArgs: EventArgs 
	{
		string url;
		string translatedUrl;
		public string Url { get { return this.url;	} 	}
		public string TranslatedUrl { get { return this.translatedUrl; } set { this.translatedUrl = value; } 	}
		public DWebBrowserEvents2_TranslateUrlEventArgs(string url)	{	this.url = this.translatedUrl = url; /* assume to be the same */	}
	}

	/// <summary>
	/// </summary>
	public delegate void BrowserTranslateUrlEventHandler(object sender, DWebBrowserEvents2_TranslateUrlEventArgs e);

	
	public delegate void DWebBrowserEvents2_PrivacyImpactedStateChangeEventHandler(object sender, DWebBrowserEvents2_PrivacyImpactedStateChangeEvent e);
	
	public class DWebBrowserEvents2_PrivacyImpactedStateChangeEvent {
		
		public bool bImpacted;
		
		public DWebBrowserEvents2_PrivacyImpactedStateChangeEvent(bool bImpacted) {
			this.bImpacted = bImpacted;
		}
	}
	
	public delegate void DWebBrowserEvents2_UpdatePageStatusEventHandler(object sender, DWebBrowserEvents2_UpdatePageStatusEvent e);
	
	public class DWebBrowserEvents2_UpdatePageStatusEvent {
		
		public object pDisp;
		
		public object nPage;
		
		public object fDone;
		
		public DWebBrowserEvents2_UpdatePageStatusEvent(object pDisp, object nPage, object fDone) {
			this.pDisp = pDisp;
			this.nPage = nPage;
			this.fDone = fDone;
		}
	}
	
	public delegate void DWebBrowserEvents2_PrintTemplateTeardownEventHandler(object sender, DWebBrowserEvents2_PrintTemplateTeardownEvent e);
	
	public class DWebBrowserEvents2_PrintTemplateTeardownEvent {
		
		public object pDisp;
		
		public DWebBrowserEvents2_PrintTemplateTeardownEvent(object pDisp) {
			this.pDisp = pDisp;
		}
	}
	
	public delegate void DWebBrowserEvents2_PrintTemplateInstantiationEventHandler(object sender, DWebBrowserEvents2_PrintTemplateInstantiationEvent e);
	
	public class DWebBrowserEvents2_PrintTemplateInstantiationEvent {
		
		public object pDisp;
		
		public DWebBrowserEvents2_PrintTemplateInstantiationEvent(object pDisp) {
			this.pDisp = pDisp;
		}
	}
	
	public delegate void DWebBrowserEvents2_NavigateErrorEventHandler(object sender, DWebBrowserEvents2_NavigateErrorEvent e);
	
	public class DWebBrowserEvents2_NavigateErrorEvent {
		
		public object pDisp;
		
		public object uRL;
		
		public object frame;
		
		public object statusCode;
		
		public bool cancel;
		
		public DWebBrowserEvents2_NavigateErrorEvent(object pDisp, object uRL, object frame, object statusCode, bool cancel) {
			this.pDisp = pDisp;
			this.uRL = uRL;
			this.frame = frame;
			this.statusCode = statusCode;
			this.cancel = cancel;
		}
	}
	
	public delegate void DWebBrowserEvents2_FileDownloadEventHandler(object sender, DWebBrowserEvents2_FileDownloadEvent e);
	
	public class DWebBrowserEvents2_FileDownloadEvent {
		
		public bool cancel;
		
		public DWebBrowserEvents2_FileDownloadEvent(bool cancel) {
			this.cancel = cancel;
		}
	}
	
	public delegate void DWebBrowserEvents2_SetSecureLockIconEventHandler(object sender, DWebBrowserEvents2_SetSecureLockIconEvent e);
	
	public class DWebBrowserEvents2_SetSecureLockIconEvent {
		
		public int secureLockIcon;
		
		public DWebBrowserEvents2_SetSecureLockIconEvent(int secureLockIcon) {
			this.secureLockIcon = secureLockIcon;
		}
	}
	
	public delegate void DWebBrowserEvents2_ClientToHostWindowEventHandler(object sender, DWebBrowserEvents2_ClientToHostWindowEvent e);
	
	public class DWebBrowserEvents2_ClientToHostWindowEvent {
		
		public int cX;
		
		public int cY;
		
		public DWebBrowserEvents2_ClientToHostWindowEvent(int cX, int cY) {
			this.cX = cX;
			this.cY = cY;
		}
	}
	
	public delegate void DWebBrowserEvents2_WindowClosingEventHandler(object sender, DWebBrowserEvents2_WindowClosingEvent e);
	
	public class DWebBrowserEvents2_WindowClosingEvent {
		
		public bool isChildWindow;
		
		public bool cancel;
		
		public DWebBrowserEvents2_WindowClosingEvent(bool isChildWindow, bool cancel) {
			this.isChildWindow = isChildWindow;
			this.cancel = cancel;
		}
	}
	
	public delegate void DWebBrowserEvents2_WindowSetHeightEventHandler(object sender, DWebBrowserEvents2_WindowSetHeightEvent e);
	
	public class DWebBrowserEvents2_WindowSetHeightEvent {
		
		public int height;
		
		public DWebBrowserEvents2_WindowSetHeightEvent(int height) {
			this.height = height;
		}
	}
	
	public delegate void DWebBrowserEvents2_WindowSetWidthEventHandler(object sender, DWebBrowserEvents2_WindowSetWidthEvent e);
	
	public class DWebBrowserEvents2_WindowSetWidthEvent {
		
		public int width;
		
		public DWebBrowserEvents2_WindowSetWidthEvent(int width) {
			this.width = width;
		}
	}
	
	public delegate void DWebBrowserEvents2_WindowSetTopEventHandler(object sender, DWebBrowserEvents2_WindowSetTopEvent e);
	
	public class DWebBrowserEvents2_WindowSetTopEvent {
		
		public int top;
		
		public DWebBrowserEvents2_WindowSetTopEvent(int top) {
			this.top = top;
		}
	}
	
	public delegate void DWebBrowserEvents2_WindowSetLeftEventHandler(object sender, DWebBrowserEvents2_WindowSetLeftEvent e);
	
	public class DWebBrowserEvents2_WindowSetLeftEvent {
		
		public int left;
		
		public DWebBrowserEvents2_WindowSetLeftEvent(int left) {
			this.left = left;
		}
	}
	
	public delegate void DWebBrowserEvents2_WindowSetResizableEventHandler(object sender, DWebBrowserEvents2_WindowSetResizableEvent e);
	
	public class DWebBrowserEvents2_WindowSetResizableEvent {
		
		public bool resizable;
		
		public DWebBrowserEvents2_WindowSetResizableEvent(bool resizable) {
			this.resizable = resizable;
		}
	}
	
	public delegate void DWebBrowserEvents2_OnTheaterModeEventHandler(object sender, DWebBrowserEvents2_OnTheaterModeEvent e);
	
	public class DWebBrowserEvents2_OnTheaterModeEvent {
		
		public bool theaterMode;
		
		public DWebBrowserEvents2_OnTheaterModeEvent(bool theaterMode) {
			this.theaterMode = theaterMode;
		}
	}
	
	public delegate void DWebBrowserEvents2_OnFullScreenEventHandler(object sender, DWebBrowserEvents2_OnFullScreenEvent e);
	
	public class DWebBrowserEvents2_OnFullScreenEvent {
		
		public bool fullScreen;
		
		public DWebBrowserEvents2_OnFullScreenEvent(bool fullScreen) {
			this.fullScreen = fullScreen;
		}
	}
	
	public delegate void DWebBrowserEvents2_OnStatusBarEventHandler(object sender, DWebBrowserEvents2_OnStatusBarEvent e);
	
	public class DWebBrowserEvents2_OnStatusBarEvent {
		
		public bool statusBar;
		
		public DWebBrowserEvents2_OnStatusBarEvent(bool statusBar) {
			this.statusBar = statusBar;
		}
	}
	
	public delegate void DWebBrowserEvents2_OnMenuBarEventHandler(object sender, DWebBrowserEvents2_OnMenuBarEvent e);
	
	public class DWebBrowserEvents2_OnMenuBarEvent {
		
		public bool menuBar;
		
		public DWebBrowserEvents2_OnMenuBarEvent(bool menuBar) {
			this.menuBar = menuBar;
		}
	}
	
	public delegate void DWebBrowserEvents2_OnToolBarEventHandler(object sender, DWebBrowserEvents2_OnToolBarEvent e);
	
	public class DWebBrowserEvents2_OnToolBarEvent {
		
		public bool toolBar;
		
		public DWebBrowserEvents2_OnToolBarEvent(bool toolBar) {
			this.toolBar = toolBar;
		}
	}
	
	public delegate void DWebBrowserEvents2_OnVisibleEventHandler(object sender, DWebBrowserEvents2_OnVisibleEvent e);
	
	public class DWebBrowserEvents2_OnVisibleEvent {
		
		public bool visible;
		
		public DWebBrowserEvents2_OnVisibleEvent(bool visible) {
			this.visible = visible;
		}
	}
	
	public delegate void DWebBrowserEvents2_DocumentCompleteEventHandler(object sender, DWebBrowserEvents2_DocumentCompleteEvent e);
	
	public class DWebBrowserEvents2_DocumentCompleteEvent {
		
		public object pDisp;
		
		public object uRL;
		
		public DWebBrowserEvents2_DocumentCompleteEvent(object pDisp, object uRL) {
			this.pDisp = pDisp;
			this.uRL = uRL;
		}
	}
	
	public delegate void DWebBrowserEvents2_NavigateComplete2EventHandler(object sender, DWebBrowserEvents2_NavigateComplete2Event e);
	
	public class DWebBrowserEvents2_NavigateComplete2Event {
		
		public object pDisp;
		
		public object uRL;
		
		public DWebBrowserEvents2_NavigateComplete2Event(object pDisp, object uRL) {
			this.pDisp = pDisp;
			this.uRL = uRL;
		}
	}
	
	public delegate void DWebBrowserEvents2_NewWindow2EventHandler(object sender, DWebBrowserEvents2_NewWindow2Event e);
	
	public class DWebBrowserEvents2_NewWindow2Event {
		
		public object ppDisp;
		
		public bool cancel;
		
		public DWebBrowserEvents2_NewWindow2Event(object ppDisp, bool cancel) {
			this.ppDisp = ppDisp;
			this.cancel = cancel;
		}
	}
	public delegate void DWebBrowserEvents2_NewWindow3EventHandler(object sender, DWebBrowserEvents2_NewWindow3Event e);
	
	public class DWebBrowserEvents2_NewWindow3Event 
	{
		
		public object ppDisp;
		
		public bool cancel;

		public string urlContext;

		public string url;
		
		public DWebBrowserEvents2_NewWindow3Event(object ppDisp, bool cancel, System.UInt32 dwFlags, string urlContext, string url) 
		{
			this.ppDisp = ppDisp;
			this.cancel = cancel;
			this.urlContext = urlContext;
			this.url = url;
		}
	}
	
	public delegate void DWebBrowserEvents2_BeforeNavigate2EventHandler(object sender, DWebBrowserEvents2_BeforeNavigate2Event e);
	
	public class DWebBrowserEvents2_BeforeNavigate2Event {
		
		public object pDisp;
		
		public object uRL;
		
		public object flags;
		
		public object targetFrameName;
		
		public object postData;
		
		public object headers;
		
		public bool cancel;
		
		public DWebBrowserEvents2_BeforeNavigate2Event(object pDisp, object uRL, object flags, object targetFrameName, object postData, object headers, bool cancel) {
			this.pDisp = pDisp;
			this.uRL = uRL;
			this.flags = flags;
			this.targetFrameName = targetFrameName;
			this.postData = postData;
			this.headers = headers;
			this.cancel = cancel;
		}
	}
	
	public delegate void DWebBrowserEvents2_PropertyChangeEventHandler(object sender, DWebBrowserEvents2_PropertyChangeEvent e);
	
	public class DWebBrowserEvents2_PropertyChangeEvent {
		
		public string szProperty;
		
		public DWebBrowserEvents2_PropertyChangeEvent(string szProperty) {
			this.szProperty = szProperty;
		}
	}
	
	public delegate void DWebBrowserEvents2_TitleChangeEventHandler(object sender, DWebBrowserEvents2_TitleChangeEvent e);
	
	public class DWebBrowserEvents2_TitleChangeEvent {
		
		public string text;
		
		public DWebBrowserEvents2_TitleChangeEvent(string text) {
			this.text = text;
		}
	}
	
	public delegate void DWebBrowserEvents2_CommandStateChangeEventHandler(object sender, DWebBrowserEvents2_CommandStateChangeEvent e);
	
	public class DWebBrowserEvents2_CommandStateChangeEvent {
		
		public int command;
		
		public bool enable;
		
		public DWebBrowserEvents2_CommandStateChangeEvent(int command, bool enable) {
			this.command = command;
			this.enable = enable;
		}
	}
	
	public delegate void DWebBrowserEvents2_ProgressChangeEventHandler(object sender, DWebBrowserEvents2_ProgressChangeEvent e);
	
	public class DWebBrowserEvents2_ProgressChangeEvent {
		
		public int progress;
		
		public int progressMax;
		
		public DWebBrowserEvents2_ProgressChangeEvent(int progress, int progressMax) {
			this.progress = progress;
			this.progressMax = progressMax;
		}
	}
	
	public delegate void DWebBrowserEvents2_StatusTextChangeEventHandler(object sender, DWebBrowserEvents2_StatusTextChangeEvent e);
	
	public class DWebBrowserEvents2_StatusTextChangeEvent {
		
		public string text;
		
		public DWebBrowserEvents2_StatusTextChangeEvent(string text) {
			this.text = text;
		}
	}
	
	public class AxWebBrowserEventMulticaster : DWebBrowserEvents2 {
		
		private AxWebBrowser parent;
		
		public AxWebBrowserEventMulticaster(AxWebBrowser parent) {
			this.parent = parent;
		}
		
		public virtual void PrivacyImpactedStateChange(bool bImpacted) {
			DWebBrowserEvents2_PrivacyImpactedStateChangeEvent privacyimpactedstatechangeEvent = new DWebBrowserEvents2_PrivacyImpactedStateChangeEvent(bImpacted);
			this.parent.RaiseOnPrivacyImpactedStateChange(this.parent, privacyimpactedstatechangeEvent);
		}
		
		public virtual void UpdatePageStatus(object pDisp, ref object nPage, ref object fDone) {
			DWebBrowserEvents2_UpdatePageStatusEvent updatepagestatusEvent = new DWebBrowserEvents2_UpdatePageStatusEvent(pDisp, nPage, fDone);
			this.parent.RaiseOnUpdatePageStatus(this.parent, updatepagestatusEvent);
			nPage = updatepagestatusEvent.nPage;
			fDone = updatepagestatusEvent.fDone;
		}
		
		public virtual void PrintTemplateTeardown(object pDisp) {
			DWebBrowserEvents2_PrintTemplateTeardownEvent printtemplateteardownEvent = new DWebBrowserEvents2_PrintTemplateTeardownEvent(pDisp);
			this.parent.RaiseOnPrintTemplateTeardown(this.parent, printtemplateteardownEvent);
		}
		
		public virtual void PrintTemplateInstantiation(object pDisp) {
			DWebBrowserEvents2_PrintTemplateInstantiationEvent printtemplateinstantiationEvent = new DWebBrowserEvents2_PrintTemplateInstantiationEvent(pDisp);
			this.parent.RaiseOnPrintTemplateInstantiation(this.parent, printtemplateinstantiationEvent);
		}
		
		public virtual void NavigateError(object pDisp, ref object uRL, ref object frame, ref object statusCode, ref bool cancel) {
			DWebBrowserEvents2_NavigateErrorEvent navigateerrorEvent = new DWebBrowserEvents2_NavigateErrorEvent(pDisp, uRL, frame, statusCode, cancel);
			this.parent.RaiseOnNavigateError(this.parent, navigateerrorEvent);
			uRL = navigateerrorEvent.uRL;
			frame = navigateerrorEvent.frame;
			statusCode = navigateerrorEvent.statusCode;
			cancel = navigateerrorEvent.cancel;
		}
		
		public virtual void FileDownload(ref bool cancel) {
			DWebBrowserEvents2_FileDownloadEvent filedownloadEvent = new DWebBrowserEvents2_FileDownloadEvent(cancel);
			this.parent.RaiseOnFileDownload(this.parent, filedownloadEvent);
			cancel = filedownloadEvent.cancel;
		}
		
		public virtual void SetSecureLockIcon(int secureLockIcon) {
			DWebBrowserEvents2_SetSecureLockIconEvent setsecurelockiconEvent = new DWebBrowserEvents2_SetSecureLockIconEvent(secureLockIcon);
			this.parent.RaiseOnSetSecureLockIcon(this.parent, setsecurelockiconEvent);
		}
		
		public virtual void ClientToHostWindow(ref int cX, ref int cY) {
			DWebBrowserEvents2_ClientToHostWindowEvent clienttohostwindowEvent = new DWebBrowserEvents2_ClientToHostWindowEvent(cX, cY);
			this.parent.RaiseOnClientToHostWindow(this.parent, clienttohostwindowEvent);
			cX = clienttohostwindowEvent.cX;
			cY = clienttohostwindowEvent.cY;
		}
		
		public virtual void WindowClosing(bool isChildWindow, ref bool cancel) {
			DWebBrowserEvents2_WindowClosingEvent windowclosingEvent = new DWebBrowserEvents2_WindowClosingEvent(isChildWindow, cancel);
			this.parent.RaiseOnWindowClosing(this.parent, windowclosingEvent);
			cancel = windowclosingEvent.cancel;
		}
		
		public virtual void WindowSetHeight(int height) {
			DWebBrowserEvents2_WindowSetHeightEvent windowsetheightEvent = new DWebBrowserEvents2_WindowSetHeightEvent(height);
			this.parent.RaiseOnWindowSetHeight(this.parent, windowsetheightEvent);
		}
		
		public virtual void WindowSetWidth(int width) {
			DWebBrowserEvents2_WindowSetWidthEvent windowsetwidthEvent = new DWebBrowserEvents2_WindowSetWidthEvent(width);
			this.parent.RaiseOnWindowSetWidth(this.parent, windowsetwidthEvent);
		}
		
		public virtual void WindowSetTop(int top) {
			DWebBrowserEvents2_WindowSetTopEvent windowsettopEvent = new DWebBrowserEvents2_WindowSetTopEvent(top);
			this.parent.RaiseOnWindowSetTop(this.parent, windowsettopEvent);
		}
		
		public virtual void WindowSetLeft(int left) {
			DWebBrowserEvents2_WindowSetLeftEvent windowsetleftEvent = new DWebBrowserEvents2_WindowSetLeftEvent(left);
			this.parent.RaiseOnWindowSetLeft(this.parent, windowsetleftEvent);
		}
		
		public virtual void WindowSetResizable(bool resizable) {
			DWebBrowserEvents2_WindowSetResizableEvent windowsetresizableEvent = new DWebBrowserEvents2_WindowSetResizableEvent(resizable);
			this.parent.RaiseOnWindowSetResizable(this.parent, windowsetresizableEvent);
		}
		
		public virtual void OnTheaterMode(bool theaterMode) {
			DWebBrowserEvents2_OnTheaterModeEvent ontheatermodeEvent = new DWebBrowserEvents2_OnTheaterModeEvent(theaterMode);
			this.parent.RaiseOnOnTheaterMode(this.parent, ontheatermodeEvent);
		}
		
		public virtual void OnFullScreen(bool fullScreen) {
			DWebBrowserEvents2_OnFullScreenEvent onfullscreenEvent = new DWebBrowserEvents2_OnFullScreenEvent(fullScreen);
			this.parent.RaiseOnOnFullScreen(this.parent, onfullscreenEvent);
		}
		
		public virtual void OnStatusBar(bool statusBar) {
			DWebBrowserEvents2_OnStatusBarEvent onstatusbarEvent = new DWebBrowserEvents2_OnStatusBarEvent(statusBar);
			this.parent.RaiseOnOnStatusBar(this.parent, onstatusbarEvent);
		}
		
		public virtual void OnMenuBar(bool menuBar) {
			DWebBrowserEvents2_OnMenuBarEvent onmenubarEvent = new DWebBrowserEvents2_OnMenuBarEvent(menuBar);
			this.parent.RaiseOnOnMenuBar(this.parent, onmenubarEvent);
		}
		
		public virtual void OnToolBar(bool toolBar) {
			DWebBrowserEvents2_OnToolBarEvent ontoolbarEvent = new DWebBrowserEvents2_OnToolBarEvent(toolBar);
			this.parent.RaiseOnOnToolBar(this.parent, ontoolbarEvent);
		}
		
		public virtual void OnVisible(bool visible) {
			DWebBrowserEvents2_OnVisibleEvent onvisibleEvent = new DWebBrowserEvents2_OnVisibleEvent(visible);
			this.parent.RaiseOnOnVisible(this.parent, onvisibleEvent);
		}
		
		public virtual void OnQuit() {
			System.EventArgs onquitEvent = new System.EventArgs();
			this.parent.RaiseOnOnQuit(this.parent, onquitEvent);
		}
		
		public virtual void DocumentComplete(object pDisp, ref object uRL) {
			DWebBrowserEvents2_DocumentCompleteEvent documentcompleteEvent = new DWebBrowserEvents2_DocumentCompleteEvent(pDisp, uRL);
			this.parent.RaiseOnDocumentComplete(this.parent, documentcompleteEvent);
			uRL = documentcompleteEvent.uRL;
		}
		
		public virtual void NavigateComplete2(object pDisp, ref object uRL) {
			DWebBrowserEvents2_NavigateComplete2Event navigatecomplete2Event = new DWebBrowserEvents2_NavigateComplete2Event(pDisp, uRL);
			this.parent.RaiseOnNavigateComplete2(this.parent, navigatecomplete2Event);
			uRL = navigatecomplete2Event.uRL;
		}
		
		public virtual void NewWindow2(ref object ppDisp, ref bool cancel) {
			DWebBrowserEvents2_NewWindow2Event newwindow2Event = new DWebBrowserEvents2_NewWindow2Event(ppDisp, cancel);
			this.parent.RaiseOnNewWindow2(this.parent, newwindow2Event);
			ppDisp = newwindow2Event.ppDisp;
			cancel = newwindow2Event.cancel;
		}
		public virtual void NewWindow3(ref object ppDisp, ref bool Cancel, uint dwFlags, string bstrUrlContext, string bstrUrl)
		{
			DWebBrowserEvents2_NewWindow3Event newwindow3Event = new DWebBrowserEvents2_NewWindow3Event(ppDisp, Cancel, dwFlags, bstrUrlContext, bstrUrl );
			this.parent.RaiseOnNewWindow3(this.parent, newwindow3Event);
			ppDisp = newwindow3Event.ppDisp;
			Cancel = newwindow3Event.cancel;
		}
		
		public virtual void BeforeNavigate2(object pDisp, ref object uRL, ref object flags, ref object targetFrameName, ref object postData, ref object headers, ref bool cancel) {
			DWebBrowserEvents2_BeforeNavigate2Event beforenavigate2Event = new DWebBrowserEvents2_BeforeNavigate2Event(pDisp, uRL, flags, targetFrameName, postData, headers, cancel);
			this.parent.RaiseOnBeforeNavigate2(this.parent, beforenavigate2Event);
			uRL = beforenavigate2Event.uRL;
			flags = beforenavigate2Event.flags;
			targetFrameName = beforenavigate2Event.targetFrameName;
			postData = beforenavigate2Event.postData;
			headers = beforenavigate2Event.headers;
			cancel = beforenavigate2Event.cancel;
		}
		
		public virtual void PropertyChange(string szProperty) {
			DWebBrowserEvents2_PropertyChangeEvent propertychangeEvent = new DWebBrowserEvents2_PropertyChangeEvent(szProperty);
			this.parent.RaiseOnPropertyChange(this.parent, propertychangeEvent);
		}
		
		public virtual void TitleChange(string text) {
			DWebBrowserEvents2_TitleChangeEvent titlechangeEvent = new DWebBrowserEvents2_TitleChangeEvent(text);
			this.parent.RaiseOnTitleChange(this.parent, titlechangeEvent);
		}
		
		public virtual void DownloadComplete() {
			System.EventArgs downloadcompleteEvent = new System.EventArgs();
			this.parent.RaiseOnDownloadComplete(this.parent, downloadcompleteEvent);
		}
		
		public virtual void DownloadBegin() {
			System.EventArgs downloadbeginEvent = new System.EventArgs();
			this.parent.RaiseOnDownloadBegin(this.parent, downloadbeginEvent);
		}
		
		public virtual void CommandStateChange(int command, bool enable) {
			DWebBrowserEvents2_CommandStateChangeEvent commandstatechangeEvent = new DWebBrowserEvents2_CommandStateChangeEvent(command, enable);
			this.parent.RaiseOnCommandStateChange(this.parent, commandstatechangeEvent);
		}
		
		public virtual void ProgressChange(int progress, int progressMax) {
			DWebBrowserEvents2_ProgressChangeEvent progresschangeEvent = new DWebBrowserEvents2_ProgressChangeEvent(progress, progressMax);
			this.parent.RaiseOnProgressChange(this.parent, progresschangeEvent);
		}
		
		public virtual void StatusTextChange(string text) {
			DWebBrowserEvents2_StatusTextChangeEvent statustextchangeEvent = new DWebBrowserEvents2_StatusTextChangeEvent(text);
			this.parent.RaiseOnStatusTextChange(this.parent, statustextchangeEvent);
		}





	}
}

