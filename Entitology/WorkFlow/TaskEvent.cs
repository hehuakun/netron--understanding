using System.Diagnostics;
using System.Reflection;
using System.IO;
using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using Netron.GraphLib;
using Netron.GraphLib.Attributes;
using System.Windows.Forms;

namespace Netron.GraphLib.Entitology
{
	
	/// <summary>
	/// Adapted (translated from Spanish and VB.Net to English and C# respectively) shape-code originally developed by Fabian Luque http://www.forumnet.com.ar/blog
	/// </summary>
	[Serializable(), Description("Task/event shape"), NetronGraphShape("TaskEvent", "17E204DA-F026-44b5-8EF9-DCCC150698E2", "Workflow", "Netron.GraphLib.Entitology.TaskEvent", "Task or event")]
	public class TaskEvent : Shape
	{
		
		#region Enums
		/// <summary>
		/// Enumerates the subtypes
		/// </summary>
		public enum SubTypes
		{
			Task,
			Event
		}
		#endregion
		
		#region Fields
		private Connector mCentralConnector;
		private string mTitle;
		private string mSubtitle;
		private string mObservation;
		private SubTypes mSubType = SubTypes.Task;
		private int mID;
		private object mEntity;
		private bool mCollapsed;
		#endregion

		#region Properties
		
		
		public int ID
		{
			get
			{
				return mID;
			}
			set
			{
				mID = value;
			}
		}
		
		public string SubTitle
		{
			get
			{
				return mSubtitle;
			}
			set
			{
				mSubtitle = value;
				this.Invalidate();
			}
		}
		
		public SubTypes SubType
		{
			get
			{
				return mSubType;
			}
			set
			{
				mSubType = value;
				switch (mSubType)
				{
					case SubTypes.Event:
						
						mTitle = "Event";
						break;
					case SubTypes.Task:
						
						mTitle = "Task";
						break;
				}
				this.Invalidate();
			}
		}
		
		public string Observation
		{
			get
			{
				return mObservation;
			}
			set
			{
				mObservation = value;
				this.Invalidate();
			}
		}
		
		public object Entity
		{
			get
			{
				return mEntity;
			}
			set
			{
				mEntity = value;
			}
		}
		#endregion
		
		#region Constructor
		public TaskEvent()
		{
			mTitle = "Task";
			mSubtitle = "Here you can set a sub-title.";
			mObservation = "Observation text" + Environment.NewLine + "...";
			Rectangle = new RectangleF(0, 0, 200, 100);
			
			mCentralConnector = new Connector(this, "", true);
			mCentralConnector.ConnectorLocation = ConnectorLocation.North;
			Connectors.Add(mCentralConnector);
			mCollapsed = true;
			
			IsResizable = false;
			this.OnMouseUp+=new MouseEventHandler(TaskEvent_OnMouseUp);
		}
		#endregion
				
		#region Overrides

		public override Bitmap GetThumbnail()
		{
			Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.GraphLib.Entitology.Resources.TaskEvent.gif");
					
			Bitmap bmp= Bitmap.FromStream(stream) as Bitmap;
			stream.Close();
			stream=null;
			return bmp;
		}

