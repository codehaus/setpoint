using System;
using System.Collections;
using PERWAPI;

namespace preWeaverPERWAPI.CodeInjection {
	/// <summary>
	/// Chooses a suitable injector for a given instruction
	/// </summary>
	public class CodeInjectorChooser {
		private IList availableInjectors;

		/// <summary>
		/// Constructor - initializes list of available code injectors
		/// </summary>
		public CodeInjectorChooser() {
			this.availableInjectors = new ArrayList();

			this.availableInjectors.Add(new CallCodeInjector());
			this.availableInjectors.Add(new CallVirtCodeInjector());
			this.availableInjectors.Add(new NewObjCodeInjector());
			//this.availableInjectors.Add(new BranchCodeInjector());
			this.availableInjectors.Add(new NeutralCodeInjector());

		}

		/// <summary>
		/// Chooses a suitable injector for a given instruction
		/// </summary>
		public IMessageInterceptionCodeInjector codeInjectorFor(Instr instruction) {
			foreach (IMessageInterceptionCodeInjector codeInjector in this.availableInjectors)
				if (codeInjector.isInterceptorFor(instruction))
					return codeInjector;

			throw new Exception("Code injector chooser is not initialized!!!! CHECK SOURCE CODE...");
		}

	}
}