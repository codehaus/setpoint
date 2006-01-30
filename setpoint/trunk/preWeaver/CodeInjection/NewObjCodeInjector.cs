using Mono.Cecil;
using Mono.Cecil.Cil;
using preWeaverCecil.CodeInjection.Il;

namespace preWeaverCecil.CodeInjection {
	/// <summary>
	/// Summary description for NewObjCodeInjector.
	/// </summary>
	internal class NewObjCodeInjector : MethodInterceptionCodeInjector {
		public override bool isInterceptorFor(Instruction instruction) {
			return instruction.OpCode == OpCodes.Newobj;
		}

		protected override MethodReference joinPointClassToInstantiate() {
			return this._methodToBeInstrumented.setPointAssemblyRef.constructorJoinPointConstructor;
		}

		protected override bool isStaticReceiver(CallInstruction instruction) {
			return true;
		}

		protected override TypeReference reifiedCallReturningType(){
			return this._originalInstruction.CallMethod.DeclaringType;			
		}

		protected override bool originalCallReturnsValue() {
			return true;
		}
	}
}