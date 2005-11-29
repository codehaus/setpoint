using PERWAPI;

namespace preWeaverPERWAPI.CodeInjection {
	/// <summary>
	/// Summary description for CallCodeInjector.
	/// </summary>
	internal class CallCodeInjector : AbstractCallCodeInjector {
		public override bool isInterceptorFor(Instr instruction) {
			return instruction.GetInstName() == "call";			
		}

		protected override MethodRef joinPointClassToInstantiate() {
			return this._methodToBeInstrumented.setPointAssemblyRef.methodCallJoinPointConstructor;
		}
		
	}
}