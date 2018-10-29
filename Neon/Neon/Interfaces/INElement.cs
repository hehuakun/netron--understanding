using System;

namespace Netron.Neon
{
	/// <summary>
	/// Describes the interface of an element participating
	///  in the mediator pattern of the framework
	/// </summary>
	public interface INElement
	{
		/// <summary>
		/// Gets or sets the root of the mediator pattern
		/// </summary>
		INUIMediator Root {get; set;}
	}
	
}
