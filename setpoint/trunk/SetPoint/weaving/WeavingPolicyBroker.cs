using setPoint.semantics;

namespace setPoint.weaving {
	/// <summary>
	/// 
	/// </summary>
	internal class WeavingPolicyBroker {
		
		private IWeavingPolicy[] policies = { new ChainWeavingPolicy() };
		
		internal IWeavingPolicy bestPolicyFor(MatchPoint aMatchPoint) {
			foreach(IWeavingPolicy weavingPolicy in this.policies)
				if (weavingPolicy.isSuitableFor(aMatchPoint))
					return weavingPolicy;
			return null;
		}

	}
}