using System;
using System.Drawing;
using System.ComponentModel;
using System.ComponentModel.Design;
namespace Netron.Neon
{
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
