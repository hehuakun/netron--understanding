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
using Netron.GraphLib.Interfaces;
using System.Security;
using System.Security.Permissions;
namespace Netron.GraphLib
{
	/// <summary>
	/// The abstract contains the abstract structure of the graph
	/// Pretty much just an enumeration of the elements with standard collection methods.
	/// Derived from the Shape class, can draw the whole plex as if it was a single shape
	/// </summary>
	[Serializable] public class GraphAbstract : IPaintable, IAutomataCell, ISerializable, IEntityBundle, IDeserializationCallback
	{
		#region Fields	
		/// <summary>
		/// the default and static background layer
		/// </summary>
		protected static GraphLayer mDefaultLayer;

		/// <summary>
		/// the shape layers
		/// </summary>
		protected GraphLayerCollection mLayers;

		/// <summary>
		/// the control this abstract belongs to
		/// </summary>
		private IGraphSite mSite;
		/// <summary>
		/// the collection of shapes
		/// </summary>
		private ShapeCollection mShapes = new ShapeCollection();
		/// <summary>
		/// the collection of connections
		/// </summary>
		private ConnectionCollection mConnections = new ConnectionCollection();
		/// <summary>
		/// the meta-info of the graph (author, description,...)
		/// </summary>
		private GraphInformation mGraphInformation;
		/// <summary>
		/// ordered collection of entities
		/// </summary>
		internal EntityCollection paintables = new EntityCollection();

		#endregion

		#region Properties

		/// <summary>
		/// Gets the shape layers
		/// </summary>
		public GraphLayerCollection Layers
		{
			get{return mLayers;}			
		}
		/// <summary>
		/// Gets the default layer of the control
		/// 
		/// </summary>
		/// <remarks>Note that this is a static property</remarks>
		public static GraphLayer DefaultLayer
		{
			get{return mDefaultLayer;}
		}

		/// <summary>
		/// Gets or sets the bounding rectangle
		/// </summary>
		[Browsable(false)]
		public RectangleF Rectangle
		{
//			set 
//			{
//				RectangleF r = Rectangle;
//				Single dX = value.X - r.X;
//				Single dY = value.Y - r.Y;
//				Single dWidth = value.Width - r.Width;
//				Single dHeight = value.Height - r.Height;
//				foreach (Shape shape in mShapes)
//				{
//					shape.X += dX; shape.Y += dY;
//					shape.Width += dWidth;
//					shape.Height += dHeight;
//				}
//				Rectangle = value;
//			}
			get
			{
				
				return SumRectangles();
			}
		}

		/// <summary>
		/// Returns the union of the bounding rectangles of all entities
		/// </summary>
		private RectangleF SumRectangles()
		{
			RectangleF r = RectangleF.Empty;

			// for each shape in mShapes of the abstract
			foreach (Shape shape in mShapes)
			{
				r = RectangleF.Union(r,shape.Rectangle);
				if( shape.Tracker != null )
				{
					RectangleF a = shape.Tracker.Grip(new Point(-1, -1));
					RectangleF b = shape.Tracker.Grip(new Point(+1, +1));
					r = RectangleF.Union(r,RectangleF.Union(a, b));
				}
			}
			return r;
		}
		/// <summary>
		/// Gets the shape collection of the graph
		/// </summary>
		public ShapeCollection Shapes
		{
			get
			{
				return mShapes;
			}
		}	

		/// <summary>
		/// Gets the collection of connections
		/// </summary>
		public ConnectionCollection Connections
		{
			get
			{
				return mConnections;
			}
		}
		/// <summary>
		/// Gets or sets the graph information, i.e. the meta-information
		/// of the graph like author, description, etc.
		/// </summary>
		public GraphInformation GraphInformation
		{
			get{return mGraphInformation;}
			set{mGraphInformation = value;}
		}

		#endregion

		#region Constructors
		
		/// <summary>
		/// Default ctor
		/// </summary>
		public GraphAbstract()
		{		 
			mGraphInformation = new GraphInformation();
			Init();
		}

		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public GraphAbstract(SerializationInfo info, StreamingContext context)
		{

			#region Version test
			//test the version, warn if the build or major is different
			Version currentversion = Assembly.GetExecutingAssembly().GetName().Version;
			Version fileversion = new Version(info.GetString("NetronGraphLibVersion"));
			int diff = currentversion.CompareTo(fileversion);

			if(fileversion.Minor!=currentversion.Minor || fileversion.Major !=currentversion.Major)
			{
				DialogResult res = MessageBox.Show("The graph was saved in version " + fileversion.ToString() + " while the current graph library has version " + currentversion.ToString() + ". It is not guaranteed that the graph will appeare as it was when saved. Are you sure you want to open the graph?","Different version",MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
				if(res==DialogResult.No) return;					
			}
			#endregion
			
			Init();

			this.mShapes = info.GetValue("mShapes", typeof(ShapeCollection)) as ShapeCollection;
			this.mConnections = info.GetValue("mConnections", typeof(ConnectionCollection)) as ConnectionCollection;
			

			this.mGraphInformation = info.GetValue("mGraphInformation", typeof(GraphInformation)) as GraphInformation;

			this.mLayers = info.GetValue("mLayers", typeof(GraphLayerCollection)) as GraphLayerCollection;

		}

		private void Init()
		{
			//the shape layers
			mLayers = new GraphLayerCollection();
			mLayers.ClearComplete+=new EventHandler(Layers_ClearComplete);
			//the default layer
			mDefaultLayer = new GraphLayer("Default",Color.WhiteSmoke,100);
			mDefaultLayer.UseColor = false; //use colors only for upper layers
			
			BindEntityCollectionEvents();

		}

		/// <summary>
		/// Binds the collections events of the ShapeCollection and ConnectionCollection
		/// </summary>
		private void BindEntityCollectionEvents()
		{
			this.mShapes.OnShapeAdded+=new ShapeInfo(mShapes_OnShapeAdded);
			this.mShapes.OnShapeRemoved+=new ShapeInfo(mShapes_OnShapeRemoved);

			this.mConnections.OnConnectionAdded+=new ConnectionInfo(mConnections_OnConnectionAdded);
			this.mConnections.OnConnectionRemoved+=new ConnectionInfo(mConnections_OnConnectionRemoved);
		}
		#endregion

		#region Methods

		internal void SortPaintables()
		{
			
			//descending because the z-axis goes 'into' the screen, we use a right-handed coordinate system
			this.paintables.Sort("ZOrder",SortDirection.Descending); 
		}

		/// <summary>
		/// Performs an additional reset of all shapes to the default layer if all layers are 
		/// removed from the collection.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>Note that this methods needs to be public in order to serialize the class, due to security reasons.</remarks>
		public void Layers_ClearComplete(object sender, EventArgs e)
		{
			foreach(Shape sh in Shapes)
				sh.SetLayer( "Default");
		}
		/// <summary>
		/// Paint overrides the base method and paints all elements of the array,
		/// i.e. the boxes and connectors. The paint method of the elements is called to draw themselves.
		/// </summary>
		/// <param name="g">Graphics class</param>
		public void Paint(Graphics g)
		{	
			/*
			if(this.Shapes.Count<1) return;
			// paint the connections
			foreach (Connection n in Connections)
				if(n.From.BelongsTo.Layer.Visible && n.To.BelongsTo.Layer.Visible && n.Layer.Visible)  n.Paint(g);	
			// paint the shapes
			foreach (Shape o in Shapes)
			{
				if(o.Layer.Visible)  
				{
						if(o.IsSelected) o.ShapeTracker.Paint(g);
					try
					{
						o.Paint(g);
					}
					catch
					{continue;}
						o.PaintAdornments(g);
						
				}
			}
			*/			
			for(int k=0; k<paintables.Count; k++)
			{
				if(paintables[k]==null || !paintables[k].Layer.Visible) continue;
				paintables[k].PaintTracker(g);
				paintables[k].Paint(g);
				paintables[k].PaintAdornments(g);

			}
			//paint the connector
			foreach (Shape o in Shapes)
			{
				if(!o.Layer.Visible) continue; //otherwise the connectors will float on the canvas
				foreach (Connector c in o.Connectors)
					if ((o.IsHovered) || (c.IsHovered))
						c.Paint(g);
			}
			
		}
		/// <summary>
		/// Paints the abstract on request of an external object (i.e. not the graph control itself) like the printer or the SaveImage method
		/// </summary>
		/// <param name="g"></param>
		public void PaintExternal(Graphics g)
		{
			g.SmoothingMode = SmoothingMode.AntiAlias;
			for(int k=0; k<paintables.Count; k++)
			{
				if(paintables[k]==null || !paintables[k].Layer.Visible) continue;
				paintables[k].PaintTracker(g);
				paintables[k].Paint(g);
				paintables[k].PaintAdornments(g);

			}
		}
		/// <summary>
		/// Inserts a new object into the plex. 
		/// </summary>
		/// <param name="so">the object to insert</param>
		/// <remarks>Note that you can add only one shape at a time.
		/// </remarks>
		internal protected void Insert(Shape so)
		{			
			so.Insert(this);
			so.AddProperties();			
			
			Site.RaiseOnShapeAdded(so);

		}

		/// <summary>
		/// Inserts a connection in the bastract
		/// </summary>
		/// <param name="con"></param>
		internal protected void Insert(Connection con)
		{
			mConnections.Add(con);			
			con.AddProperties();
			//Site.RaiseOnConnectionAdded(con,manual);
		}

		
		/// <summary>
		/// Deletes an element of the plex, goes via the History class, also deletes the mConnections.
		/// </summary>
		/// <remarks>Note that multiple mShapes can be delete in one go if they have the Selected flag set to true.</remarks>
		internal protected  void Delete()
		{
			//need a temporary list otherwise you'll get 'the collection has changed...' exception
			EntityCollection list = new EntityCollection();
			foreach (Shape so in mShapes)
			{
				if (so.IsSelected) list.Add(so);
			}
			
			foreach (Connection conn in Connections)
					if (conn.IsSelected && conn.From.BelongsTo != null && conn.To.BelongsTo!=null)
						list.Add(conn);

			foreach(Entity item in list)
				item.Delete(); //connections attached to the shape will be delete in the Delete() method of the shape
			
		}
		/// <summary>
		/// This method initiates the tramsmission of data over the mConnections. It calls the tramsit method on all sub-level objects.
		/// </summary>
		public virtual void Transmit()
		{
			//this is the old way
			//foreach (Shape so in Shapes) so.Transmit();

			//this one is much more performant and each connection transmit will occur only once:
			foreach(Connection con in mConnections) con.Transmit();
			//base.Transmit(); //the abstract has no connectors and hence no data transfer on its own
		}
		/// <summary>
		/// Starts to update all the nodes of the plex; can be a calculation on the basis of the sent values or any other.
		/// Usually the process before all the received values are reset.
		/// </summary>
		/// <remarks>
		/// In normal circumstances this method goes hand-in-hand with the transmit method.
		/// Well, maybe some new physics can be invented if you hack here.
		/// </remarks>
		public virtual void Update()
		{
			foreach (Shape o in Shapes) 
			{
				o.BeforeUpdate();
				o.Update();
				o.AfterUpdate();
			}
			
		}



	
		

		


	

		#endregion

		#region IPaintable Members
		/// <summary>
		/// Gets or sets the site of the abstract
		/// </summary>
		public IGraphSite Site
		{
			get
			{
				return mSite;
			}
			set
			{
				mSite = value;
			}
		}

		

		/// <summary>
		/// IPaintable.Invalidate implementation
		/// </summary>
		public void Invalidate(){}

		#endregion

		#region IAutomataCell Members

		/// <summary>
		/// IAutomataCell.InitAutomata implementation
		/// </summary>
		public virtual void InitAutomata(){}

		/// <summary>
		/// IAutomataCell.BeforeUpdate implementation
		/// </summary>
		public virtual void BeforeUpdate(){}

		/// <summary>
		/// IAutomataCell.AfterUpdate implementation
		/// </summary>
		public virtual void AfterUpdate(){}

		#endregion

		#region ISerializable Members

		/// <summary>
		/// ISerializable implementation
		/// </summary>
		/// <param name="info">the serialization info</param>
		/// <param name="context">the streaming context</param>
		[SecurityPermissionAttribute(SecurityAction.Demand,SerializationFormatter=true)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			//keep the version in the serialized graph to warn users
			//we forget about the minor version-number though
			Version version = Assembly.GetExecutingAssembly().GetName().Version;

			info.AddValue("NetronGraphLibVersion",version.ToString());
			
			info.AddValue("mConnections",this.mConnections);			

			info.AddValue("mShapes",this.mShapes);			

			info.AddValue("mGraphInformation",this.mGraphInformation);
			
			mLayers.ClearComplete-=new EventHandler(Layers_ClearComplete);

			info.AddValue("mLayers", this.mLayers, typeof(GraphLayerCollection));
			
			mLayers.ClearComplete+=new EventHandler(Layers_ClearComplete);
		}

		#endregion

		private void mShapes_OnShapeAdded(object sender, Shape shape)
		{
			this.paintables.Add(shape);
			this.SortPaintables();
		}

		private void mShapes_OnShapeRemoved(object sender, Shape shape)
		{
			this.paintables.Remove(shape);
			this.SortPaintables();
		}

		private bool mConnections_OnConnectionAdded(object sender, ConnectionEventArgs e)
		{
			this.paintables.Add(e.Connection);
			this.SortPaintables();
			return true;
		}

		private bool mConnections_OnConnectionRemoved(object sender, ConnectionEventArgs e)
		{
			this.paintables.Remove(e.Connection);
			this.SortPaintables();
			return true;
		}
		#region IDeserializationCallback Members

		/// <summary>
		/// IDeserializationCallback implementation
		/// </summary>
		/// <param name="sender"></param>
		public void OnDeserialization(object sender)
		{
			BindEntityCollectionEvents();
		}

		#endregion
	}
}

