using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Printing;
using Netron.GraphLib;
namespace Netron.Cobalt
{
	/// <summary>
	/// Summary description for NetronPrinter.
	/// </summary>
	public class NetronPrinter : PrintDocument
	{
		#region Fields
		/// <summary>
		/// the coordinates of the leftupper corner of the current page
		/// </summary>
		private int x,y;
		private GraphLib.UI.GraphControl graphControl;
		private PrinterSettings printerSettings;
		#endregion

		public NetronPrinter(PrinterSettings psettings,GraphLib.UI.GraphControl gc)
		{
			if(psettings!=null)
				printerSettings = psettings;
			x=y=0;
			graphControl =gc;
		}
		protected override void OnPrintPage(PrintPageEventArgs e)
		{
			base.OnPrintPage (e);
			e.HasMorePages = true;
			try
			{
				
				graphControl.PrintCanvas(printerSettings, e, ref x, ref y);
				if(( x== 0) && (y == 0))				
					e.HasMorePages = false;
			}				
			catch (Exception exc)
			{
				e.HasMorePages = false;
				MessageBox.Show(exc.Message);
			}
		}

		
	}
}




