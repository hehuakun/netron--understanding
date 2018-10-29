using System.Diagnostics;
using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace Netron.Neon
{
	// Custom control that draws the caption for each pane. Contains an active
	// state and draws the caption different for each state. Caption is drawn
	// with a gradient fill and antialias font.
	
	
	
	//'' <summary>
	//'' this class was copied from the FotoVision sample application on www.windowsforms.net
	//'' http://msdn.microsoft.com/smartclient/codesamples/FotoVision/default.aspx?pull=/library/en-us/dnnetcomp/html/fotovisiondesktop.asp
	//'' </summary>
	//'' <remarks>
	//'' </remarks>	
	public class XPCaption : System.Windows.Forms.UserControl
	{
		
		// const values
		private class Consts
		{
			public const int DefaultHeight = 20;
			public const string DefaultFontName = "arial";
			public const int DefaultFontSize = 9;
			public const int PosOffset = 4;
		}
		
		// internal members
		private bool _active = false;
		private bool _antiAlias = true;
		private bool _allowActive = true;
		
		private Color _colorActiveText = Color.Black;
		private Color _colorInactiveText = Color.White;
		
		private Color _colorActiveLow = Color.FromArgb(255, 165, 78);
		private Color _colorActiveHigh = Color.FromArgb(255, 225, 155);
		private Color _colorInactiveLow = Color.FromArgb(3, 55, 145);
		private Color _colorInactiveHigh = Color.FromArgb(90, 135, 215);
		
		// gdi objects
		private SolidBrush _brushActiveText;
		private SolidBrush _brushInactiveText;
		private LinearGradientBrush _brushActive;
		private LinearGradientBrush _brushInactive;
		private StringFormat _format;
		
		
		// public properties
		
		// the caption of the control
		[Category("Appearance"), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Description("Text that is displayed in the label.")]public new string Text
		{
			get{
				return base.Text;
			}
			set
			{
				base.Text = value;
				Invalidate();
			}
		}
		
		// if the caption is active or not
		[Description("The active state of the caption, draws the caption with different gradient colors."), Category("Appearance"), DefaultValue(false)]public bool Active
		{
			get{
				return _active;
			}
			set
			{
				_active = value;
				Invalidate();
			}
		}
		
		// if should maintain an active and inactive state
		[Description("True always uses the inactive state colors, false maintains an active and inactive state."), Category("Appearance"), DefaultValue(true)]public bool AllowActive
		{
			get{
				return _allowActive;
			}
			set
			{
				_allowActive = value;
				Invalidate();
			}
		}
		
		// if the caption is active or not
		[Description("If should draw the text as antialiased."), Category("Appearance"), DefaultValue(true)]public bool AntiAlias
		{
			get{
				return _antiAlias;
			}
			set
			{
				_antiAlias = value;
				Invalidate();
			}
		}
		
		#region " color properties "
		
		[Description("Color of the text when active."), Category("Appearance"), DefaultValue(typeof(Color), "Black")]public Color ActiveTextColor
		{
			get{
				return _colorActiveText;
			}
			set
			{
				if (value.Equals(Color.Empty))
				{
					value = Color.Black;
				}
				_colorActiveText = value;
				_brushActiveText = new SolidBrush(_colorActiveText);
				Invalidate();
			}
		}
		
		[Description("Color of the text when inactive."), Category("Appearance"), DefaultValue(typeof(Color), "White")]public Color InactiveTextColor
		{
			get{
				return _colorInactiveText;
			}
			set
			{
				if (value.Equals(Color.Empty))
				{
					value = Color.White;
				}
				_colorInactiveText = value;
				_brushInactiveText = new SolidBrush(_colorInactiveText);
				Invalidate();
			}
		}
		
		[Description("Low color of the active gradient."), Category("Appearance"), DefaultValue(typeof(Color), "255, 165, 78")]public Color ActiveGradientLowColor
		{
			get{
				return _colorActiveLow;
			}
			set
			{
				if (value.Equals(Color.Empty))
				{
					value = Color.FromArgb(255, 165, 78);
				}
				_colorActiveLow = value;
				CreateGradientBrushes();
				Invalidate();
			}
		}
		
		[Description("High color of the active gradient."), Category("Appearance"), DefaultValue(typeof(Color), "255, 225, 155")]public Color ActiveGradientHighColor
		{
			get{
				return _colorActiveHigh;
			}
			set
			{
				if (value.Equals(Color.Empty))
				{
					value = Color.FromArgb(255, 225, 155);
				}
				_colorActiveHigh = value;
				CreateGradientBrushes();
				Invalidate();
			}
		}
		
		[Description("Low color of the inactive gradient."), Category("Appearance"), DefaultValue(typeof(Color), "3, 55, 145")]public Color InactiveGradientLowColor
		{
			get{
				return _colorInactiveLow;
			}
			set
			{
				if (value.Equals(Color.Empty))
				{
					value = Color.FromArgb(3, 55, 145);
				}
				_colorInactiveLow = value;
				CreateGradientBrushes();
				Invalidate();
			}
		}
		
		[Description("High color of the inactive gradient."), Category("Appearance"), DefaultValue(typeof(Color), "90, 135, 215")]public Color InactiveGradientHighColor
		{
			get{
				return _colorInactiveHigh;
			}
			set
			{
				if (value.Equals(Color.Empty))
				{
					value = Color.FromArgb(90, 135, 215);
				}
				_colorInactiveHigh = value;
				CreateGradientBrushes();
				Invalidate();
			}
		}
		
		#endregion
		
		// internal properties
		
		// brush used to draw the caption
		private SolidBrush TextBrush
		{
			get{
				if(_active && _allowActive)
					return  _brushActiveText;
				else
					return  _brushInactiveText;
			}
		}
		
		// gradient brush for the background
		private LinearGradientBrush BackBrush
		{
			get{
				if(_active && _allowActive)
					return _brushActive;
				else 
					return _brushInactive;
			}
		}
		
		// ctor
		public XPCaption(){
			
			// this call is required by the Windows Form Designer
			InitializeComponent();
			
			// set double buffer styles
			this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
			
			// init the height
			this.Height = Consts.DefaultHeight;
			
			// format used when drawing the text
			_format = new StringFormat();
			_format.FormatFlags = StringFormatFlags.NoWrap;
			_format.LineAlignment = StringAlignment.Center;
			_format.Trimming = StringTrimming.EllipsisCharacter;
			
			// init the font
			this.Font = new Font(Consts.DefaultFontName, Consts.DefaultFontSize, FontStyle.Bold);
			
			// create gdi objects
			this.ActiveTextColor = _colorActiveText;
			this.InactiveTextColor = _colorInactiveText;
			
			// setting the height above actually does this, but leave
			// in incase change the code (and forget to init the
			// gradient brushes)
			CreateGradientBrushes();
		}
		
		// internal methods
		
		// the caption needs to be drawn
		protected override void OnPaint (PaintEventArgs e)
		{
			DrawCaption(e.Graphics);
			base.OnPaint(e);
		}
		
		// draw the caption
		private void DrawCaption (Graphics g)
		{
			// background
			g.FillRectangle(this.BackBrush, this.DisplayRectangle);
			
			// caption
			if (_antiAlias)
			{
				g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
			}
			
			// need a rectangle when want to use ellipsis
			RectangleF bounds = CtrlHelper.CheckedRectangleF(Consts.PosOffset, 0, this.DisplayRectangle.Width - Consts.PosOffset, this.DisplayRectangle.Height);
			
			g.DrawString(this.Text, this.Font, this.TextBrush, bounds, _format);
		}
		
		// clicking on the caption does not give focus,
		// handle the mouse down event and set focus to self
		protected override void OnMouseDown (MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (this._allowActive)
			{
				this.Focus();
			}
		}
		
		protected override void OnSizeChanged (System.EventArgs e)
		{
			base.OnSizeChanged(e);
			
			// create the gradient brushes based on the new size
			CreateGradientBrushes();
		}
		
		private void CreateGradientBrushes ()
		{
			// can only create brushes when have a width and height
			if (this.Width > 0 && this.Height > 0)
			{
				if (!(_brushActive == null))
				{
					_brushActive.Dispose();
				}
				_brushActive = new LinearGradientBrush(this.DisplayRectangle, _colorActiveHigh, _colorActiveLow, LinearGradientMode.Vertical);
				
				if (!(_brushInactive == null))
				{
					_brushInactive.Dispose();
				}
				_brushInactive = new LinearGradientBrush(this.DisplayRectangle, _colorInactiveHigh, _colorInactiveLow, LinearGradientMode.Vertical);
			}
		}
		
		
		#region " Windows Form Designer generated code "
		
		//UserControl overrides dispose to clean up the component list.
		protected override void Dispose (bool disposing)
		{
			if (disposing)
			{
				if (!(components == null))
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		//Required by the Windows Form Designer
		private System.ComponentModel.Container components = null;
		
//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent ()
		{
			//
			//PaneCaption
			//
			this.Name = "PaneCaption";
			this.Size = new System.Drawing.Size(150, 30);
		}
		
		#endregion
		
	}
	
}
