using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Threading;
using System.Reflection;
using System.Xml;
using System.Xml.Xsl;
using System.Drawing;
using System.Drawing.Imaging;


namespace  Netron.Xeon
{
	[ServletPage()]
	public class PictureCropper : ServletPageBase
	{

		public PictureCropper()
		{
		}

		public override string Address
		{
			get
			{
				return "/ImageCropper.xsp";
			}
		}

		public override bool IsBuffered
		{
			get
			{
				return true;
			}
		}

		//http://localhost:8080/ImageCropper.xsp?path=C:\prj\Rady\Diplomna\FrameWork\ImageViewer\view\treeimages\plus.ico&width=152&height=52
		public override void Answer(WebRequest Request)
		{
			try
			{
				string sPath = Request["path"];
				Request.Response.SendContent("image/png");

				int nWidth = Int32.Parse(Request["width"]);
				int nHeight = Int32.Parse(Request["height"]);
				Bitmap bmp = new Bitmap(sPath, false);
				//Bitmap bmpNew = new Bitmap(bmp, nWidth, nHeight);
				Bitmap bmpNew = new Bitmap(nWidth, nHeight, PixelFormat.Format32bppRgb);
				Graphics gr = Graphics.FromImage(bmpNew);
				gr.DrawImage(bmp, 0, 0, nWidth, nHeight);

				bmpNew.Save(Request.Response.OutStream, ImageFormat.Png);
				Request.Response.OutStream.Flush();

				bmp.Dispose();
				bmpNew.Dispose();
				gr.Dispose();

			}
			catch(Exception ex)
			{
				Util.WriteException(ex);
			}
		}
	}
}
