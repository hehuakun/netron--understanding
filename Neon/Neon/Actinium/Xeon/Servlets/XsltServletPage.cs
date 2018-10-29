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
	/// <summary>
	/// Abstract base class for XSL server pages
	/// </summary>
	public abstract class XsltServletPage : XsltServletPageBase
	{
		protected XslTransform m_transform = null;
		public override void getTransformation(WebRequest aRequest, out XslTransform aTransformation, out XsltArgumentList aArguments)
		{
			if(m_transform == null || DebugReload)
			{
				string sXslName = XslTransformName;
				m_transform = new XslTransform();

				Stream sm = aRequest.Server.GetResourceAsStream(sXslName);
				try
				{
					m_transform.Load(new XmlTextReader(sm));
				}
				finally
				{
					sm.Close();
				}
			}

			aTransformation = m_transform;
			aArguments = getArgumentList(aRequest);
		}

		public virtual XsltArgumentList getArgumentList(WebRequest aRequest)
		{
			return new XsltArgumentList();
		}
		
		public abstract string XslTransformName
		{
			get;
		}

		public static bool DebugReload = false;
	}

}
