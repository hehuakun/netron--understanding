using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Netron.Neon;
namespace Diverse
{
	/// <summary>
	/// Summary description for FilePopup.
	/// </summary>
	public class FilePopup : PopupForm
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.LinkLabel linkLabel1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FilePopup()
		{
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
			this.label1 = new System.Windows.Forms.Label();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.ForeColor = System.Drawing.Color.Gainsboro;
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(280, 48);
			this.label1.TabIndex = 0;
			this.label1.Text = "This is an example of a generalized menu which allows you to have more than just " +
				"a link; it can contain any control.";
			// 
			// linkLabel1
			// 
			this.linkLabel1.BackColor = System.Drawing.Color.Transparent;
			this.linkLabel1.LinkColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(0)), ((System.Byte)(0)));
			this.linkLabel1.Location = new System.Drawing.Point(8, 64);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(216, 23);
			this.linkLabel1.TabIndex = 1;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "Click this link to exit the application";
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// FilePopup
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(296, 120);
			this.Controls.Add(this.linkLabel1);
			this.Controls.Add(this.label1);
			this.DarkColor = System.Drawing.Color.LightSlateGray;
			this.Name = "FilePopup";
			this.Text = "FilePopup";
			this.ResumeLayout(false);

		}
		#endregion

		private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			Application.Exit();
		}
	}
}
