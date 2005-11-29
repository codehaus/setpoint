using System.Reflection;
using System.Collections;
using System;
using setPoint.programAnnotation;

namespace setPoint.util {
	
	internal delegate void HandleLoading(Assembly assembly);
	
	internal class CurrentAppDomain {

		ArrayList _loadedAssemblies = new ArrayList();
		internal ArrayList loadedAssemblies {
			get {
				return _loadedAssemblies;
			}
		}
		
		public CurrentAppDomain() {			
			foreach(Assembly loadedAssembly in AppDomain.CurrentDomain.GetAssemblies()){
				this.bootstrap(loadedAssembly);
			}
			AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler(OnAssemblyLoaded);
		}		

		public void OnAssemblyLoaded( object sender, AssemblyLoadEventArgs args ) {
			this.bootstrap(args.LoadedAssembly);
		}
		
		internal event HandleLoading hastJustLoaded;

		#region private methods
		private void bootstrap(Assembly loadedAssembly) {
			if(this.isSemanticated(loadedAssembly))
				this.add(loadedAssembly);			
		}

		private void add(Assembly loadedAssembly) {
			if(this.hastJustLoaded != null)
				this.hastJustLoaded(loadedAssembly);
			this._loadedAssemblies.Add(loadedAssembly);
		}

		private bool isSemanticated(Assembly loadedAssembly) {
			return loadedAssembly.GetCustomAttributes(typeof(SemanticatedAttribute),false).Length > 0;
		}
		#endregion
	}
}