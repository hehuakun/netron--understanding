using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Netron.Neon;
namespace Diverse
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private Netron.Neon.NTitleBar nTitleBar1;
		private Netron.Neon.NStatusBar nStatusBar1;
		private Netron.Neon.NButton nButton1;
		private System.Windows.Forms.Button FileButton;
		private System.Windows.Forms.Button WindowButton;
		private PopupWindowHelper popupHelper;
		private PopupForm filePop;
		private PopupForm winPop;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
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
			filePop = new FilePopup();
			winPop = new WindowsPopup();
			popupHelper = new PopupWindowHelper();
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
			this.nTitleBar1 = new Netron.Neon.NTitleBar();
			this.nStatusBar1 = new Netron.Neon.NStatusBar();
			this.nButton1 = new Netron.Neon.NButton();
			this.FileButton = new System.Windows.Forms.Button();
			this.WindowButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// nTitleBar1
			// 
			this.nTitleBar1.BellCenter = 0.5F;
			this.nTitleBar1.BellFalloff = 1F;
			this.nTitleBar1.BellShaped = true;
			this.nTitleBar1.DarkColor = System.Drawing.Color.LightSlateGray;
			this.nTitleBar1.Dock = System.Windows.Forms.DockStyle.Top;
			this.nTitleBar1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.nTitleBar1.ForeColor = System.Drawing.Color.AliceBlue;
			this.nTitleBar1.GradientAngle = 0F;
			this.nTitleBar1.LightColor = System.Drawing.Color.WhiteSmoke;
			this.nTitleBar1.Location = new System.Drawing.Point(0, 0);
			this.nTitleBar1.Name = "nTitleBar1";
			this.nTitleBar1.Root = null;
			this.nTitleBar1.ShadowColor = System.Drawing.Color.Gainsboro;
			this.nTitleBar1.ShowDefaultMenu = true;
			this.nTitleBar1.ShowShadow = true;
			this.nTitleBar1.Size = new System.Drawing.Size(448, 24);
			this.nTitleBar1.TabIndex = 0;
			this.nTitleBar1.Text = "Diverse controls";
			// 
			// nStatusBar1
			// 
			this.nStatusBar1.BellCenter = 0.66F;
			this.nStatusBar1.BellFalloff = 0.67F;
			this.nStatusBar1.BellShaped = true;
			this.nStatusBar1.DarkColor = System.Drawing.Color.LightSlateGray;
			this.nStatusBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.nStatusBar1.GradientAngle = 0F;
			this.nStatusBar1.LightColor = System.Drawing.Color.WhiteSmoke;
			this.nStatusBar1.Location = new System.Drawing.Point(0, 396);
			this.nStatusBar1.Name = "nStatusBar1";
			this.nStatusBar1.Size = new System.Drawing.Size(448, 24);
			this.nStatusBar1.TabIndex = 1;
			// 
			// nButton1
			// 
			this.nButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.nButton1.BellCenter = 0.17F;
			this.nButton1.BellFalloff = 0.67F;
			this.nButton1.BellShaped = true;
			this.nButton1.DarkColor = System.Drawing.Color.LightSlateGray;
			this.nButton1.ForeColor = System.Drawing.Color.Black;
			this.nButton1.GradientAngle = 0F;
			this.nButton1.LightColor = System.Drawing.Color.WhiteSmoke;
			this.nButton1.Location = new System.Drawing.Point(4, 372);
			this.nButton1.Name = "nButton1";
			this.nButton1.Root = null;
			this.nButton1.Rounded = Netron.Neon.NButton.Rounding.Up;
			this.nButton1.Size = new System.Drawing.Size(80, 23);
			this.nButton1.TabIndex = 2;
			this.nButton1.Text = "N-Button";
			this.nButton1.Click += new System.EventHandler(this.nButton1_Click);
			// 
			// FileButton
			// 
			this.FileButton.BackColor = System.Drawing.Color.LightSlateGray;
			this.FileButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.FileButton.Location = new System.Drawing.Point(8, 24);
			this.FileButton.Name = "FileButton";
			this.FileButton.Size = new System.Drawing.Size(40, 19);
			this.FileButton.TabIndex = 3;
			this.FileButton.Text = "File";
			this.FileButton.Click += new System.EventHandler(this.FileButton_Click);
			// 
			// WindowButton
			// 
			this.WindowButton.BackColor = System.Drawing.Color.LightSlateGray;
			this.WindowButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.WindowButton.Location = new System.Drawing.Point(48, 24);
			this.WindowButton.Name = "WindowButton";
			this.WindowButton.Size = new System.Drawing.Size(72, 19);
			this.WindowButton.TabIndex = 4;
			this.WindowButton.Text = "Window";
			this.WindowButton.Click += new System.EventHandler(this.WindowButton_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 64);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(408, 32);
			this.label1.TabIndex = 5;
			this.label1.Text = "Right-click the title bar to min/max or to exit this application.";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 104);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(408, 32);
			this.label2.TabIndex = 6;
			this.label2.Text = "See in design mode the various parameters to change the look of these controls.";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ClientSize = new System.Drawing.Size(448, 420);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.WindowButton);
			this.Controls.Add(this.FileButton);
			this.Controls.Add(this.nButton1);
			this.Controls.Add(this.nStatusBar1);
			this.Controls.Add(this.nTitleBar1);
			this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MinimumSize = new System.Drawing.Size(440, 420);
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Form1";
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

		private void nButton1_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show("This is an owner-drawn but otherwise standard button");
		}

		private void FileButton_Click(object sender, System.EventArgs e)
		{
			Point p =PointToScreen( new Point(FileButton.Left, FileButton.Bottom));
			popupHelper.ShowPopup(this,filePop, p);
		}

		private void WindowButton_Click(object sender, System.EventArgs e)
		{
			Point p =PointToScreen( new Point(WindowButton.Left, WindowButton.Bottom));
			popupHelper.ShowPopup(this,winPop, p);
		}
	}
}
