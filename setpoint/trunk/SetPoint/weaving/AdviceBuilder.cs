using setPoint.semantics;
using setPoint.messageReifying;
using System;
using System.Reflection;

namespace setPoint.weaving
{
	/// <summary>
	/// Summary description for AdviceBuilder.
	/// </summary>
	public class AdviceBuilder
	{
		protected Advice advice;
		protected Trigger currentTrigger=null;
		
		public void startBuildingAdviceUsing(IAspectFactory factory) {
			this.advice = new Advice(factory);
		}

		public void startbeforeTriggerDefinition(string eventName) {
			this.currentTrigger = new Trigger(this.advice,this.methodForEventName(eventName), true,false);
		}

		public void startafterTriggerDefinition(string eventName) {
			this.currentTrigger = new Trigger(this.advice,this.methodForEventName(eventName),false,true);
		}

		public void assignPointcutToCurrentTrigger(PointCut pointCut) {			
			this.currentTrigger.on.Add(pointCut);
		}

		public void endTriggerDefinition() {
			this.advice.triggers.Add(currentTrigger);
		}

		public Advice getBuiltAdvice() {
			return this.advice;
		}

		private MethodInfo methodForEventName(string eventName) {
			Type[] parameters = new Type[1];
			parameters[0] = typeof(IJoinPoint);
			return this.advice.factory.aspectType.GetMethod(eventName, parameters);
		}
	}
}
