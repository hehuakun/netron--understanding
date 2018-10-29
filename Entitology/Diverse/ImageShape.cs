using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Drawing.Design;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
//using System.Runtime.Serialization.Formatters.Soap;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Netron.GraphLib.Attributes;
using Netron.GraphLib.UI;
using Netron.GraphLib;
using Netron.GraphLib.Interfaces;
namespace Netron.GraphLib.Entitology
{
	/// <summary>
	/// A simple rectangular shape with four connectors.
	/// </summary>
	[Serializable]
	[Description("Image shape")]
	[NetronGraphShape("Image","47D016B9-990A-436c-ADE8-B861714EBE5A","Special shapes","Netron.GraphLib.Entitology.ImageShape",
		 "An image shape.")]
	public class ImageShape : Shape, ISerializable
	{
		#region Fields

		#region the connectors
		private Connector TopNode;
		private Connector BottomNode;
		private Connector LeftNode;
		private Connector RightNode;		
		private string mImagePath;
		private Image mImage;
		#endregion
		#endregion
		
		#region Properties
		public string ImagePath
		{
			get{return mImagePath;}
			set{
				if(value==string.Empty) return;
				if(File.Exists(value))
				{
					try
					{
						mImage = Image.FromFile(value);
						Rectangle = new RectangleF(Rectangle.X, Rectangle.Y, mImage.Width, mImage.Height);
					}
					catch(Exception exc)
					{
						Site.OutputInfo(exc.Message, OutputInfoLevels.Exception);
					}
					mImagePath = value;
				}
				else
					MessageBox.Show("The specified file does not exist.","Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		public Image Image
		{
			get{return mImage;}
			set{
				if(value==null) return;
				mImage = value;
				mImagePath = string.Empty;
				Rectangle = new RectangleF(Rectangle.X, Rectangle.Y, mImage.Width, mImage.Height);			
			}
		}
		#endregion

		#region Constructor

		/// <summary>
		/// Constructor
		/// </summary>
		public ImageShape() : base()
		{
			Rectangle = new RectangleF(0, 0, 70, 20);

			TopNode = new Connector(this, "Top", true);
			TopNode.ConnectorLocation = ConnectorLocation.North;
			Connectors.Add(TopNode);

			BottomNode = new Connector(this, "Bottom", true);
			BottomNode.ConnectorLocation = ConnectorLocation.South;
			Connectors.Add(BottomNode);

			LeftNode = new Connector(this, "Left", true);
			LeftNode.ConnectorLocation = ConnectorLocation.West;
			Connectors.Add(LeftNode);

			RightNode = new Connector(this, "Right", true);
			RightNode.ConnectorLocation = ConnectorLocation.East;
			Connectors.Add(RightNode);	

			IsResizable=true;
		}
		/// <summary>
		/// This is the default constructor of the class.
		/// </summary>
		public ImageShape(IGraphSite site) : base(site)
		{
			//set the default size
			Rectangle = new RectangleF(0, 0, 70, 20);
			//add the connectors
			TopNode = new Connector(this, "Top", true);
			TopNode.ConnectorLocation = ConnectorLocation.North;
			Connectors.Add(TopNode);

			BottomNode = new Connector(this, "Bottom", true);
			BottomNode.ConnectorLocation = ConnectorLocation.South;
			Connectors.Add(BottomNode);

			LeftNode = new Connector(this, "Left", true);
			LeftNode.ConnectorLocation = ConnectorLocation.West;
			Connectors.Add(LeftNode);

			RightNode = new Connector(this, "Right", true);
			RightNode.ConnectorLocation = ConnectorLocation.East;
			Connectors.Add(RightNode);		

			IsResizable=true;
		}

		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected ImageShape(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			TopNode = (Connector) info.GetValue("TopNode", typeof(Connector));
			TopNode.BelongsTo = this;
			Connectors.Add(TopNode);			

			BottomNode = (Connector) info.GetValue("BottomNode", typeof(Connector));
			BottomNode.BelongsTo = this;
			Connectors.Add(BottomNode);			

			LeftNode = (Connector) info.GetValue("LeftNode", typeof(Connector));
			LeftNode.BelongsTo = this;
			Connectors.Add(LeftNode);			

			RightNode = (Connector) info.GetValue("RightNode", typeof(Connector));
			RightNode.BelongsTo = this;
			Connectors.Add(RightNode);			

			IsResizable = true;

			try
			{
				mImage = info.GetValue("mImage", typeof(Image)) as Image;
				if(mImage !=null) 
				{
					//the following line will reset the original image size, but I guess it's safer to keep the serialized size;
					//Rectangle = new RectangleF(Rectangle.X, Rectangle.Y, mImage.Width, mImage.Height);
					mImagePath = info.GetString("mImagePath");
				}
				
			}
			catch
			{
				
			}
			
		}
		#endregion	

		#region Properties
		
	
		#endregion

		#region Methods
		/// <summary>
		/// Adds additional stuff to the shape's menu
		/// </summary>
		/// <returns></returns>
		public override MenuItem[] ShapeMenu()
		{
	
			MenuItem item= new MenuItem("Reset image size",new EventHandler(OnResetImageSize));
			MenuItem[] items = new MenuItem[]{item};

			return items;
		}
		/// <summary>
		/// Resets the original image size
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnResetImageSize(object sender, EventArgs e)
		{
			Rectangle = new RectangleF(Rectangle.X, Rectangle.Y, mImage.Width, mImage.Height);
			this.Invalidate();
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
				Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.GraphLib.BasicShapes.Resources.ImageShape.gif");
					
				bmp= Bitmap.FromStream(stream) as Bitmap;
				stream.Close();
				stream=null;
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.Message,"ImageShape.GetThumbnail");
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
			if(RecalculateSize)
			{
				Rectangle = new RectangleF(new PointF(Rectangle.X,Rectangle.Y),
					g.MeasureString(this.Text,Font));	
				Rectangle = System.Drawing.RectangleF.Inflate(Rectangle,10,10);
				RecalculateSize = false; //very important!
			}
			if(mImage==null)
			{
				g.FillRectangle(this.BackgroundBrush, Rectangle);
				StringFormat sf = new StringFormat();
				sf.Alignment = StringAlignment.Center;
				g.DrawString("Image shape", Font, TextBrush, Rectangle.X + (Rectangle.Width / 2), Rectangle.Y + 3, sf);
			}
			else
				g.DrawImage(mImage, Rectangle);			

			
				
					
			
		}

		/// <summary>
		/// Returns a floating-point point coordinates for a given connector
		/// </summary>
		/// <param name="c">A connector object</param>
		/// <returns>A floating-point pointF</returns>
		public override PointF ConnectionPoint(Connector c)
		{
			
			if (c == TopNode) return new PointF(Rectangle.Left + (Rectangle.Width * 1/2), Rectangle.Top);
			if (c == BottomNode) return new PointF(Rectangle.Left + (Rectangle.Width * 1/2), Rectangle.Bottom);
			if (c == LeftNode) return new PointF(Rectangle.Left , Rectangle.Top +(Rectangle.Height*1/2));
			if (c == RightNode) return new PointF(Rectangle.Right, Rectangle.Top +(Rectangle.Height*1/2));			
			return new PointF(0, 0);
			
		}


		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData (info, context);

			info.AddValue("TopNode", TopNode, typeof(Connector));

			info.AddValue("BottomNode", BottomNode, typeof(Connector));

			info.AddValue("LeftNode", LeftNode, typeof(Connector));

			info.AddValue("RightNode", RightNode, typeof(Connector));

			info.AddValue("mImage", mImage, typeof(Image));

			info.AddValue("mImagePath", this.mImagePath);
		}

		public override void AddProperties()
		{
			base.AddProperties ();
			Bag.Properties.Remove("Text");
			Bag.Properties.Remove("ShapeColor");
			Bag.Properties.Remove("ShowLabel");
			Bag.Properties.Add(new PropertySpec("ImagePath",typeof(string),"Appearance","The image to display in the shape",null,typeof(UI.FilenameUIEditor),typeof(TypeConverter))); //,string.Empty,typeof(UI.TextUIEditor), typeof(TypeConverter)
		}


		protected override void GetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			base.GetPropertyBagValue (sender, e);

			if(e.Property.Name=="ImagePath")
			{
				e.Value = this.mImagePath;
			}
		}

		protected override void SetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			base.SetPropertyBagValue (sender, e);


			if(e.Property.Name=="ImagePath")
			{
				this.ImagePath = (string) e.Value;
			}

		}


		


		
		#endregion
	}

}







		
