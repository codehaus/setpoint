using System;

namespace setPoint.semantics {
	/// <summary>
	/// Summary description for Ontology.
	/// </summary>
	public class Rule {
		private string _premises;
		private string _consequence;

		public string SeRQL {
			get{return "CONSTRUCT " + this._consequence + 
					" FROM " + this._premises;}
		}

		public string premises {
			get{return this._premises;}
			set{this._premises = value;}
		}

		public string consequences {
			get{return this._consequence;}
			set{this._consequence = value;}
		}

		public Rule(string premises, string consequence) {
			this._consequence= consequence;
			this._premises= premises;
		}

	}
}