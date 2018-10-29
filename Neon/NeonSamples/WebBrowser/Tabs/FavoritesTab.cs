using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Netron.Neon;
namespace WebBrowser
{
	/// <summary>
	/// Summary description for FavoritesTab.
	/// </summary>
	public class FavoritesTab : DockContent, IFavTab
	{
		
		public event URLHandler OnFavClick;
		private string identifier;
		private Netron.Neon.NFavorites favorites;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FavoritesTab()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FavoritesTab));
			this.favorites = new Netron.Neon.NFavorites();
			this.SuspendLayout();
			// 
			// favorites
			// 
			this.favorites.Dock = System.Windows.Forms.DockStyle.Fill;
			//this.favorites.FavoritesPath = "D:\\Documents and Settings\\fvdseype\\Favorieten";
			this.favorites.Location = new System.Drawing.Point(0, 0);
			this.favorites.Name = "favorites";
			this.favorites.Root = null;
			this.favorites.Size = new System.Drawing.Size(292, 273);
			this.favorites.Tab = null;
			this.favorites.TabIndex = 0;
			this.favorites.URLClick += new Netron.Neon.URLHandler(this.favorites_URLClick);
			// 
			// FavoritesTab
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 273);
			this.Controls.Add(this.favorites);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FavoritesTab";
			this.Text = "FavoritesTab";
			this.ResumeLayout(false);

		}
		#endregion

		private void favorites_URLClick(object sender, Netron.Neon.URLInfo e)
		{
		if(OnFavClick!=null)
			OnFavClick(sender,e);
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
			get{return TabTypes.Favorites;}
		}
	}
}
