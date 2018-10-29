using System;

namespace  Netron.Xeon
{

	/// <summary>
	/// Collects the configuration of the Xeon settings
	/// </summary>
	public class WebServerSettings
	{
		#region Fields
		private bool startUp = false;
		private int serverPort=8080;
		private string staticContent;
		private string dynamicContent;
		private string xmlDump;
		private bool xmlDebugReload;
		private string[] servlets;
		#endregion

		#region Properties

		public bool StartUp
		{
			get{return startUp;}
			set{startUp = value;}
		}

		public string[] Servlets
		{
			get{return servlets;}
			set{servlets=value;}
		}
		public bool XmlDebugReload
		{
			get{return xmlDebugReload;}
			set{xmlDebugReload=value;}		
		}
		public string XmlDumpPath
		{
			get{return xmlDump;}
			set{xmlDump=value;}
		}
		/// <summary>
		/// Gets or sets the webserver port number
		/// </summary>
		public int ServerPort
		{
			get{return serverPort;}
			set{serverPort=value;}
		}
		/// <summary>
		/// Gets or sets the location of the static content
		/// </summary>
		public string StaticContent
		{
			get{return staticContent;}
			set{staticContent=value;}
		}

		/// <summary>
		/// Gets or sets the location of the assembly where the dynamic pages reside
		/// </summary>
		public string DynamicContent
		{
			get{return dynamicContent;}
			set{dynamicContent=value;}
		}

		public override string ToString()
		{
			return "Port: " + this.serverPort + "\n" +
				"static content: "  + this.staticContent + "\n" +
				"dynamic content: " + this.dynamicContent;
		}
	

		
		#endregion
	}
}
