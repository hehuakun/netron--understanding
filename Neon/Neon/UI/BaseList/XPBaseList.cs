using System.Diagnostics;
using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Drawing2D;
namespace Netron.Neon
{	
	[Designer(typeof(BaseListDesigner)), DesignTimeVisibleAttribute(true)]
	public class NeonBaseList : System.Windows.Forms.UserControl	
	{
		#region Fields

		private const int WM_VSCROLL = 0x0115;
		private const int WM_HSCROLL = 0x0114;
		private System.ComponentModel.Container components = null;

		Size _ctrlSize = new Size(200, 200);
		Paddings _ctrlPaddings = new Paddings(4);
		int _minCols = 1;
		Control _selectedCtrl = null;
		
		Color _borderColor = Color.Black;
		ButtonBorderStyle _borderStyle = ButtonBorderStyle.Solid;
		bool _noUpdate = false;
		#endregion

		#region Properties
		//'' -----------------------------------------------------------------------------
		//'' <summary>
		//'' defines the size of ALL child controls of this container
		//'' if the width is set to 0/-1 then the controls are sized to the width of the
		//'' list (only one column of controls)
		//'' </summary>
		//'' <value></value>
		//'' <remarks>
		//'' </remarks>
		//'' <history>
		//'' 	[Mike]	13.08.2004	Created
		//'' </history>
		//'' -----------------------------------------------------------------------------
		public Size ControlSize
		{
			get
			{
				return _ctrlSize;
			}
			set
			{
				this._ctrlSize = value;
				this.Redraw();
			}
		}
		
		//'' -----------------------------------------------------------------------------
		//'' <summary>
		//'' defines the paddings between the child controls
		//'' </summary>
		//'' <value></value>
		//'' <remarks>
		//'' </remarks>
		//'' <history>
		//'' 	[Mike]	13.08.2004	Created
		//'' </history>
		//'' -----------------------------------------------------------------------------
		public Paddings ControlPaddings
		{
			get
			{
				return _ctrlPaddings;
			}
			set
			{
				_ctrlPaddings = value;
				this.Redraw();
			}
		}
		
		//'' -----------------------------------------------------------------------------
		//'' <summary>
		//'' defines the borderstyle of this controls
		//'' </summary>
		//'' <value></value>
		//'' <remarks>
		//'' </remarks>
		//'' <history>
		//'' 	[Mike]	13.08.2004	Created
		//'' </history>
		//'' -----------------------------------------------------------------------------
		public ButtonBorderStyle BorderStyle
		{
			get
			{
				return _borderStyle;
			}
			set
			{
				_borderStyle = value;
				this.Invalidate();
			}
		}
		
		//'' -----------------------------------------------------------------------------
		//'' <summary>
		//'' defines the border color of this control
		//'' </summary>
		//'' <value></value>
		//'' <remarks>
		//'' </remarks>
		//'' <history>
		//'' 	[Mike]	13.08.2004	Created
		//'' </history>
		//'' -----------------------------------------------------------------------------
		public Color BorderColor
		{
			get
			{
				return _borderColor;
			}
			set
			{
				_borderColor = value;
				this.Invalidate();
			}
		}
		
		//'' -----------------------------------------------------------------------------
		//'' <summary>
		//'' returns the currently selected control
		//'' </summary>
		//'' <value></value>
		//'' <remarks>
		//'' </remarks>
		//'' <history>
		//'' 	[Mike]	22.08.2004	Created
		//'' </history>
		//'' -----------------------------------------------------------------------------
		public Control SelectedControl
		{
			get
			{
				return _selectedCtrl;
			}
		}
		
		#endregion
	
		#region Constructor
		public NeonBaseList()
		{
			
			// Dieser Aufruf ist fr den Windows Form-Designer erforderlich.
			InitializeComponent();
			
			// Initialisierungen nach dem Aufruf InitializeComponent() hinzufgen
			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			SetStyle(ControlStyles.ContainerControl, true);
		}
		#endregion

