using System;

namespace Netron.Cobalt
{
	public interface ISample
	{
		void Run();
	}

	/// <summary>
	/// This interface helps identifying the tabs
	/// </summary>
	public interface ICobaltTab 
	{
		/// <summary>
		/// Returns which kinda tab the control is
		/// </summary>
		TabTypes TabType {get;}
		/// <summary>
		/// The unique identifier of the tab, useful if a certain tab can have multiple instances
		/// like the scripting tab to edit multiple files
		/// </summary>
		string TabIdentifier {get; set;}		
	}
}
