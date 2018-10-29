using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Netron.GraphLib;
namespace Netron.Cobalt
{
	/// <summary>
	/// Summary description for TemplatePropertiesDialog.
	/// </summary>
	public class TemplatePropertiesDialog : System.Windows.Forms.Form
	{

		private System.Windows.Forms.Panel surroundPanel;
		private System.Windows.Forms.Panel lowerPanel;
		private System.Windows.Forms.Button Cancel;
		private Netron.Neon.XPCaption xpCaption1;
		private System.Windows.Forms.Panel upperPanel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		public System.Windows.Forms.TextBox TemplateName;
		public System.Windows.Forms.TextBox Description;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.Button OKButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		

		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress (e);
			if(e.KeyChar == (char) Keys.Escape)
			{
				this.DialogResult = DialogResult.Cancel;
				Close();
			}
		}

	

		public TemplatePropertiesDialog()
		{
			
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
			this.surroundPanel = new System.Windows.Forms.Panel();
			this.upperPanel = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.Description = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.TemplateName = new System.Windows.Forms.TextBox();
			this.xpCaption1 = new Netron.Neon.XPCaption();
			this.lowerPanel = new System.Windows.Forms.Panel();
			this.Cancel = new System.Windows.Forms.Button();
			this.OKButton = new System.Windows.Forms.Button();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
			this.surroundPanel.SuspendLayout();
			this.upperPanel.SuspendLayout();
			this.lowerPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// surroundPanel
			// 
			this.surroundPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.surroundPanel.Controls.Add(this.upperPanel);
			this.surroundPanel.Controls.Add(this.xpCaption1);
			this.surroundPanel.Controls.Add(this.lowerPanel);
			this.surroundPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.surroundPanel.ForeColor = System.Drawing.Color.SlateGray;
			this.surroundPanel.Location = new System.Drawing.Point(0, 0);
			this.surroundPanel.Name = "surroundPanel";
			this.surroundPanel.Size = new System.Drawing.Size(352, 208);
			this.surroundPanel.TabIndex = 0;
			// 
			// upperPanel
			// 
			this.upperPanel.Controls.Add(this.label2);
			this.upperPanel.Controls.Add(this.Description);
			this.upperPanel.Controls.Add(this.label1);
			this.upperPanel.Controls.Add(this.TemplateName);
			this.upperPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.upperPanel.ForeColor = System.Drawing.Color.DimGray;
			this.upperPanel.Location = new System.Drawing.Point(0, 20);
			this.upperPanel.Name = "upperPanel";
			this.upperPanel.Size = new System.Drawing.Size(350, 146);
			this.upperPanel.TabIndex = 4;
			// 
			// label2
			// 
			this.label2.ForeColor = System.Drawing.Color.SlateGray;
			this.label2.Location = new System.Drawing.Point(16, 52);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(80, 23);
			this.label2.TabIndex = 3;
			this.label2.Text = "Description:";
			// 
			// Description
			// 
			this.Description.AcceptsReturn = true;
			this.Description.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Description.BackColor = System.Drawing.Color.WhiteSmoke;
			this.Description.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.Description.ForeColor = System.Drawing.Color.SlateGray;
			this.Description.Location = new System.Drawing.Point(103, 52);
			this.Description.Multiline = true;
			this.Description.Name = "Description";
			this.Description.Size = new System.Drawing.Size(217, 84);
			this.Description.TabIndex = 2;
			this.Description.Text = "";
			// 
			// label1
			// 
			this.label1.ForeColor = System.Drawing.Color.SlateGray;
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 23);
			this.label1.TabIndex = 1;
			this.label1.Text = "Name:";
			// 
			// TemplateName
			// 
			this.TemplateName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.TemplateName.BackColor = System.Drawing.Color.WhiteSmoke;
			this.TemplateName.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.errorProvider1.SetError(this.TemplateName, "The name cannot be empty.");
			this.TemplateName.ForeColor = System.Drawing.Color.SlateGray;
			this.errorProvider1.SetIconPadding(this.TemplateName, 7);
			this.TemplateName.Location = new System.Drawing.Point(104, 16);
			this.TemplateName.Name = "TemplateName";
			this.TemplateName.Size = new System.Drawing.Size(216, 14);
			this.TemplateName.TabIndex = 0;
			this.TemplateName.Text = "";
			this.TemplateName.Validated += new System.EventHandler(this.TemplateName_Validated);
			// 
			// xpCaption1
			// 
			this.xpCaption1.Dock = System.Windows.Forms.DockStyle.Top;
			this.xpCaption1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
			this.xpCaption1.Location = new System.Drawing.Point(0, 0);
			this.xpCaption1.Name = "xpCaption1";
			this.xpCaption1.Size = new System.Drawing.Size(350, 20);
			this.xpCaption1.TabIndex = 3;
			this.xpCaption1.Text = "New template";
			// 
			// lowerPanel
			// 
			this.lowerPanel.Controls.Add(this.Cancel);
			this.lowerPanel.Controls.Add(this.OKButton);
			this.lowerPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.lowerPanel.Location = new System.Drawing.Point(0, 166);
			this.lowerPanel.Name = "lowerPanel";
			this.lowerPanel.Size = new System.Drawing.Size(350, 40);
			this.lowerPanel.TabIndex = 2;
			// 
			// Cancel
			// 
			this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.Cancel.Location = new System.Drawing.Point(165, 8);
			this.Cancel.Name = "Cancel";
			this.Cancel.TabIndex = 1;
			this.Cancel.Text = "Cancel";
			// 
			// OKButton
			// 
			this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.OKButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.OKButton.Location = new System.Drawing.Point(245, 8);
			this.OKButton.Name = "OKButton";
			this.OKButton.TabIndex = 0;
			this.OKButton.Text = "OK";
			this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// TemplatePropertiesDialog
			// 
			this.AcceptButton = this.OKButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.BackColor = System.Drawing.Color.White;
			this.CancelButton = this.Cancel;
			this.ClientSize = new System.Drawing.Size(352, 208);
			this.Controls.Add(this.surroundPanel);
			this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "TemplatePropertiesDialog";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "TemplatePropertiesDialog";
			this.surroundPanel.ResumeLayout(false);
			this.upperPanel.ResumeLayout(false);
			this.lowerPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		

		

		private void CloseThisDialog()
		{
			this.DialogResult = DialogResult.OK;
			Close();
		}

		private void TemplateName_Validated(object sender, System.EventArgs e)
		{
			CheckValid();
		}

		private void OKButton_Click(object sender, System.EventArgs e)
		{
			if(CheckValid())	CloseThisDialog();
		}
		private bool CheckValid()
		{
			if(TemplateName.Text.Trim()==string.Empty)
			{
				errorProvider1.SetError(TemplateName,"The name cannot be empty");
				return false;
			}
			else
			{
				errorProvider1.SetError(TemplateName, "");
				return true;
			}
		}

	
	}
}
