using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Netron.Neon
{
	/// <summary>
	/// Rounded flat button
	/// </summary>
	public class NButton : Button, INButton
	{
		#region Enum
		public enum Rounding
		{
			/// <summary>
			/// No rounding
			/// </summary>
			None,
			/// <summary>
			/// The upper corners are rounded off
			/// </summary>
			Up,
			/// <summary>
			/// The lower corners are rounded off
			/// </summary>
			Down
		}
		#endregion

		#region Fields
		/// <summary>
		/// whether the gradient is bell-shaped
		/// </summary>
		private bool bellShaped = true;
		private INUIMediator root;
		private Color lightColor = Color.WhiteSmoke;
		private Color darkColor = Color.LightSlateGray;
	
		private Brush textBrush;
		private Rectangle rectangle;
		private Rounding rounded = Rounding.None;
		private GraphicsPath path;
		private Region region;
		private readonly int bshift =25;
		private Point[] points;
		/// <summary>
		/// the center of the bell, between 0 and 1
		/// </summary>
		private float bellCenter = 0.17f;
		/// <summary>
		/// the falloff of the bell, between 0 and 1
		/// </summary>
		private float bellFalloff = 0.67f;
		private LinearGradientBrush backBrush;
		/// <summary>
		/// the angle of the gradient
		/// </summary>
		private float gradientAngle = 90F;
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
		/// Gets or sets the root of the mediator
		/// </summary>
		public INUIMediator Root
		{
			get
			{

				return root;
			}
			set
			{
				root = value;
			}
		}

		[Category("Netron"), Description("The caption"), Browsable(true)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
				this.Invalidate();
			}
		}


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

		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				base.Font = value;
				SetBrush();
				this.Invalidate();
			}
		}

		[Category("Netron"), Description("Whether the shape is rounded at the bottom."), Browsable(true)]
		public Rounding Rounded
		{
			get{return rounded;}
			set{rounded = value; SetPoints(); this.Invalidate();}
		}

		#endregion

		#region Constructor
		public NButton()
		{
			//enable double buffering of graphics
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);

			rectangle = new Rectangle(0,0,Width,Height);
			
		}
		#endregion

		#region Methods
		/// <summary>
		/// Sets the background brush
		/// </summary>
		private void SetBrush()
		{
			backBrush = new System.Drawing.Drawing2D.LinearGradientBrush(rectangle,lightColor,darkColor,-90F);
			if(bellShaped)
				backBrush.SetSigmaBellShape(bellCenter,bellFalloff);
			textBrush = new SolidBrush(this.ForeColor);
			
		}
	

		/// <summary>
		/// Sets the color scheme
		/// </summary>
		/// <param name="scheme"></param>
		public void SetColorScheme(UIColorScheme scheme)
		{
			switch(scheme)
			{
				case UIColorScheme.SkyBlue:
					this.lightColor = Color.WhiteSmoke;
					this.darkColor = Color.SteelBlue;							
					break;
				case UIColorScheme.Dark:
					this.lightColor = Color.WhiteSmoke;
					this.darkColor = Color.SlateGray;				
					break;
				case UIColorScheme.Grey:
					this.lightColor = Color.WhiteSmoke;
					this.darkColor = Color.Silver;				
					break;
				case UIColorScheme.Colorful:
					this.lightColor = Color.LightYellow;
					this.darkColor = Color.OrangeRed;				
					break;



			}
			this.BackColor = lightColor;
			this.ForeColor = darkColor;
			SetBrush();
			Invalidate();
		}

		/// <summary>
		/// Sets the UI style
		/// </summary>
		/// <param name="style"></param>
		public void SetStyle(UIStyle style)
		{
			return;
		}
		
		/// <summary>
		/// Handles the resize event
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(EventArgs e)
		{
			base.OnResize (e);
			rectangle = new Rectangle(0,0,Width,Height);
			SetBrush();
			SetPoints();
		}

		/// <summary>
		/// Sets the points for drawing the corners
		/// </summary>
		protected void SetPoints()
		{
			switch(rounded)
			{
				case Rounding.Down:
				{
					points = new Point[8]{
											 new Point(rectangle.X,rectangle.Y),
											 new Point(rectangle.X, rectangle.Y+bshift),
											 new Point(rectangle.X+Height-bshift,rectangle.Y+Height),
											 new Point(rectangle.X+Height,rectangle.Y+Height),
											 new Point(rectangle.X+Width-Height,rectangle.Y+Height),
											 new Point(rectangle.X+Width-Height+bshift,rectangle.Y+Height),
											 new Point(rectangle.X+Width,rectangle.Y+bshift),
											 new Point(rectangle.X+Width,rectangle.Y)											 
										 };
					path = new GraphicsPath();
					path.AddBezier(points[0],points[1],points[2],points[3]);
					path.AddLine(points[3],points[4]);
					path.AddBezier(points[4],points[5],points[6],points[7]);			
					path.AddLine(points[7],points[0]);						
					region = new Region(path);					
					
				}
					break;

				case Rounding.Up:
				{
					points = new Point[8]{
											 new Point(rectangle.Left,rectangle.Bottom),
											 new Point(rectangle.Left, rectangle.Bottom-bshift),
											 new Point(rectangle.Left+Height-bshift,rectangle.Top),
											 new Point(rectangle.Left+Height,rectangle.Top),
											 new Point(rectangle.Right-Height,rectangle.Top),
											 new Point(rectangle.Right-Height+bshift,rectangle.Top),
											 new Point(rectangle.Right,rectangle.Bottom-bshift),
											 new Point(rectangle.Right,rectangle.Bottom) 
										 };
					path = new GraphicsPath();
					path.AddBezier(points[0],points[1],points[2],points[3]);
					path.AddLine(points[3],points[4]);
					path.AddBezier(points[4],points[5],points[6],points[7]);			
					path.AddLine(points[7],points[0]);						
					region = new Region(path);			
				}
					break;
			}
		}
		/// <summary>
		/// Paints the button
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			e.Graphics.FillRectangle(new SolidBrush(BackColor),rectangle);
			if(rounded==Rounding.None)
				e.Graphics.FillRectangle(backBrush,e.ClipRectangle);			
			else
				e.Graphics.FillRegion(backBrush,region);
			
			if(Text.Trim()!=string.Empty)
			{
				StringFormat sf = new StringFormat();
				sf.Alignment = StringAlignment.Center;				
				e.Graphics.DrawString(this.Text,this.Font,new SolidBrush(ForeColor),new PointF(Width/2,Height/2-e.Graphics.MeasureString(Text,Font).Height*0.5F),sf);
			}
			e.Graphics.DrawPath(new Pen(darkColor,1) ,path);

			

					
		}

		
		#region Mouse event handlers
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown (e);
			Point p = PointToScreen(new Point(e.X,e.Y));
			if(e.Button==MouseButtons.Left)
			{
				//root.FileMenu.Show(this,p);
				
			}
		}

		protected override void OnMouseEnter(EventArgs e)
		{
		
			this.Invalidate(this.ClientRectangle);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
		
			this.Invalidate(this.ClientRectangle);
		}

		#endregion


		#endregion

	}
}
