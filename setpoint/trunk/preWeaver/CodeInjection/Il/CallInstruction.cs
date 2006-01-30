using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace preWeaverCecil.CodeInjection.Il {
	/// <summary>
	/// Represents a call instruction
	/// </summary>
	public class CallInstruction {
		private Instruction instruction;
		private MethodReference callMethod; 

		/// <summary>
		/// Constructor of call instruction. Throws an exception
		/// if the isntruction is not a call.
		/// </summary>
		/// <param name="anInstruction">call instruction</param>
		public CallInstruction(Instruction anInstruction) {
			if (isCallInstruction(anInstruction)) {
				instruction = anInstruction;
				callMethod = (instruction.Operand as MethodReference);
			} else {
				throw new NotCallInstructionException();
			}
		}

		/// <summary>
		/// Checks if an instruction is a call instruction.
		/// </summary>
		/// <param name="anInstruction">instruction to check</param>
		/// <returns>true if the instruction is a call</returns>
		public static bool isCallInstruction(Instruction anInstruction) {
			return anInstruction.OpCode == OpCodes.Call ||
				anInstruction.OpCode == OpCodes.Calli ||
				anInstruction.OpCode == OpCodes.Callvirt ||
				anInstruction.OpCode == OpCodes.Newobj;
			// || anInstruction.OpCode == OpCodes.Newarr; // handle this ??
		}

		/// <summary>
		/// Method wich is called in the instruction
		/// </summary>
		public MethodReference CallMethod {
			get { return callMethod; }
		}

		/// <summary>
		/// Checks if it is a call to a constructor
		/// </summary>
		/// <returns>true if it is constructor call</returns>
		public bool isConstructorCall() {
			//return instruction.OpCode == OpCodes.Newobj ||
			//		  instruction.OpCode == OpCodes.Newarr;
			return callMethod.Name == ".ctor" || callMethod.Name == ".cctor";
		}

		/// <summary>
		/// Checks if it is a call to a static method
		/// </summary>
		/// <returns>true if it is a call to a static method</returns>
		public bool isStaticCall() {
			return !callMethod.HasThis;  // almost sure this works
		}

		/// <summary>
		/// OpCode of the instruction
		/// </summary>
		public OpCode OpCode {
			get { return instruction.OpCode; }
		}

		/// <summary>
		/// Returns the generic instruction 
		/// </summary>
		public Instruction Instruction {
			get { return instruction; }
		}
	}

	internal class NotCallInstructionException : Exception {
		public NotCallInstructionException() : base("Instruction is not a Call") {
		}
	}
}