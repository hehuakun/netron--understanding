using System;
using Netron.GraphLib;
using System.Drawing;
using System.Windows.Forms;
namespace Netron.Cobalt
{
	/// <summary>
	/// Backround/canvas sample
	/// </summary>
	public class TreeAsCodeSample : SampleBase
	{
	
		public TreeAsCodeSample( Mediator mediator):base (mediator)
		{			
		}
	

		public override void Run()
		{
			
			Point p = new Point(10,10);
			for(int k =0; k<10;k++)
			{
				mediator.GraphControl.AddBasicShape("Item " + k ,p);
			}
			
			
			ShapeCollection nodes = mediator.GraphControl.Shapes;
			Connect(nodes[0], nodes[1],ConnectionEnd.NoEnds);
			Connect(nodes[0], nodes[2],ConnectionEnd.NoEnds);
			Connect(nodes[0], nodes[3],ConnectionEnd.NoEnds);
			Connect(nodes[2], nodes[4],ConnectionEnd.NoEnds);
			Connect(nodes[2], nodes[5],ConnectionEnd.NoEnds);
			Connect(nodes[3], nodes[6],ConnectionEnd.NoEnds);
			Connect(nodes[3], nodes[7],ConnectionEnd.NoEnds);
			Connect(nodes[7], nodes[8],ConnectionEnd.NoEnds);
			Connect(nodes[7], nodes[9],ConnectionEnd.NoEnds);
			
			

			mediator.SetLayoutAlgorithm(GraphLayoutAlgorithms.Tree);
			mediator.GraphControl.StartLayout();
			mediator.GraphControl.Focus();
			mediator.GraphControl.Invalidate(mediator.GraphControl.ClientRectangle);
			mediator.GraphControl.Update();
		}

	

	
	}
}
