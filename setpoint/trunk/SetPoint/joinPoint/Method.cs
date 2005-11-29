using setPoint.semantics;
using System;
using System.Reflection;

namespace setPoint.messageReifying {
	/// <summary>
	/// Summary description for MethodMessage.
	/// </summary>
	public class Method:IMessage {
		private readonly string _uri;
		internal readonly RuntimeMethodHandle methodHandle;
		internal readonly object[] arguments;
		
		public Method(RuntimeMethodHandle handle, object[] values) {
			this.methodHandle = handle;
			this.arguments = values;
			this._uri = new OntologicalURIFactory().createURIFrom(MethodBase.GetMethodFromHandle(
						this.methodHandle));
		}

		public string uri {
			get {
				return _uri;
			}
		}
	}
}
