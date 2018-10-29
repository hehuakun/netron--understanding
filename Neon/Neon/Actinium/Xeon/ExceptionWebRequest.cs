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
	/// This class packages a webrequest which can be used to show an exception, 
	/// it's a virutal request since it never can come from a client.
	/// </summary>
	public class ExceptionWebRequest : WebRequest
	{
		#region Fields
		Exception m_exception;
		string m_originalPage;
		#endregion
		
		#region Constructor
		public ExceptionWebRequest(string sNewFileName, WebRequest aRequest, Exception aException) : base(aRequest)
		{
			m_exception = aException;
			m_originalPage = aRequest.File;
			requestedFileName = sNewFileName;
		}
		#endregion
		
		#region Properties
		/// <summary>
		/// Gets the exception that is encapsulated in this virtual request
		/// </summary>
		public Exception Exception
		{
			get
			{
				return m_exception;
			}
		}

		/// <summary>
		/// Gets the original page request that has thrown an exception
		/// </summary>
		public string OriginalPage
		{
			get
			{
				return m_originalPage;
			}
		}
		#endregion
	}
}
