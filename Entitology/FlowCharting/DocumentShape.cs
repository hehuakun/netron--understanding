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
	[Description("Document shape")]
	[NetronGraphShape("Document","98836CFE-F6B6-47d1-B1A6-A8ADAC8EE931","FlowCharting","Netron.GraphLib.Entitology.DocumentShape",
		 "A document shape.")]
	public class DocumentShape : EllipseShape, ISerializable
	{		
		#region Constructors
		/// <summary>
		/// Constructor
		/// </summary>
		public DocumentShape() : base(){	}
		/// <summary>
		/// This is the default constructor of the class.
		/// </summary>
		public DocumentShape(IGraphSite site) : base(site){}
		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected DocumentShape(SerializationInfo info, StreamingContext context) : base(info, context){	}
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
			/*
			1																2
			+-----------------------------------------------+ 
			 |															  |
			 |															  |
			 |															  |
			 |												+4		  |
			+0							+5						  +3
							+6
			 */
	
			PointF[] points = new PointF[]{
											new PointF(Rectangle.X, Rectangle.Bottom-10), //0
											new PointF(Rectangle.X, Rectangle.Top), //1
											new PointF(Rectangle.Right, Rectangle.Top),//2
											new PointF(Rectangle.Right, Rectangle.Bottom-10),//3
											new PointF(Rectangle.Right - Rectangle.Width/4, Rectangle.Bottom-20),//4
											new PointF(Rectangle.X+Rectangle.Width/2, Rectangle.Bottom-10), //5
											new PointF(Rectangle.X+Rectangle.Width/4, Rectangle.Bottom)//6
										  };

			GraphicsPath path = new GraphicsPath();
			path.AddLine(points[0],points[1]);
			path.AddLine(points[1],points[2]);
			path.AddLine(points[2],points[3]);
			path.AddBezier(points[3], points[4], points[4], points[5]);
			path.AddBezier(points[5], points[6], points[6], points[0]);
			
			g.DrawPath(Pen, path);
			g.FillPath(BackgroundBrush, path);
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







		
