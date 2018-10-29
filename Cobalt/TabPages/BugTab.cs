using System;
using System.Windows.Forms;
using Netron.Neon;

namespace Netron.Cobalt
{
	/// <summary>
	/// Summary description for CodeTab.
	/// </summary>
	public class BugTab : DockContent, ICobaltTab
	{
		#region Fields
		private Mediator mediator;
		private string identifier;
		private NetronBugLogging.BugLoggerControl bugLoggerControl;
		private ContextMenu menu;
		#endregion

		#region Properties

		public TabTypes TabType
		{
			get{return TabTypes.BugTab;}
		}

		public string TabIdentifier
		{
			get{return this.identifier;}
			set{identifier = value;}
		}

		public bool EnableEditing
		{
			get{return this.bugLoggerControl.EnableEditing;}
			set{this.bugLoggerControl.EnableEditing = value;}
		}

		

		#endregion

		#region Constructor
		public BugTab(Mediator mediator)
		{
			InitializeComponent();
			this.mediator = mediator;
				
		}
		#endregion

		#region Methods

	

		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(BugTab));
			this.menu = new System.Windows.Forms.ContextMenu();
			this.bugLoggerControl = new NetronBugLogging.BugLoggerControl();
			this.SuspendLayout();
			// 
			// bugLoggerControl
			// 
			this.bugLoggerControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.bugLoggerControl.AutoScroll = true;
			this.bugLoggerControl.BugText = "";
			this.bugLoggerControl.BugTitle = "";
			this.bugLoggerControl.DockPadding.Right = 50;
			this.bugLoggerControl.EnableEditing = true;
			this.bugLoggerControl.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.bugLoggerControl.Location = new System.Drawing.Point(0, 0);
			this.bugLoggerControl.Name = "bugLoggerControl";
			this.bugLoggerControl.Size = new System.Drawing.Size(816, 350);
			this.bugLoggerControl.TabIndex = 0;
			this.bugLoggerControl.Sent += new NetronBugLogging.SentHandler(this.bugLoggerControl_Sent);
			// 
			// BugTab
			// 
			this.AccessibleDescription = "This panel allows you to report bugs and/or feedback.";
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(744, 350);
			this.Controls.Add(this.bugLoggerControl);
			this.HideOnClose = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "BugTab";
			this.ShowHint = Netron.Neon.DockState.DockRightAutoHide;
			this.TabText = "Bug report";
			this.ResumeLayout(false);

		}



		#endregion

		private void bugLoggerControl_Sent(object sender, NetronBugLogging.SentArgs e)
		{
			MessageBox.Show(e.ReturnMessage,"Returned message",MessageBoxButtons.OK,MessageBoxIcon.Information);
		}
	}
}
