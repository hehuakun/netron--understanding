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
	/// Abstract base class for servlets
	/// </summary>
	public abstract class ServletPageBase : IServletPage
	{

	
		/// <summary>
		/// Gets the address the servlet will answer to
		/// </summary>
		public abstract string Address
		{
			get;
		}
		public virtual bool IsBuffered
		{
			get
			{
				return true;
			}
		}
		/// <summary>
		/// Initialization of the servlet
		/// </summary>
		public virtual void Initialize()
		{
			return;
		}
		/// <summary>
		/// Answers the actual request
		/// </summary>
		/// <param name="aRequest"></param>
		public abstract void Answer(WebRequest aRequest);

		public virtual string ExceptionPage
		{
			get
			{
				return null;
			}
		}

		/// <summary>
		/// Gets the exception page associated to this servlet
		/// </summary>
		/// <param name="aRequest"></param>
		/// <returns></returns>
		public virtual string getExceptionPage(WebRequest aRequest)
		{
			return ExceptionPage;
		}
	}

}
