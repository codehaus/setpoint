using System.Collections;
using PERWAPI;

namespace preWeaverPERWAPI.CodeInjection {
	/// <summary>
	/// Neutral injector - returns the very same instruction it receives
	/// </summary>
	public class NeutralCodeInjector : IMessageInterceptionCodeInjector {
		public void interceptMessageSentBy(Instr originalInstruction, MethodToBeInstrumented methodToBeInstrumented) {			
		}

		public bool isInterceptorFor(Instr instruction) {
			return true;
		}
	}
}