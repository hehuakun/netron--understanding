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
	[Description("Z node")]
	[NetronGraphShape("Z node","6E92FCD0-75DF-4f8f-A5B2-2927E22F4F0F","Z-fun","Netron.GraphLib.Entitology.ZNode",
		 "A round shape with a single omni-centralConnector colored in function of its z-order.")]
	public class ZNode : Shape, ISerializable
	{
		#region Fields	
		//the only centralConnector on this shape
		private Connector centralConnector;
		private Color alphaColor;
		#endregion

		#region Properties
		/// <summary>
		/// Note that you need to re-apply the attribute if you override a tagged property
		/// </summary>
		[GraphMLData] public override int ZOrder
		{
			get
			{
				return base.ZOrder;
			}
			set
			{
				
				base.ZOrder = value;
				//we keep the values inside [0,100], the bigger the z-order the more transparent the shape will be
				int chopped = Math.Max(0,Math.Min(100-value+3,100));
				int a = Convert.ToInt32(chopped * 255f/100);
				alphaColor = Color.FromArgb(a, ShapeColor);
				Invalidate();
			}
		}

		public override Color ShapeColor
		{
			get
			{
				return base.ShapeColor;
			}
			set
			{
				base.ShapeColor = value;
				int chopped = Math.Max(0,Math.Min(100-ZOrder+3,100));
				int a = Convert.ToInt32(chopped * 255f/100);
				alphaColor = Color.FromArgb(a, value);
				Invalidate();
			}
		}


		

		protected  override Brush BackgroundBrush
		{
			get
			{
				return new SolidBrush(alphaColor);
			}
		}

		

		#endregion
			
		#region Constructors
		

		/// <summary>
		/// This is the default constructor of the class.
		/// </summary>
		public ZNode() : base()
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
		public ZNode(IGraphSite site) : base(site)		
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
		public ZNode(SerializationInfo info, StreamingContext context) : base(info, context)
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

			g.FillEllipse(BackgroundBrush, Rectangle.X, Rectangle.Y, Rectangle.Width+1, Rectangle.Height+1);
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