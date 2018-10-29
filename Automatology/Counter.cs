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
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Netron.GraphLib.Attributes;
using Netron.GraphLib.UI;
using Netron.GraphLib.Interfaces;
using Netron.GraphLib;
using System.Text;
namespace Netron.Automatology
{
	/// <summary>
	/// Simple counter node, counting linearly up to some value
	/// </summary>
	[Serializable]
	[Description("Counter")]
	[NetronGraphShape("Counter","07D47D05-2774-418c-B444-72FEA879DB10","Automata","Netron.Automatology.Counter",
		 "Simple counter.")]
	public class Counter : Shape, ISerializable
	{
		#region Fields
		/// <summary>
		/// starting value
		/// </summary>
		private int startValue=0;
		/// <summary>
		/// the end value
		/// </summary>
		private int endValue=100;
		/// <summary>
		/// the current counter value
		/// </summary>
		private int counter;
		/// <summary>
		/// the only (out) connector
		/// </summary>
		private Connector outConnector;
		/// <summary>
		/// the size of the outoing array
		/// </summary>
		protected int arraySize = 1;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the starting value of the counter
		/// </summary>
		public int StartValue
		{
			get{return startValue;}
			set{startValue=value;}
		}
		/// <summary>
		/// Gets or sets the end value of the counter
		/// </summary>
		public int EndValue
		{
			get{return endValue;}
			set{endValue=value;}
		}
		#endregion

		#region Constructor

		/// <summary>
		/// the ctor
		/// </summary>
		public Counter():base()			
		{
			outConnector=new Connector(this,"Output",true);
			outConnector.ConnectorLocation = ConnectorLocation.East;
			this.Connectors.Add(outConnector);
			Rectangle = new RectangleF(0, 0, 70, 40);
			InitEntity();
		}

		/// <summary>
		/// ISerializable implementation
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData (info, context);		
			info.AddValue("outConnector", this.outConnector, typeof(Connector));		
			info.AddValue("arraySize", this.arraySize);
			info.AddValue("endValue", this.endValue);
			info.AddValue("startValue", this.startValue);			
			info.AddValue("counter", this.counter);
		}
		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected Counter(SerializationInfo info, StreamingContext context) : base(info, context)
		{				
			this.outConnector = info.GetValue("outConnector", typeof(Connector)) as Connector;			
			this.outConnector.BelongsTo = this;
			this.Connectors.Add(outConnector);

			this.endValue = info.GetInt32("endValue");
			this.startValue = info.GetInt32("startValue");
			this.arraySize = info.GetInt32("arraySize");
			this.counter = info.GetInt32("counter");

		}

		#endregion

		#region Methods
		/// <summary>
		/// Initializes the shape
		/// </summary>
		protected  override void InitEntity()
		{
			base.InitEntity ();
			this.IsResizable=false;
			
			
			
		}
		/// <summary>
		/// Intitlizes the automata shape
		/// </summary>
		public override void InitAutomata()
		{
			counter=startValue;			
		}
		/// <summary>
		/// the painting of the shape
		/// </summary>
		/// <param name="g"></param>
		public override void Paint(System.Drawing.Graphics g)
		{
			
			//g.DrawRectangle(new Pen(Color.Black,IsSelected ? 2F : 1F),Rectangle.Left,Rectangle.Top,Rectangle.Width,Rectangle.Height);
			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			
			g.FillRectangle(new SolidBrush(this.ShapeColor), Rectangle.X, Rectangle.Y, Rectangle.Width , 12 );
			Color Background = IsSelected ? Color.LightSteelBlue : Color.WhiteSmoke;
			g.FillRectangle(new SolidBrush(Background), Rectangle.X, Rectangle.Y+12, Rectangle.Width , Rectangle.Height-12 );					
			g.DrawString("Counter", Font, new SolidBrush(TextColor), Rectangle.Left + (Rectangle.Width / 2), Rectangle.Top , sf);
			sf.Alignment = StringAlignment.Far;
			if(counter==0)
				g.DrawString("Output", Font, new SolidBrush(Color.Black), Rectangle.Right -2, Rectangle.Top +2*(Rectangle.Height/3)-7, sf);
			else
				g.DrawString(counter.ToString(), Font, new SolidBrush(Color.Black), Rectangle.Right -2, Rectangle.Top +2*(Rectangle.Height/3)-7, sf);

			

		}
		
	
		/// <summary>
		/// Returns the connector locations
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public override System.Drawing.PointF ConnectionPoint(Connector c)
		{
			
			RectangleF r = Rectangle;
			if (c == outConnector) return new PointF(r.Right, r.Top+(r.Height/2));
			return base.ConnectionPoint (c);
			//return new PointF(0, 0);
		}
		/// <summary>
		/// Updates the automata shape to the next state
		/// </summary>
		public override void Update()
		{
			//clear the sends values
			if (counter<=endValue)
			{
				this.outConnector.Sends.Clear();
				this.outConnector.Receives.Clear();
				for(int k=0; k<arraySize; k++)	this.outConnector.Sends.Add(counter);
				counter++;			
			}
			

		}

		#region Accessible propertygrid properties
		public override void AddProperties()
		{
			base.AddProperties ();
			Bag.Properties.Add(new PropertySpec("StartValue",typeof(int),"Automata","The start value of the counter.",0));
			Bag.Properties.Add(new PropertySpec("EndValue",typeof(int),"Automata","The end value of the counter.",0));
			Bag.Properties.Add(new PropertySpec("ArraySize",typeof(int),"Automata","The array size to be outputted.",1));
		}

		protected override void SetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			base.SetPropertyBagValue (sender, e);
			switch(e.Property.Name)
			{
				case "StartValue":
					this.startValue = (int) e.Value; break;
				case "EndValue":
					if(startValue <= (int) e.Value)
						this.endValue = (int) e.Value;
					else
						e.Value = this.endValue;
					break;
				case "ArraySize":
					this.arraySize = (int) e.Value; break;
			}
		}

		protected override void GetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			base.GetPropertyBagValue (sender, e);
			switch(e.Property.Name)
			{
				case "StartValue":
					e.Value = this.startValue; break;
				case "EndValue":					
					e.Value =this.endValue;
					break;
				case "ArraySize":
					e.Value = this.arraySize; break;
			}
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
				Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.Automatology.Resources.Counter.gif");
					
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
