using System;
using System.Runtime.InteropServices;
namespace Netron.Neon
{
	[
	InterfaceType(ComInterfaceType.InterfaceIsDual), 
	ComVisible(true), 
	Guid(@"3050f485-98b5-11cf-bb82-00aa00bdce0b")
	]
	public interface IHTMLDocument3 {

		void releaseCapture(); 
		void recalc(bool fForce);
		
		[return: MarshalAs(UnmanagedType.Interface)] /* IHTMLDOMNode */
		object createTextNode(string text);

		IHTMLElement documentElement();

		//... we need only documentElement(), more functions/properties see MSHTML.Idl/.h
	}
}
