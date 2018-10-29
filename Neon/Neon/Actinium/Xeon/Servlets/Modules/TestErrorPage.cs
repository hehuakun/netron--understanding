using System;
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
	[ServletPage()]
	public class TestErrorPage : ServletPageBase
	{
		public TestErrorPage()
		{
		}

		public override void Answer(WebRequest aRequest)
		{
			try
			{
				throw new IOException("This is the inner exception");
			}
			catch(Exception ex)
			{
				throw new Exception("outer exception", ex);
			}
		
		}

		public override string Address
		{
			get
			{
				return "/ErrorPageTest/TestErrorPage.xsp";
			}
		}

		public override string ExceptionPage
		{
			get
			{
				return "/ErrorPageTest/ErrorPageHandler.xsp";
			}
		}
	}
}
