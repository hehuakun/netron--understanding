using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Netron.Neon
{
	/// <summary>
	/// Neon's simple charting control
	/// </summary>
	public class NChartingControl : ScrollableControl
	{
		#region Fields
		private System.ComponentModel.IContainer components;

		/// <summary>
		/// the props bag for the propsgrid
		/// </summary>
		PropertyBag bag;

		/// <summary>
		/// used by the data request event
		/// </summary>
		public delegate void ChartInfo(NChartingControl charter);
		/// <summary>
		/// gives a signal that new data is requested before drawing
		/// Goes hand in hand with the Data property
		/// </summary>
		public event ChartInfo OnDataRequest;

			public event PropsInfo OnShowProperties;

		/// <summary>
		/// the data drawn
		/// </summary>
		private DataList list;	
		/// <summary>
		/// the lines collection
		/// </summary>
		protected ChartLines lines;
		/// <summary>
		/// capacity of the history
		/// </summary>
		protected const int capacity = 1000;
		/// <summary>
		/// the global scaling factor
		/// </summary>
		protected float scalingFactor = 1f;
		/// <summary>
		/// the last data set
		/// </summary>
		protected float[] data;
		/// <summary>
		/// whether to show v-lines
		/// </summary>
		protected bool mVerticalLines = false;
		/// <summary>
		/// whether to show h-lines
		/// </summary>
		protected bool mHorizontalLines = false;

		/// <summary>
		/// the h-spacing
		/// </summary>
		protected int mHorizontalSpacing = 20;
		/// <summary>
		/// the v-spacing
		/// </summary>
		protected int mVerticalSpacing = 20;
		/// <summary>
		/// the timer controlling the pulse of the drawing
		/// </summary>
		private System.Windows.Forms.Timer timer;
		/// <summary>
		/// the pen for drawing the grid
		/// </summary>
		protected Pen mGridPen;
		/// <summary>
		/// the grid's color
		/// </summary>
		protected Color gridColor = Color.WhiteSmoke;
		/// <summary>
		/// the number of points to be plotted
		/// </summary>
		protected int plotPoints = 100;
		/// <summary>
		/// the pen used to plot the lines, is set in the OnPaint by the Lines collection
		/// </summary>
		protected Pen plotPen;
		/// <summary>
		/// whether to show the legend of the lines
		/// </summary>
		protected bool showLegend = false;

		private MenuItem mnuProperties;
		#endregion

		#region Properties
		[Browsable(true), Description("Whether to show the lines legend."), Category("Chart")]
		public bool ShowLegend
		{
			get{return showLegend;}
			set{showLegend = value; Invalidate();}
		}

		[Browsable(true), Description("The collection of chart lines"), Category("Chart")]
		public ChartLines Lines
		{
			get{return lines;}
			set{lines = value; Invalidate();}
		}

		[Browsable(false)]
		public float[] Data
		{
			set
			{
				if(value==null) return;
				//shift the data
				int maxindex = Math.Max(Math.Min(capacity,list.Count)-1,0);
				if(list.Count>=capacity)
				{
					for (int i = 1; i <=maxindex; i++)
						list[i - 1] = list[i];

					if(list.Count>0)
						list.RemoveAt(maxindex);		
				}
				//add the new incoming value
				if(value.Length>0)
				{
					list.Add(value);
					data = value;				
				}

				Invalidate();
			}
			
			
		}

		[Browsable(true), Description("The color of the grid."), Category("Chart")]
		public Color GridColor
		{
			get{return gridColor;}
			set{gridColor = value;
				mGridPen = new Pen(gridColor, 1f);
				Invalidate();}
		}

		[Browsable(true), Description("The number of points being plotted, i.e. the range taken from the last input towards the begining"), Category("Chart")]
		public int PlotPoints
		{
			get{return plotPoints;}
			set{if(value<=capacity) plotPoints = value;Invalidate();}
		}

		[Browsable(true), Description("Whether the vertical grid lines are shown"), Category("Chart")]
		public bool VerticalLines
		{
			get{return mVerticalLines;}
			set{mVerticalLines = value; Invalidate();}
		}
		[Browsable(true), Description("Whether the horizontal grid lines are shown"), Category("Chart")]
		public bool HorizontalLines
		{
			get{return mHorizontalLines;}
			set{mHorizontalLines = value; Invalidate();}
		}

		[Browsable(true), Description("The horizontal grid spacing"), Category("Chart")]
		public int HorizontalSpacing
		{
			get{return mHorizontalSpacing;}
			set
			{
				if(mHorizontalSpacing <=0)
					throw new Exception("The spacing needs to be positive.");
				mHorizontalSpacing = value; Invalidate();}
		}
				[Browsable(true), Description("The vertical grid spacing"), Category("Chart")]
		public int VerticalSpacing
		{
			get{return mVerticalSpacing;}
			set
			{
				if(mVerticalSpacing <=0)
					throw new Exception("The spacing needs to be positive.");
				mVerticalSpacing = value; Invalidate();}
		}
	

		[Browsable(true), Description("The global vertical scaling factor"), Category("Chart")]
		public float ScalingFactor
		{
			get{return scalingFactor;}
			set{scalingFactor = value; Invalidate();}
		}
		#endregion
		
		#region Constructor
		public NChartingControl()
		{
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);

			list = new DataList(capacity);


		
			InitializeComponent();

			mGridPen = new Pen(gridColor,1F);

			lines = new ChartLines();

			this.mnuProperties = new System.Windows.Forms.MenuItem();

			// 
			this.ContextMenu = new ContextMenu();
				
				this.ContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.mnuProperties});
			// 
			// mnuProperties
			// 
			this.mnuProperties.Index = 0;
			this.mnuProperties.Text = "Properties";
			this.mnuProperties.Click += new System.EventHandler(this.mnuProperties_Click);

			//
			//Property bag stuff
			//
			bag = new PropertyBag();

			bag.Properties.Add(new PropertySpec("Lines",typeof(ChartLines),"Graph","The collection of chart lines"));
			bag.Properties.Add(new PropertySpec("HorizontalLines",typeof(bool),"Graph","Whether the horizontal lines are shown"));
			bag.Properties.Add(new PropertySpec("VerticalLines",typeof(bool),"Graph","Whether the vertical lines are shown"));
			bag.Properties.Add(new PropertySpec("PlotPoints",typeof(int),"Graph","The number of points being plotted, i.e. the range taken from the last input towards the begining"));
			bag.Properties.Add(new PropertySpec("ScalingFactor",typeof(float),"Graph","The global vertical scaling factor of all the lines"));
			bag.Properties.Add(new PropertySpec("ShowLegend",typeof(bool),"Graph","Whether the chart legend is shown"));
			bag.Properties.Add(new PropertySpec("HorizontalSpacing",typeof(int),"Graph","The spacing between the vertical lines"));
			bag.Properties.Add(new PropertySpec("VerticalSpacing",typeof(int),"Graph","The spacing between the horizontal lines"));

			
			

			bag.GetValue+=new PropertySpecEventHandler(bag_GetValue);
			bag.SetValue+=new PropertySpecEventHandler(bag_SetValue);

		}
		#endregion

		#region Methods
		private void bag_GetValue(object sender, PropertySpecEventArgs e)
		{
			switch(e.Property.Name)
			{
				case "Lines":
					e.Value = this.lines; break;
				case "HorizontalLines":
					e.Value = this.mHorizontalLines; break;
				case "VerticalLines":
					e.Value = this.mVerticalLines; break;
				case "PlotPoints":
					e.Value = this.plotPoints; break;
				case "ScalingFactor":
					e.Value = this.scalingFactor; break;
				case "ShowLegend":
					e.Value = this.showLegend; break;
				case "HorizontalSpacing":
					e.Value = this.mHorizontalSpacing; break;
				case "VerticalSpacing":
					e.Value = this.mVerticalSpacing; break;
			}
		}

		private void bag_SetValue(object sender, PropertySpecEventArgs e)
		{
			switch(e.Property.Name)
			{
				case "Lines":
					this.Lines = e.Value as ChartLines;	break;
				case "HorizontalLines":
					this.HorizontalLines = (bool) e.Value; break;
				case "VerticalLines":
					this.VerticalLines = (bool) e.Value; break;
				case "PlotPoints":
					this.PlotPoints = (int) e.Value; break;
				case "ScalingFactor":
					this.ScalingFactor = (float) e.Value; break;
				case "ShowLegend":
					this.ShowLegend = (bool) e.Value; break;
				case "HorizontalSpacing":
					this.HorizontalSpacing = (int) e.Value; break;
				case "VerticalSpacing":
					this.VerticalSpacing = (int) e.Value; break;
			}
		}
		private void mnuProperties_Click(object sender, System.EventArgs e)
		{
			if(OnShowProperties!=null)
				OnShowProperties(bag);
		}

		private void timer_Tick(object sender, System.EventArgs e)
		{
			if(OnDataRequest!=null)
				OnDataRequest(this);
		}

		public void Start()
		{
			this.Invalidate();
			if(this.lines.Count>0)	timer.Start();
		}

		public void Start(int interval)
		{
			timer.Interval = interval;
			if(this.lines.Count>0)	timer.Start();
		}

		public void Stop()
		{
			this.Invalidate();
			timer.Stop();
		}

		/// <summary>
		/// Resets the data
		/// </summary>
		public void Clear()
		{
			this.data = null;
			this.list = new DataList(capacity);
			this.Invalidate();
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


		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown (e);
			if(e.Button==MouseButtons.Left && e.Clicks==2)
			{
				if(timer.Enabled)
					this.Stop();
				else
					this.Start();

			}
		}


		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.timer = new System.Windows.Forms.Timer(this.components);
			// 
			// timer
			// 
			this.timer.Tick += new System.EventHandler(this.timer_Tick);
			// 
			// NChartingControl
			// 
			this.Name = "NChartingControl";
			this.Size = new System.Drawing.Size(376, 360);

		}
		#endregion

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			int delta = Convert.ToInt32(((float) ClientRectangle.Width)/((float) plotPoints));
			int x;
			float scale;
			if(list.Count>0)
			{
				
				x=0;
				for (int i = Math.Max(1,list.Count-plotPoints); i < list.Count; i++)
				{
					for(int m=0; m<lines.Count; m++)
					{
						scale = lines[m].Scale;
						if(m<list[i].Length && m<list[i-1].Length)
						{
							plotPen = new Pen(lines[m].LineColor,lines[m].LineWidth);
							e.Graphics.DrawLine(plotPen, x*delta,  (ClientRectangle.Height / 2) - (list[i - 1][m] * scalingFactor*scale)+12,
								(x+1)*delta,    (ClientRectangle.Height / 2) - (list[i][m] *scalingFactor*scale)+12);
						}
					}
					x++;
				}
			}
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);

			Graphics g = e.Graphics;
			//g.FillRectangle(Brushes.Wheat,this.ClientRectangle);
			if(mVerticalLines)
			{
				for(int k= 1; k<Math.Floor((double)this.Width/mHorizontalSpacing);k++)
				{
					g.DrawLine(mGridPen,new Point(k*mHorizontalSpacing,0), new Point(k*mHorizontalSpacing,this.Height));
				}
			}
			if(mHorizontalLines)
			{
				for(int k= 1; k<Math.Floor((double)this.Height/mVerticalSpacing);k++)
				{
					g.DrawLine(mGridPen,new Point(0,k*mVerticalSpacing), new Point(this.Width,k*mVerticalSpacing));
				}
			}

			if(showLegend)
			{
				StringFormat sf = new StringFormat();
				sf.Alignment = StringAlignment.Far;
				for(int k = 0; k<lines.Count;k++)
				{
					g.FillRectangle(new SolidBrush(lines[k].LineColor),ClientRectangle.Right-30,ClientRectangle.Bottom-k*20 -20,20,10);
					g.DrawString(lines[k].Name,this.Font, Brushes.Black,ClientRectangle.Right-40, ClientRectangle.Bottom-k*20 -20,sf);
				}
			}
		}

		#endregion

		
	}
}
