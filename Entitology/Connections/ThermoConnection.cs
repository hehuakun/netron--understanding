using System;
using System.Drawing;
namespace Netron.GraphLib.Entitology
{
	/// <summary>
	/// The default connection painter
	/// </summary>
	[Netron.GraphLib.Attributes.NetronGraphConnection("Thermo","D77F4721-3742-4487-91C2-AB7E47E03340", "Netron.GraphLib.Entitology.ThermoConnection")]
	public class ThermoConnection : ConnectionPainter
	{

		private readonly int wi = 9;
		#region Constructor
		public ThermoConnection(Connection connection) : base(connection){}
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
			PointF[] ext;

			for(int k=0; k<Points.Length-1; k++)
			{
				if(Points[k]==Points[k+1]) continue;
				//this is for the offset points
				if(k==0 || k==Points.Length-2)
				{
					//g.DrawLine(pen,mPoints[k],mPoints[k+1]);
					//forget about it
					continue;
				}

				//the first point needs to be the left most one
				PointF s = Points[k] ,e = Points[k+1];
//				if(s.X > e.X)
//				{
//					PointF tmp = s;
//					s = e;
//					e =tmp;
//				}

				
				PointF normal = new PointF(1,0);
				double  alpha =0;
				ext = new PointF[4]; 
				if(s.X!=e.X)
				{
					alpha = Math.Atan2(e.Y - s.Y,e.X - s.X);
					normal = new PointF((float) Math.Sin(alpha),- (float) Math.Cos(alpha));
				}
				ext[0] = s;
				
				
				//use the convex intersection point plus/minus a delta of the normal of the line segment
				ext[1] = new PointF((float) (e.X - wi * normal.X),(float) ( e.Y - wi * normal.Y));
				ext[2] = new PointF((float) (e.X + wi * normal.X),(float) ( e.Y + wi * normal.Y));
				
				ext[3] = s;
				g.DrawLines(Pen,ext);
				RectangleF rec = RectangleF.Union(new RectangleF(s,new SizeF(50,50)),new RectangleF(e,new SizeF(50,50)));
				rec.Offset(-25,-25);
				//rec = RectangleF.FromLTRB(Math.Min(e.X,s.X),Math.Min(e.Y,s.Y),Math.Max(e.X,s.X),Math.Max(e.Y,s.Y))
				//g.DrawRectangle(pen,Rectangle.Round(rec));
				Brush brush;
				//alpha =alpha%(Math.PI/2);
				if(Math.Abs(alpha)>Math.PI/2)
					brush = new System.Drawing.Drawing2D.LinearGradientBrush(rec,Color.Red,Color.WhiteSmoke,(float) alpha);
				else
					brush = new System.Drawing.Drawing2D.LinearGradientBrush(rec,Color.WhiteSmoke,Color.Red,(float) alpha);
				
				g.FillPolygon(brush,ext);
				//g.FillClosedCurve(brush,ext);
				//g.DrawLines(pen, ext);
			}

			//g.DrawLines(pen,this.mPoints);	
			//g.DrawBeziers(pen,mPoints);
		}

		#endregion


	}
}
