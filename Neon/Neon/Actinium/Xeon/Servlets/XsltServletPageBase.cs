using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Threading;
using System.Reflection;
using System.Xml;
using System.Xml.Xsl;


namespace  Netron.Xeon
{

	

	
	public abstract class XsltServletPageBase : ServletPageBase
	{
		public abstract void getTransformation(WebRequest aRequest, out XslTransform aTransformation, out XsltArgumentList aArguments);
		public abstract XmlDocument getXML(WebRequest aRequest);

		public override void Answer(WebRequest aRequest)
		{
			XmlDocument xdoc = getXML(aRequest);

			if(XMLDumpPath != null)
			{
				string sFileName = aRequest.File;
				if(sFileName.StartsWith("/"))
					sFileName = sFileName.Substring(1);
				sFileName = Path.Combine(XMLDumpPath, sFileName); 
				sFileName += ".xml";
				FileInfo inf = new FileInfo(sFileName);
				if(!inf.Directory.Exists)
					inf.Directory.Create();
				xdoc.Save(sFileName);
			}

			XslTransform xtrans;
			XsltArgumentList xargs;
			getTransformation(aRequest, out xtrans, out xargs);

			StringWriter wr = new StringWriter();
			xtrans.Transform(xdoc, xargs, wr);
			
			aRequest.Response.SendContent("text/html");
			aRequest.Response.WriteLine(wr.ToString());
		}

		public static string XMLDumpPath = null;
	}

}
