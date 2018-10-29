using System;
using System.Collections;
namespace Netron.Neon
{
	/// <summary>
	/// STC of OutputChannel
	/// </summary>
	public class OutputChannelCollection : CollectionBase
	{


		public int Add(OutputChannel channel)
		{
			return this.InnerList.Add(channel);
		}
		public OutputChannel this[int index]
		{
			get{return this.InnerList[index] as OutputChannel;}
		}
		internal OutputChannel this[string name]
		{
			get
			{
				for(int k=0; k<this.InnerList.Count; k++)
				{
					if((this.InnerList[k] as OutputChannel).Name==name)
						return this.InnerList[k] as OutputChannel;
				}
				
					return null;
			}
		}

		public void Remove(string channelName)
		{
			for(int k=0; k<this.InnerList.Count; k++)
			{
				if((this.InnerList[k] as OutputChannel).Name==channelName)
					this.InnerList.RemoveAt(k);

			}
		}
	}
}
