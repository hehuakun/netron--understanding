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
	/// <summary>
	/// This is the simplest example of a custom servlet.
	/// It derives from the ServletPageBase and does not require XML data.
	/// </summary>
	[ServletPage()]
	public class TestPage : ServletPageBase
	{
		public TestPage()
		{
		}

		/// <summary>
		/// Here you can try out whatever and write to the browser.
		/// </summary>
		/// <param name="aRequest"></param>
		public override void Answer(WebRequest aRequest)
		{
			
			aRequest.Response.SendContent("text/html");
			string test = @"
							<script language=""javascript"">
							function fnTestExternalObject()
							{
								var str = ""Single "";
								str = window.external.GetTwice(""Single "");
								window.external.ShowMessageBox(str, ""Using window.external"");
							}
							function fnTestErrorPages()
							{
								alert(""Testing error pages"");
							}
							</script>
							<a href=""javascript:fnTestExternalObject();"">Test external object </a>
							<br>
							<a href=""/ErrorPageTest/TestErrorPage.xsp"">Test error pages </a>
							";
			aRequest.Response.WriteLine(test);
			if(aRequest.Parameters["say"] != null)
			{
				
//				NAFPluginHeaders headers= this.Root.GetPluginsInfo();
//				for(int k = 0;k<headers.Applications.Count;k++)
//				{
//					aRequest.Response.WriteLine("<b>" + headers.Applications[k].Summary.Name + "</b><br><blockquote>" +  headers.Applications[k].Location + "</blockquote><br><br>");
//				}
//				this.Root.NOutput.WriteLine("You said '" + aRequest.Parameters["say"].ToString() + "' in the browser!");
			}

			aRequest.Response.WriteLine("<form action='/Test.xsp' method='GET'><input name='say' id='say' type='text'  size='15'><input type='submit' value='submit'></form>");
		//http://localhost:8080/test.xsp
		}
		/// <summary>
		/// The address of this page
		/// </summary>
		public override string Address
		{
			get
			{
				return "/Test.xsp";
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
	}
}
