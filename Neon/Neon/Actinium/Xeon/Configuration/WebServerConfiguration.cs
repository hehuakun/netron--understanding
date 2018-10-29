using System;
using System.Configuration;
using System.Xml;
using System.Collections;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
namespace  Netron.Xeon
{
	/// <summary>
	/// Enum containing the mode options for the exceptionManagement tag.
	/// </summary>
	public enum ExceptionManagementMode 
	{
		/// <summary>The ExceptionManager should not process exceptions.</summary>
		Off,
		/// <summary>The ExceptionManager should process exceptions. This is the default.</summary>
		On
	}
	/// <summary>
	/// Implements the IConfigurationSectionHandler and reads the config section.
	/// </summary>
	public class WebServerConfiguration : IConfigurationSectionHandler
	{
		private static object StringToEnum( Type t, string Value )
		{
			foreach ( FieldInfo fi in t.GetFields() )
				if ( fi.Name == Value )
					return fi.GetValue( null );    // We use null because
			// enumeration values
			// are static

			throw new Exception( string.Format("Can't convert {0} to {1}", Value,  t.ToString()) );
		}

	
		
		#region IConfigurationSectionHandler Members

		/// <summary>
		/// Required method in the IConfigurationSectionHandler and creates a DebugSetting class
		/// per read node in the app.config.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="configContext"></param>
		/// <param name="section"></param>
		/// <returns></returns>
		public object Create(object parent, object configContext, System.Xml.XmlNode section)
		{

			WebServerSettings settings=null;
			try
			{
				XmlNode startUp=section.SelectSingleNode("StartUp");
				XmlNode portNode=section.SelectSingleNode("ServerPort");
				XmlNode staticContentNode=section.SelectSingleNode("StaticContent");
				XmlNode dynamicContentNode=section.SelectSingleNode("DynamicContent");
				XmlNode xmlDumpNode=section.SelectSingleNode("XmlDump");
				XmlNode xmlDebugReloadNode=section.SelectSingleNode("XmlDebugReload");
				XmlNode servletNode=section.SelectSingleNode("Servlets");
				settings=new WebServerSettings();
				if(startUp!=null)
					settings.StartUp = bool.Parse(startUp.Attributes["Value"].Value);
				settings.ServerPort=int.Parse(portNode.Attributes["value"].Value);
				settings.StaticContent=staticContentNode.Attributes["location"].Value;
				settings.DynamicContent=dynamicContentNode.Attributes["location"].Value;
				if(servletNode!=null)
				{
					if(servletNode.Attributes["assembly"] != null)
					{
						settings.Servlets= servletNode.Attributes["assembly"].Value.Split(',');
						for(int k =0;k<settings.Servlets.Length; k++)
							settings.Servlets[k] = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + settings.Servlets[k].Trim();
					}
				}
				settings.XmlDumpPath=xmlDumpNode.Attributes["location"].Value;
				settings.XmlDebugReload=bool.Parse(xmlDebugReloadNode.Attributes["value"].Value);
					
			}
			catch(Exception exc)
			{
				Trace.WriteLine("An error occured while reading the webserver configuration..." + exc.Message,"Warning");
			}
			
			return settings;
		}

		#endregion
	}
	
	
}
