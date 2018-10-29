using System.Diagnostics;
using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;

namespace Netron.Neon
{
	internal class CtrlHelper
	{
		/// <summary>
		/// this function returns the first non-transparent color of a control's parent(s)
		/// loops through all parents to find the first non transparent color
		/// </summary>
		/// <param name="ctrl"></param>
		/// <returns></returns>
		public static Color GetNonTransparentBackColor(Control ctrl)
		{
			Color bc;
			Control p;
			try
			{
				p = ctrl.Parent;
				bc = p.BackColor;
				while (bc.Equals(Color.Transparent) &  p.Parent != null)
				{
					p = p.Parent;
					bc = p.BackColor;
				}
				
				if (bc.Equals(Color.Transparent))
				{
					bc = SystemColors.Control;
				}
				
				return bc;
			}
			catch (Exception)
			{
				return SystemColors.Control;
			}
		}
		
		/// <summary>
		///  this function returns a checked rectangle (width and height always > 1
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public static RectangleF CheckedRectangleF(float x, float y, float width, float height)
		{
			return new RectangleF(x, y, System.Convert.ToSingle(width <= 0? 1: width), System.Convert.ToSingle(height <= 0? 1: height));
			
		}
		
		/// <summary>
		/// this function returns a checked rectangle (width and height always > 1 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public static Rectangle CheckedRectangle(int x, int y, int width, int height)
		{
			return new Rectangle(x, y, System.Convert.ToInt32(width <= 0? 1: width), System.Convert.ToInt32(height <= 0? 1: height));
		}
	}
	
}
