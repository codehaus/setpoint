using System;
using setPoint.messageReifying;

namespace setPoint.weaving {
	/// <summary>
	/// Builds
	/// </summary>
	public interface IAspectFactory {
		Type aspectType{get;}
		
		IAspect instanceFor(IJoinPoint jp);
	}
}
