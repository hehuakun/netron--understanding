using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Reflection;
namespace Netron.Neon
{
	/// <summary>
	/// Splitter which on double-click will hide/show the selected (adjacent) panel.
	/// It uses the MinMax property of the splitter to flatten the adjacent panel.
	/// A double-arrow is drawn to emphasize the effect.
	/// TODO: menu or popup help
	/// </summary>
	public class NSplitter : Splitter
	{
		private Bitmap right, left, top, bottom;
		private Panel colaspingPanel;

		private bool collapsed = false;
		private int initialWidth;
		
		public Panel ColapsingPanel
		{
			get{return colaspingPanel;}
			set{
				if(value==null) return;
				colaspingPanel = value;
				initialWidth = colaspingPanel.Width;
			}
		}

		private Bitmap GetImage(string name)
		{
			Bitmap bmp=null;
			try{
			Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.Neon.Resources.MiniArrow" + name + ".gif");
					
				bmp= Bitmap.FromStream(stream) as Bitmap;
				stream.Close();
				stream=null;
			}
			catch(Exception exc)
			{
				//Trace.WriteLine(exc.Message);
			}
			return bmp;

			
		}


		public NSplitter()
		{
		
			right = GetImage("Right");
			left = GetImage("Left");
			top = GetImage("Top");
			bottom = GetImage("Bottom");
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.FillRectangle(new SolidBrush(BackColor),e.ClipRectangle);
			if( Dock == DockStyle.Bottom || Dock == DockStyle.Top)
			{
				e.Graphics.DrawImage(bottom,Width/2-9,1); //the image is 8x8 px			
				e.Graphics.DrawImage(top,Width/2+1,1);
			}
			else
			{
				e.Graphics.DrawImage(right,0,this.Height/2-9); //the image is 8x8 px			
				e.Graphics.DrawImage(left,0,this.Height/2+1);
			}

		}
		protected override void OnDoubleClick(EventArgs e)
		{
			base.OnDoubleClick (e);
			if(colaspingPanel==null) return;
			if(collapsed)
				colaspingPanel.Width=initialWidth;
			else
				colaspingPanel.Width = MinSize;
			collapsed = !collapsed;
//			this shifts the panel smoothly
//			while(colaspingPanel.Width>this.MinSize)
//			{
//				colaspingPanel.Width--;
//				this.Invalidate();
//				//(Parent as Form).Invalidate();
//			}

		}



	}
}
