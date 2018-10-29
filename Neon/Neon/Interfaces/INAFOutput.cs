using System;

namespace Netron.Neon
{
	/// <summary>
	/// Interface a NAF outputter
	/// </summary>
	public interface INOutput 
	{

		/// <summary>
		/// Clears the default channel
		/// </summary>
		void ClearAll();
		/// <summary>
		/// Write a line to the output in the default channel
		/// </summary>
		/// <param name="message"></param>
		void WriteLine(string message);
	}
	
}
