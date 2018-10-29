using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Netron.Neon;
namespace Netron.Cobalt.TabPages
{
	/// <summary>
	/// A template for new tabs to the Cobalt IDE
	/// </summary>
	/// <remarks>
	/// To add a new tab to Cobalt:
	///  - Change the TabType and add eventually an additional TabTypes enum elements 
	///  - In the TabFactory add a Create method and properties to access the elements/controls you add to the tab
	///  - In the TabFactory.GetTab() method add an extra switch
	///  - In the Mediator.OnTabCall() add an extra switch to set the initial docking state
	///  - Make the tab accessible in some way in the main form/interface
	///</remarks>
	public class TemplateTab : DockContent, ICobaltTab
	{
		#region Fields		
		private System.ComponentModel.Container components = null;
		private Mediator mediator;
		private string identifier;
		#endregion

		#region Properties
		public TabTypes TabType
		{
			get{return TabTypes.Unknown;}
		}

		public string TabIdentifier
		{
			get{return this.identifier;}
			set{identifier = value;}
		}
		#endregion

		#region Constructor
		public TemplateTab()
		{
			InitializeComponent();
		}
		#endregion


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
			// 
			// TemplateTab
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(432, 366);
			this.Name = "TemplateTab";
			this.Text = "Template Tab";

		}
		#endregion


	}
}
