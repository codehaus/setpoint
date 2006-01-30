using Mono.Cecil.Cil;

namespace preWeaverCecil.CodeInjection {
	/// <summary>
	/// Neutral injector - returns the very same instruction it receives
	/// </summary>
	public class NeutralCodeInjector : IMessageInterceptionCodeInjector {
		public void interceptMessageSentBy(Instruction originalInstruction, MethodToBeInstrumented methodToBeInstrumented) {			
		}

		public bool isInterceptorFor(Instruction instruction) {
			return true;
		}
	}
}