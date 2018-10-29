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
using Netron.GraphLib.Maths;
namespace Netron.Automatology
{
	/// <summary>
	/// An  automata shape to map data using basic mathematical functions
	/// </summary>
	[Serializable]
	[Description("Basic function")]
	[NetronGraphShape("Basic function","7663EFF7-033E-4665-AEBA-69F8C31710EE","Automata","Netron.Automatology.BasicFunction","Executes mathematical mappings on input data.")]
	public class BasicFunction : Shape, ISerializable
	{
		
		#region Delegates
		/// <summary>
		/// the delegated function which will perform the actual mathematical mapping
		/// </summary>
		
		protected delegate double MathFunction(double input);		
		#endregion

		#region Fields
		/// <summary>
		/// the out connector
		/// </summary>
		protected Connector outConnector;
		/// <summary>
		/// the in connector
		/// </summary>
		protected Connector inConnector;
		/// <summary>
		/// the frequency value
		/// </summary>
		protected double mFrequency=1D;
		/// <summary>
		/// the amplitude of the mapping
		/// </summary>
		protected double mAmplitude=1D;
		/// <summary>
		/// the pointer to the function
		/// </summary>
		[NonSerialized]
		protected MathFunction mFunction;
		/// <summary>
		/// the returned data type
		/// </summary>
		
		protected AutomataDataType mOutputDataType=AutomataDataType.Double;

		/// <summary>
		/// the value of the function chosen
		/// </summary>
		private BasicMathFunction  basicMathFunction=BasicMathFunction.Cos;		
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the mapping function
		/// </summary>
		[Browsable(false)]		
		protected BasicMathFunction Function
		{
			get{return basicMathFunction;} 
			set
			{
				switch(value)
				{
					case BasicMathFunction.Cos:
						mFunction = new MathFunction(Cos);
						basicMathFunction=BasicMathFunction.Cos;
						break;
					case BasicMathFunction.Sin:
						mFunction = new MathFunction(Sin);
						basicMathFunction=BasicMathFunction.Sin;
						break;
					case BasicMathFunction.Abs:
						mFunction = new MathFunction(Abs);
						basicMathFunction=BasicMathFunction.Abs;
						break;
					case BasicMathFunction.ACos:
						mFunction = new MathFunction(ACos);
						basicMathFunction = BasicMathFunction.ACos;
						break;
					case BasicMathFunction.ASin:
						mFunction = new MathFunction(ASin);
						basicMathFunction = BasicMathFunction.ASin;
						break;
					case BasicMathFunction.ATan:
						mFunction = new MathFunction(ATan);
						basicMathFunction = BasicMathFunction.ATan;
						break;
					case BasicMathFunction.Cosh:
						mFunction = new MathFunction(Cosh);
						basicMathFunction = BasicMathFunction.Cosh;
						break;
					case BasicMathFunction.Exp:
						mFunction = new MathFunction(Exp);
						basicMathFunction = BasicMathFunction.Exp;
						break;
					case BasicMathFunction.Log:
						mFunction = new MathFunction(Log);
						basicMathFunction = BasicMathFunction.Log;
						break;
					case BasicMathFunction.Sinh:
						mFunction = new MathFunction(Sinh);
						basicMathFunction = BasicMathFunction.Sinh;
						break;
					case BasicMathFunction.Tan:
						mFunction = new MathFunction(Tan);
						basicMathFunction = BasicMathFunction.Tan;
						break;
					case BasicMathFunction.Tanh:
						mFunction = new MathFunction(Tanh);
						basicMathFunction = BasicMathFunction.Tanh;
						break;
				}
			}
			
			
		}

		/// <summary>
		/// Gets or sets the output data type
		/// </summary>
		[Browsable(false)]
		public AutomataDataType OutputDataType
		{
			get{return mOutputDataType;}
			set{mOutputDataType=value;}
		}
		
		/// <summary>
		/// Gets or sets the frequency
		/// </summary>
		[Browsable(false)]
		public double Frequency 
		{
			get{return mFrequency;} 
			set{mFrequency=value;}
		}
		/// <summary>
		/// Gets or sets the amplitude
		/// </summary>
		[Browsable(false)]
		public double Amplitude 
		{
			get{return mAmplitude;} 
			set{mAmplitude=value;}
		}
		#endregion
		
		#region Constructor

		/// <summary>
		/// the ctor
		/// </summary>
		public BasicFunction():base()
			
		{
			IsResizable=false;
			Rectangle = new RectangleF(0, 0, 70, 30);
		
			outConnector=new Connector(this,"Output",true);
			outConnector.ConnectorLocation = ConnectorLocation.East;
			inConnector = new Connector(this,"Input",false);
			inConnector.ConnectorLocation = ConnectorLocation.West;
			this.Connectors.Add(outConnector);
			this.Connectors.Add(inConnector);
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
			info.AddValue("inConnector", this.inConnector, typeof(Connector));
			info.AddValue("outConnector", this.inConnector, typeof(Connector));
			info.AddValue("mOutputDataType", this.mOutputDataType, typeof(AutomataDataType));
			info.AddValue("basicMathFunction", this.basicMathFunction, typeof(BasicMathFunction));
			info.AddValue("mFrequency", this.mFrequency);
			info.AddValue("mAmplitude", this.mAmplitude);			
		}
		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected BasicFunction(SerializationInfo info, StreamingContext context) : base(info, context)
		{			
			this.inConnector = info.GetValue("inConnector", typeof(Connector)) as Connector;
			this.outConnector = info.GetValue("outConnector", typeof(Connector)) as Connector;
			this.OutputDataType = (AutomataDataType) info.GetValue("mOutputDataType", typeof(AutomataDataType)) ;
			this.Function = (BasicMathFunction)  info.GetValue("basicMathFunction", typeof(BasicMathFunction));
			this.Frequency = info.GetDouble("mFrequency");
			this.Amplitude = info.GetDouble("mApmlitude");

		}

