using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Threading;
using System.Reflection;

namespace  Netron.Xeon
{
	/// <summary>
	/// This is a static resolver that links the virtual adress space of the
	/// web server to a physical file system
	/// </summary>
	public class ContentLocationResolver : StaticResolverBase
	{
		string m_sContentPath, m_sMapToPath;
		/// <summary>
		/// Creates a ContentLocationResolver
		/// </summary>
		/// <param name="sContentPath">the physical path that is linked</param>
		/// <remarks>
		/// This contructor installs the filesystem in the root of the virtual
		/// adress space of the web server
		/// </remarks>
		public ContentLocationResolver(string sContentPath)
		{
			m_sContentPath = sContentPath;
			m_sMapToPath = "";
		}

		/// <summary>
		/// Same as above
		/// </summary>
		/// <param name="sContentPath">the physical path that is linked</param>
		/// <param name="sMapToPath">the root in the virtual file system</param>
		public ContentLocationResolver(string sContentPath, string sMapToPath)
		{
			m_sContentPath = sContentPath;
			if(sMapToPath.StartsWith("/"))
				sMapToPath = sMapToPath.Substring(1);
			if(! sMapToPath.EndsWith("/") && sMapToPath.Length > 0)
				sMapToPath += "/";

			m_sMapToPath = sMapToPath;
		}

		public override Stream GetResourceAsStream(string sFileName)
		{
			if(sFileName.StartsWith("/"))
				sFileName = sFileName.Substring(1);

			if(!sFileName.StartsWith(m_sMapToPath))
				return null;

			sFileName = sFileName.Substring(m_sMapToPath.Length);
			string sFullName = Path.Combine(m_sContentPath, sFileName);
			if(!File.Exists(sFullName))
				return null;
			else
				return File.Open(sFullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
		}
		public override bool Resolves(WebRequest aRequest)
		{
			string sFileName = aRequest.File;
			if(sFileName.StartsWith("/"))
				sFileName = sFileName.Substring(1);

			if(!sFileName.StartsWith(m_sMapToPath))
				return false;

			sFileName = sFileName.Substring(m_sMapToPath.Length);
			string sFullName = Path.Combine(m_sContentPath, sFileName);
			return File.Exists(sFullName);
		}
	}

}
