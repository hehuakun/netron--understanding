using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Netron.Neon;
namespace Diverse
{
	/// <summary>
	/// Summary description for WindowsPopup.
	/// </summary>
	public class WindowsPopup : PopupForm
	{
		private System.Windows.Forms.RichTextBox richTextBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public WindowsPopup()
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
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// richTextBox1
			// 
			this.richTextBox1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(2)));
			this.richTextBox1.Location = new System.Drawing.Point(16, 16);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.Size = new System.Drawing.Size(264, 128);
			this.richTextBox1.TabIndex = 0;
			this.richTextBox1.Text = "This is RTF box, just to show that you can embed whatever control in this menu.";
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Location = new System.Drawing.Point(16, 152);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(256, 40);
			this.label1.TabIndex = 1;
			this.label1.Text = "Note that this menu inherits from the Form class and gets closed when the focus i" +
				"s lost.";
			// 
			// button1
			// 
			this.button1.BackColor = System.Drawing.Color.Transparent;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1.Location = new System.Drawing.Point(128, 208);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(144, 23);
			this.button1.TabIndex = 2;
			this.button1.Text = "Add some more text";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// WindowsPopup
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.Tan;
			this.ClientSize = new System.Drawing.Size(300, 300);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.richTextBox1);
			this.DarkColor = System.Drawing.Color.LightSlateGray;
			this.Name = "WindowsPopup";
			this.Text = "WindowsPopup";
			this.ResumeLayout(false);

		}
		#endregion

		private void button1_Click(object sender, System.EventArgs e)
		{
			this.richTextBox1.Text+=	"All events on this form keep the focus and, hence, the menu will not close unless you click outside this menu.";
		}
	}
}
