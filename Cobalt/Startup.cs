using System;
using System.Windows.Forms;
using Netron.GraphLib;
namespace Netron.Cobalt
{
	/// <summary>
	/// Summary description for Startup.
	/// </summary>
	public class Startup
	{
		
		public Startup(){}
		
#if DEBUG
		/// <summary>
		/// The startup without the splash-screen
		/// </summary>
		[STAThread]
		static void Main() 
		{
			//Application.EnableVisualStyles();		
			Application.Run(new MainForm());
		}
#else
		/// <summary>
		/// Startup with splash-screen 
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.EnableVisualStyles();		
			#region Start the fun...

			context = new ApplicationContext();

			//	then we subscribe to the OnAppIdle event...
			Application.Idle += new EventHandler(OnAppIdle);

			//	...and show our SplashForm
			//SplashScreen.ShowSplashScreen();
			SplashScreen.SetStatus("Starting the main application...");
			
			
			//	instead of running a window, we use the context
			Application.Run(context);
			#endregion

		}


		private static ApplicationContext context;
		//private static SplashScreen sForm = new SplashScreen();
		private static MainForm mForm  = new MainForm();
		private static void OnAppIdle(object sender, EventArgs e)
		{
			if(context.MainForm == null)
			{
				//	first we remove the eventhandler
				Application.Idle -= new EventHandler(OnAppIdle);

				mForm.Loading+=new Netron.Cobalt.MainForm.LoadingInfo(mainForm_Loading);
				//	here we preload our form
				mForm.PreLoad();

				//	now we set the main form for the context...
				context.MainForm = mForm;

				//	...show it...
				context.MainForm.Show();

				System.Threading.Thread.Sleep(1300);
				//	...and hide the splashscreen. done!
				SplashScreen.CloseForm();
				
			}
		}
		private static void mainForm_Loading(string moduleName)
		{
			
			SplashScreen.SetStatus(moduleName);
		}
#endif
	}
}
