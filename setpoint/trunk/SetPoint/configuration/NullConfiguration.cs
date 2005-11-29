using System;
using setPoint.semantics;
using setPoint.weaving;

namespace setPoint.configuration
{
	/// <summary>
	/// Summary description for DefaultConfiguration.
	/// </summary>
	public class NullConfiguration:IConfiguration {		
		#region IConfiguration Members

		public PointCutMap pointcuts {
			get {
				// TODO:  Add NullConfiguration.pointcuts getter implementation
				return new PointCutMap();
			}
		}

		public Ontology[] ontologies {
			get {
				// TODO:  Add NullConfiguration.ontologies getter implementation
				return new Ontology[0];
			}
		}

		public Advice[] advices {
			get {
				// TODO:  Add NullConfiguration.advices getter implementation
				return new Advice[0];
			}
		}

		public Rule[] rules {
			get { return new Rule[0]; }
		}

		#endregion
	}
}
