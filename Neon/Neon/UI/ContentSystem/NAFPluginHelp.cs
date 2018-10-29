using System;

namespace Netron.Neon
{
	/// <summary>
	/// 
	/// </summary>
	public class NAFPluginHelp
	{
		#region Fields

		/// <summary>
		/// remote or local help
		/// </summary>
		protected HelpLocations location;

		/// <summary>
		/// where to find the resources
		/// </summary>
		protected string address;

		/// <summary>
		/// the name as used in the address
		/// </summary>
		protected string helpName;

		/// <summary>
		/// whether flat or assembly resources
		/// </summary>
		protected HelpTypes helpType;
		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the resource location type (remote/local)
		/// </summary>
		public HelpLocations Location
		{
			get{return location;}
			set{location=value;}
		}
		/// <summary>
		/// Gets or sets the address of the resource
		/// </summary>
		public string Address
		{
			get{return address;}
			set{address=value;}
		}
		/// <summary>
		/// Gets or sets the name of the help as it will be accessed
		/// in the browser address
		/// </summary>
		public string HelpName
		{
			get{return helpName;}
			set{helpName=value;}
		}
		/// <summary>
		/// Gets or sets the type of resource (assembly/flat...)
		/// </summary>
		public HelpTypes HelpType
		{
			get{return helpType;}
			set{helpType=value;}
		}


		#endregion

		#region Constructor
		
		public NAFPluginHelp()
		{
			
		}
		public NAFPluginHelp(HelpLocations helpLocation, HelpTypes helpType, string address, string helpName)
		{
			this.location=helpLocation;
			this.helpType=helpType;
			this.address=address;
			this.helpName=helpName;
		}
		#endregion

		#region Methods

		#endregion
	}
}
