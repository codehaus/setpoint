using System;
using setPoint.messageReifying;

namespace setPoint.weaving {
	/// <summary>
	/// 
	/// </summary>
	public class Advice {		

		protected IAspectFactory _factory;
		internal IAspectFactory factory
		{
			get{
				return this._factory;
			}
			set{
				this._factory = value;	
			}
		}

		public readonly TriggerSet triggers;

		internal Advice(IAspectFactory wrapeeFactory) {
			this._factory = wrapeeFactory;
			this.triggers = new TriggerSet();
		}

		internal IAspect instanceFor(IJoinPoint jp) {
			return this.factory.instanceFor(jp);
		}
	}
}