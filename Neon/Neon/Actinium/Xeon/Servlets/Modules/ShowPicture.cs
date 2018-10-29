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
	public class ShowPicure : XsltServletPage
	{
		public ShowPicure()
		{
		}
		
		public override string Address
		{
			get
			{
				return "/ShowPicture.xsp";
			}
		}

		public override string XslTransformName
		{
			get
			{
				return "/Resources/ShowPicture.xslt";
			}
		}

		public override XmlDocument getXML(WebRequest aRequest)
		{
			XmlDocument xdoc = new XmlDocument();
			XmlElement elRoot = xdoc.CreateElement("page");
			xdoc.AppendChild(elRoot);
			return xdoc;
		}
		/// <summary>
		/// Takes the transfo from the assembly
		/// </summary>
		/// <param name="aRequest"></param>
		/// <param name="aTransformation"></param>
		/// <param name="aArguments"></param>
		public override void getTransformation(WebRequest aRequest, out XslTransform aTransformation, out XsltArgumentList aArguments)
		{
			if(m_transform == null || DebugReload)
			{
				string sXslName = XslTransformName;
				m_transform = new XslTransform();

				Stream sm = Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.Neon.Actinium.Xeon.Resources.ShowPicture.xslt");
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

		public override XsltArgumentList getArgumentList(WebRequest aRequest)
		{
			XsltArgumentList argList = base.getArgumentList(aRequest);
			argList.AddExtensionObject("urn:user-request", new XSLTRequestExtensionObject(aRequest));
			return argList;
		}
	}
}
