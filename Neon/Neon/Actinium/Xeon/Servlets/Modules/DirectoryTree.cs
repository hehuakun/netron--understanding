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
	public class DirectoryListing : XsltServletPage
	{
		public DirectoryListing()
		{
		}
		public override string Address
		{
			get
			{
				return "/DirectoryTree.xsp";
			}
		}

		public override string XslTransformName
		{
			get
			{
				return "/Resources/DirectoryTree.xslt";
			}
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
//				Stream sm = Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.Neon.Actinium.Xeon.Resources.DirectoryTree.xslt");
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

		public override XmlDocument getXML(WebRequest aRequest)
		{
			XmlDocument xdoc = new XmlDocument();
			string sCurrentNode = aRequest["currentNode"];

			XmlElement elRoot = xdoc.CreateElement("page");
			xdoc.AppendChild(elRoot);

			XmlElement elObjects = xdoc.CreateElement("objects");
			elRoot.AppendChild(elObjects);

			XmlElement el;
			
			if(sCurrentNode == null)
			{
				string[] drives = Directory.GetLogicalDrives();

				foreach(string drive in drives)
				{
					el = xdoc.CreateElement("directory");
					elObjects.AppendChild(el);

					XmlAttribute attr = xdoc.CreateAttribute("name");
					string sVal = drive.EndsWith("\\") ? drive.Substring(0, drive.Length - 1) : drive;
					attr.Value = sVal;
					el.Attributes.Append(attr);

					attr = xdoc.CreateAttribute("hasChildren");
					try
					{
						DirectoryInfo dinf = new DirectoryInfo(sVal + "\\");
						attr.Value = (dinf.GetDirectories().Length > 0) ? "true" : "false";
					}
					catch(Exception)
					{
						attr.Value = "false";
					}
					el.Attributes.Append(attr);
				}
			}
			else
			{
				if(!sCurrentNode.EndsWith("\\"))
					sCurrentNode += "\\";
				DirectoryInfo curNodeInf = new DirectoryInfo(sCurrentNode);
				DirectoryInfo[] subDirs = curNodeInf.GetDirectories("*.*");
				foreach(DirectoryInfo dinf in subDirs)
				{
					el = xdoc.CreateElement("directory");
					elObjects.AppendChild(el);

					string dirName = dinf.Name;
					XmlAttribute attr = xdoc.CreateAttribute("name");
					string sVal = dirName.EndsWith("\\") ? dirName.Substring(0, dirName.Length - 1) : dirName;
					attr.Value = sVal;
					el.Attributes.Append(attr);

					attr = xdoc.CreateAttribute("hasChildren");
					try
					{
						attr.Value = (dinf.GetDirectories().Length > 0) ? "true" : "false";
					}
					catch(Exception)
					{
						attr.Value = "false";
					}
					el.Attributes.Append(attr);
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
