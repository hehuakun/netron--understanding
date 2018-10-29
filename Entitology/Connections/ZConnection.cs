using System;
using System.Drawing;
namespace Netron.GraphLib.Entitology
{
	/// <summary>
	/// A gradient connection emphasizing the z-order difference between two shapes
	/// </summary>
	[Netron.GraphLib.Attributes.NetronGraphConnection("Z Connection","58BFDE43-0481-4c9c-85D8-C070A9658843", "Netron.GraphLib.Entitology.ZConnection")]
	public class ZConnection : ConnectionPainter
	{
		

		#region Constructor
		public ZConnection(Connection connection) : base(connection){}
		#endregion

		#region Methods

		public override bool Hit(System.Drawing.PointF p)
		{
			bool join = false;
			//			points = new PointF[2+insertionPoints.Count];
			//			points[0] = p1;
			//			points[2+insertionPoints.Count-1] = p2;
			//			for(int m=0; m<insertionPoints.Count; m++)
			//			{
			//				points[1+m] = (PointF)  insertionPoints[m];
			//			}
			PointF[] points = Points;

			PointF p1 = this.Connection.From.AdjacentPoint;
			PointF p2 = this.Connection.To.AdjacentPoint; 

			PointF s;
			float o, u;
			RectangleF r1=RectangleF.Empty, r2=RectangleF.Empty, r3=RectangleF.Empty;

			for(int v = 0; v<points.Length-1; v++)
			{
						
				//this is the usual segment test
				//you can do this because the PointF object is a value type!
				p1 = points[v]; p2 = points[v+1];
	
				// p1 must be the leftmost point.
				if (p1.X > p2.X) { s = p2; p2 = p1; p1 = s; }

				r1 = new RectangleF(p1.X, p1.Y, 0, 0);
				r2 = new RectangleF(p2.X, p2.Y, 0, 0);
				r1.Inflate(3, 3);
				r2.Inflate(3, 3);
				//this is like a topological neighborhood
				//the connection is shifted left and right
				//and the point under consideration has to be in between.						
				if (RectangleF.Union(r1, r2).Contains(p))
				{
					if (p1.Y < p2.Y) //SWNE
					{
						o = r1.Left + (((r2.Left - r1.Left) * (p.Y - r1.Bottom)) / (r2.Bottom - r1.Bottom));
						u = r1.Right + (((r2.Right - r1.Right) * (p.Y - r1.Top)) / (r2.Top - r1.Top));
						join |= ((p.X > o) && (p.X < u));
					}
					else //NWSE
					{
						o = r1.Left + (((r2.Left - r1.Left) * (p.Y - r1.Top)) / (r2.Top - r1.Top));
						u = r1.Right + (((r2.Right - r1.Right) * (p.Y - r1.Bottom)) / (r2.Bottom - r1.Bottom));
						join |= ((p.X > o) && (p.X < u));
					}
				}


			}
			return join;
		}

	
		public override  void Paint(System.Drawing.Graphics g)
		{


			for(int k=0; k<Points.Length-1; k++)
			{
				if(Points[k]==Points[k+1]) continue;
				PointF s = Points[k] ,e = Points[k+1];
				double  alpha =0;
				double len=Math.Sqrt((e.X-s.X)*(e.X-s.X)+(e.Y-s.Y)*(e.Y-s.Y));						
				if(len>0 && this.Connection.To!=null && this.Connection.To.ConnectorLocation==ConnectorLocation.Omni)
				{					
					e.X-=Convert.ToSingle((e.X-s.X)*this.Connection.To.BelongsTo.Rectangle.Width /(2*len));
					e.Y-=Convert.ToSingle((e.Y-s.Y)*this.Connection.To.BelongsTo.Rectangle.Width /(2*len));
				}
				if(len>0 && this.Connection.From!=null && this.Connection.From.ConnectorLocation==ConnectorLocation.Omni)
				{					
					s.X+=Convert.ToSingle((e.X-s.X)*this.Connection.From.BelongsTo.Rectangle.Width /(2*len));
					s.Y+=Convert.ToSingle((e.Y-s.Y)*this.Connection.From.BelongsTo.Rectangle.Width /(2*len));
				}
				if(s.X!=e.X)
				{
					alpha = Math.Atan2(e.Y - s.Y,e.X - s.X);
				}
				RectangleF rec = RectangleF.Union(new RectangleF(s,new SizeF(50,50)),new RectangleF(e,new SizeF(50,50)));
			
				Brush brush;
				//alpha =alpha%(Math.PI/2);
				if(Math.Abs(alpha)>Math.PI/2)
					brush = new System.Drawing.Drawing2D.LinearGradientBrush(rec,this.Connection.To.BelongsTo.ShapeColor,this.Connection.From.BelongsTo.ShapeColor,(float) alpha);					
				else
					brush = new System.Drawing.Drawing2D.LinearGradientBrush(rec,this.Connection.From.BelongsTo.ShapeColor,this.Connection.To.BelongsTo.ShapeColor,(float) alpha);
				Pen pn = null;
				switch (this.Connection.LineWeight)
				{
					case ConnectionWeight.Fat:
						pn = new Pen(brush,5f); break;
					case ConnectionWeight.Medium:
						pn = new Pen(brush,3f); break;
					case ConnectionWeight.Thin:
						pn = new Pen(brush,1f); break;

				}
				
				
				g.DrawLine(	pn,s,e);
			}
			
			
			//g.DrawBeziers(pen,mPoints);
		}

		#endregion


	}
}
