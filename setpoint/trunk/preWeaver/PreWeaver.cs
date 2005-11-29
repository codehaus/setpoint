using System;
using PERWAPI;
using preWeaverPERWAPI.CodeInjection;

namespace preWeaverPERWAPI {
	/// <summary>
	/// PreWeaver
	/// </summary>
	public class PreWeaver {
		public void preWeave(string fileName, string outputFileName) {
			MessageInterceptionInstrumenter instrumenter = new MessageInterceptionInstrumenter();

			PEFile pe = PEFile.ReadPEFile(fileName);
			SetPointAssemblyRef setPointAssemblyRef = new SetPointAssemblyRef();
			foreach (ClassDef classToWeave in pe.GetClasses())
				foreach (MethodDef methodToWeave in classToWeave.GetMethods()) {						
					instrumenter.processMethodBody(new MethodToBeInstrumented(methodToWeave,setPointAssemblyRef));
				}

			pe.GetThisAssembly().AddCustomAttribute(setPointAssemblyRef.semanticatedAttributeConstructor, new byte[0]);
			pe.SetFileName(outputFileName);
			pe.WritePEFile(true);			
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) {
			//new PreWeaver().preWeave("TestLibrary.exe", "perwapiedTest.exe");
			new PreWeaver().preWeave("dotSenkuView.exe", "perwapiedSenkuView.exe");
			new PreWeaver().preWeave("dotSenku.dll", "perwapiedSenku.dll");
			System.Diagnostics.Process.Start("roundTrip.bat");
		}
	}
}