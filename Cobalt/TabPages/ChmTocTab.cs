using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Netron.Neon;
using Netron.Neon.HtmlHelp;
using Netron.Neon.HtmlHelp.UIComponents;
namespace Netron.Cobalt
{
	/// <summary>
	/// Summary description for ChmTocTab.
	/// </summary>
	public class ChmTocTab : DockContent, ICobaltTab
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private TocTree tocTree;


		private Mediator mediator; 

		private string identifier;
		public TocTree TocTree
		{
			get{return tocTree;}
		}

		public ChmTocTab(Mediator mediator)
		{
			this.mediator = mediator;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChmTocTab));
            this.tocTree = new Netron.Neon.HtmlHelp.UIComponents.TocTree();
            this.SuspendLayout();
            // 
            // tocTree
            // 
            this.tocTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tocTree.Location = new System.Drawing.Point(0, 0);
            this.tocTree.Name = "tocTree";
            this.tocTree.Padding = new System.Windows.Forms.Padding(2);
            this.tocTree.Size = new System.Drawing.Size(292, 273);
            this.tocTree.TabIndex = 0;
            this.tocTree.TocSelected += new Netron.Neon.HtmlHelp.UIComponents.TocSelectedEventHandler(this.tocTree_TocSelected);
            // 
            // ChmTocTab
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.tocTree);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ChmTocTab";
            this.Text = "ChmTocTab";
            this.ResumeLayout(false);

		}
		#endregion

		private void tocTree_TocSelected(object sender, TocEventArgs e)
		{
			mediator.Navigate(e.Item.Url);
		}
		#region ICobaltTab Members

		public Netron.Cobalt.TabTypes TabType
		{
			get
			{
				return TabTypes.ChmToc;
			}
		}

		public string TabIdentifier
		{
			get
			{
				return identifier;
			}
			set
			{
				identifier = value;
			}
		}

		#endregion
	}
}
