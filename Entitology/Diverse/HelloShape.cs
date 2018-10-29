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
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Netron.GraphLib.Attributes;
using Netron.GraphLib.UI;
using Netron.GraphLib.Interfaces;
using Netron.GraphLib;
namespace Netron.GraphLib.TutorialShapes
{
	/*
	 This shape is part of the tutorial "Making custom shapes"; http://netron.sourceforge.net/wp/?page_id=73
	 Doesn't do anything in particular except showing you how to make your own custom shapes. 
	 */
	[Serializable]
	[NetronGraphShape("Hello world","B7F1DF58-D47C-4bb9-AF81-605968939129","Tutorial Shapes","Netron.GraphLib.TutorialShapes.HelloShape","A simple hello-rectangle with one connectors.")]
	public class HelloShape : Shape
	{
		#region Fields		
		/// <summary>
		/// the only connector of this shape,
		/// you could add multiple connectors, not a big deal
		/// </summary>
		private Connector TopConnector;				
		#endregion
		
		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public HelloShape() : base()
		{
			//the base rectangle on which the shape's design is based
			Rectangle = new RectangleF(0, 0, 70, 20);
			//initialize the connector
			TopConnector = new Connector(this, "Top", true);
			TopConnector.ConnectorLocation = ConnectorLocation.North;
			Connectors.Add(TopConnector);	
			//set to false if you donnot want the shape to be resizable
			IsResizable=true;
		}
	
	
		#endregion	

		#region Properties
	
		//no properties

		//Important note: if you add a standard property it will NOT automatically be visible in the 
		//propertygrid, see the propertybag mechanism on this.
	
		#endregion

		#region Methods
		/// <summary>
		/// Overrides the default bitmap used in the shape viewer
		/// Note that you need to embed the 'HelloShape.gif' resource in the assembly to be visble in the
		/// ShapeViewer. Otherwise this method will return null and the ShapeViewer will use the default 
		/// thumbnail.
		/// </summary>
		/// <returns></returns>
		public override Bitmap GetThumbnail()
		{
			Bitmap bmp=null;
			try
			{
				Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.GraphLib.BasicShapes.Resources.HelloShape.gif");
					
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
		/// MAKE IT PERFORMANT, this is a killer method called 200.000 times a minute!
		/// </summary>
		/// <param name="g">The graphics canvas onto which to paint</param>
		public override void Paint(Graphics g)
		{
			base.Paint(g);
			g.FillRectangle(Brushes.White, Rectangle.X, Rectangle.Y, Rectangle.Width + 1, Rectangle.Height + 1);
			g.DrawRectangle(Pen, Rectangle.X, Rectangle.Y, Rectangle.Width + 1, Rectangle.Height + 1);
			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			g.DrawString("Hello world!", Font, TextBrush, Rectangle.X + (Rectangle.Width / 2), Rectangle.Y + 3, sf);
			
		}

		/// <summary>
		/// Returns a floating-point point coordinates for a given connector
		/// </summary>
		/// <param name="c">A connector object</param>
		/// <returns>A floating-point pointF</returns>
		public override PointF ConnectionPoint(Connector c)
		{
			
			if (c == TopConnector) return new PointF(Rectangle.Left + (Rectangle.Width * 1/2), Rectangle.Top);
		
			return new PointF(0, 0);
			
		}
		#endregion
	}

}