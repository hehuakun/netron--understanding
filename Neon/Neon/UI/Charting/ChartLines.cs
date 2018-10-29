using System;
using System.Collections;
namespace Netron.Neon
{
	/// <summary>
	/// A STC of chart lines
	/// </summary>
	public class ChartLines : CollectionBase
	{
		
		public int Add(ChartLine line)
		{			
			return this.InnerList.Add(line);
		}

		public ChartLine this[int index]
		{
			get{return this.InnerList[index] as ChartLine;}
		}
		public ChartLines()
		{
			
		}
	}
}
