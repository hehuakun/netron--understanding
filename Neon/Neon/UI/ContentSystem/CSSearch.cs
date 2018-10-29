using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;



namespace  Netron.Neon
{
	/// <summary>
	/// Simple scrollable textbox for line-by-line output
	/// </summary>
	public class CSSearch : UserControl
	{
		private System.Windows.Forms.ListView listView;
		private System.Windows.Forms.ColumnHeader KeyCol;
		private System.Windows.Forms.ComboBox SearchCombo;
		#region Fields

		#endregion
		/// <summary>
		/// Default constructor
		/// </summary>
		public CSSearch()
		{			
			InitializeComponent();


			
		}

		

		/// <summary>
		/// Windows designer initialization
		/// </summary>
		private void InitializeComponent()
		{
			this.listView = new System.Windows.Forms.ListView();
			this.KeyCol = new System.Windows.Forms.ColumnHeader();
			this.SearchCombo = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// listView
			// 
			this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					   this.KeyCol});
			this.listView.Location = new System.Drawing.Point(8, 40);
			this.listView.Name = "listView";
			this.listView.Size = new System.Drawing.Size(272, 456);
			this.listView.TabIndex = 0;
			this.listView.View = System.Windows.Forms.View.Details;
			// 
			// KeyCol
			// 
			this.KeyCol.Text = "Key";
			this.KeyCol.Width = 271;
			// 
			// SearchCombo
			// 
			this.SearchCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.SearchCombo.Location = new System.Drawing.Point(8, 8);
			this.SearchCombo.Name = "SearchCombo";
			this.SearchCombo.Size = new System.Drawing.Size(272, 21);
			this.SearchCombo.TabIndex = 1;
			// 
			// CSSearch
			// 
			this.Controls.Add(this.SearchCombo);
			this.Controls.Add(this.listView);
			this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "CSSearch";
			this.Size = new System.Drawing.Size(288, 528);
			this.ResumeLayout(false);

		}
		
	}
}
