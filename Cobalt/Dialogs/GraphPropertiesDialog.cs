using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Netron.GraphLib;
namespace Netron.Cobalt
{
	/// <summary>
	/// Summary description for GraphPropertiesDialog.
	/// </summary>
	public class GraphPropertiesDialog : System.Windows.Forms.Form
	{

		private System.Windows.Forms.Panel surroundPanel;
		private System.Windows.Forms.Panel upperPanel;
		private System.Windows.Forms.Panel lowerPanel;
		private System.Windows.Forms.Button Cancel;
		private System.Windows.Forms.Button OK;
		private Netron.Neon.XPCaption xpCaption1;
		private Netron.GraphLib.UI.GraphProps graphProps1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public GraphInformation GraphInformation
		{
			get{return this.graphProps1.GraphInformation;}
			set{this.graphProps1.GraphInformation = value;}
		}

		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress (e);
			if(e.KeyChar == (char) Keys.Escape)
			{
				this.DialogResult = DialogResult.Cancel;
				Close();
			}
		}

	

		public GraphPropertiesDialog( GraphInformation info)
		{
			

			InitializeComponent();
			graphProps1.GraphInformation = info;

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(GraphPropertiesDialog));
			this.surroundPanel = new System.Windows.Forms.Panel();
			this.upperPanel = new System.Windows.Forms.Panel();
			this.xpCaption1 = new Netron.Neon.XPCaption();
			this.lowerPanel = new System.Windows.Forms.Panel();
			this.Cancel = new System.Windows.Forms.Button();
			this.OK = new System.Windows.Forms.Button();
			this.graphProps1 = new Netron.GraphLib.UI.GraphProps();
			this.surroundPanel.SuspendLayout();
			this.upperPanel.SuspendLayout();
			this.lowerPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// surroundPanel
			// 
			this.surroundPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.surroundPanel.Controls.Add(this.upperPanel);
			this.surroundPanel.Controls.Add(this.lowerPanel);
			this.surroundPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.surroundPanel.ForeColor = System.Drawing.Color.SlateGray;
			this.surroundPanel.Location = new System.Drawing.Point(0, 0);
			this.surroundPanel.Name = "surroundPanel";
			this.surroundPanel.Size = new System.Drawing.Size(504, 360);
			this.surroundPanel.TabIndex = 0;
			// 
			// upperPanel
			// 
			this.upperPanel.Controls.Add(this.graphProps1);
			this.upperPanel.Controls.Add(this.xpCaption1);
			this.upperPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.upperPanel.Location = new System.Drawing.Point(0, 0);
			this.upperPanel.Name = "upperPanel";
			this.upperPanel.Size = new System.Drawing.Size(502, 318);
			this.upperPanel.TabIndex = 3;
			// 
			// xpCaption1
			// 
			this.xpCaption1.Dock = System.Windows.Forms.DockStyle.Top;
			this.xpCaption1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
			this.xpCaption1.Location = new System.Drawing.Point(0, 0);
			this.xpCaption1.Name = "xpCaption1";
			this.xpCaption1.Size = new System.Drawing.Size(502, 20);
			this.xpCaption1.TabIndex = 0;
			this.xpCaption1.Text = "Graph properties";
			// 
			// lowerPanel
			// 
			this.lowerPanel.Controls.Add(this.Cancel);
			this.lowerPanel.Controls.Add(this.OK);
			this.lowerPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.lowerPanel.Location = new System.Drawing.Point(0, 318);
			this.lowerPanel.Name = "lowerPanel";
			this.lowerPanel.Size = new System.Drawing.Size(502, 40);
			this.lowerPanel.TabIndex = 2;
			// 
			// Cancel
			// 
			this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.Cancel.Location = new System.Drawing.Point(328, 8);
			this.Cancel.Name = "Cancel";
			this.Cancel.TabIndex = 1;
			this.Cancel.Text = "Cancel";
			this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
			// 
			// OK
			// 
			this.OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.OK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.OK.Location = new System.Drawing.Point(408, 8);
			this.OK.Name = "OK";
			this.OK.TabIndex = 0;
			this.OK.Text = "OK";
			this.OK.Click += new System.EventHandler(this.OK_Click);
			// 
			// graphProps1
			// 
			this.graphProps1.BackColor = System.Drawing.Color.White;
			this.graphProps1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.graphProps1.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.graphProps1.ForeColor = System.Drawing.Color.DimGray;
			this.graphProps1.GraphInformation = ((Netron.GraphLib.GraphInformation)(resources.GetObject("graphProps1.GraphInformation")));
			this.graphProps1.Location = new System.Drawing.Point(0, 20);
			this.graphProps1.Name = "graphProps1";
			this.graphProps1.Size = new System.Drawing.Size(502, 298);
			this.graphProps1.TabIndex = 1;
			// 
			// GraphPropertiesDialog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(504, 360);
			this.Controls.Add(this.surroundPanel);
			this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.KeyPreview = true;
			this.Name = "GraphPropertiesDialog";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "GraphPropertiesDialog";
			this.surroundPanel.ResumeLayout(false);
			this.upperPanel.ResumeLayout(false);
			this.lowerPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void Cancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			Close();
		}

		private void OK_Click(object sender, System.EventArgs e)
		{
			CloseThisDialog();
		}

		private void CloseThisDialog()
		{
			this.graphProps1.Commit();
			this.DialogResult = DialogResult.OK;

			Close();
		}

	
	}
}
