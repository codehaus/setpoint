using System.Reflection.Emit;
using PERWAPI;

namespace preWeaverPERWAPI.CodeInjection {
	/// <summary>
	/// Code injector that intercepts message sends
	/// </summary>
	internal abstract class MethodInterceptionCodeInjector : IMessageInterceptionCodeInjector {
		protected MethInstr _originalInstruction;
		protected MethodToBeInstrumented _methodToBeInstrumented;
		protected CILInstructions _code;

		#region public methods

		public void interceptMessageSentBy(Instr originalInstruction, MethodToBeInstrumented methodToBeInstrumented) {
			bool baseConstructorAlreadyCalled = false;

			this._methodToBeInstrumented = methodToBeInstrumented;
			this._originalInstruction = originalInstruction as MethInstr;			
			
			if(!this.isStaticReceiver(this._originalInstruction) && this._originalInstruction.GetInstName()=="call")
				return;
			if(!this.isConstructorSender())
				this.redirectCallToWeaver();
			else{
				if (baseConstructorAlreadyCalled)
					this.redirectCallToWeaver();
				else
					baseConstructorAlreadyCalled = this.isCallToBaseConstructor();
			}
				
		}

		public abstract bool isInterceptorFor(Instr instruction);

		#endregion

		#region private methods

		#region general call redirection methods
		private void redirectCallToWeaver() {
			this.initializeCallRedirection();

			this.insertCallRedirection();

			this.endCallRedirection();
		}

		private void endCallRedirection() {
			this._code.EndInsert();
	
			this._methodToBeInstrumented.growMaxStack();
		}

		private void insertCallRedirection() {
			this.prepareOriginalMessageArguments();				
	
			this.addWeaverReference();

			this.instantiateJoinPoint();

			this.addCallToWeavingMethod();

			if(this.originalCallReturnsValue())				
				this.castReturnedInstance();
			else
				this._code.Inst(Op.pop);
		}

		private void instantiateJoinPoint() {
			this.addJoinPointSender();
			this.addJoinPointReceiver();	
			this.addJoinPointMessage();	
			this.addCallToJoinPointConstructor();
		}

		private void initializeCallRedirection() {
			this._code = this._methodToBeInstrumented.code;
			this._code.ReplaceInstruction(this._originalInstruction.GetPos());
		}
		#endregion

		protected abstract Type reifiedCallReturningType();
		protected abstract bool isStaticReceiver(MethInstr instruction);

		private void castReturnedInstance() {
			Type returnedType = this.reifiedCallReturningType();			
			if(returnedType.mustBeBoxed()){
				this._code.TypeInst(TypeOp.unbox, returnedType);
				this.loadInstanceForType(returnedType);
			}
			else
				this._code.TypeInst(TypeOp.castclass, returnedType);
		}

		private void loadInstanceForType(Type returnedType) {
			if(returnedType is PrimitiveType) {
				PrimitiveType primitiveReturned = returnedType as PrimitiveType;
				if(primitiveReturned == PrimitiveType.Int32)
					this._code.Inst(Op.ldind_i4);
				else if(primitiveReturned == PrimitiveType.Boolean)
					this._code.Inst(Op.ldind_i1);					
			}
			else
				this._code.TypeInst(TypeOp.ldobj, returnedType);
		}

		private void addJoinPointMessage() {
			this._code.MethInst(MethodOp.ldtoken, this._originalInstruction.GetMethod());
			this._code.LoadLocal(this._methodToBeInstrumented.argumentsArray);
			this._code.MethInst(MethodOp.newobj, this._methodToBeInstrumented.setPointAssemblyRef.methodIMessageConstructor);
		}

		private void addJoinPointSender() {
			if (this._methodToBeInstrumented.isStatic) {
				this._code.TypeInst(TypeOp.ldtoken, this._methodToBeInstrumented.declaringType);
				this._code.MethInst(MethodOp.newobj, this._methodToBeInstrumented.setPointAssemblyRef.classIObjectConstructor);
			}
			else{
				this._code.LoadArg(0);
				this._code.MethInst(MethodOp.newobj, this._methodToBeInstrumented.setPointAssemblyRef.instanceIObjectConstructor);
			}
		}		

