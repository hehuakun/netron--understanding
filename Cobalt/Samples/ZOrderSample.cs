using System;
using Netron.GraphLib;
using System.Drawing;
using System.Windows.Forms;
namespace Netron.Cobalt
{
	/// <summary>
	/// Backround/canvas sample
	/// </summary>
	public class ZOrderSample : SampleBase
	{
	
		public ZOrderSample( Mediator mediator):base (mediator)
		{			
		}
	

		public override void Run()
		{
			
			Shape shape = mediator.GraphControl.AddShape("4F878611-3196-4d12-BA36-705F502C8A6B", new PointF(50,20));
			shape.Text = "Z-ordering example";
			shape.FitSize(false);

			Shape shape15050 = CreateZShape(new Point(150,50),90);							//
			Shape shape50100 = CreateZShape(new Point(50,100),60);			//								
			Shape shape250100 = CreateZShape(new Point(500,100),60);										//
			Shape shape150150 = CreateZShape(new Point(150,150),30);					//

			Shape shape150200 = CreateZShape(new Point(150,200),90);							//
			Shape shape50250 = CreateZShape(new Point(10,250),60);			//								
			Shape shape250250 = CreateZShape(new Point(250,250),60);										//
			Shape shape150300 = CreateZShape(new Point(150,400),30);					//
			
			
			Connection con1 = Connect(shape50100, shape15050, "Z Connection",ConnectionEnd.NoEnds);
			Connection con3 = Connect(shape50100, shape150150, "Z Connection",ConnectionEnd.NoEnds);
			Connection con2 = Connect(shape250100, shape15050, "Z Connection",ConnectionEnd.NoEnds);			
			Connection con4 = Connect(shape250100, shape150150, "Z Connection",ConnectionEnd.NoEnds);


			Connection con5 = Connect(shape50250, shape150200, "Z Connection",ConnectionEnd.NoEnds);
			Connection con7 = Connect(shape50250, shape150300, "Z Connection",ConnectionEnd.NoEnds);
			Connection con6 = Connect(shape250250, shape150200, "Z Connection",ConnectionEnd.NoEnds);			
			Connection con8 = Connect(shape250250, shape150300, "Z Connection",ConnectionEnd.NoEnds);

			Connection con9 = Connect(shape50100, shape50250, "Z Connection",ConnectionEnd.NoEnds);
			Connection con10 = Connect(shape250100, shape250250, "Z Connection",ConnectionEnd.NoEnds);
			Connection con11 = Connect(shape150150, shape150300, "Z Connection",ConnectionEnd.NoEnds);
			Connection con12 = Connect(shape15050, shape150200, "Z Connection",ConnectionEnd.NoEnds);


			mediator.SetLayoutAlgorithm(Netron.GraphLib.GraphLayoutAlgorithms.SpringEmbedder);
			mediator.GraphControl.StartLayout();
		}

	
		private Shape CreateZShape(Point p, int zorder)
		{
			Shape shape = mediator.GraphControl.AddShape("6E92FCD0-75DF-4f8f-A5B2-2927E22F4F0F", p);
			shape.Text = zorder.ToString();
			shape.ZOrder = zorder;
			shape.ShowLabel = false;
			//shape.FitSize(false);
			return shape;
		}
	
	}
}