		#region Methods

		/// <summary>
		/// Overrides the method to invalidate the control when the user scrolls the diagram
		/// 
		/// </summary>
		/// <param name="m"></param>
		/// <remarks>Not possible with the available overridable methods of .Net as far as I know</remarks>
		protected override void WndProc(ref Message m)
		{
			if(m.Msg==WM_VSCROLL || m.Msg==WM_HSCROLL)
			{
				this.Invalidate();
			}
			base.WndProc (ref m);
		}

		//'' -----------------------------------------------------------------------------
		//'' <summary>
		//'' redraws the child controls
		//'' </summary>
		//'' <remarks>
		//'' </remarks>
		//'' <history>
		//'' 	[Mike]	13.08.2004	Created
		//'' </history>
		//'' -----------------------------------------------------------------------------
		public void Redraw ()
		{
			Point _loc = new Point(0, 0);
			int _col = 0;
			int _row = 0;
			
			this.SuspendLayout();
			_noUpdate = false;
			
			foreach (Control c in this.Controls)
			{
				if (_ctrlSize.Width <= 0)
				{
					c.Size = new Size(this.Width - _ctrlPaddings.Left - _ctrlPaddings.Right, _ctrlSize.Height);
					c.Location = new Point(_ctrlPaddings.Left, _loc.Y + _ctrlPaddings.Top);
					c.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
					_loc.Y = _loc.Y + _ctrlPaddings.Top + _ctrlSize.Height + _ctrlPaddings.Bottom;
					_row += 1;
				}
				else
				{
					if (_col < _minCols |(this.Width - _loc.X - _ctrlSize.Width - _ctrlPaddings.Left - _ctrlPaddings.Right) >= 0)
					{
						
					}
					else
					{
						_loc.X = 0;
						_loc.Y = _loc.Y + _ctrlPaddings.Top + _ctrlSize.Height + _ctrlPaddings.Bottom;
						
						_col = 0;
						_row += 1;
					}
					
					c.Location = new Point(_loc.X + _ctrlPaddings.Left, _loc.Y + _ctrlPaddings.Top);
					c.Size = _ctrlSize;
					c.Anchor = AnchorStyles.Left | AnchorStyles.Top;
					_loc.X = _loc.X + _ctrlPaddings.Left + _ctrlSize.Width + _ctrlPaddings.Right;
					_col += 1;
				}
				
			}
			this.ResumeLayout();
			this.Invalidate();
		}
		
		//'' -----------------------------------------------------------------------------
		//'' <summary>
		//'' redraws the child elements on resize of the container controls
		//'' </summary>
		//'' <param name="sender"></param>
		//'' <param name="e"></param>
		//'' <remarks>
		//'' </remarks>
		//'' <history>
		//'' 	[Mike]	13.08.2004	Created
		//'' </history>
		//'' -----------------------------------------------------------------------------
		private void XPBaseList_Resize (object sender, System.EventArgs e)
		{
			this.Redraw();
		}
		
		private void BaseList_ControlAdded (object sender, System.Windows.Forms.ControlEventArgs e)
		{
			e.Control.Enter += new EventHandler(handlesEnter);
			e.Control.Leave += new EventHandler(handlesLeave);
			e.Control.TabIndex = this.Controls.IndexOf(e.Control);
			if (! _noUpdate)
			{
				this.Redraw();
			}
		}
		
		private void BaseList_ControlRemoved (object sender, System.Windows.Forms.ControlEventArgs e)
		{
			e.Control.Enter -= new EventHandler(handlesEnter);
			e.Control.Leave -= new EventHandler( handlesLeave);
			if (! _noUpdate)
			{
				this.Redraw();
			}
		}
		
		private void handlesEnter (object sender, EventArgs e)
		{
			_selectedCtrl = ((Control)(sender));
			OnControlEnter(sender, e);
			OnSelectionChanged(new EventArgs());
			this.Invalidate();
		}
		
