// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Mike Krüger" email="mike@icsharpcode.net"/>
//     <version value="$version"/>
// </file>

using System;
using System.Collections;
using System.Reflection;
using System.Xml;
using System.IO;

namespace Netron.Neon.TextEditor.Document
{
	public class ResourceSyntaxModeProvider : ISyntaxModeFileProvider
	{
		ArrayList syntaxModes = null;
		
		public ArrayList SyntaxModes {
			get {
				return syntaxModes;
			}
		}
		
		public ResourceSyntaxModeProvider()
		{
			Assembly assembly = typeof(SyntaxMode).Assembly;
			Stream syntaxModeStream = assembly.GetManifestResourceStream("Netron.Neon.Actinium.TextEditor.syntaxmodes.SyntaxModes.xml");
			if (syntaxModeStream == null) throw new ApplicationException("Could not fetch the manifest resource stream containing the syntax mode in path 'Netron.Neon.Actinium.TextEditor.syntaxmodes.SyntaxModes.xml'");
			syntaxModes = SyntaxMode.GetSyntaxModes(syntaxModeStream);
		}
		
		public XmlTextReader GetSyntaxModeFile(SyntaxMode syntaxMode)
		{
			Assembly assembly = typeof(SyntaxMode).Assembly;
			return new XmlTextReader(assembly.GetManifestResourceStream("Netron.Neon.Actinium.TextEditor.syntaxmodes." + syntaxMode.FileName));
		}
	}
}
