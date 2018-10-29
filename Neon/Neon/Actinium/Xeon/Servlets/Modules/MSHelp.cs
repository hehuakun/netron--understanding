using System;
using System.Configuration;
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
	public class XHelp : XsltServletPage

	{
	
		/// <summary>
		/// where the xml stuff is
		/// </summary>
		private string contentLocation;

		public XHelp()
		{
		}
		
		public override string Address
		{
			get
			{
				return "/MSHelp.xsp";
			}
		}

		public override string XslTransformName
		{
			get
			{
				return "/Resources/MSHelp.xslt";
			}
		}

		public override void Initialize()
		{

			WebServerSettings settings=(WebServerSettings) ConfigurationSettings.GetConfig("Xeon");
			contentLocation=settings.DynamicContent;
			base.Initialize ();
		}

		
		public override XmlDocument getXML(WebRequest aRequest)
		{
			Hashtable parms=aRequest.Parameters;
			string page="";
			if(parms.Count>0)
			{
				if(parms.ContainsKey("id"))
					page=parms["id"].ToString();

			}
			if(page=="") return null;
			XmlDocument xdoc = new XmlDocument();
			xdoc.Load(contentLocation + "\\"+ page + ".xml");
			return xdoc;
		}
//		/// <summary>
//		/// Takes the transfo from the assembly
//		/// </summary>
//		/// <param name="aRequest"></param>
//		/// <param name="aTransformation"></param>
//		/// <param name="aArguments"></param>
//		public override void getTransformation(WebRequest aRequest, out XslTransform aTransformation, out XsltArgumentList aArguments)
//		{
//			if(m_transform == null || DebugReload)
//			{
//				string sXslName = XslTransformName;
//				m_transform = new XslTransform();
//
//					
//				try
//				{
//					m_transform.Load(new XmlTextReader(sm));
//				}
//				finally
//				{
//					sm.Close();
//				}
//			}
//
//			aTransformation = m_transform;
//			aArguments = getArgumentList(aRequest);
//		}


		public override XsltArgumentList getArgumentList(WebRequest aRequest)
		{
			XsltArgumentList argList = base.getArgumentList(aRequest);
			argList.AddExtensionObject("urn:user-request", new XSLTRequestExtensionObject(aRequest));
			return argList;
		}
	}
}
