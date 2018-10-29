using System;
using Netron.GraphLib;
using System.Drawing;
using System.Windows.Forms;
namespace Netron.Cobalt
{
	/// <summary>
	/// Backround/canvas sample
	/// </summary>
	public class LayeringSample : SampleBase
	{
	
		public LayeringSample( Mediator mediator):base (mediator)
		{			
		}
	

		public override void Run()
		{
			
			//some shapes randomly placed on the canvas
			Shape shape1 = mediator.GraphControl.AddBasicShape("Item 1"); SetShape(shape1);
			Shape shape2 = mediator.GraphControl.AddBasicShape("Item 2"); SetShape(shape2);
			Shape shape3 = mediator.GraphControl.AddBasicShape("Item 3"); SetShape(shape3);
			Shape shape4 = mediator.GraphControl.AddBasicShape("Item 4"); SetShape(shape4);
			Shape shape5 = mediator.GraphControl.AddBasicShape("Item 5"); SetShape(shape5);
			Shape shape6 = mediator.GraphControl.AddBasicShape("Item 6"); SetShape(shape6);
			Shape shape7 = mediator.GraphControl.AddBasicShape("Item 7"); SetShape(shape7);
			Shape shape8 = mediator.GraphControl.AddBasicShape("Item 8"); SetShape(shape8);

			GraphLayer redlayer = new GraphLayer("Red layer", Color.WhiteSmoke,26);
			redlayer.UseColor = true;
			mediator.GraphControl.Layers.Add(redlayer);
			
			GraphLayer bluelayer = new GraphLayer("Blue layer", Color.DarkBlue,46);
			bluelayer.UseColor = true;			
			mediator.GraphControl.Layers.Add(bluelayer);

			shape1.SetLayer(1);
			shape2.SetLayer(1);
			shape7.SetLayer(2);

			//some connections			
			Connection cn = Connect(shape1, shape2);
			cn.SetLayer(1);
			Connect(shape2, shape3);
			Connect(shape2, shape4);
			Connect(shape4, shape5);
			Connect(shape4, shape6);
			Connect(shape2, shape7);
			Connect(shape3, shape8);
			Connect(shape7, shape8);
		}

	

	
	}
}
