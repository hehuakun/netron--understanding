using System;
using Netron.GraphLib.UI;
namespace Netron.Cobalt
{
	/// <summary>
	/// 
	/// </summary>
	public class SampleFactory
	{
		private Mediator mediator;

		public SampleFactory(Mediator mediator)
		{
			this.mediator = mediator;
		}

		public ISample GetSample(Samples sample)
		{
			switch(sample)
			{
				case Samples.ClassInheritance:
					return new ClassInheritance(mediator);
				case Samples.Background:
					return new BackgroundSample(mediator);
				case Samples.RandomTree:
					return new RandomTreeSample(mediator);
				case Samples.Controls:
					return new ControlsSample(mediator);
				case Samples.FancyConnections:
					return new FancyConnectionsSample(mediator);
				case Samples.ItemCannotMove:
					return new ItemCannotMoveSample(mediator);
				case Samples.Layering:
					return new LayeringSample(mediator);
				case Samples.Layout:
					return new LayoutSample(mediator);
				case Samples.NoLinking:
					return new NoLinkingSample(mediator);
				case Samples.NoNewConnections:
					return new NoNewConnectionsSample(mediator);
				case Samples.NoNewShapes:
					return new NoNewShapesSample(mediator);
				case Samples.ShapeEvents:
					return new ShapeEventsSample(mediator);
				case Samples.Snap:
					return new SnapSample(mediator);
				case Samples.TreeAsCode:
					return new TreeAsCodeSample(mediator);
				case Samples.ZOrder:
					return new ZOrderSample(mediator);
				case Samples.GraphLibClasses:
					return new GraphLibClassesSample(mediator);
				case Samples.GraphLibInterfaces:
					return new GraphLibInterfacesSample(mediator);
					
			}
			return null;
		}
	}
}
