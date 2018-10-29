using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Globalization;
using Microsoft.Win32;
using System.Xml;


namespace  Netron.Xeon
{
	/// <summary>
	/// MIME related functionalities
	/// </summary>
	public class MIMEManager
	{
		/// <summary>
		/// Gets the MIME application associated with a type to send to the browser,
		/// which will handle the rest.
		/// </summary>
		/// <param name="sExt"></param>
		/// <returns></returns>
		public static string getMIME(string sExt)
		{
			string sMIME = "octet/stream";
			try
			{
				sMIME = (string)Registry.ClassesRoot.OpenSubKey(sExt).GetValue("Content Type");
			}
			catch(Exception) {}
			return sMIME;
		}
	}
}
