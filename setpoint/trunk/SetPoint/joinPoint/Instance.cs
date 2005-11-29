using System;
using setPoint.semantics;

namespace setPoint.messageReifying
{
	/// <summary>
	/// Summary description for Instance.
	/// </summary>
	public class Instance: IObject
	{
		private readonly string _uri;
		private readonly object _reference;		

		public Instance(object reference)
		{
			this._reference = reference;
			this._uri = new OntologicalURIFactory().createURIFrom(reference.GetType());
		}

		public object asMethodInvokeReference() {
			return this._reference;
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
				return this._reference.GetType();
			}
		}
	}
}
