using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Netron.Neon;
namespace TabControl
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private Netron.Neon.NTabControl nuiTabControl1;
		private Netron.Neon.NTabControl nuiTabControl2;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.TabPage tabPage4;
		private System.Windows.Forms.Label label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
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
			this.nuiTabControl1 = new Netron.Neon.NTabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.nuiTabControl2 = new Netron.Neon.NTabControl();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.label1 = new System.Windows.Forms.Label();
			this.nuiTabControl1.SuspendLayout();
			this.nuiTabControl2.SuspendLayout();
			this.SuspendLayout();
			// 
			// nuiTabControl1
			// 
			this.nuiTabControl1.AllowDrop = true;
			this.nuiTabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.nuiTabControl1.BackColor = System.Drawing.SystemColors.Control;
			this.nuiTabControl1.BellCenter = 0.5F;
			this.nuiTabControl1.BellFalloff = 1F;
			this.nuiTabControl1.BellShaped = true;
			this.nuiTabControl1.Controls.Add(this.tabPage1);
			this.nuiTabControl1.Controls.Add(this.tabPage2);
			this.nuiTabControl1.DarkColor = System.Drawing.Color.RoyalBlue;
			this.nuiTabControl1.GradientAngle = 90F;
			this.nuiTabControl1.Highlight = false;
			this.nuiTabControl1.LightColor = System.Drawing.Color.Silver;
			this.nuiTabControl1.Location = new System.Drawing.Point(16, 72);
			this.nuiTabControl1.Name = "nuiTabControl1";
			this.nuiTabControl1.SelectedIndex = 0;
			this.nuiTabControl1.Size = new System.Drawing.Size(576, 144);
			this.nuiTabControl1.TabIndex = 0;
			this.nuiTabControl1.UnselectedEdgeColor = System.Drawing.Color.Silver;
			this.nuiTabControl1.UnselectedForeColor = System.Drawing.Color.Silver;
			this.nuiTabControl1.UnselectedTabColor = System.Drawing.Color.WhiteSmoke;
			// 
			// tabPage1
			// 
			this.tabPage1.AllowDrop = true;
			this.tabPage1.BackColor = System.Drawing.Color.LightSteelBlue;
			this.tabPage1.Location = new System.Drawing.Point(4, 25);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(568, 115);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "tabPage1";
			// 
			// tabPage2
			// 
			this.tabPage2.AllowDrop = true;
			this.tabPage2.BackColor = System.Drawing.Color.RoyalBlue;
			this.tabPage2.Location = new System.Drawing.Point(4, 25);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(568, 115);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "tabPage2";
			// 
			// nuiTabControl2
			// 
			this.nuiTabControl2.AllowDrop = true;
			this.nuiTabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.nuiTabControl2.BellCenter = 0.17F;
			this.nuiTabControl2.BellFalloff = 0.6F;
			this.nuiTabControl2.BellShaped = false;
			this.nuiTabControl2.Controls.Add(this.tabPage3);
			this.nuiTabControl2.DarkColor = System.Drawing.Color.SteelBlue;
			this.nuiTabControl2.GradientAngle = 0F;
			this.nuiTabControl2.Highlight = false;
			this.nuiTabControl2.LightColor = System.Drawing.Color.WhiteSmoke;
			this.nuiTabControl2.Location = new System.Drawing.Point(16, 224);
			this.nuiTabControl2.Name = "nuiTabControl2";
			this.nuiTabControl2.SelectedIndex = 0;
			this.nuiTabControl2.Size = new System.Drawing.Size(576, 136);
			this.nuiTabControl2.TabIndex = 1;
			this.nuiTabControl2.UnselectedEdgeColor = System.Drawing.Color.Silver;
			this.nuiTabControl2.UnselectedForeColor = System.Drawing.Color.Silver;
			this.nuiTabControl2.UnselectedTabColor = System.Drawing.Color.WhiteSmoke;
			// 
			// tabPage3
			// 
			this.tabPage3.Location = new System.Drawing.Point(4, 25);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size(568, 107);
			this.tabPage3.TabIndex = 0;
			this.tabPage3.Text = "tabPage3";
			// 
			// tabPage4
			// 
			this.tabPage4.Location = new System.Drawing.Point(0, 0);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(24, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(456, 40);
			this.label1.TabIndex = 2;
			this.label1.Text = "The tabs are customizable at design time. The tab pages can be dragdropped from o" +
				"ne tabcontrol to another by ctrl-clicking them.";
			// 
			// Form1
			// 
			this.AllowDrop = true;
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(616, 381);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.nuiTabControl2);
			this.Controls.Add(this.nuiTabControl1);
			this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "Form1";
			this.Text = "Owner drawn tabcontrol";
			this.nuiTabControl1.ResumeLayout(false);
			this.nuiTabControl2.ResumeLayout(false);
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

		//The overriden DragDrop and DragEnter methods are necessary since a tabcontrol without
		//any tabpages does not fire either of the events, this is by design but unfortunate.
		protected override void OnDragDrop(DragEventArgs e)
		{
			base.OnDragDrop (e);
			if(e.Data.GetDataPresent(typeof(ATP))) 
			{
				ATP tb =(ATP)  e.Data.GetData(typeof(ATP)) ;
				if(tb.Equals(null)) return;
				Point p = PointToClient(new  Point(e.X,e.Y));
				Control ctrl = GetChildAtPoint(p);
				if(ctrl==null) return;
				if(ctrl==this.nuiTabControl1)
				{
					if(this.nuiTabControl1.TabPages.Count==0)
					{
						this.nuiTabControl1.TabPages.Add(tb.Element);		
						this.nuiTabControl1.SelectedTab = tb.Element;
					}
				}

				if(ctrl==this.nuiTabControl2)
				{
					if(this.nuiTabControl2.TabPages.Count==0)
					{
						this.nuiTabControl2.TabPages.Add(tb.Element);		
						this.nuiTabControl2.SelectedTab = tb.Element;
					}
				}
			}
		}

		protected override void OnDragEnter(DragEventArgs e)
		{
			base.OnDragEnter (e);

			if(e.Data.GetDataPresent(typeof(ATP))) 
			{
				e.Effect = DragDropEffects.Move;
			}

		}


	}
}
