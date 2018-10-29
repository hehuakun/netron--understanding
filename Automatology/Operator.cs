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

namespace Netron.Automatology
{
	/// <summary>
	/// Implements basic math operations
	/// </summary>
	[Serializable]
	[Description("Operator")]
	[NetronGraphShape("Operator","E50899E4-2BB9-415f-A23B-5EA6A3840E50","Automata","Netron.Automatology.Operator","Executes mathematical operations on input data.")]
	public class Operator : Shape
	{	
		#region Fields
		/// <summary>
		/// the in connector
		/// </summary>
		private Connector outConnector;
		/// <summary>
		/// the operand1 connector
		/// </summary>
		private Connector operand1Connector;
		/// <summary>
		/// the operand2 connector
		/// </summary>
		private Connector operand2Connector;

		/// <summary>
		/// which operation
		/// </summary>
		private BasicMathOperator operatorType=BasicMathOperator.Times;

		private object calculated;
		/// <summary>
		/// the output type
		/// </summary>
		private AutomataDataType outputType=AutomataDataType.Double;
		
		/// <summary>
		/// the delegate to the operation
		/// </summary>
		private delegate object OperatorFunctionDelegate(object A,object B);
		/// <summary>
		/// the pointer to the operation
		/// </summary>
		[NonSerialized] private OperatorFunctionDelegate operatorFunction;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the operator
		/// </summary>
		public BasicMathOperator OperatorType
		{
			get{return operatorType;}
			set
			{
				switch(value)
				{
					case BasicMathOperator.Times:
						operatorFunction=new OperatorFunctionDelegate(Times);
						break;
					case BasicMathOperator.Divide:
						break;

				}
				
				operatorType=value;
			}
		}
		/// <summary>
		/// Gets or sets the output type
		/// </summary>
		public AutomataDataType OutputType
		{
			get{return outputType;}
			set{outputType=value;}
		}
		#endregion
		
		#region Constructor
		/// <summary>
		/// the ctor
		/// </summary>
		public Operator():base()
			
		{
			this.IsResizable=false;
			Rectangle = new RectangleF(0, 0, 120, 50);
		
			outConnector=new Connector(this,"Output",true);
			outConnector.ConnectorLocation = ConnectorLocation.East;
			operand1Connector = new Connector(this,"Operand A",false);	
			operand1Connector.ConnectorLocation = ConnectorLocation.West;
			operand2Connector = new Connector(this,"Operand B",false);
			operand2Connector.ConnectorLocation = ConnectorLocation.West;

			this.Connectors.Add(outConnector);
			this.Connectors.Add(operand1Connector);
			this.Connectors.Add(operand2Connector);
			
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
			info.AddValue("operand1Connector", this.operand1Connector, typeof(Connector));
			info.AddValue("operand2Connector", this.operand2Connector, typeof(Connector));

			info.AddValue("operatorType", operatorType, typeof(BasicMathOperator));
			info.AddValue("outputType", this.outputType, typeof(AutomataDataType));
			
		}
		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected Operator(SerializationInfo info, StreamingContext context) : base(info, context)
		{			
			this.outConnector = info.GetValue("outConnector", typeof(Connector)) as Connector;
			this.outConnector.BelongsTo = this;
			this.Connectors.Add(outConnector);
			
			this.operand1Connector = info.GetValue("operand1Connector", typeof(Connector)) as Connector;
			this.operand1Connector.BelongsTo = this;
			this.Connectors.Add(operand1Connector);

			this.operand2Connector = info.GetValue("operand2Connector", typeof(Connector)) as Connector;
			this.operand2Connector.BelongsTo = this;
			this.Connectors.Add(operand2Connector);
			
			this.OperatorType = (BasicMathOperator) info.GetValue("operatorType", typeof(BasicMathOperator));
			this.outputType = (AutomataDataType) info.GetValue("outputType", typeof(AutomataDataType));
			

		}
		#endregion

		#region Methods

		/// <summary>
		/// Initializes the shape
		/// </summary>
		protected  override void InitEntity()
		{
			base.InitEntity ();
			operatorFunction=new OperatorFunctionDelegate(Times);
		}

