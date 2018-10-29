using System;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
namespace Netron.Neon
{
	/// <summary>
	/// Owner drawn tabcontrol with dragdrop functionality
	/// See below for a necessary fix in the hosting form
	/// </summary>
	[ToolboxItem(true)]
	public class NTabControl : TabControl
	{

		#region Events
		[Description("Occurs as a tab is being changed.")]
		public event SelectedTabPageChangeEventHandler SelectedIndexChanging;
		#endregion

		#region Fields
		//Variable to store the tabpage which belongs to the headeritem
		//over which the cursor is currently hovering.
		private TabPage CurrentTabItem;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private System.Windows.Forms.ContextMenu contextMenu;

		bool highlight = false;

		Size itemSize = Size.Empty;		

		#region Colors
		/// <summary>
		/// the darker color of the gradient of the selected tab
		/// </summary>
		private Color  darkColor = Color.SlateGray;
		/// <summary>
		/// the lighter color of the gradient of the selected tab
		/// </summary>
		private Color lightColor = Color.Silver;
		/// <summary>
		/// the color of the unselected tab
		/// </summary>
		private Color unselectedTabColor = Color.WhiteSmoke;
		/// <summary>
		/// the color of the edge of the unselected tab
		/// </summary>
		private Color unselectedEdgeColor = Color.Silver;
		/// <summary>
		/// the color of the text of the unselected tab
		/// </summary>
		private Color unselectedForeColor = Color.Silver;
		
		
		
		#endregion

		#region Brushes
		/// <summary>
		/// the brush of the selected tab
		/// </summary>
		private LinearGradientBrush tabBrush;
		/// <summary>
		/// the brush of the unselected tab
		/// </summary>
		private Brush unselectedForeBrush;
		/// <summary>
		/// the brush to draw the rectangle around the selected tab page
		/// </summary>
		private Brush darkBrush;
		/// <summary>
		/// the brush for the unselected tab
		/// </summary>
		private Brush unselectedTabBrush;
		
		#endregion

		#region Pens
		/// <summary>
		/// the pen to draw the rectangle around the unselected tabs
		/// </summary>
		private Pen unselectedEdgePen;
		#endregion
		/// <summary>
		/// the center of the bell, between 0 and 1
		/// </summary>
		private float bellCenter = 0.5f;
		/// <summary>
		/// the falloff of the bell, between 0 and 1
		/// </summary>
		private float bellFalloff = 1.0f;		
		private Pen roundingPen;
		private Brush foreBrush;
		private GraphicsPath path;
		private Region region;
		private readonly int bshift =16;
		/// <summary>
		/// the angle of the gradient
		/// </summary>
		private float gradientAngle = 0F;
		private Point[] points;
		/// <summary>
		/// whether the gradient is bell-shaped
		/// </summary>
		private bool bellShaped = true;
		#endregion

		#region Properties
		[Browsable(true)] public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
				SetBrush();
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

		public bool Highlight
		{
			get{return highlight;}
			set{highlight = value; Invalidate();}
		}


		public Color DarkColor
		{
			get{return darkColor;}
			set{
				darkColor = value;
				SetBrush();
			}
		}

		public Color UnselectedTabColor
		{
			get{return unselectedTabColor;}
			set
			{
				unselectedTabColor = value;
				SetBrush();
			}
		}

