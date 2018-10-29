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
	/// This automata shape allows an affine (aka linear)  mapping of data
	/// </summary>
	[Serializable]
	[Description("Affine Transformation")]
	[NetronGraphShape("Affine map","7A231785-4198-4828-9BA9-24895BE54A59","Automata","Netron.Automatology.AffineTransformation",
		 "Scales and shift the given input.")]
	public class AffineTransformation : Shape, ISerializable
	{
		#region Fields
		/// <summary>
		/// the data shift
		/// </summary>
		protected float shiftValue=0;
		/// <summary>
		/// the scaling value
		/// </summary>
		protected float scalingValue=100;		
		/// <summary>
		/// the out connector
		/// </summary>
		protected Connector outConnector;
		/// <summary>
		/// the in connector
		/// </summary>
		protected Connector inConnector;
		/// <summary>
		/// the kinda data type send out
		/// </summary>
		protected Netron.GraphLib.AutomataDataType outputType;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the shift value
		/// </summary>
		public float ShiftValue
		{
			get{return shiftValue;}
			set{shiftValue=value;}
		}
		/// <summary>
		/// Gets or set the scaling of the incoming data
		/// </summary>
		public float ScalingValue
		{
			get{return scalingValue;}
			set{scalingValue=value;}
		}
		#endregion


		#region Constructor

		/// <summary>
		/// the ctor
		/// </summary>
		public AffineTransformation():base()			
		{
			Rectangle = new RectangleF(0, 0, 100, 40);			
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
			info.AddValue("outConnector", this.outConnector, typeof(Connector));
			
			info.AddValue("outputType", this.outputType, typeof(AutomataDataType));
			
			info.AddValue("shiftValue", this.shiftValue);
			info.AddValue("scalingValue", this.scalingValue);
			
		}
		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected AffineTransformation(SerializationInfo info, StreamingContext context) : base(info, context)
		{			
			outConnector = (Connector) info.GetValue("outConnector", typeof(Connector));
			outConnector.BelongsTo = this;
			Connectors.Add(outConnector);		

			inConnector = (Connector) info.GetValue("inConnector", typeof(Connector));
			inConnector.BelongsTo = this;
			Connectors.Add(inConnector);	

			outputType = (AutomataDataType) info.GetValue("outputType", typeof(AutomataDataType));

			shiftValue = info.GetInt32("shiftValue");

			scalingValue = info.GetInt32("scalingValue");			
		}
		#endregion

		#region Methods

		/// <summary>
		/// Initalizes the shape
		/// </summary>
		protected override void InitEntity()
		{
			base.InitEntity();
			this.IsResizable=false;
			
		}

		/// <summary>
		/// How the shape is drawn on the canvas
		/// </summary>
		/// <param name="g"></param>
		public override void Paint(System.Drawing.Graphics g)
		{
			
			//g.DrawRectangle(new Pen(Color.Black,IsSelected ? 2F : 1F),Rectangle.Left,Rectangle.Top,Rectangle.Width,Rectangle.Height);
			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			
			g.FillRectangle(new SolidBrush(ShapeColor), Rectangle.X, Rectangle.Y, Rectangle.Width , 12 );
			Color Background = IsSelected ? Color.LightSteelBlue : Color.WhiteSmoke;
			g.FillRectangle(new SolidBrush(Background), Rectangle.X, Rectangle.Y+12, Rectangle.Width , Rectangle.Height-12 );					
			g.DrawString("Affine map", Font, new SolidBrush(TextColor), Rectangle.Left + (Rectangle.Width / 2), Rectangle.Top , sf);
			sf.Alignment = StringAlignment.Far;
			g.DrawString("Output", Font, new SolidBrush(Color.Black), Rectangle.Right -5,  Rectangle.Top+(Rectangle.Height/2)-5, sf);
			sf.Alignment = StringAlignment.Near;
			g.DrawString("Input", Font, new SolidBrush(Color.Black), Rectangle.Left +5,  Rectangle.Top+(Rectangle.Height/2)-5, sf);
			

		}
		
	
		/// <summary>
		/// Returns the locations of the connecorts
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
		/// Updates the automaton to the next state
		/// </summary>
		public override void Update()
		{
			this.outConnector.Sends.Clear();		
			if (inConnector.Receives.Count>0)
			{
				IEnumerator enumer = inConnector.Receives.GetEnumerator();				
				switch(outputType)
				{
					case AutomataDataType.Integer:
							
						while(enumer.MoveNext())
						{
							try
							{							
								outConnector.Sends.Add(Convert.ToInt32(shiftValue+scalingValue*Convert.ToDouble( enumer.Current)));							
							}
							catch(Exception)
							{
								continue;
							}
						}
						break;
					case AutomataDataType.Double:
						while(enumer.MoveNext())
						{								
							try
							{
								outConnector.Sends.Add(Convert.ToDouble(shiftValue+scalingValue*Convert.ToDouble(enumer.Current)));							
							}
							catch{continue;}
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
			Bag.Properties.Add(new PropertySpec("ShiftValue",typeof(float),"Automata","The translational value of the mapping.",50));
			Bag.Properties.Add(new PropertySpec("ScalingValue",typeof(float),"Automata","The scaling value of the mapping.",1));
			Bag.Properties.Add(new PropertySpec("OutputType", typeof(AutomataDataType),"Automata","The output type after mapping.",AutomataDataType.Double));
		}

		protected override void SetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			base.SetPropertyBagValue (sender, e);
			switch(e.Property.Name)
			{
				case "ShiftValue":
					this.shiftValue = (float) e.Value; break;
				case "ScalingValue":
					
					this.scalingValue = (float) e.Value;
					
					break;
				case "OutputType":
					this.outputType = (AutomataDataType) e.Value; break;
			}
		}

		protected override void GetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			base.GetPropertyBagValue (sender, e);
			switch(e.Property.Name)
			{
				case "ShiftValue":
					e.Value = this.shiftValue; break;
				case "ScalingValue":					
					e.Value =this.scalingValue;
					break;
				case "OutputType":
					e.Value = this.outputType; break;
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
				Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.Automatology.Resources.AffineMap.gif");
					
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
