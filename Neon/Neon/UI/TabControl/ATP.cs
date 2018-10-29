using System;
using System.Windows.Forms;
namespace Netron.Neon
{
	/// <summary>
	/// The only way to make a dragdrop-able object
	/// is to have it serializable. The TabPage isn't, so
	/// this struct does the trick.
	/// </summary>
	[Serializable] public struct ATP
	{
		/// <summary>
		/// The actual dragged page
		/// </summary>
		public TabPage Element;
		/// <summary>
		/// The tabcontrol it's coming from
		/// </summary>
		public NTabControl Origin;
	}
}
