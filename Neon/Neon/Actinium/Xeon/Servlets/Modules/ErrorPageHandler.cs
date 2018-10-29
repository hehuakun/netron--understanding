using System;
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
	[ServletPage()]
	public class ErrorPageHandler : XsltServletPage
	{
		public override string Address
		{
			get
			{
				return "/ErrorPageTest/ErrorPageHandler.xsp";
			}
		}

		public override string XslTransformName
		{
			get
			{
				return "/Resources/ErrorPageHandler.xslt";
			}
		}

		public ErrorPageHandler()
		{
		}
		
		public override XmlDocument getXML(WebRequest aRequest)
		{
			if(!(aRequest is ExceptionWebRequest))
				throw new Exception("this is an exception only page");
			
			XmlDocument xdoc = new XmlDocument();

			XmlElement elRoot = xdoc.CreateElement("page");
			xdoc.AppendChild(elRoot);

			XmlElement el;
			//XmlAttribute attr;
			XmlElement elExceptions = xdoc.CreateElement("exceptions");
			elRoot.AppendChild(elExceptions);

			ExceptionWebRequest eWebRequest = (ExceptionWebRequest)aRequest;
			Exception ex = eWebRequest.Exception;
			while(ex != null)
			{
				XmlElement elException = xdoc.CreateElement("exception");
				elExceptions.AppendChild(elException);

				el = xdoc.CreateElement("message");
				el.InnerText = ex.Message;
				elException.AppendChild(el);

				el = xdoc.CreateElement("short-type-name");
				el.InnerText = ex.GetType().Name;
				elException.AppendChild(el);

				el = xdoc.CreateElement("full-type-name");
				el.InnerText = ex.GetType().FullName;
				elException.AppendChild(el);

				el = xdoc.CreateElement("stack-trace");
				el.InnerText = ex.StackTrace;
				elException.AppendChild(el);

				ex = ex.InnerException;
			}

			return xdoc;
		}
	}
}