		#endregion

		#region Methods

		protected  override void InitEntity()
		{
			base.InitEntity ();
			mFunction=new MathFunction(Cos);
		}



		#region Maths implementation

		//TODO; the rest of the math functions
		private double Cos(double input){return mAmplitude*Math.Cos(mFrequency*input);}
		private double Sin(double input){return mAmplitude*Math.Sin(mFrequency*input);}
		private double Abs(double input){return Math.Abs(input);}
		private double ACos(double input){return Math.Acos(input);}
		private double ASin(double input){return Math.Asin(input);}
		private double ATan(double input){return Math.Atan(input);}
		private double Cosh(double input){return Math.Cosh(input);}
		private double Exp(double input){return Math.Exp(input);}
		private double Tan(double input){return Math.Tan(input);}
		private double Tanh(double input){return Math.Tanh(input);}
		private double Log(double input){return Math.Log(input);}
		private double Sinh(double input){return Math.Sinh(input);}
		#endregion

		/// <summary>
		/// the way the shape is drawn on the canvas
		/// </summary>
		/// <param name="g"></param>
		
		public override void Paint(System.Drawing.Graphics g)
		{
			
			SizeF ss=g.MeasureString("Random",Font);
			
			

			g.DrawRectangle(new Pen(Color.Black,IsSelected ? 2F : 1F),Rectangle.Left,Rectangle.Top,Rectangle.Width,Rectangle.Height);
			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			
			g.FillRectangle(new SolidBrush(ShapeColor), Rectangle.X, Rectangle.Y, Rectangle.Width , 12 );
			Color Background = IsSelected ? Color.LightSteelBlue : Color.WhiteSmoke;
			g.FillRectangle(new SolidBrush(Background), Rectangle.X, Rectangle.Y+12, Rectangle.Width , Rectangle.Height-12 );					
			g.DrawString(basicMathFunction.ToString(), Font, new SolidBrush(this.TextColor), Rectangle.Left + (Rectangle.Width / 2), Rectangle.Top , sf);
			sf.Alignment=StringAlignment.Far;
			g.DrawString("Output", Font, new SolidBrush(Color.Black), Rectangle.Right -2, Rectangle.Top +2*(Rectangle.Height/3)-7, sf);

		}
		
		/// <summary>
		/// the locations of the connectors
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public override System.Drawing.PointF ConnectionPoint(Connector c)
		{
			
			RectangleF r = Rectangle;
			if (c == outConnector) return new PointF(r.Right, r.Top+(r.Height/2));
			if (c == inConnector) return new PointF(r.Left, r.Top+(r.Height/2));
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
			if (inConnector.Receives.Count>0)
			{
				IEnumerator enumer = inConnector.Receives.GetEnumerator();
				switch(mOutputDataType)
				{
					case AutomataDataType.Integer:
							
						while(enumer.MoveNext())
						{								
							outConnector.Sends.Add(mFunction(Convert.ToInt32(enumer.Current)));							
						}
						break;
					case AutomataDataType.Double:
						while(enumer.MoveNext())
						{								
							outConnector.Sends.Add(mFunction(Convert.ToDouble(enumer.Current)));							
						}
						break;
					default:
						break;
				}


					
					
			}
			this.inConnector.Receives.Clear();
			


		}

		#region Accessible propertygrid properties
		public override void AddProperties()
		{
			base.AddProperties ();
			Bag.Properties.Add(new PropertySpec("Amplitude",typeof(double),"Automata","The amplitude of the mappingr.",1D));
			Bag.Properties.Add(new PropertySpec("Frequency",typeof(double),"Automata","The frequency of the mappingr.",1D));
			Bag.Properties.Add(new PropertySpec("Function",typeof(BasicMathFunction),"Automata","The mathematical function which will map the data.",BasicMathFunction.Cos));
		}

		protected override void SetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			base.SetPropertyBagValue (sender, e);
			switch(e.Property.Name)
			{
				case "Amplitude":
					this.mAmplitude = (int) e.Value; break;
				case "Frequency":
					if(this.mFrequency <= (double) e.Value)
						this.mFrequency = (double) e.Value;
					else
						e.Value = this.mFrequency;
					break;
				case "Function":
					this.Function = (BasicMathFunction) e.Value; break;
			}
		}

		protected override void GetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			base.GetPropertyBagValue (sender, e);
			switch(e.Property.Name)
			{
				case "Amplitude":
					e.Value = this.mAmplitude; break;
				case "Frequency":					
					e.Value =this.mFrequency;
					break;
				case "Function":
					e.Value = this.basicMathFunction; break;
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
				Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.Automatology.Resources.BasicFunction.gif");
					
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
