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
	/// A simple rectangular shape with four connectors.
	/// </summary>
	[Serializable]
	[Description("Round panel shape")]
	[NetronGraphShape("Round panel","3E13DD66-8C44-4a45-822E-9D87D519B4B3","Special shapes","Netron.GraphLib.Entitology.RoundPanelShape",
		 "A panel to emphasize conceptual groups.")]
	public class RoundPanelShape : Shape, ISerializable
	{
		#region Fields

		private Color mLightColor = Color.WhiteSmoke;
		private Color mDarkColor = Color.LightSteelBlue;
		private LinearGradientMode mGradientMode = LinearGradientMode.Vertical;
		private bool mFilled;
		private DashStyle mDashStyle = DashStyle.Solid;
		private float mLineWeight = 1F;
		private Color  mLineColor = Color.DimGray;

		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the line color
		/// </summary>
		public Color LineColor
		{
			get{return mLineColor;}
			set{mLineColor = value;
			UpdatePen();
			}
		}
		/// <summary>
		/// Gets or sets the dash style of the surrounding line
		/// </summary>
		public DashStyle DashStyle
		{
			get{return mDashStyle;}
			set{mDashStyle = value;
			UpdatePen();
			}
		}
		/// <summary>
		/// Gets or sets whether the shape is filled
		/// </summary>
		public bool Filled
		{
			get{return mFilled;}
			set{mFilled = value;
			this.Invalidate();
			}
		}
		/// <summary>
		/// Gets or sets the line weight
		/// </summary>
		public float LineWeight
		{
			get{return mLineWeight;}
			set{
				mLineWeight = value;
				UpdatePen();
			}
		}

		/// <summary>
		/// Gets or sets the gradient mode
		/// </summary>
		public LinearGradientMode GradientMode
		{
			get{return mGradientMode;}
			set{mGradientMode = value;
			this.Invalidate();
			}
		}	
	
		public Color LightColor
		{
			get{return mLightColor;}
			set{mLightColor = value;
				this.Invalidate();
			}
		}

		public Color DarkColor
		{
			get{return mDarkColor;}
			set{mDarkColor = value;
				this.Invalidate();
			}
		}

		#endregion
		
		#region Constructor

		/// <summary>
		/// Constructor
		/// </summary>
		public RoundPanelShape() : base()
		{
			Rectangle = new RectangleF(0, 0, 70, 20);
			
			IsResizable=true;
		}
		/// <summary>
		/// This is the default constructor of the class.
		/// </summary>
		public RoundPanelShape(IGraphSite site) : base(site)
		{
			//set the default size
			Rectangle = new RectangleF(0, 0, 70, 20);			

			IsResizable=true;
		}

		/// <summary>
		/// ISerializable implementation
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData (info, context);
			info.AddValue("mLightColor", this.LightColor, typeof(Color));
			info.AddValue("mDarkColor", this.DarkColor, typeof(Color));

			info.AddValue("mFilled", mFilled);
			info.AddValue("mDashStyle", this.mDashStyle, typeof(DashStyle));
			info.AddValue("mLineWeight", this.mLineWeight);

			info.AddValue("mLineColor", this.mLineColor, typeof(Color));
			
		}

		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected RoundPanelShape(SerializationInfo info, StreamingContext context) : base(info, context)
		{			
			try
			{
				this.mLightColor = (Color) info.GetValue("mLightColor", typeof(Color));				
			}
			catch
			{
				this.mLightColor = Color.WhiteSmoke;
			}

			try
			{
				this.mDarkColor = (Color) info.GetValue("mDarkColor",typeof(Color));
			}
			catch
			{
				this.mDarkColor = Color.LightGreen;
			}

			this.mFilled = info.GetBoolean("mFilled");

			this.mDashStyle = (DashStyle) info.GetValue("mDashStyle", typeof(DashStyle));
			this.mLineWeight = info.GetSingle("mLineWeight");

			this.mLineColor = (Color) info.GetValue("mLineColor", typeof(Color));

		
			
		}

		public override void PostDeserialization()
		{
			base.PostDeserialization ();
			UpdatePen();
		}

		#endregion	

		#region Methods

		/// <summary>
		/// Updates the pen object in function of the settings
		/// </summary>
		private void UpdatePen()
		{
			Pen = new Pen(mLineColor, mLineWeight);
			Pen.DashStyle = mDashStyle;

		}
		/// <summary>
		/// Overrides the default bitmap used in the shape viewer
		/// </summary>
		/// <returns></returns>
		public override Bitmap GetThumbnail()
		{
			Bitmap bmp=null;
			try
			{
				Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.GraphLib.BasicShapes.Resources.RoundPanelShape.gif");
					
				bmp= Bitmap.FromStream(stream) as Bitmap;
				stream.Close();
				stream=null;
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.Message,"RoundPanelShape.GetThumbnail");
			}
			return bmp;
		}
		protected  override Brush BackgroundBrush
		{
			get
			{
				return new LinearGradientBrush(Rectangle,mLightColor,mDarkColor, this.mGradientMode);
			}
		}

		
		
		public override void Paint (Graphics g)
		{
			base.Paint(g);
			
			
				GraphicsPath path = new GraphicsPath();			
				path.AddArc(Rectangle.X, Rectangle.Y, 20, 20, -180, 90);			
				path.AddLine(Rectangle.X + 10, Rectangle.Y, Rectangle.X + Rectangle.Width - 10, Rectangle.Y);			
				path.AddArc(Rectangle.X + Rectangle.Width - 20, Rectangle.Y, 20, 20, -90, 90);			
				path.AddLine(Rectangle.X + Rectangle.Width, Rectangle.Y + 10, Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height - 10);			
				path.AddArc(Rectangle.X + Rectangle.Width - 20, Rectangle.Y + Rectangle.Height - 20, 20, 20, 0, 90);			
				path.AddLine(Rectangle.X + Rectangle.Width - 10, Rectangle.Y + Rectangle.Height, Rectangle.X + 10, Rectangle.Y + Rectangle.Height);			
				path.AddArc(Rectangle.X, Rectangle.Y + Rectangle.Height - 20, 20, 20, 90, 90);			
				path.AddLine(Rectangle.X, Rectangle.Y + Rectangle.Height - 10, Rectangle.X, Rectangle.Y + 10);			
			
			if(mFilled)
			{
				//background
				g.FillPath(this.BackgroundBrush, path);
			}
			
			/* This is an upper bar if you wish
				GraphicsPath gradientPath = new GraphicsPath();				
				gradientPath.AddArc(Rectangle.X + 1, Rectangle.Y + 1, 18, 18, -180, 90);				
				gradientPath.AddLine(Rectangle.X + 11, Rectangle.Y + 1, Rectangle.X + Rectangle.Width - 11, Rectangle.Y + 1);				
				gradientPath.AddArc(Rectangle.X + Rectangle.Width - 19, Rectangle.Y + 1, 18, 18, -90, 90);				
				gradientPath.AddLine(Rectangle.X + Rectangle.Width - 1, Rectangle.Y + 50, Rectangle.X + 1, Rectangle.Y + 50);				
				//gradient
				Brush unBrush = new LinearGradientBrush(new Point(((int)(Rectangle.X)),((int)(Rectangle.Y))), new Point(((int)(Rectangle.X + Rectangle.Width)),((int)(Rectangle.Y))), ShapeColor, Color.White);
				Region unaRegion = new Region(gradientPath);
				g.FillRegion(unBrush, unaRegion);
			*/
			
			//the border
			g.DrawPath(Pen, path);
			
			if(ShowLabel)
			{
				StringFormat sf = new StringFormat();
				sf.Trimming = StringTrimming.EllipsisCharacter;
				g.DrawString(Text,Font, TextBrush, Rectangle.X + 20, Rectangle.Y + 5);				
			}
		}
		

		#region Properties
		public override void AddProperties()
		{
			base.AddProperties ();

			Bag.Properties.Remove("ShapeColor");

			Bag.Properties.Add(new PropertySpec("UpperColor",typeof(Color),"Appearance","The upper color of the gradient.",Color.WhiteSmoke)); 
			Bag.Properties.Add(new PropertySpec("LowerColor",typeof(Color),"Appearance","The lower color of the gradient.",Color.LightGreen)); 
			Bag.Properties.Add(new PropertySpec("Filled",typeof(bool),"Appearance","Whether the shape is being filled", false)); 
			Bag.Properties.Add(new PropertySpec("LineWeight",typeof(float),"Appearance","The line weight", 1F)); 
			Bag.Properties.Add(new PropertySpec("DashStyle",typeof(DashStyle),"Appearance","The line weight", DashStyle.Solid)); 			
			Bag.Properties.Add(new PropertySpec("LineColor",typeof(Color),"Appearance","The line color",Color.DimGray)); 
		}
		
		protected override void GetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			base.GetPropertyBagValue (sender, e);

			switch(e.Property.Name)
			{
				case "UpperColor":				
					e.Value = this.LightColor;
					break;
				case "LowerColor":				
					e.Value = this.DarkColor;
					break;				
				case "Filled":
					e.Value = this.mFilled;
					break;
				case "DashStyle":
					e.Value = this.mDashStyle;
					break;
				case "LineWeight":
					e.Value = this.mLineWeight;
					break;
				case "LineColor":
					e.Value = this.mLineColor;
					break;

			}
		}

		protected override void SetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			base.SetPropertyBagValue (sender, e);


			switch(e.Property.Name)
			{
				case "UpperColor":
					this.LightColor = (Color) e.Value;
					break;
				case "LowerColor":
					this.DarkColor = (Color) e.Value;
					break;
				case "Filled":
					this.Filled = (bool) e.Value;
					break;
				case "DashStyle":
					this.DashStyle = (DashStyle) e.Value;
					break;
				case "LineWeight":
					this.LineWeight = (float) e.Value;
					break;
				case "LineColor":
					this.LineColor = (Color) e.Value;
					break;
			}

		}
		#endregion
		

		


		
		#endregion
	}

}







		
