using setPoint.messageReifying;
using setPoint.semantics;

namespace setPoint.weaving
{
	/// <summary>
	/// Summary description for ITrigger.
	/// </summary>
	public class Trigger
	{		
		public readonly PointCut[] on;
		public readonly Event fires;
		
		internal Trigger(PointCut[] pointCuts, Event fires) {
			this.on = pointCuts;
			this.fires = fires;
		}

		void applyOn(IJoinPoint jp) {
			this.fires.Invoke(jp);
		}

	}
}