		private void handlesLeave (object sender, EventArgs e)
		{
			_selectedCtrl = null;
			OnControlLeave(sender, e);
			OnSelectionChanged(new EventArgs());
			this.Invalidate();
		}
		
		
		protected override void OnPaint (System.Windows.Forms.PaintEventArgs e)
		{
			base.OnPaint(e);
			
			if ( _selectedCtrl != null)
			{
				Rectangle _rect = CtrlHelper.CheckedRectangle(_selectedCtrl.Location.X - 2, _selectedCtrl.Location.Y - 2, _selectedCtrl.Width + 4, _selectedCtrl.Height + 4);
				
				ControlPaint.DrawBorder(e.Graphics, _rect, SystemColors.Highlight, ButtonBorderStyle.Solid);
			}
		}
		
		protected override void OnPaintBackground (System.Windows.Forms.PaintEventArgs pevent)
		{
			base.OnPaintBackground(pevent);
			
			if ( _borderStyle != ButtonBorderStyle.None)
			{
				ControlPaint.DrawBorder(pevent.Graphics, CtrlHelper.CheckedRectangle(0, 0, this.Width, this.Height), _borderColor, _borderStyle);
			}
		}
		
		protected override void OnClick (System.EventArgs e)
		{
			base.OnClick(e);
			
		}
		
		public void BeginUpdate ()
		{
			_noUpdate = true;
		}
		
		public void EndUpdate ()
		{
			_noUpdate = false;
			this.Redraw();
		}

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
		
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent ()
		{
			//
			//BaseList
			//
			this.AutoScroll = true;
			this.Resize += new EventHandler(this.XPBaseList_Resize);
			this.ControlAdded += new ControlEventHandler(this.BaseList_ControlAdded);
			this.ControlRemoved += new ControlEventHandler(this.BaseList_ControlRemoved);
			this.Name = "XPBaseList";
			this.Size = new System.Drawing.Size(224, 224);
			
		}
		
		#endregion
		
		#region "Event Declarations"
		public delegate void SelectionChangedHandler(object sender, EventArgs e);		
		public event SelectionChangedHandler SelectionChanged;
		
		protected virtual void OnSelectionChanged (EventArgs e)
		{
			if(SelectionChanged!=null)
				SelectionChanged(this, e);
		}
		
		public delegate void ControlEnterHandler(object sender, EventArgs e);
		
		public event ControlEnterHandler ControlEnter;
		
		protected virtual void OnControlEnter (object sender, EventArgs e)
		{
			if(ControlEnter!=null)
				ControlEnter(sender, e);
		}
		
		public delegate void ControlLeaveHandler(object sender, EventArgs e);		
		public event ControlLeaveHandler ControlLeave;
		
		protected virtual void OnControlLeave (object sender, EventArgs e)
		{
			if(ControlLeave!=null)
				ControlLeave(sender, e);
		}
		#endregion
		

		
	}
	
	
	
	public class Paddings
	{
		private int _top;
		private int _bottom;
		private int _left;
		private int _right;
		
		public int Top
		{
			get{
				return _top;
			}
			set
			{
				_top = value;
			}
		}
		
		public int Bottom
		{
			get{
				return _bottom;
			}
			set
			{
				_bottom = value;
			}
		}
		
		public int Left
		{
			get{
				return _left;
			}
			set
			{
				_left = value;
			}
		}
		
		public int Right
		{
			get{
				return _right;
			}
			set
			{
				_right = value;
			}
		}
		
		public int All
		{
			set
			{
				_top = value;
				_bottom = value;
				_left = value;
				_right = value;
			}
		}
		
		public Paddings(){
			this.All = 0;
		}
		
		public Paddings(int all){
			this.All = all;
		}
		
		public Paddings(int top, int left, int bottom, int right){
			this._top = top;
			this._bottom = bottom;
			this._left = left;
			this._right = right;
		}
	}
	
	
}
