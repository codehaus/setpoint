using System;
using Mono.Cecil;
using Mono.Cecil.Cil;
//using PERWAPI;

namespace preWeaverCecil.CodeInjection {
	/// <summary>
	/// Represents the message where code is to be injected
	/// </summary>
	public class MethodToBeInstrumented {
		private VariableDefinition _argumentsArray = null, _temporalLocalVariable = null;
		private readonly SetPointAssemblyRef _setPointAssemblyRef;

		public bool isStatic {
			get { return this._method.IsStatic; }
		}

		private readonly MethodDefinition _method;

		public MethodDefinition method {
			get { return _method; }
		}

		public VariableDefinition argumentsArray {
			get {
				// Lazy initialization
				if (_argumentsArray == null) {
					_argumentsArray = newLocalVariable("argsArray", typeof (System.Object[]));
				}
				return _argumentsArray;
			}
		}

		public VariableDefinition temporalLocalVariable {
			get {
				// Lazy initialization
				if (_temporalLocalVariable == null) {
					_temporalLocalVariable = newLocalVariable("tempVar", typeof (Object));
				}
				return _temporalLocalVariable;
			}
		}

		public SetPointAssemblyRef setPointAssemblyRef {
			get { return _setPointAssemblyRef; }
		}

		public TypeReference declaringType {
			get { return this._method.DeclaringType; }
		}

		public MethodBody code {
			get { return this._method.Body; }
		}

		public MethodToBeInstrumented(MethodDefinition method, SetPointAssemblyRef assemblyRef) {
			this._method = method;
			this._setPointAssemblyRef = assemblyRef;
		}

		private VariableDefinition newLocalVariable(string varName, Type varType) {
			TypeReference typeRef = declaringType.Module.Import(varType);
			VariableDefinition localVar = new VariableDefinition(varName, method.Body.Variables.Count, _method, typeRef);
			_method.Body.Variables.Add(localVar);
			_method.Body.InitLocals = true;
			return localVar;
		}
	}
}