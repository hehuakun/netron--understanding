using System;
using System.Windows.Forms;
namespace Netron.Neon
{
	public interface INUITabElement : INElement
	{
		event EventHandler OnShow;
		TabPage Tab {get; set;}

		void RaiseShow();
	}
}
