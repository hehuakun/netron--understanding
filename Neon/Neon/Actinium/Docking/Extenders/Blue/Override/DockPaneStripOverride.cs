using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Netron.Neon.Docking.Extenders.Blue
{
	[ToolboxItem(false)]
	public class DockPaneStripOverride : DockPaneStripVS2003
	{
		protected internal DockPaneStripOverride(DockPane pane) : base(pane)
		{
			BackColor = SystemColors.ControlLight;
		}
	}
}
