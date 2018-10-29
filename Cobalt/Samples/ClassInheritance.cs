using System;
using Netron.GraphLib;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
namespace Netron.Cobalt
{
	/// <summary>
	/// Backround/canvas sample
	/// </summary>
	public class ClassInheritance : SampleBase
	{
	
		public ClassInheritance( Mediator mediator):base (mediator)
		{			
		}
	

		public override void Run()
		{
			
		
			Shape engine = mediator.GraphControl.AddShape("F9E27E10-1B57-4580-B7DE-3018911BE1DD", new PointF(80,20));
			SetClass(engine, "Engine","Generic engine", Color.Lavender);
			
			Shape car = mediator.GraphControl.AddShape("F9E27E10-1B57-4580-B7DE-3018911BE1DD", new PointF(80,150));
			SetClass(car, "Car","A car", Color.LightSlateGray);

			Connection con = mediator.GraphControl.AddConnection(engine.Connectors["Bottom"], car.Connectors["Top"]);
			con.LineColor = Color.DimGray;
			con.LineEnd = ConnectionEnd.LeftOpenArrow;

			Shape mycar = mediator.GraphControl.AddShape("F9E27E10-1B57-4580-B7DE-3018911BE1DD", new PointF(80,10));
			SetClass(mycar, "My car","My own car", Color.LightSeaGreen);

			Shape hercar = mediator.GraphControl.AddShape("F9E27E10-1B57-4580-B7DE-3018911BE1DD", new PointF(80,10));
			SetClass(hercar, "Her car","Her car", Color.LightSalmon);



			con = mediator.GraphControl.AddConnection(car.Connectors["Bottom"], hercar.Connectors["Top"]);
			con.LineColor = Color.Green;
			con.LineStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
			con.LineEnd = ConnectionEnd.LeftOpenArrow;

			con = mediator.GraphControl.AddConnection(car.Connectors["Bottom"], mycar.Connectors["Top"]);
			con.LineColor = Color.Green;
			con.LineStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
			con.LineEnd = ConnectionEnd.LeftOpenArrow;

			//the label
			GraphLib.BasicShapes.TextLabel shape = mediator.GraphControl.AddShape("4F878611-3196-4d12-BA36-705F502C8A6B", new PointF(50,400)) as GraphLib.BasicShapes.TextLabel;
			shape.Text = @"Note that in this example the position was not coded via the API " + Environment.NewLine + "but calculated by means of the tree-layout.";
			shape.ShowPage = false;
			shape.FitSize(false);
			shape.IsFixed = true;
			
			mediator.GraphControl.GraphLayoutAlgorithm = GraphLib.GraphLayoutAlgorithms.Tree;
			mediator.GraphControl.StartLayout();

			

			

	
		}

		private void SetClass(Shape shape, string className, string subTitle, Color color)
		{	
			PropertyInfo info;
			
			info = shape.GetType().GetProperty("ClassName");			
			info.SetValue(shape, className, null);

			info = shape.GetType().GetProperty("SubTitle");
			info.SetValue(shape, subTitle, null);

			info = shape.GetType().GetProperty("ClassName");
			info.SetValue(shape, className, null);
			
			MethodInfo minfo = shape.GetType().GetMethod("Collapse");
			minfo.Invoke(shape, null);

			shape.ShapeColor = color;

			

		}

		
	}
}
