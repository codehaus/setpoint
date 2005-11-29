using System.Collections;
using setPoint.messageReifying;
using setPoint.semantics;

namespace setPoint.weaving {
	
	public interface IWeavingPolicy {		

		bool isSuitableFor(MatchPoint aMatchPoint);
		void proceedOn(IJoinPoint aJoinPoint, TriggerSet aTriggerSet); 
	}
}