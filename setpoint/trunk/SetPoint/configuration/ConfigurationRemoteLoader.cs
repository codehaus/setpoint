using System;
using System.IO;
using System.Reflection;

namespace setPoint.configuration {
	/// <summary>
	/// Summary description for RemoteLoader.
	/// </summary>
	public class ConfigurationRemoteLoader: MarshalByRefObject {		
		
		public IConfiguration LoadAssembly(string fullname) {		
			string path = Path.GetDirectoryName(fullname);
			string filename = Path.GetFileNameWithoutExtension(fullname);
						
			Assembly assembly = Assembly.Load(filename);			
			//Assembly assembly = AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(fullname));
			
			IConfiguration result = null;
			foreach(Type type in assembly.GetExportedTypes()) {
				if(type.GetInterface("setPoint.configuration.IConfiguration")!=null)
					result = (IConfiguration)type.GetConstructors()[0].Invoke(null);
			}
			
			return result;
		}
	}
}
