using System;
using System.Windows.Forms;
using Netron.Neon;

namespace Netron.Cobalt
{
	/// <summary>
	/// Summary description for CodeTab.
	/// </summary>
	public class OutputExTab : DockContent, ICobaltTab
	{
		#region Fields
		private Mediator mediator;
		private string identifier;
		private Netron.Neon.NOutput output;
		private ContextMenu menu;
		#endregion

		#region Properties

		public TabTypes TabType
		{
			get{return TabTypes.Output;}
		}

		public string TabIdentifier
		{
			get{return this.identifier;}
			set{identifier = value;}
		}

		public NOutput Output
		{
			get{return output;}			
		}

		public string OutputText
		{
			get{return this.output.Text;}
			set{this.output.Text = value;}
		}

		

		#endregion

		#region Constructor
		public OutputExTab(Mediator mediator)
		{
			InitializeComponent();
			this.mediator = mediator;
			this.output.AddChannel("Exception");
			this.Output.AddChannel("Info");
				
		}
		#endregion

		#region Methods

		private void OnClearAll(object sender, EventArgs e)
		{
			ClearAll();
		}

		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(OutputExTab));
			this.menu = new System.Windows.Forms.ContextMenu();
			this.output = new Netron.Neon.NOutput();
			this.SuspendLayout();
			// 
			// output
			// 
			this.output.BackColor = System.Drawing.Color.LightSteelBlue;
			this.output.Current = "Default";
			this.output.Dock = System.Windows.Forms.DockStyle.Fill;
			this.output.Image = null;
			this.output.Label = "";
			this.output.Location = new System.Drawing.Point(0, 0);
			this.output.Name = "output";
			this.output.Root = null;
			this.output.ShowBottomPanel = false;
			this.output.Size = new System.Drawing.Size(280, 462);
			this.output.TabIndex = 1;
			// 
			// OutputExTab
			// 
			this.AccessibleDescription = "The output-panel collects data outputted by the various parts of the application." +
				" The output is separated in three categories; use the combo-box to change betwee" +
				"n views. The content of the output can be cleared via the content-menu.";
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(280, 462);
			this.Controls.Add(this.output);
			this.HideOnClose = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "OutputExTab";
			this.ShowHint = Netron.Neon.DockState.DockBottom;
			this.TabText = "Output";
			this.ResumeLayout(false);

		}

		public void ClearAll()
		{
			this.output.Text = "";
		}

		#endregion
	}
}
