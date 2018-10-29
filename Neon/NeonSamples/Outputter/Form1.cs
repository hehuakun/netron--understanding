using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace Outputter
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel rightPanel;
		private Netron.Neon.NOutput nOutput1;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button AddChannel;
		private System.Windows.Forms.TextBox channelText;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button DeleteChannel;
		private System.Windows.Forms.Button AddText;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox pictures;
		private System.Windows.Forms.TextBox labelText;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button SetLabel;
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
			this.rightPanel = new System.Windows.Forms.Panel();
			this.nOutput1 = new Netron.Neon.NOutput();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label4 = new System.Windows.Forms.Label();
			this.labelText = new System.Windows.Forms.TextBox();
			this.SetLabel = new System.Windows.Forms.Button();
			this.pictures = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.AddText = new System.Windows.Forms.Button();
			this.DeleteChannel = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.channelText = new System.Windows.Forms.TextBox();
			this.AddChannel = new System.Windows.Forms.Button();
			this.rightPanel.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// rightPanel
			// 
			this.rightPanel.Controls.Add(this.nOutput1);
			this.rightPanel.Dock = System.Windows.Forms.DockStyle.Right;
			this.rightPanel.Location = new System.Drawing.Point(288, 0);
			this.rightPanel.Name = "rightPanel";
			this.rightPanel.Size = new System.Drawing.Size(152, 438);
			this.rightPanel.TabIndex = 1;
			// 
			// nOutput1
			// 
			this.nOutput1.BackColor = System.Drawing.Color.LightSteelBlue;
			this.nOutput1.Current = "Default";
			this.nOutput1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.nOutput1.Image = null;
			this.nOutput1.Label = "";
			this.nOutput1.Location = new System.Drawing.Point(0, 0);
			this.nOutput1.Name = "nOutput1";
			this.nOutput1.Root = null;
			this.nOutput1.Size = new System.Drawing.Size(152, 438);
			this.nOutput1.TabIndex = 1;
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
			this.splitter1.Location = new System.Drawing.Point(285, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 438);
			this.splitter1.TabIndex = 2;
			this.splitter1.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.label4);
			this.panel1.Controls.Add(this.labelText);
			this.panel1.Controls.Add(this.SetLabel);
			this.panel1.Controls.Add(this.pictures);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.AddText);
			this.panel1.Controls.Add(this.DeleteChannel);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.channelText);
			this.panel1.Controls.Add(this.AddChannel);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(285, 438);
			this.panel1.TabIndex = 3;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 232);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(216, 24);
			this.label4.TabIndex = 10;
			this.label4.Text = "Add a label and/or picture:";
			// 
			// labelText
			// 
			this.labelText.Location = new System.Drawing.Point(16, 264);
			this.labelText.Name = "labelText";
			this.labelText.Size = new System.Drawing.Size(128, 21);
			this.labelText.TabIndex = 9;
			this.labelText.Text = "";
			// 
			// SetLabel
			// 
			this.SetLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.SetLabel.Location = new System.Drawing.Point(152, 296);
			this.SetLabel.Name = "SetLabel";
			this.SetLabel.Size = new System.Drawing.Size(56, 23);
			this.SetLabel.TabIndex = 8;
			this.SetLabel.Text = "Set";
			this.SetLabel.Click += new System.EventHandler(this.SetLabel_Click);
			// 
			// pictures
			// 
			this.pictures.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.pictures.Items.AddRange(new object[] {
														  "None",
														  "Exclamation",
														  "Info",
														  "Question"});
			this.pictures.Location = new System.Drawing.Point(16, 296);
			this.pictures.Name = "pictures";
			this.pictures.Size = new System.Drawing.Size(128, 21);
			this.pictures.TabIndex = 7;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 72);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(216, 24);
			this.label3.TabIndex = 6;
			this.label3.Text = "Add some text to the current channel:";
			// 
			// AddText
			// 
			this.AddText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.AddText.Location = new System.Drawing.Point(152, 112);
			this.AddText.Name = "AddText";
			this.AddText.Size = new System.Drawing.Size(56, 23);
			this.AddText.TabIndex = 5;
			this.AddText.Text = "Add";
			this.AddText.Click += new System.EventHandler(this.AddText_Click);
			// 
			// DeleteChannel
			// 
			this.DeleteChannel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.DeleteChannel.Location = new System.Drawing.Point(152, 192);
			this.DeleteChannel.Name = "DeleteChannel";
			this.DeleteChannel.Size = new System.Drawing.Size(56, 23);
			this.DeleteChannel.TabIndex = 4;
			this.DeleteChannel.Text = "Delete";
			this.DeleteChannel.Click += new System.EventHandler(this.DeleteChannel_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 152);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(216, 48);
			this.label2.TabIndex = 3;
			this.label2.Text = "Delete current channel (the default channel cannot be deleted):";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(144, 23);
			this.label1.TabIndex = 2;
			this.label1.Text = "Add new channel:";
			// 
			// channelText
			// 
			this.channelText.Location = new System.Drawing.Point(16, 32);
			this.channelText.Name = "channelText";
			this.channelText.Size = new System.Drawing.Size(128, 21);
			this.channelText.TabIndex = 1;
			this.channelText.Text = "";
			// 
			// AddChannel
			// 
			this.AddChannel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.AddChannel.Location = new System.Drawing.Point(152, 32);
			this.AddChannel.Name = "AddChannel";
			this.AddChannel.Size = new System.Drawing.Size(56, 23);
			this.AddChannel.TabIndex = 0;
			this.AddChannel.Text = "Add";
			this.AddChannel.Click += new System.EventHandler(this.AddChannel_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(440, 438);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.rightPanel);
			this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "Form1";
			this.Text = "Output panel example";
			this.rightPanel.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
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

		private void DeleteChannel_Click(object sender, System.EventArgs e)
		{
			this.nOutput1.RemoveChannel(this.nOutput1.Current);
		}

		private void AddChannel_Click(object sender, System.EventArgs e)
		{
			this.nOutput1.AddChannel(this.channelText.Text.Trim());
		}

		private void AddText_Click(object sender, System.EventArgs e)
		{
			this.nOutput1.CurrentChannel.WriteLine("Some dummy text", DateTime.Now.ToLongTimeString());
		}

		private void SetLabel_Click(object sender, System.EventArgs e)
		{

			this.nOutput1.Label = this.labelText.Text.Trim();
			if(this.pictures.SelectedItem!=null)
				this.nOutput1.SetImage((Netron.Neon.OutputPicture) Enum.Parse(typeof(Netron.Neon.OutputPicture),(string) this.pictures.SelectedItem) );
		}
	}
}
