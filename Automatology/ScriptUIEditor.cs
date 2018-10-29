using System;
using System.Drawing.Design;
using System.Windows.Forms;
using Netron.GraphLib;
using System.ComponentModel.Design;
using Netron.GraphLib.Interfaces;
namespace Netron.AutomataShapes
{
	public class ScriptUIEditor : UITypeEditor 
	{ 
		public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context) 
		{ 			
			// We will use a window for property editing. 
			return UITypeEditorEditStyle.Modal; 
		} 

		public override object EditValue( System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value) 
		{ 
			ScriptEditor editor = new  ScriptEditor();
			editor.ScriptSource = (string) value;
			DialogResult res=editor.ShowDialog();
			// Return the new value. 
			if(res==DialogResult.OK)
			{
				PropertyBag bag = context.Instance as PropertyBag;
				Shape shape =  bag.Owner as Shape;	
				shape.Tag = editor.CompiledScript as IScript;
				
				
//				IHost host = (IHost) obj;
//				//((Scripter)(g)context.Instance).Owner ).Script = editor.CompiledScript;
//				editor.CompiledScript.Initialize((context.Instance as Scripter));

				return editor.ScriptSource;
			}
			else
				return (string) value;
		} 

		public override bool GetPaintValueSupported(	System.ComponentModel.ITypeDescriptorContext context) 
		{ 
			return false; 
		}

		
		

	} 

}
