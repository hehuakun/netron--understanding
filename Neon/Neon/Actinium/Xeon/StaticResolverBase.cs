using System.IO;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Threading;
using System.Reflection;

namespace  Netron.Xeon
{
	/// <summary>
	/// This is a base class for static resolvers. A static resolver is a resolver
	/// whose answer does not depend on the parameters of the request
	/// </summary>
	/// <remarks>
	/// This class simplifies the development of static resolvers. It provides an 
	/// implementation of Answer based only on GetResourceAsStream.
	/// </remarks>
	public abstract class StaticResolverBase : IResourceResolver
	{
	
		/// <summary>
		/// Answers the web request.
		/// </summary>
		/// <param name="aRequest"></param>
		public void Answer(WebRequest aRequest)
		{
			Stream sm = GetResourceAsStream(aRequest.File);
			if(sm != null)
			{
				aRequest.Response.SendContent(MIMEManager.getMIME(Path.GetExtension(aRequest.File)));
				aRequest.Response.Write(sm);
				sm.Close();
			}
			else
				aRequest.Response.SendFileNotFound();
		}
		public abstract System.IO.Stream GetResourceAsStream(string sFileName);
		public abstract bool Resolves(WebRequest aRequest);
	}
}
