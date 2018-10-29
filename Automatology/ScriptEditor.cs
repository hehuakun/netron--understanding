
using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.CodeDom.Compiler;
using System.Reflection;
using System.IO;
using Netron.GraphLib.Interfaces;
namespace Netron.AutomataShapes
{
	/// <summary>
	/// This form allows to edit and indirectly compile scripts of the scripter node
	/// </summary>
	public class ScriptEditor : System.Windows.Forms.Form
	{
		#region Fields
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem mnuOpen;
		private System.Windows.Forms.MenuItem mnuNew;
		private System.Windows.Forms.MenuItem mnuSave;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem mnuSaveAs;
		[NonSerialized] private CompilerResults results;
		private System.Windows.Forms.Panel panel1;
		internal System.Windows.Forms.Button btnOk;
		internal System.Windows.Forms.ListView lvwErrors;
		internal System.Windows.Forms.ColumnHeader ColumnHeader1;
		internal System.Windows.Forms.ColumnHeader ColumnHeader2;
		internal System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Splitter splitter1;
		internal System.Windows.Forms.TextBox txtScript;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem mnuBiomorph;
		public string FileName;
		[NonSerialized] private IScript _compiledScript = null;

		#endregion

		#region Constructor
		public ScriptEditor()
		{
			
			InitializeComponent();
		}

		#endregion

		#region Properties
		public string ScriptSource
		{
			get { return txtScript.Text; }
			set
			{
				txtScript.Text = value;
				txtScript.SelectionLength = 0;
			}
		}
		public string AssemblyName
		{
			get { return results.PathToAssembly ; }			
		}
		public IScript CompiledScript
		{
			get { return _compiledScript; }
		}


		#endregion

