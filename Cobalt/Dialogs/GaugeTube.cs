using System;
using System.Drawing.Drawing2D;
using System.Drawing.Design;
using System.Drawing;
using System.Windows.Forms;
namespace Netron.Cobalt
{
	/// <summary>
	/// Summary description for GaugeTube.
	/// </summary>
	public class GaugeTube : Panel
	{

		private int mPercentage = 0;
		private RectangleF fillRectangle = RectangleF.Empty;
		private Color fillColor = Color.SteelBlue;
		private LinearGradientBrush fillBrush;
		private bool mShowValue = true;
		private StringFormat format;
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
				SetBrush();					
				Invalidate();
			}
		}

		public bool ShowValue
		{
			get{return mShowValue;}
			set{mShowValue = value;}
		}


		public void SetPercentage(int value)
		{
			if(value>0 && value <=100)
			{
				mPercentage = value;
				SetBrush();
				Invalidate();
			}
		}

		private void SetBrush()
		{
			if(this.ClientRectangle.Width!=0 && this.ClientRectangle.Height!=0 && mPercentage!=0)
			{
				fillRectangle = new RectangleF(0,0, this.ClientRectangle.Width*mPercentage/100, this.ClientRectangle.Height);
				fillBrush = new LinearGradientBrush(fillRectangle, this.BackColor, Color.WhiteSmoke, 90);
				fillBrush.SetSigmaBellShape(0.2F,0.6F);
			}
		}

		public GaugeTube()
		{
			SetBrush();			
			format = new StringFormat();
			format.Alignment = StringAlignment.Center;
		}		

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint (e);
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			Rectangle  r = this.ClientRectangle;
			GraphicsPath path = new GraphicsPath();			
			path.AddArc(r.X, r.Y, 20, 20, -180, 90);			
			path.AddLine(r.X + 10, r.Y, r.X + r.Width - 10, r.Y);			
			path.AddArc(r.X + r.Width - 20, r.Y, 20, 20, -90, 90);			
			path.AddLine(r.X + r.Width, r.Y + 10, r.X + r.Width, r.Y + r.Height - 10);			
			path.AddArc(r.X + r.Width - 20, r.Y + r.Height - 20, 20, 20, 0, 90);			
			path.AddLine(r.X + r.Width - 10, r.Y + r.Height, r.X + 10, r.Y + r.Height);			
			path.AddArc(r.X, r.Y + r.Height - 20, 20, 20, 90, 90);			
			path.AddLine(r.X, r.Y + r.Height - 10, r.X, r.Y + 10);					
			//shadow
			Region darkRegion = new Region(path);
			//darkRegion.Translate(5, 5);
			//e.Graphics.FillRegion(new SolidBrush(Color.FromArgb(20, Color.Black)), darkRegion);
			//e.Graphics.FillRectangle(Brushes.Transparent, this.ClientRectangle);
			//e.Graphics.FillPath(Brushes.WhiteSmoke, path);
			//e.Graphics.Clip = darkRegion;
			if(mPercentage>0)
			{
				r = new Rectangle(0,0, this.ClientRectangle.Width*mPercentage/100, this.ClientRectangle.Height);
				path = new GraphicsPath();			
				path.AddArc(r.X, r.Y, 20, 20, -180, 90);			
				path.AddLine(r.X + 10, r.Y, r.X + r.Width , r.Y);							
				path.AddLine(r.X + r.Width, r.Y , r.X + r.Width, r.Y + r.Height );											
				path.AddLine(r.X + r.Width, r.Y + r.Height , r.X + 10 , r.Y + r.Height);			
				path.AddArc(r.X, r.Y + r.Height - 20, 20, 20, 90, 90);			
				path.AddLine(r.X, r.Y + r.Height - 10, r.X, r.Y + 10);		

//				path.AddArc(r.X, r.Y, 20, 20, -180, 90);			
//				path.AddLine(r.X + 10, r.Y, r.X + r.Width - 10, r.Y);			
//				path.AddArc(r.X + r.Width - 20, r.Y, 20, 20, -90, 90);			
//				path.AddLine(r.X + r.Width, r.Y + 10, r.X + r.Width, r.Y + r.Height - 10);			
//				path.AddArc(r.X + r.Width - 20, r.Y + r.Height - 20, 20, 20, 0, 90);			
//				path.AddLine(r.X + r.Width - 10, r.Y + r.Height, r.X + 10, r.Y + r.Height);			
//				path.AddArc(r.X, r.Y + r.Height - 20, 20, 20, 90, 90);			
//				path.AddLine(r.X, r.Y + r.Height - 10, r.X, r.Y + 10);	

				e.Graphics.FillPath(fillBrush, path);
				if(mShowValue)
				{
					r.Offset(0,(int)r.Height/2-7);
					e.Graphics.DrawString(mPercentage + "%", this.Font, Brushes.Black,r, format);
				}
			}
			
		}

	}
}
