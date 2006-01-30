using System;
using Mono.Cecil;
using Mono.Cecil.Cil;
using preWeaverCecil.CodeInjection.Il;

namespace preWeaverCecil.CodeInjection {
	/// <summary>
	/// Code injector that intercepts message sends
	/// </summary>
	internal abstract class MethodInterceptionCodeInjector : IMessageInterceptionCodeInjector {
		protected CallInstruction _originalInstruction;
		protected MethodToBeInstrumented _methodToBeInstrumented;
		protected MethodCodeBuilder codeBuilder;
		protected PrimitiveTypeLoadOpCodeChooser opcodeChooser = new PrimitiveTypeLoadOpCodeChooser();

		#region public methods

		public void interceptMessageSentBy(Instruction originalInstruction, MethodToBeInstrumented methodToBeInstrumented) {
			/*
			// This code doesn't make sense
			  
			bool baseConstructorAlreadyCalled = false;

			this._methodToBeInstrumented = methodToBeInstrumented;
			this._originalInstruction = new CallInstruction(originalInstruction);

			if (!this.isStaticReceiver(this._originalInstruction) && this._originalInstruction.OpCode == OpCodes.Call)
				return;
			if (!this.isConstructorSender())
				this.redirectCallToWeaver();
			else if (baseConstructorAlreadyCalled)
				this.redirectCallToWeaver();
			else
				baseConstructorAlreadyCalled = this.isCallToBaseConstructor();
			*/

			this._methodToBeInstrumented = methodToBeInstrumented;
			this._originalInstruction = new CallInstruction(originalInstruction);

			if(!this.isCallToBaseConstructor()) {
				this.redirectCallToWeaver();
			}			
		}

		public abstract bool isInterceptorFor(Instruction instruction);

		#endregion

		#region private methods

		#region general call redirection methods

		private void redirectCallToWeaver() {
			this.initializeCallRedirection();

			this.insertCallRedirection();

			this.endCallRedirection();
		}

		private void endCallRedirection() {
			this.codeBuilder.endInstructionInsertion();
		}

		private void insertCallRedirection() {
			this.prepareOriginalMessageArguments();

			this.addWeaverReference();

			this.instantiateJoinPoint();

			this.addCallToWeavingMethod();

			if (this.originalCallReturnsValue())
				this.castReturnedInstance();
			else
				codeBuilder.addInstruction(codeBuilder.Code.Create(OpCodes.Pop));
		}

		private void instantiateJoinPoint() {
			this.addJoinPointSender();
			this.addJoinPointReceiver();
			this.addJoinPointMessage();
			this.addCallToJoinPointConstructor();
		}

		private void initializeCallRedirection() {
			this.codeBuilder = new MethodCodeBuilder(_methodToBeInstrumented.code.CilWorker);
			this.codeBuilder.beginInstructionInsertion(_originalInstruction);
		}

		#endregion

		protected abstract TypeReference reifiedCallReturningType();
		protected abstract bool isStaticReceiver(CallInstruction instruction);

		private void castReturnedInstance() {
			TypeReference returnedType = reifiedCallReturningType();
			if (returnedType.IsValueType) {
				codeBuilder.addInstruction(codeBuilder.Code.Create(OpCodes.Unbox, returnedType));
				this.loadInstanceForType(returnedType);
			} else {
				codeBuilder.addInstruction(codeBuilder.Code.Create(OpCodes.Castclass, returnedType));
			}
		}

		private void loadInstanceForType(TypeReference returnedType) {
			if (opcodeChooser.isPrimitiveType(returnedType))
				codeBuilder.addInstruction(codeBuilder.Code.Create(opcodeChooser.getLoadInstructionOpCode(returnedType)));
			else
				codeBuilder.addInstruction(codeBuilder.Code.Create(OpCodes.Ldobj, returnedType));
		}

