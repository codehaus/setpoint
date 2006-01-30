using System.Collections;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace preWeaverCecil.CodeInjection.Il {
	/// <summary>
	/// Chooser for primitive types load instriction opcodes. 
	/// </summary>
	public class PrimitiveTypeLoadOpCodeChooser {
		private IDictionary opCodeChooser;

		/// <summary>
		/// Constructor
		/// </summary>
		public PrimitiveTypeLoadOpCodeChooser() {
			fillOpCodeDictionary();
		}

		/// <summary>
		/// Checks if a type is primitive.
		/// </summary>
		/// <param name="type">type to be checked</param>
		/// <returns>true if it is a primitive type</returns>
		public bool isPrimitiveType(TypeReference type) {
			return opCodeChooser.Contains(type.FullName);
		}

		/// <summary>
		/// Get the load instruction opcode for a primitive type 
		/// </summary>
		/// <param name="type">primitive type</param>
		/// <returns>opcode for the load instruction</returns>
		public OpCode getLoadInstructionOpCode(TypeReference type) {
			return (OpCode) opCodeChooser[type.FullName];
		}

		#region private methods

		/// <summary>
		/// fills the dictionary with mapping beween primitive types and
		/// load instruction opcodes.
		/// </summary>
		private void fillOpCodeDictionary() {
			opCodeChooser = new Hashtable();
			// check if this are all the primitive types...
			opCodeChooser.Add(typeof (System.Boolean).FullName, OpCodes.Ldind_I1);
			opCodeChooser.Add(typeof (System.SByte).FullName, OpCodes.Ldind_I1);
			opCodeChooser.Add(typeof (System.Byte).FullName, OpCodes.Ldind_U1);
			opCodeChooser.Add(typeof (System.Int16).FullName, OpCodes.Ldind_I2);
			opCodeChooser.Add(typeof (System.UInt16).FullName, OpCodes.Ldind_U2);
			opCodeChooser.Add(typeof (System.Int32).FullName, OpCodes.Ldind_I4);
			opCodeChooser.Add(typeof (System.UInt32).FullName, OpCodes.Ldind_U4);
			opCodeChooser.Add(typeof (System.Int64).FullName, OpCodes.Ldind_I8);
			opCodeChooser.Add(typeof (System.UInt64).FullName, OpCodes.Ldind_I8);
			opCodeChooser.Add(typeof (System.Single).FullName, OpCodes.Ldind_R4);
			opCodeChooser.Add(typeof (System.Double).FullName, OpCodes.Ldind_R8);
			opCodeChooser.Add(typeof (System.IntPtr).FullName, OpCodes.Ldind_I);
			//opCodeChooser.Add(typeof(System.UIntPtr).FullName,  OpCodes);
		}

		#endregion
	}
}