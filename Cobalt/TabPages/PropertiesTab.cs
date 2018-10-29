using System;
using System.Windows.Forms;
using Netron.Neon;
namespace Netron.Cobalt
{
	/// <summary>
	/// Summary description for CodeTab.
	/// </summary>
	public class PropertiesTab : DockContent, ICobaltTab
	{
		#region Fields
		private Mediator mediator;
		private System.Windows.Forms.PropertyGrid propsGrid;
		private string identifier;
		#endregion

		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(PropertiesTab));
			this.propsGrid = new System.Windows.Forms.PropertyGrid();
			this.SuspendLayout();
			// 
			// propsGrid
			// 
			this.propsGrid.CommandsVisibleIfAvailable = true;
			this.propsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propsGrid.LargeButtons = false;
			this.propsGrid.LineColor = System.Drawing.SystemColors.ScrollBar;
			this.propsGrid.Location = new System.Drawing.Point(0, 0);
			this.propsGrid.Name = "propsGrid";
			this.propsGrid.Size = new System.Drawing.Size(292, 266);
			this.propsGrid.TabIndex = 0;
			this.propsGrid.Text = "PropertyGrid";
			this.propsGrid.ViewBackColor = System.Drawing.SystemColors.Window;
			this.propsGrid.ViewForeColor = System.Drawing.SystemColors.WindowText;
			// 
			// PropertiesTab
			// 
			this.AccessibleDescription = "The property grid allows you to change properties of shapes and the canvas. Doubl" +
				"e-click a shape to edit its properties.";
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Controls.Add(this.propsGrid);
			this.HideOnClose = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "PropertiesTab";
			this.ShowHint = Netron.Neon.DockState.DockRight;
			this.ResumeLayout(false);

		}

		
		#region Properties

		public TabTypes TabType
		{
			get{return TabTypes.PropertyGrid;}
		}

		public string TabIdentifier
		{
			get{return this.identifier;}
			set{identifier = value;}
		}


		public PropertyGrid PropertyGrid
		{
			get{return propsGrid;}
		}
		

		#endregion

		#region Constructor
		public PropertiesTab(Mediator mediator)
		{
			InitializeComponent();
			this.mediator = mediator;
		}
		#endregion

		#region Methods

		#endregion
	}
}
