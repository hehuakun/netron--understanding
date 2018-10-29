using System;

namespace Netron.GraphLib.Entitology
{
	/// <summary>
	/// Enumerates the subtypes
	/// </summary>
	public enum ClassDeclarationModifiers
	{
		Abstract,
		Sealed,
		Public,
		Private,
		None
	}

	/// <summary>
	/// Flags the state op de ClassShape
	/// </summary>
	[Flags]
	public enum CollapseStates : int
	{
		Main =0x4,				
		Properties =0x2,
		Methods = 0x1
	}
}
