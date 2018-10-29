using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
namespace Netron.GraphLib.Entitology
{
	/// <summary>
	/// A dialog to edit the properties of the ClassShape shape
	/// </summary>
	public class ClassDialog : System.Windows.Forms.Form
	{
		#region Fields

		private object mPropertyBag;
		private System.Windows.Forms.TabControl tabControl;
		/// <summary>
		/// a pointer to the class-shape this dialog is editing
		/// </summary>
		private ClassShape shape;	
		private System.Windows.Forms.ColumnHeader colName;
		private System.Windows.Forms.ColumnHeader colDataType;
		private ListViewEx lvMethods;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.ComboBox comboBox1;
		private Control[] PropertyEditors;
		private Control[] MethodEditors;
		private bool editingMode = false;
		private System.Windows.Forms.ComboBox comboBox2;
		private System.Windows.Forms.TextBox textBox2;
		private ListViewEx lvProperties;
		private System.Windows.Forms.ColumnHeader colPropName;
		private System.Windows.Forms.ColumnHeader colPropReturnType;
		private System.Windows.Forms.ColumnHeader colMethodName;
		private System.Windows.Forms.ColumnHeader colMethodReturntype;
		private System.Windows.Forms.PropertyGrid propertyGrid;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem mnuClass;
		private System.Windows.Forms.MenuItem mnuAddProperty;
		private System.Windows.Forms.MenuItem mnuAddMethod;
		private System.Windows.Forms.TabPage tabClassMethods;
		private System.Windows.Forms.ContextMenu mnuPropertiesContextMenu;
		private System.Windows.Forms.ContextMenu mnuMethodsContextMenu;
		private System.Windows.Forms.TabPage tabClassProperties;
		private System.Windows.Forms.TabPage tabGlobalProperties;
		private System.Windows.Forms.MenuItem mnuCtAddProperty;
		private System.Windows.Forms.MenuItem mnuCtAddMethod;
		private System.Windows.Forms.MenuItem mnuCtRemoveMethod;
		private System.Windows.Forms.MenuItem mnuCtRemoveProperty;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.MenuItem mnuLoadFrom;
		private System.Windows.Forms.TabPage tabLoad;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button LoadButton;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox AssemblyLocation;
		private System.Windows.Forms.TextBox TypeNamespace;
		private System.Windows.Forms.Button BrowseAssembly;
		private Point lastPosition;
		#endregion

		#region Properties
		public ClassMethodCollection Methods
		{
			get
			{
				ClassMethodCollection col = new ClassMethodCollection();
				foreach(ListViewItem item in lvMethods.Items)
				{
					col.Add(new ClassMethod(item.Text,item.SubItems[1].Text));
				}
				return col;
			}
			set
			{
				foreach(ClassMethod m in value)
				{
					lvMethods.Items.Add(new ListViewItem(new string[]{m.Name,m.DataType}));
				}
			}

		}
		public ClassPropertyCollection Properties
		{
			get
			{
				ClassPropertyCollection col = new ClassPropertyCollection();
				foreach(ListViewItem item in lvProperties.Items)
				{
					col.Add(new ClassProperty(item.Text, item.SubItems[1].Text));
				}
				return col;
			}
			set
			{
				foreach(ClassProperty p in value)
				{
					lvProperties.Items.Add(new ListViewItem(new string[]{p.Name,p.DataType}));
				}
			}

		}

		
		public ClassShape Shape
		{
			get{return shape;}
			set{shape = value;}
		}

		/// <summary>
		/// Gets or sets the property bag for the grid
		/// </summary>
		public object PropertyBag
		{
			get{return mPropertyBag;}
			set{
				mPropertyBag = value;
				this.propertyGrid.SelectedObject = mPropertyBag;
			}
		}

		public int SelectedTabIndex
		{
			set{
				if(value>=0 && value<tabControl.TabCount)
					tabControl.SelectedIndex = value;
			}
		}
		#endregion
		
