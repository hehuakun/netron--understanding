using System.Diagnostics;
using System.Text;
using System.Reflection;
using System;
using System.Data;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using Netron.GraphLib;
using Netron.GraphLib.Attributes;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.IO;
namespace Netron.GraphLib.Entitology
{
	
	/// <summary>
	/// UML class shape
	/// </summary>
	[Serializable(), Description("Class shape"), NetronGraphShape("Class", "F9E27E10-1B57-4580-B7DE-3018911BE1DD", "UML", "Netron.GraphLib.Entitology.ClassShape", "A UML class shape ")]
	public class ClassShape : Shape, ISerializable
	{
		#region Fields
		private int recHeight = 100;
		private float recPropertiesHeight = 20;
		private float recMethodsHeight = 20;
		private Connector mTopConnector;
		private Connector mBottomConnector;
		private string mClassName;
		private string mSubtitle;
		private string mBody;
		private string mMethodsBody = string.Empty;
		private string mPropertiesBody = string.Empty;
		private ClassDeclarationModifiers mClassModifier = ClassDeclarationModifiers.None;
		
		private object mEntity;
		private CollapseStates mCollapsed = CollapseStates.Main | CollapseStates.Properties | CollapseStates.Methods;
		private ClassPropertyCollection mPropeties = new ClassPropertyCollection();
		private ClassMethodCollection mMethods = new ClassMethodCollection();
		private Font boldFont;
		private bool showGetSet = true;
		private MenuItem itemShowGetSet;
		#endregion
		
		#region Constructors
		/// <summary>
		/// Constructor
		/// </summary>
		public ClassShape()
		{
			Init();
			
		}

		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected ClassShape(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			

			this.mClassName = info.GetString("mClassName");

			this.mCollapsed =(CollapseStates) info.GetValue("mCollapsed", typeof(CollapseStates));

			this.mMethods = info.GetValue("mMethods", typeof(ClassMethodCollection)) as ClassMethodCollection;

			this.mPropeties = info.GetValue("mProperties", typeof(ClassPropertyCollection)) as ClassPropertyCollection;

			Connectors.Clear();
			this.mTopConnector = info.GetValue("mTopConnector",  typeof(Connector)) as Connector;
			this.mTopConnector.BelongsTo = this;
			Connectors.Add(mTopConnector);

			this.mBottomConnector = info.GetValue("mBottomConnector",  typeof(Connector)) as Connector;
			this.mBottomConnector.BelongsTo = this;
			Connectors.Add(mBottomConnector);

			IsResizable = false;
			boldFont = new Font(Font, FontStyle.Bold);
			this.OnMouseUp+=new MouseEventHandler(TaskEvent_OnMouseUp);

		
		}

		private void Init()
		{
			mClassName = "Class";
			mSubtitle = "";
			mBody = "";
			Rectangle = new RectangleF(0, 0, 200, 100);			
			mTopConnector = new Connector(this, "Top", true);
			mTopConnector.ConnectorLocation = ConnectorLocation.North;
			mBottomConnector = new Connector(this, "Bottom", true);
			mBottomConnector.ConnectorLocation = ConnectorLocation.South;
			Connectors.Add(mBottomConnector);					
			Connectors.Add(mTopConnector);					
			IsResizable = false;
			boldFont = new Font(Font, FontStyle.Bold);
			this.OnMouseUp+=new MouseEventHandler(TaskEvent_OnMouseUp);
		}
		#endregion
	
		#region Properties

		public void Collapse()
		{
			mCollapsed |= CollapseStates.Main;	
			this.Invalidate();
		}

		public void Expand()
		{
			mCollapsed &= ~CollapseStates.Main;			//set main collapsed off
			this.Invalidate();
		}
		
		[GraphMLData] public CollapseStates CollapseState
		{
			get{return mCollapsed;}
			set{mCollapsed = value;}
		}

		

		/// <summary>
		/// Overrides the base method to re-define the boldFont <see cref="boldFont"/> member
		/// </summary>
		protected  override string FontFamily
		{
			get
			{
				return base.FontFamily;
			}
			set
			{
				base.FontFamily = value;
				boldFont = new Font(base.Font, FontStyle.Bold);
			}
		}
		/// <summary>
		/// Gets or sets the methods of the class
		/// </summary>
		[GraphMLData]public ClassMethodCollection ClassMethods
		{
			get{return mMethods;}
			set{
				mMethods = value;
				UpdateBody();
			}
		}
		/// <summary>
		/// Gets or sets the properties of the class
		/// </summary>
		[GraphMLData]public ClassPropertyCollection ClassProperties
		{
			get{return mPropeties;}
			set
			{
				mPropeties = value;
				UpdateBody();
			}
		}
		
