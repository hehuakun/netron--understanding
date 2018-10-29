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
	/// A text label shape
	/// </summary>
	[Serializable]
	[Description("Text bubble")] 
	[NetronGraphShape("Text bubble","28B60D7E-B62B-4aee-89B4-45FF2B8AF314","Special shapes","Netron.GraphLib.Entitology.TextBubble",
		 "Text bubble")]
	public class TextBubble : Shape
	{
			
		#region Fields
		/// <summary>
		/// the alignment of the text
		/// </summary>
	
	
		private RectangleF textRectangle;
		private RectangleF bubbleRectangle;
		int xshift = 20;
		#endregion

		#region Properties

	
	

		#endregion

		#region Constructors
		/// <summary>
		/// This is the default constructor of the class.
		/// </summary>
		public TextBubble() : base()
		{
			Rectangle = new RectangleF(0, 0, 70, 80);
			ShapeColor = Color.Gainsboro;
			
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="site"></param>
		public TextBubble(IGraphSite site) : base(site)
		{
			Rectangle = new RectangleF(0, 0, 70, 20);
			ShapeColor = Color.LightYellow;
		}

		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected TextBubble(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			
		}
		#endregion

		#region Methods
		/// <summary>
		/// Adds additional properties to the shape
		/// </summary>
		public override void AddProperties()
		{
			base.AddProperties ();
			//replace the default text editing with something more extended for a label
			Bag.Properties.Remove("Text");
			Bag.Properties.Add(new PropertySpec("Text",typeof(string),"Appearance","The text attached to the entity","[Not set]",typeof(TextUIEditor),typeof(TypeConverter)));
			

		}

		/// <summary>
		/// Returns a thumbanil representation of this shape
		/// </summary>
		/// <returns></returns>
		public override Bitmap GetThumbnail()
		{
			Bitmap bmp=null;
			try
			{
				Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.GraphLib.BasicShapes.Resources.TextBubble.gif");
					
				bmp= Bitmap.FromStream(stream) as Bitmap;
				stream.Close();
				stream=null;
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.Message, "TextBubble.GetThumbnail");
			}
			return bmp;
		}

		/// <summary>
		/// Paints the shape of the person object in the plex. Here you can let your imagination go.
		/// </summary>
		/// <param name="g">The graphics canvas onto which to paint</param>
		public override void Paint(Graphics g)
		{

//			if(RecalculateSize)
//			{
//				textRectangle = Rectangle;
//				textRectangle.Inflate(-2,-2);
////				SizeF s = g.MeasureString(this.Text,Font);
////				Rectangle = new RectangleF(Rectangle.X,Rectangle.Y,s.Width,Math.Max(s.Height+10,Rectangle.Height));	
//				RecalculateSize = false; //very important!				
//			}			
			

			textRectangle = Rectangle;
			textRectangle.Inflate(-10,-35);
			textRectangle.Offset(0,-31);
			bubbleRectangle = Rectangle;
			bubbleRectangle.Inflate(0,-30);
			bubbleRectangle.Offset(0,-30);
			GraphicsPath path = new GraphicsPath();			
			path.AddArc(bubbleRectangle.X, bubbleRectangle.Y, 20, 20, -180, 90);			
			path.AddLine(bubbleRectangle.X + 10, bubbleRectangle.Y, bubbleRectangle.X + bubbleRectangle.Width - 10, bubbleRectangle.Y);			
			path.AddArc(bubbleRectangle.X + bubbleRectangle.Width - 20, bubbleRectangle.Y, 20, 20, -90, 90);			
			path.AddLine(bubbleRectangle.X + bubbleRectangle.Width, bubbleRectangle.Y + 10, bubbleRectangle.X + bubbleRectangle.Width, bubbleRectangle.Y + bubbleRectangle.Height - 10);			
			path.AddArc(bubbleRectangle.X + bubbleRectangle.Width - 20, bubbleRectangle.Y + bubbleRectangle.Height - 20, 20, 20, 0, 90);			
			
			path.AddLine(bubbleRectangle.X + bubbleRectangle.Width - 10, bubbleRectangle.Y + bubbleRectangle.Height, bubbleRectangle.X + xshift +10, bubbleRectangle.Y + bubbleRectangle.Height);			
			path.AddLine(bubbleRectangle.X + xshift +10, bubbleRectangle.Y + bubbleRectangle.Height, bubbleRectangle.X + xshift, Rectangle.Bottom-5);			
			path.AddLine( bubbleRectangle.X + xshift, Rectangle.Bottom-5, bubbleRectangle.X + xshift, bubbleRectangle.Y + bubbleRectangle.Height);			
			path.AddLine(bubbleRectangle.X + xshift, bubbleRectangle.Y + bubbleRectangle.Height, bubbleRectangle.X + 10, bubbleRectangle.Y + bubbleRectangle.Height);			
			
			path.AddArc(bubbleRectangle.X, bubbleRectangle.Y + bubbleRectangle.Height - 20, 20, 20, 90, 90);			
			path.AddLine(bubbleRectangle.X, bubbleRectangle.Y + bubbleRectangle.Height - 10, bubbleRectangle.X, bubbleRectangle.Y + 10);			
			//shadow
			Region darkRegion = new Region(path);
			darkRegion.Translate(5, 5);
			g.FillRegion(new SolidBrush(Color.FromArgb(20, Color.Black)), darkRegion);
			
			//background
			g.FillPath(this.BackgroundBrush, path);
	
				StringFormat sf = new StringFormat();

				sf.Alignment = StringAlignment.Near;
				g.DrawString(Text, Font, TextBrush, textRectangle, sf); 
		
						
			base.Paint(g);
		}

		
		


		/// <summary>
		/// Changes the default context-menu
		/// </summary>
		/// <returns></returns>
		public override MenuItem[] ShapeMenu()
		{
			MenuItem[] subitems = new MenuItem[]{new MenuItem("First one",new EventHandler(TheHandler)),new MenuItem("Second one",new EventHandler(TheHandler))};

			MenuItem[] items = new MenuItem[]{new MenuItem("Special menu",subitems)};

			return items;
		}

		private void TheHandler(object sender, EventArgs e)
		{
			MessageBox.Show("Just an example.");
		}

		/// <summary>
		/// ISerializable serialization
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
