using System;
using Netron.GraphLib;
using System.Drawing;
using System.Windows.Forms;
namespace Netron.Cobalt
{
	/// <summary>
	/// Backround/canvas sample
	/// </summary>
	public class ItemCannotMoveSample : SampleBase
	{
	
		public ItemCannotMoveSample( Mediator mediator):base (mediator)
		{			
		}
	

		public override void Run()
		{
			Shape shape = mediator.GraphControl.AddShape("4F878611-3196-4d12-BA36-705F502C8A6B", new PointF(80,20));
			shape.Text = "Note that 'Item 1' cannot be moved. This is set in the code. \n You can disable shape moves globally in \n the canvas properties (see 'AllowShapeMove')";
			shape.FitSize(false);

			Shape shape1 = mediator.GraphControl.AddBasicShape("Item 1"); SetShape(shape1);
			
			Shape shape2 = mediator.GraphControl.AddBasicShape("Item 2"); SetShape(shape2);
			shape1.CanMove=false;
			Connect(shape1, shape2);
	

		}



	
	}
}