		public string SubTitle
		{
			get
			{
				return mSubtitle;
			}
			set
			{
				mSubtitle = value;
				this.Invalidate();
			}
		}
		
		public ClassDeclarationModifiers ClassModifier
		{
			get
			{
				return mClassModifier;
			}
			set
			{
				mClassModifier = value;
				switch (mClassModifier)
				{					
					case ClassDeclarationModifiers.None:
						
						mSubtitle = "";
						break;
					default:
						mSubtitle = value.ToString();
						break;
				}
				this.Invalidate();
			}
		}
		
		public string Body
		{
			get
			{
				return mBody;
			}
			set
			{
				mBody = value;
				this.Invalidate();
			}
		}
		
		public object Entity
		{
			get
			{
				return mEntity;
			}
			set
			{
				mEntity = value;
			}
		}

		[GraphMLData]public string ClassName
		{
			get{return mClassName;}
			set{mClassName = value;}
		}
		#endregion
		
		#region Overrides
		/// <summary>
		/// Paints the shape on the canvas
		/// </summary>
		/// <param name="g"></param>
		public override void Paint (Graphics g)
		{			
			
			
			#region Base rectangle
			if ((mCollapsed & CollapseStates.Main)!=CollapseStates.Main)
			{
				Rectangle = new RectangleF(Rectangle.X, Rectangle.Y, 200, 50);
			}
			else
			{
				float rec = 0;
				if(mPropertiesBody.Length>0)
				{
					rec+= 15;
					if ((mCollapsed & CollapseStates.Properties)==CollapseStates.Properties)
						rec+=recPropertiesHeight;
				}
				if(mMethodsBody.Length>0)
				{
					rec+= 15;
					if ((mCollapsed & CollapseStates.Methods)==CollapseStates.Methods)
						rec+=recMethodsHeight;
				}

				Rectangle = new RectangleF(Rectangle.X, Rectangle.Y, 200, rec + 70);
			}
			#endregion

			#region Artist's material
			DashStyle dashStyle = Pen.DashStyle;
			float anchoAnterior = Pen.Width;
			Color colorAnterior = Pen.Color;
			if (this.IsSelected)
			{
				Pen.DashStyle = DashStyle.Solid;
				Pen.Color = Color.Orange;
				Pen.Width = 2;
			}
			else
			{
				Pen.Color = Color.Gray;
				Pen.DashStyle = DashStyle.Solid;
			}
			#endregion
			
			#region Shape's shadow and container
			GraphicsPath path = new GraphicsPath();			
			path.AddArc(Rectangle.X, Rectangle.Y, 20, 20, -180, 90);			
			path.AddLine(Rectangle.X + 10, Rectangle.Y, Rectangle.X + Rectangle.Width - 10, Rectangle.Y);			
			path.AddArc(Rectangle.X + Rectangle.Width - 20, Rectangle.Y, 20, 20, -90, 90);			
			path.AddLine(Rectangle.X + Rectangle.Width, Rectangle.Y + 10, Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height - 10);			
			path.AddArc(Rectangle.X + Rectangle.Width - 20, Rectangle.Y + Rectangle.Height - 20, 20, 20, 0, 90);			
			path.AddLine(Rectangle.X + Rectangle.Width - 10, Rectangle.Y + Rectangle.Height, Rectangle.X + 10, Rectangle.Y + Rectangle.Height);			
			path.AddArc(Rectangle.X, Rectangle.Y + Rectangle.Height - 20, 20, 20, 90, 90);			
			path.AddLine(Rectangle.X, Rectangle.Y + Rectangle.Height - 10, Rectangle.X, Rectangle.Y + 10);			
			//shadow
			Region darkRegion = new Region(path);
			darkRegion.Translate(5, 5);
			g.FillRegion(new SolidBrush(Color.FromArgb(20, Color.Black)), darkRegion);
			
			//background
			g.FillPath(new SolidBrush(Color.White), path);
			
			#endregion

			#region the header
			if ((mCollapsed & CollapseStates.Main)!=CollapseStates.Main)
			{
				//paint the gradient
				Brush unBrush = new LinearGradientBrush(new Point(((int)(Rectangle.X)),((int)(Rectangle.Y))), new Point(((int)(Rectangle.X + Rectangle.Width)),((int)(Rectangle.Y))), ShapeColor, Color.White);
				Region unaRegion = new Region(path);
				g.FillRegion(unBrush, unaRegion);
			}
			else
			{
				
				GraphicsPath gradientPath = new GraphicsPath();				
				gradientPath.AddArc(Rectangle.X + 1, Rectangle.Y + 1, 18, 18, -180, 90);				
				gradientPath.AddLine(Rectangle.X + 11, Rectangle.Y + 1, Rectangle.X + Rectangle.Width - 11, Rectangle.Y + 1);				
				gradientPath.AddArc(Rectangle.X + Rectangle.Width - 19, Rectangle.Y + 1, 18, 18, -90, 90);				
				gradientPath.AddLine(Rectangle.X + Rectangle.Width - 1, Rectangle.Y + 50, Rectangle.X + 1, Rectangle.Y + 50);				
				//gradient
				Brush unBrush = new LinearGradientBrush(new Point(((int)(Rectangle.X)),((int)(Rectangle.Y))), new Point(((int)(Rectangle.X + Rectangle.Width)),((int)(Rectangle.Y))), ShapeColor, Color.White);
				Region unaRegion = new Region(gradientPath);
				g.FillRegion(unBrush, unaRegion);
			}
			#endregion

			#region Border
			//the border
			g.DrawPath(Pen, path);
			#endregion

			#region Textual info and icon
			Pen.DashStyle = dashStyle;
			Pen.Width = anchoAnterior;
			Pen.Color = colorAnterior;
			
			#region Icon
			g.DrawImage(new Bitmap(this.GetType(), "Resources.Class.ico"),((int)(Rectangle.X + 5)),((int)(Rectangle.Y + 5)));
			#endregion

			#region Text and body
			StringFormat sf = new StringFormat();
			sf.Trimming = StringTrimming.EllipsisCharacter;
			g.DrawString(mClassName, boldFont, TextBrush, Rectangle.X + 20, Rectangle.Y + 5);
			g.DrawString(mSubtitle, Font, TextBrush, new RectangleF(Rectangle.X + 5, Rectangle.Y + 22, Rectangle.Width - 10, 28), sf);
			if ((mCollapsed & CollapseStates.Main) !=CollapseStates.Main)
			{
				g.DrawImage(new Bitmap(this.GetType(), "Resources.down.ico"),((int)(Rectangle.Right - 20)),((int)(Rectangle.Y + 5)));
			}
			else
			{
				if(mMethodsBody.Length==0 && mPropertiesBody.Length==0)
				{
					
					g.DrawString("Right-click to add members.", Font, TextBrush, new RectangleF(Rectangle.X + 5, Rectangle.Y + 55, Rectangle.Width - 10, recHeight), sf);
				}
				g.DrawImage(new Bitmap(this.GetType(), "Resources.up.ico"),((int)(Rectangle.Right - 20)),((int)(Rectangle.Y + 5)));
				//g.FillRectangle(Brushes.Yellow,new RectangleF(Rectangle.X + 5, Rectangle.Y + 55, Rectangle.Width - 10, recHeight));
				#region Properties
				if(mPropertiesBody.Length>0)
				{
					
					g.DrawString("Properties", Font, TextBrush, new RectangleF(Rectangle.X + 18, Rectangle.Y + 55, Rectangle.Width - 10, 15), sf);
					if((mCollapsed & CollapseStates.Properties)==CollapseStates.Properties)
					{
						g.DrawImage(new Bitmap(this.GetType(), "Resources.minus.ico"),Rectangle.X + 5, Rectangle.Y + 55);
						g.DrawString(mPropertiesBody, Font, TextBrush, new RectangleF(Rectangle.X + 35, Rectangle.Y + 55 + 15 , Rectangle.Width - 10, recPropertiesHeight), sf);
						for(int k = 0; k<mPropeties.Count; k++)
							g.DrawImage(new Bitmap(this.GetType(), "Resources.SPP.ico"),Rectangle.X + 18, Rectangle.Y + 55 + 15 + k*13);
					}
					else
					{
						g.DrawImage(new Bitmap(this.GetType(), "Resources.plus.ico"),Rectangle.X + 5, Rectangle.Y + 55);					
					}

				}
				#endregion

				#region Methods
				if( mMethodsBody.Length>0)
				{
					float shift;
					if((mCollapsed & CollapseStates.Properties)==CollapseStates.Properties)
						shift = recPropertiesHeight;
					else
						shift = 0;
					g.DrawString("Methods", Font, TextBrush, new RectangleF(Rectangle.X + 18, Rectangle.Y + 55 + 15 + shift, Rectangle.Width - 10, 15), sf);
					if((mCollapsed & CollapseStates.Methods)==CollapseStates.Methods)
					{
						g.DrawImage(new Bitmap(this.GetType(), "Resources.minus.ico"),Rectangle.X + 5, Rectangle.Y + 55 + 15 + shift);
						g.DrawString(mMethodsBody, Font, TextBrush, new RectangleF(Rectangle.X + 33, Rectangle.Y + 55 + 15 +15 + shift, Rectangle.Width - 10, recMethodsHeight), sf);
						for(int k = 0; k<mMethods.Count; k++)
							g.DrawImage(new Bitmap(this.GetType(), "Resources.SPM.ico"),Rectangle.X + 18, Rectangle.Y + 55 + 15 +15 +shift + k*13);

					}
					else
					{
						g.DrawImage(new Bitmap(this.GetType(), "Resources.plus.ico"),Rectangle.X + 5, Rectangle.Y + 55 + 15 + shift);					
					}
				}
				#endregion
				
			}
			#endregion
			#endregion
		}
		/// <summary>
		/// Returns the location of the connectors
		/// </summary>
		/// <param name="c">a connector</param>
		/// <returns>the location where to draw the connector</returns>
		public override PointF ConnectionPoint(Connector c)
		{
			if (c == mTopConnector)			
				return new PointF(Rectangle.Left + Rectangle.Width / 2, Rectangle.Top );
			else if (c==mBottomConnector)
				return new PointF(Rectangle.Left + Rectangle.Width / 2, Rectangle.Bottom );
			else
			{
				return new PointF(0, 0);
			}
		}
		
