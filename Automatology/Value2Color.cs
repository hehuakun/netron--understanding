using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Netron.GraphLib.Attributes;
using Netron.GraphLib.UI;
using Netron.GraphLib.Interfaces;
using Netron.GraphLib;
using System.Text;

namespace Netron.AutomataShapes
{
	[Serializable]
	[Description("Value to color transformation")]
	[NetronGraphShape("Value2Color","6C7072D9-9A23-4bc9-A6D6-1B08CF2354E2","Automata","Netron.AutomataShapes.Value2Color","Converts data to color.")]
	public class Value2Color : Shape
	{
		#region Fields
		private Connector OutConnector;
		private Connector InConnector;
		private Color baseColor;

		#endregion

		#region Properties
		public Color BaseColor{ get{return baseColor;} set{baseColor=value;}}
		#endregion

		#region Constructor
		public Value2Color():base()			
		{
			this.IsResizable=false;
			Rectangle = new RectangleF(0, 0, 70, 30);
		
			OutConnector=new Connector(this,"Output",true);
			InConnector = new Connector(this,"Input",false);
			this.Connectors.Add(OutConnector);
			this.Connectors.Add(InConnector);
		}
		#endregion

		#region Methods
		public override void Paint(System.Drawing.Graphics g)
		{
			
			SizeF ss=g.MeasureString("Random",Font);
			
			RectangleF r = Rectangle;

			g.DrawRectangle(new Pen(Color.Black,IsSelected ? 2F : 1F),r.Left,r.Top,r.Width,r.Height);
			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			
			g.FillRectangle(new SolidBrush(ShapeColor), r.X, r.Y, r.Width , 12 );
			Color Background = IsSelected ? Color.LightSteelBlue : Color.WhiteSmoke;
			g.FillRectangle(new SolidBrush(Background), r.X, r.Y+12, r.Width , r.Height-12 );					
			g.DrawString("Value->Color", Font, new SolidBrush(TextColor), r.Left + (r.Width / 2), r.Top , sf);
			sf.Alignment=StringAlignment.Far;
			//g.DrawString(_OutDataType.ToString(), mFont, new SolidBrush(Color.Black), r.Right -2, r.Top +2*(r.Height/3)-7, sf);

			
		}
		
	
		public override System.Drawing.PointF ConnectionPoint(Connector c)
		{
			
			RectangleF r = Rectangle;
			if (c == OutConnector) return new PointF(r.Right, r.Top+(r.Height/2));
			if (c == InConnector) return new PointF(r.Left, r.Top+(r.Height/2));
			return base.ConnectionPoint (c);
			//return new PointF(0, 0);
		}
		public override void Update()
		{
			//clear the sends values
			
			this.OutConnector.Sends.Clear();		
			if (InConnector.Receives.Count>0)
			{
				int inp =Convert.ToInt32( InConnector.Receives[0]);
				this.OutConnector.Sends.Add(Color.FromArgb(Math.Max(inp,0),baseColor));
					
			}
			this.InConnector.Receives.Clear();
			
			//base.Update ();

		}


		/// <summary>
		/// Overrides the default thumbnail used in the shape viewer
		/// </summary>
		/// <returns></returns>
		public override Bitmap GetThumbnail()
		{
			Bitmap bmp=null;
			try
			{
				Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.AutomataShapes.Resources.Value2Color.gif");
					
				bmp= Bitmap.FromStream(stream) as Bitmap;
				stream.Close();
				stream=null;
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.Message);
			}
			return bmp;
		}
		#endregion

	}
	
}
