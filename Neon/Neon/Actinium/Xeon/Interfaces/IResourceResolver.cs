using System;
using System.IO;
namespace  Netron.Xeon
{
	/// <summary>
	/// This is the resolver class.
	/// </summary>
	/// <remarks>
	/// A resolver is like a plugable file system. See the <see cref="Xeon"/> class.
	/// </remarks>
	public interface IResourceResolver
	{
		/// <summary>
		/// This function tells the WebServer whether the resolver recognizes the requested resource
		/// </summary>
		/// <param name="aRequest">the request object</param>
		/// <returns>true - if it recognizes it, false otherwise</returns>
		bool Resolves(WebRequest aRequest);
		/// <summary>
		/// If the resolver has recognized the request, this function is called to let the resolver
		/// generate the content sent back to the client
		/// </summary>
		/// <param name="aRequest">the request object</param>
		void Answer(WebRequest aRequest);

		/// <summary>
		/// Gets a static resource (one that doesnt change depending on the request)
		/// as a Stream
		/// </summary>
		Stream GetResourceAsStream(string sFileName);
	}
}
