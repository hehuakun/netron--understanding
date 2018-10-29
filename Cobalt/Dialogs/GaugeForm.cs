using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Netron.Cobalt
{
	/// <summary>
	/// Summary description for Gauge.
	/// </summary>
	public class GaugeForm : System.Windows.Forms.Form
	{
		private GaugeTube gaugeTube;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public void SetPercentage(int value)
		{
			this.gaugeTube.SetPercentage(value);
		}

		public GaugeForm()
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
			this.gaugeTube = new GaugeTube();
			this.SuspendLayout();
			// 
			// gaugeTube
			// 
			this.gaugeTube.BackColor = System.Drawing.Color.Transparent;
			this.gaugeTube.Location = new System.Drawing.Point(8, 24);
			this.gaugeTube.Name = "gaugeTube";
			this.gaugeTube.Size = new System.Drawing.Size(400, 24);
			this.gaugeTube.TabIndex = 0;
			// 
			// Gauge
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.Green;
			this.ClientSize = new System.Drawing.Size(424, 72);
			this.Controls.Add(this.gaugeTube);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "Gauge";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Gauge";
			this.TransparencyKey = System.Drawing.Color.Green;
			this.ResumeLayout(false);

		}
		#endregion


	}
}
