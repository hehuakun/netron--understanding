using System;

namespace Netron.Neon
{
	/// <summary>
	/// Channel info passed by the output control
	/// </summary>
	public delegate void ChannelNotification(string channelName, string msg);
	/// <summary>
	/// General purpose string delegate
	/// </summary>
	public delegate void StringNotification(string msg);
	/// <summary>
	/// Passes the object to be shown in the propsgrid
	/// </summary>
	public delegate void PropsInfo(object obj);
	/// <summary>
	/// Passes URL info
	/// </summary>
	public delegate void URLHandler(object sender, URLInfo e);
	/// <summary>
	/// Passes tab info
	/// </summary>
	public delegate void SelectedTabPageChangeEventHandler(Object sender, TabPageChangeEventArgs e);
	/// <summary>
	/// Passes tak info
	/// </summary>
	public delegate void TaskInfo(NTask task);

}
