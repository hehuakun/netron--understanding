using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Diagnostics;
namespace Netron.AutomataShapes
{
	/// <summary>
	/// Helper class for compiling scripts from the scripter node
	/// </summary>
	public class DotnetCompiler
	{
		
		/// <summary>
		/// The scripting languages
		/// </summary>
		public enum ScriptLanguages
		{
			/// <summary>
			/// VB.Net
			/// </summary>
			VB,
			/// <summary>
			/// C#
			/// </summary>
			CSharp
		}

		/// <summary>
		/// Returns the results of the compilation
		/// </summary>
		/// <param name="Source"></param>
		/// <param name="Reference"></param>
		/// <param name="Language"></param>
		/// <returns></returns>
		public static CompilerResults CompileScript(string Source, string Reference, ScriptLanguages Language)
		{
			CodeDomProvider provider = null;

			switch(Language)
			{
				case ScriptLanguages.VB:
					provider = new Microsoft.VisualBasic.VBCodeProvider();
					break;
				case ScriptLanguages.CSharp:
					provider = new Microsoft.CSharp.CSharpCodeProvider();
					break;
			}

			return CompileScript(Source, Reference, provider);
		}

		/// <summary>
		/// Returns the results of the compilation
		/// </summary>
		/// <param name="Source"></param>
		/// <param name="Reference"></param>
		/// <param name="Provider"></param>
		/// <returns></returns>
		public static CompilerResults CompileScript(string Source, string Reference, CodeDomProvider Provider)
		{
			ICodeCompiler compiler = Provider.CreateCompiler();
			CompilerParameters parms = new CompilerParameters();
			CompilerResults results;
			
			// Configure parameters
			parms.MainClass="Script";
			parms.GenerateExecutable = false;
			parms.GenerateInMemory = true;
			parms.TempFiles=new TempFileCollection(Path.GetTempPath(),false);
			//parms.OutputAssembly="scripter_"+Assembly.GetCallingAssembly().GetName().Version.Build;
			parms.IncludeDebugInformation = false;
			if (Reference != null && Reference.Length != 0)
				parms.ReferencedAssemblies.Add(Reference);
			foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
			{
				parms.ReferencedAssemblies.Add(asm.Location);
			}
			parms.ReferencedAssemblies.Add("System.Windows.Forms.dll");
			parms.ReferencedAssemblies.Add("System.dll");

			// Compile
			results = compiler.CompileAssemblyFromSource(parms, Source);
			
			return results;

			
				
			
		}

		/// <summary>
		/// Finds an interface in a given assembly
		/// </summary>
		/// <param name="DLL"></param>
		/// <param name="InterfaceName"></param>
		/// <returns></returns>
		public static object FindInterface(System.Reflection.Assembly DLL, string InterfaceName)
		{
			// Loop through types looking for one that implements the given interface
			try
			{
				foreach(Type t in DLL.GetTypes())
				{
					//System.Diagnostics.Trace.WriteLine(t.FullName);
					if (t.GetInterface(InterfaceName, true) != null)					
						return DLL.CreateInstance(t.FullName);
				}
			}
			catch(System.Reflection.ReflectionTypeLoadException exc)
			{
				Trace.WriteLine(exc.Message);
			}

			return null;
		}
		

	}


}
