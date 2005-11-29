using System;
using System.Reflection;

namespace setPoint.semantics
{
	/// <summary>
	/// Summary description for OntologicalURIFactory.
	/// </summary>
	internal class OntologicalURIFactory
	{
		public static readonly string CTS_PREFIX = "semantics://CTS/";

		internal string createURIFrom(Type type) {
			string uri= CTS_PREFIX + type.Assembly.GetName().Name + "/"
				+ type.Module.ScopeName + "#" + type.FullName;			
		    return uri;
		}

		internal string createURIFrom(MethodBase method) {
			string uri= this.createURIFrom(method.DeclaringType) + ".." +
				method.Name;
			return uri;
		}
	}
}
