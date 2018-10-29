using System;
using System.Collections;

namespace  Netron.Neon
{
	/// <summary>
	/// Strongly typed collection of tasks
	/// </summary>
	public class NTaskCollection : CollectionBase
	{
		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructor
		
		public NTaskCollection()
		{
			
		}
		#endregion

		#region Methods

		/// <summary>
		/// Adds a task to the collection
		/// </summary>
		/// <param name="task">a NTask object</param>
		/// <returns>the index number in the list</returns>
		public int Add(NTask task)
		{
			if(task == null) return -1;
			return this.InnerList.Add(task);
		}

		/// <summary>
		/// Adds a range of tasks to the collection
		/// </summary>
		/// <param name="tasks"></param>
		public void AddRange(NTaskCollection tasks)
		{
			this.InnerList.AddRange(tasks);
		}


		#endregion
	}
}
