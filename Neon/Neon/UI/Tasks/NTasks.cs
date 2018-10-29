using System;
using System.Windows.Forms;

using System.Drawing;
namespace  Netron.Neon
{
	/// <summary>
	/// 
	/// </summary>
	public class NTasks : ListView, INUITasks
	{
		#region Events
		public event EventHandler OnShow;
		public event TaskInfo OnTaskAdded;
		public event TaskInfo OnTaskRemoved;
		public event TaskInfo OnBeforeTaskChanged;
		public event TaskInfo OnAfterTaskChanged;

		#endregion

		#region Fields
		private System.Windows.Forms.ComboBox cmbBox;
		private System.Windows.Forms.TextBox editBox;
		private int lastX=0;
		private int lastY=0;
		private string subItemText ;
		private int subItemSelected = 0 ;
		private ListViewItem currentListItem;
		
		private System.Windows.Forms.ColumnHeader colDescription;
		private System.Windows.Forms.ColumnHeader colFile;
		private System.Windows.Forms.ColumnHeader colLine;
		private System.Windows.Forms.ColumnHeader colStatus;
	
		
		#endregion

		#region Properties
	
		/// <summary>
		/// Gets the combo box which is used as a dropdown selection 
		/// </summary>
		public ComboBox ComboBox
		{
			get{return this.cmbBox;}
		}

		#endregion

		#region Constructor
		public NTasks()
		{	
			
		}
		#endregion

		#region Methods

		protected override void InitLayout()
		{
			base.InitLayout ();
			this.SendToBack();
			base.Enabled = true;
			InitializeComponent();			
			//editBox.Show();

			
		}
		



		private void InitializeComponent()
		{
			this.cmbBox = new System.Windows.Forms.ComboBox();
			this.editBox = new System.Windows.Forms.TextBox();
			
			this.SuspendLayout();
			// 
			// cmbBox
			// 
			this.cmbBox.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.cmbBox.Items.AddRange(new object[] {
														"Handled",
														"Urgent", "Waiting"});
			this.cmbBox.Location = new System.Drawing.Point(17, 17);
			this.cmbBox.Name = "cmbBox";
			this.cmbBox.TabIndex = 0;
			this.cmbBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CmbKeyPress);
			this.cmbBox.SelectedIndexChanged += new System.EventHandler(this.CmbSelected);
			this.cmbBox.LostFocus += new System.EventHandler(this.CmbFocusOver);
			this.cmbBox.Visible = false;
			// 
			// editBox
			// 
			this.editBox.AcceptsReturn = true;
			this.editBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.editBox.Location = new System.Drawing.Point(113, 17);
			this.editBox.Name = "editBox";
			this.editBox.TabIndex = 0;
			this.editBox.Text = "";
			this.editBox.LostFocus += new System.EventHandler(this.FocusOver);
			this.editBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EditOver);
			this.editBox.Visible = false;
			// 
			// NTasks
			// 
			this.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.CheckBoxes = true;
			
			this.Controls.Add(this.cmbBox);
			this.Controls.Add(this.editBox);
			this.FullRowSelect = true;
			this.GridLines = true;
			this.View = System.Windows.Forms.View.Details;

			this.colDescription = new System.Windows.Forms.ColumnHeader();
			this.colDescription.Text = "Description";
			this.colDescription.Width = 130;
			this.colFile = new System.Windows.Forms.ColumnHeader();
			this.colFile.Text = "File";
			this.colFile.Width = 90;
			this.colLine = new System.Windows.Forms.ColumnHeader();			
			this.colLine.Text = "Line";
			this.colLine.Width = 100;
			this.colStatus = new System.Windows.Forms.ColumnHeader();
			this.colStatus.Text = "Status";
			this.colStatus.Width = 80;