		public System.Drawing.PointF ConnectionPoint(GraphLib.Connector c, System.Drawing.PointF initialPoint, System.Drawing.PointF finalPoint)
		{
			PointF sourcePt = new PointF();
			RectangleF shapeRect =  this.Rectangle;
			if (Intersects(initialPoint, 
							finalPoint,
							new PointF(shapeRect.Left, shapeRect.Top),
							new PointF(shapeRect.Right, shapeRect.Top),
							ref sourcePt) )
				return sourcePt;
            if(Intersects(initialPoint, finalPoint, new PointF(shapeRect.Left, shapeRect.Top), new PointF(shapeRect.Left, shapeRect.Bottom), ref sourcePt))
				return sourcePt;
			if(Intersects(initialPoint, finalPoint, new PointF(shapeRect.Left, shapeRect.Bottom), new PointF(shapeRect.Right, shapeRect.Bottom), ref sourcePt))
				return sourcePt;
            if(Intersects(initialPoint, finalPoint, new PointF(shapeRect.Right, shapeRect.Top), new PointF(shapeRect.Right, shapeRect.Bottom),ref sourcePt))
				return sourcePt;

			return this.ConnectionPoint(c);
			
		}
				
		private bool Intersects(PointF line1Point1, PointF line1Point2, PointF line2Point1, PointF line2Point2, ref PointF intersection)
		{
			//Based on the 2d line intersection method from "comp.graphics.algorithmsFrequently Asked Questions"
			//			(Ay-Cy)(Dx-Cx)-(Ax-Cx)(Dy-Cy)
			//		r = -----------------------------  (eqn 1)
			//			(Bx-Ax)(Dy-Cy)-(By-Ay)(Dx-Cx)
					
			double q = (line1Point1.Y - line2Point1.Y) *(line2Point2.X - line2Point1.X) -(line1Point1.X - line2Point1.X) *(line2Point2.Y - line2Point1.Y);
			double d = (line1Point2.X - line1Point1.X) *(line2Point2.Y - line2Point1.Y) -(line1Point2.Y - line1Point1.Y) *(line2Point2.X - line2Point1.X);
					
			//parallel lines so no intersection anywhere in space (in curved space, maybe, but not here in Euclidian space.)
			if (d == 0)
			{
				return false;
			}
					
			double r = q / d;
					
			//		   (Ay-Cy)(Bx-Ax)-(Ax-Cx)(By-Ay)
			//	   s = -----------------------------  (eqn 2)
			//		   (Bx-Ax)(Dy-Cy)-(By-Ay)(Dx-Cx)
					
			q = (line1Point1.Y - line2Point1.Y) *(line1Point2.X - line1Point1.X) -(line1Point1.X - line2Point1.X) *(line1Point2.Y - line1Point1.Y);
			double s = q / d;
					
			//If r>1, P is located on extension of AB
			//If r<0, P is located on extension of BA
			//If s>1, P is located on extension of CD
			//If s<0, P is located on extension of DC
					
			//The above basically checks if the intersection is located at an extrapolated
			//point outside of the line segments. To ensure the intersection is only(within)
			//the line segments then the above must all be false, ie r between 0 and 1
			//and s between 0 and 1.
					
			if (r < 0 | r > 1 | s < 0 | s > 1)
			{
				return false;
			}
					
			//Px = Ax + r(Bx - Ax)
			//Py = Ay + r(By - Ay)
					
			intersection.X = line1Point1.X +(float)(0.5 + r * (line1Point2.X - line1Point1.X));
			intersection.Y = line1Point1.Y +(float)(0.5 + r * (line1Point2.Y - line1Point1.Y));
					
			return true;
		}
				
