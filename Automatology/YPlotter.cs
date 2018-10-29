using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
//using System.Runtime.Serialization.Formatters.Soap;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Netron.GraphLib.Attributes;
using Netron.GraphLib.UI;
using Netron.GraphLib.Interfaces;
using Netron.GraphLib;
using System.Text;

namespace Netron.Automatology
{
	/// <summary>
	/// Simple oscilloscope representation of incoming data
	/// </summary>
	[Serializable]
	[Description("Simple Y-plotter")]
	[NetronGraphShape("Y-plotter","798E2DE8-D81A-44a3-B9D9-6C690F8F72CA","Automata","Netron.Automatology.YPlotter",
		 "Simple y-plotter.")]public class YPlotter : Shape, ISerializable
	{
		#region Fields
		/// <summary>
		/// the data to be plotted
		/// </summary>
		protected int[] PlotValues;
		/// <summary>
		/// to redraw or not, that is the question
		/// </summary>
		protected int redraw;
		/// <summary>
		/// the color used for plotting
		/// </summary>
		protected Color plotColor=Color.Black;
		/// <summary>
		/// the in connector
		/// </summary>
		protected Connector inConnector;
		/// <summary>
		/// the width of the plot
		/// </summary>
		protected int plotWindowWidth=100;
		/// <summary>
		/// the height of the plot
		/// </summary>
		protected int plotWindowHeight=30;
		/// <summary>
		/// the scaling of the data before plotting
		/// </summary>
		protected float scalingFactor=1;

		/// <summary>
		/// the pen to plot the data (is not serializable in .Net)
		/// </summary>
		[NonSerialized] protected Pen plotPen;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the width of the plot
		/// </summary>
		public int PlotWindowWidth
		{
			get{return plotWindowWidth;}
			set{plotWindowWidth=value;Resize();}
		}		
		/// <summary>
		/// Gets or sets the height of the plot
		/// </summary>
		public int PlotWindowHeight
		{
			get{return plotWindowHeight;}
			set{plotWindowHeight=value;Resize();}
		}		
		/// <summary>
		/// Gets or sets the scaling of the data before plotting
		/// </summary>
		public float ScalingFactor
		{
			get{return scalingFactor;}
			set{scalingFactor=value; this.Invalidate();}
		}
		/// <summary>
		/// Gets or sets the color of the plot
		/// </summary>
		public Color PlotColor
		{
			get{return plotColor;}
			set{plotColor=value; plotPen = new Pen(plotColor,1F); this.Invalidate();}
		}
		#endregion

		#region Constructor
		/// <summary>
		/// the ctor
		/// </summary>
		public YPlotter() : base()
		{
			Rectangle = new RectangleF(0, 0, plotWindowWidth, plotWindowHeight);
			IsResizable = false;
			inConnector = new Connector(this, "Incoming values", false);
			inConnector.ConnectorLocation = ConnectorLocation.West;
			Connectors.Add(inConnector);    
			InitEntity();
		}

		/// <summary>
		/// ISerializable implementation
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData (info, context);
			info.AddValue("inConnector", this.inConnector, typeof(Connector));

			info.AddValue("scalingFactor", this.scalingFactor);		
			info.AddValue("plotColor", this.plotColor, typeof(Color));
			info.AddValue("plotWindowWidth", this.plotWindowWidth);
			info.AddValue("plotWindowHeight", plotWindowHeight);
		}
		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected YPlotter(SerializationInfo info, StreamingContext context) : base(info, context)
		{			
			this.inConnector = info.GetValue("inConnector", typeof(Connector)) as Connector;
			this.inConnector.BelongsTo = this;
			this.Connectors.Add(inConnector);

			this.PlotWindowWidth = info.GetInt32("plotWindowWidth");
			this.PlotColor = (Color) info.GetValue("plotColor", typeof(Color));
			this.PlotWindowHeight= info.GetInt32("plotWindowHeight");
			this.scalingFactor = info.GetSingle("scalingFactor");

			PlotValues = new int[plotWindowWidth];

		}

		#endregion

		#region Methods
		/// <summary>
		/// Initializes the shape
		/// </summary>
		protected  override void InitEntity()
		{
			base.InitEntity ();
			
			PlotValues = new int[plotWindowWidth];
			redraw = 0;
			plotPen = BlackPen;
		}


		


		#region Access Property properties

