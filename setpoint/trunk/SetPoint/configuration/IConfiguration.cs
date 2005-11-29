using setPoint.semantics;
using setPoint.weaving;
using System;

namespace setPoint.configuration
{
	/// <summary>
	/// Summary description for IConfiguration.
	/// </summary>
	public interface IConfiguration{
		// Returns the list of declared pointcuts
		PointCutMap pointcuts{get;}
		// Returns the list(union?) of declared ontologies
		Ontology[] ontologies{get;}
		// Returns the list of declared advices
		Advice[] advices{get;}
		// Returns the list of declared rules
		Rule[] rules{get;}
	}
}
