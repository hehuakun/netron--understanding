using System;
using System.Xml;
using System.Xml.Xsl;


namespace  Netron.Xeon
{
	public class XSLTRequestExtensionObject
	{
		WebRequest m_request;

		public XSLTRequestExtensionObject(WebRequest aRequest)
		{
			m_request = aRequest;
		}

		public string getParameter(string sName)
		{
			return m_request[sName] == null ? "" : m_request[sName];
		}
		public string URLEncode(string sURL)
		{
			return Util.EscapeURL(sURL);
		}
	}
}