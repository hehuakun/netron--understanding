using System;
using System.Windows.Forms;
using Netron.Neon;
using Netron.GraphLib;
namespace Netron.Cobalt
{
	/// <summary>
	/// The tab containing the graphcontrol.
	/// </summary>
	public class GraphTab : DockContent, ICobaltTab
	{
		#region Fields
		private Mediator mediator;
		private Netron.GraphLib.UI.GraphControl graphControl;
        private System.ComponentModel.IContainer components;
        private string identifier;
		

	
		#endregion

		#region Properties

		public Netron.GraphLib.UI.GraphControl GraphControl
		{
			get{return graphControl;}			
		}

		public TabTypes TabType
		{
			get{return TabTypes.NetronDiagram;}
		}

		public string TabIdentifier
		{
			get{return this.identifier;}
			set{identifier = value;}
		}

		

		

		#endregion

		#region Constructor
		public GraphTab(Mediator mediator)
		{
			InitializeComponent();
			this.mediator = mediator;
		
		}
		#endregion

		#region Methods

	
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			//load in the main control
			graphControl.LoadLibraries();
		}


		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GraphTab));
            this.graphControl = new Netron.GraphLib.UI.GraphControl();
            this.SuspendLayout();
            // 
            // graphControl
            // 
            this.graphControl.AllowAddConnection = true;
            this.graphControl.AllowAddShape = true;
            this.graphControl.AllowDeleteShape = true;
            this.graphControl.AllowDrop = true;
            this.graphControl.AllowMoveShape = true;
            this.graphControl.AutomataPulse = 10;
            this.graphControl.AutoScroll = true;
            this.graphControl.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.graphControl.BackgroundImagePath = null;
            this.graphControl.BackgroundType = Netron.GraphLib.CanvasBackgroundType.FlatColor;
            this.graphControl.DefaultConnectionEnd = Netron.GraphLib.ConnectionEnd.NoEnds;
            this.graphControl.DefaultConnectionPath = "Default";
            this.graphControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graphControl.DoTrack = false;
            this.graphControl.EnableContextMenu = true;
            this.graphControl.EnableLayout = false;
            this.graphControl.EnableToolTip = true;
            this.graphControl.FileName = null;
            this.graphControl.GradientBottom = System.Drawing.Color.White;
            this.graphControl.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.graphControl.GradientTop = System.Drawing.Color.LightSteelBlue;
            this.graphControl.GraphLayoutAlgorithm = Netron.GraphLib.GraphLayoutAlgorithms.SpringEmbedder;
            this.graphControl.GridSize = 20;
            this.graphControl.Location = new System.Drawing.Point(0, 0);
            this.graphControl.Name = "graphControl";
            this.graphControl.RestrictToCanvas = true;
            this.graphControl.ShowAutomataController = false;
            this.graphControl.ShowGrid = false;
            this.graphControl.Size = new System.Drawing.Size(696, 589);
            this.graphControl.Snap = false;
            this.graphControl.TabIndex = 0;
            this.graphControl.Zoom = 1F;
            this.graphControl.OnShowProperties += new Netron.GraphLib.PropertiesInfo(this.OnShowProperties);
            this.graphControl.OnInfo += new Netron.GraphLib.InfoDelegate(this.graphControl_OnInfo);
            this.graphControl.OnClear += new System.EventHandler(this.graphControl_OnClear);
            this.graphControl.OnDiagramSaved += new Netron.GraphLib.FileInfo(this.graphControl_OnDiagramSaved);
            this.graphControl.OnDiagramOpened += new Netron.GraphLib.FileInfo(this.graphControl_OnDiagramOpened);
            // 
            // GraphTab
            // 
            this.AccessibleDescription = "This panel contains the Netron graph control. Drag-drop shapes from the library o" +
    "r use the context menu to add shapes.";
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(696, 589);
            this.Controls.Add(this.graphControl);
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GraphTab";
            this.TabText = "Diagram";
            this.ResumeLayout(false);

		}

		

		#endregion
		
		#region Graphcontrol event couplings

		private void graphControl_OnClear(object sender, System.EventArgs e)
		{
			mediator.parent.SetCaption("");
			mediator.parent.AskForSaving();
		}
		/// <summary>
		/// Coupling of the OnShowProperties event of the graph control to the propertygrid
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="props"></param>
		private void OnShowProperties(object sender, object[] props)
		{
			try
			{
				mediator.ShowProperties(props);				
				mediator.Output(Environment.NewLine +  "The requested properties:  " + Environment.NewLine + props.ToString());
			}
			catch(Exception exc)
			{
				mediator.Output("The property bag of this object has thrown an exception and cannot be displayed.",OutputInfoLevels.Exception);
				mediator.Output(exc.Message,OutputInfoLevels.Exception);
				
			}
		}

		/// <summary>
		/// Handles the info sent by the graphcontrol
		/// </summary>
		/// <param name="obj"></param>
		private void graphControl_OnInfo(object obj, Netron.GraphLib.OutputInfoLevels level)
		{
			mediator.Output(obj.ToString(),level);
		}

		/// <summary>
		/// Coupling of the OnShapeAdded event. This event allows you to perform a certain action when 
		/// a new shape is added. You can disallow the addition of shapes on the control level via the 'AllowAddShape' property.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="shape"></param>
		private void OnShapeAdded(object sender, Shape shape)
		{
			mediator.Output(Environment.NewLine + "Shape '" + shape.Text + "' was added.");
		}


		/// <summary>
		/// Coupling of the OnNewConnection event. You can react to this event and return 'false' if you 
		/// disallow the new connection or 'true' to accept it.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <returns></returns>
		private bool OnNewConnection(object sender, ConnectionEventArgs e)
		{
			
			if(e.From.BelongsTo.UID.ToString().ToUpper()=="BD70BD65-EF60-4326-A5F7-D4A698297A5C") 
			{
				mediator.Output(Environment.NewLine + "The new connection from shape '" + e.From.BelongsTo.Text + "' is not accepted in this case. Note, however, that creating a connection to the shape is allowed.");
				return false;
			}
			return true;
		}

		

		#endregion

		private void graphControl_OnDiagramOpened(object sender, System.IO.FileInfo info)
		{
			mediator.parent.SetCaption(info.Name);
		}

		private void graphControl_OnDiagramSaved(object sender, System.IO.FileInfo info)
		{
			mediator.parent.SetCaption(info.Name);
		}
	}
}
