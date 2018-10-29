using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Netron.Neon;
namespace Tasks
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private Netron.Neon.NTasks nTasks1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem mnuAdd;
		private System.Windows.Forms.MenuItem mnuRemoveTask;
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
			System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
																													 "Bug 213",
																													 "c:\\temp\\Form1.cs",
																													 "1025",
																													 "Waiting"}, -1);
			this.nTasks1 = new Netron.Neon.NTasks();
			this.label1 = new System.Windows.Forms.Label();
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.mnuAdd = new System.Windows.Forms.MenuItem();
			this.mnuRemoveTask = new System.Windows.Forms.MenuItem();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// nTasks1
			// 
			this.nTasks1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.nTasks1.CheckBoxes = true;
			this.nTasks1.ContextMenu = this.contextMenu1;
			this.nTasks1.FullRowSelect = true;
			this.nTasks1.GridLines = true;
			listViewItem1.StateImageIndex = 0;
			this.nTasks1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
																					listViewItem1});
			this.nTasks1.Location = new System.Drawing.Point(248, 8);
			this.nTasks1.Name = "nTasks1";
			this.nTasks1.Size = new System.Drawing.Size(400, 256);
			this.nTasks1.TabIndex = 0;
			this.nTasks1.View = System.Windows.Forms.View.Details;
			this.nTasks1.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.nTasks1_ItemCheck);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(216, 23);
			this.label1.TabIndex = 1;
			this.label1.Text = "Double-click the items to edit them.";
			// 
			// contextMenu1
			// 
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.mnuAdd,
																						 this.mnuRemoveTask});
			// 
			// mnuAdd
			// 
			this.mnuAdd.Index = 0;
			this.mnuAdd.Text = "Add task";
			this.mnuAdd.Click += new System.EventHandler(this.mnuAdd_Click);
			// 
			// mnuRemoveTask
			// 
			this.mnuRemoveTask.Index = 1;
			this.mnuRemoveTask.Text = "Remove task";
			this.mnuRemoveTask.Click += new System.EventHandler(this.mnuRemoveTask_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(216, 112);
			this.label2.TabIndex = 2;
			this.label2.Text = "The context menu is set at design time and various event can be handled. For exam" +
				"ple, the ItemCheck event changes the Strikeout property. See the code for more d" +
				"etails.";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(664, 273);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.nTasks1);
			this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "Form1";
			this.Text = "Tasks example";
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

		private void mnuAdd_Click(object sender, System.EventArgs e)
		{
			NTask task = new NTask("New task", true,"Some file","1230", "Pending", Color.SteelBlue);
			this.nTasks1.AddTask(task);
		}

		private void mnuRemoveTask_Click(object sender, System.EventArgs e)
		{
			this.nTasks1.RemoveSelected();
		}

		private void nTasks1_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{
			if(e.CurrentValue==CheckState.Checked)
				this.nTasks1.Items[e.Index].Font = new Font(this.nTasks1.Items[e.Index].Font,FontStyle.Regular);
			else
				this.nTasks1.Items[e.Index].Font = new Font(this.nTasks1.Items[e.Index].Font,FontStyle.Strikeout);
		}
	}
}