		public override void AddProperties ()
		{
			base.AddProperties();
		
			Bag.Properties.Add(new PropertySpec("Class name", typeof(string),"Class","The name of the class"));
			//bag.Properties.Add(new PropertySpec("Class name",  typeof(string),"Class","The methods of the class",string.Empty,typeof(UI.ClassUIEditor),typeof(TypeConverter)));			
			//bag.Properties.Add(new PropertySpec("Observations",  typeof(string)));

			#region The class modifier is a reflected Enum type, hence this construction.
			PropertySpec specSubType = new PropertySpec("ClassModifier",typeof(string),"Appearance","Gets or sets the sub-type of the shape.","Task",typeof(ReflectedEnumStyleEditor),typeof(TypeConverter));
			ArrayList list = new ArrayList();
			string[] names = Enum.GetNames(typeof(ClassDeclarationModifiers));
			for(int k =0; k<names.Length; k++)			
				list.Add(names[k]);		
			specSubType.Attributes = new Attribute[]{ new ReflectedEnumAttribute(list)};
			Bag.Properties.Add(specSubType);
			//bag.Properties.Add(new PropertySpec("Type", typeof(string),"Test","The shape's sub-type","Task",typeof(ReflectedEnumStyleEditor),typeof(TypeConverter))  );
			#endregion

			
		}
		#region PropertyBag
		protected override void GetPropertyBagValue (object sender, PropertySpecEventArgs e)
		{
			base.GetPropertyBagValue(sender, e);
					
			switch (e.Property.Name)
			{
				case "Class name":							
					e.Value = mClassName;
					break;

				case "Methods":							
					e.Value = mMethods;
					break;
				
				case "ClassModifier":							
					e.Value = mClassModifier.ToString();
					break;
				
			}
		}
				