		private void addJoinPointMessage() {
			codeBuilder.addInstruction(codeBuilder.Code.Create(OpCodes.Ldtoken, _originalInstruction.CallMethod));
			codeBuilder.addInstruction(loadLocalVarInstruction(_methodToBeInstrumented.argumentsArray));
			MethodReference methodIMessageConstructor = module().Import(_methodToBeInstrumented.setPointAssemblyRef.methodIMessageConstructor);
			codeBuilder.addInstruction(codeBuilder.Code.Create(OpCodes.Newobj, methodIMessageConstructor));
		}

		private void addJoinPointSender() {
			MethodReference constructor;
			if (_methodToBeInstrumented.isStatic) {
				codeBuilder.addInstruction(codeBuilder.Code.Create(OpCodes.Ldtoken, _methodToBeInstrumented.declaringType));
				constructor = module().Import(_methodToBeInstrumented.setPointAssemblyRef.classIObjectConstructor);
			} else {
				codeBuilder.addInstruction(codeBuilder.Code.Create(OpCodes.Ldarg_0));
				constructor = module().Import(_methodToBeInstrumented.setPointAssemblyRef.instanceIObjectConstructor);
			}
			codeBuilder.addInstruction(codeBuilder.Code.Create(OpCodes.Newobj, constructor));
		}

		private void addJoinPointReceiver() {
			MethodReference constructor;
			if (isStaticReceiver(_originalInstruction)) {
				codeBuilder.addInstruction(codeBuilder.Code.Create(OpCodes.Ldtoken, _originalInstruction.CallMethod.DeclaringType));
				constructor = module().Import(_methodToBeInstrumented.setPointAssemblyRef.classIObjectConstructor);
				codeBuilder.addInstruction(codeBuilder.Code.Create(OpCodes.Newobj, constructor));
			} else {
				codeBuilder.addInstruction(loadLocalVarInstruction(_methodToBeInstrumented.temporalLocalVariable));
				constructor = module().Import(_methodToBeInstrumented.setPointAssemblyRef.instanceIObjectConstructor);
				codeBuilder.addInstruction(codeBuilder.Code.Create(OpCodes.Newobj, constructor));
			}
		}

		private void addCallToJoinPointConstructor() {
			MethodReference joinPointClass = module().Import(joinPointClassToInstantiate());
			codeBuilder.addInstruction(codeBuilder.Code.Create(OpCodes.Newobj, joinPointClass));
		}

		private void addCallToWeavingMethod() {
			MethodReference weavingMethod = module().Import(_methodToBeInstrumented.setPointAssemblyRef.weavingMethod);
			codeBuilder.addInstruction(codeBuilder.Code.Create(OpCodes.Call, weavingMethod));
		}

		private void addWeaverReference() {
			FieldReference weaverReference = module().Import(_methodToBeInstrumented.setPointAssemblyRef.weaverReference);
			codeBuilder.addInstruction(codeBuilder.Code.Create(OpCodes.Ldsfld, weaverReference));
		}

		private bool isCallToBaseConstructor() {
			// It should work almost always
			return isConstructorSender() && 
				this._originalInstruction.OpCode == OpCodes.Call && 
				_originalInstruction.isConstructorCall() &&
				(calledMethodDeclaringType() == callingMethodBaseType());
		}
	
		private TypeReference calledMethodDeclaringType() {
			return _originalInstruction.CallMethod.DeclaringType;
		}

		private TypeReference callingMethodBaseType() {
			return module().Types[_methodToBeInstrumented.declaringType.FullName].BaseType;
		}

		private bool isConstructorSender() {
			return this._methodToBeInstrumented.method.IsConstructor;
		}