		#region Constructor
		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="shape"></param>
		public ClassDialog(ClassShape shape)
		{			
			InitializeComponent();
			
			this.shape = shape;
		
			MethodEditors = new Control[] {
										textBox1,	// for column 0
										comboBox1			// for column 1
											};

			PropertyEditors = new Control[] {
											  textBox2,	// for column 0
											  comboBox2			// for column 1
												  };
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
			this.components = new System.ComponentModel.Container();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabGlobalProperties = new System.Windows.Forms.TabPage();
			this.propertyGrid = new System.Windows.Forms.PropertyGrid();
			this.tabClassProperties = new System.Windows.Forms.TabPage();
			this.comboBox2 = new System.Windows.Forms.ComboBox();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.lvProperties = new ListViewEx();
			this.colPropName = new System.Windows.Forms.ColumnHeader();
			this.colPropReturnType = new System.Windows.Forms.ColumnHeader();
			this.mnuPropertiesContextMenu = new System.Windows.Forms.ContextMenu();
			this.mnuCtAddProperty = new System.Windows.Forms.MenuItem();
			this.mnuCtRemoveProperty = new System.Windows.Forms.MenuItem();
			this.tabClassMethods = new System.Windows.Forms.TabPage();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.lvMethods = new ListViewEx();
			this.colMethodName = new System.Windows.Forms.ColumnHeader();
			this.colMethodReturntype = new System.Windows.Forms.ColumnHeader();
			this.mnuMethodsContextMenu = new System.Windows.Forms.ContextMenu();
			this.mnuCtAddMethod = new System.Windows.Forms.MenuItem();
			this.mnuCtRemoveMethod = new System.Windows.Forms.MenuItem();
			this.colName = new System.Windows.Forms.ColumnHeader();
			this.colDataType = new System.Windows.Forms.ColumnHeader();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.mnuClass = new System.Windows.Forms.MenuItem();
			this.mnuAddProperty = new System.Windows.Forms.MenuItem();
			this.mnuAddMethod = new System.Windows.Forms.MenuItem();
			this.mnuLoadFrom = new System.Windows.Forms.MenuItem();
			this.tabLoad = new System.Windows.Forms.TabPage();
			this.LoadButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.AssemblyLocation = new System.Windows.Forms.TextBox();
			this.TypeNamespace = new System.Windows.Forms.TextBox();
			this.BrowseAssembly = new System.Windows.Forms.Button();
			this.tabControl.SuspendLayout();
			this.tabGlobalProperties.SuspendLayout();
			this.tabClassProperties.SuspendLayout();
			this.tabClassMethods.SuspendLayout();
			this.tabLoad.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl
			// 
			this.tabControl.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
			this.tabControl.Controls.Add(this.tabGlobalProperties);
			this.tabControl.Controls.Add(this.tabClassProperties);
			this.tabControl.Controls.Add(this.tabClassMethods);
			this.tabControl.Controls.Add(this.tabLoad);
			this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl.Location = new System.Drawing.Point(0, 0);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(464, 276);
			this.tabControl.TabIndex = 0;
			// 
			// tabGlobalProperties
			// 
			this.tabGlobalProperties.Controls.Add(this.propertyGrid);
			this.tabGlobalProperties.Location = new System.Drawing.Point(4, 25);
			this.tabGlobalProperties.Name = "tabGlobalProperties";
			this.tabGlobalProperties.Size = new System.Drawing.Size(456, 247);
			this.tabGlobalProperties.TabIndex = 3;
			this.tabGlobalProperties.Text = "Shape Properties";
			// 
			// propertyGrid
			// 
			this.propertyGrid.CommandsVisibleIfAvailable = true;
			this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyGrid.HelpVisible = false;
			this.propertyGrid.LargeButtons = false;
			this.propertyGrid.LineColor = System.Drawing.SystemColors.ScrollBar;
			this.propertyGrid.Location = new System.Drawing.Point(0, 0);
			this.propertyGrid.Name = "propertyGrid";
			this.propertyGrid.Size = new System.Drawing.Size(456, 247);
			this.propertyGrid.TabIndex = 0;
			this.propertyGrid.Text = "propertyGrid1";
			this.propertyGrid.ToolbarVisible = false;
			this.propertyGrid.ViewBackColor = System.Drawing.SystemColors.Window;
			this.propertyGrid.ViewForeColor = System.Drawing.SystemColors.WindowText;
			// 
			// tabClassProperties
			// 
			this.tabClassProperties.Controls.Add(this.comboBox2);
			this.tabClassProperties.Controls.Add(this.textBox2);
			this.tabClassProperties.Controls.Add(this.lvProperties);
			this.tabClassProperties.Location = new System.Drawing.Point(4, 25);
			this.tabClassProperties.Name = "tabClassProperties";
			this.tabClassProperties.Size = new System.Drawing.Size(456, 247);
			this.tabClassProperties.TabIndex = 0;
			this.tabClassProperties.Text = "Class Properties";
			// 
			// comboBox2
			// 
			this.comboBox2.Items.AddRange(new object[] {
														   "int",
														   "string",
														   "bool"});
			this.comboBox2.Location = new System.Drawing.Point(384, 128);
			this.comboBox2.Name = "comboBox2";
			this.comboBox2.Size = new System.Drawing.Size(121, 20);
			this.comboBox2.TabIndex = 5;
			this.comboBox2.Text = "comboBox2";
			this.comboBox2.Visible = false;
			this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(384, 88);
			this.textBox2.Name = "textBox2";
			this.textBox2.TabIndex = 4;
			this.textBox2.Text = "textBox2";
			this.textBox2.Visible = false;
			// 
			// lvProperties
			// 
			this.lvProperties.AllowColumnReorder = true;
			this.lvProperties.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						   this.colPropName,
																						   this.colPropReturnType});
			this.lvProperties.ContextMenu = this.mnuPropertiesContextMenu;
			this.lvProperties.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvProperties.DoubleClickActivation = true;
			this.lvProperties.FullRowSelect = true;
			this.lvProperties.GridLines = true;
			this.lvProperties.Location = new System.Drawing.Point(0, 0);
			this.lvProperties.Name = "lvProperties";
			this.lvProperties.Size = new System.Drawing.Size(456, 247);
			this.lvProperties.TabIndex = 3;
			this.toolTip1.SetToolTip(this.lvProperties, "Double-click to add or edit an item");
			this.lvProperties.View = System.Windows.Forms.View.Details;
			this.lvProperties.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvProperties_MouseDown);
			this.lvProperties.SubItemEndEditing += new SubItemEndEditingEventHandler(this.lvProperties_SubItemEndEditing);
			this.lvProperties.SubItemClicked += new SubItemEventHandler(this.lvProperties_SubItemClicked);
			// 
			// colPropName
			// 
			this.colPropName.Text = "Property name";
			this.colPropName.Width = 191;
			// 
			// colPropReturnType
			// 
			this.colPropReturnType.Text = "Return type";
			this.colPropReturnType.Width = 281;
			// 
			// mnuPropertiesContextMenu
			// 
			this.mnuPropertiesContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																									 this.mnuCtAddProperty,
																									 this.mnuCtRemoveProperty});
			// 
			// mnuCtAddProperty
			// 
			this.mnuCtAddProperty.Index = 0;
			this.mnuCtAddProperty.Text = "Add property";
			this.mnuCtAddProperty.Click += new System.EventHandler(this.mnuCtAddProperty_Click);
			// 
			// mnuCtRemoveProperty
			// 
			this.mnuCtRemoveProperty.Index = 1;
			this.mnuCtRemoveProperty.Text = "Remove property";
			this.mnuCtRemoveProperty.Click += new System.EventHandler(this.mnuCtRemoveProperty_Click);
			// 
			// tabClassMethods
			// 
			this.tabClassMethods.Controls.Add(this.comboBox1);
			this.tabClassMethods.Controls.Add(this.textBox1);
			this.tabClassMethods.Controls.Add(this.lvMethods);
			this.tabClassMethods.Location = new System.Drawing.Point(4, 25);
			this.tabClassMethods.Name = "tabClassMethods";
			this.tabClassMethods.Size = new System.Drawing.Size(456, 247);
			this.tabClassMethods.TabIndex = 1;
			this.tabClassMethods.Text = "Class Methods";
			// 
			// comboBox1
			// 
			this.comboBox1.Items.AddRange(new object[] {
														   "dklfgs",
														   "dsfgs",
														   "ertze"});
			this.comboBox1.Location = new System.Drawing.Point(384, 128);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(121, 20);
			this.comboBox1.TabIndex = 2;
			this.comboBox1.Text = "comboBox1";
			this.comboBox1.Visible = false;
			this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(384, 88);
			this.textBox1.Name = "textBox1";
			this.textBox1.TabIndex = 1;
			this.textBox1.Text = "textBox1";
			this.textBox1.Visible = false;
			// 
			// lvMethods
			// 
			this.lvMethods.AllowColumnReorder = true;
			this.lvMethods.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						this.colMethodName,
																						this.colMethodReturntype});
			this.lvMethods.ContextMenu = this.mnuMethodsContextMenu;
			this.lvMethods.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvMethods.DoubleClickActivation = true;
			this.lvMethods.FullRowSelect = true;
			this.lvMethods.GridLines = true;
			this.lvMethods.Location = new System.Drawing.Point(0, 0);
			this.lvMethods.Name = "lvMethods";
			this.lvMethods.Size = new System.Drawing.Size(456, 247);
			this.lvMethods.TabIndex = 0;
			this.toolTip1.SetToolTip(this.lvMethods, "Double-click to add or edit an item");
			this.lvMethods.View = System.Windows.Forms.View.Details;
			this.lvMethods.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvMethods_MouseDown);
			this.lvMethods.SubItemEndEditing += new SubItemEndEditingEventHandler(this.lvMethods_SubItemEndEditing);
			this.lvMethods.SubItemClicked += new SubItemEventHandler(this.lvMethods_SubItemClicked);
			// 
			// colMethodName
			// 
			this.colMethodName.Text = "Method name";
			this.colMethodName.Width = 191;
			// 
			// colMethodReturntype
			// 
			this.colMethodReturntype.Text = "Return type";
			this.colMethodReturntype.Width = 281;
			// 
			// mnuMethodsContextMenu
			// 
			this.mnuMethodsContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																								  this.mnuCtAddMethod,
																								  this.mnuCtRemoveMethod});
			// 
			// mnuCtAddMethod
			// 
			this.mnuCtAddMethod.Index = 0;
			this.mnuCtAddMethod.Text = "Add method";
			this.mnuCtAddMethod.Click += new System.EventHandler(this.mnuCtAddMethod_Click);
			// 
			// mnuCtRemoveMethod
			// 
			this.mnuCtRemoveMethod.Index = 1;
			this.mnuCtRemoveMethod.Text = "Remove method";
			this.mnuCtRemoveMethod.Click += new System.EventHandler(this.mnuCtRemoveMethod_Click);
			// 
			// colName
			// 
			this.colName.Text = "Name";
			this.colName.Width = 171;
			// 
			// colDataType
			// 
			this.colDataType.Text = "Data type";
			this.colDataType.Width = 140;
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuClass});
			// 
			// mnuClass
			// 
			this.mnuClass.Index = 0;
			this.mnuClass.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.mnuAddProperty,
																					 this.mnuAddMethod,
																					 this.mnuLoadFrom});
			this.mnuClass.Text = "Class";
			// 
			// mnuAddProperty
			// 
			this.mnuAddProperty.Index = 0;
			this.mnuAddProperty.Text = "Add property...";
			this.mnuAddProperty.Click += new System.EventHandler(this.mnuAddProperty_Click);
			// 
			// mnuAddMethod
			// 
			this.mnuAddMethod.Index = 1;
			this.mnuAddMethod.Text = "Add method...";
			this.mnuAddMethod.Click += new System.EventHandler(this.mnuAddMethod_Click);
			// 
			// mnuLoadFrom
			// 
			this.mnuLoadFrom.Index = 2;
			this.mnuLoadFrom.Text = "Load from assembly...";
			this.mnuLoadFrom.Click += new System.EventHandler(this.mnuLoadFrom_Click);
			// 
			// tabLoad
			// 
			this.tabLoad.Controls.Add(this.BrowseAssembly);
			this.tabLoad.Controls.Add(this.TypeNamespace);
			this.tabLoad.Controls.Add(this.AssemblyLocation);
			this.tabLoad.Controls.Add(this.label2);
			this.tabLoad.Controls.Add(this.label1);
			this.tabLoad.Controls.Add(this.LoadButton);
			this.tabLoad.Location = new System.Drawing.Point(4, 25);
			this.tabLoad.Name = "tabLoad";
			this.tabLoad.Size = new System.Drawing.Size(456, 247);
			this.tabLoad.TabIndex = 4;
			this.tabLoad.Text = "Reflect";
			// 
			// LoadButton
			// 
			this.LoadButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.LoadButton.Location = new System.Drawing.Point(320, 96);
			this.LoadButton.Name = "LoadButton";
			this.LoadButton.TabIndex = 0;
			this.LoadButton.Text = "Load";
			this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 23);
			this.label1.TabIndex = 1;
			this.label1.Text = "Assembly:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 56);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 23);
			this.label2.TabIndex = 2;
			this.label2.Text = "Type:";
			// 
			// AssemblyLocation
			// 
			this.AssemblyLocation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.AssemblyLocation.Location = new System.Drawing.Point(80, 24);
			this.AssemblyLocation.Name = "AssemblyLocation";
			this.AssemblyLocation.Size = new System.Drawing.Size(312, 21);
			this.AssemblyLocation.TabIndex = 3;
			this.AssemblyLocation.Text = "";
			this.toolTip1.SetToolTip(this.AssemblyLocation, "The path to the assembly");
			// 
			// TypeNamespace
			// 
			this.TypeNamespace.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.TypeNamespace.Location = new System.Drawing.Point(80, 56);
			this.TypeNamespace.Name = "TypeNamespace";
			this.TypeNamespace.Size = new System.Drawing.Size(312, 21);
			this.TypeNamespace.TabIndex = 4;
			this.TypeNamespace.Text = "";
			this.toolTip1.SetToolTip(this.TypeNamespace, "The full namespace to the type to be reflected");
			// 
			// BrowseAssembly
			// 
			this.BrowseAssembly.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.BrowseAssembly.Location = new System.Drawing.Point(400, 24);
			this.BrowseAssembly.Name = "BrowseAssembly";
			this.BrowseAssembly.Size = new System.Drawing.Size(25, 23);
			this.BrowseAssembly.TabIndex = 5;
			this.BrowseAssembly.Text = "...";
			this.BrowseAssembly.Click += new System.EventHandler(this.BrowseAssembly_Click);
			// 
			// ClassDialog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(464, 276);
			this.Controls.Add(this.tabControl);
			this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Menu = this.mainMenu1;
			this.MinimumSize = new System.Drawing.Size(472, 300);
			this.Name = "ClassDialog";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Class defintion";
			this.tabControl.ResumeLayout(false);
			this.tabGlobalProperties.ResumeLayout(false);
			this.tabClassProperties.ResumeLayout(false);
			this.tabClassMethods.ResumeLayout(false);
			this.tabLoad.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Methods grid
		private void lvMethods_SubItemClicked(object sender, SubItemEventArgs e)
		{
			lvMethods.StartEditing(MethodEditors[e.SubItem], e.Item, e.SubItem);
			editingMode = true;
		}

		private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			lvMethods.EndEditing(true);
			
		}

		private void lvMethods_SubItemEndEditing(object sender, SubItemEndEditingEventArgs e)
		{
			
				if (e.SubItem == 3) 
				{
					
				}
			editingMode = false;
		}

		
		private void lvMethods_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(e.Clicks ==1)
			{
				lastPosition = new Point(e.X, e.Y);
			}
			else if(e.Clicks==2)
			{
				if(editingMode) return;
				ListViewItem item = null;
				lvMethods.GetSubItemAt(e.X,e.Y,out item);

				if(item==null)
				{
					lvMethods.Items.Add(new ListViewItem(new string[]{"NewMethod",""}));
				
				}
			}
		}

		#endregion

		#region Properties grid

		private void lvProperties_SubItemClicked(object sender, SubItemEventArgs e)
		{
			lvProperties.StartEditing(PropertyEditors[e.SubItem], e.Item, e.SubItem);
			editingMode = true;
		}

		private void lvProperties_SubItemEndEditing(object sender, SubItemEndEditingEventArgs e)
		{
			editingMode = false;
		}

		private void lvProperties_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(e.Clicks ==1)
			{
				lastPosition = new Point(e.X, e.Y);
			}
			else if(e.Clicks==2)
			{
				if(editingMode) return;
				ListViewItem item = null;
				lvProperties.GetSubItemAt(e.X,e.Y,out item);

				if(item==null)
				{
					lvProperties.Items.Add(new ListViewItem(new string[]{"NewProperty",""}));
				
				}
			}
		}

		private void comboBox2_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			lvProperties.EndEditing(true);
		}
		#endregion

		#region Main menu
		private void mnuAddProperty_Click(object sender, System.EventArgs e)
		{
			AddProperty();
		}		

		private void mnuAddMethod_Click(object sender, System.EventArgs e)
		{
			AddMethod();
		}
		#endregion

		#region Context menu

		

		private void mnuCtAddProperty_Click(object sender, System.EventArgs e)
		{
			AddProperty();
		}
		private void mnuCtRemoveProperty_Click(object sender, System.EventArgs e)
		{
			RemoveProperty();
		}

		private void mnuCtAddMethod_Click(object sender, System.EventArgs e)
		{
			AddMethod();
		}

		private void mnuCtRemoveMethod_Click(object sender, System.EventArgs e)
		{
			RemoveMethod();
		}
		#endregion

		#region Listview add/remove
		private void AddProperty()
		{
			tabControl.SelectedTab = tabClassProperties;
			ListViewItem item = new ListViewItem(new string[]{"NewProperty",""});
			lvProperties.Items.Add(item);
			lvProperties.StartEditing(PropertyEditors[0], item, 0);
			editingMode = true;
		}

		private void AddProperty(string name)
		{
			tabControl.SelectedTab = tabClassProperties;
			ListViewItem item = new ListViewItem(new string[]{name,""});			
			lvProperties.Items.Add(item);
			lvProperties.StartEditing(PropertyEditors[0], item, 0);
			editingMode = true;
		}
		private void AddProperty(ClassProperty prop)
		{

			tabControl.SelectedTab = tabClassProperties;
			ListViewItem item = new ListViewItem(new string[]{prop.Name,prop.DataType});			
			lvProperties.Items.Add(item);
			lvProperties.StartEditing(PropertyEditors[0], item, 0);
			editingMode = true;
		}

		private void AddMethod(string name)
		{
			tabControl.SelectedTab = tabClassMethods;
			ListViewItem item = new ListViewItem(new string[]{name,""});			
			lvMethods.Items.Add(item);
			lvMethods.StartEditing(MethodEditors[0], item, 0);
			editingMode = true;
		}
		private void AddMethod(ClassMethod method)
		{
			//drop the methods corresponding to properties or events
			if(method.Name.StartsWith("get_") || method.Name.StartsWith("set_") || method.Name.StartsWith("add_") || method.Name.StartsWith("remove_")  ) return;
			tabControl.SelectedTab = tabClassMethods;
			ListViewItem item = new ListViewItem(new string[]{method.Name,method.DataType});			
			lvMethods.Items.Add(item);
			lvMethods.StartEditing(MethodEditors[0], item, 0);
			editingMode = true;
		}

		private void AddMethod()
		{
			tabControl.SelectedTab = tabClassMethods;
			ListViewItem item = new ListViewItem(new string[]{"NewMethod",""});
			lvMethods.Items.Add(item);
			lvMethods.StartEditing(MethodEditors[0], item, 0);
			editingMode = true;
		}
		private void RemoveMethod()
		{
			ListViewItem item = null;
			lvMethods.GetSubItemAt(lastPosition.X, lastPosition.Y,out item);

			if(item!=null)
			{
				lvMethods.Items.Remove(item);
			}
		}
		private void RemoveProperty()
		{
			ListViewItem item = null;
			lvProperties.GetSubItemAt(lastPosition.X, lastPosition.Y,out item);

			if(item!=null)
			{
				lvProperties.Items.Remove(item);
			}
		}


		#endregion

		private void mnuLoadFrom_Click(object sender, System.EventArgs e)
		{
		
		}

		/// <summary>
		/// Loads the properties and methods defined in an assembly
		/// </summary>
		/// <param name="assemblyPath">the full path to the assembly</param>
		/// <param name="type">the full namespace of the type to be loaded</param>
		public void ReflectType(string assemblyPath, string type)
		{
			try
			{
				Directory.SetCurrentDirectory(Path.GetDirectoryName(Application.ExecutablePath));
				Assembly ass = Assembly.LoadFrom(assemblyPath);
				
				Type tp = ass.GetType(type,true);
				//exception thrown if not found, so we're sure tp is not null here
				this.Properties.Clear();
				this.Methods.Clear();
				PropertyInfo[] props = tp.GetProperties();
				for(int k=0; k<props.Length; k++)
					this.AddProperty(new ClassProperty(props[k]));
				MethodInfo[] meths = tp.GetMethods();
				for(int k=0; k<meths.Length; k++)
					this.AddMethod(new ClassMethod(meths[k]));
				
			}
			catch(Exception exc)
			{
				System.Diagnostics.Trace.WriteLine(exc.Message);
			}
		}
		#endregion

		private void LoadButton_Click(object sender, System.EventArgs e)
		{

			ReflectType(this.AssemblyLocation.Text.Trim(),this.TypeNamespace.Text.Trim());
		}

		private void BrowseAssembly_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.CheckFileExists = true;
			dialog.AddExtension = true;
			dialog.CheckPathExists = true;
			dialog.Filter = ".Net class library (*.dll)|*.dll|.Net executable (*.exe)|*.exe" ;
			dialog.RestoreDirectory = true;
			dialog.FilterIndex = 1;
			DialogResult res = dialog.ShowDialog(this);
			if(res==DialogResult.OK)
			{
				this.AssemblyLocation.Text = dialog.FileName;
			}

		}
	
	}
}
