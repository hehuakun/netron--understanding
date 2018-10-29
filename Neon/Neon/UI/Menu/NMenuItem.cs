using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
namespace Netron.Neon
{
		/// <summary>
		/// Summary description for NMenuItem.
		/// http://homepage.ntlworld.com/mdaudi100/alternate/menus.html
		/// </summary>
		public class NMenuItem : MenuItem
		{
			#region Fields
			private Image m_Image = null;
			private MenuItemColors m_ItemColors;
			private Font m_Font = SystemInformation.MenuFont;
			private HighliteStyles m_HiliteStyle = HighliteStyles.Flat;
			private int m_ImageIndex = -1;
			private ImageList m_Imagelist = null;
			private Image m_ShadowImage = null;
        
			private bool m_IsXPThemed = IsXPThemed;

			//  ________________1______________
			// |    |  _________3_________     |
			// |  2 | |_________4_________|    |
			// |____|__________________________|

			private Rectangle m_ItemRect;   //1
			private Rectangle m_ImageRect;  //2
			private Rectangle m_BackRect;   //3
			private Rectangle m_TextRect;   //4
			/// <summary> 
			/// Required designer variable.
			/// </summary>
			private System.ComponentModel.Container components = null;
			#endregion

			#region Properties
			//Image
			[Category("Appearance"), DefaultValue(typeof(Image), null)]
			public Image image
			{
				get
				{
					if (m_Imagelist != null)
					{
						if (m_ImageIndex > -1)
							return m_Imagelist.Images[m_ImageIndex];
					}
					return m_Image;
				}
				set
				{
					m_Image = value;
				}
			}


			//ImageList
			[Category("Appearance"), DefaultValue(typeof(ImageList), null)]
			public ImageList imageList
			{
				get
				{
					return m_Imagelist;
				}
				set
				{
					m_Imagelist = value;
				}
			}


