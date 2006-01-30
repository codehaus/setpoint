using Mono.Cecil.Cil;

namespace preWeaverCecil.CodeInjection {
	/// <summary>
	/// Base class for code injectors that aim to intercept messages being sent
	/// </summary>
	public interface IMessageInterceptionCodeInjector {
		/// <summary>
		/// Returns the code that shall intercept the message being sent in 'originalInstruction', if any
		/// </summary>
		void interceptMessageSentBy(Instruction originalInstruction, MethodToBeInstrumented methodToBeInstrumented);

		/// <summary>
		/// Is this code injector that shall deal with this instruction?
		/// </summary>
		bool isInterceptorFor(Instruction originalInstruction);

	}
}