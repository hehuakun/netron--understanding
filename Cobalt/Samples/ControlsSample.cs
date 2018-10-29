using System;
using Netron.GraphLib;
using System.Drawing;
using System.Windows.Forms;
namespace Netron.Cobalt
{
	/// <summary>
	/// Backround/canvas sample
	/// </summary>
	public class ControlsSample : SampleBase
	{
	
		public ControlsSample( Mediator mediator):base (mediator)
		{			
		}
	

		public override void Run()
		{
			
			Shape shape = mediator.GraphControl.AddShape("4F878611-3196-4d12-BA36-705F502C8A6B", new PointF(50,20));
			shape.Text = "Shown here is an assortiment of shapes with WinForms-like controls.";
			shape.FitSize(false);

			Shape shape1 = mediator.GraphControl.AddShape("28A748DC-2A0B-4cf2-B74D-FD1AF44FBE35", new PointF(80,120));
			/*
						Shape shape1 = mediator.GraphControl.AddBasicShape("Item 1"); SetShape(shape1);
						Shape shape2 = mediator.GraphControl.AddBasicShape("Item 2"); SetShape(shape2);
						Shape shape3 = mediator.GraphControl.AddBasicShape("Item 3"); SetShape(shape3);
						Shape shape4 = mediator.GraphControl.AddBasicShape("Item 4"); SetShape(shape4);
						Connect(shape1, shape2, "Resistant");
						Connection con = Connect(shape1, shape3, "Thermo");
						con.LineEnd = ConnectionEnd.NoEnds;
						con = Connect(shape2,shape4,"Bezier");
						con.LineEnd = ConnectionEnd.BothFilledArrow;
			*/			
		}

	

	
	}
}
