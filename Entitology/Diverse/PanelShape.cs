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
	[Description("Panel shape")]
	[NetronGraphShape("Panel","C12AFDC3-3C28-4c8a-BEFD-B8E62651A384","Special shapes","Netron.GraphLib.Entitology.PanelShape",
		 "A panel to emphasize conceptual groups.")]
	public class PanelShape : Shape, ISerializable
	{
		#region Fields

		private Color mLightColor = Color.WhiteSmoke;
		private Color mDarkColor = Color.LightGreen;
		private LinearGradientMode mGradientMode = LinearGradientMode.Vertical;

		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the gradient mode
		/// </summary>
		public LinearGradientMode GradientMode
		{
			get{return mGradientMode;}
			set{mGradientMode = value;}
		}	
	
		public Color LightColor
		{
			get{return mLightColor;}
			set{mLightColor = value;

			}
		}

		public Color DarkColor
		{
			get{return mDarkColor;}
			set{mDarkColor = value;

			}
		}

		#endregion
		
		#region Constructor

		/// <summary>
		/// Constructor
		/// </summary>
		public PanelShape() : base()
		{
			Rectangle = new RectangleF(0, 0, 70, 20);
			
			IsResizable=true;
		}
		/// <summary>
		/// This is the default constructor of the class.
		/// </summary>
		public PanelShape(IGraphSite site) : base(site)
		{
			//set the default size
			Rectangle = new RectangleF(0, 0, 70, 20);			

			IsResizable=true;
		}

		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected PanelShape(SerializationInfo info, StreamingContext context) : base(info, context)
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
		}
		#endregion	

		#region Methods
		/// <summary>
		/// Overrides the default bitmap used in the shape viewer
		/// </summary>
		/// <returns></returns>
		public override Bitmap GetThumbnail()
		{
			Bitmap bmp=null;
			try
			{
				Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.GraphLib.BasicShapes.Resources.PanelShape.gif");
					
				bmp= Bitmap.FromStream(stream) as Bitmap;
				stream.Close();
				stream=null;
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.Message,"PanelShape.GetThumbnail");
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

		
		
		public override void Paint(Graphics g)
		{
			base.Paint(g);
			if(RecalculateSize)
			{
				Rectangle = new RectangleF(new PointF(Rectangle.X,Rectangle.Y),
					g.MeasureString(this.Text,Font));	
				Rectangle = System.Drawing.RectangleF.Inflate(Rectangle,10,10);
				RecalculateSize = false; //very important!
			}
		
	
			g.FillRectangle(this.BackgroundBrush,Rectangle);


			
			
		}

		#region Properties
		public override void AddProperties()
		{
			base.AddProperties ();

			Bag.Properties.Remove("ShowLabel");
			Bag.Properties.Remove("Text");
			Bag.Properties.Remove("ShapeColor");

			Bag.Properties.Add(new PropertySpec("UpperColor",typeof(Color),"Appearance","The upper color of the gradient.",Color.WhiteSmoke)); 
			Bag.Properties.Add(new PropertySpec("LowerColor",typeof(Color),"Appearance","The lower color of the gradient.",Color.LightGreen)); 
			
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
			}

		}
		#endregion
		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData (info, context);
			info.AddValue("mLightColor", this.LightColor, typeof(Color));
			info.AddValue("mDarkColor", this.DarkColor, typeof(Color));
		}

		


		
		#endregion
	}

}







		
