using System;
using System.ComponentModel;
namespace Netron.Neon
{
	//MenuItemColorStringConvertor
	internal class MenuItemColorsStringConverter : ExpandableObjectConverter
	{

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return false;
		}

 
		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string) && value is NMenuItem.MenuItemColors)
				return "MenuItem Colors";
			return base.ConvertTo (context, culture, value, destinationType);
		}
            
        
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}


		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes) 
		{
			if (context.PropertyDescriptor.PropertyType.Name.Equals("MenuItemColors"))
			{
				System.ComponentModel.PropertyDescriptorCollection propertyDescriptorCollection; 
				string[] propNames = new string[5]; 
				propertyDescriptorCollection = TypeDescriptor.GetProperties(typeof(NMenuItem.MenuItemColors),attributes); 
				propNames[0] = @"ImageBarColor"; 
				propNames[1] = @"GradientStartColor";
				propNames[2] = @"GradientEndColor";
				propNames[3] = @"HiliteColor";
				propNames[4] = @"HiliteBorderColor";
				//for some reason setting the last element stops the Property
				//expanding in C# but not in VB. Not that it matters since
				//it's the only property left and so will be auto sorted.
				//propNames[5] = @"ForeColor";
				return propertyDescriptorCollection.Sort(propNames); 
			}
			return base.GetProperties(context, value, attributes);
		} 

 
	}

}
