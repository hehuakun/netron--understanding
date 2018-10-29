using System;
using System.Drawing;
namespace Netron.Neon
{
	/// <summary>
	/// A line representing data evolution in the chart control	/// 
	/// </summary>
	public class ChartLine
	{
		#region Fields
		private Random rnd;
		protected float scale = 1f;
		protected Color lineColor;
		protected string name = "[Not set]";
		protected float lineWidth = 1f;
		#endregion 

		#region Constructor
		public ChartLine()
		{
			rnd = new Random();
			lineColor = Color.FromArgb(rnd.Next(100,255), rnd.Next(100,255), rnd.Next(100,255));
			
		}

		public ChartLine(Color color)
		{
			rnd = new Random();
			lineColor = color;
		}
		public ChartLine(string name, Color color)
		{
			rnd = new Random();
			this.name = name;
			lineColor = color;
		}
		#endregion

		#region Properties

		public float Scale
		{
			get{return scale;}
			set{scale = value;}
		}
		public float LineWidth
		{
			get{return lineWidth;}
			set{lineWidth = value;}
		}

		public Color LineColor
		{
			get{return lineColor;}
			set{lineColor = value;}
		}

		public string Name
		{
			get{return name;}
			set{name= value;}
		}
		#endregion
	}
}
