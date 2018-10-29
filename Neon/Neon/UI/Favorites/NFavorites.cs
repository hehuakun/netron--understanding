using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Windows.Forms;



namespace  Netron.Neon
{


	/// <summary>
	/// Displays the favorites of the current user
	/// </summary>
	public class NFavorites : UserControl, INAFFavorites
	{
		public event EventHandler OnShow;

		#region Fields

		protected TabPage tab;

		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.TreeView tvFavorites;
		private System.Windows.Forms.ImageList imgList;

		private FileSystemWatcher _objFSW;
		private string _strFavoritesPath = "";
		private string _strCurrentURL = "";
		private string _strCurrentURLName = "";
		private string _strCurrentFolder = "";
		private int _intX = 0;
		private int _intY = 0;
		private INUIMediator root;
		#endregion

		#region Events
		// This event is raised when a URL is clicked
		public event URLHandler URLClick;
		
		#endregion 

		#region Properties
		public TabPage Tab
		{
			get{return tab;}
			set{tab = value;}
		}
		public INUIMediator Root
		{
			get{return root;}
			set{root = value;}
		}
		#endregion

		#region Consructors and Destructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NFavorites()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

		

			// Get the Internet Explorer NFavorites path
			_GetFavoritesPath();

			// Load the TreeView
			_RefreshFavorites();

