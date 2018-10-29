using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Globalization;
using Microsoft.Win32;

namespace  Netron.Xeon
{
	/// <summary>
	/// This is the web request class. It is used to acces the paramters of
	/// the query;
	/// </summary>
	public class WebRequest
	{

		#region Fields
		protected Hashtable requestParams = new Hashtable();
		protected string requestedFileName = "";
		protected RequestMethod requestMethod;
		protected Stream stream;
		protected WebResponse webResponse;
		protected WebServer webServer;
		#endregion

		#region Constructor
		/// <summary>
		/// Ctor
		/// </summary>
		protected WebRequest()
		{
		}

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="aRequest"></param>
		protected WebRequest(WebRequest aRequest)
		{
			requestParams = aRequest.requestParams;
			requestedFileName = aRequest.requestedFileName;
			requestMethod = aRequest.requestMethod;
			stream = aRequest.stream;
			webResponse = aRequest.webResponse;
			webServer = aRequest.webServer;
		}
		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="smRequest"></param>
		/// <param name="aServer"></param>
		internal WebRequest(Stream smRequest, WebServer aServer)
		{
			webServer = aServer;
			TextReader txt = new StreamReader(smRequest);
			string sLine = txt.ReadLine();
			if (sLine.StartsWith("GET"))
			{
				sLine = sLine.Substring(3).Trim();
				int nIndex = sLine.IndexOf('?');
				string sFile, sRest;
				if(nIndex == -1)
				{
					sFile = sLine.Split(' ')[0];
					sFile = Unescape(sFile);
					sRest = "";
				}
				else
				{
					sFile = sLine.Substring(0, nIndex);
					sFile = Unescape(sFile);
					sRest = sLine.Substring(nIndex + 1).Split(' ')[0];
				}
				requestMethod = RequestMethod.Get;
				requestedFileName = sFile;
				stream = smRequest;
				ParseParams(sRest);

			}
			else 
			{
				sLine = sLine.Substring(4).Trim();
				string sFile = sLine.Split(' ')[0];
				sFile = Unescape(sFile);
				
				for(;;)
				{
					string nextLine = txt.ReadLine();
					if (nextLine == string.Empty)
						break;
				}
				string rqLine = txt.ReadLine();
				requestMethod = RequestMethod.Post;
				requestedFileName = sFile;
				stream = smRequest;
				ParseParams(rqLine);
			}



		}
		#endregion
		
		#region Properties
		/// <summary>
		/// The server object that created the request
		/// </summary>
		public WebServer Server
		{
			get
			{
				return webServer;
			}
		}

		/// <summary>
		/// Gets or sets the file requested.
		/// The set method allows for a redirect, e.g. when requesting the root (=/) of the server.
		/// </summary>
		public string File 
		{
			get
			{
				return requestedFileName;
			}
			set{
				requestedFileName = value;
			}
		}

		/// <summary>
		/// The request method
		/// </summary>
		public RequestMethod Method 
		{
			get
			{
				return requestMethod;
			}
		}
		/// <summary>
		/// Returns the response object. Use it to answer the client
		/// </summary>
		public WebResponse Response
		{
			get
			{
				if(webResponse == null)
					webResponse = new WebResponse(stream);
				return webResponse;
			}
		}
		public Hashtable Parameters
		{
			get{return requestParams;}
		}

		#endregion

		#region Methods
	

		/// <summary>
		/// Reformats the url escape codes
		/// 
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		string Unescape(string s)
		{
			StringBuilder bld = new StringBuilder();
			for(int i = 0; i < s.Length; i++)
			{
				if(s[i] == '%')
				{
					bld.Append((char)Int32.Parse(s.Substring(i + 1, 2), NumberStyles.HexNumber));
					i += 2;
				}
				else
					bld.Append(s[i]);
			}
			return bld.ToString();
		}

		/// <summary>
		/// Parses the parameters of the request
		/// </summary>
		/// <param name="sRequest">the incoming request</param>
		void ParseParams(string sRequest)
		{
			string[] arrParams = sRequest.Split('&');
			foreach(string sParam in arrParams)
			{
				string[] arrCurParam = sParam.Split('=');
				if(arrCurParam.Length > 0)
				{
					string sKey = Unescape(arrCurParam[0]);
					string sValue = arrCurParam.Length > 1 ? Unescape(arrCurParam[1]) : "";
					requestParams[sKey] = sValue;
				}

			}
		}

		#endregion
	
		#region Indexers
		/// <summary>
		/// The request parameters
		/// </summary>
		public string this[string aIndex]
		{
			get
			{
				return (string)requestParams[aIndex];
			}

		}

		#endregion

		
	}

}
