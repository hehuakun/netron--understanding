using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Windows.Forms;
namespace Netron.Neon
{
	/// <summary>
	/// Summary description for NStatusBar.
	/// </summary>
	public class NStatusBar : Panel
	{
		#region Fields
		/// <summary>
		/// the angle of the gradient
		/// </summary>
		private float gradientAngle = 90F;
		/// <summary>
		/// the shadow pen
		/// </summary>
		private Pen shadowPen;		
		/// <summary>
		/// whether the gradient is bell-shaped
		/// </summary>
		private bool bellShaped = true;
		/// <summary>
		/// the center of the bell, between 0 and 1
		/// </summary>
		private float bellCenter = 0.17f;
		/// <summary>
		/// the falloff of the bell, between 0 and 1
		/// </summary>
		private float bellFalloff = 0.67f;
		private LinearGradientBrush backBrush;
		private Brush textBrush;
		private Rectangle rectangle;
		private Color lightColor = Color.WhiteSmoke;
		private Color darkColor = Color.LightSlateGray;
		private int x,y;
		private bool tracking = false;
		#endregion


		#region Properties
		/// <summary>
		/// The center of the bell-shaped gradient is a floating value between 0 and 1.
		/// </summary>
		[Category("Netron"), Description("The center of the bell-shaped gradient is a floating value between 0 and 1."), Browsable(true)]
		public  float BellCenter
		{
			get
			{
				return bellCenter;
			}
			set
			{
				bellCenter= value;
				SetBrush();
				this.Invalidate();
			}
		}
		/// <summary>
		/// The fall-off of the  bell-shaped gradient is a floating value between 0 and 1.
		/// </summary>
		[Category("Netron"), Description("The fall-off of the  bell-shaped gradient is a floating value between 0 and 1."), Browsable(true)]
		public  float BellFalloff
		{
			get
			{
				return bellFalloff;
			}
			set
			{
				bellFalloff= value;
				SetBrush();
				this.Invalidate();
			}
		}
		/// <summary>
		/// Whether the gradient is bell-shaped
		/// </summary>
		[Category("Netron"), Description("Whether the gradient is bell-shaped"), Browsable(true)]
		public  bool BellShaped
		{
			get
			{
				return bellShaped;
			}
			set
			{
				bellShaped = value;
				SetBrush();
				this.Invalidate();
			}
		}

		/// <summary>
		/// The gradient angle
		/// </summary>
		[Category("Netron"), Description("The gradient angle"), Browsable(true)]
		public  float GradientAngle
		{
			get
			{
				return gradientAngle;
			}
			set
			{
				gradientAngle = value;
				SetBrush();
				this.Invalidate();
			}
		}
		/// <summary>
		/// The lighter gradient color
		/// </summary>
		[Category("Netron"), Description("The lower gradient color"), Browsable(true)]
		public Color LightColor
		{
			get{return lightColor;}
			set
			{
				lightColor = value;				
				SetBrush();
				this.Invalidate();
			}
		}
		/// <summary>
		/// The darker gradient color
		/// </summary>
		[Category("Netron"), Description("The upper gradient color"), Browsable(true)]
		public Color DarkColor
		{
			get{return darkColor;}
			set
			{
				darkColor = value; 
				SetBrush();
				this.Invalidate();}
		}

		#endregion

		public NStatusBar()
		{
			rectangle = new Rectangle(0,0,Width,Height);
			//enable double buffering of graphics
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);

			SetBrush();

		}

		protected override void OnPaint(PaintEventArgs e)
		{
			
			//Point p = new Point(this.Location.X+this.rectangle.Width,this.Location.Y + Height);
			//ControlPaint.FillReversibleRectangle(new Rectangle(p, new Size(50, this.Height)),Color.Gray);
			//ControlPaint.DrawSizeGrip(e.Graphics,Color.Red,this.Location.X+this.rectangle.Width,this.Top,50, this.Height);
			
			e.Graphics.FillRectangle(backBrush,rectangle);
			Point[] points = new Point[3]{new Point(rectangle.X+rectangle.Width,rectangle.Y),
											new Point(rectangle.X+this.rectangle.Width,rectangle.Y+Height),
											new Point(rectangle.X+this.rectangle.Width+50,rectangle.Y),
										 };
			e.Graphics.FillPolygon(backBrush,points);
			//Point p = PointToScreen(new Point(rectangle.X+rectangle.Width,rectangle.Y));
			ControlPaint.DrawSizeGrip(e.Graphics,Color.WhiteSmoke,rectangle.Width-20,0,Height,Height);

		}
		

		private void SetBrush()
		{
			backBrush = new System.Drawing.Drawing2D.LinearGradientBrush(rectangle,darkColor,lightColor,gradientAngle);
			if(bellShaped)
				backBrush.SetSigmaBellShape(bellCenter,bellFalloff);
			textBrush = new SolidBrush(this.ForeColor);
			Invalidate();
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize (e);
			
			rectangle = new Rectangle(0,0,Width,Height);
			SetBrush();
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			if(e.Button==MouseButtons.Left)
			{
				tracking = true;		
				Capture = true;
				x= e.X;
				y= e.Y;
			}
			else
			{

			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			if(e.Button==MouseButtons.Left)
			{
				tracking = false;
				Capture = false;
			}
			else
			{

			}
		}


		protected override void OnMouseMove(MouseEventArgs e)
		{
			if(tracking)
			{
				Form frm = Parent as Form;
				Point p = PointToScreen(new Point(e.X,e.Y));
				frm.Bounds = new Rectangle( frm.Left,frm.Top,Math.Max(p.X - frm.Left,200), Math.Max(p.Y-frm.Top,100));
				frm.Invalidate();
				//Trace.WriteLine(e.X-x);
				//(Parent as Form).Height+= e.Y-y;				
			}

		}


			

	}
}
