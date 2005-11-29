using System.Reflection.Emit;
using PERWAPI;

namespace preWeaverPERWAPI.CodeInjection {
	/// <summary>
	/// Summary description for NewObjCodeInjector.
	/// </summary>
	internal class NewObjCodeInjector : MethodInterceptionCodeInjector {
		public override bool isInterceptorFor(Instr instruction) {
			return instruction.GetInstName() ==  "newobj";
		}

		protected override MethodRef joinPointClassToInstantiate() {
			return this._methodToBeInstrumented.setPointAssemblyRef.constructorJoinPointConstructor;
		}

		protected override bool isStaticReceiver(MethInstr instruction) {
			return true;
		}

		protected override Type reifiedCallReturningType(){
			return this._originalInstruction.GetMethod().GetParent() as Type;			
		}

		protected override bool originalCallReturnsValue() {
			return true;
		}
	}
}