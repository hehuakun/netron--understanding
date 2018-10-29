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
	public enum RequestMethod
	{
		Get,
		Post
	}


	/// <summary>
	/// This is the response class. Used to answer the client
	/// </summary>
	public class WebResponse
	{
		Stream m_stream;
		Stream m_originalStream = null;
		StreamWriter m_writer;
		Hashtable m_header = new Hashtable();
		int m_httpCode = -1;
		string m_httpMessage = "";

		enum ResponseStatus
		{
			Initial,
			HeaderSent,
			Done
		};
		ResponseStatus m_status = ResponseStatus.Initial;

		internal WebResponse(Stream aStream)
		{
			m_stream = aStream;
			m_writer = new StreamWriter(aStream);
		}

		void CheckHeaderSent()
		{
			if(m_status != ResponseStatus.Initial)
				throw new Exception("A header has already been sent");
		}
		
		/// <summary>
		/// The output stream. It is preferable not to use it directly
		/// </summary>
		public Stream OutStream
		{
			get
			{
				return m_stream;
			}
		}
		
		/// <summary>
		/// A hash table containing the Response header parameters
		/// </summary>
		/// <example>
		/// Key: 'content-location'
		/// Value: 'http://localhost:8080/view/stf.xsp'
		/// </example>
		public Hashtable Header
		{
			get
			{
				return m_header;
			}
		}

		/// <summary>
		/// The http status code of the reaponse
		/// </summary>
		public int HTTPCode
		{
			get
			{
				return m_httpCode;
			}
			set
			{
				CheckHeaderSent();
				m_httpCode = value;
			}
		}

		/// <summary>
		/// The HTTP status message of the response
		/// </summary>
		public string HTTPMessage
		{
			get
			{
				return m_httpMessage;
			}
			set
			{
				CheckHeaderSent();
				m_httpMessage = value;
			}
		}
		
		/// <summary>
		/// Used to clear the header + Code + Message 
		/// </summary>
		public void ClearHeader()
		{
			CheckHeaderSent();
			HTTPCode = -1;
			HTTPMessage = "";
			Header.Clear();
		}

		/// <summary>
		/// Writes the header to the output stream
		/// </summary>
		public void SendHeader()
		{
			CheckHeaderSent();
			m_writer.Write("HTTP/1.0 " + HTTPCode.ToString("000") + " " + HTTPMessage + "\r\n");
			foreach(DictionaryEntry pair in Header)
				m_writer.Write(pair.Key + ": " + pair.Value + "\r\n");
			m_writer.Write("\r\n");
			m_writer.Flush();
			m_status = ResponseStatus.HeaderSent;
		}

		/// <summary>
		/// Fills in a redirect header
		/// </summary>
		/// <param name="aURL">the new URL</param>
		public void FillRedirectHeader(string aURL)
		{
			CheckHeaderSent();
			HTTPCode = 307;
			HTTPMessage = "OK";
			Header["Content-Location"] = Util.EscapeURL(aURL);
		}
		/// <summary>
		/// Fills in and send a redirect header
		/// </summary>
		/// <param name="aURL">the new URL</param>
		public void SendRedirect(string aURL)
		{
			FillRedirectHeader(aURL);
			SendHeader();
		}

		/// <summary>
		/// Sends a file not found error
		/// </summary>
		public void SendFileNotFound()
		{
			this.SwitchToMemoryStream();			
			this.SendContent("text/html");
			WriteResource("PageNotFound.htm");
//			CheckHeaderSent();
//			HTTPCode = 404;
//			HTTPMessage = "Not Found";
//			
//			
//			SendHeader();
		}

		/// <summary>
		/// Fills a content header
		/// </summary>
		/// <param name="sEncoding">the MIME encoding of the content</param>
		public void FillContentHeader(string sEncoding)
		{
			CheckHeaderSent();
			HTTPCode = 200;
			HTTPMessage = "OK";

			Header["Content-type"] = sEncoding;
			Header["Date"] = Util.getResponseDate(DateTime.UtcNow);
		}
		/// <summary>
		/// Fills and sends a content header
		/// </summary>
		/// <param name="sEncoding"></param>
		public void SendContent(string sEncoding)
		{
			FillContentHeader(sEncoding);
			SendHeader();
		}

		/// <summary>
		/// Writes a string to the output stream
		/// </summary>
		/// <param name="sContent">the string to be written</param>
		public void Write(string sContent)
		{
			if(m_status != ResponseStatus.HeaderSent || HTTPCode != 200)
				throw new Exception("call SendContent prior to calling this function");

			m_writer.Write(sContent);
			m_writer.Flush();
		}

		/// <summary>
		/// Writes a line to the output stream
		/// </summary>
		/// <param name="sContent">the string to be written</param>
		public void WriteLine(string sContent)
		{
			if(m_status != ResponseStatus.HeaderSent || HTTPCode != 200)
				throw new Exception("call SendContent prior to calling this function");

			m_writer.WriteLine(sContent);
			m_writer.Flush();
		}

		public void Write(byte[] aContent)
		{
			if(m_status != ResponseStatus.HeaderSent || HTTPCode != 200)
				throw new Exception("call SendContent prior to calling this function");

			m_stream.Write(aContent, 0, aContent.Length);
			m_stream.Flush();

		}

		/// <summary>
		/// Copies a stream to the output stream
		/// </summary>
		/// <param name="aStream">the stream to be copied</param>
		public void Write(Stream aStream)
		{
			if(m_status != ResponseStatus.HeaderSent || HTTPCode != 200)
				throw new Exception("call SendContent prior to calling this function");

			const int bufSize = 1024 * 1024;
			byte[] buf = new byte[bufSize];
			int len;
			do
			{
				len = aStream.Read(buf, 0, bufSize);
				m_stream.Write(buf, 0, len);
			}while(len != 0);
			m_stream.Flush();
		}

		/// <summary>
		/// Writes a file from the Resources
		/// </summary>
		/// <param name="name"></param>
		public void WriteResource(string name)
		{
			try
			{
				Stream aStream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.Neon.Actinium.Xeon.Resources." + name);
				if(m_status != ResponseStatus.HeaderSent || HTTPCode != 200)
					throw new Exception("call SendContent prior to calling this function");

				const int bufSize = 1024 * 1024;
				byte[] buf = new byte[bufSize];
				int len;
				do
				{
					len = aStream.Read(buf, 0, bufSize);
					m_stream.Write(buf, 0, len);
				}while(len != 0);
				m_stream.Flush();
				
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.Message);
			}
		}

		/// <summary>
		/// If nothing has been sent this method replaces the output stream with
		/// a memory stream. All that is written to the response after that is actually
		/// buffered. When <code>Done</code> is called the memory stream is flushed
		/// to the network stream
		/// </summary>
		public void SwitchToMemoryStream()
		{
			CheckHeaderSent();

			if(m_originalStream != null)
				throw new Exception("SwitchToMemoryStream has already been called");

			m_originalStream = m_stream;
			m_stream = new MemoryStream();
			m_writer = new StreamWriter(m_stream);
		}

		/// <summary>
		/// Tells whether a Memory stream or a network stream is currently in use
		/// </summary>
		public bool IsInMemoryStreamMode
		{
			get
			{
				return m_originalStream != null;
			}
		}

		/// <summary>
		/// Flushes the memory stream to the network one and than flushes the network stream
		/// </summary>
		public void Done()
		{
			if(m_status == ResponseStatus.Done)
				throw new Exception("method Done has already been called for this response");

			m_status = ResponseStatus.Done;
			if(m_originalStream != null)
			{
				((MemoryStream)m_stream).WriteTo(m_originalStream);

				m_stream.Close();
				m_stream = m_originalStream;
			}

			m_writer.Flush();
			m_stream.Flush();
			/*m_writer.Close();
			m_stream.Close();*/
		}

		/// <summary>
		/// If the Response is currently using a memory stream it clears the stream (buffer)
		/// </summary>
		public void Reset()
		{
			if(m_status == ResponseStatus.Done)
				throw new Exception("method Done has already been called for this response");

			if(m_originalStream == null)
				throw new Exception("call SwitchToMemoryStream first");

			m_stream.Close();
			m_stream = new MemoryStream();
			m_writer = new StreamWriter(m_stream);
			m_status = ResponseStatus.Initial;
		}
	}
}
