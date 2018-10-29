using System;
using Netron.Neon;
using System.Collections;
namespace WebBrowser
{
	/// <summary>
	/// STC of ITab objects
	/// </summary>
	public class TabCollection : CollectionBase
	{
		#region Fields

		#endregion

		#region Properties

		public ITab this[int index]
		{
			get{return this.InnerList[index] as ITab;}
		}
		public ITab this[string name]
		{
			get
			{
				for(int k=0;k<this.InnerList.Count; k++)
				{
					if(this[k].Identifier==name)
						return this[k];
				}
				return null;
			}
		}
		#endregion

		#region Constructor

		#endregion

		#region Methods
		public int Add(ITab tab)
		{
			return this.InnerList.Add(tab);
		}

		public void Remove(ITab tab)
		{
			this.InnerList.Remove(tab);
		}
		#endregion
		
	}

}

