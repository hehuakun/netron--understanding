using System;

namespace Netron.Neon
{
	/// <summary>
	/// Interface of the favorites service 
	/// </summary>
	public interface INAFFavorites : INUITabElement
	{
		void AddFavorite(URLInfo url);
	}
}
