using Mono.Cecil;
using preWeaverCecil.CodeInjection.Il;

namespace preWeaverCecil.CodeInjection {
	/// <summary>
	/// Summary description for CallVirtCodeInjector.
	/// </summary>
	internal abstract class AbstractCallCodeInjector : MethodInterceptionCodeInjector {				

		protected override bool isStaticReceiver(CallInstruction instruction) {
			return instruction.isStaticCall();
		} 
	
		protected override TypeReference reifiedCallReturningType(){
			return this._originalInstruction.CallMethod.ReturnType.ReturnType;
		}

		protected override bool originalCallReturnsValue() {
			return this._originalInstruction.CallMethod.ReturnType.ReturnType.FullName != "System.Void";
		}
	}
}