using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Threading;
using System.Reflection;



namespace  Netron.Xeon
{


	
	/// <summary>
	/// This is the web server class. See the remarks to undestand how it works
	/// </summary>
	/// <remarks>
	/// The web server is a true web server but with very limited functionality.
	/// It has a collection of resolvers. The resovler is like a pluggable file system.
	/// When the web server receives a request from the client it start walking this collection
	/// until it finds a suitable resolver - one that claims it can handle the request.
	/// The resolvers are tested using their <code>Resolves</code> function. Once a suitable
	/// resolver is found it's Answer function is called. This function should send 
	/// the content of the Requested resource back to the client. If no suitable resolver
	/// is found a 404 http error is sent.
	/// 
	/// The Answer method of the resolver can be called multiple times simultaniosly - 
	/// running in different threads. For more information see the ResolverThreadPool class
	/// </remarks>
	public class WebServer
	{
		
		#region Fields

		/// <summary>
		/// the thread onto which the server runs
		/// </summary>
		Thread serverThread = null;
		/// <summary>
		/// The port on which the server operates
		/// </summary>
		int serverPort;

		/// <summary>
		/// The collection of resolvers
		/// </summary>
		/// <remarks>
		/// This collection actualy contains objects of type ResolverThreadPool,
		/// the resolvers are pooled
		/// </remarks>
		ArrayList resolvers = new ArrayList();
		
		/// <summary>
		/// standard .Net TCP listener
		/// </summary>
		TcpListener tcpListener;

		#endregion

		#region Constructor
		/// <summary>
		/// Creates the web server
		/// </summary>
		/// <param name="aPort">The port on which the server listens</param>
		public WebServer(int serverPort)
		{
			this.serverPort = serverPort;
		}
		#endregion

		#region Methods
		

		/// <summary>
		/// Adds a resolver to the resolvers collection
		/// </summary>
		/// <param name="aLocation">the resolver to be added</param>
		/// <param name="aMinThreads">The number of pooled threads</param>
		/// <param name="aMaxThreads">The maximum number of threads. -1 for unlimited</param>
		/// <remarks>For more information see the ResolverThreadPool class</remarks>
		public void AddResolver(IResourceResolver aLocation, int aMinThreads, int aMaxThreads)
		{
			resolvers.Add(new ResolverThreadPool(aMinThreads, aMaxThreads, aLocation, this));
		}

		/// <summary>
		/// Internal routine. Finds the suitable resolver an runs it's answer routine
		/// in a different thread. Thread control is done by the ResolverThreadPool class
		/// </summary>
		/// <param name="webRequest"></param>
		/// <param name="aClient"></param>
		/// <param name="aStream"></param>
		void Answer(WebRequest webRequest, TcpClient aClient, NetworkStream aStream)
		{
			bool found = false;
			if(webRequest.File=="/") //the root
			{
					webRequest.File ="/Resources/Xeon.jpg";
			}				
			
			foreach(ResolverThreadPool resolverPool in resolvers)
			{
				if(resolverPool.Resolver.Resolves(webRequest))
				{
					resolverPool.Answer(webRequest, aClient, aStream);
					found = true;
					break;
				}
			}

			if(!found)
			{			
				webRequest.Response.SendFileNotFound();
				webRequest.Response.Done();
				aStream.Close();
				aClient.Close();
			}
		}
		
		/// <summary>
		/// The main loop of the server; the runnable on the thread(pool).
		/// </summary>
		void Run()
		{
			IPAddress ipAddress = Dns.Resolve("localhost").AddressList[0];

			tcpListener = new TcpListener(ipAddress,serverPort);
			tcpListener.Start();
			
			try
			{
				for (;;)
				{
					TcpClient tcpClient = null;
					try
					{
						tcpClient = tcpListener.AcceptTcpClient();
						tcpClient.ReceiveTimeout = 1000;
						tcpClient.SendTimeout = 1000;

						//what's the stuff coming in?
						NetworkStream networkStream = tcpClient.GetStream();

						//analyze the request
						WebRequest webRequest = new WebRequest(networkStream, this);

						//if all was allright then answer the request
						Answer(webRequest, tcpClient, networkStream);
					}
					catch(Exception e)
					{
						Util.WriteException(e);
					}
				}
			}
			finally 
			{
				tcpListener.Stop();
			}
		}

	

		/// <summary>
		/// This function starts the web server on the separate thread from the main thread
		/// </summary>
		public void Start()
		{
			if(serverThread != null)
				throw new Exception("Server already started");

			serverThread = new Thread(new ThreadStart(Run));
			serverThread.Start();
		}

		/// <summary>
		/// This function stops the web server. Be sure to call or your program wont exit
		/// </summary>
		public void Stop()
		{
			if(serverThread == null)
				throw new Exception("Server already stoped");

			try
			{
				tcpListener.Stop();
				serverThread.Abort();
				serverThread.Join();
				serverThread = null;

				foreach(ResolverThreadPool pool in resolvers)
					pool.ShutDown();
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.Message);
			}
		}

		/// <summary>
		/// Gets a static resource as a stream. To do so it calls the <code>GetResourceAsStream</code>
		/// function of all installed resolvers. See <code>IResourceResolver.GetResourceAsStream</code>
		/// </summary>
		/// <param name="sName">the name of the resource</param>
		/// <returns>a stream for reading from the resource</returns>
		public Stream GetResourceAsStream(string sName)
		{
			foreach(ResolverThreadPool pool in resolvers)
			{
				IResourceResolver res = pool.Resolver;
				Stream strm = res.GetResourceAsStream(sName);
				if(strm != null)
					return strm;
			}

			return null;
		}

		/// <summary>
		/// Internal routine. Used for exception page handling
		/// </summary>
		/// <param name="webRequest">the request object passed to the page</param>
		internal void InvokeRequest(WebRequest webRequest)
		{
			foreach(ResolverThreadPool pool in resolvers)
			{
				IResourceResolver res = pool.Resolver;
				if(res.Resolves(webRequest))
				{
					res.Answer(webRequest);
					break;
				}
			}
		}
		#endregion

	}

}