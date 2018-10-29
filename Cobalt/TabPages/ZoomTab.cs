using System;
using System.Windows.Forms;
using Netron.Neon;
using Netron.GraphLib.UI;
namespace Netron.Cobalt
{
	/// <summary>
	/// Summary description for CodeTab.
	/// </summary>
	public class ZoomTab : DockContent, ICobaltTab
	{
		#region Fields
		private Mediator mediator;
		private string identifier;
		private System.Windows.Forms.Button ZoomIn;
		private System.Windows.Forms.Button ZoomReset;
		private System.Windows.Forms.Button ZoomOut;
		private System.Windows.Forms.Label label1;
		private Netron.GraphLib.UI.Stamper stamp;
		private System.Windows.Forms.ComboBox ZoomValues;
	
		private Netron.GraphLib.UI.GraphControl graphControl;
		#endregion

		#region Properties

		public TabTypes TabType
		{
			get{return TabTypes.PropertyGrid;}
		}

		public string TabIdentifier
		{
			get{return this.identifier;}
			set{identifier = value;}
		}



		public Netron.GraphLib.UI.GraphControl GraphControl
		{
			get{return graphControl;}
			set{graphControl = value;}
		}

		public Stamper GraphStamp
		{
			get{return stamp;}
		}
		#endregion

		#region Constructor
		public ZoomTab(Mediator mediator)
		{
			InitializeComponent();
			this.mediator = mediator;
			
		}
		#endregion

		#region Methods
	
		private void ZoomValues_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			Zoom();
		}

		
		private void ZoomValues_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar == (char) Keys.Enter)
			{
				Zoom();
			}
		}

		#region Zoom
	

	
		private void Zoom()
		{
			float factor = 100F;
			try
			{
				factor = float.Parse(this.ZoomValues.Text);
			}
			catch
			{
				MessageBox.Show("The zoom value is not allowed.");
			}
			this.graphControl.Zoom=factor/100F;
			mediator.StatusBar.Text="Zoom: " + this.graphControl.Zoom*100 +"%";
			this.graphControl.Invalidate();
		}

		private void ZoomOut_Click(object sender, System.EventArgs e)
		{
			mediator.GraphControl.Zoom /= 1.505f;		
			mediator.StatusBar.Text="Zoom: " + this.graphControl.Zoom*100 +"%";
			mediator.GraphControl.Invalidate();
		}

		private void ZoomIn_Click(object sender, System.EventArgs e)
		{
			this.graphControl.Zoom *= 1.05f;		
			mediator.StatusBar.Text="Zoom: " + this.graphControl.Zoom*100 +"%";
			this.graphControl.Invalidate();
		}

		private void ZoomReset_Click(object sender, System.EventArgs e)
		{
			this.graphControl.Zoom =1.0F;		
			mediator.StatusBar.Text="Zoom: " + this.graphControl.Zoom*100 +"%";
			this.graphControl.Invalidate();
		}
		#endregion

		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ZoomTab));
			this.ZoomIn = new System.Windows.Forms.Button();
			this.ZoomReset = new System.Windows.Forms.Button();
			this.ZoomOut = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.stamp = new Netron.GraphLib.UI.Stamper();
			this.ZoomValues = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// ZoomIn
			// 
			this.ZoomIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ZoomIn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.ZoomIn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.ZoomIn.ImageIndex = 0;
			this.ZoomIn.Location = new System.Drawing.Point(176, 195);
			this.ZoomIn.Name = "ZoomIn";
			this.ZoomIn.Size = new System.Drawing.Size(96, 23);
			this.ZoomIn.TabIndex = 23;
			this.ZoomIn.Text = "Zoom in";
			this.ZoomIn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.ZoomIn.Click += new System.EventHandler(this.ZoomIn_Click);
			// 
			// ZoomReset
			// 
			this.ZoomReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ZoomReset.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.ZoomReset.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.ZoomReset.ImageIndex = 2;
			this.ZoomReset.Location = new System.Drawing.Point(176, 227);
			this.ZoomReset.Name = "ZoomReset";
			this.ZoomReset.Size = new System.Drawing.Size(96, 23);
			this.ZoomReset.TabIndex = 22;
			this.ZoomReset.Text = "Zoom reset";
			this.ZoomReset.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.ZoomReset.Click += new System.EventHandler(this.ZoomReset_Click);
			// 
			// ZoomOut
			// 
			this.ZoomOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ZoomOut.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.ZoomOut.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.ZoomOut.ImageIndex = 1;
			this.ZoomOut.Location = new System.Drawing.Point(176, 163);
			this.ZoomOut.Name = "ZoomOut";
			this.ZoomOut.Size = new System.Drawing.Size(96, 23);
			this.ZoomOut.TabIndex = 21;
			this.ZoomOut.Text = "Zoom out";
			this.ZoomOut.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.ZoomOut.Click += new System.EventHandler(this.ZoomOut_Click);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.Location = new System.Drawing.Point(8, 160);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(86, 23);
			this.label1.TabIndex = 20;
			this.label1.Text = "Zoom factor:";
			// 
			// stamp
			// 
			this.stamp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.stamp.AutoScroll = true;
			this.stamp.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.stamp.Location = new System.Drawing.Point(8, 8);
			this.stamp.Name = "stamp";
			this.stamp.Size = new System.Drawing.Size(264, 136);
			this.stamp.TabIndex = 18;
			this.stamp.Zoom = 0.2F;
			// 
			// ZoomValues
			// 
			this.ZoomValues.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ZoomValues.Items.AddRange(new object[] {
															"500",
															"400",
															"300",
															"200",
															"100",
															"75",
															"50",
															"25"});
			this.ZoomValues.Location = new System.Drawing.Point(96, 160);
			this.ZoomValues.Name = "ZoomValues";
			this.ZoomValues.Size = new System.Drawing.Size(56, 21);
			this.ZoomValues.TabIndex = 19;
			this.ZoomValues.Text = "100";
			this.ZoomValues.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ZoomValues_KeyPress);
			this.ZoomValues.SelectedIndexChanged += new System.EventHandler(this.ZoomValues_SelectedIndexChanged);
			// 
			// ZoomTab
			// 
			this.AccessibleDescription = "This panel allows you to navigate the diagram if it extends beyond the visible po" +
				"rtion. ";
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(280, 261);
			this.Controls.Add(this.ZoomIn);
			this.Controls.Add(this.ZoomReset);
			this.Controls.Add(this.ZoomOut);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.stamp);
			this.Controls.Add(this.ZoomValues);
			this.HideOnClose = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ZoomTab";
			this.TabText = "Zoom";
			this.ResumeLayout(false);

		}




		#endregion
	}
}
