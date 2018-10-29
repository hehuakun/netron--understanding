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
	/// Plots pixels at given coordinates
	/// </summary>
	[Serializable]
	[Description("XY plotter")]
	[NetronGraphShape("XY plotter","0F7C6E8B-5685-419a-89BE-5AB37210DBCE","Automata","Netron.Automatology.XYPlotter","Plots X-Y graphics.")]
	public class XYPlotter : Shape, ISerializable
	{
		#region Fields
		/// <summary>
		/// the in X coordinate
		/// </summary>
		private Connector XConnector;
		/// <summary>
		/// the in Y coordinate
		/// </summary>
		private Connector YConnector;
		/// <summary>
		/// the color of the pixel
		/// </summary>
		private Connector RGBConnector;
		/// <summary>
		/// the underlying bitmap
		/// </summary>
		private Bitmap bm;
		/// <summary>
		/// the bitmap rectangle
		/// </summary>
		private RectangleF bitmaprec;
		
		/// <summary>
		/// volatile vars
		/// </summary>
		private int XValue, oldXValue;
		private int YValue, oldYValue;
		/// <summary>
		/// the pixel color
		/// </summary>
		private Color DotColor;
		/// <summary>
		/// scaling factors
		/// </summary>
		protected float YScale =1F;
		protected float XScale = 1F;
		/// <summary>
		/// whether to connect the dots
		/// </summary>
		protected bool connected;
		/// <summary>
		/// the pen
		/// </summary>
		[NonSerialized] protected Pen linePen;
		[NonSerialized] private PictureBox pb;

		#endregion
		
		#region Properties
		
	
		#endregion

		#region Constructor
		/// <summary>
		/// the ctor
		/// </summary>
		public XYPlotter() : base()
		{	
			Rectangle= new RectangleF(0,0,100,112);
		
			IsResizable = false;
			XConnector = new Connector(this, "XValue", true);
			XConnector.ConnectorLocation = ConnectorLocation.West;
			Connectors.Add(XConnector);

			YConnector = new Connector(this, "YValue", true);
			YConnector.ConnectorLocation = ConnectorLocation.West;
			Connectors.Add(YConnector);

			RGBConnector = new Connector(this,"RGB Color",false);
			RGBConnector.ConnectorLocation = ConnectorLocation.West;
			Connectors.Add(RGBConnector);
		
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
			info.AddValue("XConnector", this.XConnector, typeof(Connector));
			info.AddValue("YConnector", this.YConnector, typeof(Connector));
			info.AddValue("RGBConnector", this.RGBConnector, typeof(Connector));

			info.AddValue("bm", bm, typeof(Bitmap));

			info.AddValue("XScale", this.XScale);		
			info.AddValue("YScale", this.YScale);

			info.AddValue("connected", this.connected);
			info.AddValue("DotColor", DotColor, typeof(Color));
		}
		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected XYPlotter(SerializationInfo info, StreamingContext context) : base(info, context)
		{			
			this.XConnector = info.GetValue("XConnector", typeof(Connector)) as Connector;
			this.XConnector.BelongsTo = this;
			this.Connectors.Add(XConnector);

			this.YConnector = info.GetValue("YConnector", typeof(Connector)) as Connector;
			this.YConnector.BelongsTo = this;
			this.Connectors.Add(YConnector);

			this.RGBConnector = info.GetValue("RGBConnector", typeof(Connector)) as Connector;
			this.RGBConnector.BelongsTo = this;
			this.Connectors.Add(RGBConnector);

			this.connected = info.GetBoolean("connected");
			this.DotColor = (Color) info.GetValue("DotColor", typeof(Color));
			this.bm = info.GetValue("bm", typeof(Bitmap)) as Bitmap;

			this.XScale = info.GetSingle("XScale");
			this.YScale = info.GetSingle("YScale");


			pb=new PictureBox();
			pb.Location=new Point(0,0);
			bitmaprec=new RectangleF(Rectangle.Left,Rectangle.Top+12,Rectangle.Width,Rectangle.Height-12);				
			pb.Image=bm;

		}
		#endregion

		#region Methods

		/// <summary>
		/// Initializes the shape
		/// </summary>
		protected  override void InitEntity()
		{
			base.InitEntity ();
			pb=new PictureBox();
			pb.Location=new Point(0,0);
			bitmaprec=new RectangleF(Rectangle.Left,Rectangle.Top+12,Rectangle.Width,Rectangle.Height-12);	
			bm=new Bitmap(pb.Width,pb.Height);
			pb.Image=bm;
			
		
		}

		/// <summary>
		/// Returns the locations of the connectors
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public override PointF ConnectionPoint(Connector c)
		{
			
			if (c == XConnector) return new PointF(Rectangle.Left, Rectangle.Top + 12 + 12);
			if (c == YConnector) return new PointF(Rectangle.Left, Rectangle.Top + 32 + 12 );
			if (c == RGBConnector) return new PointF(Rectangle.Left, Rectangle.Top + 70 +12 );
			return new PointF(0, 0);
		}

		/// <summary>
		/// Paints the shape on the canvas
		/// </summary>
		/// <param name="g"></param>
		public override void Paint(Graphics g)
		{

		
			
			bitmaprec.X=Rectangle.Left;
			bitmaprec.Y=Rectangle.Top+12;
			//g.DrawRectangle(new Pen(Color.Black,IsSelected ? 2F : 1F),Rectangle.Left-1,Rectangle.Top-1,Rectangle.Width+2,Rectangle.Height+2);
			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			g.FillRectangle(new SolidBrush(ShapeColor), Rectangle.X, Rectangle.Y, Rectangle.Width , 12 );
			Color Background = IsSelected ? Color.LightSteelBlue : Color.WhiteSmoke;
			g.FillRectangle(new SolidBrush(Background), Rectangle.X, Rectangle.Y+12, Rectangle.Width , Rectangle.Height-12);					
			g.DrawString("XY-Plotter", Font, new SolidBrush(this.TextColor), Rectangle.Left + (Rectangle.Width / 2), Rectangle.Top , sf);
			//sf.Alignment=StringAlignment.Far;
			//g.DrawString(_OutDataType.ToString(), font, new SolidBrush(Color.Black), Rectangle.Right -2, Rectangle.Top +2*(Rectangle.Height/3)-7, sf);

			pb.BackColor=Color.Black;
			if(IsGuiReset)
			{

				Graphics.FromImage(bm).Clear(Color.White);
				oldXValue=0; XValue = 0;
				oldYValue=0; YValue=0;
				this.XConnector.Receives.Clear();
				this.YConnector.Receives.Clear();
				IsGuiReset=false;
			}
			else
				g.DrawImage(bm,bitmaprec);

		}
	

		/// <summary>
		/// Updates the automata to the next state
		/// </summary>
		public override void Update()
		{

			//			if (Redraw == 0) 
			//			{
			//				Invalidate();
			//				Redraw = 1;
			//			}
			//			Redraw--;
			try
			{
				for(int k=0; k<Math.Min(XConnector.Receives.Count,YConnector.Receives.Count); k++)
				{
					XValue=Convert.ToInt32(Convert.ToDouble(XConnector.Receives[k])*XScale);				
					YValue=Convert.ToInt32(Convert.ToDouble(YConnector.Receives[k])*YScale);				
					if(RGBConnector.Receives.Count>0)
					{
						if(RGBConnector.Receives[0] is Color)
							DotColor=(Color) RGBConnector.Receives[0];
						else if (RGBConnector.Receives[0] is int)
							DotColor = Color.FromArgb(Math.Max(0,(int) RGBConnector.Receives[0]),Color.Black);
						else
							DotColor = Color.Red;
						linePen = new Pen(new SolidBrush(DotColor),1F);
					}
					else
					{
						DotColor=Color.Red;
						linePen = BlackPen;
					}
					
				

					if(XValue<Rectangle.Width && YValue<(Rectangle.Height-12) && XValue>=0 && YValue>=0)
					{
						if(this.connected)
							try
							{
							
								Graphics.FromImage(bm).DrawLine(linePen,oldXValue,oldYValue,XValue,YValue);
							}
							catch(Exception exc)
							{
								Trace.WriteLine(exc.Message);
							}
						else
							bm.SetPixel(XValue,YValue,DotColor);
						oldXValue = XValue;
						oldYValue = YValue;
					}
				}
			}
			catch(Exception exc)
			{
				System.Diagnostics.Trace.WriteLine(exc.Message);
			}
			XConnector.Receives.Clear();
			YConnector.Receives.Clear();
			RGBConnector.Receives.Clear();
		}
		/// <summary>
		/// Sets the size in function of the bitmap rectangle
		/// </summary>
		private void SetSize()
		{
			
			Rectangle = new RectangleF(this.Rectangle.Location,new SizeF(Rectangle.Width,Rectangle.Height+12));
			bitmaprec=new RectangleF(Rectangle.Left,Rectangle.Top+12,Rectangle.Width,Rectangle.Height-12);
			pb.Width=Convert.ToInt32(Rectangle.Width);
			pb.Height=Convert.ToInt32(Rectangle.Height-12);
			Invalidate();
		}
		/// <summary>
		/// Initializes the automata
		/// </summary>
		public override void InitAutomata()
		{
			base.InitAutomata ();

			if(bm!=null) bm.Dispose();
			bm=new Bitmap(pb.Width,pb.Height);
			IsGuiReset=true;
			oldXValue=0; XValue = 0;
			oldYValue=0; YValue=0;
			this.XConnector.Receives.Clear();
			this.YConnector.Receives.Clear();

		}

		#region Access the propertygrid properties
		public override void AddProperties()
		{
			base.AddProperties ();
			Bag.Properties.Add(new PropertySpec("Connected", typeof(bool),"Automata","Gets or sets whether plotted dots should be connected to another.", false));
		}

		protected override void GetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			base.GetPropertyBagValue (sender, e);
			switch(e.Property.Name)
			{
				case "Connected":
					e.Value = this.connected; break;
			}
		}

		protected override void SetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			base.SetPropertyBagValue (sender, e);
			switch(e.Property.Name)
			{
				case "Connected":
					this.connected=(bool) e.Value; break;
				case "Size":
					SetSize(); break;
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
				Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.Automatology.Resources.XYPlotter.gif");
					
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