			this.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					  this.colDescription,
																					  this.colFile,
																					  this.colLine,
																					  this.colStatus});


			this.ResumeLayout(false);

		}
	
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown (e);
			currentListItem = this.GetItemAt(e.X , e.Y);
			lastX = e.X ;
			lastY = e.Y ;
		}


		protected override void OnDoubleClick(EventArgs e)
		{
			base.OnDoubleClick (e);
			if(currentListItem==null) return;

			
			// Check the subitem clicked .
			try
			{
				int nStart = lastX ;
				int spos = 0 ; 
				int epos = Columns[0].Width ;
				for ( int i=0; i < this.Columns.Count ; i++)
				{
					if ( nStart > spos && nStart < epos ) 
					{
						subItemSelected = i ;
						break; 
					}
				
					spos = epos ; 
					epos += this.Columns[i+1].Width;
				}

				//Console.WriteLine("SUB ITEM SELECTED = " + currentListItem.SubItems[subItemSelected].Text);
				subItemText = currentListItem.SubItems[subItemSelected].Text ;
				
				//this causes a problem if the value is empty
				string colName = this.Columns[subItemSelected].Text ;
				RaiseBeforeTaskChanged(GetTask(currentListItem.Index));
				//TODO: can this be generalized or made available in design mode?
				if ( colName == "Status" ) 
				{
					Rectangle r = new Rectangle(spos , currentListItem.Bounds.Y , epos , currentListItem.Bounds.Bottom);
					cmbBox.Size  = new System.Drawing.Size(epos - spos , currentListItem.Bounds.Bottom-currentListItem.Bounds.Top);
					cmbBox.Location = new System.Drawing.Point(spos+2, currentListItem.Bounds.Y+1);
					cmbBox.Visible = true;
					cmbBox.Show() ;
					cmbBox.Text = subItemText;
					cmbBox.SelectAll() ;
					cmbBox.Focus();
				}
				else
				{
					
					Rectangle r = new Rectangle(spos , currentListItem.Bounds.Y , epos , currentListItem.Bounds.Bottom);
					editBox.Size  = new System.Drawing.Size(epos - spos , currentListItem.Bounds.Bottom-currentListItem.Bounds.Top);
					editBox.Location = new System.Drawing.Point(spos+2  , currentListItem.Bounds.Y+1);
					editBox.Visible = true;
					editBox.Show() ;
					editBox.Text = subItemText;
					editBox.SelectAll() ;
					editBox.Focus();
				}
				currentListItem.Checked = !currentListItem.Checked; //seems double-clicking switches the check state
			}
			catch(Exception exc)
			{

			}
		}


		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter (e);
			this.editBox.Hide();
			this.cmbBox.Hide();
		}

		
		/// <summary>
		/// Adds a task item to the tasks
		/// </summary>
		/// <param name="task">a NTask item</param>
		public void AddTask(NTask task)
		{
			
			ListViewItem item=new ListViewItem(new string[]{task.Description,task.FileName,task.LineNumber, task.Status});
			item.ForeColor = task.Color;
			item.Font = new Font(item.Font,task.Strikeout? FontStyle.Strikeout: FontStyle.Regular);
			item.Checked = task.IsChecked;
			this.Items.Add(item);
			this.Invalidate();
		}

		/// <summary>
		/// Adds a new line and sets the edit mode on the first colom
		/// </summary>
		public void NewTask()
		{
			ListViewItem item=new ListViewItem(new string[]{"","New task","",""});
			//item.ForeColor = task.Color;
			//item.Font = new Font(item.Font,task.Strikeout? FontStyle.Strikeout: FontStyle.Regular);
			
			this.Items.Add(item);
			currentListItem = item;

			//simulate double-click
			EditColumn(1);
			item.Checked=false;
			this.Invalidate();
		}
		public NTaskCollection GetTasks()
		{
			if(this.Items.Count>0)
			{
				NTaskCollection tasks=new NTaskCollection();
				for(int k=0 ; k<this.Items.Count;k++)
				{
					tasks.Add(GetTask(k));
				}
				return tasks;
			}
			else
				return null;
		}
		/// <summary>
		/// Removes the currently selected item from the list
		/// </summary>
		public void RemoveSelected()
		{
			RaiseTaskRemoved(this.GetTask(this.SelectedIndices[0]));
			if(this.SelectedItems.Count>0)
			{
				this.Items.RemoveAt(this.SelectedIndices[0]);
			}
			this.Invalidate();
			
		}

		/// <summary>
		/// Retrieves a task from the panel with the specified index.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public NTask GetTask(int index)
		{
			string icon,description,filename,linenumber, status;
			ListViewItem item = this.Items[index];
			if(item !=null)
			{
				//icon = (item.SubItems[0]==null)?"":item.SubItems[0].Text;
				description = (item.SubItems[0]==null)?"":item.SubItems[0].Text;
				filename = (item.SubItems[1]==null)?"":item.SubItems[1].Text;
				linenumber = (item.SubItems[2]==null)?"":item.SubItems[2].Text;
				status = (item.SubItems[3]==null)?"":item.SubItems[3].Text;
				return new NTask(description,false,filename,linenumber,status);
			}
			else
				return null;
		}

		public void RemoveTask(int index)
		{
		
		}
		/// <summary>
		/// Sets the control in edit mode on the specified column
		/// </summary>
		/// <param name="column"></param>
		public void EditColumn(int column)
		{
			if(column <this.Columns.Count && column>0)
			{
				lastX=0;
				for(int k=0;k<column;k++)
					lastX += this.Columns[k].Width;
				lastX+=2;
			}
			//listView_DoubleClick(this, EventArgs.Empty);
			this.Invalidate();
		}

		/// <summary>
		/// Handles the edit change when the focus gets lost
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FocusOver(object sender, System.EventArgs e)
		{
			currentListItem.SubItems[subItemSelected].Text = editBox.Text;
			editBox.Hide();
			RaiseAfterTaskChanged(GetTask(currentListItem.Index));
		}
		/// <summary>
		/// Handles the enter key event on one of the editors
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void EditOver(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if ( e.KeyChar == 13 ) 
			{
				currentListItem.SubItems[subItemSelected].Text = editBox.Text;
				editBox.Hide();
				RaiseAfterTaskChanged(GetTask(currentListItem.Index));
			}

			if ( e.KeyChar == 27 ) 
				editBox.Hide();
		}
		private void CmbKeyPress(object sender , System.Windows.Forms.KeyPressEventArgs e)
		{
			if ( e.KeyChar == 13 || e.KeyChar == 27 )
			{
				cmbBox.Hide();
			}
		}

		private void CmbSelected(object sender , System.EventArgs e)
		{
			int sel = cmbBox.SelectedIndex;
			if ( sel >= 0 )
			{
				string itemSel = cmbBox.Items[sel].ToString();
				currentListItem.SubItems[subItemSelected].Text = itemSel;
			}
			cmbBox.Hide();
		}

		private void CmbFocusOver(object sender , System.EventArgs e)
		{
			cmbBox.Hide() ;
		}
		#region Raisers
		public void RaiseShow()
		{
			if(OnShow!=null)
				OnShow(this,EventArgs.Empty);
		}

		public void RaiseTaskAdded(NTask task)
		{
			if(OnTaskAdded!=null)
				OnTaskAdded(task);
		}

		public void RaiseTaskRemoved(NTask task)
		{
			if(OnTaskRemoved!=null)
				OnTaskRemoved(task);
		}
		public void RaiseBeforeTaskChanged(NTask task)
		{
			if(OnBeforeTaskChanged!=null)
				OnBeforeTaskChanged(task);
		}
		public void RaiseAfterTaskChanged(NTask task)
		{
			if(OnAfterTaskChanged!=null)
				OnAfterTaskChanged(task);
		}

		#endregion
		#endregion
	}
}
