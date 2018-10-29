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
	/// Generates systematically all the coordinates of a certain X-Y area
	/// </summary>
	[Serializable]
	[Description("XY coordinates")]
	[NetronGraphShape("XY coordinates","CA296E97-B687-4514-8AEA-C0AA3AA21EBE","Automata","Netron.Automatology.XYCoordinates","Generates XY coordinates.")]
	public class XYCoordinates : Shape, ISerializable
	{
	
		#region Fields
		/// <summary>
		/// the region covered
		/// </summary>
		int xMin=0,xMax=100,yMin=0,yMax=100;
		/// <summary>
		/// the out X connector
		/// </summary>
		private Connector XOutConnector;
		/// <summary>
		/// the out Y connector
		/// </summary>
		private Connector YOutConnector;

		/// <summary>
		/// current X-Y
		/// </summary>
		private static int currentx,currenty;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the lower X value
		/// </summary>
		public int XMin{get{return xMin;} set{xMin=value;}}
		/// <summary>
		/// Gets or sets the upper X value
		/// </summary>
		public int XMax{get{return xMax;} set{xMax=value;}}
		/// <summary>
		/// Gets or sets the lower Y value
		/// </summary>
		public int YMin{get{return yMin;} set{yMin=value;}}
		/// <summary>
		/// Gets or sets the upper Y value
		/// </summary>
		public int YMax{get{return yMax;} set{yMax=value;}}


		#endregion
		
		#region Constructor		
		/// <summary>
		/// the ctor
		/// </summary>
		public XYCoordinates():base()			
		{	
			this.IsResizable=false;
			Rectangle = new RectangleF(0, 0, 100, 30);
			
			XOutConnector=new Connector(this,"x-coordinate Output",true);
			XOutConnector.ConnectorLocation = ConnectorLocation.East;
			this.Connectors.Add(XOutConnector);

			YOutConnector=new Connector(this,"y-coordinate output",true);
			YOutConnector.ConnectorLocation = ConnectorLocation.East;
			this.Connectors.Add(YOutConnector);

			
			
		}
		/// <summary>
		/// ISerializable implementation
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData (info, context);
			info.AddValue("XOutConnector", this.XOutConnector, typeof(Connector));
			info.AddValue("YOutConnector", this.YOutConnector, typeof(Connector));
			
			info.AddValue("XMin", this.XMin);
			info.AddValue("XMax", this.XMax);

			info.AddValue("YMin", this.YMin);			
			info.AddValue("YMax", this.YMax);

			info.AddValue("currentx", currentx);
			info.AddValue("currenty", currenty);




			
		}
		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected XYCoordinates(SerializationInfo info, StreamingContext context) : base(info, context)
		{			
			XOutConnector = (Connector) info.GetValue("XOutConnector", typeof(Connector));
			XOutConnector.BelongsTo = this;
			Connectors.Add(XOutConnector);		

			YOutConnector = (Connector) info.GetValue("YOutConnector", typeof(Connector));
			YOutConnector.BelongsTo = this;
			Connectors.Add(YOutConnector);	
			
			XMin = info.GetInt32("XMin");
			XMax = info.GetInt32("XMax");

			YMin = info.GetInt32("YMin");
			YMax = info.GetInt32("YMax");
			
			currentx = info.GetInt32("currentx");
			currenty = info.GetInt32("currenty");
			
		}

		#endregion

		#region Methods

		

		/// <summary>
		/// Initializes the automata
		/// </summary>
		public override void InitAutomata()
		{
			
			//xMin=0;xMax=100;yMin=0;yMax=100;
			currentx=xMin;currenty=yMin;
		}

		/// <summary>
		/// Paints the shape on the canvas
		/// </summary>
		/// <param name="g"></param>
		public override void Paint(System.Drawing.Graphics g)
		{
			
			
			SizeF ss=g.MeasureString("XY-Coordinates",Font);

			Color Background = IsSelected ? Color.LightSteelBlue : Color.WhiteSmoke;
			g.DrawRectangle(new Pen(Color.Black,IsSelected ? 2F : 1F),Rectangle.Left,Rectangle.Top,Rectangle.Width,Rectangle.Height);
			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			
			g.FillRectangle(new SolidBrush(ShapeColor), Rectangle.X, Rectangle.Y, Rectangle.Width , 12 );
			g.FillRectangle(new SolidBrush(Background), Rectangle.X, Rectangle.Y+12, Rectangle.Width , 18 );					
			g.DrawString("XY-Coordinates", Font, new SolidBrush(TextColor), Rectangle.Left + (Rectangle.Width / 2), Rectangle.Top , sf);
			

		}
		
		/// <summary>
		/// Returns the locations of the connectors
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public override System.Drawing.PointF ConnectionPoint(Connector c)
		{
			
			if (c == XOutConnector) return new PointF(Rectangle.Right, Rectangle.Top+12+(Rectangle.Height-12)/3);
			if (c == YOutConnector) return new PointF(Rectangle.Right, Rectangle.Top+12+2*(Rectangle.Height-12)/3);
			return base.ConnectionPoint (c);
			//return new PointF(0, 0);
		}
		/// <summary>
		/// Updates the automata to the next state
		/// </summary>
		public override void Update()
		{
			//clear the sends values

			if (currentx==xMax)
			{
					
				if (currenty==yMax)
					return; //all points were delivered
				else
				{currenty++;currentx=xMin;} //back to start on x-line
						
			}
			else
				currentx++;

			
			//clear the previous output
			XOutConnector.Sends.Clear();
			YOutConnector.Sends.Clear();
			XOutConnector.Receives.Clear();
			YOutConnector.Receives.Clear();
			//deliver the new data	
			XOutConnector.Sends.Add(currentx);
			YOutConnector.Sends.Add(currenty);
				
			
			//base.Update ();

		}

		#region Access the propertygrid proerties
		public override void AddProperties()
		{
			base.AddProperties ();
			Bag.Properties.Add(new PropertySpec("XMin", typeof(int),"Automata","Gets or sets the lower X value of the interval.",0));
			Bag.Properties.Add(new PropertySpec("XMax", typeof(int),"Automata","Gets or sets the upper X value of the interval.",100));
			Bag.Properties.Add(new PropertySpec("YMin", typeof(int),"Automata","Gets or sets the lower Y value of the interval.",0));
			Bag.Properties.Add(new PropertySpec("YMax", typeof(int),"Automata","Gets or sets the upper Y value of the interval.",100));

		}

		protected override void GetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			base.GetPropertyBagValue (sender, e);
			switch(e.Property.Name)
			{
				case "XMin":
					e.Value = this.XMin; break;
				case "XMax":
					e.Value = this.XMax; break;
				case "YMin":
					e.Value = this.YMin; break;
				case "YMax":
					e.Value = this.YMax; break;
			}
		}

		protected override void SetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			base.SetPropertyBagValue (sender, e);
			switch(e.Property.Name)
			{
				case "XMin":
					this.XMin= (int) e.Value; break;
				case "XMax":
					this.XMax = (int) e.Value; break;
				case "YMin":
					this.YMin=(int) e.Value; break;
				case "YMax":
					this.yMax=(int) e.Value ; break;
			}
		}


		#endregion

		/// <summary>
		/// Overrides the default thumbnail used in the shape viewer
		/// </summary>
		/// <returns></returns>
		public override Bitmap GetThumbnail()
		{
			Bitmap bmp=null;
			try
			{
				Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.Automatology.Resources.XYCoordinates.gif");
					
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
