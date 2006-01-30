using System;
using Mono.Cecil;

namespace preWeaverCecil.CodeInjection {
	/// <summary>
	/// Summary description for SetPointAssemblyRef.
	/// </summary>
	public class SetPointAssemblyRef {
		private AssemblyDefinition referencedAssembly;
		private TypeDefinition _weaverTypeRef;

		public SetPointAssemblyRef() {
			openSetPointAssembly();
			_weaverTypeRef = referencedAssembly.MainModule.Types["setPoint.weaving.Weaver"];
		}

		private void openSetPointAssembly() {
			try {
				referencedAssembly = AssemblyFactory.GetAssembly("SetPoint.dll");
			} catch (Exception e) {
				throw new Exception("Could not open SetPoint assembly: " + e.Message);
			}
		}

		#region public properties		

		private FieldReference _weaverReference;

		public FieldReference weaverReference {
			get {
				if (_weaverReference == null)
					_weaverReference = _weaverTypeRef.Fields.GetField("instance");
				return _weaverReference;
			}
		}

		private MethodReference _semanticatedAttributeConstructor;

		public MethodReference semanticatedAttributeConstructor {
			get {
				if (_semanticatedAttributeConstructor == null)
					_semanticatedAttributeConstructor = getConstructorReference("setPoint.programAnnotation.SemanticatedAttribute");
				return _semanticatedAttributeConstructor;
			}
		}

		private MethodReference _constructorJoinPointConstructor;

		public MethodReference constructorJoinPointConstructor {
			get {
				if (_constructorJoinPointConstructor == null)
					_constructorJoinPointConstructor = getConstructorReference("setPoint.messageReifying.ConstructorJoinPoint");
				return _constructorJoinPointConstructor;
			}
		}

		private MethodReference _methodCallJoinPointConstructor;

		public MethodReference methodCallJoinPointConstructor {
			get {
				if (_methodCallJoinPointConstructor == null)
					_methodCallJoinPointConstructor = getConstructorReference("setPoint.messageReifying.MethodCallJoinPoint");
				return _methodCallJoinPointConstructor;
			}
		}

		private MethodReference _instanceIObjectConstructor;

		public MethodReference instanceIObjectConstructor {
			get {
				if (_instanceIObjectConstructor == null)
					_instanceIObjectConstructor = getConstructorReference("setPoint.messageReifying.Instance");
				return _instanceIObjectConstructor;
			}
		}

		private MethodReference _classIObjectConstructor;

		public MethodReference classIObjectConstructor {
			get {
				if (_classIObjectConstructor == null)
					_classIObjectConstructor = getConstructorReference("setPoint.messageReifying.Class");
				return _classIObjectConstructor;
			}
		}

		private MethodReference _methodIMessageConstructor;

		public MethodReference methodIMessageConstructor {
			get {
				if (_methodIMessageConstructor == null)
					_methodIMessageConstructor = getConstructorReference("setPoint.messageReifying.Method");
				return _methodIMessageConstructor;
			}
		}

		private MethodReference _weavingMethod;

		public MethodReference weavingMethod {
			get {
				if (_weavingMethod == null)
					_weavingMethod = _weaverTypeRef.Methods.GetMethod("weave")[0];
				return _weavingMethod;
			}
		}

		#endregion

		private MethodDefinition getConstructorReference(string typeFullName) {
			return referencedAssembly.MainModule.Types[typeFullName].Constructors[0];
		}

	}
}