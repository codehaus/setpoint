using System;
using setPoint.messageReifying;
using setPoint.util;
using setPoint.semantics;
using setPoint.configuration;
using System.IO;
using System.Reflection;

namespace setPoint.weaving
{
	/// <summary>
	/// It weaves like hell!
	/// </summary>
	public class Weaver
	{
		private readonly WeavingPolicyBroker policyBroker;
		private readonly CurrentAppDomain currentAppDomain;
		private readonly JoinPointUniverse joinPointUniverse;
		private readonly AdviceMap adviceMap;

		public static readonly Weaver instance = new Weaver();
				
		public Weaver() 
		{
			this.adviceMap = new AdviceMap();
			ConfigurationLoader loader = new ConfigurationLoader();
			loader.hasLoadedConfiguration += new ConfigurationLoadingHandler(this.handleConfigurationLoading);
			this.handleConfigurationLoading(loader.configuration);			

			this.policyBroker = new WeavingPolicyBroker();			
			this.currentAppDomain = new CurrentAppDomain();
			this.joinPointUniverse = new JoinPointUniverse(new OntologicalUniverse(this.currentAppDomain, loader), loader);			
		}

		private void handleConfigurationLoading(IConfiguration configuration) {		
			this.adviceMap.Clear();
			foreach(Advice advice in configuration.advices)			
				foreach(Trigger trigger in advice.triggers)
					foreach(PointCut pointCut in trigger.on)
						if(this.adviceMap.Contains(pointCut))
							this.adviceMap[pointCut].Add(trigger);
						else {													
							TriggerSet t = new TriggerSet();
							t.Add(trigger);
							this.adviceMap.Add(pointCut, t);
						}
		}

		public object weave(IJoinPoint aJoinPoint) {					
			
			MatchPoint matchPoint = this.joinPointUniverse.pointCutsIncluding(aJoinPoint);

			TriggerSet triggersToFire = new TriggerSet();

			foreach(PointCut pointCut in matchPoint)
				triggersToFire.AddRange(this.adviceMap[pointCut]);

			IWeavingPolicy weavingPolicy = this.policyBroker.bestPolicyFor(matchPoint);
			weavingPolicy.proceedOn(aJoinPoint, triggersToFire);
			
			return aJoinPoint.returnValue;
		}
	}
}