		private void addJoinPointReceiver() {
			if (this.isStaticReceiver(this._originalInstruction)) {
				this._code.TypeInst(TypeOp.ldtoken, this._originalInstruction.GetMethod().GetParent() as Type);				
				this._code.MethInst(MethodOp.newobj, this._methodToBeInstrumented.setPointAssemblyRef.classIObjectConstructor);
			}
			else{
				this._code.LoadLocal(this._methodToBeInstrumented.temporalLocalVariable);
				this._code.MethInst(MethodOp.newobj, this._methodToBeInstrumented.setPointAssemblyRef.instanceIObjectConstructor);
			}
		}

		private void addCallToJoinPointConstructor() {
			this._code.MethInst(MethodOp.newobj, this.joinPointClassToInstantiate());
		}

		private void addCallToWeavingMethod() {
			this._code.MethInst(MethodOp.call, this._methodToBeInstrumented.setPointAssemblyRef.weavingMethod);
		}

		private void addWeaverReference() {
			this._code.FieldInst(FieldOp.ldsfld, this._methodToBeInstrumented.setPointAssemblyRef.weaverReference);			
		}

		private bool isCallToBaseConstructor() {
			return (this._originalInstruction.GetInstName() == "call") &&
				((this._originalInstruction.GetMethod().Name() == ".ctor") ||
					(this._originalInstruction.GetMethod().Name() == ".cctor"));
		}

		private bool isConstructorSender() {
			return ((this._methodToBeInstrumented.method.Name() == ".ctor") ||
				(this._methodToBeInstrumented.method.Name() == ".cctor"));
		}

		private void prepareOriginalMessageArguments() {
			// Array length (push it, so that we can create it) 				
			this._code.IntInst(IntOp.ldc_i4, this._originalInstruction.GetMethod().GetParTypes().Length);

			// Let's construct the array...(defined of type System.Object)
			this._code.TypeInst(TypeOp.newarr, MSCorLib.mscorlib.Object());

			// Reference the constructed array
			this._code.StoreLocal(this._methodToBeInstrumented.argumentsArray);

			// Now it's time to store all those already-pushed arguments in the array
			// We need an index, so that the array can be accessed
			for (int i = this._originalInstruction.GetMethod().GetParTypes().Length - 1; i >= 0; i--) {
				Type argumentType = this._originalInstruction.GetMethod().GetParTypes()[i];

				// This argument is now on top of the stack...we should store it somewhere in the meantime...
				if (argumentType.mustBeBoxed())
					this._code.TypeInst(TypeOp.box, argumentType);
				this._code.StoreLocal(this._methodToBeInstrumented.temporalLocalVariable);

				// We need to push a reference to the array, so that the CLR knows who is it that we're talking about
				this._code.LoadLocal(this._methodToBeInstrumented.argumentsArray);

				// In what part of the array are we storing the argument? - The index must be pushed
				this._code.IntInst(IntOp.ldc_i4, i);

				// What are we storing? - Push once again the i-th argument (arguments are originally pushed in order before a method call, 
				//  so we must carefully store them in reverse order this time)
				this._code.LoadLocal(this._methodToBeInstrumented.temporalLocalVariable);

				// We finally store the i-th argument in the array
				this._code.Inst(Op.stelem_ref);
			}

			// If the original instruction was an instance call, the "receiver" is now on top of the stack
			if(!this.isStaticReceiver(this._originalInstruction)){
				Type receiverType = this._originalInstruction.GetMethod().GetParent() as Type;
				// If the reciver is a value type, then there's a pointer to it on the stack
				if(receiverType.mustBeBoxed()) {
					// We first turn the pointer into the actual value
					this.loadInstanceForType(receiverType);
					// Then it needs to be boxed, so that we can store in our local var
					this._code.TypeInst(TypeOp.box, receiverType);
				}
				// We store it in a local variable, so that it can later be retrieved
				this._code.StoreLocal(this._methodToBeInstrumented.temporalLocalVariable);
			}
		}

		protected abstract MethodRef joinPointClassToInstantiate();

		protected abstract bool originalCallReturnsValue();		
	

		#endregion		
	}

}