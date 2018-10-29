using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
//using System.Runtime.Serialization.Formatters.Soap;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Netron.GraphLib.Attributes;
using Netron.GraphLib.UI;
using Netron.GraphLib.Interfaces;
using Netron.GraphLib;
using System.Text;
namespace Netron.AutomataShapes
{
	/// <summary>
	/// Allows any C# code to be used in order to generate automata output
	/// </summary>
	[Serializable]
	[Description("Scripter")]
	[NetronGraphShape("Scripter","8A8DA1FD-48CE-4a44-9883-25F4AA89F4BD","Automata","Netron.AutomataShapes.Scripter",
		 "Scripting node.")]
	public class Scripter : Shape, IHost, IDisposable
	{

		#region Fields
		/// <summary>
		/// the source code
		/// </summary>
		[NonSerialized] string scriptSourceCode = "";
		/// <summary>
		/// the script
		/// </summary>
		[NonSerialized] IScript script = null;
		/// <summary>
		/// the out connector
		/// </summary>
		private Connector OutConnector;
		/// <summary>
		/// the X connector
		/// </summary>
		private Connector XInConnector;
		/// <summary>
		/// the Y connector
		/// </summary>
		private Connector YInConnector;
		/// <summary>
		/// temporary assemblies
		/// </summary>
		[NonSerialized] private ArrayList TempAssemblies;
		#endregion


		#region Constructor
		/// <summary>
		/// the ctor
		/// </summary>
		public Scripter():base()
			
		{
			this.IsResizable=false;
			Rectangle = new RectangleF(0, 0, 100, 60);
		
			OutConnector=new Connector(this,"Output",true);
			OutConnector.ConnectorLocation = ConnectorLocation.East;
			XInConnector = new Connector(this,"X-Input",false);
			XInConnector.ConnectorLocation = ConnectorLocation.West;
			YInConnector = new Connector(this,"Y-Input",false);
			YInConnector.ConnectorLocation = ConnectorLocation.West;
			this.Connectors.Add(OutConnector);
			this.Connectors.Add(XInConnector);
			this.Connectors.Add(YInConnector);
			this.TempAssemblies=new ArrayList();
			LoadDefaultScript();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the script
		/// </summary>
		public IScript Script
		{
			get{return script;}
			set{script = value;}
		}
		#endregion

		#region Methods

		#region Access to the propertygrid
		public override void AddProperties()
		{
			base.AddProperties ();
			Bag.Properties.Add(new PropertySpec("script",typeof(string),"Automata","The script to be executed","",typeof(ScriptUIEditor), typeof(TypeConverter)));
			PropertySpec spec=new PropertySpec("Owner",typeof(Scripter));
			spec.Attributes=new Attribute[]{new System.ComponentModel.ReadOnlyAttribute(true)};
			Bag.Properties.Add(spec);
		}

		protected override void GetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			base.GetPropertyBagValue (sender, e);
			switch(e.Property.Name)
			{
				case "script":
					e.Value = this.scriptSourceCode; break;
				case "Owner":
					e.Value = this; break;
			}
		}
		protected override void SetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			base.SetPropertyBagValue (sender, e);
			switch(e.Property.Name)
			{
				case "script":
					this.scriptSourceCode = (string) e.Value; 
					try
					{
						if(this.Tag ==null) throw new Exception("Script will not execute, invalid script.Check the source code.");
						//if(this.tag is Script)
						{
							//do the crossing
							this.script = (IScript) Tag ;
							script.Initialize(this);
						}

					}
					catch(Exception exc)
					{
						MessageBox.Show(exc.Message);
					}
					
					break;

			}
		}


		#endregion
		

