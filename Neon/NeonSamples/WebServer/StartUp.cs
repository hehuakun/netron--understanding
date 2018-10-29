using System;
using System.Reflection;
using Netron.Xeon;
using System.Configuration;
using System.Windows.Forms;
namespace WebServer
{
	/// <summary>
	/// Summary description for StartUp.
	/// </summary>
	public class StartUp
	{
		static bool started = false;
		static Netron.Xeon.WebServer srv;
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			if(StartServer())
			{
				Application.Run(new Form1());
				srv.Stop();
			}
			else
				MessageBox.Show("The HTML server could not be started.");
		}


		static bool StartServer()
		{
			if(started) return false;
			try
			{
				WebServerSettings webSettings=(WebServerSettings) ConfigurationSettings.GetConfig("Xeon");
				srv = new Netron.Xeon.WebServer(webSettings.ServerPort); 
				

#if DEBUG

				//add static pages in flat format, you can use embedded resources as well, see below
				//			string sPath = Path.GetDirectoryName(Application.ExecutablePath);
				//			sPath = Path.Combine(sPath, @"..\..\view");
				//			sPath = Path.GetFullPath(sPath);
			
				ContentLocationResolver clr = new ContentLocationResolver(webSettings.StaticContent, "");
				srv.AddResolver(clr, 10, 20);

				XsltServletPage.DebugReload = webSettings.XmlDebugReload;
				//sPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"..\xmldeb"));
				XsltServletPage.XMLDumpPath = webSettings.XmlDumpPath;
#else
			EmbeddedLocationResolver elr = new EmbeddedLocationResolver(Assembly.GetExecutingAssembly(), "ImageViewerExample.view", "view");
			srv.AddResolver(elr, 10, 20);
#endif

				EmbeddedLocationResolver elr = new EmbeddedLocationResolver(Assembly.GetAssembly(typeof(EmbeddedLocationResolver)), "Netron.Neon.Actinium.Xeon.Resources", "Resources");
				srv.AddResolver(elr, 10, 20);

				//add dynamic content
				ServletResolver sr = new ServletResolver();
				
				//could be a separate assembly, here the resolvers are in the control library
				string[] assems = webSettings.Servlets;
				Assembly ass;
				for(int k =0; k<assems.Length; k++)
				{
					ass = Assembly.LoadFile(assems[k]);
					sr.AddAllPages(ass);
				}
				srv.AddResolver(sr, 2, 10); //10,20

				MyComputerResolver res = new MyComputerResolver();
				srv.AddResolver(res, 10, 20);

				srv.Start();
				started = true;
				return true;
			}
			catch(Exception exc)
			{
				
				System.Diagnostics.Trace.WriteLine("The exception causing the boot process to stop was:\n" + exc.Message,"Critical");
				
				return false;
			}
		}
	}
}
