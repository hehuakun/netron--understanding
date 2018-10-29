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

namespace Netron.GraphLib.Entitology
{
	[Serializable]
	[Description("Basic node")]
	[NetronGraphShape("Interface","2C8160FA-BE20-40ef-9AB1-E3AE1EBCCA9A","UML","Netron.GraphLib.Entitology.InterfaceShape", "A UML interface")]
	public class InterfaceShape : Shape, ISerializable
	{
		#region Fields	
		/// <summary>
		/// the left one
		/// </summary>
		private Connector leftConnector;
		/// <summary>
		/// the lollipop one
		/// </summary>
		private Connector	rightConnector;
		/// <summary>
		/// the central one
		/// </summary>
		private Connector centralConnector;
		#endregion

		#region Properties
		public override Color ShapeColor
		{
			get
			{
				return base.ShapeColor;
			}
			set
			{
				base.ShapeColor = value;
				Pen.Color = ShapeColor;				
			}
		}


		#endregion	
		
		#region Constructors
		

		/// <summary>
		/// This is the default constructor of the class.
		/// </summary>
		public InterfaceShape() : base()
		{
			Init();
		}
		public InterfaceShape(IGraphSite site) : base(site)		
		{
			Init();
		}

		private void Init()		
		{
			//set the default size
			Rectangle = new RectangleF(0, 0, 70, 40);
			//set the connectors
			leftConnector = new Connector(this, "Left", true);
			leftConnector.ConnectorLocation = ConnectorLocation.Omni;
			leftConnector.ConnectionShift = 0;
			Connectors.Add(leftConnector);		
			rightConnector = new Connector(this, "Right", true);
			rightConnector.ConnectorLocation = ConnectorLocation.Omni;
			rightConnector.ConnectionShift = 0;
			Connectors.Add(rightConnector);	
			centralConnector = new Connector(this, "Central", true);
			centralConnector.ConnectorLocation = ConnectorLocation.Omni;
			centralConnector.ConnectionShift = 0;
			Connectors.Add(centralConnector);	

			//cannot be resized
			IsResizable=false;
			ShapeColor = Color.DimGray;
			Pen.Color = ShapeColor; //the shape is not being filled, we use the shapecolor for the pen
		}
		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public InterfaceShape(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			
			this.leftConnector = info.GetValue("leftConnector", typeof(Connector)) as Connector;
			leftConnector.BelongsTo = this;
			Connectors.Add(leftConnector);
			this.rightConnector = info.GetValue("rightConnector", typeof(Connector)) as Connector;
			rightConnector.BelongsTo = this;
			Connectors.Add(rightConnector);
			this.centralConnector = info.GetValue("centralConnector", typeof(Connector)) as Connector;
			centralConnector.BelongsTo = this;
			Connectors.Add(centralConnector);



		}
		#endregion
		
		#region Methods

		public override Bitmap GetThumbnail()
		{
			Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.GraphLib.Entitology.Resources.Interface.gif");
					
			Bitmap bmp= Bitmap.FromStream(stream) as Bitmap;
			stream.Close();
			stream=null;
			return bmp;
		}

		public override void PostDeserialization()
		{
			base.PostDeserialization ();
			Pen.Color = ShapeColor;
		}


		/// <summary>
		/// Paints the shape of the person object in the plex. Here you can let your imagination go.
		/// </summary>
		/// <param name="g">The graphics canvas onto which to paint</param>
		public override void Paint(Graphics g)
		{			
			base.Paint(g);
			g.DrawLine(Pen,Rectangle.Left,Rectangle.Top+Rectangle.Height/2,Rectangle.Left + 60,Rectangle.Top+Rectangle.Height/2);
			g.DrawEllipse(Pen,Rectangle.Left+60, Rectangle.Top+Rectangle.Height/2-5,10,10);			
			if (ShowLabel)
			{
				StringFormat sf = new StringFormat();
				sf.Alignment = StringAlignment.Center;
				g.DrawString(Text, Font, TextBrush, Rectangle.Left+Rectangle.Width/2, Rectangle.Top + 3, sf);
			}		

		}

		/// <summary>
		/// Returns a floating-point point coordinates for a given connector
		/// </summary>
		/// <param name="c">A connector object</param>
		/// <returns>A floating-point pointF</returns>
		public override PointF ConnectionPoint(Connector c)
		{

			if (c == leftConnector) return new PointF(Rectangle.Left, Rectangle.Top +(Rectangle.Height*1/2));			
			else if (c == rightConnector) return new PointF(Rectangle.Right-5, Rectangle.Top +(Rectangle.Height*1/2));			
			else if (c == centralConnector) return new PointF(Rectangle.X + Rectangle.Width/2, Rectangle.Top +(Rectangle.Height*1/2));			
			return new PointF(0, 0);
		}

	
		#endregion

		#region ISerializable Members

		/// <summary>
		/// ISerializable implementation
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("leftConnector", leftConnector, typeof(Connector));
			info.AddValue("rightConnector", rightConnector, typeof(Connector));
			info.AddValue("centralConnector", centralConnector, typeof(Connector));
		}

		#endregion
	}

	
	


}