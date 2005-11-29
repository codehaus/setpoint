using System.Collections;
using System.Reflection.Emit;
using PERWAPI;

namespace preWeaverPERWAPI.CodeInjection {
	/// <summary>
	/// Branch injector - Temporal. Change branch opcode, nowadays always to the normal form
	/// </summary>
	public class BranchCodeInjector : IMessageInterceptionCodeInjector {
		void interceptMessageSentBy(Instr originalInstruction, MethodToBeInstrumented methodToBeInstrumented){
			OpCode opCode = originalInstruction.OpCode;
			ArrayList interceptingCode = new ArrayList();
			((ILBranch) originalInstruction).NormalizeScope();
			//interceptingCode.Add(new ILBranch(opCode, ((ILBranch) originalInstruction).Target));
			interceptingCode.Add(originalInstruction);
			return interceptingCode;
		}

		public bool isInterceptorFor(Instr instruction) {
			return (instruction.OpCode.OperandType.Equals(OperandType.InlineBrTarget) ||
				instruction.OpCode.OperandType.Equals(OperandType.ShortInlineBrTarget));
		}
	}
}