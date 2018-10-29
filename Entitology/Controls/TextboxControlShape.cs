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
using System.Drawing.Design;
using Netron.GraphLib.Utils;
namespace Netron.GraphLib.Entitology
{
	[Serializable]
	[Description("Basic node")]
	[NetronGraphShape("Textbox control","8BF5F303-0422-464c-BD92-C370E0411A47","Special shapes","Netron.GraphLib.Entitology.TextBoxControlShape",
		 "Shape with a true textbox control")]
	public class TextBoxControlShape : Shape
	{
		#region Fields	
		//the only connector on this shape
		private Connector leftConnector;
		NTextBox textbox;
		#endregion

		#region Properties
		

		#endregion
		
		#region Constructors
		

		/// <summary>
		/// This is the default constructor of the class.
		/// </summary>
		public TextBoxControlShape() : base()
		{
			//set the default size
			Rectangle = new RectangleF(0, 0, 70, 40);
			//set the connector
			leftConnector = new Connector(this, "Connector", true);
			leftConnector.ConnectorLocation = ConnectorLocation.West;
			Connectors.Add(leftConnector);			
			//cannot be resized
			IsResizable=false;
			this.OnMouseDown+=new MouseEventHandler(ComboControl_OnMouseDown);
			textbox = new NTextBox(this, 50,15);
			textbox.OnResize+=new Netron.GraphLib.NTextBox.ResizeInfo(combo_OnResize);
			
			this.Text = "Physicists";
			this.Controls.Add(textbox);
			this.IsResizable = false;
			
			
		}
		public TextBoxControlShape(IGraphSite site) : base(site)		
		{
			//set the default size
			Rectangle = new RectangleF(0, 0, 70, 40);
			//set the connector
			leftConnector = new Connector(this, "Connector", true);
			leftConnector.ConnectorLocation = ConnectorLocation.West;
			Connectors.Add(leftConnector);
			//cannot be resized
			IsResizable=false;
		}

		#endregion
		
		#region Methods
		/// <summary>
		/// Overrides the default thumbnail used in the shape viewer
		/// </summary>
		/// <returns></returns>
		public override Bitmap GetThumbnail()
		{
			Bitmap bmp=null;
			try
			{
				Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.GraphLib.Entitology.Resources.ComboControlShape.gif");
					
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
		/// </summary>
		/// <param name="g">The graphics canvas onto which to paint</param>
		public override void Paint(Graphics g)
		{
			//g.FillRectangle(Brushes.Red,this.Rectangle.X-100,this.Rectangle.Y-100,20,20);
			
			if(RecalculateSize)
			{
				Rectangle = new RectangleF(new PointF(Rectangle.X,Rectangle.Y),	g.MeasureString(Text,Font));	
				RecalculateSize = false; //very important!
			}
			g.FillRectangle(BackgroundBrush, Rectangle.X, Rectangle.Y, Rectangle.Width + 1, Rectangle.Height + 1);
			g.DrawRectangle(Pen, Rectangle.X, Rectangle.Y, Rectangle.Width + 1, Rectangle.Height + 1);
			textbox.Location = new Point((int) Rectangle.X+10,(int) Rectangle.Y+20);
			textbox.Paint(g);
			
			if (ShowLabel)
			{
				StringFormat sf = new StringFormat();
				sf.Alignment = StringAlignment.Center;
				g.DrawString(Text, Font, TextBrush, Rectangle.X + (Rectangle.Width / 2), Rectangle.Y + 3, sf);
			}	
			
			base.Paint(g);
		}

		/// <summary>
		/// Returns a floating-point point coordinates for a given connector
		/// </summary>
		/// <param name="c">A connector object</param>
		/// <returns>A floating-point pointF</returns>
		public override PointF ConnectionPoint(Connector c)
		{
			
			if (c == leftConnector) return new PointF(Rectangle.Left, Rectangle.Top +(Rectangle.Height*1/2));			
			return new PointF(0, 0);
		}

		public override Cursor GetCursor(PointF p)
		{
			if(textbox.Hit(Point.Round(p)))
				return Cursors.Hand;
			return base.GetCursor(p);
		}
		private void ComboControl_OnMouseDown(object sender, MouseEventArgs e)
		{
			textbox.OnMouseDown(e);
		}

		private void combo_OnResize(SizeF newSize)
		{
			this.Width = newSize.Width + 10;
		}
		public override bool Hit(RectangleF r)
		{
			if(textbox.Expanded)
			{
				return textbox.Hit(Point.Round(r.Location));
			}
			else
				return base.Hit(r);
		}
	
//		public override void AddProperties()
//		{
//			base.AddProperties ();
//			bag.Properties.Add(new PropertySpec("ListItems",typeof(NListItemCollection),"ComboBox","The list items of the textbox box."));//,new ComboItemCollection(),typeof(UITypeEditor),typeof(ExpandableObjectConverter)));
//		}
//		protected override void GetPropertyBagValue(object sender, PropertySpecEventArgs e)
//		{
//			base.GetPropertyBagValue (sender, e);
//			switch(e.Property.Name)
//			{
//				case "ListItems":
//					e.Value = this.textbox.ListItems; break;
//
//			}
//		}
//
//		protected override void SetPropertyBagValue(object sender, PropertySpecEventArgs e)
//		{
//			base.SetPropertyBagValue (sender, e);
//			switch(e.Property.Name)
//			{
//				case "ListItems":
//					this.textbox.ListItems = (NListItemCollection) e.Value; break;
//			}
//		}


		public override void OnKeyDown(KeyEventArgs e)
		{
			textbox.OnKeyDown(e);
		}

		public override void OnKeyPress(KeyPressEventArgs e)
		{
			textbox.OnKeyPress(e);
		}

		public new bool IsSelected
		{
			get
			{
				return base.IsSelected;
			}
			set
			{
				base.IsSelected = value;
				if(!value) this.textbox.Editing = false;
			}
		}

		#endregion	

	}

	
	
	


}