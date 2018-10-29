using System;
using System.IO;
namespace Netron.Xeon
{
	/// <summary>
	/// Allows to browse the local machine via something like
	/// http://localhost:8080/MyComputer/c:/temp/csharp.txt
	/// </summary>
	public class MyComputerResolver : StaticResolverBase
	{
		public override Stream GetResourceAsStream(string sFileName)
		{
			if(sFileName.StartsWith("/MyComputer/"))
			{
					
				string sNewPath = sFileName.Substring("/MyComputer/".Length);
				return new FileStream(sNewPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			}
			else
				return null;
		}

		public override bool Resolves(WebRequest aRequest)
		{
			string sFileName = aRequest.File;
			if(sFileName.StartsWith("/MyComputer/"))
			{
					
				string sNewPath = sFileName.Substring("/MyComputer/".Length);
				return File.Exists(sNewPath);
			}
			else
				return false;
		}
	}

}
