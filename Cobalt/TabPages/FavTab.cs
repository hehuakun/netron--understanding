using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Netron.Neon;
using Netron.GraphLib;
using System.Drawing;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
namespace Netron.Cobalt
{
	/// <summary>
	/// Summary description for CodeTab.
	/// </summary>
	public class FavTab : DockContent, ICobaltTab
	{
		#region Fields
		private Mediator mediator;
		private System.Windows.Forms.Panel uppderPanel;
		private System.Windows.Forms.ListView templates;
		private System.Windows.Forms.ColumnHeader colName;
		private System.Windows.Forms.ColumnHeader colDescription;
		private System.Windows.Forms.ColumnHeader colCategory;
		private System.Windows.Forms.Button OpenButton;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Button SaveButton;
		private System.Windows.Forms.Button OpenDefaultButton;
		private System.Windows.Forms.Button DeleteButton;
		private string currentDirectory;
		private string identifier;
		private string defaultFolder = "c:\\";
		#endregion

		#region Properties

		public TabTypes TabType
		{
			get{return TabTypes.ShapeFavorites;}
		}

		public string DefaultDirectory
		{
			get{return defaultFolder;}
			set{defaultFolder = value;}
		}

		public string TabIdentifier
		{
			get{return this.identifier;}
			set{identifier = value;}
		}

	



	
		

		#endregion

		#region Constructor
		public FavTab(Mediator mediator)
		{
			InitializeComponent();

			this.mediator = mediator;
			
		}
		#endregion

		#region Methods

		private void Favorites_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			
			
