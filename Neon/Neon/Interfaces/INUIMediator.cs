


// Collects all interfaces and hence the foundation of the framework.
namespace Netron.Neon
{
	
	public interface INUIMediator
	{		
		
		
		INOutput Output {get; set;}
		INUIPropertyGrid PropertyGrid {get; set;}
		INAFFavorites NFavorites {get; set;}
		INUITextEditor TextEditor {get; set;}		
		INUITitleBar TitleBar {get; set;}
	}

	
} 

