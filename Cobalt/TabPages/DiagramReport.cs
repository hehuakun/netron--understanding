using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
namespace Netron.Cobalt
{
	/// <summary>
	/// Summary description for DiagramReport.
	/// </summary>
	public class DiagramReport : System.Windows.Forms.UserControl
	{
		#region Events
		public delegate void PathInfo(string path);
		public event PathInfo LoadDiagram;
		public event PathInfo DeleteDiagram;
		public event EventHandler ReloadDirectory;
		public event EventHandler SelectDirectory;
		#endregion

		#region Fields
		private System.Windows.Forms.Label TitleLabel;
		public System.Windows.Forms.PictureBox ThumbNail;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		public System.Windows.Forms.Label Author;
		private System.Windows.Forms.Label label4;
		public System.Windows.Forms.Label Description;
		public string FilePath;
		LinearGradientBrush brushBody;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Label label3;
		public System.Windows.Forms.Label CreationDate;
		private System.Windows.Forms.Label label5;
		public System.Windows.Forms.Label FileName;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem mnuOpen;
		private System.Windows.Forms.MenuItem mnuDelete;
		private System.Windows.Forms.MenuItem mnuReload;
		private System.Windows.Forms.MenuItem mnuSelectDirectory;
		private System.ComponentModel.IContainer components;
		#endregion

		public string Title
		{
			get{return TitleLabel.Text;}
			set{TitleLabel.Text = value; toolTip1.SetToolTip(ThumbNail, value + " (click to load the diagram)");}
		}

		#region Constructor
		public DiagramReport()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			brushBody = new LinearGradientBrush(this.ClientRectangle,this.BackColor,Color.WhiteSmoke, LinearGradientMode.Horizontal);

		}
		#endregion

		#region Methods
		private void OnSelectDirectory()
		{
			if(SelectDirectory!=null)
				SelectDirectory(this, EventArgs.Empty);
		}

		private void OnReloadDirectory()
		{
			if(ReloadDirectory!=null)
				ReloadDirectory(this,EventArgs.Empty);
		}

		private void OnLoadDiagram(string path)
		{
			if(LoadDiagram!=null)
				LoadDiagram(path);
		}
		private void OnDeleteDiagram(string path)
		{
			if(DeleteDiagram!=null)
				DeleteDiagram(path);
		}

		
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
				brushBody = new LinearGradientBrush(this.ClientRectangle,value,Color.WhiteSmoke, LinearGradientMode.Horizontal);
				this.Invalidate();
			}
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

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.FillRectangle(Brushes.WhiteSmoke,this.ClientRectangle);
			e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
			Rectangle r = this.ClientRectangle;
			GraphicsPath path = new GraphicsPath();			
			path.AddArc(r.X, r.Y, 20, 20, -180, 90);			
			path.AddLine(r.X + 10, r.Y, r.X + r.Width - 10, r.Y);			
			path.AddArc(r.X + r.Width - 20, r.Y, 20, 20, -90, 90);			
			path.AddLine(r.X + r.Width, r.Y + 10, r.X + r.Width, r.Y + r.Height - 10);			
			path.AddArc(r.X + r.Width - 20, r.Y + r.Height - 20, 20, 20, 0, 90);			
			path.AddLine(r.X + r.Width - 10, r.Y + r.Height, r.X + 10, r.Y + r.Height);			
			path.AddArc(r.X, r.Y + r.Height - 20, 20, 20, 90, 90);			
			path.AddLine(r.X, r.Y + r.Height - 10, r.X, r.Y + 10);			
			
			//shadow
