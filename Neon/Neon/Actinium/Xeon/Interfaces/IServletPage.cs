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
	/// Servlets interface
	/// </summary>
	public interface IServletPage
	{
		/// <summary>
		/// Gets the address the servlet will answer to
		/// </summary>
		string Address
		{
			get;
		}
		bool IsBuffered
		{
			get;
		}
		/// <summary>
		/// Answers the actual request
		/// </summary>
		/// <param name="aRequest"></param>
		void Answer(WebRequest aRequest);
		/// <summary>
		/// Gets the exception page associated to this servlet
		/// </summary>
		/// <param name="aRequest"></param>
		/// <returns></returns>
		string getExceptionPage(WebRequest aRequest);
		/// <summary>
		/// Initialization of the servlet
		/// </summary>
		void Initialize();

		
	}

}
