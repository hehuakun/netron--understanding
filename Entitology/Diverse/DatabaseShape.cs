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
	[Serializable]
	[Description("Database shape")] 
	[NetronGraphShape("Database shape","93281885-4A86-412c-A939-2D6BAAF6CFDB","Special shapes","Netron.GraphLib.Entitology.DatabaseShape",
		 "A typical database cylinder.")]
	public class DatabaseShape : Shape, ISerializable
	{
			
		#region Fields
		private Connector TopNode;
		private Connector BottomNode;
		private Connector LeftNode;
		private Connector RightNode;		
		private StringAlignment stringAlignment;
		private Region mRegion;
		private System.Drawing.Drawing2D.GraphicsPath apath;
		#endregion

		#region Properties

	

		#endregion

		#region Constructors
		/// <summary>
		/// This is the default constructor of the class.
		/// </summary>
		public DatabaseShape() : base()
		{
			Rectangle = new RectangleF(0, 0, 70, 80);
			ShapeColor = Color.DarkRed;
			stringAlignment = StringAlignment.Center;
			
				
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

		}


		public DatabaseShape(IGraphSite site) : base(site)
		{
			Rectangle = new RectangleF(0, 0, 70, 20);
			ShapeColor = Color.DarkRed;
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
				
		}

		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public DatabaseShape(SerializationInfo info, StreamingContext context) : base(info, context)
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
		}
		#endregion

		#region Methods

		public override Bitmap GetThumbnail()
		{
			Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.GraphLib.Entitology.Resources.Database.gif");
					
			Bitmap bmp= Bitmap.FromStream(stream) as Bitmap;
			stream.Close();
			stream=null;
			return bmp;
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


		public override void AddProperties()
		{
			base.AddProperties ();
			//replace the default text editing with something more extended for a label
			Bag.Properties.Remove("Text");
			Bag.Properties.Add(new PropertySpec("Text",typeof(string),"Appearance","The text attached to the entity","[Not set]",typeof(TextUIEditor),typeof(TypeConverter)));

			Bag.Properties.Add(new PropertySpec("Alignment",typeof(StringAlignment),"Graph","Gets or sets the string alignment.",StringAlignment.Near));

		}

		

		/// <summary>
		/// Paints the shape of the person object in the plex. Here you can let your imagination go.
		/// </summary>
		/// <param name="g">The graphics canvas onto which to paint</param>
		public override void Paint(Graphics g)
		{

			if(RecalculateSize)
			{
				SizeF s = g.MeasureString(Text,Font);
				Rectangle = new RectangleF(Rectangle.X,Rectangle.Y,s.Width,Math.Max(s.Height+10,Rectangle.Height));	
				RecalculateSize = false; //very important!
				
			}			
			/*
			apath = new GraphicsPath();
			PointF[] pts = new PointF[3]{new PointF(Rectangle.X,Rectangle.Y), new PointF(Rectangle.Right,Rectangle.Top), new PointF(Rectangle.X + Rectangle.Width/2,Rectangle.Bottom)};
			apath.AddClosedCurve(pts);
			mRegion = new Region(apath);
			g.FillRegion(Brushes.Red,mRegion);
			
			*/

			apath = new GraphicsPath();
			PointF[] pts = new PointF[10]{
							new PointF(Rectangle.X,Rectangle.Y),
							new PointF(Rectangle.X,Rectangle.Y+20),
							new PointF(Rectangle.Right,Rectangle.Y+20),
							new PointF(Rectangle.Right,Rectangle.Y),
							new PointF(Rectangle.X,Rectangle.Bottom),
							new PointF(Rectangle.X,Rectangle.Bottom+20),
							new PointF(Rectangle.Right,Rectangle.Bottom+20),
							new PointF(Rectangle.Right,Rectangle.Bottom),
							new PointF(Rectangle.X,Rectangle.Y-20),
							new PointF(Rectangle.Right,Rectangle.Y-20)
									   };
			Brush br = new LinearGradientBrush(pts[0],pts[3],this.ShapeColor,Color.WhiteSmoke);
			
			apath.AddBezier(pts[0],pts[1],pts[2],pts[3]);
			apath.AddLine(pts[4],pts[0]);
			apath.AddLine(pts[7],pts[3]);
			apath.AddBezier(pts[4],pts[5],pts[6],pts[7]);
			mRegion = new Region(apath);
			
			g.FillRegion(br,mRegion);
			
			apath = new GraphicsPath();
			apath.AddBezier(pts[0],pts[8],pts[9],pts[3]);
			apath.AddBezier(pts[0],pts[1],pts[2],pts[3]);
			mRegion = new Region(apath);

			g.FillRegion(this.BackgroundBrush,mRegion);

			

			if (ShowLabel)
			{
				StringFormat sf = new StringFormat();
				sf.Alignment = stringAlignment;
				switch(stringAlignment)
				{
					case StringAlignment.Center:
						g.DrawString(Text, Font, TextBrush, Rectangle.X + (Rectangle.Width / 2), Rectangle.Y + 3, sf); break;
					case StringAlignment.Far:
						g.DrawString(Text, Font, TextBrush, Rectangle.X + Rectangle.Width-1, Rectangle.Y + 3, sf); break;
					case StringAlignment.Near:
						g.DrawString(Text, Font, TextBrush, Rectangle.X +1, Rectangle.Y + 3, sf); break;
				}
			}			
			base.Paint(g);
		}

		protected override void GetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			base.GetPropertyBagValue (sender, e);
			switch(e.Property.Name)
			{
				case "Alignment":
					e.Value = this.stringAlignment; break;
					
			}
		}

		protected override void SetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			base.SetPropertyBagValue (sender, e);
			switch(e.Property.Name)
			{
				case "Alignment":
					this.stringAlignment = (StringAlignment) e.Value; this.Invalidate(); break;
			}
		}


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


		#endregion

		#region ISerializable Members

		



		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData (info, context);

			info.AddValue("TopNode", TopNode, typeof(Connector));

			info.AddValue("BottomNode", BottomNode, typeof(Connector));

			info.AddValue("LeftNode", LeftNode, typeof(Connector));

			info.AddValue("RightNode", RightNode, typeof(Connector));
		}
		}

		#endregion
		
		
	}


