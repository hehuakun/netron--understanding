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
using Netron.GraphLib.Maths;
using System.Text;

namespace Netron.Automatology
{
	/// <summary>
	/// Versatile random generator automata shape
	/// </summary>
	[Serializable]
	[Description("Random generator")]
	[NetronGraphShape("Random generator","2DAB2E56-3E62-43bc-A188-E043ECB09701","Automata","Netron.Automatology.RandomGenerator",
		 "Generates random data.")]
	public class RandomGenerator : Shape, ISerializable
	{

		#region Fields
		/// <summary>
		/// the random generator
		/// </summary>
		protected MersenneTwister MT;		
		/// <summary>
		/// the out connector
		/// </summary>
		protected Connector outConnector;
		/// <summary>
		/// holder of the color
		/// </summary>
		//protected Color _color = Color.White;

		/// <summary>
		/// the output type
		/// </summary>
		protected AutomataDataType outDataType=AutomataDataType.Integer;
		/// <summary>
		/// whether outputting positive values
		/// </summary>
		protected bool positive=false;
		/// <summary>
		/// the amplitude of the random numbers
		/// </summary>
		protected int amplitude = 100;
		/// <summary>
		/// grayscale colors
		/// </summary>
		protected bool grayScale=false;
		/// <summary>
		/// the size of the array
		/// </summary>
		protected int arraySize=2;
		/// <summary>
		/// shoot once?
		/// </summary>
		protected bool once=false;
		/// <summary>
		/// whether already shot
		/// </summary>
		protected bool shot;

		#endregion

		#region Constructor

		/// <summary>
		/// default constructor
		/// </summary>
		public RandomGenerator():base()
			
		{
			this.IsResizable=false;
			Rectangle = new RectangleF(0, 0, 70, 40);
			
			outConnector=new Connector(this,"Output",true);
			outConnector.ConnectorLocation = ConnectorLocation.East;
			this.Connectors.Add(outConnector);
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
			info.AddValue("outDataType", this.outDataType, typeof(AutomataDataType));
			info.AddValue("positive", this.positive);
			info.AddValue("amplitude", this.amplitude);
			info.AddValue("grayScale", this.grayScale);
			info.AddValue("arraySize", this.arraySize);
			info.AddValue("once", this.once);
		}
		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected RandomGenerator(SerializationInfo info, StreamingContext context) : base(info, context)
		{			
			outConnector = (Connector) info.GetValue("outConnector", typeof(Connector));
			outConnector.BelongsTo = this;
			Connectors.Add(outConnector);		

			outDataType = (AutomataDataType) info.GetValue("outDataType", typeof(AutomataDataType));

			positive = info.GetBoolean("positive");

			amplitude = info.GetInt32("amplitude");

			grayScale = info.GetBoolean("grayScale");

			arraySize = info.GetInt32("arraySize");

			once = info.GetBoolean("once");
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets whether the generator should only fire once
		/// </summary>
		[Browsable(false)]
		public bool Once
		{
			get  {return once;}
			set{once=value;}
		}
		/// <summary>
		/// Gets or sets the size of the array
		/// </summary>
		[Browsable(false)]
		public int ArraySize
		{
			get{return arraySize;}
			set{arraySize=value;}
		}
		/// <summary>
		/// Gets or sets whether only gray-scale colors should be served
		/// </summary>
		[Browsable(false)]
		public bool GrayScale
		{
			get{return grayScale;}
			set{grayScale=value;}
		}
		/// <summary>
		/// Gets or sets whether to take only positive numbers
		/// </summary>
		[Browsable(false)]
		public bool Positive
		{
			get{return positive;}
			set{positive=value;}
		}
		/// <summary>
		/// Gets or sets the data type that the generator generates
		/// </summary>
		public AutomataDataType DataType
		{
			get{return outDataType;}
			set{outDataType=value;}
		}
		/// <summary>
		/// Gets or sets the amplitude of the random values
		/// </summary>
		[Browsable(false)]
		public int Amplitude
		{
			get{return amplitude;}
			set{amplitude=value;}
		}
		
		#endregion

		#region Methods

		#region Access the propertygrid properties
		/// <summary>
		/// Adds the propertygrid visible properties
		/// </summary>
		public override void AddProperties()
		{
			base.AddProperties ();
			//doesn't make sense to have the Text prop in this shape
			Bag.Properties.Remove("Text");
			//the amplitude
			Bag.Properties.Add(new PropertySpec("Amplitude",typeof(int),"Automata","Gets or sets the amplitude of the random data",1));
			//the data type
			Bag.Properties.Add(new PropertySpec("DataType",typeof(AutomataDataType),"Automata","Gets or sets the type of data the generator serves",AutomataDataType.Integer));
			//positive
			Bag.Properties.Add(new PropertySpec("Positive",typeof(bool),"Automata","Gets or sets whether only positive values should be generated (if applicable).",false));
			//gray-scale
			Bag.Properties.Add(new PropertySpec("GrayScale",typeof(bool),"Automata","Gets or sets whether only gray-scale colors should be generated (if applicable).",false));
			//array size
			Bag.Properties.Add(new PropertySpec("ArraySize",typeof(int),"Automata","Gets or sets the size of the arry generated.",10));

		}

		/// <summary>
		/// Gets the values of the propertygrid properties
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void GetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			base.GetPropertyBagValue (sender, e);

			switch(e.Property.Name)
			{
				case "Amplitude": e.Value= this.amplitude; break;
				case "DataType": e.Value = (AutomataDataType) this.outDataType; break;
				case "Positive": e.Value = this.positive; break;
				case "GrayScale": e.Value = this.grayScale; break;
				case "ArraySize": e.Value = this.arraySize; break;

			}
		}

