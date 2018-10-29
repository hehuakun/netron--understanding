using System;
using Netron.GraphLib;
using System.Drawing;
using System.Windows.Forms;
namespace Netron.Cobalt
{
	/// <summary>
	/// Backround/canvas sample
	/// </summary>
	public class NoLinkingSample : SampleBase
	{
	
		public NoLinkingSample( Mediator mediator):base (mediator)
		{			
		}
	

		public override void Run()
		{
			
		
			Shape shape = mediator.GraphControl.AddShape("4F878611-3196-4d12-BA36-705F502C8A6B", new PointF(80,20));
			shape.Text = "Note that 'Item 1' cannot link to another shape. This is set in the code. \n You can disable connections globally in \n the canvas properties (see 'AllowAddConnection')";
			shape.FitSize(false);

			Shape shape1 = mediator.GraphControl.AddBasicShape("Item 1"); SetShape(shape1);
			//we explicitly set the Guid here to disallow new connections to this item,
			//see the OnNewConnection handler.
			shape1.UID = new Guid("BD70BD65-EF60-4326-A5F7-D4A698297A5C");
			shape1.ShapeColor = Color.OrangeRed;
			shape1.NewConnectionsFrom(false);
			Shape shape2 = mediator.GraphControl.AddBasicShape("Item 2"); SetShape(shape2);
			shape2.ShapeColor = Color.DarkGreen;		
			Connect(shape1, shape2);
	
		}

		
	}
}
