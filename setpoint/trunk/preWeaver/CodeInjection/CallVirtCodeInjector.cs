using PERWAPI;

namespace preWeaverPERWAPI.CodeInjection {
	/// <summary>
	/// Summary description for CallVirtCodeInjector.
	/// </summary>
	internal class CallVirtCodeInjector : AbstractCallCodeInjector {
		public override bool isInterceptorFor(Instr instruction) {
			return instruction.GetInstName() == "callvirt";
		}

		protected override MethodRef joinPointClassToInstantiate() {			
			return this._methodToBeInstrumented.setPointAssemblyRef.methodCallJoinPointConstructor;
		}
		
	}
}