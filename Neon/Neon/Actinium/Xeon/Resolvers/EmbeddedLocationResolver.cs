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
	/// This resolver links the virtual file system to the embedded reources in
	/// some assembly.
	/// </summary>
	/// <example>
	/// if aRootLocation is "ImageViewerExample" and m_mapToPath is "view/"
	/// '/view/stefan/stf.xslt' will map to the manifest resource 'ImageViewerExample.stefan.stf.xslt'
	/// </example>
	public class EmbeddedLocationResolver  : StaticResolverBase
	{
		string m_rootLocation, m_mapToPath;
		Assembly m_assembly;
		public EmbeddedLocationResolver(Assembly aAssembly, string aRootLocation)
		{
			m_rootLocation = aRootLocation;
			m_mapToPath = "";
			m_assembly = aAssembly;
		}


		public EmbeddedLocationResolver(Assembly aAssembly, string aRootLocation, string aMapToPath)
		{
			m_rootLocation = aRootLocation;
			if(!m_rootLocation.EndsWith("."))
				m_rootLocation = m_rootLocation + ".";
			
			if(aMapToPath.StartsWith("/"))
				aMapToPath = aMapToPath.Substring(1);
			if(! aMapToPath.EndsWith("/") && aMapToPath.Length > 0)
				aMapToPath += "/";

			m_mapToPath = aMapToPath;
			m_assembly = aAssembly;
		}

		public override Stream GetResourceAsStream(string sFileName)
		{
			if(sFileName.StartsWith("/"))
				sFileName = sFileName.Substring(1);

			if(!sFileName.StartsWith(m_mapToPath))
				return null;

			sFileName = sFileName.Substring(m_mapToPath.Length);

			sFileName = m_rootLocation + sFileName.Replace("/", ".");
			return m_assembly.GetManifestResourceStream(sFileName);
		}

		public override bool Resolves(WebRequest aRequest)
		{
			string sFileName = aRequest.File;

			if(sFileName.StartsWith("/"))
				sFileName = sFileName.Substring(1);

			if(!sFileName.StartsWith(m_mapToPath))
				return false;

			sFileName = sFileName.Substring(m_mapToPath.Length);

			sFileName = m_rootLocation + sFileName.Replace("/", ".");

			//string[] sNames = m_assembly.GetManifestResourceNames();

			return m_assembly.GetManifestResourceInfo(sFileName) != null;
		}
	}
}
