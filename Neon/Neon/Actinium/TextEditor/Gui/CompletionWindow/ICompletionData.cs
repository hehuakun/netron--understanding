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
using Netron.Neon.TextEditor;

namespace Netron.Neon.TextEditor.Gui.CompletionWindow
{
	public interface ICompletionData : IComparable
	{
		int ImageIndex {
			get;
		}
		
		string[] Text {
			get;
		}
		
		string Description {
			get;
		}
		
		void InsertAction(TextEditorControl control);
	}
}