		/// <summary>
		/// Paints the shape on the canvas
		/// </summary>
		/// <param name="g"></param>
		public override void Paint(System.Drawing.Graphics g)
		{

			
			Color Background = IsSelected ? Color.LightSteelBlue : Color.WhiteSmoke;
		
			g.FillRectangle(new SolidBrush(Background), Rectangle.Left, Rectangle.Top , Rectangle.Width , Rectangle.Height );
			g.FillRectangle(new SolidBrush(ShapeColor), Rectangle.X, Rectangle.Y, Rectangle.Width , 12 );
			g.DrawRectangle(new Pen(Color.Black,IsSelected ? 2F : 1F),Rectangle.Left,Rectangle.Top,Rectangle.Width,Rectangle.Height);
			
			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			g.DrawString("Scripter", Font, new SolidBrush(TextColor), Rectangle.Left + (Rectangle.Width / 2), Rectangle.Top , sf);
		
		}
		/// <summary>
		/// Initializes the automata
		/// </summary>
		public override void InitAutomata()
		{
			//this one should not re-initialize or the user script will be lost
		}
		/// <summary>
		/// Loads the default script 
		/// </summary>
		private void LoadDefaultScript()
		{
			System.IO.Stream s;
			byte[] b;

			// Get default script source from embedded text file
			//Note: the file should have the 'embedded resource' property in VS! Also, the first name is the default namespace and is necessary.
			s=Assembly.GetCallingAssembly().GetManifestResourceStream("Netron.AutomataShapes.Resources.Scripts.TemplateScript.txt");
			
			try
			{
				b = new byte[Convert.ToInt32(s.Length)];
				s.Read(b, 0, Convert.ToInt32(s.Length));
				scriptSourceCode = System.Text.ASCIIEncoding.ASCII.GetString(b);
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.Message);
				scriptSourceCode="";
			}
			finally
			{
			
			}
		}
		
		/// <summary>
		/// Returns the locations of the connectors
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public override System.Drawing.PointF ConnectionPoint(Connector c)
		{
			
			
			if (c == OutConnector) return new PointF(Rectangle.Right, Rectangle.Top+(Rectangle.Height/2));
			if (c == XInConnector) return new PointF(Rectangle.Left, Rectangle.Top+12+(Rectangle.Height-12)/3);
			if (c == YInConnector) return new PointF(Rectangle.Left, Rectangle.Top+12+2*(Rectangle.Height-12)/3);
			return base.ConnectionPoint (c);
			//return new PointF(0, 0);
		}
		/// <summary>
		/// Updates the automata to the next state
		/// </summary>
		public override void Update()
		{
			//clear the sends values
			
			this.OutConnector.Sends.Clear();		
			if (XInConnector.Receives.Count>0 && YInConnector.Receives.Count>0)
			{
				//int inp = (int) XInConnector.Receives[0];
				if (script != null) 
					//this.OutConnector.Sends.Add(script.Compute());
					script.Compute();
				//Trace.WriteLine(OutConnector.Sends[0].ToString());
					
			}
			this.XInConnector.Receives.Clear();
			this.YInConnector.Receives.Clear();
			//base.Update ();

		}
		
		 
		

		#region IHost Members

		

		public void ShowMessage(string message)
		{
		
			Trace.WriteLine(message);
		}
		public Connector Out
		{
			get
			{
				return this.OutConnector;
			}
		}
		public Connector XIn
		{
			get
			{
				return this.XInConnector;
			}
		}
		public Connector YIn
		{
			get
			{
				return this.YInConnector;
			}
		}

		#endregion

		#region IDisposable Members

		// Vincent J. Lasorsa 07/5/2005 11:34 AM EST
		// added keyword override
		/*public override void Dispose() 
		{
			Dispose(true);
			GC.SuppressFinalize(this); 
		}*/

		protected virtual void Dispose(bool disposing) 
		{
			if (disposing) 
			{
				// Free other state (managed objects).
			}
			// Free your own state (unmanaged objects).
			if (script != null)
				script.Dispose();		
			if(TempAssemblies!=null)
			{
				foreach(string asspath in this.TempAssemblies)
				{
					try
					{
						//TODO: none of the files are deleted, donnot know how to free these resources...
						File.Delete(asspath);
					}
					catch(Exception exc)
					{
						Trace.WriteLine(exc.Message);
					}
				}
			}
			// Set large fields to null.
		}

		// Use C# destructor syntax for finalization code.
		

		~Scripter()
		{
			Dispose (false);
		}
		#endregion


		/// <summary>
		/// Overrides the default thumbnail used in the shape viewer
		/// </summary>
		/// <returns></returns>
		public override Bitmap GetThumbnail()
		{
			Bitmap bmp=null;
			try
			{
				Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.AutomataShapes.Resources.Scripter.gif");
					
				bmp= Bitmap.FromStream(stream) as Bitmap;
				stream.Close();
				stream=null;
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.Message);
			}
			return bmp;
		}
		#endregion
		
	}


}


