using System;
using setPoint.messageReifying;

namespace setPoint.weaving {
	/// <summary>
	/// Builds
	/// </summary>
	public class DefaultAspectFactory:IAspectFactory {		
		private IAspect _singleInstance = null;
		private Type _aspectType;
		
		public Type aspectType {
			get {
				return _aspectType;
			}
		}
		public DefaultAspectFactory(Type aspectType) {
			this._aspectType = aspectType;	
		}

		protected virtual IAspect newInstance() {
			return this._aspectType.GetConstructors()[0].Invoke(null) as IAspect;
		}

		public IAspect instanceFor(IJoinPoint jp) {
			if(this._singleInstance == null)
				this._singleInstance = this.newInstance();
			return this._singleInstance;
		}
	}
}
