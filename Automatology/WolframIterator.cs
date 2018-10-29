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
	/// Implements the Wolfram automata
	/// </summary>
	[Serializable]
	[Description("Wolfram iterator")]
	[NetronGraphShape("Wolfram iterator","0FD4F746-1037-45f1-80FC-1EF652CBF7BC","Automata","Netron.Automatology.WolframAutomaton",
		 "Iterates an algorithm over a bit array according to the Wolfram principle")]
	public class WolframAutomaton : Shape, ISerializable
	{	
		#region Fields
		/// <summary>
		/// the out connector
		/// </summary>
		private Connector outConnector;
		/// <summary>
		/// the in connector
		/// </summary>
		private Connector inConnector;
		/// <summary>
		/// whether initialized
		/// </summary>
		private bool initialized;
		/// <summary>
		/// 1-state before memory
		/// </summary>
		private int[] memory;	
		/// <summary>
		/// the rule number
		/// </summary>
		private int ruleNumber=30;
		/// <summary>
		/// the size of the array (you need +2 to have the cyclic periodicity)
		/// </summary>
		private int arraySize=102;
		/// <summary>
		/// which initial state
		/// </summary>
		private AutomataInitialState initialState=AutomataInitialState.SingleDot;

		/// <summary>
		/// the bits to be plotted
		/// </summary>
		private int[] bits;
		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the initial state
		/// </summary>
		public AutomataInitialState InitialState
		{
			get{return initialState;}
			set{initialState=value;}
		}
		
		
		/// <summary>
		/// Gets or sets the array size
		/// </summary>
		public int ArraySize
		{
			get{return arraySize-2;}
			set{arraySize=value+2;}
		}
		/// <summary>
		/// Gets or sets the rule number
		/// </summary>
		public int RuleNumber
		{
			get{return ruleNumber;}
			set{ruleNumber=value;}
		}
		
		#endregion

		#region Constructor

		/// <summary>
		/// the ctor
		/// </summary>
		public WolframAutomaton():base()
			
		{
			this.IsResizable=false;
			Rectangle = new RectangleF(0, 0, 70, 30);
		
			outConnector=new Connector(this,"Output",true);
			outConnector.ConnectorLocation=ConnectorLocation.East;
			inConnector = new Connector(this,"Input",false);
			inConnector.ConnectorLocation=ConnectorLocation.West;
			this.Connectors.Add(outConnector);
			this.Connectors.Add(inConnector);
			
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
			info.AddValue("inConnector", this.inConnector, typeof(Connector));

			info.AddValue("ruleNumber", this.ruleNumber);
			info.AddValue("arraySize", this.arraySize);
			info.AddValue("initialState", this.initialState, typeof(AutomataInitialState));

			
			
		}
		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected WolframAutomaton(SerializationInfo info, StreamingContext context) : base(info, context)
		{			
			this.outConnector = info.GetValue("outConnector", typeof(Connector)) as Connector;
			this.outConnector.BelongsTo = this;
			this.Connectors.Add(outConnector);
			
			this.inConnector = info.GetValue("inConnector", typeof(Connector)) as Connector;
			this.inConnector.BelongsTo = this;
			this.Connectors.Add(inConnector);

			this.ruleNumber = info.GetInt32("ruleNumber");
			this.arraySize = info.GetInt32("arraySize");
			this.initialState = (AutomataInitialState) info.GetValue("initialState", typeof(AutomataInitialState));

		

		}

		#endregion

		#region Methods
		/// <summary>
		/// Paints the shape on the canvas
		/// </summary>
		/// <param name="g"></param>
		public override void Paint(System.Drawing.Graphics g)
		{
			
			SizeF ss=g.MeasureString("Random",Font);
			
			RectangleF r = Rectangle;

			g.DrawRectangle(new Pen(Color.Black,IsSelected ? 2F : 1F),r.Left,r.Top,r.Width,r.Height);
			StringFormat sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			
			g.FillRectangle(new SolidBrush(ShapeColor), r.X, r.Y, r.Width , 12 );
			Color Background = IsSelected ? Color.LightSteelBlue : Color.WhiteSmoke;
			g.FillRectangle(new SolidBrush(Background), r.X, r.Y+12, r.Width , r.Height-12 );					
			g.DrawString("Wolfram", Font, new SolidBrush(this.TextColor), r.Left + (r.Width / 2), r.Top , sf);
			sf.Alignment=StringAlignment.Far;
			g.DrawString("Output", this.Font, new SolidBrush(Color.Black), r.Right -2, r.Top +2*(r.Height/3)-7, sf);

		}
		/// <summary>
		/// Initializes the automata
		/// </summary>
		public override void InitAutomata()
		{
			memory=new int[arraySize];
			
			switch(initialState)
			{
				case AutomataInitialState.SingleDot:
					int k;
					int mid=(int) Math.Ceiling((double)arraySize/2);
					memory[mid]=1;
					initialized=true;
					break;
				case AutomataInitialState.Alternate:
					for(k=0;k<Math.Min(arraySize,inConnector.Receives.Count);k++)
					{
						memory[k+1]=(int)(k % 2);
					}
					memory[0]=memory[k]; //cylindrical boundary conditions
					memory[k+1]=memory[1];
					initialized=true;
					break;
				case AutomataInitialState.Black:
					for(k=0;k<Math.Min(arraySize,inConnector.Receives.Count);k++)
					{
						memory[k+1]=1;
					}
					memory[0]=1; //cylindrical boundary conditions
					memory[k+1]=1;
					initialized=true;
					break;
				case AutomataInitialState.White:
					initialized=true;
					break;

				default:
					initialized=false;//the initialized bit says wether the automaton was initialized with a data string from another plex component
					break;
			}
			//calculate the rule or key
			bits=new int[8];
			string bitstring = Convert.ToString(ruleNumber,2).PadLeft(8,'0');
			for(int k=0;k<8;k++)
			{
				bits[7-k]=int.Parse(bitstring.Substring(k,1));
			}
				
			


		}

		
		/// <summary>
		/// Returns the locations of the connectors
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
			if (initialized) 
			{
				int k;
				//do the NKS tricks
				for(k=0;k<arraySize-2;k++)
				{
					//convert to number
					int position=Convert.ToInt32(memory[k].ToString()+memory[k+1].ToString()+memory[k+2].ToString(),2);
					
					outConnector.Sends.Add((int) bits[position]);
				}
				//re-iterate for the next round
				for(k=0;k<arraySize-2;k++)
				{
					memory[k+1]=(int) outConnector.Sends[k];					
				}
				memory[0]=memory[k]; //cylindrical boundary conditions
				memory[k+1]=memory[1];

			}
			else //get the initial data
			{
				if (inConnector.Receives.Count>0)
				{
					int k;
					try
					{						
						for(k=0;k<Math.Min(arraySize-1,inConnector.Receives.Count);k++)
						{
							memory[k+1]=(int) inConnector.Receives[k];
						}
						memory[0]=memory[k]; //cylindrical boundary conditions
						memory[k+1]=memory[1];
						initialized=true;
					}
					catch(Exception exc)
					{
						Trace.WriteLine(exc.Message);					}
				}
			}
			this.inConnector.Receives.Clear();



		}

		#region Access the propertygrid properties
		public override void AddProperties()
		{
			base.AddProperties ();
			Bag.Properties.Add(new PropertySpec("InitialState", typeof(AutomataInitialState), "Automata", "The initial state of the automataon.", AutomataInitialState.SingleDot));
			Bag.Properties.Add(new PropertySpec("RuleNumber", typeof(int),"Automata","The rule number of the Wolfram automaton."));
		}

		protected override void GetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			base.GetPropertyBagValue (sender, e);
			switch(e.Property.Name)
			{
				case "InitialState":
					e.Value = this.initialState; break;
				case "RuleNumber":
					e.Value = this.ruleNumber; break;
			}
		}
		protected override void SetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			base.SetPropertyBagValue (sender, e);
			switch(e.Property.Name)
			{
				case "InitialState":
					this.initialState = (AutomataInitialState) e.Value; break;
				case "RuleNumber":
					this.ruleNumber = (int) e.Value; break;
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
				Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.Automatology.Resources.Wolfram.gif");
					
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