		public override void AddProperties()
		{
			base.AddProperties ();

			//plot color
			Bag.Properties.Add(new PropertySpec("PlotColor",typeof(Color),"Automata","The graph line color.", Color.Black));
			//plot width
			Bag.Properties.Add(new PropertySpec("PlotWindowWidth",typeof(int),"Automata","The width of the line plot.",100));
			//plot height
			Bag.Properties.Add(new PropertySpec("PlotWindowHeight",typeof(int),"Automata","The height of the line plot.",30));
			//scaling factor
			Bag.Properties.Add(new PropertySpec("ScalingFactor",typeof(float),"Automata","A scaling factor for the Y-value.",1F));

		}

		protected override void GetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			base.GetPropertyBagValue (sender, e);
			switch(e.Property.Name)
			{
				case "PlotColor":
					e.Value = this.plotColor; break;
				case "PlotWindowWidth":
					e.Value = this.plotWindowWidth;break;
				case "PlotWindowHeight":
					e.Value = this.plotWindowHeight; break;
				case "ScalingFactor":
					e.Value = this.scalingFactor; break;

			}
		}

		protected override void SetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			base.SetPropertyBagValue (sender, e);
			switch(e.Property.Name)
			{
				case "PlotColor":
					this.PlotColor = (Color) e.Value; break;
				case "PlotWindowWidth":
					this.PlotWindowWidth=(int) e.Value; break;
				case "PlotWindowHeight":
					this.PlotWindowHeight=(int) e.Value; break;
				case "ScalingFactor":
					this.ScalingFactor = (float) e.Value; break;
			}
		}


		#endregion


		/// <summary>
		/// Returns the locations of the connectors
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public override PointF ConnectionPoint(Connector c)
		{
			RectangleF r = Rectangle;
			if (c == inConnector) return new PointF(r.Left, r.Top + (r.Height /2 )+12);
			return new PointF(0, 0);
		}

		/// <summary>
		/// Paints the shape on the canvas
		/// </summary>
		/// <param name="g"></param>
		public override void Paint(Graphics g)
		{
			StringFormat sf = new StringFormat();
			//title part
			g.FillRectangle(new SolidBrush(ShapeColor), Rectangle.X-1, Rectangle.Y-1, Rectangle.Width +2, 12 );
			sf.Alignment = StringAlignment.Center;
			g.DrawString("Y-Plotter", Font, new SolidBrush(this.TextColor), Rectangle.Left + (Rectangle.Width / 2), Rectangle.Top , sf);

			//the data part
			Color Background = IsSelected ? Color.LightSteelBlue : Color.WhiteSmoke;			
			g.FillRectangle(new SolidBrush(Background), Rectangle.X - 1, Rectangle.Y - 1+12, Rectangle.Width + 2, Rectangle.Height + 2);
			for (int i = 1; i < plotWindowWidth; i++)
				g.DrawLine(plotPen, Rectangle.X + i - 1, Rectangle.Y + (Rectangle.Height / 2) - (PlotValues[i - 1] * scalingFactor)+12,
					Rectangle.X + i,     Rectangle.Y + (Rectangle.Height / 2) - (PlotValues[i] *scalingFactor)+12);
			
		}
		/// <summary>
		/// Resizes the shape in function of the plot width and height
		/// </summary>
		private void Resize()
		{
			Rectangle=new RectangleF(this.Rectangle.Location,new SizeF(plotWindowWidth,plotWindowHeight));
			PlotValues = new int[plotWindowWidth];
		}
		/// <summary>
		/// Updates the automata to the next state
		/// </summary>
		public override void Update()
		{
			if (redraw == 0) 
			{
				Invalidate();
				redraw = 1;
			}
			redraw--;
    	
			for (int i = 1; i < plotWindowWidth; i++)
				PlotValues[i - 1] = PlotValues[i];

			PlotValues[plotWindowWidth - 1] = 0;

			if (inConnector.Receives.Count >0)
			{
				int val=Convert.ToInt32(inConnector.Receives[0]);
				PlotValues[plotWindowWidth - 1] = Math.Sign(val)*Math.Min(Math.Abs(val),plotWindowHeight/2);
			}
			inConnector.Receives.Clear();
		}
		

		/// <summary>
		/// Overrides the default thumbnail used in the shape viewer
		/// </summary>
		/// <returns></returns>
		public override Bitmap GetThumbnail()
		{
			Bitmap bmp=null;
			try
			{
				Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.Automatology.Resources.YPlotter.gif");
					
				bmp= Bitmap.FromStream(stream) as Bitmap;
				stream.Close();
				stream=null;
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.Message);
			}
			return bmp;
		}
		#endregion
	}
}