		protected override void SetPropertyBagValue (object sender, PropertySpecEventArgs e)
		{
			base.SetPropertyBagValue(sender, e);
			switch (e.Property.Name)
			{
				
				case "Class name":
							
					//use the logic and the constraint of the object that is being reflected
					if (!(e.Value.ToString() == null))
					{
						mClassName = System.Convert.ToString(e.Value);
					}
					else
					{
						//MessageBox.Show("Not a valid label", "Invalid label", MessageBoxButtons.OK, MessageBoxIcon.Warning)
						this.Invalidate();
					}
					break;
				case "Methods":
					mMethods = e.Value as ClassMethodCollection;
					break;
				case "ClassModifier":
							
					ClassModifier = (ClassDeclarationModifiers) Enum.Parse(typeof(ClassDeclarationModifiers), e.Value.ToString());
					break;
				
			}
		}
		#endregion


		#endregion

		#region Methods
		/// <summary>
		/// Adds an extra menu item to the context menu
		/// </summary>
		/// <returns></returns>
		public override MenuItem[] ShapeMenu()
		{
			MenuItem itemLoadAssembly = new MenuItem("Load from assembly",new EventHandler(AssemblyLoadClick));
			itemShowGetSet = new MenuItem("Property access",new EventHandler(GetSetClick));
			itemShowGetSet.Checked = showGetSet;
			MenuItem itemClassProps = new MenuItem("Class properties", new EventHandler(ClassPropertiesClick));

			return new MenuItem[]{itemLoadAssembly, itemShowGetSet, itemClassProps};
		}
		private void ClassPropertiesClick(object sender, EventArgs e)
		{
			ShowClassShapeDialog(0);
		}

