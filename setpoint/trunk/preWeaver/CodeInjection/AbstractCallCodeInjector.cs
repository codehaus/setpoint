using PERWAPI;

namespace preWeaverPERWAPI.CodeInjection {
	/// <summary>
	/// Summary description for CallVirtCodeInjector.
	/// </summary>
	internal abstract class AbstractCallCodeInjector : MethodInterceptionCodeInjector {				

		protected override bool isStaticReceiver(MethInstr instruction) {
			return instruction.GetMethod().isStatic();
		} 
	
		protected override Type reifiedCallReturningType(){
			return this._originalInstruction.GetMethod().GetRetType();			
		}

		protected override bool originalCallReturnsValue() {
			bool result;
			if (this._originalInstruction.GetMethod().GetRetType() is PrimitiveType)
				result = !(this._originalInstruction.GetMethod().GetRetType() as PrimitiveType).SameType(PrimitiveType.Void);
			else
				result = true;
			return result;
		}
	}
}