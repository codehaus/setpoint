using setPoint.messageReifying;
using setPoint.configuration;
using System.Collections;

namespace setPoint.semantics {
	/// <summary>
	/// 
	/// </summary>
	internal class JoinPointUniverse {
		
		private PointCutMap pointCutMap;
		private OntologicalUniverse ontologicalUniverse;

		internal JoinPointUniverse(OntologicalUniverse universe, ConfigurationLoader loader) {			
			loader.hasLoadedConfiguration += new ConfigurationLoadingHandler(this.handleConfigurationLoading);
			this.handleConfigurationLoading(loader.configuration);

			this.ontologicalUniverse = universe;			
		}
		
		private void handleConfigurationLoading(IConfiguration configuration) {
			this.pointCutMap = configuration.pointcuts;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="jp"></param>
		/// <returns></returns>
		internal MatchPoint pointCutsIncluding(IJoinPoint jp) {			
			MatchPoint result = new MatchPoint();
			foreach(PointCut pointCut in this.pointCutMap.Values)
				if(pointCut.includes(jp, ontologicalUniverse))				
					result.Add(pointCut);
			
			return result;
		}
	}
}