		public Color UnselectedForeColor
		{
			get{return unselectedForeColor;}
			set
			{
				unselectedForeColor = value;
				SetBrush();
			}
		}
		public Color UnselectedEdgeColor
		{
			get{return unselectedEdgeColor;}
			set
			{
				unselectedEdgeColor = value;
				SetBrush();
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
		public Color LightColor
		{
			set{lightColor = value; SetBrush();}
			get{return lightColor;}
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

	
		#endregion

		#region Constructor

		public NTabControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

				
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
			NMenuItem mnuSwitch = new NMenuItem("About");
			mnuSwitch.OwnerDraw = true;
			mnuSwitch.Click+=new EventHandler(mnuSwitch_Click);
			this.contextMenu.MenuItems.Add(mnuSwitch);

			AllowDrop = true;
			this.Appearance = TabAppearance.Normal;

			

			//init the brushes for painting
			SetBrush();
			

		}

		
		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.contextMenu = new System.Windows.Forms.ContextMenu();

		}
		#endregion

		#region Interop

		[StructLayout(LayoutKind.Sequential)]
		private struct NMHDR
		{
			public IntPtr HWND;
			public uint idFrom;
			public int code;
			public override String ToString()
			{
				return String.Format("Hwnd: {0}, ControlID: {1}, Code: {2}", HWND, idFrom, code);
			}
		}
        
		private const int TCN_FIRST = 0 - 550;                 
		private const int TCN_SELCHANGING = (TCN_FIRST - 2);
        
		private const int WM_USER = 0x400;
		private const int WM_NOTIFY = 0x4E;
		private const int WM_REFLECT = WM_USER + 0x1C00;
        
		#endregion

		#region BackColor Manipulation

		//As well as exposing the property to the Designer we want it to behave just like any other 
		//controls BackColor property so we need some clever manipulation.

		private Color m_Backcolor = Color.Empty;
		[Browsable(true),Description("The background color used to display text and graphics in a control.")]
		public override Color BackColor
		{
			get
			{
				if (m_Backcolor.Equals(Color.Empty))
				{
					if (Parent == null)
						return Control.DefaultBackColor;
					else
						return Parent.BackColor;
				}
				return m_Backcolor;
			}
			set
			{
				if (m_Backcolor.Equals(value)) return;
				m_Backcolor = value;
				roundingPen = new Pen(m_Backcolor,5);
				Invalidate();
				//Let the Tabpages know that the backcolor has changed.
				base.OnBackColorChanged(EventArgs.Empty);
			}
		}
		public bool ShouldSerializeBackColor()
		{
			return !m_Backcolor.Equals(Color.Empty);
		}
		public override void ResetBackColor()
		{
			m_Backcolor = Color.Empty;
			Invalidate();
		}

		#endregion

		#region Methods

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove (e);			

			int HoverTab = TestTabIndex(new Point(e.X, e.Y));
			if (HoverTab >= 0)
				CurrentTabItem = TabPages[HoverTab];          
			if (this.ContextMenu == null)
				ContextMenu = contextMenu;
		
		}
		
		protected override void OnMouseDown(MouseEventArgs e)
		{
			
			
			base.OnMouseDown (e);
			
			

			if(e.Button==MouseButtons.Left && Control.ModifierKeys==Keys.Control )
			{
				//dragging = true;
				Point pt = new Point(e.X, e.Y); 
				TabPage tp = GetTabPageByTab(pt) as TabPage; 
				
				ATP atp=new ATP();
				atp.Element = tp;
				atp.Origin = this;
				if(tp != null) 
				{ 
					
					DragDropEffects effect= DoDragDrop(atp , DragDropEffects.All); 
					
					
				} 
			}

		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp (e);
			
		
		}

	

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave (e);
			ContextMenu = null;
		}

	
        
			


		private int TestTabIndex(Point pt)
		{
			int returnIndex = -1;
			for (int index = 0; index <= TabCount - 1; index++)
			{
				if (GetTabRect(index).Contains(pt.X, pt.Y))
					returnIndex = index;
			} 
			return returnIndex;
		}


		protected override void OnCreateControl()
		{
			base.OnCreateControl ();
			
			
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}


		protected override void OnParentBackColorChanged(EventArgs e)
		{
			base.OnParentBackColorChanged (e);
			Invalidate();
		}

	
		protected override void OnSelectedIndexChanged(EventArgs e)
		{
			
				base.OnSelectedIndexChanged (e);
				Invalidate();
			
		}