//			Region darkRegion = new Region(path);
//			darkRegion.Translate(5, 5);
//			e.Graphics.FillRegion(new SolidBrush(Color.FromArgb(20, Color.Black)), darkRegion);
			
			//background
			e.Graphics.Clip = new Region(path);
			e.Graphics.FillPath(brushBody, path);

			//e.Graphics.FillRectangle(brushBody, this.ClientRectangle);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged (e);
			brushBody = new LinearGradientBrush(this.ClientRectangle,this.BackColor,Color.WhiteSmoke, LinearGradientMode.Horizontal);
		}


		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.ThumbNail = new System.Windows.Forms.PictureBox();
			this.TitleLabel = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.Author = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.Description = new System.Windows.Forms.Label();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.label3 = new System.Windows.Forms.Label();
			this.CreationDate = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.FileName = new System.Windows.Forms.Label();
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.mnuOpen = new System.Windows.Forms.MenuItem();
			this.mnuDelete = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.mnuReload = new System.Windows.Forms.MenuItem();
			this.mnuSelectDirectory = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// ThumbNail
			// 
			this.ThumbNail.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ThumbNail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ThumbNail.Cursor = System.Windows.Forms.Cursors.Hand;
			this.ThumbNail.Location = new System.Drawing.Point(8, 8);
			this.ThumbNail.Name = "ThumbNail";
			this.ThumbNail.Size = new System.Drawing.Size(150, 150);
			this.ThumbNail.TabIndex = 0;
			this.ThumbNail.TabStop = false;
			this.toolTip1.SetToolTip(this.ThumbNail, "Click to load the diagram");
			this.ThumbNail.Click += new System.EventHandler(this.ThumbNail_Click);
			// 
			// TitleLabel
			// 
			this.TitleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.TitleLabel.BackColor = System.Drawing.Color.Transparent;
			this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.TitleLabel.Location = new System.Drawing.Point(216, 8);
			this.TitleLabel.Name = "TitleLabel";
			this.TitleLabel.Size = new System.Drawing.Size(472, 23);
			this.TitleLabel.TabIndex = 1;
			this.TitleLabel.Click += new System.EventHandler(this.OnClick);
			this.TitleLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDown);
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Location = new System.Drawing.Point(163, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(32, 16);
			this.label1.TabIndex = 3;
			this.label1.Text = "Title:";
			this.label1.Click += new System.EventHandler(this.OnClick);
			this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDown);
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.Location = new System.Drawing.Point(163, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(40, 16);
			this.label2.TabIndex = 5;
			this.label2.Text = "Author:";
			this.label2.Click += new System.EventHandler(this.OnClick);
			this.label2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDown);
			// 
			// Author
			// 
			this.Author.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Author.BackColor = System.Drawing.Color.Transparent;
			this.Author.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Author.Location = new System.Drawing.Point(216, 32);
			this.Author.Name = "Author";
			this.Author.Size = new System.Drawing.Size(472, 23);
			this.Author.TabIndex = 4;
			this.Author.Click += new System.EventHandler(this.OnClick);
			this.Author.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDown);
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.Color.Transparent;
			this.label4.Location = new System.Drawing.Point(163, 80);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(64, 16);
			this.label4.TabIndex = 7;
			this.label4.Text = "Description:";
			this.label4.Click += new System.EventHandler(this.OnClick);
			this.label4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDown);
			// 
			// Description
			// 
			this.Description.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Description.BackColor = System.Drawing.Color.Transparent;
			this.Description.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Description.Location = new System.Drawing.Point(240, 80);
			this.Description.Name = "Description";
			this.Description.Size = new System.Drawing.Size(472, 72);
			this.Description.TabIndex = 6;
			this.Description.Click += new System.EventHandler(this.OnClick);
			this.Description.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDown);
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.Transparent;
			this.label3.Location = new System.Drawing.Point(163, 56);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(48, 16);
			this.label3.TabIndex = 9;
			this.label3.Text = "Created:";
			this.label3.Click += new System.EventHandler(this.OnClick);
			this.label3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDown);
			// 
			// CreationDate
			// 
			this.CreationDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.CreationDate.BackColor = System.Drawing.Color.Transparent;
			this.CreationDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.CreationDate.Location = new System.Drawing.Point(216, 56);
			this.CreationDate.Name = "CreationDate";
			this.CreationDate.Size = new System.Drawing.Size(120, 16);
			this.CreationDate.TabIndex = 8;
			this.CreationDate.Click += new System.EventHandler(this.OnClick);
			this.CreationDate.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDown);
			// 
			// label5
			// 
			this.label5.BackColor = System.Drawing.Color.Transparent;
			this.label5.Location = new System.Drawing.Point(352, 56);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(56, 16);
			this.label5.TabIndex = 11;
			this.label5.Text = "File name:";
			this.label5.Click += new System.EventHandler(this.OnClick);
			this.label5.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDown);
			// 
			// FileName
			// 
			this.FileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.FileName.BackColor = System.Drawing.Color.Transparent;
			this.FileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FileName.Location = new System.Drawing.Point(416, 56);
			this.FileName.Name = "FileName";
			this.FileName.Size = new System.Drawing.Size(240, 16);
			this.FileName.TabIndex = 10;
			this.FileName.Click += new System.EventHandler(this.OnClick);
			this.FileName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDown);
			// 
			// contextMenu1
			// 
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.mnuOpen,
																						 this.mnuDelete,
																						 this.menuItem3,
																						 this.mnuReload,
																						 this.mnuSelectDirectory});
			// 
			// mnuOpen
			// 
			this.mnuOpen.Index = 0;
			this.mnuOpen.Text = "Open diagram";
			this.mnuOpen.Click += new System.EventHandler(this.mnuOpen_Click);
			// 
			// mnuDelete
			// 
			this.mnuDelete.Index = 1;
			this.mnuDelete.Text = "Delete diagram";
			this.mnuDelete.Click += new System.EventHandler(this.mnuDelete_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 2;
			this.menuItem3.Text = "-";
			// 
			// mnuReload
			// 
			this.mnuReload.Index = 3;
			this.mnuReload.Text = "Reload directory";
			this.mnuReload.Click += new System.EventHandler(this.mnuReload_Click);
			// 
			// mnuSelectDirectory
			// 
			this.mnuSelectDirectory.Index = 4;
			this.mnuSelectDirectory.Text = "Select directory";
			this.mnuSelectDirectory.Click += new System.EventHandler(this.mnuSelectDirectory_Click);
			// 
			// DiagramReport
			// 
			this.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ContextMenu = this.contextMenu1;
			this.Controls.Add(this.label5);
			this.Controls.Add(this.FileName);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.CreationDate);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.Description);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.Author);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.TitleLabel);
			this.Controls.Add(this.ThumbNail);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "DiagramReport";
			this.Size = new System.Drawing.Size(720, 162);
			this.ResumeLayout(false);

		}
		#endregion

		private void ThumbNail_Click(object sender, System.EventArgs e)
		{
			OnLoadDiagram(FilePath);					
		}

		private void mnuOpen_Click(object sender, System.EventArgs e)
		{
			OnLoadDiagram(FilePath);
		}

		private void mnuDelete_Click(object sender, System.EventArgs e)
		{
			OnDeleteDiagram(FilePath);			
		}

		private void mnuReload_Click(object sender, System.EventArgs e)
		{
			OnReloadDirectory();		
		}

		private void mnuSelectDirectory_Click(object sender, System.EventArgs e)
		{
			OnSelectDirectory();
		}

		private void OnClick(object sender, EventArgs e)
		{
			this.Select();
		}

		private void MouseDown(object sender, MouseEventArgs e)
		{
			this.Select();
		}

		#endregion

	}
}