		/// <summary>
		/// Handles the load-click event of the shape
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AssemblyLoadClick(object sender, EventArgs e)
		{
			ShowClassShapeDialog(3);
			//ResetContent();
			//string libPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\" + Assembly.GetAssembly(typeof(Netron.GraphLib.UI.GraphControl)).GetName().Name + ".dll";
			//this.ReflectType(libPath,"Netron.GraphLib.BasicShapes.BasicNode");
		}
		/// <summary>
		/// Changes the display of the shape to include/exclude the get/set note next to the property name
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GetSetClick(object sender, EventArgs e)
		{
			showGetSet = !showGetSet;
			itemShowGetSet.Checked = !itemShowGetSet.Checked;
			UpdateBody();
		}

		/// <summary>
		/// Re-loads the content of the body
		/// </summary>
		private void ResetContent()
		{
			mPropeties.Clear();
			mMethods.Clear();
			mClassName = "Class";
			mClassModifier = ClassDeclarationModifiers.None;
		}

		/// <summary>
		/// Adds a class-method to the collection
		/// </summary>
		/// <param name="methodName"></param>
		public void AddMethod(ClassMethod methodName)
		{
			//drop the methods corresponding to properties or events
			if(methodName.Name.StartsWith("get_") || methodName.Name.StartsWith("set_") || methodName.Name.StartsWith("add_") || methodName.Name.StartsWith("remove_")  ) return;
			this.mMethods.Add(methodName);
			UpdateBody();
		}
		/// <summary>
		/// Adds a class-property to the collection
		/// </summary>
		/// <param name="property"></param>
		public void AddProperty(ClassProperty property)
		{
			
			this.mPropeties.Add(property);

			UpdateBody();
		}

		/// <summary>
		/// Updates the collapsible body in function of the added properties and methods
		/// </summary>
		private void UpdateBody()
		{
			StringBuilder msb = new StringBuilder();
			StringBuilder psb = new StringBuilder();
			Graphics g = this.Site.Graphics;
			
			recPropertiesHeight = 0;
			for(int k=0; k<mPropeties.Count; k++)
			{
				
				if(showGetSet)
				{
					psb.Append(mPropeties[k].GetSetName);
					recPropertiesHeight+=g.MeasureString(mPropeties[k].GetSetName,this.Font).Height;
				}
				else
				{
					psb.Append(mPropeties[k].Name);
					recPropertiesHeight+=g.MeasureString(mPropeties[k].Name,this.Font).Height;
				}
				psb.Append(Environment.NewLine);
			}
			mPropertiesBody = psb.ToString();
			
			recMethodsHeight = 0;
			for(int k=0; k<mMethods.Count; k++)
			{
				msb.Append(mMethods[k].Name);
				recMethodsHeight+=g.MeasureString(mMethods[k].Name,this.Font).Height;
				msb.Append(Environment.NewLine);				
			}

			mMethodsBody = msb.ToString();
			recHeight =(int) (recMethodsHeight + recPropertiesHeight +70);
			Invalidate();

		}
		/// <summary>
		/// Gets a specific cursor in function of the region over which the mouse is hovering
		/// </summary>
		/// <param name="p">a point</param>
		/// <returns>a hand-cursor on collapsible regions</returns>
		public override Cursor GetCursor(PointF p)
		{
			RectangleF collapseRect = new RectangleF(((int)(Rectangle.Right - 20)), ((int)(Rectangle.Y + 5)), 16, 16);
					
			if (collapseRect.Contains(p))
			{
				return Cursors.Hand;
			}
			collapseRect =  new RectangleF(Rectangle.X + 5, Rectangle.Y + 55,18,18);
			if (collapseRect.Contains(p))
			{
				return Cursors.Hand;
			}

			float shift;
			if((mCollapsed & CollapseStates.Properties)==CollapseStates.Properties)
				shift = recPropertiesHeight;
			else
				shift = 0;

			collapseRect = new RectangleF(Rectangle.X + 5, Rectangle.Y + 55 + 15 + shift,18, 18);
			if (collapseRect.Contains(p))
			{
				return Cursors.Hand;
			}

			//fall back to the default
			return base.GetCursor(p);
		}


		
		#endregion
				
