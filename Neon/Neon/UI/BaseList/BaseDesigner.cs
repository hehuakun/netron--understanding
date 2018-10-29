using System.Diagnostics;
using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
namespace Netron.Neon
{
	
	public abstract class BaseDesigner : System.Windows.Forms.Design.ParentControlDesigner
	{
		
		private Pen m_BorderPen = new Pen(SystemColors.ControlDark);
		private Pen m_WorkspacePen = new Pen(SystemColors.ControlLight);
		private Font m_Font = new Font("Arial", 6, FontStyle.Regular);
		private StringFormat m_Format = new StringFormat(StringFormatFlags.LineLimit);
		private DesignerVerbCollection m_Verbs = new DesignerVerbCollection();
		private bool m_mouseover = false;
		
		private const string compName = "The Netron Project";
		
		public BaseDesigner(){
			m_WorkspacePen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
			m_BorderPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
			m_Format.Trimming = StringTrimming.EllipsisCharacter;
			m_Verbs.Add(new DesignerVerb("About", new EventHandler( AboutEvent)));
		}
		
		protected override void OnMouseEnter ()
		{
			this.m_mouseover = true;
			this.Control.Refresh();
		}
		
		protected override void OnMouseLeave ()
		{
			this.m_mouseover = false;
			this.Control.Refresh();
		}
		
		protected override void OnPaintAdornments (PaintEventArgs pe)
		{
			base.OnPaintAdornments(pe);
			pe.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
			
			//place the text in the left bottom
			SizeF _s = pe.Graphics.MeasureString(compName, m_Font, this.Control.Width, m_Format);
			if (this.MouseOver == true)
			{
				pe.Graphics.FillRectangle(new SolidBrush(Color.White), 0, this.Control.Height - _s.Height, _s.Width, _s.Height);
				pe.Graphics.DrawRectangle(new Pen(Color.Black), 0, this.Control.Height - _s.Height, _s.Width, _s.Height);
				pe.Graphics.DrawString("The Netron Project", m_Font, new SolidBrush(Color.Black), CtrlHelper.CheckedRectangleF(0, this.Control.Height - _s.Height, _s.Width, _s.Height), m_Format);
			}
		}
		
		public override System.ComponentModel.Design.DesignerVerbCollection Verbs
		{
			get{
				return m_Verbs;
			}
		}
		
		protected void AboutEvent (object sender, EventArgs e)
		{
			
		}
		
		#region "protected properties"
		protected Pen BorderPen
		{
			get{
				return m_BorderPen;
			}
			set
			{
				m_BorderPen = value;
			}
		}
		
		protected Pen WorkspacePen
		{
			get{
				return m_WorkspacePen;
			}
			set
			{
				m_WorkspacePen = value;
			}
		}
		
		protected Font Font
		{
			get{
				return m_Font;
			}
			set
			{
				m_Font = value;
			}
		}
		
		protected bool MouseOver
		{
			get{
				return m_mouseover;
			}
			set
			{
				m_mouseover = value;
			}
		}
		
		protected string CompanyName
		{
			get{
				return compName;
			}
		}
		#endregion
	}
	
}
