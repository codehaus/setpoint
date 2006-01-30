using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace preWeaverCecil.CodeInjection.Il {
	/// <summary>
	/// Edits a method's code.
	/// </summary>
	public class MethodCodeBuilder {
		private CilWorker methodCilWorker;
		private Instruction instructionToReplace;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="aMethodCilWorker">CilWorker of the method to instrument</param>
		public MethodCodeBuilder(CilWorker aMethodCilWorker) {
			methodCilWorker = aMethodCilWorker;
		}

		/// <summary>
		/// Returns a CilWorker for the method being instrumented
		/// </summary>
		public CilWorker Code {
			get { return methodCilWorker; }
		}
		
		/// <summary>
		/// Starts instruction insertion. Must be called before 
		/// inserting any instruction.
		/// </summary>
		/// <param name="anInstructionToReplace"> instruction to be replaced by others</param>
		public void beginInstructionInsertion(CallInstruction anInstructionToReplace) {
			instructionToReplace = anInstructionToReplace.Instruction;
		}

		/// <summary>
		/// Adds a new instruction after the previous instruction inserted.
		/// If it is the first it is inserted before the instruction
		/// to be replaced. 
		/// </summary>
		/// <param name="instruction">instruction to add</param>
		public void addInstruction(Instruction instruction) {
			methodCilWorker.InsertBefore(instructionToReplace, instruction);
		}

		/// <summary>
		/// Finalizes instruction insertion. Replaces shrot 
		/// branches with long ones (Workarround to a Cecil bug)
		/// </summary>
		public void endInstructionInsertion() {
			methodCilWorker.Remove(instructionToReplace);

			/* This is a hack !!!
			 * It replaces all short branches with the coresponding 
			 * long branch. It's done because cecil does not verify
			 * short branchas whe you insert instruction between branch
			 * and destination, this can cause the offset operand to
			 * everflow. 
			 * All branchas are replaced because there is no way of
			 * knowing the new offset until it's written to the file.
			 * Just remove this when the bug is corrected in Cecil  
			*/
			new BranchEnlarger(methodCilWorker.GetBody()).replaceShortBranches();
		}
	}
}