		#region Mouse handler
		/// <summary>
		/// Handles the click event on the shape and initiates the expands/collapses the elements
		/// </summary>
		/// <param name="sender">the sender</param>
		/// <param name="e">mouse event arguments</param>
		private void TaskEvent_OnMouseUp (object sender, System.Windows.Forms.MouseEventArgs e)
		{
			#region Test the main expansion icon
			RectangleF collapseRect = new RectangleF(((int)(Rectangle.Right - 20)), ((int)(Rectangle.Y + 5)), 16, 16);
					
			if (collapseRect.Contains(e.X, e.Y))
			{
				//switch the main state
				if((mCollapsed & CollapseStates.Main) !=CollapseStates.Main)
					mCollapsed |= CollapseStates.Main;			//set main collapsed on
				else
					mCollapsed &= ~CollapseStates.Main;			//set main collapsed off
				this.Invalidate();
				return;
			}

			#endregion

			#region Test the properties expansion icon
			collapseRect = new RectangleF(Rectangle.X + 5, Rectangle.Y + 55,18,18);

			if (collapseRect.Contains(e.X, e.Y))
			{
				//switch the props state
				if((mCollapsed & CollapseStates.Properties) !=CollapseStates.Properties)
					mCollapsed |= CollapseStates.Properties;
				else
					mCollapsed &= ~CollapseStates.Properties;
				this.Invalidate();
				return;
			}

			#endregion

			#region Test the method expansion icon
			float shift;
			if((mCollapsed & CollapseStates.Properties)==CollapseStates.Properties)
				shift = recPropertiesHeight;
			else
				shift = 0;

			collapseRect = new RectangleF(Rectangle.X + 5, Rectangle.Y + 55 + 15 + shift,18, 18);
			if (collapseRect.Contains(e.X, e.Y))
			{
				//switch the methods state
				if((mCollapsed & CollapseStates.Methods) !=CollapseStates.Methods)
					mCollapsed |= CollapseStates.Methods;
				else
					mCollapsed &= ~CollapseStates.Methods;
				this.Invalidate();
				return;
			}
			#endregion

		}

		#endregion

		private void ShowClassShapeDialog(int tabindex)
		{
			ClassDialog dlg = new ClassDialog(this);
			dlg.Methods = ClassMethods;
			dlg.Properties = ClassProperties;
			dlg.SelectedTabIndex = tabindex;	
			dlg.PropertyBag = Bag;
			dlg.ShowDialog();
			
			//on return of the dialog, transfer the new data
			ClassMethods = dlg.Methods;
			ClassProperties = dlg.Properties;
		}

		public override Bitmap GetThumbnail()
		{
			Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.GraphLib.Entitology.Resources.ClassShape.gif");
					
			Bitmap bmp= Bitmap.FromStream(stream) as Bitmap;
			stream.Close();
			stream=null;
			return bmp;
		}


		#region ISerializable Members

		public override void PostDeserialization()
		{
			base.PostDeserialization ();
			UpdateBody();
		}



		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);

			info.AddValue("mClassName", mClassName);

			info.AddValue("mCollapsed",this.mCollapsed);

			info.AddValue("mMethods",this.mMethods, typeof(ClassMethodCollection));

			info.AddValue("mProperties", this.mPropeties, typeof(ClassPropertyCollection));

			info.AddValue("mTopConnector", this.mTopConnector, typeof(Connector));

			info.AddValue("mBottomConnector", this.mBottomConnector, typeof(Connector));

		}

		#endregion
	}
	
}