		public override void Paint (Graphics g)
		{
			base.Paint(g);
			
			if (mCollapsed)
			{
				Rectangle = new RectangleF(Rectangle.X, Rectangle.Y, 200, 50);
			}
			else
			{
				Rectangle = new RectangleF(Rectangle.X, Rectangle.Y, 200, 100);
			}
			DashStyle estiloAnterior = Pen.DashStyle;
			float anchoAnterior = Pen.Width;
			Color colorAnterior = Pen.Color;
			if (this.IsSelected)
			{
				Pen.DashStyle = DashStyle.Solid;
				Pen.Color = Color.Orange;
				Pen.Width = 2;
			}
			else
			{
				Pen.DashStyle = DashStyle.Dash;
			}
			
			
			GraphicsPath path = new GraphicsPath();			
			path.AddArc(Rectangle.X, Rectangle.Y, 20, 20, -180, 90);			
			path.AddLine(Rectangle.X + 10, Rectangle.Y, Rectangle.X + Rectangle.Width - 10, Rectangle.Y);			
			path.AddArc(Rectangle.X + Rectangle.Width - 20, Rectangle.Y, 20, 20, -90, 90);			
			path.AddLine(Rectangle.X + Rectangle.Width, Rectangle.Y + 10, Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height - 10);			
			path.AddArc(Rectangle.X + Rectangle.Width - 20, Rectangle.Y + Rectangle.Height - 20, 20, 20, 0, 90);			
			path.AddLine(Rectangle.X + Rectangle.Width - 10, Rectangle.Y + Rectangle.Height, Rectangle.X + 10, Rectangle.Y + Rectangle.Height);			
			path.AddArc(Rectangle.X, Rectangle.Y + Rectangle.Height - 20, 20, 20, 90, 90);			
			path.AddLine(Rectangle.X, Rectangle.Y + Rectangle.Height - 10, Rectangle.X, Rectangle.Y + 10);			
			//shadow
			Region darkRegion = new Region(path);
			darkRegion.Translate(5, 5);
			g.FillRegion(new SolidBrush(Color.FromArgb(20, Color.Black)), darkRegion);
			
			//background
			g.FillPath(new SolidBrush(Color.White), path);
			
			if (mCollapsed)
			{
				//Pinto el gradiente
				Brush unBrush = new LinearGradientBrush(new Point(((int)(Rectangle.X)),((int)(Rectangle.Y))), new Point(((int)(Rectangle.X + Rectangle.Width)),((int)(Rectangle.Y))), ShapeColor, Color.White);
				Region unaRegion = new Region(path);
				g.FillRegion(unBrush, unaRegion);
			}
			else
			{
				
				GraphicsPath gradientPath = new GraphicsPath();				
				gradientPath.AddArc(Rectangle.X + 1, Rectangle.Y + 1, 18, 18, -180, 90);				
				gradientPath.AddLine(Rectangle.X + 11, Rectangle.Y + 1, Rectangle.X + Rectangle.Width - 11, Rectangle.Y + 1);				
				gradientPath.AddArc(Rectangle.X + Rectangle.Width - 19, Rectangle.Y + 1, 18, 18, -90, 90);				
				gradientPath.AddLine(Rectangle.X + Rectangle.Width - 1, Rectangle.Y + 50, Rectangle.X + 1, Rectangle.Y + 50);				
				//gradient
				Brush unBrush = new LinearGradientBrush(new Point(((int)(Rectangle.X)),((int)(Rectangle.Y))), new Point(((int)(Rectangle.X + Rectangle.Width)),((int)(Rectangle.Y))), ShapeColor, Color.White);
				Region unaRegion = new Region(gradientPath);
				g.FillRegion(unBrush, unaRegion);
			}
			
			//the border
			g.DrawPath(Pen, path);
			
			Pen.DashStyle = estiloAnterior;
			Pen.Width = anchoAnterior;
			Pen.Color = colorAnterior;
			
			switch (mSubType)
			{
				case SubTypes.Task:
					
					g.DrawImage(new Bitmap(this.GetType(), "Resources.task.ico"),((int)(Rectangle.X + 5)),((int)(Rectangle.Y + 5)));
					break;
				case SubTypes.Event:
					
					g.DrawImage(new Bitmap(this.GetType(), "Resources.event.ico"),((int)(Rectangle.X + 5)),((int)(Rectangle.Y + 5)));
					break;
			}
			
			StringFormat sf = new StringFormat();
			sf.Trimming = StringTrimming.EllipsisCharacter;
			g.DrawString(mTitle, new Font("Verdana", 10, FontStyle.Bold), TextBrush, Rectangle.X + 20, Rectangle.Y + 5);
			g.DrawString(mSubtitle, new Font("Verdana", 8), TextBrush, new RectangleF(Rectangle.X + 5, Rectangle.Y + 22, Rectangle.Width - 10, 28), sf);
			if (mCollapsed)
			{
				g.DrawImage(new Bitmap(this.GetType(), "Resources.down.ico"),((int)(Rectangle.Right - 20)),((int)(Rectangle.Y + 5)));
			}
			else
			{
				g.DrawImage(new Bitmap(this.GetType(), "Resources.up.ico"),((int)(Rectangle.Right - 20)),((int)(Rectangle.Y + 5)));
				g.DrawString(mObservation, new Font("Verdana", 8), TextBrush, new RectangleF(Rectangle.X + 5, Rectangle.Y + 55, Rectangle.Width - 10, 40), sf);
			}
		}
		