		private void prepareOriginalMessageArguments() {
			// Array length (push it, so that we can create it)
			codeBuilder.addInstruction(codeBuilder.Code.Create(OpCodes.Ldc_I4, _originalInstruction.CallMethod.Parameters.Count));

			// Let's construct the array...(defined of type System.Object)
			codeBuilder.addInstruction(codeBuilder.Code.Create(OpCodes.Newarr, module().Import(typeof (Object))));

			// Reference the constructed array
			codeBuilder.addInstruction(storeLocalVarInstruction(_methodToBeInstrumented.argumentsArray));

			// Now it's time to store all those already-pushed arguments in the array
			// We need an index, so that the array can be accessed
			for (int i = this._originalInstruction.CallMethod.Parameters.Count - 1; i >= 0; i--) {
				TypeReference argumentType = this._originalInstruction.CallMethod.Parameters[i].ParameterType;

				// This argument is now on top of the stack...we should store it somewhere in the meantime...
				if (argumentType.IsValueType)
					codeBuilder.addInstruction(codeBuilder.Code.Create(OpCodes.Box, argumentType));

				codeBuilder.addInstruction(storeLocalVarInstruction(_methodToBeInstrumented.temporalLocalVariable));

				// We need to push a reference to the array, so that the CLR knows who is it that we're talking about
				codeBuilder.addInstruction(loadLocalVarInstruction(_methodToBeInstrumented.argumentsArray));

				// In what part of the array are we storing the argument? - The index must be pushed
				codeBuilder.addInstruction(codeBuilder.Code.Create(OpCodes.Ldc_I4, i));

				// What are we storing? - Push once again the i-th argument (arguments are originally pushed in order before a method call, 
				//  so we must carefully store them in reverse order this time)
				codeBuilder.addInstruction(loadLocalVarInstruction(_methodToBeInstrumented.temporalLocalVariable));

				// We finally store the i-th argument in the array
				codeBuilder.addInstruction(codeBuilder.Code.Create(OpCodes.Stelem_Ref));
			}

			// If the original instruction was an instance call, the "receiver" is now on top of the stack
			if (!this.isStaticReceiver(this._originalInstruction)) {
				TypeReference receiverType = _originalInstruction.CallMethod.DeclaringType;
				// If the reciver is a value type, then there's a pointer to it on the stack
				if (receiverType.IsValueType) { //if (receiverType.mustBeBoxed()) {
					// We first turn the pointer into the actual value
					this.loadInstanceForType(receiverType);
					// Then it needs to be boxed, so that we can store in our local var
					codeBuilder.addInstruction(codeBuilder.Code.Create(OpCodes.Box, receiverType));
				}
				// We store it in a local variable, so that it can later be retrieved
				codeBuilder.addInstruction(storeLocalVarInstruction(_methodToBeInstrumented.temporalLocalVariable));
			}
		}

		private ModuleDefinition module() {
			return _methodToBeInstrumented.declaringType.Module;
		}

		private Instruction storeLocalVarInstruction(VariableDefinition variableDefinition) {
			// Not very nice...
			switch (variableDefinition.Index) {
				case 0:
					return codeBuilder.Code.Create(OpCodes.Stloc_0);
				case 1:
					return codeBuilder.Code.Create(OpCodes.Stloc_1);
				case 2:
					return codeBuilder.Code.Create(OpCodes.Stloc_2);
				case 3:
					return codeBuilder.Code.Create(OpCodes.Stloc_3);
				default:
					if (variableDefinition.Index < Byte.MaxValue)
						return codeBuilder.Code.Create(OpCodes.Stloc_S, variableDefinition);
					else
						return codeBuilder.Code.Create(OpCodes.Stloc, variableDefinition);
			}
		}

		private Instruction loadLocalVarInstruction(VariableDefinition variableDefinition) {
			switch (variableDefinition.Index) {
				case 0:
					return codeBuilder.Code.Create(OpCodes.Ldloc_0);
				case 1:
					return codeBuilder.Code.Create(OpCodes.Ldloc_1);
				case 2:
					return codeBuilder.Code.Create(OpCodes.Ldloc_2);
				case 3:
					return codeBuilder.Code.Create(OpCodes.Ldloc_3);
				default:
					if (variableDefinition.Index < Byte.MaxValue)
						return codeBuilder.Code.Create(OpCodes.Ldloc_S, variableDefinition);
					else
						return codeBuilder.Code.Create(OpCodes.Ldloc, variableDefinition);
			}
		}

		protected abstract MethodReference joinPointClassToInstantiate();

		protected abstract bool originalCallReturnsValue();

		#endregion
	}

}