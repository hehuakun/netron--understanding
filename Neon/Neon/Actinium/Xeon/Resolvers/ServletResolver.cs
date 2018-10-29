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
	/// Resolves the servlets, ie the compiled page which will answers .xsp requests.
	/// The servlets are loaded at runtime from given assemblies specified in the app.config.
	/// </summary>
	public class ServletResolver : IResourceResolver
	{
		
		Hashtable m_pages = new Hashtable();
		bool m_isCaseSensitive;

		public ServletResolver(bool isCaseSensitive)
		{
			m_isCaseSensitive = isCaseSensitive;
		}

		public Stream GetResourceAsStream(string sFileName)
		{
			return null;
		}

		public ServletResolver() : this(false)
		{
		}

		
		public void AddPage(IServletPage aPage)
		{
			string sName = aPage.Address;
			if(!m_isCaseSensitive)
				sName = sName.ToLower();
			m_pages[sName] = aPage;
		}

		public void AddAllPages(Assembly aAssembly)
		{
			foreach(Type tp in aAssembly.GetTypes())
			{
				if(tp.IsClass && !tp.IsAbstract && tp.IsPublic && tp.GetCustomAttributes(typeof(ServletPage), false).Length > 0)
				{
					try
					{
						object obj = tp.GetConstructor(new Type[]{}).Invoke(new object[]{});	
						IServletPage aPage = (IServletPage) obj;
						//some servlets might need initialization
						
						aPage.Initialize();
						//TODO://
						AddPage(aPage);
					}
					catch(Exception ex)
					{
						Util.WriteException(ex);
					}
				}
			}
		}

		IServletPage getPage(string sName)
		{
			return m_pages[(!m_isCaseSensitive ? sName.ToLower() : sName)] as IServletPage;
		}

		public bool Resolves(WebRequest aRequest)
		{
			return getPage(aRequest.File) != null;
		}

		//http://localhost:8080/DirectoryTree.xsp
		public void Answer(WebRequest aRequest)
		{
			IServletPage page = getPage(aRequest.File);
			if(page != null)
			{
			
				try
				{
					if(page.IsBuffered && !aRequest.Response.IsInMemoryStreamMode)
						aRequest.Response.SwitchToMemoryStream();
			
					page.Answer(aRequest);
				}
				catch(Exception ex)
				{
					if(page.IsBuffered)
					{
						try
						{
							string sNewPage = page.getExceptionPage(aRequest);
							if(sNewPage == null)
								throw new Exception("No error page defined");

							aRequest.Response.Reset();
							ExceptionWebRequest exRequest = new ExceptionWebRequest(sNewPage, aRequest, ex);
							aRequest.Server.InvokeRequest(exRequest);
						}
						catch(Exception innerEx)
						{
							try
							{
								aRequest.Response.Reset();
								aRequest.Response.SendContent("text/html");
								aRequest.Response.WriteLine("<html><body><pre>");
								Util.WriteExceptionToResponse(innerEx, aRequest.Response);
								aRequest.Response.WriteLine("</pre><br>Original exeception was:<pre>");
								Util.WriteExceptionToResponse(ex, aRequest.Response);
								aRequest.Response.WriteLine("</pre></body></html>");
							}
							catch(Exception innerEx2)
							{
								Util.WriteException(innerEx2);
							}
						}
					}
				}
			}
		}
	}
}