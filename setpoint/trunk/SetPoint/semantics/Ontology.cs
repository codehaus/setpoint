using System;

namespace setPoint.semantics
{
	/// <summary>
	/// Summary description for Ontology.
	/// </summary>
	public class Ontology
	{
		private string _name;
		private string _rdfXml;

		public string name
		{
			get{return this._name;}
			set{this._name = value;}
		}

		public string rdfXml
		{
			get{return this._rdfXml;}
			set{this._rdfXml = value;}
		}

		public Ontology(string name, string rdfXml)
		{
			this.name = name;
			this.rdfXml = rdfXml;
		}

	}
}


