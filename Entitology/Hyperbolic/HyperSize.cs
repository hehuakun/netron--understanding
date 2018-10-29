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

namespace Netron.GraphLib.Entitology
{
	[Serializable]
	[Description("HyperSize")]
	[NetronGraphShape("HyperSize","036F0380-07E7-4b70-9F0D-A3014FF6C796","Hyperbolic","Netron.GraphLib.Entitology.HyperSize",
		 "A shape changing size in function of its distance to the center of the canvas.")]
	public class HyperSize : Shape, ISerializable
	{
		#region Fields	
		//the only centralConnector on this shape
		private Connector centralConnector;
		private RectangleF reccer;
		private double distance;
		private float fraction;
		#endregion

		#region Properties

		#endregion
			
		#region Constructors
		

		/// <summary>
		/// This is the default constructor of the class.
		/// </summary>
		public HyperSize() : base()
		{
			//set the default size
			Rectangle = new RectangleF(0, 0, 30, 30);
			//set the centralConnector
			centralConnector = new Connector(this, "Connector", true);
			centralConnector.ConnectorLocation = ConnectorLocation.Omni;
			Connectors.Add(centralConnector);			
			//cannot be resized
			IsResizable=false;
			ShapeColor = Color.DarkSlateBlue;
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="site"></param>
		public HyperSize(IGraphSite site) : base(site)		
		{
			//set the default size
			Rectangle = new RectangleF(0, 0, 30, 30);
			//set the centralConnector
			centralConnector = new Connector(this, "Connector", true);
			centralConnector.ConnectorLocation = ConnectorLocation.Omni;
			Connectors.Add(centralConnector);
			//cannot be resized
			IsResizable=false;
			ShapeColor = Color.DarkSlateBlue;
		}
		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public HyperSize(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			Connectors.Clear();
			this.centralConnector = info.GetValue("centralConnector",  typeof(Connector)) as Connector;
			this.centralConnector.BelongsTo = this;
			Connectors.Add(centralConnector);
		}

		#endregion
		
		#region Methods

		public override void PostDeserialization()
		{
			base.PostDeserialization ();
			//set the brush
			this.ZOrder = base.ZOrder;
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
				Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.GraphLib.BasicShapes.Resources.SimpleNode.gif");
					
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

		/// <summary>
		/// Paints the shape of the person object in the plex. Here you can let your imagination go.
		/// </summary>
		/// <param name="g">The graphics canvas onto which to paint</param>
		public override void Paint(Graphics g)
		{
			if(RecalculateSize)
			{
				Rectangle = new RectangleF(new PointF(Rectangle.X,Rectangle.Y),
											g.MeasureString(Text,Font));	
				RecalculateSize = false; //very important!
			}
			if(Site.Height==0 || Site.Width==0) return;//otherwise a division by zero
			//the distance to the center
			distance = Math.Sqrt((Rectangle.Y - Site.Height/2)*(Rectangle.Y - Site.Height/2) + (Rectangle.X - Site.Width/2)*(Rectangle.X - Site.Width/2));
			//map the distance to an alpha value and let it be zero beyond the infinity-circle
			if(distance>Math.Min(Site.Height,Site.Width))
				reccer = new RectangleF(Rectangle.X, Rectangle.Y, 7,7);
			else
			{
				fraction = Convert.ToSingle(Math.Max(Math.Min(1-distance/Math.Min(Site.Height,Site.Width), 1),0.1));
				reccer = new RectangleF(Rectangle.X, Rectangle.Y, Rectangle.Width*fraction,Rectangle.Height*fraction);
			}

			g.FillEllipse(BackgroundBrush, reccer.X, reccer.Y, reccer.Width, reccer.Height);
			//g.DrawEllipse(pen, Rectangle.X, Rectangle.Y, Rectangle.Width + 1, Rectangle.Height + 1);
			if (ShowLabel)
			{
				StringFormat sf = new StringFormat();
				sf.Alignment = StringAlignment.Center;
				g.DrawString(ZOrder.ToString(), Font, TextBrush, Rectangle.X + (Rectangle.Width / 2), Rectangle.Y + 3, sf);
			}		
			base.Paint(g);

			
		}



		/// <summary>
		/// Returns a floating-point point coordinates for a given centralConnector
		/// </summary>
		/// <param name="c">A centralConnector object</param>
		/// <returns>A floating-point pointF</returns>
		public override PointF ConnectionPoint(Connector c)
		{
			
			if (c == centralConnector) return new PointF(Rectangle.Left+Rectangle.Width/2, Rectangle.Top +Rectangle.Height/2);			
			return new PointF(0, 0);
		}

	
		#endregion

		#region ISerializable Members

		



		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);		
			info.AddValue("centralConnector", this.centralConnector, typeof(Connector));
		}

		#endregion
		
	}

	
	


}