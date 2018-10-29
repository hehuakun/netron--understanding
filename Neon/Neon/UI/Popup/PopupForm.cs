using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace Netron.Neon
{
	/// <summary>
	/// The PopupForm is a generalized menu
	/// </summary>
	public class PopupForm : System.Windows.Forms.Form, INUIPopup
	{		
		#region Fields
		/// <summary>
		/// required by VS
		/// </summary>
		private System.ComponentModel.Container components = null;
		/// <summary>
		/// access to the root
		/// </summary>
		protected INUIMediator root;
		/// <summary>
		/// part of the color scheme
		/// </summary>
		private Color lightColor = Color.WhiteSmoke, darkColor = Color.SteelBlue;	
		/// <summary>
		/// the brush for the background
		/// </summary>
		private Brush backBrush;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the darker color of the gradient
		/// </summary>
		public Color DarkColor
		{
			get
			{
				return darkColor;
			}
			set
			{
				darkColor = value;
				SetBrush();
				Invalidate();
			}
		}

		/// <summary>
		/// Gets or sets the lighter color of the gradient
		/// </summary>
		public Color LightColor
		{
			get
			{				
				return lightColor;
			}
			set
			{
				lightColor = value;
				SetBrush();
				Invalidate();
			}
		}
		/// <summary>
		/// Gets or sets the root of the mediator
		/// </summary>
		public INUIMediator Root
		{
			get
			{

				return root;
			}
			set
			{
				root = value;
			}
		}
		#endregion

		#region Constructor
		public PopupForm()
		{			
			InitializeComponent();			
			SetBrush();			
		}
		#endregion

		#region Methods

		/// <summary>
		/// Sets the brush in function of the style and scheme
		/// </summary>
		protected void SetBrush()
		{
			backBrush = new System.Drawing.Drawing2D.LinearGradientBrush(this.ClientRectangle,darkColor, lightColor,90);
		}
		/// <summary>
		/// Paints the background
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			//base.OnPaintBackground (e);
			e.Graphics.FillRectangle(backBrush,this.ClientRectangle);
		}
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}


		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// PopupForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.SteelBlue;
			this.ClientSize = new System.Drawing.Size(200, 168);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "PopupForm";
			this.ShowInTaskbar = false;
			this.Text = "PopupForm";

		}
		#endregion

		protected override void OnResize(EventArgs e)
		{
			base.OnResize (e);
			SetBrush();
		}

		/// <summary>
		/// Sets the UI style
		/// </summary>
		/// <param name="style"></param>
		public void SetStyle(Netron.Neon.UIStyle style)
		{
			
		}

		/// <summary>
		/// Sets the Color scheme
		/// </summary>
		/// <param name="scheme"></param>
		public void SetColorScheme(Netron.Neon.UIColorScheme scheme)
		{
			switch(scheme)
			{
				case UIColorScheme.SkyBlue:
					this.lightColor = Color.WhiteSmoke;
					this.darkColor = Color.SteelBlue;				
					break;
				case UIColorScheme.Dark:
					this.lightColor = Color.WhiteSmoke;
					this.darkColor = Color.SlateGray;				
					break;
				case UIColorScheme.Grey:
					this.lightColor = Color.WhiteSmoke;
					this.darkColor = Color.Silver;				
					break;
				case UIColorScheme.Colorful:
					this.lightColor = Color.LightYellow;
					this.darkColor = Color.OrangeRed;				
					break;



			}
			SetBrush();
			Invalidate();
		}

		
		#endregion

	}
}
