using System;
using System.Drawing;
namespace Netron.GraphLib.Entitology
{
	/// <summary>
	/// A multi-line connection painter, this code is part of an online tutorial: see the Netron site for more information
	/// http://netron.sf.net
	/// </summary>
	[Netron.GraphLib.Attributes.NetronGraphConnection("Multiline","B57546B1-0D6B-473d-8EFC-7597822D9469", "Netron.GraphLib.Entitology.MultiLineConnection")]
	public class MultiLineConnection : ConnectionPainter
	{
		private readonly int interpoints = 12;
		PointF[] ext;
		private readonly int wi = 5;

		#region Constructor
		public MultiLineConnection(Connection connection) : base(connection){}
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
				{
					//the following would draw a single straight line 
					//but for this multiline connection we'll neglect the space between the connector 
					//and the adjacent point and simply continue with the other points (if any)
					//g.DrawLine(Pen,Points[k],Points[k+1]);
					continue;
				}

				//the first point needs to be the left most one
				PointF s = Points[k] ,e = Points[k+1];
				if(s.X > e.X)
				{
					PointF tmp = s;
					s = e;
					e =tmp;
				}
				
				PointF normal = new PointF(1,0);
				double  alpha =0;
				//the ext points define the parallell lines and are constructed by means of the normal onto the direct (underlying) connection
				ext = new PointF[4]; 
				if(s.X!=e.X)
				{
					alpha = Math.Atan2(e.Y - s.Y,e.X - s.X);
					normal = new PointF((float) Math.Sin(alpha),- (float) Math.Cos(alpha));
				}
				//use the convex intersection point plus/minus a delta of the normal of the line segment
				ext[0] = new PointF((float) (s.X - wi * normal.X),(float) ( s.Y - wi * normal.Y));
				ext[1] = new PointF((float) (e.X - wi * normal.X),(float) ( e.Y - wi * normal.Y));

				ext[2] = new PointF((float) (s.X + wi * normal.X),(float) ( s.Y + wi * normal.Y));
				ext[3] = new PointF((float) (e.X + wi * normal.X),(float) ( e.Y + wi * normal.Y));
				
				g.DrawLine(Pen, ext[0], ext[1]);
				g.DrawLine(Pen, ext[2], ext[3]);
			}

		}

		#endregion


	}
}