		#region Access to the propertygrid properties
		public override void AddProperties()
		{
			base.AddProperties ();
			Bag.Properties.Add(new PropertySpec("Operation",typeof(BasicMathOperator),"Automata","The operation to be used.", BasicMathOperator.Times));
			Bag.Properties.Add(new PropertySpec("OutputType",typeof(AutomataDataType),"Automata","The data type to be outputted.",AutomataDataType.Double));
		}

		protected override void GetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			base.GetPropertyBagValue (sender, e);
			switch(e.Property.Name)
			{
				case "Operation":
					e.Value = this.operatorType; break;
				case "OutputType":
					e.Value = this.outputType; break;
			}
		}
		protected override void SetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			base.SetPropertyBagValue (sender, e);
			switch(e.Property.Name)
			{
				case "Operation":
					this.operatorType = (BasicMathOperator) e.Value; break;
				case "OutputType":
					this.outputType = (AutomataDataType) e.Value; break;
			}
		}


		#endregion

		/// <summary>
		/// Implements the times operator (Geri loves Swa a lotttttt....)[women, you know]
		/// </summary>
		/// <param name="A"></param>
		/// <param name="B"></param>
		/// <returns></returns>
		private object Times(object A,object B)
		{
			if(A.GetType().Equals(B.GetType()))
			{
				switch(outputType)
				{
					case AutomataDataType.Integer:
						return ((int) A)*((int) B);
					case AutomataDataType.Double:
						return ((double) A)*((double) B);
					default:
						return null;
				}

			}
			else
				return null;
		}
		/// <summary>
		/// Paints the shape on the canvas
		/// </summary>
		/// <param name="g"></param>
		public override void Paint(System.Drawing.Graphics g)
		{
			
			
			
			RectangleF r = Rectangle;

			g.DrawRectangle(new Pen(Color.Black,IsSelected ? 2F : 1F),r.Left,r.Top,r.Width,r.Height);
			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			
			g.FillRectangle(new SolidBrush(ShapeColor), r.X, r.Y, r.Width , 12 );
			Color Background = IsSelected ? Color.LightSteelBlue : Color.WhiteSmoke;
			g.FillRectangle(new SolidBrush(Background), r.X, r.Y+12, r.Width , r.Height-12 );					
			g.DrawString( OperatorType.ToString() + " (" + outputType.ToString() + ")", Font, new SolidBrush(this.TextColor), r.Left + (r.Width / 2), r.Top , sf);
			sf.Alignment = StringAlignment.Far;
			g.DrawString("Output", Font, new SolidBrush(Color.Black), r.Right -2, r.Top +2*(r.Height/3)-7, sf);
			
		}
		
		/// <summary>
		/// Returns the locations of the connectors
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public override System.Drawing.PointF ConnectionPoint(Connector c)
		{
			
			RectangleF r = Rectangle;
			if (c == operand1Connector) return new PointF(r.Left, r.Top+32);
			if (c == operand2Connector) return new PointF(r.Left, r.Top+42);
			if (c == outConnector) return new PointF(r.Right, r.Top+25);
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
			try
			{
				if (operand1Connector.Receives.Count>0 && operand2Connector.Receives.Count>0)
				{
					IEnumerator Aenumer=operand1Connector.Receives.GetEnumerator();
					IEnumerator Benumer=operand2Connector.Receives.GetEnumerator();

					while(Aenumer.MoveNext() && Benumer.MoveNext())
					{
						if(Aenumer.Current!=null && Benumer.Current!=null)
						{
							calculated=operatorFunction(Aenumer.Current,Benumer.Current);
							if(calculated !=null)
								this.outConnector.Sends.Add(calculated);

						}
					}
					
				}
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.Message);
			}
			this.operand1Connector.Receives.Clear();
			this.operand2Connector.Receives.Clear();
			
			//base.Update ();

		}


		/// <summary>
		/// Overrides the default thumbnail used in the shape viewer
		/// </summary>
		/// <returns></returns>
		public override Bitmap GetThumbnail()
		{
			Bitmap bmp=null;
			try
			{
				Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.Automatology.Resources.Operator.gif");
					
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
