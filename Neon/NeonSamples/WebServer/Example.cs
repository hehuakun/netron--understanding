using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Threading;
using System.Reflection;
using System.Xml;
using System.Xml.Xsl;
using System.Configuration;



namespace  Netron.Xeon
{
	/// <summary>
	/// This is the startup/home page of the framework...what a satisfaction to write this one after another 100.000+ lines of code :-)
	/// </summary>
	[ServletPage()]
	public class Example : ServletPageBase
	{
		private string[] assLocations; //assembly location
		private string page;
		private string webroot;
		private string outputtext;
		public Example()
		{
			WebServerSettings webSettings=(WebServerSettings) ConfigurationSettings.GetConfig("Xeon");
			webroot = "http://localhost:" + webSettings.ServerPort + "/";
			assLocations = webSettings.Servlets;
		}
		void ShowOutputter()
		{
			//this.Root.RaiseLoadService(Netron.Neon.BaseServices.OutputService,"From the NAF guide:");this.Root.RaiseLoadService(Netron.Neon.BaseServices.OutputService,"From the NAF guide:");
		}

		/// <summary>
		/// The actual response to the browser
		/// </summary>
		/// <param name="aRequest"></param>
		public override void Answer(WebRequest aRequest)
		{
			string resp = string.Empty;;
			aRequest.Response.SendContent("text/html");
			//set to 'default.htm' if nothing was specified
			if(aRequest.Parameters["page"]==null)
				page = "default.htm";
			else
				page = aRequest.Parameters["page"].ToString() + ".htm";

			
			#region		the various form posts			

		
			//for the output panel
			if(aRequest.Parameters["OutputText"] !=null)
			{
				
				outputtext = Util.URLDecode(aRequest.Parameters["OutputText"].ToString());
				
				resp = outputtext;
				
			}		
			if(aRequest.Parameters["id"]!=null)
			{
				if(aRequest.Parameters["id"].ToString().ToLower()=="me")
					aRequest.Response.SendRedirect(webroot + "MyComputer/");
			}

			#endregion

			//return the template with the body replaced by the requested page
			resp =this.GetPage("Template.htm").Replace("$body",resp);
			
			//aRequest.Response.WriteLine("<font size='5' color='steelblue'>The Netron Application Framework version 1.0</font><p>Go to the <a href='http://netron.sourceforge.net/ewiki/netron.php?id=NAFSDK'>NAF SDK</a></p>");
		
			aRequest.Response.WriteLine(resp);
		}
		/// <summary>
		/// The address of this page
		/// </summary>
		public override string Address
		{
			get
			{
				return "/Example.xsp";
			}
		}

		/// <summary>
		/// In case of troubles this will be the page that is served.
		/// </summary>
		public override string ExceptionPage
		{
			get
			{
				return "/ErrorPageTest/ErrorPageHandler.xsp";
			}
		}
		
		public string GetPage(string sPageName)
		{
			string ret = string.Empty;
			
			Assembly assem;
			Stream stream;
			StreamReader sr;
			for(int k =0; k<assLocations.Length; k++)
			{

				try
				{
					assem = Assembly.LoadFrom(assLocations[k]);			
					stream = assem.GetManifestResourceStream("WebServer." + sPageName);
					sr = new StreamReader(stream,System.Text.Encoding.ASCII);
					ret = sr.ReadToEnd();
					sr.Close();
					stream.Close();
					if(ret !=null) return ret;
				}
				catch(Exception exc)
				{
					Trace.WriteLine(exc.Message,"Warning");
					continue;
				}
			}
			return string.Empty;
		}
			
		}
	}
