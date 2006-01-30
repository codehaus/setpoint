using System;
using System.Collections;
using Mono.Cecil.Cil;

namespace preWeaverCecil.CodeInjection.Il {
	/// <summary>
	/// This class pourpose is ti cinvers short branches of a methos
	/// to the equivalent long branch. This is because cecil does no
	/// verification on qhat it emits, and can cause shrot offsets
	/// to overflow. This is used only as a workarround.
	/// </summary>
	public class BranchEnlarger {
		private MethodBody methodBody;
		private IDictionary branchMap;

		public BranchEnlarger(MethodBody aMethodBody) {
			methodBody = aMethodBody;
			branchMap = new Hashtable();
			fillDictionary();
		}

		/// <summary>
		/// replaces opcodes with the corresponding matching
		/// </summary>
		public void replaceShortBranches() {
			Instruction instruction = methodBody.Instructions[0];
			while (instruction != null) {
				if (branchMap.Contains(instruction.OpCode)) {
					instruction.OpCode = (OpCode) branchMap[instruction.OpCode];
				}
				instruction = instruction.Next;
			}
		}

		/// <summary>
		/// Maps between short and long branches.
		/// </summary>
		private void fillDictionary() {
			branchMap.Add(OpCodes.Beq_S, OpCodes.Beq);
			branchMap.Add(OpCodes.Bge_S, OpCodes.Bge);
			branchMap.Add(OpCodes.Bge_Un_S, OpCodes.Bge_Un);
			branchMap.Add(OpCodes.Bgt_S, OpCodes.Bgt);
			branchMap.Add(OpCodes.Bgt_Un_S, OpCodes.Bgt_Un);
			branchMap.Add(OpCodes.Ble_S, OpCodes.Ble);
			branchMap.Add(OpCodes.Ble_Un_S, OpCodes.Ble_Un);
			branchMap.Add(OpCodes.Blt_S, OpCodes.Blt);
			branchMap.Add(OpCodes.Blt_Un_S, OpCodes.Blt_Un);
			branchMap.Add(OpCodes.Bne_Un_S, OpCodes.Bne_Un);
			branchMap.Add(OpCodes.Br_S, OpCodes.Br);
			branchMap.Add(OpCodes.Brfalse_S, OpCodes.Brfalse);
			branchMap.Add(OpCodes.Brtrue_S, OpCodes.Brtrue);
			branchMap.Add(OpCodes.Leave_S, OpCodes.Leave);
		}
	}
}