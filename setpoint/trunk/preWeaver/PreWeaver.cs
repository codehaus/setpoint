using System;
using Mono.Cecil;
using preWeaverCecil.CodeInjection;

namespace preWeaverCecil {
	/// <summary>
	/// PreWeaver
	/// </summary>
	public class PreWeaver {
		public void preWeave(string fileName, string outputFileName) {
			MessageInterceptionInstrumenter instrumenter = new MessageInterceptionInstrumenter();

			AssemblyDefinition assembly = AssemblyFactory.GetAssembly(fileName);
			SetPointAssemblyRef setPointAssemblyRef = new SetPointAssemblyRef();
			foreach(TypeDefinition classToWeave in assembly.MainModule.Types) {
				foreach(MethodDefinition constructorToWeave in classToWeave.Constructors) {
					instrumenter.processMethodBody(new MethodToBeInstrumented(constructorToWeave, setPointAssemblyRef));
				}
				foreach(MethodDefinition methodToWeave in classToWeave.Methods) {
					instrumenter.processMethodBody(new MethodToBeInstrumented(methodToWeave, setPointAssemblyRef));					
				}
			}

			MethodReference semanticatedAttributeConstructor = assembly.MainModule.Import(setPointAssemblyRef.semanticatedAttributeConstructor);
			assembly.CustomAttributes.Add(new CustomAttribute(semanticatedAttributeConstructor));
			AssemblyFactory.SaveAssembly(assembly, outputFileName);
		}


		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) {
			//new PreWeaver().preWeave("TestLibrary.exe", "perwapiedTest.exe");
			//new PreWeaver().preWeave("dotSenkuView.exe", "perwapiedSenkuView.exe");
			//new PreWeaver().preWeave("dotSenku.dll", "perwapiedSenku.dll");
			//System.Diagnostics.Process.Start("roundTrip.bat");
			//new PreWeaver().preWeave(args[0], args[0]);
			//new PreWeaver().preWeave("wh.exe", "wh.exe");
			new PreWeaver().preWeave("WindowsApplication.exe", "WindowsApplication.exe");
			//new PreWeaver().preWeave("Mono.Cecil2.dll", "Mono.Cecil2.dll");
		}
	}
}