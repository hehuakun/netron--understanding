using System;
using Netron.GraphLib;
using System.Drawing;
using System.Windows.Forms;
namespace Netron.Cobalt
{
	/// <summary>
	/// Backround/canvas sample
	/// </summary>
	public class NoNewShapesSample : SampleBase
	{
	
		public NoNewShapesSample( Mediator mediator):base (mediator)
		{			
		}
	

		public override void Run()
		{
			
			Shape shape = mediator.GraphControl.AddShape("4F878611-3196-4d12-BA36-705F502C8A6B", new PointF(80,20));
			shape.Text = "In this example you cannot add new shapes to the canvas. \n Try both the context menu and drag-drop from the shapes library.";
			shape.FitSize(false);
			
			mediator.GraphControl.AllowAddShape = false;

			//some shapes randomly placed on the canvas
			Shape shape1 = mediator.GraphControl.AddBasicShape("Item 1"); SetShape(shape1);
			Shape shape2 = mediator.GraphControl.AddBasicShape("Item 2"); SetShape(shape2);
			Shape shape3 = mediator.GraphControl.AddBasicShape("Item 3"); SetShape(shape3);
			Shape shape4 = mediator.GraphControl.AddBasicShape("Item 4"); SetShape(shape4);

			//some connections

			Connect(shape1, shape2);
			Connect(shape2, shape3);
			Connect(shape2, shape4);
	

		}
	}
}
