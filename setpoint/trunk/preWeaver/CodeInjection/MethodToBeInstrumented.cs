using PERWAPI;

namespace preWeaverPERWAPI.CodeInjection {
	/// <summary>
	/// Represents the message where code is to be injected
	/// </summary>
	public class MethodToBeInstrumented {
		private Local _argumentsArray = null, _temporalLocalVariable = null;
		private readonly SetPointAssemblyRef _setPointAssemblyRef;
		private bool _maxStackHasAlreadyGrown;

		public bool isStatic {
			get { return this._method.isStatic(); }
		}

		private readonly MethodDef _method;

		public MethodDef method {
			get { return _method; }
		}

		public int argumentsArray {
			get {
				// Lazy initialization
				if (_argumentsArray == null)
					this._method.AddLocal(_argumentsArray = new Local("argsArray", MSCorLib.mscorlib.ObjectArray()));
				return _argumentsArray.GetIndex();
			}
		}

		public int temporalLocalVariable {
			get {
				// Lazy initialization
				if (_temporalLocalVariable == null){
					this._method.AddLocal(_temporalLocalVariable = new Local("tempVar", MSCorLib.mscorlib.Object()));
				}
				return _temporalLocalVariable.GetIndex();
			}
		}

		public SetPointAssemblyRef setPointAssemblyRef {
			get {				
				return _setPointAssemblyRef;
			}
		}

		public Type declaringType {
			get { return (this._method.GetParent() as Type); }
		}

		public CILInstructions code {
			get { return this._method.GetCodeBuffer(); }
		}

		public void growMaxStack() {
			if(!this._maxStackHasAlreadyGrown) {
				// Patch until PERWAPI calculates max stack automatically for fat methods
				int currentStack = this._method.GetMaxStack()==0 ? 8 : this._method.GetMaxStack();
				this._method.SetMaxStack(currentStack+5);
				this._maxStackHasAlreadyGrown = true;
			}
		}

		public MethodToBeInstrumented(MethodDef method, SetPointAssemblyRef assemblyRef) {
			this._method = method;
			this._setPointAssemblyRef = assemblyRef;
		}
	}
}