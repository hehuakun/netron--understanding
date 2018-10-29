using System;
using System.Collections;
namespace Netron.Neon
{
	/// <summary>
	/// STC of float array with a constraint capacity
	/// </summary>
	public class DataList : System.Collections.CollectionBase
	{

		#region Fields
		int capacity;
		#endregion 

		#region Constructor

		public DataList(int capacity) 
		{
			this.capacity = capacity;
		}
		
		#endregion

		#region Methods
		public int Add(float[] val)
		{
			if(this.InnerList.Count>capacity) throw new OverflowException("The maximum capacity of the DataListwas reached.");
			return this.InnerList.Add(val);
		}

		public float[] this[int index]
		{
			get{return this.InnerList[index] as float[];}
			set
			{
				this.InnerList[index] = value;}
		}

		#endregion
	}
}
