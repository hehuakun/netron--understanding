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
	/// Plots an array of bits
	/// </summary>
	[Serializable]
	[Description("Bit plotter")]
	[NetronGraphShape("Bit plotter","4FE5732C-3817-4dc8-A3AE-5955F6865075","Automata","Netron.Automatology.BitPlotter",
		 "Displays the values of a bit stream.")]
	public class BitPlotter : Shape, ISerializable
	{
		#region Fields
		/// <summary>
		/// the in connector
		/// </summary>
		protected  Connector inConnector;		
		/// <summary>
		/// the color connector
		/// </summary>
		protected Connector RGBColor;
		/// <summary>
		/// the bitmap used to draw
		/// </summary>
		[NonSerialized] protected Bitmap bm;
		/// <summary>
		/// the bitmap rectangle
		/// </summary>
		protected RectangleF bitmapRec;
		/// <summary>
		/// the sizes
		/// </summary>
		protected int XSize=100,YSize=100;

		protected int XValue;
		/// <summary>
		/// the current row of the plot
		/// </summary>
		protected int currentrow=1;

		/// <summary>
		/// the color of the current dot
		/// </summary>
		protected Color DotColor;
		/// <summary>
		/// to redraw or not to redraw
		/// </summary>
		protected int Redraw;
		/// <summary>
		/// the scaling factor
		/// </summary>
		protected float YScale =1F;
		/// <summary>
		/// the picture box holding the plot
		/// </summary>
		[NonSerialized]  protected PictureBox pb;
		#endregion

		#region Properties
		
		#endregion

		#region Constructor
		
		/// <summary>
		/// the ctor
		/// </summary>
		public BitPlotter() : base()
		{
			Rectangle= new RectangleF(0,0,100,112);			
			IsResizable = false;
			inConnector = new Connector(this, "In bits", false);
			inConnector.ConnectorLocation = ConnectorLocation.West;
			Connectors.Add(inConnector);
			
			RGBColor = new Connector(this,"RGB Color",false);
			RGBColor.ConnectorLocation = ConnectorLocation.West;
			Connectors.Add(RGBColor);
		
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
			info.AddValue("RGBColor", this.RGBColor, typeof(Connector));

			info.AddValue("bm", this.bm, typeof(Bitmap));
			info.AddValue("bitmapRec", this.bitmapRec, typeof(RectangleF));

			info.AddValue("XSize", this.XSize);
			info.AddValue("YSize", this.YSize);
			info.AddValue("XValue", this.XValue);
			info.AddValue("currentrow", this.currentrow);		
			info.AddValue("DotColor", this.DotColor, typeof(Color));
			info.AddValue("Redraw", this.Redraw);
			info.AddValue("YScale", YScale);
		}
		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected BitPlotter(SerializationInfo info, StreamingContext context) : base(info, context)
		{			
			this.inConnector = info.GetValue("inConnector", typeof(Connector)) as Connector;
			this.inConnector.BelongsTo = this;
			this.Connectors.Add(inConnector);
			try
			{
				this.RGBColor = info.GetValue("RGBColor", typeof(Connector)) as Connector;
				this.RGBColor.BelongsTo = this;
				this.Connectors.Add(RGBColor);
			}
			catch
			{
				
			}
			
			this.bm = info.GetValue("bm", typeof(Bitmap)) as Bitmap;
			this.bitmapRec = (RectangleF) info.GetValue("bitmapRec", typeof(RectangleF));

			this.XSize = info.GetInt32("XSize");
			this.YSize = info.GetInt32("YSize");
			this.XValue = info.GetInt32("XValue");
			this.currentrow = info.GetInt32("currentrow");
			this.DotColor = (Color) info.GetValue("DotColor", typeof(Color));
			this.Redraw = info.GetInt32("Redraw");
			this.YScale = info.GetSingle("YScale");

			pb=new PictureBox();
			pb.Location=new Point(0,0);			
			pb.Image=bm;

		}

		#endregion

		#region Methods
		
		#region Access the PropertyGrid properties
		public override void AddProperties()
		{
			base.AddProperties ();

		}
		protected override void GetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			base.GetPropertyBagValue (sender, e);
		}

		protected override void SetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			base.SetPropertyBagValue (sender, e);
			switch(e.Property.Name)
			{
				case "Size":
					SetSize(); break;
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
			if (c == inConnector) return new PointF(Rectangle.Left, Rectangle.Top+Rectangle.Height/3 );
			if (c == RGBColor) return new PointF(Rectangle.Left, Rectangle.Top +2*Rectangle.Height/3 );
			return new PointF(0, 0);
		}

		/// <summary>
		/// Paints the shape on the canvas
		/// </summary>
		/// <param name="g"></param>
		public override void Paint(Graphics g)
		{	
			bitmapRec.X=Rectangle.Left;
			bitmapRec.Y=Rectangle.Top+12;
			//g.DrawRectangle(new Pen(Color.Black,IsSelected ? 2F : 1F),Rectangle.Left-1,Rectangle.Top-1,Rectangle.Width+2,Rectangle.Height+2);
			//the title part
			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			g.FillRectangle(new SolidBrush(ShapeColor), Rectangle.X, Rectangle.Y, Rectangle.Width , 12 );
			g.DrawString("BitPlotter", Font, new SolidBrush(this.TextColor), Rectangle.Left + (Rectangle.Width / 2), Rectangle.Top , sf);
			//the data part
			Color Background = IsSelected ? Color.LightSteelBlue : Color.WhiteSmoke;
			g.FillRectangle(new SolidBrush(Background), Rectangle.X, Rectangle.Y+12, Rectangle.Width , Rectangle.Height-12 );
			
			//sf.Alignment=StringAlignment.Far;
			//g.DrawString(_OutDataType.ToString(), font, new SolidBrush(Color.Black), Rectangle.Right -2, Rectangle.Top +2*(Rectangle.Height/3)-7, sf);

			pb.BackColor=Color.Black;
			
			g.DrawImage(bm,bitmapRec);
			//			if(XValue<XSize && YValue<YSize)
			//				bm.SetPixel(XValue,YValue,DotColor);
		}
		

		/// <summary>
		/// Updates the automata to the next state
		/// </summary>
		public override void Update()
		{

			if (Redraw == 0) 
			{
				Invalidate();
				Redraw = 1;
			}
			Redraw--;
			try
			{
				if(inConnector.Receives.Count>0 && currentrow<=Rectangle.Height)
				{
					if(RGBColor.Receives.Count>0)
						DotColor=(Color) RGBColor.Receives[0];
					else
						DotColor=Color.Black;

					for(int k=0;k<Math.Min(inConnector.Receives.Count,Rectangle.Width);k++)
					{
						XValue=(int) inConnector.Receives[k];
						if(XValue!=0)
							bm.SetPixel(k,currentrow,DotColor);
					}
					currentrow++;
				}
				//				if(YConnector.Receives.Count>0)
				//					YValue=Convert.ToInt32(Convert.ToDouble(YConnector.Receives[0])*YScale);
				//System.Diagnostics.Trace.WriteLine("(" + XValue + "," + YValue+")");
				

				
			}
			catch{}			
			inConnector.Receives.Clear();			
			RGBColor.Receives.Clear();
		}
		/// <summary>
		/// Sets the size of the shape and plotting area
		/// </summary>
		private void SetSize()
		{			
			//Rectangle = new RectangleF(this.Rectangle.Location,new SizeF(XSize,YSize+12));
			bitmapRec=new RectangleF(Rectangle.Left,Rectangle.Top+12,Rectangle.Width,Rectangle.Height);
			pb.Width=Convert.ToInt32(Rectangle.Width);
			pb.Height=Convert.ToInt32(Rectangle.Height);
			Invalidate();
		}
		/// <summary>
		/// Initializes the automata
		/// </summary>
		public override void InitAutomata()
		{
			base.InitAutomata ();			
			bm=new Bitmap(Convert.ToInt32(Rectangle.Width),Convert.ToInt32(Rectangle.Height));
			currentrow=1;
		}

		/// <summary>
		/// Initializes the shape
		/// </summary>
		protected  override void InitEntity()
		{
			base.InitEntity ();
			bitmapRec=new RectangleF(Rectangle.Left,Rectangle.Top+12,XSize,YSize);
			bm=new Bitmap(Convert.ToInt32(Rectangle.Width),Convert.ToInt32(Rectangle.Height));
			pb=new PictureBox();
			pb.Location=new Point(0,0);			
			pb.Image=bm;
			Redraw = 0;

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
				Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.Automatology.Resources.BitPlotter.gif");
					
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