			if(e.Data.GetDataPresent(DataFormats.Serializable, true))			
			{
				e.Effect = DragDropEffects.Copy;//GetDragDropEffect(e);
			}
		}
	
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FavTab));
            this.uppderPanel = new System.Windows.Forms.Panel();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.OpenDefaultButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.OpenButton = new System.Windows.Forms.Button();
            this.templates = new System.Windows.Forms.ListView();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCategory = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.uppderPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // uppderPanel
            // 
            this.uppderPanel.Controls.Add(this.DeleteButton);
            this.uppderPanel.Controls.Add(this.OpenDefaultButton);
            this.uppderPanel.Controls.Add(this.SaveButton);
            this.uppderPanel.Controls.Add(this.OpenButton);
            this.uppderPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.uppderPanel.Location = new System.Drawing.Point(0, 0);
            this.uppderPanel.Name = "uppderPanel";
            this.uppderPanel.Size = new System.Drawing.Size(292, 34);
            this.uppderPanel.TabIndex = 0;
            // 
            // DeleteButton
            // 
            this.DeleteButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.DeleteButton.Image = ((System.Drawing.Image)(resources.GetObject("DeleteButton.Image")));
            this.DeleteButton.Location = new System.Drawing.Point(121, 2);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(28, 25);
            this.DeleteButton.TabIndex = 3;
            this.toolTip1.SetToolTip(this.DeleteButton, "Delete selected item");
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // OpenDefaultButton
            // 
            this.OpenDefaultButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.OpenDefaultButton.Image = ((System.Drawing.Image)(resources.GetObject("OpenDefaultButton.Image")));
            this.OpenDefaultButton.Location = new System.Drawing.Point(83, 3);
            this.OpenDefaultButton.Name = "OpenDefaultButton";
            this.OpenDefaultButton.Size = new System.Drawing.Size(27, 25);
            this.OpenDefaultButton.TabIndex = 2;
            this.toolTip1.SetToolTip(this.OpenDefaultButton, "Open default directory");
            this.OpenDefaultButton.Click += new System.EventHandler(this.OpenDefaultButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.BackColor = System.Drawing.SystemColors.Control;
            this.SaveButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.SaveButton.Image = ((System.Drawing.Image)(resources.GetObject("SaveButton.Image")));
            this.SaveButton.Location = new System.Drawing.Point(44, 3);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(28, 25);
            this.SaveButton.TabIndex = 1;
            this.toolTip1.SetToolTip(this.SaveButton, "Save templates to files");
            this.SaveButton.UseVisualStyleBackColor = false;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // OpenButton
            // 
            this.OpenButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.OpenButton.Image = ((System.Drawing.Image)(resources.GetObject("OpenButton.Image")));
            this.OpenButton.Location = new System.Drawing.Point(6, 3);
            this.OpenButton.Name = "OpenButton";
            this.OpenButton.Size = new System.Drawing.Size(28, 25);
            this.OpenButton.TabIndex = 0;
            this.toolTip1.SetToolTip(this.OpenButton, "Open directory");
            this.OpenButton.Click += new System.EventHandler(this.OpenButton_Click);
            // 
            // templates
            // 
            this.templates.AllowDrop = true;
            this.templates.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.templates.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colDescription,
            this.colCategory});
            this.templates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.templates.FullRowSelect = true;
            this.templates.Location = new System.Drawing.Point(0, 34);
            this.templates.Name = "templates";
            this.templates.Size = new System.Drawing.Size(292, 232);
            this.templates.TabIndex = 2;
            this.templates.UseCompatibleStateImageBehavior = false;
            this.templates.View = System.Windows.Forms.View.Details;
            this.templates.ItemActivate += new System.EventHandler(this.Favorites_ItemActivate);
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 120;
            // 
            // colDescription
            // 
            this.colDescription.Text = "Description";
            this.colDescription.Width = 177;
            // 
            // colCategory
            // 
            this.colCategory.Text = "Category";
            this.colCategory.Width = 84;
            // 
            // FavTab
            // 
            this.AccessibleDescription = "This panel collects diagram templates. You can save templates like diagrams. Doub" +
    "le-click an item to load it into the canvas.";
            this.AllowDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.templates);
            this.Controls.Add(this.uppderPanel);
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FavTab";
            this.TabText = "Templates";
            this.uppderPanel.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		private void Favorites_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			Shape sh = null;
			sh = e.Data.GetData(DataFormats.Serializable, true) as Shape;
			ListViewItem item;
			Netron.GraphLib.Configuration.ShapeSummary sum;
			if( sh==null) return;
			try
			{
				
					//only taking the first one
					//sh = mediator.GraphControl.SelectedShapes[0];
					//reflect what's in it
					sum = mediator.GraphControl.GetShapeSummary(sh);
					//sample of possible info
					item = new ListViewItem(new string[]{sh.GetType().Name,sum.Key,sh.Text});
					item.Tag = sh;
					//add to the favs
					this.templates.Items.Add(item);
					
				
				
			}
			catch
			{
				MessageBox.Show("No valid shape data found to add to favorites.","No data error",MessageBoxButtons.OK,MessageBoxIcon.Warning);
			}


			/* the old way
			 Shape sh = null;
			sh = e.Data.GetData(DataFormats.Serializable, true) as Shape;
			ListViewItem item;
			Netron.GraphLib.Configuration.ShapeSummary sum;
			try
			{
				if(mediator.GraphControl.SelectedShapes.Count>0)
				{
					//only taking the first one
					sh = mediator.GraphControl.SelectedShapes[0];
					//reflect what's in it
					sum = mediator.GraphControl.GetShapeSummary(sh);
					//sample of possible info
					item = new ListViewItem(new string[]{sh.GetType().Name,sum.Key,sh.Text});
					//add to the favs
					this.templates.Items.Add(item);
					
				}
				
			}
			catch
			{
				MessageBox.Show("No valid shape data found to add to favorites.","No data error",MessageBoxButtons.OK,MessageBoxIcon.Warning);
			} 
			 */
		
			
			
		}

		private void Favorites_ItemActivate(object sender, System.EventArgs e)
		{
			ListViewItem item;
			if(templates.SelectedItems.Count>0)
			{
				item = templates.SelectedItems[0];
				EntityBundle bundle = item.Tag as EntityBundle;
				EntityBundle copy;
				if(bundle!=null)
				{
					//make sure the graphcontrol is there
					mediator.OpenGraphTab();
					bundle.Site = mediator.GraphControl;
					//here you need to take another copy, otherwise it'll result in a linked instance
					copy = bundle.Copy();
					//unwrap the bundle, this assigns the site and does some postserialization
					GraphLib.IO.Binary.BinarySerializer.UnwrapBundle(copy, mediator.GraphControl);

				//change the UID
				if(copy!=null)
				{
					for(int k=0; k<copy.Shapes.Count; k++)					
					{
						copy.Shapes[k].GenerateNewUID();
						//the connectors as well
						foreach(Netron.GraphLib.Connector c in copy.Shapes[k].Connectors)
						{
							c.GenerateNewUID();
						}
					}
					for(int k=0; k<copy.Connections.Count; k++)					
						copy.Connections[k].GenerateNewUID();					
				}
					mediator.GraphControl.AddBundle(copy);
					
				}
			}
		}

		private void ActivateSelectedItem()
		{
			ListViewItem item;
			if(templates.SelectedItems.Count>0)
			{
				item = templates.SelectedItems[0];
				Shape sh = item.Tag as Shape;
				if(sh==null) return;
				mediator.GraphControl.AddShape(sh);				
			}
		}

		public int AddFavorite(object favorite, string name, string description)
		{

			ListViewItem item = new ListViewItem(new string[]{name, description, ""});
			item.Tag = favorite;
			this.templates.Items.Add(item);
			return item.Index;

		}


		/// <summary>
		/// Loads a template directory
		/// </summary>
		public void LoadDirectory()
		{		
			Netron.Neon.FolderBrowser	 browser = new FolderBrowser(this);
			browser.Caption = "Select a folder wherein the templates are stored:";
			browser.Title = "Folder to load templates from";
			string path = string.Empty;
			if((path=browser.BrowseForFolder())!=null)
			{
				
				currentDirectory = path;
				LoadDirectory(path);
			}
		}
		/// <summary>
		/// Saves the templates to file
		/// </summary>
		public void SaveTemplates()
		{
			Netron.Neon.FolderBrowser	 browser = new FolderBrowser(this);
			browser.Caption = "Select a folder wherein the templates are stored:";
			browser.Title = "Templates folder";
			browser.InitialSelection = currentDirectory;
			string folder = browser.BrowseForFolder();
			SaveTemplates(folder);


		}

		private void DeleteButton_Click(object sender, System.EventArgs e)
		{
			if(templates.SelectedItems.Count>0)
			{

					templates.Items.RemoveAt(templates.SelectedItems[0].Index);
			}
		}

		private void OpenDefaultButton_Click(object sender, System.EventArgs e)
		{
			LoadDefaultDirectory();
		}

		private void SaveButton_Click(object sender, System.EventArgs e)
		{
			SaveTemplates();
		}

		private void OpenButton_Click(object sender, System.EventArgs e)
		{
			LoadDirectory();
		}
		/// <summary>
		/// Loads the default directory. The default directory is specified in the app.config.
		/// </summary>
		public void LoadDefaultDirectory()
		{
			LoadDirectory(defaultFolder);
		}
		/// <summary>
		/// Deletes the selectd item from the list
		/// </summary>
		public void DeleteSelectedItem()
		{
		
		}

		/// <summary>
		/// Loads the templates in from the given directory
		/// </summary>
		/// <param name="path"></param>
		public void LoadDirectory(string path)
		{
			FileStream fs=null;
			try
			{
				
				if(!Directory.Exists(path)) return;
				path = Path.GetFullPath(path);
				if(!path.EndsWith("\\")) path += "\\";
				currentDirectory = path;
				templates.Items.Clear();
				string[] files = Directory.GetFiles(path, "*.template");
				ListViewItem item;
				for(int k = 0; k<files.Length; k++)
				{
					try
					{

						
						BinaryFormatter formatter = new BinaryFormatter();
						
			
						try
						{
							fs= File.OpenRead(files[k]);
						}
						catch (System.IO.DirectoryNotFoundException exc)
						{
							System.Windows.Forms.MessageBox.Show(exc.Message);
						}
						catch(System.IO.FileLoadException exc)
						{				
							System.Windows.Forms.MessageBox.Show(exc.Message);
						}
						catch (System.IO.FileNotFoundException exc)
						{
							System.Windows.Forms.MessageBox.Show(exc.Message);
						}
						catch
						{				
							mediator.Output("Non-CLS exception caught.", OutputInfoLevels.Exception);
						}
						//donnot open anything if filestream is not there
						if (fs==null) return;
						
				
						BinaryFormatter f = new BinaryFormatter();

						EntityBundle bundle = (EntityBundle) f.Deserialize(fs); 
						if(bundle==null) continue;
						item = new ListViewItem(new string[]{bundle.Name, bundle.Description,""});
						item.Tag = bundle;				
						templates.Items.Add(item);
						mediator.Output("Found template '" + path + "\\" + files[k] + "' in directory.",GraphLib.OutputInfoLevels.Info);
					}
					catch(Exception exc)
					{
						mediator.Output("File '" + path + "\\" + files[k] + "' seems not to be a valid template file.",GraphLib.OutputInfoLevels.Info);
						Trace.WriteLine(exc.Message,"FavTab.LoadDirectory(string)");
						continue;
					}
					finally
					{
						fs.Close();
					}
				}
			}
			catch(System.IO.DirectoryNotFoundException e1)
			{
				mediator.Output("The directory was not found",GraphLib.OutputInfoLevels.Exception);
				Trace.WriteLine(e1.Message,"FavTab.LoadDirectory");
			}
			catch(System.IO.FileNotFoundException e2)
			{
				mediator.Output("The file was not found",GraphLib.OutputInfoLevels.Exception);
				Trace.WriteLine(e2.Message,"FavTab.LoadDirectory");
			}
			catch(Exception e)
			{
				mediator.Output(e.Message,GraphLib.OutputInfoLevels.Exception);
				Trace.WriteLine(e.Message,"FavTab.LoadDirectory");
			}
		}

		/// <summary>
		/// Saves the templates to the given folder
		/// </summary>
		/// <param name="path">a folder</param>
		private void SaveTemplates(string path)
		{
			for(int k=0; k<templates.Items.Count; k++)
			{
				EntityBundle bundle = templates.Items[k].Tag as EntityBundle;
				if(bundle==null) return;
				FileStream fs = new FileStream(path + "\\" + bundle.Name + ".template", FileMode.Create);

				BinaryFormatter f = new BinaryFormatter();			
			
				try
				{					
					f.Serialize(fs, bundle);					
				}			
				catch(Exception exc)			
				{
						mediator.Output(exc.Message,OutputInfoLevels.Exception);
						continue;
				}
				catch
				{
					mediator.Output("Non-CLS exception caught.",OutputInfoLevels.Exception);
					continue;
				}
				finally
				{
					fs.Close();
				}
			}
		}
		#endregion

	
	}
}