		public void SetBrush()
		{

			
			itemSize = this.ItemSize;			
//			if(itemSize==Size.Empty)
//				itemSize=new Size(3,20);
			if(itemSize==Size.Empty) return;
			tabBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0,0,itemSize.Width,itemSize.Height),darkColor,lightColor,gradientAngle);
			if(bellShaped)
				tabBrush.SetSigmaBellShape(bellCenter,bellFalloff);

			darkBrush = new SolidBrush(darkColor);
			roundingPen = new Pen(darkColor,1);
			foreBrush = new SolidBrush(this.ForeColor);
			unselectedForeBrush = new SolidBrush(unselectedForeColor);
			unselectedEdgePen = new Pen(unselectedEdgeColor,1);			
			unselectedTabBrush = new SolidBrush(UnselectedTabColor);
			Invalidate();
		
			
		}
		

		

		protected override void OnLayout(LayoutEventArgs levent)
		{
			base.OnLayout (levent);
			SetBrush();
		}


		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint (e);
			Rectangle r;
			StringFormat sf ;//= new StringFormat();
			TabPage tp;// = TabPages[SelectedIndex];		
			int sindex = -1; //index of the selected page

			//make it nice
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			//clear the whole control and fill it with the backcolor
			e.Graphics.Clear(BackColor);

			//this is the rectangle around the whole tabcontrol
			//Rectangle r = ClientRectangle;
			//e.Graphics.DrawRectangle(Pens.Red,r);

			//nothing else to do if there are no tabs
			if (TabCount <= 0) return;

			//this is the rectangle around the tabcontent
			//r = SelectedTab.Bounds;
			
			//e.Graphics.DrawString("tadaa", this.Font,Brushes.Red,5,8);
			//highlight the whole control if there is a tabpage being dragged over
			if(highlight)
			{
				r = ClientRectangle;								
				e.Graphics.FillRectangle(Brushes.Orange, r);
			}
			#region Drawing the tabs	
			
			//Draw the Tabs except the selected
			for (int index = 0; index < TabCount; index++)
			{
				tp = TabPages[index];
				//if(tp.Text=="@hidden") continue;
				r = GetTabRect(index);		
				points = new Point[11]{
										  new Point(r.Left,r.Bottom), //0
										  new Point(r.Left, r.Top+bshift), //1
										  new Point(r.Left,r.Top), //2
										  new Point(r.Left+bshift,r.Top), //3
										  new Point(r.Right-bshift,r.Top), //4
										  new Point(r.Right,r.Top), //5
										  new Point(r.Right,r.Top+bshift), //6
										  new Point(r.Right,r.Bottom-bshift), //7
										  new Point(r.Right,r.Bottom), //8
										  new Point(r.Right+bshift,r.Bottom), //9
										  new Point(r.Right,r.Bottom) //10									  
									  };
				//r.Offset(0,20);
				if(index==this.SelectedIndex)
				{				
					sindex = index;
					
				
				}
				else
				{
					
					//e.Graphics.FillRectangle(unselectedTabBrush, r);
					
					
					//e.Graphics.DrawLine(unselectedEdgePen,points[8],points[0]);
					path = new GraphicsPath();
					path.AddLine(points[0],points[2]);
					path.AddLine(points[2],points[4]);
					path.AddBezier(points[4],points[5],points[5],points[6]);	
					path.AddLine(points[6],points[8]);
					path.AddLine(points[8],points[0]);
					region = new Region(path);
					e.Graphics.FillRegion(unselectedTabBrush,region);

					e.Graphics.DrawLine(unselectedEdgePen,points[0],points[2]);
					e.Graphics.DrawLine(unselectedEdgePen,points[2],points[4]);
					e.Graphics.DrawBezier(unselectedEdgePen,points[4],points[5],points[5],points[6]);	
					e.Graphics.DrawLine(unselectedEdgePen,points[6],points[8]);

					sf = new StringFormat();
					sf.Alignment = StringAlignment.Center;			
					e.Graphics.DrawString(tp.Text, Font, unselectedForeBrush, new PointF(r.X+r.Width/2,r.Y+r.Height/2-e.Graphics.MeasureString(tp.Text,Font).Height*0.57F),sf);

				}
			
				e.Graphics.ResetTransform();
			}
			#region Drawing the selected tab

			if(sindex<0) return;
			r = GetTabRect(sindex);	
			
			points = new Point[11]{
									  new Point(r.Left,r.Bottom), //0
									  new Point(r.Left, r.Top+bshift), //1
									  new Point(r.Left,r.Top), //2
									  new Point(r.Left+bshift,r.Top), //3
									  new Point(r.Right-bshift,r.Top), //4
									  new Point(r.Right,r.Top), //5
									  new Point(r.Right,r.Top+bshift), //6
									  new Point(r.Right,r.Bottom-bshift), //7
									  new Point(r.Right,r.Bottom), //8
									  new Point(r.Right+bshift,r.Bottom), //9
									  new Point(r.Right,r.Bottom) //10									  
								  };