		#region Methods

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
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.mnuNew = new System.Windows.Forms.MenuItem();
			this.mnuOpen = new System.Windows.Forms.MenuItem();
			this.mnuSave = new System.Windows.Forms.MenuItem();
			this.mnuSaveAs = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnOk = new System.Windows.Forms.Button();
			this.lvwErrors = new System.Windows.Forms.ListView();
			this.ColumnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.ColumnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.btnCancel = new System.Windows.Forms.Button();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.txtScript = new System.Windows.Forms.TextBox();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.mnuBiomorph = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1,
																					  this.menuItem2});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuNew,
																					  this.mnuOpen,
																					  this.mnuSave,
																					  this.mnuSaveAs,
																					  this.menuItem5,
																					  this.menuItem6});
			this.menuItem1.Text = "Script";
			// 
			// mnuNew
			// 
			this.mnuNew.Index = 0;
			this.mnuNew.Text = "New";
			this.mnuNew.Click += new System.EventHandler(this.mnuNew_Click);
			// 
			// mnuOpen
			// 
			this.mnuOpen.Index = 1;
			this.mnuOpen.Text = "Open ";
			// 
			// mnuSave
			// 
			this.mnuSave.Index = 2;
			this.mnuSave.Text = "Save";
			this.mnuSave.Click += new System.EventHandler(this.mnuSave_Click);
			// 
			// mnuSaveAs
			// 
			this.mnuSaveAs.Index = 3;
			this.mnuSaveAs.Text = "Save as...";
			this.mnuSaveAs.Click += new System.EventHandler(this.mnuSaveAs_Click);
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 4;
			this.menuItem5.Text = "-";
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 5;
			this.menuItem6.Text = "Exit";
			this.menuItem6.Click += new System.EventHandler(this.menuItem6_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnOk);
			this.panel1.Controls.Add(this.lvwErrors);
			this.panel1.Controls.Add(this.btnCancel);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 297);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(652, 128);
			this.panel1.TabIndex = 7;
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.Location = new System.Drawing.Point(546, 24);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(96, 32);
			this.btnOk.TabIndex = 9;
			this.btnOk.Text = "Ok";
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// lvwErrors
			// 
			this.lvwErrors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lvwErrors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						this.ColumnHeader1,
																						this.ColumnHeader2});
			this.lvwErrors.FullRowSelect = true;
			this.lvwErrors.GridLines = true;
			this.lvwErrors.Location = new System.Drawing.Point(10, 8);
			this.lvwErrors.MultiSelect = false;
			this.lvwErrors.Name = "lvwErrors";
			this.lvwErrors.Size = new System.Drawing.Size(520, 96);
			this.lvwErrors.TabIndex = 7;
			this.lvwErrors.View = System.Windows.Forms.View.Details;
			// 
			// ColumnHeader1
			// 
			this.ColumnHeader1.Text = "Error";
			this.ColumnHeader1.Width = 456;
			// 
			// ColumnHeader2
			// 
			this.ColumnHeader2.Text = "Line";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(546, 64);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(96, 32);
			this.btnCancel.TabIndex = 8;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splitter1.Location = new System.Drawing.Point(0, 289);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(652, 8);
			this.splitter1.TabIndex = 8;
			this.splitter1.TabStop = false;
			// 
			// txtScript
			// 
			this.txtScript.AcceptsReturn = true;
			this.txtScript.AcceptsTab = true;
			this.txtScript.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtScript.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtScript.Location = new System.Drawing.Point(0, 0);
			this.txtScript.Multiline = true;
			this.txtScript.Name = "txtScript";
			this.txtScript.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtScript.Size = new System.Drawing.Size(652, 289);
			this.txtScript.TabIndex = 9;
			this.txtScript.Text = "";
			this.txtScript.WordWrap = false;
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuBiomorph,
																					  this.menuItem4});
			this.menuItem2.Text = "Examples";
			// 
			// mnuBiomorph
			// 
			this.mnuBiomorph.Index = 0;
			this.mnuBiomorph.Text = "Biomorph";
			this.mnuBiomorph.Click += new System.EventHandler(this.mnuBiomorph_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 1;
			this.menuItem4.Text = "Moire";
			// 
			// ScriptEditor
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(652, 425);
			this.Controls.Add(this.txtScript);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.panel1);
			this.Menu = this.mainMenu1;
			this.Name = "ScriptEditor";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Scripter: Edit Script";
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		

	
		private void btnOk_Click(object sender, System.EventArgs e)
		{
			
			string reference;

			Cursor = Cursors.WaitCursor;

			// Find reference
//			reference = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
//			if (!reference.EndsWith(@"\"))
//				reference += @"\";
//			reference += "interfaces.dll";
			reference="";
			// Compile script
			lvwErrors.Items.Clear();
			results = DotnetCompiler.CompileScript(ScriptSource, reference, DotnetCompiler.ScriptLanguages.CSharp);

			if (results.Errors.Count == 0)
			{
				_compiledScript = (IScript)DotnetCompiler.FindInterface(results.CompiledAssembly, "IScript");
				
				DialogResult = DialogResult.OK;this.Close();
			}
			else
			{
				ListViewItem l;

				// Add each error as a listview item with its line number
				foreach (CompilerError err in results.Errors)
				{
					l = new ListViewItem(err.ErrorText);
					l.SubItems.Add(err.Line.ToString());
					lvwErrors.Items.Add(l);
				}

	            MessageBox.Show("Compile failed with " + results.Errors.Count.ToString() + " errors.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}

			Cursor = Cursors.Default;
			
		}

		private void lvwErrors_ItemActivate(object sender, System.EventArgs e)
		{
			int l = Convert.ToInt32(lvwErrors.SelectedItems[0].SubItems[1].Text);
			int i, pos;

			if (l != 0)
			{
				i = 1;
				pos = 0;
				while (i < l)
				{
					pos = txtScript.Text.IndexOf(Environment.NewLine, pos + 1);
					i++;
				}
				txtScript.SelectionStart = pos;
				txtScript.SelectionLength = txtScript.Text.IndexOf(Environment.NewLine, pos + 1) - pos;
			}

			txtScript.Focus();
		}

		private void mnuNew_Click(object sender, System.EventArgs e)
		{
			DialogResult result=MessageBox.Show("Do you want to save before quitting?","New Script",MessageBoxButtons.YesNoCancel,MessageBoxIcon.Question);
			switch (result)
			{
				case DialogResult.Yes:
					if (FileName != null)
						SaveAs(FileName);
					else
						mnuSaveAs_Click(sender,e);
					this.txtScript.Text="";
					this.Text="Scripter: new script";
					this.FileName=null;
					break;
				case DialogResult.Cancel:
					//do nothing
					break;
				case DialogResult.No:
					this.txtScript.Text="";
					this.FileName=null;
					this.Text="Scripter: new script";					
					break;
					
			}
		}
		private void SaveAs(string FileName)
		{
			File.Delete(FileName);
			FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write);
			StreamWriter w = new StreamWriter(fs); // create a stream writer 			
			w.BaseStream.Seek(0, SeekOrigin.End); // set the file pointer to the end of file 
			w.Write(this.txtScript.Text); 
			w.Flush(); // update underlying file
			fs.Close();
			this.FileName = FileName;
		}

		private void mnuSaveAs_Click(object sender, System.EventArgs e)
		{
			SaveFileDialog fileChooser=new SaveFileDialog();
			DialogResult result =fileChooser.ShowDialog();
			string filename;
			fileChooser.CheckFileExists=false;
			if(result==DialogResult.Cancel) return;
			filename=fileChooser.FileName;
			if (filename=="" || filename==null)
				MessageBox.Show("Invalid file name","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
			else
			{
				SaveAs(filename);
			}
		}

		private void menuItem6_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void mnuSave_Click(object sender, System.EventArgs e)
		{
			if (FileName != null)
				SaveAs(FileName);
			else
				mnuSaveAs_Click(sender,e);
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
			this.Dispose();
		}

		private void mnuBiomorph_Click(object sender, System.EventArgs e)
		{
			System.IO.Stream s;
			byte[] b;
			s=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.AutomataShapes.Resources.Scripts.BiomorphScript.txt");
			try
			{
				b = new byte[Convert.ToInt32(s.Length)];
				s.Read(b, 0, Convert.ToInt32(s.Length));
				this.ScriptSource = System.Text.ASCIIEncoding.ASCII.GetString(b);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.Message);
				this.ScriptSource="";
			}
		}

		

		#endregion

	}
}












