using System;
using System.IO;
using Netron.GraphLib;
using System.Drawing;
using System.Reflection;
namespace Netron.Cobalt
{
	/// <summary>
	/// Backround/canvas sample
	/// </summary>
	public class BackgroundSample : SampleBase
	{
		string[] colors = new string[10]{"Purple", "SteelBlue", "Red", "OrangeRed", "Blue", "Green","Violet","Yellow", "RoyalBlue","Wheat"};
		public BackgroundSample( Mediator mediator):base (mediator)
		{			
		}
		#region ISample Members

		public override void Run()
		{
			Random rnd = mediator.Randomizer;
			int amount = 20;
			try
			{
			
				mediator.GraphControl.BackgroundImagePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\GraphBackground.jpg";
				mediator.GraphControl.BackgroundType = CanvasBackgroundType.Image;
			}
			catch //went wrong, let's use a flat white color
			{
				mediator.GraphControl.BackColor = Color.WhiteSmoke;
				mediator.GraphControl.BackgroundType = CanvasBackgroundType.FlatColor;
			}

			Shape shape = mediator.GraphControl.AddShape("4F878611-3196-4d12-BA36-705F502C8A6B", new PointF(80,20));
			shape.ShapeColor = Color.LightSteelBlue;
			
			shape.Text = "You can change any of the canvas properties by right-clicking the canvas, \n select 'Properties' and change the various elements.";
			shape.FitSize(false);

			if(amount<1) return;
			Shape shape1, shape2;
			if(mediator.GraphControl.Shapes.Count==1)
			{
				shape1 = CreateZShape( new Point(100,100), 66); 
				amount--;
			}
			Point p ;
			int zorder;
			Connection con;
			for(int k=0; k<amount;k++)
			{
				
				shape1 = mediator.GraphControl.Shapes[rnd.Next(1,mediator.GraphControl.Shapes.Count-1)];
				zorder = rnd.Next(0,90);
				p = new Point(rnd.Next(20,mediator.GraphControl.Width-70),rnd.Next(20,mediator.GraphControl.Height-30));
				shape2 = CreateZShape(p, zorder);
				con =  Connect(shape1,shape2, "Z Connection",ConnectionEnd.NoEnds);
				con.LineWeight = ConnectionWeight.Thin;
			}
			mediator.SetLayoutAlgorithm(Netron.GraphLib.GraphLayoutAlgorithms.SpringEmbedder);
			mediator.GraphControl.StartLayout();

		
		}

		
		private Shape CreateZShape(Point p, int zorder)
		{
			Shape shape = mediator.GraphControl.AddShape("6E92FCD0-75DF-4f8f-A5B2-2927E22F4F0F", p);
			shape.Text = zorder.ToString();
			shape.Width = 12;
			shape.Height = 12;
			shape.ShowLabel = false;
			shape.ShapeColor = Color.FromName(colors[mediator.Randomizer.Next(0,9)]);
			shape.ZOrder = zorder;
			//shape.FitSize(false);
			return shape;
		}

		#endregion
	}
}
