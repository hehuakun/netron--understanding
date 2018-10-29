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
	/// Serves to tag the servlet classes
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class ServletPage : Attribute
	{
	}

}
