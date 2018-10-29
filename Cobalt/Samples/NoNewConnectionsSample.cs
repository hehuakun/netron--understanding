using System;
using Netron.GraphLib;
using System.Drawing;
using System.Windows.Forms;
namespace Netron.Cobalt
{
	/// <summary>
	/// Backround/canvas sample
	/// </summary>
	public class NoNewConnectionsSample : SampleBase
	{
	
		public NoNewConnectionsSample( Mediator mediator):base (mediator)
		{			
		}
	

		public override void Run()
		{
			Shape shape = mediator.GraphControl.AddShape("4F878611-3196-4d12-BA36-705F502C8A6B", new PointF(80,20));
			shape.Text = "In this example you cannot \n add new connections between shapes.";
			shape.FitSize(false);
			mediator.GraphControl.AllowAddConnection = false;

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

		/// <summary>
		/// Do something when the shape was clicked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void shape1_OnMouseDown(object sender, MouseEventArgs e)
		{
			mediator.Output(Environment.NewLine +  "This shows the delegated mouse-down event");
		}

		/// <summary>
		/// Do something when the shape was clicked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void shape1_OnMouseUp(object sender, MouseEventArgs e)
		{
			mediator.Output(Environment.NewLine + "This shows the delegated mouse-up event");
			
		}

	
	}
}
