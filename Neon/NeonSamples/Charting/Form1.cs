using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Netron.Neon;
namespace Charting
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private Netron.Neon.NChartingControl chartingControl1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private System.Windows.Forms.Label label1;

		private Random rnd;
		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			rnd = new Random();

			this.chartingControl1.OnDataRequest+=new Netron.Neon.NChartingControl.ChartInfo(chartingControl1_OnDataRequest);

			this.chartingControl1.Lines.Add(new ChartLine("Stream 1", Color.OrangeRed));
			this.chartingControl1.Lines.Add(new ChartLine("Stream 2", Color.SteelBlue));
			this.chartingControl1.VerticalLines = false;
			this.chartingControl1.ShowLegend = true;
			this.chartingControl1.Start(200);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
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
			this.chartingControl1 = new Netron.Neon.NChartingControl();
			this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// chartingControl1
			// 
			this.chartingControl1.BackColor = System.Drawing.Color.AliceBlue;
			this.chartingControl1.GridColor = System.Drawing.Color.LightGray;
			this.chartingControl1.HorizontalLines = true;
			this.chartingControl1.HorizontalSpacing = 20;
			this.chartingControl1.Location = new System.Drawing.Point(16, 16);
			this.chartingControl1.Name = "chartingControl1";
			this.chartingControl1.PlotPoints = 100;
			this.chartingControl1.ScalingFactor = 1F;
			this.chartingControl1.ShowLegend = false;
			this.chartingControl1.Size = new System.Drawing.Size(408, 376);
			this.chartingControl1.TabIndex = 0;
			this.chartingControl1.Text = "chartingControl1";
			this.chartingControl1.VerticalLines = true;
			this.chartingControl1.VerticalSpacing = 20;
			this.chartingControl1.OnShowProperties += new Netron.Neon.PropsInfo(this.chartingControl1_OnShowProperties);
			// 
			// propertyGrid1
			// 
			this.propertyGrid1.CommandsVisibleIfAvailable = true;
			this.propertyGrid1.LargeButtons = false;
			this.propertyGrid1.LineColor = System.Drawing.SystemColors.ScrollBar;
			this.propertyGrid1.Location = new System.Drawing.Point(440, 16);
			this.propertyGrid1.Name = "propertyGrid1";
			this.propertyGrid1.Size = new System.Drawing.Size(176, 408);
			this.propertyGrid1.TabIndex = 1;
			this.propertyGrid1.Text = "propertyGrid1";
			this.propertyGrid1.ViewBackColor = System.Drawing.SystemColors.Window;
			this.propertyGrid1.ViewForeColor = System.Drawing.SystemColors.WindowText;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 400);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(336, 23);
			this.label1.TabIndex = 2;
			this.label1.Text = "Right-click the control to access its properties.";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(624, 430);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.propertyGrid1);
			this.Controls.Add(this.chartingControl1);
			this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "Form1";
			this.Text = "Graph drawing sample";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void chartingControl1_OnDataRequest(Netron.Neon.NChartingControl charter)
		{
			charter.Data =		new float[]{ rnd.Next(50), rnd.Next(88)};
		}

		private void chartingControl1_OnShowProperties(object obj)
		{
			this.propertyGrid1.SelectedObject = obj;
		}
	}
}
