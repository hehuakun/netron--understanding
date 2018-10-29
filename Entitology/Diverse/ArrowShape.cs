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
	[Description("Arrow shape")]
	[NetronGraphShape("Arrow shape","B05DD908-C256-472e-8602-9D8117ADC166","Special shapes","Netron.GraphLib.Entitology.ArrowShape",
		 "An arrow shape.")]
	public class ArrowShape : Shape, ISerializable
	{
		#region Fields

		private float mAngle = 45F;
		private float mXShift = 30F;
		private float mYShift = 10F;
		
		private float sqr;
		private bool doRotate;
		private bool doArrow;
		private PointF rotationPoint;
		private PointF arrowPoint;
		private PointF fixPoint;
		private PointF[] points;
		private PointF upperPoint, leftPoint;
		/*															
		   +-------------------------------------------------------+
		    |																		|
		    |mYShift     					   upperPoint+\				|		
		    |           										   |	\			|
		    |													   |		\		|
leftPoint+----------mXShift-----------------------+          \    |		
			|											arrowPoint			 \  -|rotationPoint
			|																	 /	|
			+------------------------------------------+		  /  	|
			|													   |		/		|
			|													   |	/			|
			|													   +/				|
			|																		|
			+------------------------------------------+------------+
		
		 
		 */
		#endregion
		
		#region Constructor

		/// <summary>
		/// Constructor
		/// </summary>
		public ArrowShape() : base()
		{
			Rectangle = new RectangleF(0, 0, 100, 100);
			this.OnMouseDown +=new MouseEventHandler(ArrowShape_OnMouseDown);
			this.OnMouseMove+=new MouseEventHandler(ArrowShape_OnMouseMove);
			this.OnMouseUp+=new MouseEventHandler(ArrowShape_OnMouseUp);			
			IsResizable=true;
			ShapeColor = Color.Red;
			this.IsSquare = true;
		}
		/// <summary>
		/// This is the default constructor of the class.
		/// </summary>
		public ArrowShape(IGraphSite site) : base(site)
		{
			//set the default size
			Rectangle = new RectangleF(0, 0, 100, 100);
			ShapeColor = Color.Red;
			IsResizable=true;
			this.IsSquare = true;
		}

		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected ArrowShape(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.IsSquare = true;

			this.OnMouseDown +=new MouseEventHandler(ArrowShape_OnMouseDown);
			this.OnMouseMove+=new MouseEventHandler(ArrowShape_OnMouseMove);
			this.OnMouseUp+=new MouseEventHandler(ArrowShape_OnMouseUp);			

			this.mAngle = info.GetSingle("mAngle");
			this.mXShift = info.GetSingle("mXShift");
			this.mYShift = info.GetSingle("mYShift");
			
		}
		#endregion	

		#region Properties
	
		public float Angle
		{
			get{return mAngle;}
			set{mAngle = value;}
		}

		public float XShift
		{
			get{return mXShift;}
			set{mXShift = value;}
		}

		public float YShift
		{
			get{return mYShift;}
			set{mYShift = value;}
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
				Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.GraphLib.BasicShapes.Resources.ArrowShape.gif");
					
				bmp= Bitmap.FromStream(stream) as Bitmap;
				stream.Close();
				stream=null;
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.Message,"ArrowShape.GetThumbnail");
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

			Matrix m = g.Transform; //keep reference to the current transform
			
			points = new PointF[]{
								  new PointF(Rectangle.X, Rectangle.Y + mYShift),
								  new PointF(Rectangle.X+mXShift, Rectangle.Y + mYShift),
								  new PointF(Rectangle.X+mXShift, Rectangle.Y + mYShift - 10),
								  new PointF(Rectangle.Right, Rectangle.Y + Rectangle.Height/2),
								  new PointF(Rectangle.X +mXShift, Rectangle.Bottom-mYShift+10),
								  new PointF(Rectangle.X +mXShift, Rectangle.Bottom - mYShift),
								  new PointF(Rectangle.X, Rectangle.Bottom - mYShift)
							  };
			GraphicsPath path = new GraphicsPath();
			path.AddPolygon(points);			
			g.TranslateTransform(Rectangle.X+Rectangle.Width/2,Rectangle.Y+Rectangle.Height/2);
			g.RotateTransform(mAngle,MatrixOrder.Prepend);		
			g.TranslateTransform(-Rectangle.X - Rectangle.Width/2,-Rectangle.Y - Rectangle.Height/2);
			g.FillPath(this.BackgroundBrush, path);
			if(IsSelected)		
			{
				g.FillEllipse(Brushes.Green, Rectangle.Right-5, Rectangle.Y + Rectangle.Height/2-2 ,5,5);
				g.FillEllipse(Brushes.Yellow, Rectangle.X+mXShift, Rectangle.Y + mYShift-2 ,5,5);
			}
			rotationPoint = new PointF(Rectangle.Right, Rectangle.Y + Rectangle.Height/2);
			upperPoint = new PointF(Rectangle.X+mXShift, Rectangle.Y );
			leftPoint = new PointF(Rectangle.X, Rectangle.Y + mYShift);
			arrowPoint = new PointF(Rectangle.X+mXShift, Rectangle.Y + mYShift-2 );
			PointF[] rp = new PointF[]{rotationPoint, upperPoint, leftPoint, arrowPoint};
			g.Transform.TransformPoints(rp); //let the framework compute the new coordinates, much safer and mathematically correct
			g.Transform.TransformPoints(points);
			rotationPoint = rp[0];			
			upperPoint = rp[1];
			leftPoint = rp[2];
			arrowPoint = rp[3];
			// Reset world transformation.
			g.Transform = m;

			
		}

		


		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData (info, context);
			info.AddValue("mAngle", this.mAngle);
			info.AddValue("mXShift", this.mXShift);
			info.AddValue("mYShift", this.mYShift);		
		}

		
		


		
			#endregion

		public override Cursor GetCursor(PointF p)
		{
			// Get current mouse point adjusted by the current scroll position and zoom factor
			//p = new PointF(p.X - Site.AutoScrollPosition.X, p.Y - Site.AutoScrollPosition.Y);
			//p = Site.UnzoomPoint(Point.Round(p));
			RectangleF r1 = new RectangleF(rotationPoint.X-10,rotationPoint.Y-10,25,25);
			RectangleF r2 = new RectangleF(arrowPoint.X-10,arrowPoint.Y-10,25,25);
			if((r1.Contains(p) || r2.Contains(p)) && IsSelected) return Cursors.Cross;
			else return base.GetCursor(p);
		}

		

		private void ArrowShape_OnMouseDown(object sender, MouseEventArgs e)
		{
			RectangleF r1 = new RectangleF(rotationPoint.X-10,rotationPoint.Y-10,25,25);
			RectangleF r2 = new RectangleF(arrowPoint.X-10,arrowPoint.Y-10,25,25);
			if(r1.Contains(e.X,e.Y) && IsSelected) 
			{
				doRotate = true;
				
			}
			else if(r2.Contains(e.X,e.Y) && IsSelected) 
			{
				doArrow = true;
			}
			fixPoint = new PointF(Rectangle.X, Rectangle.Y);
			
		}

		private void ArrowShape_OnMouseMove(object sender, MouseEventArgs e)
		{
			sqr = Rectangle.Width;
			
			PointF reference = new PointF(fixPoint.X+sqr/2, fixPoint.Y+sqr/2);
			if(doRotate && IsSelected)
			{	
				Rectangle =new RectangleF(fixPoint.X,fixPoint.Y, sqr,sqr);
				if(e.X>=reference.X)
				{
					if(e.Y>=reference.Y) //SE
					{
						mAngle =(float) ( Math.Atan2(e.Y-reference.Y,e.X-reference.X)*180/Math.PI);
					}
					else //NE
					{
						mAngle = 270F+ (float)(Math.Atan2(e.X-reference.X,reference.Y-e.Y)*180/Math.PI);
					}
				}
				else
				{
					if(e.Y>=reference.Y) //SW
					{
						mAngle = 90F+ (float)(Math.Atan2(reference.X-e.X,e.Y-reference.Y)*180/Math.PI);
					}
					else //NW
					{
						mAngle = 270F-(float)(Math.Atan2(reference.X-e.X,reference.Y-e.Y)*180/Math.PI);
					}
				}
				Invalidate();
			}
			else if(doArrow)
			{
				//Rectangle =new RectangleF(fixPoint.X,fixPoint.Y, sqr,sqr);
				mXShift =(float)  Math.Max(Math.Sqrt((leftPoint.X-e.X)*(leftPoint.X-e.X) + (leftPoint.Y-e.Y)*(leftPoint.Y-e.Y)),10);
				mYShift = (float)  Math.Max(Math.Sqrt((upperPoint.X-e.X)*(upperPoint.X-e.X) + (upperPoint.Y-e.Y)*(upperPoint.Y-e.Y)),10);
				IsSelected = false;				
			}

			
		}

		private void ArrowShape_OnMouseUp(object sender, MouseEventArgs e)
		{
			doRotate = false;
			doArrow = false;
			IsSelected = true;
		}
		}

}







		

