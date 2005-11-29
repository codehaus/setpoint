using PERWAPI;

namespace preWeaverPERWAPI.CodeInjection {
	/// <summary>
	/// Summary description for SetPointAssemblyRef.
	/// </summary>
	public class SetPointAssemblyRef {
		private AssemblyRef referencedAssembly;
		private ClassRef _weaverTypeRef;

		public SetPointAssemblyRef() {
			this.referencedAssembly = PEFile.ReadExportedInterface("SetPoint.dll") as AssemblyRef;
			_weaverTypeRef = this.referencedAssembly.GetClass("setPoint.weaving", "Weaver");			
		}

		#region public properties		

		private FieldRef _weaverReference;

		internal FieldRef weaverReference {
			get {
				if (_weaverReference == null)
					_weaverReference = this._weaverTypeRef.GetField("instance");
				return _weaverReference;
			}
		}

		private MethodRef _semanticatedAttributeConstructor;

		internal MethodRef semanticatedAttributeConstructor {
			get {
				if (_semanticatedAttributeConstructor == null)
					_semanticatedAttributeConstructor = this.referencedAssembly.GetClass("SemanticatedAttribute").GetMethod(".ctor");
				return _semanticatedAttributeConstructor;
			}
		}		
		
		private MethodRef _constructorJoinPointConstructor;

		internal MethodRef constructorJoinPointConstructor {
			get {
				if (_constructorJoinPointConstructor == null)
					_constructorJoinPointConstructor = this.referencedAssembly.GetClass("ConstructorJoinPoint").GetMethod(".ctor");
				return _constructorJoinPointConstructor;
			}
		}

		private MethodRef _methodCallJoinPointConstructor;

		internal MethodRef methodCallJoinPointConstructor {
			get {
				if (_methodCallJoinPointConstructor == null)
					_methodCallJoinPointConstructor = this.referencedAssembly.GetClass("MethodCallJoinPoint").GetMethod(".ctor");
				return _methodCallJoinPointConstructor;
			}
		}

		private MethodRef _instanceIObjectConstructor;

		internal MethodRef instanceIObjectConstructor {
			get {
				if (_instanceIObjectConstructor == null)
					_instanceIObjectConstructor = this.referencedAssembly.GetClass("setPoint.messageReifying", "Instance").GetMethod(".ctor");
				return _instanceIObjectConstructor;
			}
		}

		private MethodRef _classIObjectConstructor;

		internal MethodRef classIObjectConstructor {
			get {
				if (_classIObjectConstructor == null)
					_classIObjectConstructor = this.referencedAssembly.GetClass("setPoint.messageReifying","Class").GetMethod(".ctor");
				return _classIObjectConstructor;
			}
		}

		private MethodRef _methodIMessageConstructor;

		internal MethodRef methodIMessageConstructor {
			get {
				if (_methodIMessageConstructor == null)
					_methodIMessageConstructor = this.referencedAssembly.GetClass("setPoint.messageReifying","Method").GetMethod(".ctor");
				return _methodIMessageConstructor;
			}
		}

		private MethodRef _weavingMethod;

		internal MethodRef weavingMethod {
			get {
				if (_weavingMethod == null)
					_weavingMethod = this._weaverTypeRef.GetMethod("weave");
				return _weavingMethod;
			}
		}		

		#endregion		
	}
}