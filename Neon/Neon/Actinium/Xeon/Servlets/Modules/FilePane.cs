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
	public class FilePane : XsltServletPage
	{
		public override string Address
		{
			get
			{
				return "/FilePane.xsp";
			}
		}

		public override string XslTransformName
		{
			get
			{
				return "/Resources/FilePane.xslt";
			}
		}

		public FilePane()
		{
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
//				Stream sm = Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.Neon.Actinium.Xeon.Resources.FilePane.xslt");
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
		bool isPicture(string sExt)
		{
			string sExtensions = ".gif.jpg.png.bmp.ico.";
			return sExtensions.IndexOf(sExt.ToLower() + ".") != -1 && sExt != "";
		}
		public override XmlDocument getXML(WebRequest aRequest)
		{
			string sPath = aRequest["path"];
			DirectoryInfo dinf = new DirectoryInfo(sPath);
			if(!dinf.Exists)
				throw new Exception("Error reading directory contents");

			XmlDocument xdoc = new XmlDocument();
			XmlElement elRoot = xdoc.CreateElement("page");
			xdoc.AppendChild(elRoot);
			XmlElement elObjects = xdoc.CreateElement("objects");
			elRoot.AppendChild(elObjects);
			XmlElement el;
			XmlAttribute attr;
			
			FileInfo[] arrInfo = dinf.GetFiles("*.*");
			foreach(FileInfo finf in arrInfo)
			{
				if(isPicture(finf.Extension))
				{
					el = xdoc.CreateElement("picture");
					attr = xdoc.CreateAttribute("path");
					attr.Value = finf.FullName;
					el.Attributes.Append(attr);
					elObjects.AppendChild(el);
				}
			}

			return xdoc;
		}

		public override XsltArgumentList getArgumentList(WebRequest aRequest)
		{
			XsltArgumentList argList = base.getArgumentList(aRequest);
			argList.AddExtensionObject("urn:user-request", new XSLTRequestExtensionObject(aRequest));
			return argList;
		}
	}
}
