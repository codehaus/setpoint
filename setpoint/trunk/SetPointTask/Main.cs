using System;
using System.IO;
using NAnt.Core;
using NAnt.Core.Attributes;
using preWeaverCecil;
using semanticator;

namespace SetPointTask {
		[TaskName("simAddin")]
		public class SimAddinTask : Task {
			String inputFile;
			String outputFile;

			public SimAddinTask(){
				outputFile = "";
			}

			[TaskAttribute("inputFile", Required=true)]
			public String InputFile {
				get { return inputFile; }
				set { inputFile = value; }
			}

			[TaskAttribute("outputFile", Required=false)]
			public String OutputFile {
				get { return outputFile; }
				set { outputFile = value; }
			}
		
			protected override void ExecuteTask() {
				if(outputFile.Equals("")) {
					outputFile = inputFile;
				}
				Console.WriteLine("SetPoint NAnt Task for Semantication and preWeaving");
				Console.WriteLine("\nProcessing " + inputFile + " to apply aspects...");
				try {
					new Semanticator().semanticate(inputFile);			
					new PreWeaver().preWeave(inputFile, outputFile);
					Console.WriteLine("¡done!\n");
				} catch (FileNotFoundException e){
					Console.Error.WriteLine("simAddinTask: File " + e.FileName + " not found");
				}
			}

		}
	}