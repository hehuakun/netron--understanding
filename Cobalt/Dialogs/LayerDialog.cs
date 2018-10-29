using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Netron.GraphLib;
using Netron.GraphLib.UI;
namespace Netron.Cobalt
{
	/// <summary>
	/// Summary description for LayersDialog.
	/// </summary>
	public class LayersDialog : System.Windows.Forms.Form
	{
		public Netron.GraphLib.UI.GraphLayerManager Manager;
		private System.Windows.Forms.Button Cancel;
		private System.Windows.Forms.Button OK;
		private System.Windows.Forms.Button Apply;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private GraphControl site;
		public LayersDialog(GraphControl  site)
		{
			this.site = site;
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(LayersDialog));
			this.Manager = new Netron.GraphLib.UI.GraphLayerManager();
			this.Cancel = new System.Windows.Forms.Button();
			this.OK = new System.Windows.Forms.Button();
			this.Apply = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// Manager
			// 
			this.Manager.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Manager.Location = new System.Drawing.Point(0, 0);
			this.Manager.Name = "Manager";
			this.Manager.Size = new System.Drawing.Size(512, 381);
			this.Manager.TabIndex = 0;
			// 
			// Cancel
			// 
			this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.Cancel.Location = new System.Drawing.Point(344, 384);
			this.Cancel.Name = "Cancel";
			this.Cancel.TabIndex = 1;
			this.Cancel.Text = "Cancel";
			this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
			// 
			// OK
			// 
			this.OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.OK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.OK.Location = new System.Drawing.Point(424, 384);
			this.OK.Name = "OK";
			this.OK.TabIndex = 2;
			this.OK.Text = "OK";
			this.OK.Click += new System.EventHandler(this.OK_Click);
			// 
			// Apply
			// 
			this.Apply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.Apply.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.Apply.Location = new System.Drawing.Point(248, 384);
			this.Apply.Name = "Apply";
			this.Apply.TabIndex = 3;
			this.Apply.Text = "Apply";
			this.Apply.Click += new System.EventHandler(this.Apply_Click);
			// 
			// LayersDialog
			// 
			this.AcceptButton = this.OK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.Cancel;
			this.ClientSize = new System.Drawing.Size(512, 422);
			this.Controls.Add(this.Apply);
			this.Controls.Add(this.OK);
			this.Controls.Add(this.Cancel);
			this.Controls.Add(this.Manager);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "LayersDialog";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Graph layers";
			this.ResumeLayout(false);

		}
		#endregion

		private void Cancel_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private void OK_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Manager.UpdateLayerData();
			foreach(Shape sh in site.Shapes)
			{
				if(sh.Layer!=null)
					sh.SetLayer(sh.Layer.Name);//update colors and stuff
			}
			site.Invalidate();
			Close();
		}

		private void Apply_Click(object sender, System.EventArgs e)
		{
			Manager.UpdateLayerData();
			site.Invalidate();
		}
	}
}