			// Setup a file watcher
			_InitFileSystemWatcher();
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

		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(NFavorites));
			this.tvFavorites = new System.Windows.Forms.TreeView();
			this.imgList = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// tvFavorites
			// 
			this.tvFavorites.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.tvFavorites.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.tvFavorites.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvFavorites.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.tvFavorites.ForeColor = System.Drawing.Color.SlateGray;
			this.tvFavorites.FullRowSelect = true;
			this.tvFavorites.HideSelection = false;
			this.tvFavorites.HotTracking = true;
			this.tvFavorites.ImageList = this.imgList;
			this.tvFavorites.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.tvFavorites.Location = new System.Drawing.Point(0, 0);
			this.tvFavorites.Name = "tvFavorites";
			this.tvFavorites.ShowLines = false;
			this.tvFavorites.ShowPlusMinus = false;
			this.tvFavorites.ShowRootLines = false;
			this.tvFavorites.Size = new System.Drawing.Size(184, 224);
			this.tvFavorites.TabIndex = 0;
			this.tvFavorites.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tvFavorites_MouseDown);
			this.tvFavorites.Click += new System.EventHandler(this.tvFavorites_Click);
			this.tvFavorites.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tvFavorites_MouseMove);
			// 
			// imgList
			// 
			this.imgList.ImageSize = new System.Drawing.Size(16, 16);
			this.imgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgList.ImageStream")));
			this.imgList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// NFavorites
			// 
			this.Controls.Add(this.tvFavorites);
			this.Name = "Favorites";
			this.Size = new System.Drawing.Size(184, 224);
			this.ResumeLayout(false);

		}
		#endregion

		#region Public Properties
		
		/// <summary>
		/// Returns the currently (or last) selected URL.
		/// </summary>
		public string CurrentURL
		{
			get
			{
				return _strCurrentURL;
			}
		}

		/// <summary>
		/// Returns the currently (or last) selected URL Name.
		/// </summary>
 		public string CurrentURLName
		{
			get
			{
				return _strCurrentURLName;
			}
		}
		
		/// <summary>
		/// Returns the currently selected folder.
		/// </summary>
		public string CurrentFolder
		{
			get
			{
				return _strCurrentFolder;
			}
		}

		/// <summary>
		/// Returns/sets the Internet Explorer favorites path for the current user.
		/// </summary>
		[Browsable(false)]
		public string FavoritesPath
		{
			get
			{
				return _strFavoritesPath;
			}
			set
			{
				_strFavoritesPath = value;
				_RefreshFavorites();
			}
		}

		#endregion

		#region Methods

		public void AddFavorite(URLInfo url)
		{
			
		}

		public void RaiseShow()
		{
			if(OnShow!=null)
				OnShow(this,EventArgs.Empty);
		}

		/// <summary>
		/// Gets the Internet Explorer NFavorites path for the current user 
		/// from the Windows Registry.
		/// </summary>
		private void _GetFavoritesPath()
		{
			Microsoft.Win32.RegistryKey objRegKey = Microsoft.Win32.Registry.CurrentUser;
			Microsoft.Win32.RegistryKey objFav = objRegKey.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Shell Folders");

			_strFavoritesPath = (string)objFav.GetValue("Favorites");
		}

		/// <summary>
		/// Sets up a FileSystemWatcher object to monitor the current user's
		/// NFavorites folder.  If any changes are made to the NFavorites, the
		/// list will be refreshed.
		/// </summary>
		private void _InitFileSystemWatcher()
		{
			// Create a new file system watcher object
			_objFSW = new FileSystemWatcher();

			// Set the path to be watched (the current user's NFavorites)
			_objFSW.Path = _strFavoritesPath;

			// Set the modification filters
			_objFSW.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | 
				NotifyFilters.DirectoryName | NotifyFilters.FileName;

			// Set the filter mask (i.e. watch all files)
			_objFSW.Filter = "*.*";

			// We want to watch subdirectories as well
			_objFSW.IncludeSubdirectories = true;

			// Setup the event handlers
			_objFSW.Changed += new FileSystemEventHandler(FSW_OnChanged);
			_objFSW.Created += new FileSystemEventHandler(FSW_OnChanged);
			_objFSW.Deleted += new FileSystemEventHandler(FSW_OnChanged);
			_objFSW.Renamed += new RenamedEventHandler(FSW_OnRenamed);

			// Let's start watching
			_objFSW.EnableRaisingEvents = true;
		}

		/// <summary>
		/// This is the main method for loading the favorites list.
		/// </summary>
		private void _RefreshFavorites() 
		{
			// Suppress repainting the TreeView until the nodes are added
			tvFavorites.BeginUpdate();

			// Clear the NFavorites list
			tvFavorites.Nodes.Clear(); 

			// Load favorites from all sub-directories
			_LoadFavoritesFromFolder(new System.IO.DirectoryInfo(_strFavoritesPath), null);

			// Load the favorites from the favorites folder
			_LoadFavoritesFromPath(_strFavoritesPath, null);

			// Repaint the TreeView
			tvFavorites.EndUpdate();
		}

		/// <summary>
		/// Load each sub-folder's favorites.  This method is called recursivley for
		/// each sub-directory.
		/// </summary>
		/// <param name="aobjDirInfo">Directory information for the directory being processed.</param>
		/// <param name="aobjNode">The current TreeView Node being processed</param>
		private void _LoadFavoritesFromFolder(System.IO.DirectoryInfo aobjDirInfo, TreeNode aobjNode) 
		{
			System.Windows.Forms.TreeNode objNode;

			try
			{
				foreach (System.IO.DirectoryInfo objDir in aobjDirInfo.GetDirectories())
				{
					if (aobjNode == null) 
						objNode = tvFavorites.Nodes.Add(objDir.Name);
					else
						objNode = aobjNode.Nodes.Add(objDir.Name);

					// Set the icon to be displayed when the node is selected
					objNode.SelectedImageIndex = 1;

					// Set the icon to be displayed when the node is not selected
					objNode.ImageIndex = 2;

					if (objDir.GetDirectories().Length == 0) 
						// This node has no further sub-directories
						_LoadFavoritesFromPath(objDir.FullName, objNode);
					else 
						// Add this folder to the current node and continue
						// processing sub-directories.
						_LoadFavoritesFromFolder(objDir, objNode);
				}
			}
			catch{}
		}

		/// <summary>
		/// Loads the favorites from the specified path.
		/// </summary>
		/// <param name="astrPath">The path to read the favorites (URL links)
		/// from.</param>
		/// <param name="aobjNode">The TreeView node the favorites should
		/// be placed in.</param>
		private void _LoadFavoritesFromPath(string astrPath, TreeNode aobjNode)
		{
			System.IO.DirectoryInfo objDir = new System.IO.DirectoryInfo(astrPath);

			// Process each URL in the path (URL files end with a ".url" extension
			try
			{
				foreach (System.IO.FileInfo objFile in objDir.GetFiles("*.url")) 
				{
					TreeNode objNode = new System.Windows.Forms.TreeNode();
					objNode.ImageIndex = 0;
					objNode.SelectedImageIndex = 0;

					// The URL file is in standard "INI" format
					IniFile objINI = new IniFile(objFile.FullName);

					// Set the TreeView node's Tag property to the URL
					objNode.Tag = objINI.IniReadValue("InternetShortcut", "URL");

					// Set the Text property to the "Friendly" name
					objNode.Text = objFile.Name.Substring(0, objFile.Name.IndexOf(".url"));

					if (aobjNode == null)
						tvFavorites.Nodes.Add(objNode);
					else 
						aobjNode.Nodes.Add(objNode);
				}
			}
			catch{}
		}

		/// <summary>
		/// Collapses all siblings nodes.
		/// </summary>
		/// <param name="aobjNode">The node whose siblings will be collapsed.</param>
		private void _CollapseSiblings(TreeNode aobjNode)
		{
			TreeNode objNode = aobjNode.PrevNode;
			while (objNode != null)
			{
				objNode.Collapse();
				objNode = objNode.PrevNode;
			}

			objNode = aobjNode.NextNode;
			while (objNode != null)
			{
				objNode.Collapse();
				objNode = objNode.NextNode;
			}
		}

		/// <summary>
		/// This event handler is called when a file/folder has been modified.
		/// </summary>
		private void FSW_OnChanged(object source, FileSystemEventArgs e)
		{
			// We must invoke a delegate on the underlying control's thread
			// to refresh the favorites list.
			try
			{
				this.Invoke(new EventHandler(FSW_Reload));
			}
			catch(Exception exc)
			{
				//TODO: use NAF exception here
				System.Diagnostics.Trace.WriteLine(exc.Message,"Warning");
			}
		}

		/// <summary>
		/// This event handler is called when a file/folder has been renamed.
		/// </summary>
		private void FSW_OnRenamed(object source, RenamedEventArgs e)
		{
			// We must invoke a delegate on the underlying control's thread
			// to refresh the favorites list.
			this.Invoke(new EventHandler(FSW_Reload));
		}

		/// <summary>
		/// This event is called when the favorites list needs to be refreshed.
		/// </summary>
		private void FSW_Reload(object source, EventArgs args)
		{
			_RefreshFavorites();
		}

		/// <summary>
		/// Change the cursor to a hand for URL links or to an arrow for folders.
		/// </summary>
		private void tvFavorites_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			TreeView objTreeView = (TreeView)sender;
			TreeNode objNode = objTreeView.GetNodeAt(e.X, e.Y);

			if (objNode != null)
			{
				if (objNode.Tag != null)
					// We're over a URL
					objTreeView.Cursor = Cursors.Hand;
				else
					// We're over a folder
					objTreeView.Cursor = Cursors.Default;
			}
			else
				// We're over an empty region in the TreeView
				objTreeView.Cursor = Cursors.Default;

		}

		/// <summary>
		/// Processes node clicks and expands or collapses nodes accordingly.
		/// </summary>
		private void tvFavorites_Click(object sender, System.EventArgs e)
		{
			
			TreeView objTreeView = (TreeView)sender;
			TreeNode objNode = objTreeView.GetNodeAt(_intX, _intY);

			if (objNode != null)
			{
				if (objNode.Tag != null) 
				{
					// A URL was clicked
					_strCurrentURL = (string)objNode.Tag;
					_strCurrentURLName = (string)objNode.Text;

					// Raise an event notifying the owner that a URL was clicked
					if(URLClick !=null)
						URLClick(this, new URLInfo(_strCurrentURL,_strCurrentURLName ));
				} 
				else 
				{
					// A folder node was clicked
					_strCurrentFolder = (string)objNode.Text;

					// Collapse all sibling nodes so only one folder is open at any given level
					_CollapseSiblings(objNode);

					// Toggle the folder
					if (!objNode.IsExpanded) 
						objNode.Expand();
					else
						objNode.Collapse();
				}
			}
		
		}

		/// <summary>
		/// Stores the current position of the mouse when it is clicked for future use.
		/// </summary>
		private void tvFavorites_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(e.Button==MouseButtons.Left)
			{
				_intX = e.X;
				_intY = e.Y;
			}
		}

		#endregion

	}
}
