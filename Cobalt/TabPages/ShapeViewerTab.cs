using System;
using System.IO;
using System.Reflection;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Netron.Neon;
namespace Netron.Cobalt
{
	/// <summary>
	/// Summary description for ShapeViewerTab.
	/// </summary>
	public class ShapeViewerTab : DockContent, ICobaltTab
	{
		#region Fields

		private Netron.GraphLib.UI.GraphShapesView graphShapesView1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private string identifier;

		#endregion

		#region Properties
		public TabTypes TabType
		{
			get{return TabTypes.ShapesViewer;}
		}

		public string TabIdentifier
		{
			get{return this.identifier;}
			set{identifier = value;}
		}
		#endregion
		
		
		public ShapeViewerTab(Mediator mediator)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			#region load the libraries of shapes defined in the app.config
			
			//load it in the shapes-viewer as well
			graphShapesView1.LoadLibraries();

			string libPath = Path.GetDirectoryName(Application.ExecutablePath) + "\\" + Assembly.GetAssembly(typeof(Netron.GraphLib.UI.GraphControl)).GetName().Name + ".dll";
			this.graphShapesView1.AddLibrary(libPath);
			#endregion
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ShapeViewerTab));
			this.graphShapesView1 = new Netron.GraphLib.UI.GraphShapesView();
			((System.ComponentModel.ISupportInitialize)(this.graphShapesView1)).BeginInit();
			this.SuspendLayout();
			// 
			// graphShapesView1
			// 
			this.graphShapesView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.graphShapesView1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.graphShapesView1.Location = new System.Drawing.Point(0, 0);
			this.graphShapesView1.Name = "graphShapesView1";
			this.graphShapesView1.Size = new System.Drawing.Size(292, 273);
			this.graphShapesView1.TabIndex = 1;
			this.graphShapesView1.View = System.Windows.Forms.View.LargeIcon;
			// 
			// ShapeViewerTab
			// 
			this.AccessibleDescription = "The shapes library collects the shapes found in the shape assemblies. Drag-drop s" +
				"hapes from the library onto the canvas to add them.";
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 273);
			this.Controls.Add(this.graphShapesView1);
			this.HideOnClose = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ShapeViewerTab";
			this.Text = "ShapeViewerTab";
			((System.ComponentModel.ISupportInitialize)(this.graphShapesView1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion
	}
}
