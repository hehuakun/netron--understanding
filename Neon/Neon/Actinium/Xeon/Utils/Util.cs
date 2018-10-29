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
	public class Util
	{
		static bool[] m_escape;

		static Util()
		{
			m_escape = new bool[256];
			Array.Clear(m_escape, 0, m_escape.Length);
			for (int i = 0; i <= 0x1f; i++)
				m_escape[i] = true;
			m_escape[0x7f] = true;
			string sEscape = " <>#%\"{}|\\^[]`";
			byte[] chars = Encoding.ASCII.GetBytes(sEscape);
			foreach(byte b in chars)
				m_escape[b] = true;
		}

		public static void WriteException(Exception e)
		{ 
			lock(typeof(Util))
			{
				while (e != null)
				{
					Trace.WriteLine("Exception: " + e.Message);
					Trace.WriteLine(e.StackTrace);
					e = e.InnerException;
				}
			}
		}

		public static void WriteExceptionToResponse(Exception e, WebResponse r)
		{ 
			while (e != null)
			{
				r.WriteLine("Exception: " + e.Message);
				r.WriteLine(e.StackTrace);
				e = e.InnerException;
			}
		}

		static string[] m_dayNames = new  string[] {"Mon" , "Tue" , "Wed", "Thu" , "Fri" , "Sat" , "Sun"};
		static string[] m_monthNames = new string[] {"Jan" , "Feb" , "Mar" , "Apr", "May" , "Jun" , "Jul" , "Aug", "Sep" , "Oct" , "Nov" , "Dec"};
		public static string getResponseDate(DateTime dtNow)
		{
			string sRet = "";
			switch(dtNow.DayOfWeek)
			{
				case DayOfWeek.Monday:
					sRet = m_dayNames[0];
					break;
				case DayOfWeek.Tuesday:
					sRet = m_dayNames[1];
					break;
				case DayOfWeek.Wednesday:
					sRet = m_dayNames[2];
					break;
				case DayOfWeek.Thursday:
					sRet = m_dayNames[3];
					break;
				case DayOfWeek.Friday:
					sRet = m_dayNames[4];
					break;
				case DayOfWeek.Saturday:
					sRet = m_dayNames[5];
					break;
				case DayOfWeek.Sunday:
					sRet = m_dayNames[6];
					break;
			}
			sRet +=	", " + dtNow.Day + " " + m_monthNames[dtNow.Month] + " " + dtNow.Year;
			sRet += " " + dtNow.Hour + ":" + dtNow.Minute + ":" + dtNow.Second + " GMT";
			return sRet;
		}

		public static string EscapeURL(string aStr)
		{
			string sHex = "0123456789ABCDEF";
			StringBuilder bld = new StringBuilder();
			foreach(char c in aStr)
			{
				if(m_escape[(byte)c])
				{
					bld.Append("%");
					bld.Append(sHex[((byte)c) >> 4]);
					bld.Append(sHex[((byte)c) & 15]);

				}
				else 
					bld.Append(c);
			}
			return bld.ToString();
		}
		public static string URLDecode(string url)		
		{
			
			System.Text.StringBuilder sb =new StringBuilder();
			
			int k = 0;
			for(int c = 0;c < url.Length;c++)
			{
				int cTr = url[c];
				if((cTr == '%')&&((c + 2) <= url.Length))
				{
					
					sb.Append( (char) int.Parse(url.Substring((c+1),((c+=2)+1))));
				}
				else if(cTr == '+')
				{
					sb.Append( ' ');
				}
				else
				{
					sb.Append((char) cTr);
				}
				k++;
			}
			return sb.ToString();
		}
	}

	
}
