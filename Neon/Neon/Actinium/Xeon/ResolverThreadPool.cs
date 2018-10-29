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
	/// A thread pool class
	/// </summary>
	/// <remarks>
	/// This class is used to manage the simultanous execution of the Answer routine
	/// of the ResourceResolcer class.
	/// When adding a resolver to the WebServer the program specifies two numbers :
	/// the number of pooled answer threads and the number of maximum answer threads.
	/// When a request is made, the server passes it on to this class's Answer routine
	/// The Resolvers anwer routine is then executed in a different thread from the server's
	/// one. 
	/// There are 2 types of threads - pooled and free.
	/// Pooled threads are threads that are in sleeping mode. When <code>answer</code>
	/// it first sees if there is currently a sleeping thread. If yes it wakes it up
	/// and passes the request object to it. The thread than invokes the answer routine
	/// of the Resolver and after that goes to sleep again. If all pooled threads are
	/// currently working a new thread is created - a free thread. It proccesses the
	/// request and than terminates. If the number of free threads + the number of
	/// pooled threads is >= the number of maximum threads, the request is queued and
	/// is processed the moment a thread becomes available
	/// </remarks>
	class ResolverThreadPool
	{
		static void SleepUntilInterrupt()
		{
			try
			{
				Thread.Sleep(-1);
			}
			catch(ThreadInterruptedException)
			{
			}
		}
		class ResolverThread
		{
			ResolverThreadPool m_pool;
			bool m_executeOneTime;
			public TcpClient m_client;
			public NetworkStream m_stream;
			public WebRequest m_request;
			public ResolverThread(ResolverThreadPool aPool, bool aExecuteOneTime)
			{
				m_pool = aPool;
				m_executeOneTime = aExecuteOneTime;
			}

			public void Run()
			{
				if(!m_executeOneTime)
					SleepUntilInterrupt();

				for(;;)
				{
					try
					{
						m_pool.resolver.Answer(m_request);

						m_request.Response.Done();
						m_stream.Close();
					}
					catch(Exception ex)
					{
						Util.WriteException(ex);
					}


					try
					{
						m_client.Close();
					}
					catch(Exception e)
					{
						Util.WriteException(e);
					}

					if(m_executeOneTime)
					{
						m_pool.StartStopFreeThread(this, null, false);
						break;
					}

					m_pool.SleepWakeup(this, true);
				}
			}
		}

		int m_minThreads, m_maxThreads, m_freeThreads;
		IResourceResolver resolver;
		Hashtable m_runningThreads = Hashtable.Synchronized(new Hashtable());
		Hashtable m_waitingThreads = Hashtable.Synchronized(new Hashtable());
		Queue m_answerQueue = new Queue();
		Thread m_answerPoolThread;
		WebServer m_server;
		Hashtable m_runningFreeThreads = Hashtable.Synchronized(new Hashtable());

		public ResolverThreadPool(int aMinThreads, int aMaxThreads, IResourceResolver aResolver, WebServer aServer)
		{
			m_minThreads = aMinThreads;
			m_maxThreads = aMaxThreads;
			resolver = aResolver;
			m_server = aServer;
			SetUp();
		}

		void WaitForSleep(Thread th)
		{
			for(;;)
			{
				if(th.ThreadState != System.Threading.ThreadState.WaitSleepJoin)
					try
					{
						Thread.Sleep(10);
					}
					catch(ThreadInterruptedException)
					{
					}
				else
					break;
			}
		}

		void SetUp()
		{
			for(int i = 0; i < m_minThreads; i++)
			{
				ResolverThread resThread = new ResolverThread(this, false);
				Thread th = new Thread(new ThreadStart(resThread.Run));
				th.Start();
				WaitForSleep(th);

				m_waitingThreads[resThread] = th;
			}

			m_answerPoolThread = new Thread(new ThreadStart(AnswerPoolThread));
			m_answerPoolThread.Start();
			WaitForSleep(m_answerPoolThread);
		}

		void SleepWakeup(ResolverThread aThread, bool aSleep)
		{
			lock(this)
			{
				if(aSleep)
				{
					m_waitingThreads[aThread] = m_runningThreads[aThread];
					m_runningThreads.Remove(aThread);
				}
				else
				{
					m_runningThreads[aThread] = m_waitingThreads[aThread];
					m_waitingThreads.Remove(aThread);
				}
			}

			if(aSleep)
			{
				m_answerPoolThread.Interrupt();
				SleepUntilInterrupt();
			}
			else
				((Thread)m_runningThreads[aThread]).Interrupt();
		}

		void StartStopFreeThread(ResolverThread aResThread, Thread aThread, bool aStart)
		{
			lock(this)
			{
				if(aStart)
				{
					m_freeThreads ++;
					m_runningFreeThreads[aResThread] = aThread;
				}
				else
				{
					m_freeThreads --;
					m_runningFreeThreads.Remove(aResThread);
				}

				m_answerPoolThread.Interrupt();
			}
		}

		void AnswerPoolThread()
		{
			SleepUntilInterrupt();
			for(;;)
			{
				if(m_answerQueue.Count == 0)
				{
					SleepUntilInterrupt();
					continue;
				}

				object[] arrRequest = (object[])m_answerQueue.Peek();
				if(m_waitingThreads.Count > 0)
				{
					IEnumerator en = m_waitingThreads.Keys.GetEnumerator();
					en.MoveNext();
					ResolverThread resThread = (ResolverThread)en.Current;

					resThread.m_request = (WebRequest)arrRequest[0];
					resThread.m_client = (TcpClient)arrRequest[1];
					resThread.m_stream = (NetworkStream)arrRequest[2];

					Thread th = (Thread)m_waitingThreads[resThread];

					WaitForSleep(th);

					SleepWakeup(resThread, false);
					m_answerQueue.Dequeue();
				}
				else if(m_freeThreads + m_minThreads < m_maxThreads || m_maxThreads == -1)
				{
					ResolverThread resThread = new ResolverThread(this, true);
					
					resThread.m_request = (WebRequest)arrRequest[0];
					resThread.m_client = (TcpClient)arrRequest[1];
					resThread.m_stream = (NetworkStream)arrRequest[2];

					Thread th = new Thread(new ThreadStart(resThread.Run)); 

					StartStopFreeThread(resThread, th, true);

					th.Start();
					m_answerQueue.Dequeue();
				}
				else
					SleepUntilInterrupt();
			}
		}

		public void Answer(WebRequest aRequest, TcpClient aClient, NetworkStream aStream)
		{
			m_answerQueue.Enqueue(new object[] {aRequest, aClient, aStream});
			m_answerPoolThread.Interrupt();
		}

		/// <summary>
		/// Gets the resolver this pool is serving
		/// </summary>
		public IResourceResolver Resolver
		{
			get
			{
				return resolver;
			}
		}

		public void ShutDown()
		{
			m_answerPoolThread.Abort();
			m_answerPoolThread.Join();
			foreach(Thread th in m_runningFreeThreads.Values)
			{
				th.Abort();
				th.Join();
			}

			foreach(Thread th in m_runningThreads.Values)
			{
				th.Abort();
				th.Join();
			}

			foreach(Thread th in m_waitingThreads.Values)
			{
				th.Abort();
				th.Join();
			}
		}
	}

}
