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
	/// Neon titlebar
	/// </summary>
	public class NTitleBar :Panel, INUITitleBar
	{
		
		#region Fields

		/// <summary>
		/// adds the bezier rounding at the left
		/// </summary>
		private bool neon = true;
		/// <summary>
		/// pointer to the root
		/// </summary>
		protected INUIMediator root;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		/// <summary>
		/// the lighter color of the gradient
		/// </summary>
		private Color lightColor = Color.WhiteSmoke;
		/// <summary>
		/// the darker color of the gradient
		/// </summary>
		private Color darkColor = Color.LightSlateGray;
		/// <summary>
		/// the shadow color underneath the bar, if set to visible
		/// </summary>
		private Color shadowColor = Color.Gainsboro;
		/// <summary>
		/// the brush of the main part
		/// </summary>
		private LinearGradientBrush frontBrush;
		/// <summary>
		/// the brush to draw the text
		/// </summary>
		private Brush textBrush;
		/// <summary>
		/// the rectangle wherein the smooth transition occurs
		/// </summary>
		private Rectangle rectangle;
		/// <summary>
		/// tracking bit
		/// </summary>
		private bool tracking = false;
		/// <summary>
		/// last position of the mouse after a press
		/// </summary>
		private int x,y;
		Point p;
		/// <summary>
		/// the popup menu if enable
		/// </summary>
		private System.Windows.Forms.ContextMenu popMenu;

		/// <summary>
		/// the angle of the gradient
		/// </summary>
		private float gradientAngle = 0F;
		/// <summary>
		/// the shadow pen
		/// </summary>
		private Pen shadowPen;
		/// <summary>
		/// whether to draw the shadow
		/// </summary>
		private bool showShadow = true;
		/// <summary>
		/// whether the gradient is bell-shaped
		/// </summary>
		private bool bellShaped = true;
		/// <summary>
		/// the center of the bell, between 0 and 1
		/// </summary>
		private float bellCenter = 0.5f;
		/// <summary>
		/// the falloff of the bell, between 0 and 1
		/// </summary>
		private float bellFalloff = 1.0f;
		/// <summary>
		/// whether the default menu is enabled
		/// </summary>
		private bool enableMenu = true;
		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the root
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
		/// Whether to show the shadow underneath
		/// </summary>
		[Category("Netron"), Description("Whether to show the shadow underneath"), Browsable(true)]
		public  bool ShowShadow
		{
			get
			{
				return showShadow;
			}
			set
			{
				showShadow = value;
				SetBrush();
				this.Invalidate();
			}
		}
		/// <summary>
		/// Whether to show the pop-up menu
		/// </summary>
		[Category("Netron"), Description("Whether to show the pop-up menu"), Browsable(true)]
		public  bool ShowDefaultMenu
		{
			get
			{
				return enableMenu;
			}
			set
			{
				enableMenu = value;				
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
		/// The caption
		/// </summary>
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
		/// The shadow color
		/// </summary>
		[Category("Netron"), Description("The shadow color"), Browsable(true)]
		public Color ShadowColor
		{
			get{return shadowColor;}
			set
			{
				shadowColor = value;				
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

		/// <summary>
		/// Overrides the font to update the brushes
		/// </summary>
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

		/// <summary>
		/// Overrides the ForeColor to update the brushes
		/// </summary>
		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
				SetBrush();
				Invalidate();
			}
		}

		

		#endregion

		#region Constructor
		/// <summary>
		/// Default ctor
		/// </summary>
		public NTitleBar()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			rectangle = new Rectangle(0,0,Width,Height);
			//enable double buffering of graphics
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetBrush();
			
		}
		#endregion

		#region Methods

		protected override void OnCreateControl()
		{
			base.OnCreateControl ();
			BuildMenu();
		}


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
			SetBrush();
			Invalidate();
		}

		public void SetStyle(UIStyle style)
		{
			switch(style)
			{
				case UIStyle.Flat:
					gradientAngle = 0f;
					this.bellShaped = false;
					neon = false;
					break;
				case UIStyle.WinXP:
					gradientAngle = 90f;
					this.bellShaped = true;
					neon = false;
					break;
				case UIStyle.Neon:
					gradientAngle = 90f;
					this.bellShaped = true;
					neon = true;
					break;
			}
			SetBrush();
			Invalidate();
		}
		

		#region Menu handlers
		/// <summary>
		/// Ends the application
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuExit_Click(object sender, EventArgs e)
		{
			(Parent as Form).Close();
			//Application.Exit();
		}
		/// <summary>
		/// Maximizes the window
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuMaximize_Click(object sender, EventArgs e)
		{
			Maximize();
		}

		public void Maximize()
		{
			if(Parent==null) return;
			try
			{
				(Parent as Form).WindowState=FormWindowState.Maximized;
			}
			catch
			{			
				
			}
		}
		public void Minimize()
		{
			if(Parent==null) return;
			try
			{
				(Parent as Form).WindowState=FormWindowState.Minimized;
			}
			catch
			{}
		}
		/// <summary>
		/// Minimizes the window
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuMinimize_Click(object sender, EventArgs e)
		{
			Minimize();
		}

		#endregion

		/// <summary>
		/// Builds the context menu
		/// </summary>
		private void BuildMenu()
		{			
			
			NMenuItem mnuMaximize = new NMenuItem("Maximize");
			mnuMaximize.Click+=new EventHandler(mnuMaximize_Click);
			mnuMaximize.ItemColors.ForeColor = Color.Black;
			mnuMaximize.ItemColors.GradientStartColor = this.lightColor;
			mnuMaximize.ItemColors.GradientEndColor= this.darkColor;
			//mnuMaximize.image = GetImage();
			popMenu.MenuItems.Add(mnuMaximize);
			mnuMaximize.OwnerDraw =true;

			NMenuItem mnuMinimize = new NMenuItem("Minimize");
			mnuMinimize.Click+=new EventHandler(mnuMinimize_Click);			
			mnuMinimize.ItemColors.ForeColor = Color.Black;
			mnuMinimize.ItemColors.GradientStartColor = this.lightColor;
			mnuMinimize.ItemColors.GradientEndColor= this.darkColor;
			popMenu.MenuItems.Add(mnuMinimize);
			mnuMinimize.OwnerDraw =true;

			NMenuItem mnuDash = new NMenuItem("-");
			popMenu.MenuItems.Add(mnuDash);
			mnuDash.OwnerDraw =true;

			NMenuItem mnuExit = new NMenuItem("Exit");
			mnuExit.Click+=new EventHandler(mnuExit_Click);
			
			mnuExit.ItemColors.ForeColor = Color.Black;
			mnuExit.ItemColors.GradientStartColor = this.lightColor;
			mnuExit.ItemColors.GradientEndColor= this.darkColor;


			popMenu.MenuItems.Add(mnuExit);
			mnuExit.OwnerDraw =true;


		}

		/// <summary>
		/// Resizes the drawing
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(EventArgs e)
		{
			base.OnResize (e);
			rectangle = new Rectangle(0,0,Width,Height);
			SetBrush();
			Invalidate();
		}

		/// <summary>
		/// Sets the brushes in function of the chosen colors
		/// </summary>
		private void SetBrush()
		{
			//			Rectangle rec = rectangle;
			//			rec.Inflate(500,0);
			//frontBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(rectangle.Width-200,0,200,Height),darkColor,lightColor,gradientAngle);
			frontBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0,0,rectangle.Width,Height),darkColor,lightColor,gradientAngle);
			if(bellShaped)
				frontBrush.SetSigmaBellShape(bellCenter,bellFalloff);
			textBrush = new SolidBrush(this.ForeColor);
			shadowPen = new Pen(new SolidBrush(shadowColor),3.5F);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.popMenu = new System.Windows.Forms.ContextMenu();
			// 
			// NTitleBar
			// 
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.NAFTitleBar_MouseUp);
			this.DoubleClick += new System.EventHandler(this.NAFTitleBar_DoubleClick);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.NAFTitleBar_MouseMove);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.NAFTitleBar_MouseDown);

		}
		

		/// <summary>
		/// Paints the control
		/// </summary>
		/// <param name="pe"></param>
		protected override void OnPaint(PaintEventArgs pe)
		{
			pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			//pe.Graphics.FillRectangle(new SolidBrush(darkColor),new Rectangle(0,0,rectangle.Width-199,Height));
			//pe.Graphics.FillRectangle(frontBrush,new Rectangle(rectangle.Width-200,0,200,Height));
			
			//the gradient
			pe.Graphics.FillRectangle(frontBrush,new Rectangle(0,0,rectangle.Width,Height-2));

			//the text
			if(Text.Trim()!=string.Empty)
				pe.Graphics.DrawString(this.Text,this.Font,textBrush,5,5);

			//the smooth rounding
			if(neon)
			{
				Point[] points = new Point[5]{
												 new Point(rectangle.X+rectangle.Width,rectangle.Y+rectangle.Height),
												 new Point(rectangle.X+rectangle.Width-50,rectangle.Y+2),
												 new Point(rectangle.X+rectangle.Width,rectangle.Y+2),
												 new Point(rectangle.X+rectangle.Width-50,rectangle.Y+rectangle.Height),
												 new Point(rectangle.X+rectangle.Width-100,rectangle.Y + rectangle.Height)
											 };
				GraphicsPath path = new GraphicsPath();
				path.AddBezier(points[2],points[1],points[3],points[4]);
				path.AddLine(points[2],points[0]);
				path.AddLine(points[0],points[4]);						
				Region region = new Region(path);
				pe.Graphics.FillRegion(new SolidBrush(this.BackColor),region);
        			
				//the shadow or smoothing
			
				if(showShadow)
				{
					points = new Point[4]{	 new Point(rectangle.X+rectangle.Width-50,rectangle.Y+2), //topleft
											 new Point(rectangle.X+rectangle.Width,rectangle.Y+2), //topright
											 new Point(rectangle.X+rectangle.Width-50,rectangle.Y+rectangle.Height-2), //bottomright
											 new Point(rectangle.X+rectangle.Width-100,rectangle.Y + rectangle.Height-2) //bottomleft
										 };
				
					pe.Graphics.DrawBezier(shadowPen,points[1],points[0],points[2],points[3]); 
					pe.Graphics.DrawLine(shadowPen,0,rectangle.Height-2,rectangle.Width-100,rectangle.Height-2);
				}
				else
				{
					points = new Point[4]{	 new Point(rectangle.X+rectangle.Width-50,rectangle.Y+2), //topleft
											 new Point(rectangle.X+rectangle.Width,rectangle.Y+2), //topright
											 new Point(rectangle.X+rectangle.Width-50,rectangle.Y+rectangle.Height-2), //bottomright
											 new Point(rectangle.X+rectangle.Width-100,rectangle.Y + rectangle.Height) //bottomleft
										 };
					pe.Graphics.DrawBezier(new Pen(BackColor,2),points[1],points[0],points[2],points[3]); 
				}
			}

		}
		

		/// <summary>
		/// Handles the mouse down event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void NAFTitleBar_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
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
				p = new Point(e.X,e.Y);				
				if (enableMenu) popMenu.Show(this,p);
				//ControlPaint.DrawReversibleFrame(new Rectangle(p,new Size(120,200)),Color.Gray,FrameStyle.Thick);
				//				ControlPaint.FillReversibleRectangle(new Rectangle(p,new Size(120,200)),Color.Gray);
				//				this.Invalidate();

			}
		}


		/// <summary>
		/// Handles the mouse up event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void NAFTitleBar_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(e.Button==MouseButtons.Left)
			{
				tracking = false;
				Capture = false;
			}
		
		}


		/// <summary>
		/// Handles the mouse move event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void NAFTitleBar_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			
			if(tracking)
			{
				(Parent as Form).Left+= e.X-x;
				(Parent as Form).Top+= e.Y-y;				
			}
		}

		
		/// <summary>
		/// Handles the double-click event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void NAFTitleBar_DoubleClick(object sender, System.EventArgs e)
		{
			if(this.Parent==null) return;

			try
			{
				Form frm = this.Parent as Form;
				
				if(frm.WindowState==FormWindowState.Maximized)
					frm.WindowState=FormWindowState.Normal;
				else if(frm.WindowState==FormWindowState.Normal)
					frm.WindowState=FormWindowState.Maximized;
				
				
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.Message);
			}
		}
		#endregion


	}
}