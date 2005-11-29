using System.Collections;
using setPoint.messageReifying;
using setPoint.semantics;

namespace setPoint.weaving
{	
	public class ChainWeavingPolicy:IWeavingPolicy
	{		
		private bool _executionCancelled;
		private ArrayList _executedAdvices;

		public bool isSuitableFor(MatchPoint aMatchPoint)
		{
			return true;
		}

		public void proceedOn(IJoinPoint aJoinPoint, TriggerSet aTriggerSet) 
		{
			this.beforeChain(aJoinPoint, aTriggerSet);
			
			if (!this._executionCancelled)
				aJoinPoint.execute();	
			
			this.afterChain(aJoinPoint, aTriggerSet);
		}

		private void beforeChain(IJoinPoint aJoinPoint, TriggerSet aTriggerSet)
		{
			this._executedAdvices = new ArrayList();
			this._executionCancelled = false;
			foreach(Trigger trigger in aTriggerSet)
				if (trigger.isBefore)
				{
					trigger.applyOn(aJoinPoint);
					this._executedAdvices.Add(trigger.parent);
					if (trigger.parent.instanceFor(aJoinPoint).mustCancelExecution)
					{
						this._executionCancelled = true;
						return;
					}
				}
		}


		private void afterChain(IJoinPoint aJoinPoint, TriggerSet aTriggerSet)
		{
			if (!this._executionCancelled)
			{			
				foreach(Trigger trigger in aTriggerSet)
					if ((trigger.isAfter)&&(!this._executedAdvices.Contains(trigger.parent)))
						trigger.applyOn(aJoinPoint);				
			}

			foreach(Trigger trigger in aTriggerSet)
				if ((trigger.isAfter)&&(this._executedAdvices.Contains(trigger.parent)))
					trigger.applyOn(aJoinPoint);				
		}
	}
}
