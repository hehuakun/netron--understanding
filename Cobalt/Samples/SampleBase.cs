using System;
using System.Drawing;
using Netron.GraphLib;
namespace Netron.Cobalt
{
	/// <summary>
	/// Base class fopr the Cobalt samples
	/// </summary>
	public class SampleBase : ISample
	{
		protected Mediator mediator;

		public SampleBase(Mediator mediator )
		{
			this.mediator = mediator;
		}


		protected void AddRandomNodes(int amount)
		{
			mediator.AddRandomNodes(amount);
		}

		/// <summary>
		/// Override this method to start a sample/demo
		/// </summary>
		public virtual void Run()
		{
			return;
		}

		#region Shape creation utils
		protected void SetShape(Shape shape)
		{
			mediator.SetShape(shape);
		}

		protected Connection Connect(Shape s1, Shape s2)
		{
			Connection con = mediator.Connect(s1,s2);
			con.LineColor = Color.Black;
			return con;
		}
		protected Connection Connect(Shape s1, Shape s2, ConnectionEnd lineEnd)
		{
			
			
			return mediator.Connect(s1,s2,lineEnd);
		}
		protected Connection Connect(Shape s1, Shape s2, string linePath)
		{
			return mediator.Connect(s1,s2,linePath);
		}

		protected Connection Connect(Shape s1, Shape s2, string linePath, ConnectionEnd lineEnd)
		{
			return mediator.Connect(s1,s2,linePath,lineEnd);

		}

		#endregion

	}
}