		public override PointF ConnectionPoint(Connector c)
		{
			if (c == mCentralConnector)
			{
				return new PointF(Rectangle.Left + Rectangle.Width / 2, Rectangle.Top + Rectangle.Height / 2);
			}
			else
			{
				return new PointF(0, 0);
			}
		}
		
		public System.Drawing.PointF ConnectionPoint(GraphLib.Connector c, System.Drawing.PointF initialPoint, System.Drawing.PointF finalPoint)
		{
			PointF sourcePt = new PointF();
			RectangleF shapeRect =  this.Rectangle;
			if (Intersects(initialPoint, 
							finalPoint,
							new PointF(shapeRect.Left, shapeRect.Top),
							new PointF(shapeRect.Right, shapeRect.Top),
							ref sourcePt) )
				return sourcePt;
            if(Intersects(initialPoint, finalPoint, new PointF(shapeRect.Left, shapeRect.Top), new PointF(shapeRect.Left, shapeRect.Bottom), ref sourcePt))
				return sourcePt;
			if(Intersects(initialPoint, finalPoint, new PointF(shapeRect.Left, shapeRect.Bottom), new PointF(shapeRect.Right, shapeRect.Bottom), ref sourcePt))
				return sourcePt;
            if(Intersects(initialPoint, finalPoint, new PointF(shapeRect.Right, shapeRect.Top), new PointF(shapeRect.Right, shapeRect.Bottom),ref sourcePt))
				return sourcePt;

			return this.ConnectionPoint(c);
			
		}
				
		private bool Intersects(PointF line1Point1, PointF line1Point2, PointF line2Point1, PointF line2Point2, ref PointF intersection)
		{
			//Based on the 2d line intersection method from "comp.graphics.algorithmsFrequently Asked Questions"
			//			(Ay-Cy)(Dx-Cx)-(Ax-Cx)(Dy-Cy)
			//		r = -----------------------------  (eqn 1)
			//			(Bx-Ax)(Dy-Cy)-(By-Ay)(Dx-Cx)
					
			double q = (line1Point1.Y - line2Point1.Y) *(line2Point2.X - line2Point1.X) -(line1Point1.X - line2Point1.X) *(line2Point2.Y - line2Point1.Y);
			double d = (line1Point2.X - line1Point1.X) *(line2Point2.Y - line2Point1.Y) -(line1Point2.Y - line1Point1.Y) *(line2Point2.X - line2Point1.X);
					
			//parallel lines so no intersection anywhere in space (in curved space, maybe, but not here in Euclidian space.)
			if (d == 0)
			{
				return false;
			}
					
			double r = q / d;
					
			//		   (Ay-Cy)(Bx-Ax)-(Ax-Cx)(By-Ay)
			//	   s = -----------------------------  (eqn 2)
			//		   (Bx-Ax)(Dy-Cy)-(By-Ay)(Dx-Cx)
					
			q = (line1Point1.Y - line2Point1.Y) *(line1Point2.X - line1Point1.X) -(line1Point1.X - line2Point1.X) *(line1Point2.Y - line1Point1.Y);
			double s = q / d;
					
			//If r>1, P is located on extension of AB
			//If r<0, P is located on extension of BA
			//If s>1, P is located on extension of CD
			//If s<0, P is located on extension of DC
					
			//The above basically checks if the intersection is located at an extrapolated
			//point outside of the line segments. To ensure the intersection is only(within)
			//the line segments then the above must all be false, ie r between 0 and 1
			//and s between 0 and 1.
					
			if (r < 0 | r > 1 | s < 0 | s > 1)
			{
				return false;
			}
					
			//Px = Ax + r(Bx - Ax)
			//Py = Ay + r(By - Ay)
					
			intersection.X = line1Point1.X +(float)(0.5 + r * (line1Point2.X - line1Point1.X));
			intersection.Y = line1Point1.Y +(float)(0.5 + r * (line1Point2.Y - line1Point1.Y));
					
			return true;
		}
				
