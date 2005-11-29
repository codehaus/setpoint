using PERWAPI;

namespace preWeaverPERWAPI.CodeInjection {
	/// <summary>
	/// Intercepts evey single message send 
	/// </summary>
	/// <remarks> None yet</remarks>
	public class MessageInterceptionInstrumenter {
		private CodeInjectorChooser chooser = new CodeInjectorChooser();

		internal void processMethodBody(MethodToBeInstrumented methodToBeInstrumented) {
			CILInstructions code = methodToBeInstrumented.code;
			CILInstruction originalInstruction;
			
			if (code==null) // It's an abstract method
				return;
			while ((originalInstruction = code.GetNextInstruction() as CILInstruction) != null)
				if(originalInstruction is Instr)
					this.interceptMessageSentBy(originalInstruction as Instr, methodToBeInstrumented);
		}

		#region private (refactored) methods		

		private void interceptMessageSentBy(Instr instruction, MethodToBeInstrumented message) {
			this.chooser.codeInjectorFor(instruction).interceptMessageSentBy(instruction, message);
		}

		#endregion
	}
}