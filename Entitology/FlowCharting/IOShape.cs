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
	/// <summary>
	/// A decision shape with four connectors.
	/// </summary>
	/// <remarks>The EllipseShape is used as a template to make the coding easier.</remarks>
	[Serializable]
	[Description("I/O shape")]
	[NetronGraphShape("I/O","E089D002-47A1-4655-B117-26546E8A0292","FlowCharting","Netron.GraphLib.Entitology.IOShape",
		 "An I/O shape.")]
	public class IOShape : EllipseShape, ISerializable
	{		
		#region Constructors
		/// <summary>
		/// Constructor
		/// </summary>
		public IOShape() : base(){	}
		/// <summary>
		/// This is the default constructor of the class.
		/// </summary>
		public IOShape(IGraphSite site) : base(site){}
		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected IOShape(SerializationInfo info, StreamingContext context) : base(info, context){	}
		#endregion	

		#region Methods
		/// <summary>
		/// Paints the shape of the person object in the plex. Here you can let your imagination go.
		/// MAKE IT PERFORMANT, this is a killer method called 200.000 times a minute!
		/// </summary>
		/// <param name="g">The graphics canvas onto which to paint</param>
		public override void Paint(Graphics g)
		{
			
			if(RecalculateSize)
			{
				Rectangle = new RectangleF(new PointF(Rectangle.X,Rectangle.Y),
					g.MeasureString(this.Text,Font));	
				Rectangle = System.Drawing.RectangleF.Inflate(Rectangle,10,10);
				RecalculateSize = false; //very important!
			}
			GraphicsPath path = new GraphicsPath();
			path.AddLines(new PointF[]{
													new PointF(Rectangle.X+10, Rectangle.Top),
													new PointF(Rectangle.Right+10, Rectangle.Top),
													new PointF(Rectangle.Right -10, Rectangle.Bottom),
													new PointF(Rectangle.Left-10, Rectangle.Bottom),
													new PointF(Rectangle.X+10, Rectangle.Top)
			});
			Region region = new Region(path);
			if(this.ShapeColor!=Color.Transparent)
				g.FillRegion(this.BackgroundBrush,region);
			g.DrawPath(this.Pen,path);
			if (ShowLabel)
			{
				StringFormat sf = new StringFormat();
				sf.Alignment = StringAlignment.Center;
				g.DrawString(Text, Font, TextBrush, Rectangle.X + (Rectangle.Width / 2), Rectangle.Y + Rectangle.Height/2-3, sf);
			}		
			
		}
		protected  override Brush BackgroundBrush
		{
			get
			{
				return new LinearGradientBrush(Rectangle,Color.WhiteSmoke, this.ShapeColor,LinearGradientMode.Vertical);
			}
		}

		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData (info, context);
		}
		#endregion
	}

}







		
