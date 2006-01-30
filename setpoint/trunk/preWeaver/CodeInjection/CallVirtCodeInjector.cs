using Mono.Cecil;
using Mono.Cecil.Cil;

namespace preWeaverCecil.CodeInjection {
	/// <summary>
	/// Summary description for CallVirtCodeInjector.
	/// </summary>
	internal class CallVirtCodeInjector : AbstractCallCodeInjector {
		public override bool isInterceptorFor(Instruction instruction) {
			return instruction.OpCode == OpCodes.Callvirt;
		}

		protected override MethodReference joinPointClassToInstantiate() {			
			return this._methodToBeInstrumented.setPointAssemblyRef.methodCallJoinPointConstructor;
		}
		
	}
}