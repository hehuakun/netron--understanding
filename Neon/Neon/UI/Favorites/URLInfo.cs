using System;

namespace Netron.Neon
{
	public class URLInfo : EventArgs
	{
		protected string url;
		protected string urlName;

		public string URL
		{
			get{return url;}
			set{url= value;}
		}
		public string URLName
		{
			get{return urlName;}
			set{urlName = value;}
		}

		public URLInfo(string url, string urlName) 
		{
			this.url = url;
		}
		public URLInfo()
		{}
	}

}
