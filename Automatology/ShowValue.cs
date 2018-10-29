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
	/// This node allows to display outputted values from other nodes in a variety of ways
	/// </summary>
	[Serializable]
	[Description("Show value")]
	[NetronGraphShape("Show value","AB154C5D-EDC2-400e-BEC7-5D31396D8AFA","Automata","Netron.Automatology.ShowValue",
		 "Displays the value of a connector stream.")]
	public class ShowValue : Shape, ISerializable
	{	
		#region Fields
		/// <summary>
		/// the unique (input) connector
		/// </summary>
		protected Connector inConnector;
		/// <summary>
		/// the list of values to be displayed by this node
		/// </summary>
		protected ArrayList displayValues;

		/// <summary>
		/// the size or portion of the data array to be shown
		/// </summary>
		protected int displayLength;

		/// <summary>
		/// the type of visualization
		/// </summary>
		protected VisualizationTypes visualization;

		

		#endregion

		#region Constructor
		/// <summary>
		/// the ctor
		/// </summary>
		public ShowValue():base()			
		{
			IsResizable=false;
			Rectangle = new RectangleF(0, 0, 120, 40);
			displayLength = 5;
			inConnector=new Connector(this,"Input",false);
			inConnector.ConnectorLocation = ConnectorLocation.West;
			this.Connectors.Add(inConnector);
			this.visualization = VisualizationTypes.Value;
			this.displayValues = new ArrayList();
			
			
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
			info.AddValue("visualization", this.visualization, typeof(VisualizationTypes));
			info.AddValue("displayLength", this.displayLength);


			
		}
		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected ShowValue(SerializationInfo info, StreamingContext context) : base(info, context)
		{			
			inConnector = (Connector) info.GetValue("inConnector", typeof(Connector));
			inConnector.BelongsTo = this;
			Connectors.Add(inConnector);		

			visualization = (VisualizationTypes) info.GetValue("visualization", typeof(VisualizationTypes));
			//Trace.WriteLine("Visualization is " + visualization.ToString());
			displayLength = info.GetInt32("displayLength");
			//Trace.WriteLine("display length: " + DisplayLength);

			this.displayValues = new ArrayList();

			//Trace.WriteLine("displayValues initialized: " + displayValues!=null? "ok" : "nok");
		}
	
		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets how many values should be displayed from the array (if any)
		/// </summary>
		public int DisplayLength
		{
			get{return displayLength;}
			set
			{
				displayLength=value;
				
			}
		}
		#endregion

		#region Methods

		
		/// <summary>
		/// Resets the state of the (automata) node
		/// </summary>
		public override void InitAutomata()
		{
			base.InitAutomata ();
			displayValues=new ArrayList();
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
				Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.Automatology.Resources.ShowValue.gif");
					
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
		public override void Paint(Graphics g)
		{		
				
			if(inConnector.Receives==null) return;				
			try
			{
				StringFormat sf = new StringFormat();
				SizeF stringsize=g.MeasureString(Text,Font);
				int excess=Math.Max((int) (stringsize.Width-120F),0)+5;
				
				//the title part
				g.FillRectangle(new SolidBrush(ShapeColor), Rectangle.X, Rectangle.Y, Rectangle.Width+excess , 12 );
				sf.Alignment = StringAlignment.Center;
				g.DrawString("Value Display", Font, new SolidBrush(Color.Black), Rectangle.Left + (Rectangle.Width / 2), Rectangle.Top , sf);
				
				//data part
				Color Background = IsSelected ? Color.LightSteelBlue : Color.WhiteSmoke;
				g.FillRectangle(new SolidBrush(Background), Rectangle.X, Rectangle.Y+12, Rectangle.Width +excess, Rectangle.Height - 12);
				switch(this.visualization)
				{
					case VisualizationTypes.Value:
						sf.Alignment=StringAlignment.Near;				
						g.DrawString(Text, Font, new SolidBrush(Color.Black), Rectangle.X+5 , Rectangle.Top +22, sf);
						break;
					case VisualizationTypes.Color:
						if(displayValues==null) return;
						for(int k= 0; k< displayValues.Count; k++)
						{
							g.FillRectangle(new SolidBrush((Color) displayValues[k]),Rectangle.X+5 + k*10, Rectangle.Top+22,10,10);
						}
						break;
					case VisualizationTypes.Chernoff:
						if(displayValues==null) return;
						if(displayValues.Count>0)
							this.DrawChernoff(g,Convert.ToSingle(displayValues[0]),Convert.ToSingle(displayValues[1]),Convert.ToSingle(displayValues[2]),Convert.ToSingle(displayValues[3]),Convert.ToSingle(displayValues[4]));
						break;

				}
			}
			catch{}

			//main rectangle
			//g.DrawRectangle(blackpen,Rectangle.Left,Rectangle.Top,Rectangle.Width+excess,Rectangle.Height);
		}

		/// <summary>
		/// Returns the locations of the connectors
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public override System.Drawing.PointF ConnectionPoint(Connector c)
		{						
			if (c == inConnector) return new PointF(Rectangle.Left, Rectangle.Top+(Rectangle.Height/2));
			return base.ConnectionPoint (c);
			//return new PointF(0, 0);
		}
		/// <summary>
		/// Updates the state of the automata to the next state
		/// </summary>
		public override void Update()
		{
			try
			{
				//clear the sends values
				inConnector.Sends.Clear();
				StringBuilder sb = new StringBuilder();
				if(inConnector.Receives.Count>0)
				{
					switch(this.visualization)
					{
						case VisualizationTypes.Value:
							bool first=true;
							for(int k=0; (k<inConnector.Receives.Count) && (k<displayLength); k++)
							{
								try
								{
									if(inConnector.Receives[k]!=null)						
									{
										if (first)
										{sb.Append( inConnector.Receives[k].ToString());first=false;}
										else
											sb.Append(" | " + inConnector.Receives[k].ToString());	
									}
								}
								catch{} //would'nt be a good idea to output debug things here!
							}
							Text=sb.ToString();			
							break;
						case VisualizationTypes.Color:
							displayValues.Clear();
							for(int k=0; (k<inConnector.Receives.Count) && (k<displayLength); k++)
							{
								try
								{
									if(inConnector.Receives[k]!=null)						
									{
										if(inConnector.Receives[k] is Color)
										{
											displayValues.Add(inConnector.Receives[k]);
										}
										else if(inConnector.Receives[k] is int)
										{
											displayValues.Add(Color.FromArgb(Math.Max(0, (int) inConnector.Receives[k]),Color.Black));
										
										}
										else if(inConnector.Receives[k] is double || inConnector.Receives[k] is float)
										{
											displayValues.Add(Color.FromArgb(Math.Max(0,Convert.ToInt32(inConnector.Receives[k])),Color.Black));
										
										}
										else if(inConnector.Receives[k] is bool)
										{
											if((bool) inConnector.Receives[k]==true)
												displayValues.Add(Color.Black);
											else
												displayValues.Add(Color.White);

										}
										else
										{
											displayValues.Add(Color.Empty);
										}

									}
								}
								catch{}
							}
							break;
						case VisualizationTypes.Chernoff:
							displayValues.Clear();
							for(int k=0; (k<inConnector.Receives.Count) && (k<5); k++)
							{
								if(inConnector.Receives[k] != null)
								{
									if(inConnector.Receives[k] is int || inConnector.Receives[k] is double || inConnector.Receives[k] is float)
									{
										displayValues.Add(inConnector.Receives[k]);
									}
									else
										displayValues.Add(0);

								}
								else
									displayValues.Add(0);
							}
							break;
			
					}
				}
				else
				{
					Text="";
					if(displayValues!=null)
						displayValues.Clear();
			
				}
				//			if (inConnector.Receives.Count>0)
				//			{
				//				displayValues.Clear(); //the previous content is not kept in memory
				//				IEnumerator enumer=inConnector.Receives.GetEnumerator();
				//				while (enumer.MoveNext())
				//				{
				//					displayValues.Add(enumer.Current.ToString());
				//				}
				//			
				//				
				//				
				//			}
				inConnector.Receives.Clear();			
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.Message, "ShowValue.Update");
			}
		}


		/// <summary>
		/// Changes the visualization
		/// </summary>
		public void ChangeVisualization()
		{
			switch(this.visualization)
			{
				case VisualizationTypes.Color: case VisualizationTypes.Value:
					Rectangle = new RectangleF(Rectangle.X,Rectangle.Y, 120, 40);
					break;
				case VisualizationTypes.Chernoff: case VisualizationTypes.Pie:
					Rectangle = new RectangleF(Rectangle.X,Rectangle.Y, 90, 140);
					break;

			}
		}

		#region Access to property properties
		public override void AddProperties()
		{
			base.AddProperties ();
			Bag.Properties.Add(new PropertySpec("DisplayArrayLength",typeof(int),"Automata","Gets or sets the portion of the data array visible in the node (if applicable)."));
			Bag.Properties.Add(new PropertySpec("VisualizationType",typeof(VisualizationTypes), "Automata","Gets or sets the type of visualization for the data.",VisualizationTypes.Value));
		}

		protected override void GetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			base.GetPropertyBagValue (sender, e);

			switch(e.Property.Name)
			{
				case "DisplayArrayLength":
					e.Value = displayLength; break;
				case "VisualizationType":
					e.Value = this.visualization; break;
			}
		}

		protected override void SetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			base.SetPropertyBagValue (sender, e);
			switch(e.Property.Name)
			{
				case "DisplayArrayLength":
					DisplayLength = (int) e.Value; break;
				case "VisualizationType":
					this.visualization = (VisualizationTypes) e.Value;
					ChangeVisualization();
					break;
			}
		}

		#endregion

		/// <summary>
		/// Draws Cheroff faces of the incoming data
		/// <seealso cref="http://mathworld.wolfram.com/ChernoffFace.html"/>
		/// </summary>
		/// <param name="g"></param>
		/// <param name="head"></param>
		/// <param name="nose"></param>
		/// <param name="eye"></param>
		/// <param name="mouth"></param>
		/// <param name="brows"></param>
		private void DrawChernoff(Graphics g, float head, float nose, float eye, float mouth, float brows)
		{

			//clamp the values
			float headValue = Math.Min(1F, Math.Max(-1F, head));
			float noseValue = Math.Min(1F, Math.Max(-1F, nose));
			float mouthValue = Math.Min(1F, Math.Max(-1F, mouth));
			float eyeValue = Math.Min(1F, Math.Max(-1F, eye));
			float eyebrowsValue = Math.Min(1F, Math.Max(-1F, brows));

			//g.Clear(Color.White);
			//base rectangle
			RectangleF r = new RectangleF(Rectangle.X+5,Rectangle.Y+22, 80 ,80+ 35*headValue);

			
			//head
			g.DrawEllipse(Pens.Black,r);
			//nose
			PointF[] nosePoints = new PointF[3];
			nosePoints[0] = new PointF(r.Left + r.Width/2, r.Top + r.Height/2 -10);
			nosePoints[1] = new PointF(r.Left +r.Width/2-10, r.Top + r.Height/2 + 10 + 6*noseValue);
			nosePoints[2] = new PointF(r.Left +r.Width/2+10, r.Top + r.Height/2 + 10 + 6*noseValue);
			g.FillPolygon(Brushes.Black,nosePoints);

			//mouth
			float remains = r.Bottom - nosePoints[1].Y - 10;
			PointF mlt = new PointF(nosePoints[1].X-20,nosePoints[1].Y+2);
			if(mouthValue>0)
				g.DrawArc(new Pen(Brushes.Black,2F),new RectangleF(mlt,new SizeF(40+nosePoints[2].X-nosePoints[1].X,remains*mouthValue)),0,180);
			else if(mouthValue==0)
			{
				g.DrawLine(new Pen(Brushes.Black,2F),mlt, new PointF(40+nosePoints[2].X-nosePoints[1].X,mlt.Y));
			}
			else if(mouthValue<0)
				g.DrawArc(new Pen(Brushes.Black,2F),new RectangleF(mlt,new SizeF(40+nosePoints[2].X-nosePoints[1].X,remains*(-mouthValue))),180,180);
			//g.DrawRectangle(Pens.Red,mlt.X,mlt.Y,40+nose[2].X-nose[1].X,remains);
		
			//eyes
			g.FillEllipse(Brushes.Black,r.Left + r.Width/2 - 25, r.Top+r.Height/2-15,10,10+5*eyeValue);
			g.FillEllipse(Brushes.Black,r.Left + r.Width/2 + 15, r.Top+r.Height/2-15,10,10+5*eyeValue);


			//eyebrows
			PointF[] browsPoints = new PointF[4];
			browsPoints[0] = new PointF(r.Left + r.Width/2 - 27, r.Top+r.Height/2-20 - 5*eyebrowsValue);
			browsPoints[1] = new PointF(r.Left + r.Width/2 - 15, r.Top+r.Height/2-20 +  5*eyebrowsValue);
			browsPoints[2] = new PointF(r.Left + r.Width/2 + 27, r.Top+r.Height/2-20 - 5*eyebrowsValue);
			browsPoints[3] = new PointF(r.Left + r.Width/2 + 15, r.Top+r.Height/2-20 +  5*eyebrowsValue);

			g.DrawLine(new Pen(Brushes.Black,2F),browsPoints[0],browsPoints[1]);
			g.DrawLine(new Pen(Brushes.Black,2F),browsPoints[2],browsPoints[3]);

			//frame
			//r.Inflate(10, 10);
			//g.DrawRectangle(Pens.SteelBlue,System.Drawing.Rectangle.Round(r));
		}


		#endregion
	}
	
}