		/// <summary>
		/// Sets the values of the propertygrid properties
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void SetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			base.SetPropertyBagValue (sender, e);
			switch(e.Property.Name)
			{
				case "Amplitude": 
					this.amplitude = (int) e.Value; break;
				case "DataType": 
					this.outDataType = (AutomataDataType) e.Value; break;
				case "Positive":
					this.positive = (bool) e.Value; break;
				case "GrayScale":
					this.grayScale = (bool) e.Value; break;
				case "ArraySize":
					this.arraySize = (int) e.Value; break;


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
				Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.Automatology.Resources.RandomGenerator.gif");
					
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
		/// <summary>
		/// Paints the shape on the canvas
		/// </summary>
		/// <param name="g"></param>
		public override void Paint(System.Drawing.Graphics g)
		{	
			StringFormat sf = new StringFormat();
			SizeF ss=g.MeasureString("Random",Font);

			//the title part
			g.FillRectangle(new SolidBrush(ShapeColor), Rectangle.X, Rectangle.Y, Rectangle.Width , 12 );
			sf.Alignment = StringAlignment.Center;
			g.DrawString("Random",Font, new SolidBrush(Color.Black), Rectangle.Left + (Rectangle.Width / 2), Rectangle.Top , sf);
			//the data part
			Color Background = IsSelected ? Color.LightSteelBlue : Color.WhiteSmoke;
			g.FillRectangle(new SolidBrush(Background), Rectangle.X, Rectangle.Y+12, Rectangle.Width , Rectangle.Height-12 );								
			sf.Alignment=StringAlignment.Far;
			g.DrawString(outDataType.ToString(), Font, new SolidBrush(Color.Black), Rectangle.Right -2, Rectangle.Top +2*(Rectangle.Height/3)-7, sf);

			//the rectangle around the shape
			//g.DrawRectangle(blackpen,Rectangle.Left,Rectangle.Top,Rectangle.Width,Rectangle.Height);
			
		}
		/// <summary>
		/// Initializes the automata
		/// </summary>
		public override void InitAutomata()
		{
			shot=true;//single shot of data or multiple

		}

		/// <summary>
		/// Initializes the shape
		/// </summary>
		protected  override void InitEntity()
		{
			base.InitEntity ();
			MT=new MersenneTwister();
		}

	
		/// <summary>
		/// Returns the locations of the connectors
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public override System.Drawing.PointF ConnectionPoint(Connector c)
		{
			
			RectangleF r = Rectangle;
			if (c == outConnector) return new PointF(r.Right, r.Top+2*(r.Height/3));
			return base.ConnectionPoint (c);
			//return new PointF(0, 0);
		}
		/// <summary>
		/// Updates the automata to the next state
		/// </summary>
		public override void Update()
		{
			//clear the sends values
			this.outConnector.Sends.Clear();
			this.outConnector.Receives.Clear();
			
			if(once && !shot) return;
			if(once && shot)	shot=false;
			

			for(int counter = 0; counter<arraySize; counter++)
			{
				switch(outDataType)
				{
					case AutomataDataType.Bool:
						if(arraySize>0)
						{
							for(int k=0;k<arraySize;k++)
							{							
								outConnector.Sends.Add(MT.Next(0,1)==1? true: false);
							}
						}
						break;
					case AutomataDataType.Color:
						if(grayScale)
						{
							int rgb=MT.Next(255);
							outConnector.Sends.Add(Color.FromArgb(rgb,rgb,rgb));
						}
						else
						{
							outConnector.Sends.Add(Color.FromArgb(MT.Next(255),MT.Next(255),MT.Next(255)));
						}
						break;
					case AutomataDataType.DateTime:
						TimeSpan span = (DateTime.MaxValue - DateTime.MinValue);
						TimeSpan rnd=  TimeSpan.FromTicks(Convert.ToInt64( span.Ticks *MT.NextFloatPositive()));						
						outConnector.Sends.Add(DateTime.MinValue + rnd);
						break;
					case AutomataDataType.Degree:						
						outConnector.Sends.Add(MT.Next(0,amplitude));
						break;
					case AutomataDataType.Double:
						if(positive)
							outConnector.Sends.Add(MT.NextDouble()*amplitude);
						else
							outConnector.Sends.Add(-amplitude + 2*MT.NextDouble()*amplitude);
						break;					
					case AutomataDataType.Integer:
						if(positive)
							outConnector.Sends.Add(MT.Next(amplitude));						
						else
							outConnector.Sends.Add(MT.Next(-amplitude,amplitude));						
						break;
					case AutomataDataType.Object:
						outConnector.Sends.Add(null);
						break;
					case AutomataDataType.Radians:
						outConnector.Sends.Add(MT.NextFloatPositive()*Math.PI * amplitude);			
						break;
					case AutomataDataType.String:
						if (amplitude>0)
						{
							StringBuilder concat = new StringBuilder();
							for(int k=0;k<amplitude;k++)
							{							
								concat.Append( (char) MT.Next(97,122));
							}
							outConnector.Sends.Add(concat.ToString());
						}
						break;
					case AutomataDataType.Vector:
						outConnector.Sends.Add(new NetronVector(MT.NextDouble()*amplitude,MT.NextDouble()*amplitude,MT.NextDouble()*amplitude));
						break;

				

					
				}
			}
				

		}



		#endregion

	}
	
}