			//ImageIndex
			[Category("Appearance"), DefaultValue(-1), TypeConverter(typeof(ImageIndexConverter)),
			Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design", typeof(System.Drawing.Design.UITypeEditor))]
			public int ImageIndex
			{
				get
				{
					if (m_Imagelist == null)
						m_ImageIndex = -1;
					return m_ImageIndex;
				}
				set
				{
					m_ImageIndex = value;
				}
			}


			//ItemColors
			[Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
			[RefreshProperties(RefreshProperties.All)]
			public MenuItemColors ItemColors
			{
				get
				{
					return m_ItemColors;
				}
				set
				{
					m_ItemColors = value;
				}
			}


			//Font
			[Category("Appearance"),Description("The Font that displays in this MenuItem.")]
			public Font font
			{
				get
				{
					return m_Font;
				}
				set
				{
					if (!m_Font.Equals(value))
					{
						if (value != null)
							ResetFont();
						else
							m_Font = value;
					}
				}
			}
			private bool ShouldSerializeFont()
			{
				return !m_Font.Equals(SystemInformation.MenuFont);
			}
			private void ResetFont()
			{
				m_Font = SystemInformation.MenuFont;
			}


			//HiliteStyle
			[Category("Behavior"), DefaultValue(typeof(HighliteStyles), "Flat")]
			public HighliteStyles HiliteStyle 
			{
				get
				{
					return m_HiliteStyle;
				}
				set
				{
					m_HiliteStyle = value;
				}
			}


			private static bool IsXPThemed
			{
				get
				{
					if (Environment.OSVersion.Version.Major >= 5 && Environment.OSVersion.Version.Minor >= 1)
						return IsAppThemed();
					return false;
				}
			}


			private bool RTLMenu
			{
				get
				{
					if (GetContextMenu() == null)                    
						return (bool)(GetMainMenu().RightToLeft == RightToLeft.Yes);
					else
						return (bool)(GetContextMenu().RightToLeft == RightToLeft.Yes);
                
				}
			}

			#endregion

			#region Constructor
			public NMenuItem()
			{
				// This call is required by the Windows.Forms Form Designer.
				InitializeComponent();
				this.OwnerDraw = true;
				// TODO: Add any initialization after the InitializeComponent call
				m_ItemColors = new MenuItemColors(this);    
			}

			#endregion

			#region Methods

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


			#region Component Designer generated code
			/// <summary> 
			/// Required method for Designer support - do not modify 
			/// the contents of this method with the code editor.
			/// </summary>
			private void InitializeComponent()
			{
				// 
				// NMenuItem
				// 
				this.OwnerDraw = true;

			}
			#endregion

			#region XP Related InterOp

			[DllImport("uxtheme.dll", CallingConvention=CallingConvention.Cdecl)]
			private static extern bool IsAppThemed();
        
			[DllImport("user32.dll", CallingConvention=CallingConvention.Cdecl)]
			private static extern int GetSysColor(int index);

			private const int COLOR_MENUBAR = 30;

			#endregion

			#region Custom Constructors

			public NMenuItem(String caption) : this(caption,(Image)null)
			{
			}


			public NMenuItem(String caption,Image image): base()
			{
				m_Image = image;
				this.Text = caption;
				m_ItemColors = new MenuItemColors(this);    
			}
        

			#endregion
      
			protected override void OnMeasureItem(MeasureItemEventArgs e)
			{
				String ItemText = Text;
				if (ShowShortcut && Shortcut != Shortcut.None)
				{
					Keys k = (Keys)Shortcut;
					ItemText += "  " + TypeDescriptor.GetConverter(typeof(Keys)).ConvertToString(k);
				}
            
				StringFormat sf = new StringFormat(StringFormatFlags.MeasureTrailingSpaces);
				sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Hide;
            
				e.ItemWidth = (int)e.Graphics.MeasureString(ItemText, m_Font, SizeF.Empty, sf).Width;
            
				if (Text == "-")
					e.ItemHeight = 3;
				else if (this.Parent is MainMenu == false)
				{
					e.ItemHeight = 22;
					e.ItemWidth += 44;
				}

			}


			protected override void OnDrawItem(DrawItemEventArgs e)
			{
				m_ItemRect = e.Bounds;
				
				m_ImageRect = new Rectangle(e.Bounds.Location, new Size(24, e.Bounds.Height));
				m_BackRect = new Rectangle(e.Bounds.Left + 24, e.Bounds.Top, e.Bounds.Width - 24, e.Bounds.Height);
				m_TextRect = new Rectangle(e.Bounds.Left + 34, e.Bounds.Top, e.Bounds.Width - 52, e.Bounds.Height);

				if (RTLMenu)
				{
					m_ImageRect.Offset(e.Bounds.Width - 24, 0);
					m_BackRect.Offset(-24, 0);
					m_TextRect.Offset(-14, 0);
				}
            
				if (Text == "-")
				{
					DrawSideBar(e.Graphics);
					DrawBackground(e.Graphics);
					DrawSeparator(e.Graphics);
				}
				else if(Parent is MainMenu == false)
				{
					DrawSideBar(e.Graphics);
					DrawBackground(e.Graphics);
					if (System.Convert.ToBoolean(e.State & DrawItemState.Selected)) 
						DrawHilite(e.Graphics);
					DrawImage(e.Graphics, ((System.Convert.ToBoolean(e.State & DrawItemState.Selected) && (m_HiliteStyle == HighliteStyles.Shadow))));
					DrawText(e.Graphics, !System.Convert.ToBoolean(e.State & DrawItemState.NoAccelerator));
				}
				else
				{
					m_TextRect = e.Bounds;
					DrawMenuBar(e.Graphics);
					if (System.Convert.ToBoolean(e.State & DrawItemState.Selected) || System.Convert.ToBoolean(e.State & DrawItemState.HotLight)) 
						DrawHilite(e.Graphics);
					DrawText(e.Graphics, !System.Convert.ToBoolean(e.State & DrawItemState.NoAccelerator));
				}
            
			}


			#region Custom Methods

			void DrawMenuBar(Graphics g)
			{
				SolidBrush FillBrush = new SolidBrush(SystemColors.Menu);
				if (m_IsXPThemed)
					FillBrush.Color = ColorTranslator.FromOle(GetSysColor(COLOR_MENUBAR));
				g.FillRectangle(FillBrush, m_ItemRect);
				FillBrush.Dispose();
			}
        

			void DrawSeparator(Graphics g)
			{
				Pen DrawingPen = new Pen(m_ItemColors.ForeColor);
				Point startpoint = new Point(m_TextRect.Left, m_TextRect.Top + 1);
				Point endpoint = new Point(m_BackRect.Right - 1, m_BackRect.Top + 1);
				if (RTLMenu)
				{
					startpoint.Offset(-20, 0);
					endpoint.Offset(-10, 0);
				}
				g.DrawLine(DrawingPen, startpoint, endpoint);
				DrawingPen.Dispose();
			}


			void DrawSideBar(Graphics g)
			{
				SolidBrush DrawingBrush = new SolidBrush(m_ItemColors.ImageBarColor);
				g.FillRectangle(DrawingBrush, m_ImageRect);
				DrawingBrush.Dispose();
			}


			void DrawBackground(Graphics g)
			{
				Brush FillBrush;
				if (RTLMenu)
					FillBrush = new LinearGradientBrush(m_BackRect, m_ItemColors.GradientEndColor, m_ItemColors.GradientStartColor, LinearGradientMode.Horizontal);
				else
					FillBrush = new LinearGradientBrush(m_BackRect, m_ItemColors.GradientStartColor, m_ItemColors.GradientEndColor, LinearGradientMode.Horizontal);
				g.FillRectangle(FillBrush, m_BackRect);
				FillBrush.Dispose();
			}


			void DrawHilite(Graphics g)
			{
				SolidBrush FillBrush = new SolidBrush(m_ItemColors.HiliteColor);
				Rectangle HiliteRect = m_ItemRect;
				if (m_HiliteStyle == HighliteStyles.Box && Parent is MainMenu == false)
				{
					HiliteRect.Width -= 24;
					if (!RTLMenu)
						HiliteRect.Offset(24, 0);
					BoxImage(g);
				}
				g.FillRectangle(FillBrush, HiliteRect);
				Pen BorderPen = new Pen(m_ItemColors.HiliteBorderColor);
				HiliteRect.Width -= 1;
				HiliteRect.Height -= 1;
				g.DrawRectangle(BorderPen, HiliteRect);
				FillBrush.Dispose();
				BorderPen.Dispose();
			}
        

			void BoxImage(Graphics g)
			{
				if (image == null) return;
				Pen BorderPen = new Pen(ControlPaint.LightLight(m_ItemColors.ImageBarColor));
				Rectangle BorderRect = m_ImageRect;
				BorderRect.Inflate(-1, -1);
				g.DrawRectangle(BorderPen, BorderRect);
				BorderPen.Color = ControlPaint.Dark(m_ItemColors.ImageBarColor);
				g.DrawLine(BorderPen, BorderRect.Right - 1, BorderRect.Top, BorderRect.Right - 1, BorderRect.Bottom);
				g.DrawLine(BorderPen, BorderRect.Left, BorderRect.Bottom, BorderRect.Right - 1, BorderRect.Bottom);
				BorderPen.Dispose();
			}


			void DrawText(Graphics g, bool ShowHotKey)
			{
				SolidBrush TextBrush = new SolidBrush(m_ItemColors.ForeColor);
				StringFormat sf = new StringFormat();
				sf.LineAlignment = StringAlignment.Center;
				sf.Alignment = StringAlignment.Near;
				if (ShowHotKey)
					sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show;
				else
					sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Hide;
            
				String ShorCutText = "";
            
				if (ShowShortcut && Shortcut != Shortcut.None)
				{
					Keys k = (Keys)Shortcut;
					ShorCutText = TypeDescriptor.GetConverter(typeof(Keys)).ConvertToString(k);
				}
            
				if (RTLMenu)
					sf.FormatFlags = StringFormatFlags.DirectionRightToLeft;
            
				if (Parent is MainMenu)
					sf.Alignment = StringAlignment.Center;
            
                //RectangleF textRectF = RectangleF.FromLTRB(m_TextRect.Left,m_TextRect.Top,m_TextRect.Right,m_TextRect.Bottom);
				if (Enabled)
				{
					g.DrawString(Text, m_Font, TextBrush, m_TextRect, sf);
					sf.Alignment = StringAlignment.Far;
					g.DrawString(ShorCutText, m_Font, TextBrush, m_TextRect, sf);
				}
				else
				{
					ControlPaint.DrawStringDisabled(g, Text, m_Font, m_ItemColors.GradientEndColor, m_TextRect, sf);
					sf.Alignment = StringAlignment.Far;
					ControlPaint.DrawStringDisabled(g, ShorCutText, m_Font, m_ItemColors.GradientEndColor,m_TextRect, sf);
				}
            
				TextBrush.Dispose();
				sf.Dispose();
            
			}



			void DrawImage(Graphics g, bool Shadow)
			{
				Rectangle Iconrect = m_ImageRect;
				
				
				if (image == null) 
				{
					//Iconrect.Inflate(-8,-8);					
					//g.DrawString("*",new Font("Arial",10),Brushes.Black,Iconrect);
					return;
				}
				
				Iconrect.Inflate(4, 4);
				if (Shadow)
				{
					if (m_ShadowImage == null)
						m_ShadowImage = new Bitmap(CreateShadowImage(image));
					Iconrect.Offset(1, 1);
					g.DrawImage(m_ShadowImage, Iconrect);
					Iconrect.Offset(-2, -2);
				}

				g.DrawImage(image, Iconrect);
            
			}


			Bitmap CreateShadowImage(Image bmp)
			{
				Bitmap shadow = new Bitmap(bmp, 16, 16);
				Color ShadowColor = ControlPaint.Dark(m_ItemColors.HiliteColor);
            
				for (int x = 0; x<=15; x++)
				{
					for (int y = 0;  y<=15; y++)
					{
						if (shadow.GetPixel(x, y).A > 32)
							shadow.SetPixel(x, y, ShadowColor);
					}
				}
            
				return shadow;
            
			}


			#endregion
			#endregion



			//MenuItemColors
			[TypeConverter(typeof(MenuItemColorsStringConverter))]
				public class MenuItemColors
			{

				public MenuItemColors(NMenuItem parent) : base()
				{
					owner = parent;
				}

				internal NMenuItem owner;
				private Color m_StartColor = Color.WhiteSmoke;
				private Color m_EndColor = Color.LightSlateGray;
				private Color m_ImageBarColor = Color.WhiteSmoke;
				private Color m_HiliteColor = Color.LightSlateGray;
				private Color m_HiliteBorderColor = Color.Black;
				private Color m_ForeColor = Color.Black;

				//GradientStartColor
				[DefaultValue(typeof(Color), "Ivory")]
				public Color GradientStartColor
				{
					get
					{
						return m_StartColor;
					}
					set
					{
						m_StartColor = value;
						updateProperties();
					}
				}
    

				//GradientEndColor
				[DefaultValue(typeof(Color), "PowderBlue")]
				public Color GradientEndColor
				{
					get
					{
						return m_EndColor;
					}
					set
					{
						m_EndColor = value;
						updateProperties();
					}
				}
            

				//ImageBarColor
				[DefaultValue(typeof(Color), "PowderBlue")]
				public Color ImageBarColor
				{
					get
					{
						return m_ImageBarColor;
					}
					set
					{
						m_ImageBarColor = value;
						updateProperties();
					}
				}


				//HiliteColor
				[DefaultValue(typeof(Color), "Moccasin")]
				public Color HiliteColor
				{
					get
					{
						return m_HiliteColor;
					}
					set
					{
						m_HiliteColor = value;
						updateProperties();
					}
				}


				//HiliteBorderColor
				[DefaultValue(typeof(Color), "Coral")]
				public Color HiliteBorderColor
				{
					get
					{
						return m_HiliteBorderColor;
					}
					set
					{
						m_HiliteBorderColor = value;
						updateProperties();
					}
				}


				//ForeColor
				[DefaultValue(typeof(Color), "OrangeRed")]
				public Color ForeColor
				{
					get
					{
						return m_ForeColor;
					}
					set
					{
						m_ForeColor = value;
						updateProperties();
					}
				}


				//This method is necessary for the property to be deserialized 
				//upon reset, and caused me the biggest headache.
				void updateProperties()
				{
					if (owner == null) return;
					IComponentChangeService ccs =(IComponentChangeService)	owner.GetService(typeof(IComponentChangeService));
					if (ccs == null) return;
					ccs.OnComponentChanged(owner.ItemColors, null, null, null);
                
				}


			}


		}
	}



