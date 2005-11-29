using setPoint.messageReifying;

namespace setPoint.weaving {
	/// <summary>
	/// 
	/// </summary>
	public class Advice {		

		public readonly IAspect wraps;

		public readonly TriggerSet triggers;

		internal Advice(IAspect wrapee, TriggerSet triggerSet) {
			this.wraps = wrapee;
			this.triggers = triggerSet;
		}
	}
}