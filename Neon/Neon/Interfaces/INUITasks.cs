using System;

namespace Netron.Neon
{
	/// <summary>
	/// Interface a NAF tasks
	/// </summary>
	public interface INUITasks 
	{
		

		void RemoveSelected();
		/// <summary>
		/// Sets the given column in edit mode
		/// </summary>
		/// <param name="column"></param>
		void EditColumn(int column);
		/// <summary>
		/// Adds a new task and sets it to edit mode
		/// </summary>
		void NewTask();
		/// <summary>
		/// Add a task
		/// </summary>
		/// <param name="message"></param>
		void AddTask(NTask task);
		/// <summary>
		/// Gets all tasks
		/// </summary>
		NTaskCollection GetTasks();
		/// <summary>
		/// Removes an item at the specified index.
		/// </summary>
		/// <param name="index"></param>
		void RemoveTask(int index);

		/// <summary>
		/// Retrieves a task item from the list
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		NTask GetTask(int index);

	}
}
