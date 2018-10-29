using System.Diagnostics;
using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
namespace Netron.Neon
{
	internal class BaseListDesigner : BaseDesigner
	{
		
		private NeonBaseList m_Control;
		
		public BaseListDesigner(){
		}
		
		public override void Initialize (System.ComponentModel.IComponent component)
		{
			base.Initialize(component);
			m_Control = ((NeonBaseList)(this.Control));
		}
		
		protected override void OnPaintAdornments (PaintEventArgs pe)
		{
			base.OnPaintAdornments(pe);
			ControlPaint.DrawBorder(pe.Graphics, CtrlHelper.CheckedRectangle(0, 0, m_Control.Width, m_Control.Height), Color.FromKnownColor(KnownColor.ControlDarkDark), ButtonBorderStyle.Dotted);
		}
	}
}
