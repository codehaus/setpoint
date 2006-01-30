using System;
using Mono.Cecil.Cil;

namespace preWeaverCecil.CodeInjection {
	/// <summary>
	/// Intercepts evey single message send 
	/// </summary>
	/// <remarks> None yet</remarks>
	public class MessageInterceptionInstrumenter {
		private CodeInjectorChooser chooser = new CodeInjectorChooser();

		internal void processMethodBody(MethodToBeInstrumented methodToBeInstrumented) {
			if (this.hasEmptyBody(methodToBeInstrumented)) return;

			Instruction originalInstruction = methodToBeInstrumented.code.Instructions[0];
			while (originalInstruction != null) {
				this.interceptMessageSentBy(originalInstruction, methodToBeInstrumented);
				originalInstruction = originalInstruction.Next;
			}
		}

		#region private (refactored) methods		

		private void interceptMessageSentBy(Instruction instruction, MethodToBeInstrumented message) {
			this.chooser.codeInjectorFor(instruction).interceptMessageSentBy(instruction, message);
		}

		public bool hasEmptyBody(MethodToBeInstrumented methodToBeInstrumented) {
			return (methodToBeInstrumented.method.IsAbstract ||
				methodToBeInstrumented.code == null ||
				methodToBeInstrumented.code.Instructions.Count == 0);
		}

		#endregion
	}
}