using System;
using System.Drawing;
namespace Netron.GraphLib.Entitology
{
	/// <summary>
	/// The default connection painter
	/// </summary>
	[Netron.GraphLib.Attributes.NetronGraphConnection("Resistant","369411CB-97F6-4415-9473-159513C0BFB2", "Netron.GraphLib.Entitology.ResistantConnection")]
	public class ResistantConnection : ConnectionPainter
	{
		private readonly int interpoints = 12;
		private readonly int wi = 9;
		#region Constructor
		public ResistantConnection(Connection connection) : base(connection){}
		#endregion

		#region Methods
		

		 public override bool Hit(System.Drawing.PointF p)
		{
			bool join = false;		
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
				{g.DrawLine(Pen,Points[k],Points[k+1]);continue;}

				//the first point needs to be the left most one
				PointF s = Points[k] ,e = Points[k+1];
				if(s.X > e.X)
				{
					PointF tmp = s;
					s = e;
					e =tmp;
				}

				
				PointF normal = PointF.Empty;
				double  alpha =0;
				ext = new PointF[2+2*interpoints]; //two points for each intermediate line-piece
				if(s.X!=e.X)
				{
					alpha = Math.Atan2(e.Y - s.Y,e.X - s.X);
					normal = new PointF((float) Math.Sin(alpha),- (float) Math.Cos(alpha));
				}
				ext[0] = s;
				float step = 1F/(interpoints+1);
				for(int m =1; m<=interpoints;m++)
				{
					//use the convex intersection point plus/minus a delta of the normal of the line segment
					ext[2*m-1] = new PointF((float) ((1-m*step) * s.X + m*step * e.X - wi * normal.X),(float) ( (1-m*step) * s.Y + m*step * e.Y - wi * normal.Y));
					ext[2*m]    = new PointF((float) ((1-m*step) * s.X + m*step * e.X + wi * normal.X),(float) ( (1-m*step) * s.Y + m*step * e.Y + wi * normal.Y));
				}
				ext[2+2*interpoints-1] = e;
				g.DrawLines(Pen,ext);
			}

			//g.DrawLines(pen,this.mPoints);	
			//g.DrawBeziers(pen,mPoints);
		}

		#endregion


	}
}
