using System;
using setPoint.semantics;

namespace setPoint.messageReifying
{
	/// <summary>
	/// Summary description for Class.
	/// </summary>
	public class Class:IObject
	{
		private readonly string _uri;
		private readonly RuntimeTypeHandle _typeHandle;
		private Type _type;

		public Class(RuntimeTypeHandle handle)
		{
			this._typeHandle = handle;
			this._type = Type.GetTypeFromHandle(this._typeHandle);
			this._uri = new OntologicalURIFactory().createURIFrom(this._type);
		}

		public object asMethodInvokeReference() {
			return null;
		}	
	
		public string uri {
			get {
				return _uri;
			}
		}

		public Type type
		{
			get 
			{
				return this._type;
			}
		}
	}
}
