using Mono.Cecil;
using Mono.Cecil.Cil;

namespace preWeaverCecil.CodeInjection {
	/// <summary>
	/// Summary description for CallCodeInjector.
	/// </summary>
	internal class CallCodeInjector : AbstractCallCodeInjector {
		public override bool isInterceptorFor(Instruction instruction) {
			return instruction.OpCode == OpCodes.Call;
		}

		protected override MethodReference joinPointClassToInstantiate() {
			return this._methodToBeInstrumented.setPointAssemblyRef.methodCallJoinPointConstructor;
		}
	}
}