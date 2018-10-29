// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Mike Krüger" email="mike@icsharpcode.net"/>
//     <version value="$version"/>
// </file>

using Netron.Neon.TextEditor.Undo;

namespace Netron.Neon.TextEditor.Document
{
	/// <summary>
	/// This interface is used to describe a span inside a text sequence
	/// </summary>
	public interface ISegment
	{
		/// <value>
		/// The offset where the span begins
		/// </value>
		int Offset {
			get;
			set;
		}
		
		/// <value>
		/// The length of the span
		/// </value>
		int Length {
			get;
			set;
		}
	}
	
}
