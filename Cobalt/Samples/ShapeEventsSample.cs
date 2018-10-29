using System;
using Netron.GraphLib;
using System.Drawing;
using System.Windows.Forms;
namespace Netron.Cobalt
{
	/// <summary>
	/// Backround/canvas sample
	/// </summary>
	public class ShapeEventsSample : SampleBase
	{
	
		public ShapeEventsSample( Mediator mediator):base (mediator)
		{			
		}
	

		public override void Run()
		{
			Shape shape = mediator.GraphControl.AddShape("4F878611-3196-4d12-BA36-705F502C8A6B", new PointF(80,20));
			shape.Text = "You can attach handler to shapes...etc.";
			shape.FitSize(false);

			Shape shape1 = mediator.GraphControl.AddBasicShape("Item 1"); SetShape(shape1);
			
			shape1.ShapeColor = Color.OrangeRed;
			
			shape1.OnMouseDown+=new MouseEventHandler(shape1_OnMouseDown);
			shape1.OnMouseUp+=new MouseEventHandler(shape1_OnMouseUp);
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
