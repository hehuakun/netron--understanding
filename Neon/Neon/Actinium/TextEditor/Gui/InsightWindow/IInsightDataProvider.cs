// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Mike Krüger" email="mike@icsharpcode.net"/>
//     <version value="$version"/>
// </file>

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.Collections;

using Netron.Neon.TextEditor.Document;

namespace Netron.Neon.TextEditor.Gui.InsightWindow
{
	public interface IInsightDataProvider
	{
		void SetupDataProvider(string fileName, TextArea textArea);
		
		bool CaretOffsetChanged();
		bool CharTyped();
		
		string GetInsightData(int number);
		
		int InsightDataCount {
			get;
		}
	}
}