//			points = new Point[11]{
//									  new Point(r.Left,r.Bottom), //0
//									  new Point(r.Left, r.Top+bshift), //1
//									  new Point(r.Left,r.Top), //2
//									  new Point(r.Left+bshift,r.Top), //3
//									  new Point(r.Right-bshift,r.Top-3), //4
//									  new Point(r.Right+3,r.Top-3), //5
//									  new Point(r.Right+3,r.Top+bshift), //6
//									  new Point(r.Right,r.Bottom-bshift), //7
//									  new Point(r.Right,r.Bottom), //8
//									  new Point(r.Right+bshift,r.Bottom), //9
//									  new Point(r.Right+133,r.Bottom) //10									  
//								  };
			//the selected one has to be drawn on top of the others
			if(sindex<0) return;
			tp = TabPages[sindex];
					
			//e.Graphics.FillRectangle(tabBrush,r);
			path = new GraphicsPath();
			path.AddLine(points[0],points[2]);
			path.AddLine(points[2],points[4]);
			path.AddBezier(points[4],points[5],points[5],points[6]);	
			path.AddLine(points[6],points[8]);
			path.AddLine(points[8],points[0]);
			region = new Region(path);

			if(itemSize==Size.Empty) SetBrush();

			e.Graphics.FillRegion(tabBrush,region);

			e.Graphics.DrawLine(roundingPen,points[0],points[2]);
			e.Graphics.DrawLine(roundingPen,points[2],points[4]);
			e.Graphics.DrawBezier(roundingPen,points[4],points[5],points[5],points[6]);	
			e.Graphics.DrawLine(roundingPen,points[6],points[8]);
			
			//e.Graphics.DrawBezier(roundingPen,points[4],points[5],points[5],points[6]);			
			
			if(tp.Text.Trim()!=string.Empty)
			{
				sf = new StringFormat();
				sf.Alignment = StringAlignment.Center;				
				e.Graphics.DrawString(tp.Text,this.Font,foreBrush,new PointF(r.X+r.Width/2,r.Y+r.Height/2-e.Graphics.MeasureString(tp.Text,Font).Height*0.47F),sf);
			}

			//Draw a border around TabPage
			r = SelectedTab.Bounds;
			//r = GetTabRect(sindex);	
			r.Inflate(3, 3);
			e.Graphics.FillRectangle(darkBrush,r);
			#endregion
			#endregion

		
		}


		
		

			
		private TabPage TestTab(Point pt)
		{
			for (int index = 0; index <= TabCount - 1; index++)
			{
				if (GetTabRect(index).Contains(pt.X, pt.Y))
					return TabPages[index];
			}
			return null;
		}
        

		#region DragDrop

		protected override void OnDragEnter(DragEventArgs e)
		{
			base.OnDragEnter (e);
			
			if(e.Data.GetDataPresent(typeof(ATP)))
			{
				if(((ATP) e.Data.GetData(typeof(ATP))).Origin!=this)
				{
					e.Effect = DragDropEffects.Move;
					highlight = true;
					this.Invalidate();
				}
			}
			

		}



		protected override void OnDragLeave(EventArgs e)
		{
			base.OnDragLeave (e);
			
			
			highlight = false;
			Refresh();
	
		}

		

		protected override void OnDragDrop(DragEventArgs e)
		{
			base.OnDragDrop (e);
			if(e.Data.GetDataPresent(typeof(ATP))) 
			{
				ATP tb =(ATP)  e.Data.GetData(typeof(ATP)) ;
				if(tb.Equals(null)) return;
				this.TabPages.Add(tb.Element);		
				this.SelectedTab = tb.Element;
	
				highlight = false;

				

//				if((tb.Origin as TabControl).TabCount==1)
//				{
//					//Trace.WriteLine((tb.Tag as TabControl).Name + " is empty now");
//					(tb.Origin as TabControl).Height = 30;
//				}
				this.Invalidate();
				Refresh();
			}
			
	
		}

		/// <summary> 
		/// Loops over all the TabPages to find the index of the given TabPage. 
		/// </summary> 
		/// <param name="page">The TabPage we want the index for.</param> 
		/// <returns>The index of the given TabPage(-1 if it isn't found.)</returns> 
		private int FindIndex(TabPage page) 
		{ 
			for(int i = 0; i < TabPages.Count; i++) 
			{ 
				if(TabPages[i] == page) 
					return i; 
			} 


			return -1; 
		} 
	
		private TabPage GetTabPageByTab(Point pt) 
		{ 
			TabPage tp = null; 


			for(int i = 0; i < TabPages.Count; i++) 
			{ 
				if(GetTabRect(i).Contains(pt)) 
				{ 
					tp = TabPages[i]; 
					break; 
				} 
			} 


			return tp; 
		} 





		protected override void OnDragOver(System.Windows.Forms.DragEventArgs e) 
		{ 
			base.OnDragOver(e); 


			Point pt = new Point(e.X, e.Y); 
			//We need client coordinates. 
			pt = PointToClient(pt); 


			//Get the tab we are hovering over. 
			TabPage hover_tab = GetTabPageByTab(pt); 


			//Make sure we are on a tab. 
			if(hover_tab != null) 
			{ 
				//Make sure there is a TabPage being dragged. 
				//if(e.Data.GetDataPresent(typeof(IDummy))) 
				if(e.Data.GetType().FullName=="lkjlkl")
				{ 
					e.Effect = DragDropEffects.Move; 
					TabPage drag_tab = (TabPage)e.Data.GetData(typeof(TabPage)); 


					int item_drag_index = FindIndex(drag_tab); 
					int drop_location_index = FindIndex(hover_tab); 


					//Don't do anything if we are hovering over ourself. 
					if(item_drag_index != drop_location_index) 
					{ 
						ArrayList pages = new ArrayList(); 


						//Put all tab pages into an array. 
						for(int i = 0; i < TabPages.Count; i++) 
						{ 
							//Except the one we are dragging. 
							if(i != item_drag_index) 
								pages.Add(TabPages[i]); 
						} 


						//Now put the one we are dragging it at the proper location. 
						pages.Insert(drop_location_index, drag_tab); 


						//Make them all go away for a nanosec. 
						TabPages.Clear(); 


						//Add them all back in. 
						TabPages.AddRange((TabPage[])pages.ToArray(typeof(TabPage))); 


						//Make sure the drag tab is selected. 
						SelectedTab = drag_tab; 
					} 
				} 
			} 
			else 
			{ 
				e.Effect = DragDropEffects.None; 
			} 
		} 



		#endregion


		#endregion

		private void mnuSwitch_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Neon version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
		}
	}

	
	#region Of interest
	//		protected override void WndProc(ref Message m)
	//		{
	//			base.WndProc (ref m);
	//			switch (m.Msg)
	//			{
	//				case (WM_REFLECT + WM_NOTIFY):
	//				{
	//					NMHDR hdr = (NMHDR)(Marshal.PtrToStructure(m.LParam, typeof(NMHDR)));
	//					if (hdr.code == TCN_SELCHANGING)
	//					{
	//						TabPage tp = TestTab(PointToClient(Cursor.Position));
	//						if (tp != null)
	//						{
	//							TabPageChangeEventArgs e = new TabPageChangeEventArgs(SelectedTab, tp);
	//							if(SelectedIndexChanging!=null)
	//								SelectedIndexChanging(this, e);
	//							if (e.Cancel || tp.Enabled == false)
	//							{
	//								m.Result = new IntPtr(1);
	//								return;
	//							}
	//						}
	//					}
	//				}
	//					break;
	//			
	//				case 0x0114: // WM_HSCROLL
	//				{
	//					Console.WriteLine(m.ToString());
	//					break;
	//				}
	//				
	//			}
	//
	//		}
	#endregion

	#region Host code
	

	/*
	
		//The overriden DragDrop and DragEnter methods are necessary since a tabcontrol without
		//any tabpages does not fire either of the events, this is by design but unfortunate.
		protected override void OnDragDrop(DragEventArgs e)
		{
			base.OnDragDrop (e);
			if(e.Data.GetDataPresent(typeof(ATP))) 
			{
				ATP tb =(ATP)  e.Data.GetData(typeof(ATP)) ;
				if(tb.Equals(null)) return;
				Point p = PointToClient(new  Point(e.X,e.Y));
				Control ctrl = GetChildAtPoint(p);
				if(ctrl==null) return;
				if(ctrl==this.nuiTabControl1)
				{
					if(this.nuiTabControl1.TabPages.Count==0)
					{
						this.nuiTabControl1.TabPages.Add(tb.Element);		
						this.nuiTabControl1.SelectedTab = tb.Element;
					}
				}

				if(ctrl==this.nuiTabControl2)
				{
					if(this.nuiTabControl2.TabPages.Count==0)
					{
						this.nuiTabControl2.TabPages.Add(tb.Element);		
						this.nuiTabControl2.SelectedTab = tb.Element;
					}
				}
			}
		}

		protected override void OnDragEnter(DragEventArgs e)
		{
			base.OnDragEnter (e);

			if(e.Data.GetDataPresent(typeof(ATP))) 
			{
				e.Effect = DragDropEffects.Move;
			}

		}  
	*/
	#endregion

}