		public override void AddProperties ()
		{
			base.AddProperties();
		
			Bag.Properties.Add(new PropertySpec("Title", typeof(string)));
			Bag.Properties.Add(new PropertySpec("Subtitle",  typeof(string)));			
			Bag.Properties.Add(new PropertySpec("Observations",  typeof(string)));

			#region The subtype is a reflected Enum type, hence this construction.
			PropertySpec specSubType = new PropertySpec("SubType",typeof(string),"Appearance","Gets or sets the sub-type of the shape.","Task",typeof(ReflectedEnumStyleEditor),typeof(TypeConverter));
			ArrayList list = new ArrayList();
			string[] names = Enum.GetNames(typeof(SubTypes));
			for(int k =0; k<names.Length; k++)			
				list.Add(names[k]);		
			specSubType.Attributes = new Attribute[]{ new ReflectedEnumAttribute(list)};
			Bag.Properties.Add(specSubType);
			//bag.Properties.Add(new PropertySpec("Type", typeof(string),"Test","The shape's sub-type","Task",typeof(ReflectedEnumStyleEditor),typeof(TypeConverter))  );
			#endregion

			
		}
		#region PropertyBag
		protected override void GetPropertyBagValue (object sender, PropertySpecEventArgs e)
		{
			base.GetPropertyBagValue(sender, e);
					
			switch (e.Property.Name)
			{
				case "Title":
							
					e.Value = mTitle;
					break;
				case "Subtitle":
							
					e.Value = mSubtitle;
					break;
				case "Observations":
							
					e.Value = mObservation;
					break;
				case "SubType":
							
					e.Value = mSubType.ToString();
					break;
				
			}
		}
				
		protected override void SetPropertyBagValue (object sender, PropertySpecEventArgs e)
		{
			base.SetPropertyBagValue(sender, e);
			switch (e.Property.Name)
			{
				
				case "Title":
							
					//use the logic and the constraint of the object that is being reflected
					if (!(e.Value.ToString() == null))
					{
						mTitle = System.Convert.ToString(e.Value);
					}
					else
					{
						//MessageBox.Show("Not a valid label", "Invalid label", MessageBoxButtons.OK, MessageBoxIcon.Warning)
						this.Invalidate();
					}
					break;
				case "Subtitle":
							
					//use the logic and the constraint of the object that is being reflected
					if (!(e.Value.ToString() == null))
					{
						mSubtitle = System.Convert.ToString(e.Value);
					}
					else
					{
						//MessageBox.Show("Not a valid label", "Invalid label", MessageBoxButtons.OK, MessageBoxIcon.Warning)
						this.Invalidate();
					}
					break;
				case "SubType":
							
					SubType = (SubTypes) Enum.Parse(typeof(SubTypes), e.Value.ToString());
					break;
				case "Observations":
							
					if (!(e.Value.ToString() == null))
					{
						mObservation = System.Convert.ToString(e.Value);
					}
					else
					{
						//MessageBox.Show("Not a valid label", "Invalid label", MessageBoxButtons.OK, MessageBoxIcon.Warning)
						this.Invalidate();
					}
					break;
			}
		}
		#endregion
		#endregion
				
		#region Mouse handler
		private void TaskEvent_OnMouseUp (object sender, System.Windows.Forms.MouseEventArgs e)
		{
			RectangleF collapseRect = new RectangleF(((int)(Rectangle.Right - 20)), ((int)(Rectangle.Y + 5)), 16, 16);
					
			if (collapseRect.Contains(e.X, e.Y))
			{
				mCollapsed = ! mCollapsed;
				this.Invalidate();
			}
		}
		#endregion
				
	}
			
}
